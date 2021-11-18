using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApp1.Codes;
using WebApp1.Models;

//<!-- Auto Generated user1 -->

namespace WebApp1.Areas.Tem.Dto
{
    public partial class DtoTemMesaj : TemMesaj
    {
        protected readonly DataContext dataContext;

        public int? CcUstId{
            get
            {
                int? rV = null;
                if (this.UstId > 0)
                {
                    rV = this.UstId;
                }
                return rV;
            }
        }

        public Boolean HasChildren {
            get
            {
                Boolean rV = false;
                try
                {
                    rV = this.dataContext.TemAdres.Where(c => c.UstId == this.Id).Any();
                }
                catch { }
                return rV;
            }
        }

        public string CcGondericiIdAd{ get; set; }
        public string CcAliciIdAd{ get; set; }

        //Constructor
        public DtoTemMesaj(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    }
}


