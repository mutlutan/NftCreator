using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemMailAntet : TemMailAntet
    {
        protected readonly DataContext dataContext;


        //Constructor
        public DtoTemMailAntet(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


