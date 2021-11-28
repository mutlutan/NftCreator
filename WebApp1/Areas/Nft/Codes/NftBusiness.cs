using System;
using System.Collections.Generic;
using System.Linq;
using WebApp1.Codes;
using WebApp1.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Reflection;

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


    }
}
