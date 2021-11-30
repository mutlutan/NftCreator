using System;
using System.Collections.Generic;
using System.Linq;
using WebApp1.Codes;
using WebApp1.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApp1.Areas.Nft.Codes
{
    public class NftBusiness
    {
        private readonly DataContext dataContext;
        private readonly _Rep rep = null;

        public NftBusiness(DataContext _dataContext)
        {
            this.dataContext = _dataContext;
            this.rep = new Models._Rep(this.dataContext);
            
        }


        #region generate Bitmap
        public System.IO.MemoryStream CreateBitmapStream(string userCode, MoProjectInfo projectInfo, int _width, int _height, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            System.Drawing.Bitmap bmp = new(_width, _height);
            System.Drawing.Graphics grap = System.Drawing.Graphics.FromImage(bmp);

            foreach (var layer in projectInfo.LayerList.Where(c => c.Status == true))
            {
                System.IO.DirectoryInfo directoryInfo = new(MyApp.UserImportDirectory(userCode, projectInfo.ProjectName) + "\\" + layer.Name);

                var randomFile = directoryInfo
                    .EnumerateFiles("*.png", System.IO.SearchOption.TopDirectoryOnly)
                    .OrderBy(o => Guid.NewGuid()).FirstOrDefault();

                if (randomFile != null)
                {
                    var img = System.Drawing.Image.FromFile(randomFile.FullName);
                    grap.DrawImage(img, 0, 0, _width, _height);
                    img.Dispose();
                }
            }

            grap.Dispose();

            System.IO.MemoryStream memoryStream = new();
            bmp.Save(memoryStream, imageFormat);

            bmp.Dispose();

            return memoryStream;
        }

        public System.IO.MemoryStream CreateBitmapStream(string userCode, MoMetaData metaData, int _width, int _height, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            System.Drawing.Bitmap bmp = new(_width, _height);
            System.Drawing.Graphics grap = System.Drawing.Graphics.FromImage(bmp);

            foreach (var attribute in metaData.Attributes)
            {
                string imageFileFullName = MyApp.UserImportDirectory(userCode, metaData.ProjectName) + "\\" + attribute.TraitType + "\\" + attribute.ImageName;

                var img = System.Drawing.Image.FromFile(imageFileFullName);
                grap.DrawImage(img, 0, 0, _width, _height);
                img.Dispose();
            }

            grap.Dispose();

            System.IO.MemoryStream memoryStream = new();
            bmp.Save(memoryStream, imageFormat);

            bmp.Dispose();

            return memoryStream;
        }

        public void GenerateImages(string userCode, List<MoMetaData> metaDataList, string exportDirectory)
        {
            string imageDirectory = exportDirectory + "\\" + "images";
            if (!System.IO.Directory.Exists(imageDirectory))
            {
                System.IO.Directory.CreateDirectory(imageDirectory);
            }

            string jsonDirectory = exportDirectory + "\\" + "json";
            if (!System.IO.Directory.Exists(jsonDirectory))
            {
                System.IO.Directory.CreateDirectory(jsonDirectory);
            }

            foreach (var metaData in metaDataList)
            {
                //image
                var bmpStream = this.CreateBitmapStream(userCode, metaData, 3000, 3000, System.Drawing.Imaging.ImageFormat.Png);
                System.IO.File.WriteAllBytes(imageDirectory + "\\" + metaData.ImageName, bmpStream.ToArray());
                //json
                System.IO.File.WriteAllText(jsonDirectory + "\\" + metaData.Edition + ".json", metaData.MyObjToJsonText());
            }
            //json metadata all
            System.IO.File.WriteAllText(jsonDirectory + "\\" + "_metadata.json", metaDataList.MyObjToJsonText());
            System.IO.Compression.ZipFile.CreateFromDirectory(exportDirectory, exportDirectory + ".zip");
        }

        public void GenerateImagesForParalel(string userCode, MoProjectInfo projectInfo, int quantity, string exportDirectory)
        {
            string imageDirectory = exportDirectory + "\\" + "images";
            if (!System.IO.Directory.Exists(imageDirectory))
            {
                System.IO.Directory.CreateDirectory(imageDirectory);
            }

            string jsonDirectory = exportDirectory + "\\" + "json";
            if (!System.IO.Directory.Exists(jsonDirectory))
            {
                System.IO.Directory.CreateDirectory(jsonDirectory);
            }

            int islemciAdet = 10; // Environment.ProcessorCount;
            var islemciler = new double[islemciAdet];
            for (int x = 1; x <= islemciAdet; x++)
            {
                islemciler[x - 1] = Math.Pow(2, x - 1); //işlemci numaraları üstel olduğundan ceviriyorum
            }

            ParallelOptions parOpts = new();
            parOpts.MaxDegreeOfParallelism = islemciAdet;

            Parallel.ForEach(islemciler, parOpts, p =>
            {
                int x = Array.IndexOf(islemciler, p);
                x += 1;

                var startIndex = quantity / islemciAdet * (x - 1);
                var endIndex = quantity / islemciAdet * x;

                for (int i = startIndex; i < endIndex; i++)
                {
                    var bmpStream = this.CreateBitmapStream(userCode, projectInfo, 3000, 3000, System.Drawing.Imaging.ImageFormat.Png);
                    string fileName = (1 + i).ToString().PadLeft(quantity.MyToStr().Length, '0') + ".png";
                    string pathFileName = imageDirectory + "\\" + fileName;
                    System.IO.File.WriteAllBytes(pathFileName, bmpStream.ToArray());
                }

            });

            System.IO.Compression.ZipFile.CreateFromDirectory(exportDirectory, exportDirectory + ".zip");
        }

        #endregion

        #region PreviewGenerate & StartGenerateImages

        public MoResponse<object> PreviewGenerateImages(string userCode, MoGenerateImageInput generateImageInput)
        {
            MoResponse<object> response = new();

            try
            {
                var metaDataList = this.CreateMetaData(userCode, generateImageInput);
                if (metaDataList.Success)
                {
                    List<string> images = new();
                    foreach (var metaData in metaDataList.Data)
                    {
                        using var bmpStream = this.CreateBitmapStream(userCode, metaData, 250, 250, System.Drawing.Imaging.ImageFormat.Png);

                        var str = "data:image/png;base64," + Convert.ToBase64String(bmpStream.ToArray());
                        images.Add(str);
                    }

                    response.Success = true;
                    response.Data = new { Images = images };
                }
                else
                {
                    response.Message = metaDataList.Message;
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> StartGenerateImages(string userCode, MoGenerateImageInput generateImageInput)
        {
            MoResponse<object> response = new();

            try
            {
                string exportDirectoryName = MyApp.UserExportDirectory(userCode, generateImageInput.ProjectName) + "\\" + DateTime.Now.ToString("yyyy.MM.dd_HH_mm_ss");
                var metaDataList = this.CreateMetaData(userCode, generateImageInput);
                if (metaDataList.Success)
                {
                    var task = Task.Run(() =>
                    {
                        this.GenerateImages(userCode, metaDataList.Data, exportDirectoryName);
                    });

                    //Dosyalar oluşturuluyor, tamamlandığında dosyalar bölümünden indirip dışa aktarabilirsiniz.
                    response.Message.Add("The files are being created, when completed, you can download and export them from the Files section.");
                    response.Success = true;
                    response.Data = new { ExportDirectoryName = exportDirectoryName };
                }
                else
                {
                    response.Message = metaDataList.Message;
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        #endregion

        #region metadata 
        public MoResponse<List<MoMetaData>> CreateMetaData(string userCode, MoGenerateImageInput generateImageInput)
        {
            MoResponse<List<MoMetaData>> response = new();

            try
            {
                System.Diagnostics.Stopwatch stopwatch1 = new();
                stopwatch1.Start();
                response.Data = new List<MoMetaData>();

                List<MoLayerInfo> layerInfoList = this.GetLayerInfoListForStatusTrue(userCode, generateImageInput).Data;

                stopwatch1.Stop();

                System.Diagnostics.Stopwatch stopwatch2 = new();
                stopwatch2.Start();
                int i = 0; int conflict = 0;
                while (response.Data.Count < generateImageInput.Quantity)
                {
                    MoMetaData metaData = new();
                    metaData.ProjectName = generateImageInput.ProjectName;
                    metaData.ImageName = $"{i}.png";
                    metaData.Name = $"#{i}";
                    metaData.Description = $"The {generateImageInput.ProjectName}  are a pack of {generateImageInput.Quantity} unique Crypto in NFT form";
                    metaData.Image = $"http://194.233.64.57/Data/Files/{generateImageInput.ProjectName}/images/{i}.png";
                    metaData.Edition = i;
                    metaData.Date = DateTime.Now.Ticks;

                    foreach (var layer in layerInfoList)
                    {
                        var imageList = layerInfoList.Where(c => c.LayerName == layer.LayerName).FirstOrDefault();
                        if (imageList != null)
                        {
                            var image = imageList.ImageList.
                                Where(c => c.UsableQuantity > 0)
                                .OrderBy(o => Guid.NewGuid()).FirstOrDefault();
                            if (image != null)
                            {
                                metaData.Attributes.Add(new MoAttribute() { ImageName = image.Name, TraitType = layer.LayerName, Value = System.IO.Path.GetFileNameWithoutExtension(image.Name) });
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Image exhausted in layer " + layer.LayerName);
                            }
                        }
                    }

                    //en sona
                    metaData.UniqueName = string.Join(',', metaData.Attributes.Select(s => s.Value).ToArray());
                    metaData.Dna = string.Join(',', metaData.Attributes.Select(s => s.Value).ToArray()).MyToMD5();
                    metaData.Compiler = "NFT Art Engine";

                    if (!response.Data.Where(c => c.UniqueName == metaData.UniqueName).Any())
                    {
                        response.Data.Add(metaData);
                        i++;

                        foreach (var attr in metaData.Attributes)
                        {
                            var layer = layerInfoList.Where(c => c.LayerName == attr.TraitType).FirstOrDefault();
                            var image = layer.ImageList.Where(c => c.Name == attr.ImageName).FirstOrDefault();
                            image.UsableQuantity -= 1;
                        }
                    }
                    else
                    {
                        conflict++;
                        System.Diagnostics.Debug.WriteLine($"conflict(data:{response.Data.Count} conflict:{conflict} )" + metaData.UniqueName);
                    }

                    if (conflict > 2000)
                    {
                        throw new Exception("The planning is not enough to create a unique file. " + "max:" + response.Data.Count);
                    }
                }

                stopwatch2.Stop();

                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Message.Add("Warning: " + ex.MyLastInner().Message);
            }

            return response;
        }

        #endregion

        #region Projects method

        public MoResponse<List<MoProject>> GetProjectList(string userCode)
        {
            MoResponse<List<MoProject>> response = new();
            try
            {
                if (System.IO.Directory.Exists(MyApp.UserFilesDirectory(userCode)))
                {
                    var data = new System.IO.DirectoryInfo(MyApp.UserFilesDirectory(userCode))
                         .GetDirectories()
                         .OrderBy(o => o.Name)
                         .Select(s => new MoProject
                         {
                             Name = s.Name,
                             ImageUrl = (MyApp.UserImportDirectory(userCode, s.Name) + "\\" + "project.png").Replace(MyApp.Env.WebRootPath, "").Replace("\\", "/")
                         }).ToList();

                    response.Success = true;
                    response.Data = data;
                }
                else
                {
                    response.Success = true;
                    response.Data = new List<MoProject>();
                }

            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<List<MoLayer>> GetProjectLayerList(string userCode, string projectName)
        {
            MoResponse<List<MoLayer>> response = new();

            try
            {
                var data = new System.IO.DirectoryInfo(MyApp.UserImportDirectory(userCode, projectName))
                     .GetDirectories()
                     .OrderBy(o => o.Name)
                     .Select(s => new MoLayer
                     {
                         Name = s.Name
                     }).ToList();

                response.Success = true;
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<MoProjectInfo> GetProjectInfo(string userCode, string projectName)
        {
            MoResponse<MoProjectInfo> response = new() { Data = new MoProjectInfo() };

            try
            {
                if (!string.IsNullOrEmpty(projectName))
                {
                    var projectLayerList = this.GetProjectLayerList(userCode,projectName);

                    string jsonFileName = MyApp.UserImportDirectory(userCode,projectName) + "//project.json";

                    //json dosya yok ise oluşturuluyor
                    if (!System.IO.File.Exists(jsonFileName))
                    {
                        response.Data.ProjectName = projectName;
                        response.Data.LayerList = projectLayerList.Data;
                        this.SetProjectInfo(userCode, response.Data);
                    }
                    else
                    {
                        //bu satırda json dosya kesin olmalı
                        string jsonString = System.IO.File.ReadAllText(jsonFileName);
                        response.Data = System.Text.Json.JsonSerializer.Deserialize<MoProjectInfo>(jsonString);
                        response.Data.ProjectName = projectName;

                        List<string> deleteItemNames = new();
                        // jsonda olupda dizin yoksa sil
                        foreach (var item in response.Data.LayerList)
                        {
                            if (!projectLayerList.Data.Where(c => c.Name == item.Name).Any())
                            {
                                //response.Data.LayerList.Remove(item); //hata verir deneme
                                deleteItemNames.Add(item.Name);
                            }
                        }
                        response.Data.LayerList = response.Data.LayerList.Where(c => !deleteItemNames.Contains(c.Name)).ToList();

                        //dizin olupda jsonda yoksa ekle
                        foreach (var item in projectLayerList.Data)
                        {
                            if (!response.Data.LayerList.Where(c => c.Name == item.Name).Any())
                            {
                                response.Data.LayerList.Add(item);
                            }
                        }
                    }

                    response.Success = true;
                }
                else
                {
                    response.Message.Add("'projectName' cannot be empty.");
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> SetProjectInfo(string userCode, MoProjectInfo projectInfo)
        {
            MoResponse<object> response = new();

            try
            {
                string jsonFileName = MyApp.UserImportDirectory(userCode, projectInfo.ProjectName) + "//project.json";
                string jsonString = System.Text.Json.JsonSerializer.Serialize(projectInfo);

                System.IO.File.WriteAllText(jsonFileName, jsonString);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        #endregion

        #region layer methods

        public MoResponse<List<MoImage>> GetLayerImage(string userCode, string projectName, string layerName)
        {
            MoResponse<List<MoImage>> response = new();

            var files = new System.IO.DirectoryInfo(MyApp.UserImportDirectory(userCode, projectName) + "\\" + layerName)
                    .EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                    .OrderBy(p => p.Name).ToArray();

            var data = files
             .Select(s => new MoImage
             {
                 Name = s.Name,
                 Status = true,
                 UsagePercentage = 0
             })
            .ToList();

            foreach (var item in data)
            {
                string fileUrl = MyApp.UserImportDirectory(userCode, projectName) + "\\" + layerName + "\\" + item.Name;

                if (System.IO.File.Exists(fileUrl))
                {
                    //var file = new System.IO.FileInfo(fileUrl);
                    using var img = System.Drawing.Image.FromFile(fileUrl);
                    item.ImageWidth = img.Width;
                    item.ImageHeight = img.Height;
                }

                item.ImageUrl = MyApp.UserImportDirectory(userCode, projectName) + "\\" + layerName + "\\" + item.Name + "?v." + DateTime.Now.ToString("yyyyMMddHHmmss");
                item.ImageUrl = item.ImageUrl.Replace(MyApp.Env.WebRootPath, "");
                item.ImageUrl = item.ImageUrl.Replace("\\", "/");
            }


            response.Success = true;
            response.Data = data;

            return response;
        }

        public MoResponse<MoLayerInfo> GetLayerInfo(string userCode, MoLayerInfoInput layerInfoInput)
        {
            MoResponse<MoLayerInfo> response = new() { Data = new MoLayerInfo() };

            try
            {
                if (!string.IsNullOrEmpty(layerInfoInput.ProjectName))
                {
                    var layerImageList = this.GetLayerImage(userCode, layerInfoInput.ProjectName, layerInfoInput.LayerName);

                    string jsonFileName = MyApp.UserImportDirectory(userCode, layerInfoInput.ProjectName) + "//" + layerInfoInput.LayerName + ".json";

                    //json dosya yok ise oluşturuluyor
                    if (!System.IO.File.Exists(jsonFileName))
                    {
                        response.Data.ProjectName = layerInfoInput.ProjectName;
                        response.Data.LayerName = layerInfoInput.LayerName;
                        response.Data.ImageList = layerImageList.Data;
                        this.SetLayerInfo(userCode, response.Data);
                    }
                    else
                    {
                        //bu satırda json dosya kesin olmalı
                        string jsonString = System.IO.File.ReadAllText(jsonFileName);
                        response.Data = System.Text.Json.JsonSerializer.Deserialize<MoLayerInfo>(jsonString);
                        response.Data.ProjectName = layerInfoInput.ProjectName;
                        response.Data.LayerName = layerInfoInput.LayerName;

                        List<string> deleteItemNames = new();
                        // jsonda olupda dizin yoksa sil
                        foreach (var item in response.Data.ImageList)
                        {
                            var query = layerImageList.Data.Where(c => c.Name == item.Name);
                            if (!query.Any())
                            {
                                //response.Data.ImageList.Remove(item); //hata verir deneme
                                deleteItemNames.Add(item.Name);
                            }
                            else
                            {
                                item.ImageUrl = query.FirstOrDefault().ImageUrl;
                                item.ImageWidth = query.FirstOrDefault().ImageWidth;
                                item.ImageHeight = query.FirstOrDefault().ImageHeight;
                            }
                        }
                        response.Data.ImageList = response.Data.ImageList.Where(c => !deleteItemNames.Contains(c.Name)).ToList();

                        //dizin olupda jsonda yoksa ekle
                        foreach (var item in layerImageList.Data)
                        {
                            if (!response.Data.ImageList.Where(c => c.Name == item.Name).Any())
                            {
                                response.Data.ImageList.Add(item);
                            }
                        }
                    }

                    response.Data.ImageList = response.Data.ImageList
                        .OrderByDescending(o => o.UsagePercentage).ThenBy(o => o.Name)
                        .ToList();

                    response.Success = true;
                }
                else
                {
                    response.Message.Add("'projectName' and 'layerName' cannot be empty.");
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<List<MoLayerInfo>> GetLayerInfoListForStatusTrue(string userCode, MoGenerateImageInput generateImageInput)
        {
            MoResponse<List<MoLayerInfo>> response = new() { Data = new List<MoLayerInfo>() };

            try
            {
                //üretimde kullanılacak bir metot
                var layerList = this.GetProjectInfo(userCode, generateImageInput.ProjectName).Data.LayerList.Where(c => c.Status == true).ToList();

                foreach (var layer in layerList)
                {
                    var res = this.GetLayerInfo(userCode, new MoLayerInfoInput()
                    {
                        ProjectName = generateImageInput.ProjectName,
                        LayerName = layer.Name
                    });

                    var imageList = res.Data.ImageList.Where(c => c.Status == true && c.UsagePercentage > 0).ToList();
                    //yüzde kullanımına uyan layerlerı dön
                    if (imageList.Sum(s => s.UsagePercentage) == 100)
                    {
                        var LayerInfo = new MoLayerInfo()
                        {
                            ProjectName = res.Data.ProjectName,
                            LayerName = res.Data.LayerName,
                            ImageList = imageList
                        };
                        response.Data.Add(LayerInfo);
                    }
                }

                //resimlerin kullanım yüzdesine göre kaç adet kullanılabileceği
                foreach (var layerInfo in response.Data)
                {
                    foreach (var image in layerInfo.ImageList)
                    {
                        var sayi = Math.Ceiling((generateImageInput.Quantity / 100) * image.UsagePercentage);
                        image.UsableQuantity = Convert.ToInt32(Math.Ceiling(sayi * 1.40m)); //olasılığı %20 artırıyoruz
                        if (image.UsableQuantity == 0)
                        {
                            image.UsableQuantity = 1;
                        }
                    }

                    var sumUsableQuantity = layerInfo.ImageList.Sum(s => s.UsableQuantity);
                    if (sumUsableQuantity < generateImageInput.Quantity)
                    {
                        var enfazlaOlan = layerInfo.ImageList.OrderBy(o => o.UsableQuantity).LastOrDefault();
                        enfazlaOlan.UsableQuantity += generateImageInput.Quantity - sumUsableQuantity;
                    }

                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> SetLayerInfo(string userCode, MoLayerInfo layerInfo)
        {
            MoResponse<object> response = new();

            try
            {
                string jsonFileName = MyApp.UserImportDirectory(userCode, layerInfo.ProjectName) + "//" + layerInfo.LayerName + ".json";
                string jsonString = System.Text.Json.JsonSerializer.Serialize(layerInfo);

                System.IO.File.WriteAllText(jsonFileName, jsonString);

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> ChangeLayerName(string userCode, MoLayerNameChangeInput layerNameChangeInput)
        {
            MoResponse<object> response = new();

            try
            {
                var resGetProjectInfo = this.GetProjectInfo(userCode, layerNameChangeInput.ProjectName);
                if (resGetProjectInfo.Success)
                {
                    string oldLayerDirectory = MyApp.UserImportDirectory(userCode, layerNameChangeInput.ProjectName) + "//" + layerNameChangeInput.OldLayerName;
                    string newLayerDirectory = MyApp.UserImportDirectory(userCode, layerNameChangeInput.ProjectName) + "//" + layerNameChangeInput.NewLayerName;

                    //layer directory değişecek
                    System.IO.DirectoryInfo di = new(oldLayerDirectory);
                    if (di.Exists)
                    {
                        di.MoveTo(newLayerDirectory);

                        //layer json değişecek
                        string oldLayerFileName = MyApp.UserImportDirectory(userCode, layerNameChangeInput.ProjectName) + "//" + layerNameChangeInput.OldLayerName + ".json";
                        string newLayerFileName = MyApp.UserImportDirectory(userCode, layerNameChangeInput.ProjectName) + "//" + layerNameChangeInput.NewLayerName + ".json";
                        System.IO.FileInfo fi = new(oldLayerFileName);
                        if (fi.Exists)
                        {
                            fi.MoveTo(newLayerFileName);

                            //project infoda değişecek
                            var layerItem = resGetProjectInfo.Data.LayerList.Where(c => c.Name == layerNameChangeInput.OldLayerName).FirstOrDefault();
                            layerItem.Name = layerNameChangeInput.NewLayerName;
                            var resSetProjectInfo = this.SetProjectInfo(userCode, resGetProjectInfo.Data);
                            if (resSetProjectInfo.Success)
                            {
                                response.Success = true;
                            }
                            else
                            {
                                response.Message = response.Message;
                            }
                        }
                        else
                        {
                            response.Message.Add("Error: " + "File not found. " + System.IO.Path.GetFileName(oldLayerFileName));
                        }
                    }
                    else
                    {
                        response.Message.Add("Error: " + "Directory not found." + System.IO.Path.GetDirectoryName(oldLayerDirectory));
                    }
                }
                else
                {
                    response.Message = resGetProjectInfo.Message;
                }
            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        public MoResponse<object> ChangeImageName(string userCode, MoImageNameChangeInput imageNameChangeInput)
        {
            MoResponse<object> response = new();

            try
            {
                var resGetLayerInfo = this.GetLayerInfo(userCode, new MoLayerInfoInput()
                {
                    ProjectName = imageNameChangeInput.ProjectName,
                    LayerName = imageNameChangeInput.LayerName
                });

                if (resGetLayerInfo.Success)
                {
                    //layer json değişecek
                    string oldImageFileName = MyApp.UserImportDirectory(userCode, imageNameChangeInput.ProjectName) + "//" + imageNameChangeInput.LayerName + "//" + imageNameChangeInput.OldImageName;
                    string newImageFileName = MyApp.UserImportDirectory(userCode, imageNameChangeInput.ProjectName) + "//" + imageNameChangeInput.LayerName + "//" + imageNameChangeInput.NewImageName;
                    System.IO.FileInfo fi = new(oldImageFileName);
                    if (fi.Exists)
                    {
                        fi.MoveTo(newImageFileName);

                        //layer infoda değişecek
                        var layerItem = resGetLayerInfo.Data.ImageList.Where(c => c.Name == imageNameChangeInput.OldImageName).FirstOrDefault();
                        layerItem.Name = imageNameChangeInput.NewImageName;
                        var resSetProjectInfo = this.SetLayerInfo(userCode, resGetLayerInfo.Data);
                        if (resSetProjectInfo.Success)
                        {
                            response.Success = true;
                        }
                        else
                        {
                            response.Message = response.Message;
                        }
                    }
                    else
                    {
                        response.Message.Add("Error: " + "File not found. " + System.IO.Path.GetFileName(oldImageFileName));
                    }
                }
                else
                {
                    response.Message = resGetLayerInfo.Message;
                }

            }
            catch (Exception ex)
            {
                response.Message.Add("Error: " + ex.MyLastInner().Message);
            }

            return response;
        }

        #endregion

    }
}
