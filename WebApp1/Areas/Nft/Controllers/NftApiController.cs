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
    public class NftController : _Controller
    {
        public NftController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }




    }
}
