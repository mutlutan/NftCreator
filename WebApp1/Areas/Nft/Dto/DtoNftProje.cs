using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Nft.Dto
{
    public partial class DtoNftProje : NftProje
    {
        protected readonly DataContext dataContext;

        public string CcDurum
        {
            get { return (this.Durum ? MyApp.TranslateTo("xLng.Aktif", this.dataContext.Language) : MyApp.TranslateTo("xLng.Pasif", this.dataContext.Language)); }
        }
        public string CcKullaniciIdAd{ get; set; }

        //Constructor
        public DtoNftProje(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


