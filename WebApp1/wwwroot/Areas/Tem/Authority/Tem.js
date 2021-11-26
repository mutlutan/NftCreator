﻿
window.TemAuthority =
    {
        id: "Tem.", text: mnLang.f("xLng.Authority.Tem"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-th fa-fw", expanded: true, prefix: false, menu: true, yetkiGrups: "11,21,31,41", items: [
            {
                id: "Tem.Tanim.", text: mnLang.f("xLng.Authority.Tem.Tanim"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-newspaper-o fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21,31,41", items: [
                    {
                        id: "Tem.Tanim.TemKullanici.", text: mnLang.f("xTem.TemKullanici.Title"), hint: "", area: "Tem", rout: "TemKullanici", params: "", showType: "Page", header: true, viewName: "TemKullaniciForGrid", cssClass: "fa fa-user-o fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                            { id: "Tem.Tanim.TemKullanici.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemKullanici.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemKullanici.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemKullanici.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemKullanici.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemKullanici.A_ResetPassword.", text: mnLang.f("xLng.Authority.SifreSifirlayabilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-refresh fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            {
                                id: "Tem.Tanim.TemKullaniciLisans.", text: mnLang.f("xTem.TemKullaniciLisans.Title"), hint: "", area: "Tem", rout: "TemKullaniciLisans", params: "", showType: "Page", header: true, viewName: "TemKullaniciLisansForGrid", cssClass: "fa fa-id-card-o fa-fw", expanded: false, prefix: true, menu: false, yetkiGrups: "11,21", items: [
                                    { id: "Tem.Tanim.TemKullaniciLisans.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemKullaniciLisans.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemKullaniciLisans.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemKullaniciLisans.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemKullaniciLisans.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                                ]
                            }
                        ]
                    },
                    {
                        id: "Tem.Tanim.TemParametre.", text: mnLang.f("xTem.TemParametre.Title"), hint: "", area: "Tem", rout: "TemParametre", params: "Id=1", showType: "Form", header: true, viewName: "TemParametreForForm", cssClass: "fa fa-wrench fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                            { id: "Tem.Tanim.TemParametre.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemParametre.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                        ]
                    },
                    {
                        id: "Tem.Tanim.TemMailAntet.", text: mnLang.f("xTem.TemMailAntet.Title"), hint: "", area: "Tem", rout: "TemMailAntet", params: "", showType: "Page", header: true, viewName: "TemMailAntetForGrid", cssClass: "fa fa-id-card-o fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                            { id: "Tem.Tanim.TemMailAntet.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemMailAntet.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemMailAntet.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemMailAntet.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemMailAntet.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                        ]
                    },
                    {
                        id: "Tem.Tanim.TemMailSablon.", text: mnLang.f("xTem.TemMailSablon.Title"), hint: "", area: "Tem", rout: "TemMailSablon", params: "", showType: "Page", header: true, viewName: "TemMailSablonForGrid", cssClass: "fa fa-wrench fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                            { id: "Tem.Tanim.TemMailSablon.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemMailSablon.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemMailSablon.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                        ]
                    },
                    {
                        id: "Tem.Tanim.TemRol.", text: mnLang.f("xTem.TemRol.Title"), hint: "", area: "Tem", rout: "TemRol", params: "", showType: "Page", header: true, viewName: "TemRolForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                            { id: "Tem.Tanim.TemRol.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemRol.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemRol.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemRol.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemRol.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                        ]
                    },
                    //{
                    //    id: "Tem.Tanim.TemDepartman.", text: mnLang.f("xTem.TemDepartman.Title"), hint: "", area: "Tem", rout: "TemDepartman", params: "", showType: "Page", header: true, viewName: "TemDepartmanForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                    //        { id: "Tem.Tanim.TemDepartman.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemDepartman.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemDepartman.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemDepartman.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemDepartman.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //},
                    //{
                    //    id: "Tem.Tanim.TemGorev.", text: mnLang.f("xTem.TemGorev.Title"), hint: "", area: "Tem", rout: "TemGorev", params: "", showType: "Page", header: true, viewName: "TemGorevForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                    //        { id: "Tem.Tanim.TemGorev.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemGorev.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemGorev.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemGorev.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemGorev.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //},
                    //{
                    //    id: "Tem.Tanim.TemMesaj.", text: mnLang.f("xTem.TemMesaj.Title"), hint: "", area: "Tem", rout: "TemMesaj", params: "", showType: "Page", header: true, viewName: "TemMesajForTreeList", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                    //        { id: "Tem.Tanim.TemMesaj.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemMesaj.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemMesaj.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemMesaj.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemMesaj.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //},
                    {
                        id: "Tem.Tanim.TemUlke.", text: mnLang.f("xTem.TemUlke.Title"), hint: "", area: "Tem", rout: "TemUlke", params: "", showType: "Page", header: true, viewName: "TemUlkeForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "", items: [
                            { id: "Tem.Tanim.TemUlke.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemUlke.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemUlke.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemUlke.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.TemUlke.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            {
                                id: "Tem.Tanim.TemSehir.", text: mnLang.f("xTem.TemSehir.Title"), hint: "", area: "Tem", rout: "TemSehir", params: "", showType: "Page", header: true, viewName: "TemSehirForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: false, yetkiGrups: "11,21", items: [
                                    { id: "Tem.Tanim.TemSehir.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemSehir.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemSehir.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemSehir.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    { id: "Tem.Tanim.TemSehir.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                    {
                                        id: "Tem.Tanim.TemIlce.", text: mnLang.f("xTem.TemIlce.Title"), hint: "", area: "Tem", rout: "TemIlce", params: "", showType: "Page", header: true, viewName: "TemIlceForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: false, yetkiGrups: "11,21", items: [
                                            { id: "Tem.Tanim.TemIlce.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                            { id: "Tem.Tanim.TemIlce.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                            { id: "Tem.Tanim.TemIlce.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                            { id: "Tem.Tanim.TemIlce.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                                            { id: "Tem.Tanim.TemIlce.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                                        ]
                                    }
                                ]
                            }
                        ]
                    },
                    //{
                    //    id: "Tem.Tanim.TemAdres.", text: mnLang.f("xTem.TemAdres.Title"), hint: "", area: "Tem", rout: "TemAdres", params: "", showType: "Page", header: true, viewName: "TemAdresForTreeList", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                    //        { id: "Tem.Tanim.TemAdres.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemAdres.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemAdres.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemAdres.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemAdres.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //},
                    //{
                    //    id: "Tem.Tanim.TemParaBirim.", text: mnLang.f("xTem.TemParaBirim.Title"), hint: "", area: "Tem", rout: "TemParaBirim", params: "", showType: "Page", header: true, viewName: "TemParaBirimForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                    //        { id: "Tem.Tanim.TemParaBirim.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemParaBirim.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemParaBirim.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemParaBirim.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Tanim.TemParaBirim.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //},
                    {
                        id: "Tem.Tanim.Gorseller.", text: mnLang.f("xLng.viewGorseller.Title"), hint: "", area: "_", rout: "Gorseller", params: "", showType: "Page", header: true, viewName: "viewGorseller", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                            { id: "Tem.Tanim.Gorseller.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.Gorseller.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Tanim.Gorseller.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                        ]
                    }

                ]
            },
            {
                id: "Tem.Logs.", text: mnLang.f("xLng.Authority.Tem.Logs"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-eye fa-fw", expanded: false, prefix: false, menu: true, yetkiGrups: "11,21", items: [
                    //{
                    //    id: "Tem.Logs.TemRequestLog.", text: mnLang.f("xTem.TemRequestLog.Title"), hint: "", area: "Tem", rout: "TemRequestLog", params: "", showType: "Page", header: true, viewName: "TemRequestLogForGrid", cssClass: "fa fa-list-alt fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "", items: [
                    //        { id: "Tem.Logs.TemRequestLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemRequestLog.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemRequestLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //},
                    {
                        id: "Tem.Logs.TemOturumLog.", text: mnLang.f("xTem.TemOturumLog.Title"), hint: "", area: "Tem", rout: "TemOturumLog", params: "", showType: "Page", header: true, viewName: "TemOturumLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                            { id: "Tem.Logs.TemOturumLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Logs.TemOturumLog.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Logs.TemOturumLog.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Logs.TemOturumLog.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                            { id: "Tem.Logs.TemOturumLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                        ]
                    }
                    //{
                    //    id: "Tem.Logs.TemAuditLog.", text: mnLang.f("xTem.TemAuditLog.Title"), hint: "", area: "Tem", rout: "TemAuditLog", params: "", showType: "Page", header: true, viewName: "TemAuditLogForGrid", cssClass: "fa fa-tint fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                    //        { id: "Tem.Logs.TemAuditLog.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemAuditLog.D_C.", text: mnLang.f("xLng.Authority.Ekleyebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-plus fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemAuditLog.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemAuditLog.D_D.", text: mnLang.f("xLng.Authority.Silebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-trash-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemAuditLog.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //},
                    //{
                    //    id: "Tem.Logs.TemMailHareket.", text: mnLang.f("xTem.TemMailHareket.Title"), hint: "", area: "Tem", rout: "TemMailHareket", params: "", showType: "Page", header: true, viewName: "TemMailHareketForGrid", cssClass: "fa fa-envelope-o fa-fw", expanded: false, prefix: true, menu: true, yetkiGrups: "11,21", items: [
                    //        { id: "Tem.Logs.TemMailHareket.D_R.", text: mnLang.f("xLng.Authority.Gorebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-search fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemMailHareket.D_U.", text: mnLang.f("xLng.Authority.Duzeltebilir"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-pencil fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] },
                    //        { id: "Tem.Logs.TemMailHareket.D_E.", text: mnLang.f("xLng.Authority.SaveAsExcel"), hint: "", area: "Tem", rout: "", params: "", showType: "Page", header: false, viewName: "", cssClass: "fa fa-file-excel-o fa-fw", expanded: false, prefix: false, menu: false, yetkiGrups: "11,21", items: [] }
                    //    ]
                    //}
                ]
            }
        ]
    };
