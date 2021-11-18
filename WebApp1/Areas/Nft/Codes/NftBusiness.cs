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
            if (_dataContext != null)
            {
                this.dataContext = new DataContext(new DbContextOptions<DataContext>());
                this.dataContext.Database.GetDbConnection().ConnectionString = _dataContext.Database.GetDbConnection().ConnectionString;
                this.dataContext.IPAddress = _dataContext.IPAddress;
                this.dataContext.KurulusKod = _dataContext.KurulusKod;
                this.dataContext.ConStrings = _dataContext.ConStrings;
                this.dataContext.UserId = _dataContext.UserId;
                this.dataContext.UserName = _dataContext.UserName;

                this.rep = new Models._Rep(this.dataContext);
            }
        }


    }
}
