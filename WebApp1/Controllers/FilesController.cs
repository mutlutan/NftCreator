using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using WebApp1.Codes;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Drawing;

namespace WebApp1.Controllers
{
    public class FilesController : _Controller
    {
        public FilesController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        [HttpGet]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_R.")]
        public IActionResult ReadDirectoryList(string id)
        {
            DataSourceResult dsr = new();

            try
            {
                //thumbs dir
                string thumbsDirectoryPath = MyApp.EnvWebRootPath + "\\" + MyApp.AppThumbsDirectory;
                if (!System.IO.Directory.Exists(thumbsDirectoryPath))
                {
                    var newDir = System.IO.Directory.CreateDirectory(thumbsDirectoryPath);
                    newDir.Attributes = System.IO.FileAttributes.Hidden;
                }

                //files dir
                string rootDirectoryPath = MyApp.EnvWebRootPath + "\\" + MyApp.AppFilesDirectory;
                if (!System.IO.Directory.Exists(rootDirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(rootDirectoryPath);
                }

                if (!string.IsNullOrEmpty(id))
                {
                    rootDirectoryPath = id;
                }

                var directories = new System.IO.DirectoryInfo(rootDirectoryPath).GetDirectories()
                    .Where(c => !c.Attributes.HasFlag(System.IO.FileAttributes.Hidden))
                    .Select(dir => new
                    {
                        id = dir.FullName,
                        hasChildren = new System.IO.DirectoryInfo(dir.FullName).GetDirectories().Where(c => !c.Attributes.HasFlag(System.IO.FileAttributes.Hidden)).Any(),
                        text = dir.Name,
                        expanded = false
                    });

                dsr.Data = directories;
                dsr.Total = directories.Count();
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }

            return Json(dsr);
        }

        [HttpGet]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_R.")]
        public IActionResult ReadFilesInDirectory(string _DirectoryName)
        {
            DataSourceResult dsr = new();

            try
            {
                string filePath = MyApp.EnvWebRootPath + "\\" + MyApp.AppFilesDirectory;

                if (!string.IsNullOrEmpty(_DirectoryName))
                {
                    filePath = _DirectoryName;
                }

                if (System.IO.Directory.Exists(filePath))
                {
                    List<MyFile> myFileList = new();
                    System.IO.DirectoryInfo directoryInfo = new(filePath);

                    //if (directoryInfo.Attributes != System.IO.FileAttributes.Hidden)
                    //{
                    System.IO.FileInfo[] files = directoryInfo
                        .EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                        .OrderBy(p => p.Name).ToArray();

                    foreach (System.IO.FileInfo file in files)
                    {
                        string fileId = file.DirectoryName.Replace(MyApp.EnvWebRootPath, "") + "\\" + file.Name;
                        string fileUrl = fileId.Replace("\\", "/");
                        string fileVersion = "";

                        string thumbsRootDir = MyApp.EnvWebRootPath + "\\" + MyApp.AppFilesDirectory;
                        string thumbFileName = file.FullName.Replace(MyApp.EnvWebRootPath + "\\" + MyApp.AppFilesDirectory, MyApp.EnvWebRootPath + "\\" + MyApp.AppThumbsDirectory);
                        string thumbUrl = thumbFileName.Replace(MyApp.EnvWebRootPath, "");

                        string extension = file.Extension.MyToLower().Replace(".","");// ".xxx" => "xxx"
                        string[] imgExtensions = new string[] { "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "apng", "webp", "gif", "svg" };
                        string[] imgExtensionsForVersion = new string[] { "jpg", "jpeg", "png", "gif" };
                        string[] imgExtensionsForthumb = new string[] { "jpg", "jpeg", "jfif", "pjpeg", "pjp", "png", "apng", "webp", "gif" }; //thumbu oluşacak olan dosyalar


                        if (!imgExtensions.Contains(extension))
                        {
                            thumbUrl = $"/img/file/{extension}.png";
                        }
                        else
                        if (!imgExtensionsForthumb.Contains(extension))
                        {
                            thumbUrl = fileUrl;
                        }
                        else
                        {
                            //thumb yok ise
                            //if (!System.IO.File.Exists(thumbFileName))
                            if (true) //her durumda yapsın dosya güncellemeleri başka bir kanaldan olunca thumb eski kalıyor
                            {
                                try
                                {
                                    using var originalImage = System.Drawing.Image.FromFile(file.FullName);
                                    var newWidth = 100;
                                    using var resizedImage = originalImage.GetThumbnailImage(newWidth, (newWidth * originalImage.Height) / originalImage.Width, null, IntPtr.Zero);
                                    string thumbPath = System.IO.Path.GetDirectoryName(thumbFileName);
                                    if (!System.IO.Directory.Exists(thumbPath))
                                    {
                                        System.IO.Directory.CreateDirectory(thumbPath);
                                    }
                                    resizedImage.Save(thumbFileName);
                                }
                                catch { }
                            }
                        }

                        if (imgExtensionsForVersion.Contains(extension))
                        {
                            //version set ediliyor, image olanlara version set ediliyor
                            fileVersion = "?v." + file.LastWriteTime.Ticks.ToString();
                        }

                        myFileList.Add(new MyFile()
                        {
                            Id = fileId,
                            Name = file.Name,
                            Extension = file.Extension,
                            FileUrl = fileUrl.Replace("\\", "/"),
                            FileViewUrl = thumbUrl.Replace("\\", "/") + fileVersion,
                            FileVersion = fileVersion,
                            ModifiedDate = file.LastWriteTime,
                            Size = file.Length,
                            SizeText = (file.Length / 1024).MyToStr() + " KB"
                        });
                    }

                    dsr.Data = myFileList; // myFileList.Skip(skip).Take(take);
                    dsr.Total = myFileList.Count;
                    //}
                }
            }
            catch (Exception ex)
            {
                dsr.Errors = ex.MyLastInner().Message;
            }
            return Json(dsr);
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_C.")]
        public IActionResult AddDirectory(string _directoryName)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                if (!System.IO.Directory.Exists(_directoryName))
                {
                    System.IO.Directory.CreateDirectory(_directoryName);

                    System.IO.DirectoryInfo di = new(_directoryName);
                    if (di.Parent.Name == "tr-TR" || di.Parent.Name == "en-US")
                    {
                        System.IO.Directory.CreateDirectory(_directoryName + "//Anlatim");
                        System.IO.Directory.CreateDirectory(_directoryName + "//Klavuz");
                        System.IO.Directory.CreateDirectory(_directoryName + "//Kodlar");
                        System.IO.Directory.CreateDirectory(_directoryName + "//Tanitim");
                    }

                    rMessage += MyApp.TranslateTo("xLng.viewGorseller.DizinEklendi", this.dataContext.Language);
                }
                else
                {
                    rMessage += MyApp.TranslateTo("xLng.viewGorseller.EklemekIstediginizDizinMevcut", this.dataContext.Language);
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }

        [HttpGet]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_C.")]
        public IActionResult DownloadDirectory(string directoryName)
        {
            string rMessage = "";
            try
            {
                if (System.IO.Directory.Exists(directoryName))
                {
                    System.IO.DirectoryInfo directoryInfo = new(directoryName);
                    var files = directoryInfo.EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories);
                    if (files.Any())
                    {
                        var zipFileMemoryStream = new System.IO.MemoryStream();
                        using (var archive = new System.IO.Compression.ZipArchive(zipFileMemoryStream, System.IO.Compression.ZipArchiveMode.Update, leaveOpen: true))
                        {
                            foreach (var file in files)
                            {
                                string entryName = directoryInfo.Name + file.DirectoryName.Replace(directoryName, "") + "\\" + file.Name;
                                var entry = archive.CreateEntry(entryName);
                                using var entryStream = entry.Open();
                                using var img = Bitmap.FromFile(file.FullName);
                                img.Save(entryStream, System.Drawing.Imaging.ImageFormat.Png);

                            }
                        }
                        zipFileMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                        return File(zipFileMemoryStream, "application/octet-stream", directoryInfo.Name + ".zip");
                    }
                    else
                    {
                        rMessage = "The directory you want to download is not empty";
                    }
                }
                else
                {
                    rMessage = "The directory you want to download is not found";
                }
            }
            catch (Exception ex)
            {
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { sMessage = rMessage });
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_D.")]
        public IActionResult DeleteDirectory(string _directoryName)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                if (System.IO.Directory.Exists(_directoryName))
                {
                    System.IO.DirectoryInfo directoryInfo = new(_directoryName);
                    var files = directoryInfo.EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories);
                    if (!files.Any())
                    {
                        directoryInfo.Delete(false);
                        rMessage = MyApp.TranslateTo("xLng.viewGorseller.DizinSilindi", this.dataContext.Language);
                    }
                    else
                    {
                        rError = true;
                        rMessage = MyApp.TranslateTo("xLng.viewGorseller.SilmekIstediginizDizinBosDegil", this.dataContext.Language);
                    }
                }
                else
                {
                    rError = true;
                    rMessage = MyApp.TranslateTo("xLng.viewGorseller.SilmekIstediginizDizinBulunamadı", this.dataContext.Language);
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_D.")]
        public IActionResult ClearDirectory(string _directoryName)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                if (System.IO.Directory.Exists(_directoryName))
                {
                    System.IO.DirectoryInfo directoryInfo = new(_directoryName);
                    var dirs = directoryInfo.GetDirectories("*.*",System.IO.SearchOption.TopDirectoryOnly);
                    
                    if (!dirs.Any())
                    {
                        directoryInfo.EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                            .ToList().ForEach(f => f.Delete());
                        rMessage = MyApp.TranslateTo("xLng.viewGorseller.DizinTemizlendi", this.dataContext.Language);
                    }
                    else
                    {
                        rError = true;
                        rMessage = MyApp.TranslateTo("xLng.viewGorseller.TemizlemekIstediginizDizinIcindeDizinOlmamaz", this.dataContext.Language);
                    }
                }
                else
                {
                    rError = true;
                    rMessage = MyApp.TranslateTo("xLng.viewGorseller.TemizlemekIstediginizDizinBulunamadı", this.dataContext.Language);
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }


        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_C.")]
        [RequestSizeLimit(52428800 * 3)] //150mb
        public IActionResult UploadFile(string _directoryName, string _fileName, string _fileContent)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                string imgFullFileName = _directoryName + "/" + _fileName;

                //image anlatım dizinine atılıyorsa boyutlandırıyoruz
                System.IO.DirectoryInfo di = new(_directoryName);
                if (di.Name == "Anlatim" && (di.Parent.Parent.Name == "tr-TR" || di.Parent.Parent.Name == "en-US"))
                {
                    Image imgFrom = (Bitmap)((new ImageConverter()).ConvertFrom(Convert.FromBase64String(_fileContent)));

                    if (imgFrom.Size.Width > 1920 || imgFrom.Size.Height > 1080)
                    {
                        var imgTo = (Image)(new Bitmap(imgFrom, new Size(1920, 1080)));
                        imgTo.Save(imgFullFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                    {
                        System.IO.File.WriteAllBytes(imgFullFileName, Convert.FromBase64String(_fileContent));
                    }
                }
                else
                {
                    System.IO.File.WriteAllBytes(imgFullFileName, Convert.FromBase64String(_fileContent));
                }

                rMessage += MyApp.TranslateTo("xLng.viewGorseller.DosyaGonderildi", this.dataContext.Language);
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage });
        }

        [HttpPost]
        [AuthenticateRequired(AuthorityGrups = "Admin,Personel", AuthorityKeys = "Tem.Tanim.Gorseller.D_D.")]
        public IActionResult RemoveFile(string _fileId)
        {
            Boolean rError = false;
            string rMessage = "";
            try
            {
                string fileFullName = MyApp.EnvWebRootPath + _fileId;

                //delete normal file
                if (System.IO.File.Exists(fileFullName))
                {
                    System.IO.File.Delete(fileFullName);
                    rMessage += MyApp.TranslateTo("xLng.viewGorseller.DosyaSilindi", this.dataContext.Language);
                }
                else
                {
                    rError = true;
                    rMessage += MyApp.TranslateTo("xLng.viewGorseller.DosyaBulunamadi", this.dataContext.Language);
                }

                //delete thumb
                string thumbFileName = fileFullName.Replace(MyApp.EnvWebRootPath + "\\" + MyApp.AppFilesDirectory, MyApp.EnvWebRootPath + "\\" + MyApp.AppThumbsDirectory);

                if (System.IO.File.Exists(thumbFileName))
                {
                    System.IO.File.Delete(thumbFileName);
                }

            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }

            return Json(new { bError = rError, sMessage = rMessage });
        }


    }
}
