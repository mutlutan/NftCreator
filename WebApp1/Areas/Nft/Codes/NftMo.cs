using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApp1.Areas.Nft.Codes
{
    #region Modeller
    public class MoProjectExport
    {
        public string DirectoryName { get; set; }
        public string DownloadUrl { get; set; }
        public int PlannedImageQuantity { get; set; } = 0; //palalalan image sayısı
        public int CreatedImageQuantity { get; set; } = 0; //Oluşan image sayısı
    }
    public class MoProject
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<MoProjectExport> ExportList { get; set; } = new List<MoProjectExport>();
    }

    public class MoLayer
    {
        public string Name { get; set; }
        public Boolean Status { get; set; } = false;
    }

    public class MoImage
    {
        public string Name { get; set; }
        public Boolean Status { get; set; } = false;
        public decimal UsagePercentage { get; set; } = 0;
        public int UsableQuantity { get; set; } = 0; //

        public string ImageUrl { get; set; }
        public int ImageWidth { get; set; } = 0;
        public int ImageHeight { get; set; } = 0;
    }

    public class MoProjectInfo
    {
        public string ProjectName { get; set; }
        public List<MoLayer> LayerList { get; set; } = new List<MoLayer>();
    }

    public class MoLayerInfo
    {
        public string ProjectName { get; set; }
        public string LayerName { get; set; }

        public List<MoImage> ImageList { get; set; } = new List<MoImage>();

    }

    public class MoLayerInfoInput
    {
        public string ProjectName { get; set; }
        public string LayerName { get; set; }
    }

    public class MoLayerNameChangeInput
    {
        public string ProjectName { get; set; }
        public string OldLayerName { get; set; }
        public string NewLayerName { get; set; }
    }

    public class MoImageNameChangeInput
    {
        public string ProjectName { get; set; }
        public string LayerName { get; set; }
        public string OldImageName { get; set; }
        public string NewImageName { get; set; }
    }

    public class MoGenerateImageInput
    {
        public string ProjectName { get; set; }
        public int Quantity { get; set; }

    }
    #endregion

    #region meta models
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class MoAttribute
    {
        [JsonPropertyName("trait_type")]
        public string TraitType { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonIgnore]
        public string ImageName { get; set; }
    }

    public class MoMetaData
    {
        [JsonIgnore]
        public string ProjectName { get; set; }

        [JsonIgnore]
        public string ImageName { get; set; }

        [JsonIgnore]
        public string UniqueName { get; set; }

        [JsonPropertyName("dna")]
        public string Dna { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("edition")]
        public int Edition { get; set; }

        [JsonPropertyName("date")]
        public long Date { get; set; }

        [JsonPropertyName("attributes")]
        public List<MoAttribute> Attributes { get; set; } = new List<MoAttribute>();

        [JsonPropertyName("compiler")]
        public string Compiler { get; set; }
    }


    #endregion

    #region Gorseller model
    public class MyFile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FileUrl { get; set; }
        public string FileViewUrl { get; set; }
        public string FileVersion { get; set; }
        public long Size { get; set; }
        public string SizeText { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    #endregion


}
