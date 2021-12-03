using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp1.Codes;
using WebApp1.Controllers;
using Microsoft.EntityFrameworkCore;
using WebApp1.Areas.Nft.Codes;
using System.Reflection;


namespace WebApp1.Areas.Nft.Controllers
{
    //[Produces("application/json", "application/xml")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NftController : _Controller
    {
        public NftController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        #region resim işlemelri
        [HttpPost("SetProjectIcon")]
        [ResponseCache(Duration = 0)]
        public ActionResult SetProjectIcon([FromBody] object obj)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            dynamic jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(obj.ToString());
            string projectName = jsonResponse.projectName;

            MoResponse<object> response = nftBusiness.SetProjectIcon(userToken.UserCode, projectName);

            return Json(response);
        }

        [HttpPost("PreviewGenerateImages")]
        [ResponseCache(Duration = 0)]
        public ActionResult PreviewGenerateImages([FromBody] MoGenerateImageInput generateImageInput)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            MoResponse<object> response = nftBusiness.PreviewGenerateImages(this.userToken.UserCode, generateImageInput);

            return Json(response);
        }

        [HttpPost("StartGenerateImages")]
        [ResponseCache(Duration = 0)]
        public ActionResult StartGenerateImages([FromBody] MoGenerateImageInput generateImageInput)
        {
            var nftBusiness = new NftBusiness(this.dataContext);
            MoResponse<object> response = nftBusiness.StartGenerateImages(this.userToken, generateImageInput);

            return Json(response);
        }

        #endregion

        #region export planlama
        [HttpPost("AddExport")]
        [ResponseCache(Duration = 0)]
        public ActionResult AddExport([FromBody] object obj)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            dynamic jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(obj.ToString());
            string projectName = jsonResponse.projectName;
            int quantity = jsonResponse.quantity;

            var response = nftBusiness.AddExport(this.userToken, projectName, quantity);

            return Json(response);
        }

        [HttpPost("DeleteExportDirectory")]
        [ResponseCache(Duration = 0)]
        public ActionResult DeleteExportDirectory([FromBody] object obj)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            dynamic jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(obj.ToString());
            string projectName = jsonResponse.projectName;
            string directoryName = jsonResponse.directoryName;

            var response = nftBusiness.DeleteExportDirectory(this.userToken.UserCode, projectName, directoryName);

            return Json(response);
        }

        #endregion

        #region proje işlemelri

        [HttpGet("GetProjectList")]
        [ResponseCache(Duration = 0)]
        public ActionResult GetProjectList()
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            var response = nftBusiness.GetProjectList(this.userToken.UserCode);

            return Json(response);
        }


        [HttpPost("AddProject")]
        [ResponseCache(Duration = 0)]
        public ActionResult AddProject([FromBody] object obj)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            dynamic jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(obj.ToString());
            string projectName = jsonResponse.projectName;

            var response = nftBusiness.AddProject(this.userToken.UserCode, projectName);

            return Json(response);
        }

        [HttpPost("GetProjectInfo")]
        [ResponseCache(Duration = 0)]
        public ActionResult GetProjectInfo([FromBody] object obj)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            dynamic jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(obj.ToString());
            string projectName = jsonResponse.projectName;

            var response = nftBusiness.GetProjectInfo(this.userToken.UserCode, projectName);

            return Json(response);
        }

        [HttpPost("SetProjectInfo")]
        [ResponseCache(Duration = 0)]
        public ActionResult SetProjectInfo([FromBody] MoProjectInfo projectInfo)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            MoResponse<object> response = nftBusiness.SetProjectInfo(this.userToken.UserCode, projectInfo);

            return Json(response);
        }

        #endregion

        #region Layer işlemelri

        [HttpPost("GetLayerInfo")]
        [ResponseCache(Duration = 0)]
        public ActionResult GetLayerInfo([FromBody] MoLayerInfoInput layerInfoInput)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            var response = nftBusiness.GetLayerInfo(this.userToken.UserCode, layerInfoInput);

            return Json(response);
        }

        [HttpPost("SetLayerInfo")]
        [ResponseCache(Duration = 0)]
        public ActionResult SetLayerInfo([FromBody] MoLayerInfo layerInfo)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            MoResponse<object> response = nftBusiness.SetLayerInfo(this.userToken.UserCode, layerInfo);

            return Json(response);
        }

        [HttpPost("ChangeLayerName")]
        [ResponseCache(Duration = 0)]
        public ActionResult ChangeLayerName([FromBody] MoLayerNameChangeInput layerNameChangeInput)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            MoResponse<object> response = nftBusiness.ChangeLayerName(this.userToken.UserCode, layerNameChangeInput);

            return Json(response);
        }

        [HttpPost("ChangeImageName")]
        [ResponseCache(Duration = 0)]
        public ActionResult ChangeImageName([FromBody] MoImageNameChangeInput imageNameChangeInput)
        {
            var nftBusiness = new NftBusiness(this.dataContext);

            MoResponse<object> response = nftBusiness.ChangeImageName(this.userToken.UserCode, imageNameChangeInput);

            return Json(response);
        }

        #endregion



    }
}
