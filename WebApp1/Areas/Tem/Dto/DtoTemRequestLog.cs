using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemRequestLog : TemRequestLog
    {
        protected readonly DataContext dataContext;


        //Constructor
        public DtoTemRequestLog(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


