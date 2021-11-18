using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApp1.Codes
{

    #region Cache

    public class MyCache
    {
        //Tablolar
        public static List<Areas.Tem.Dto.DtoTemKullanici> TemKullaniciList { get; set; } = null;
        public static List<Areas.Tem.Dto.DtoTemRol> TemRolList { get; set; } = null;
  

        public static void TemKullaniciListLoad(Models._Rep _rep)
        {
            MyCache.TemKullaniciList = _rep.Areas_Tem_RepTemKullanici.Get()
                .Where(c => c.Id > 0)
                .ToList();
        }
        public static void TemRolListLoad(Models._Rep _rep)
        {
            MyCache.TemRolList = _rep.Areas_Tem_RepTemRol.Get()
                .Where(c => c.Id > 0)
                .ToList();
        }


        public static void RefreshAll(Models._Rep _rep)
        {
            if (MyCache.TemKullaniciList == null)
            {
                MyCache.TemKullaniciListLoad(_rep);
            }
            if (MyCache.TemRolList == null)
            {
                MyCache.TemRolListLoad(_rep);
            }

        }

    }

    #endregion
}
