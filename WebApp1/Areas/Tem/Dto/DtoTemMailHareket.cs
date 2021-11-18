using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemMailHareket : TemMailHareket
    {
        protected readonly DataContext dataContext;

        public string CcSablonIdKonu{ get; set; }
        public string CcDurumIdAd{ get; set; }

        //Constructor
        public DtoTemMailHareket(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


