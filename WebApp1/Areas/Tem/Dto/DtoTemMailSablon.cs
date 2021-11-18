using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemMailSablon : TemMailSablon
    {
        protected readonly DataContext dataContext;

        public string CcAntetIdAd{ get; set; }

        //Constructor
        public DtoTemMailSablon(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


