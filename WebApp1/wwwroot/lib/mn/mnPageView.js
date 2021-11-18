
//mnPageView 
window.mnPageView = function () {
    var self = {};

    self.Container = "#mnPageViewContainer";

    self.dashboardShow = function () {
        //admin, personel
        if ('11,21'.indexOf(mnUser.Info.nYetkiGrup) >= 0) {
            mnPageView.create({
                areaName: '_',
                viewFileName: 'viewDashboard',
                viewName: 'viewDashboard',
                header: false,
                onShow: function (e) {
                    e.beforeShow();
                }
            });
        } else {
            //öğretmen
            mnPageView.create({
                areaName: 'Rob',
                viewFileName: 'viewOgretmenAnasayfa',
                viewName: 'viewOgretmenAnasayfa',
                header: false,
                onShow: function (e) {
                    e.beforeShow();
                }
            });
        }

    };

    function fGetTemp(opt, _viewContent) {
        var temp = '';

        if (opt.viewName == "viewDashboard") {
            temp = ''
                + '<div id="' + opt.viewName + '" class="mnPageView" >'
                + '    <div class="mnDashboardViewContent" style="padding:0px 15px; " >'
                + '      ' + _viewContent
                + '    </div>'
                + '</div>';

        } else {
            temp = ''
                + '<div id="' + opt.viewName + '" class="mnPageView" >'
                + '    <div class="mnPageViewHeader mn-default-border-color mn-default-bg-color mn-default-text-color" >'
                + '        <span class="text-nowrap" style="font-size:1.2em;" >'
                + '            <span id="mainTitle" data-langkey-text=""></span>'
                + '            <small id="subTitle" data-langkey-text="" style="padding-left:15px; opacity:0.5; font-style:italic; font-size:0.8em;" ></small>'
                + '        </span>'
                + ''
                + '        <button id="btnGeri" type="button" class="btn btn-link float-right mn-opacity-075 mn-opacity-hover-1 mn-default-text-color" data-langkey-title="xLng.Kapat" style="padding: 0px;" > <i class="k-icon k-i-close" style="font-size:1.3em;"></i> </button>'
                + ''
                + '        <hr class="mnHeaderLine">'
                + ''
                + '    </div>'
                + '    <div class="mnPageViewContent mn-default-border-color" >'
                + '      ' + _viewContent
                + '    </div>'
                + '</div>';
        }

        return temp;
    }

    self.create = function (options) {
        var opt = $.extend({
            areaName: "",
            viewFileName: "", //Bu kısma html ve js dosyasının adını yazarsın
            viewName: "", // bu kısmada yeni oluşacak name i yazarsan çoğaltarak kullanabilirsin
            ownerViewName: "",
            title: "", subTitle: "",
            header: true,
            actions: ["backward"]
        }, options);

        if (fFindView(opt.viewName).length > 0) {
            fShow(opt);
        } else {
            fCreate(opt);
        }
    };

    function fFindView(selector) {
        selector = ".mnPageView#" + selector;
        return $(self.Container).find(selector);
    }

    function fCreate(opt) {
        var htmlUrl = "/Areas" + "/" + opt.areaName + "/Views/" + opt.viewFileName + ".html?" + mnApp.version;
        $.ajax({
            url: htmlUrl,
            type: "GET",
            beforeSend: function (xhr, settings) {
                //kendo.ui.progress($(self.Container), true); //progress On // datasourceların read fonksiyonlarının çağırdığı progres ile çakışıyor, başka bir progres türü kullanıp yapabilirsin ama kendonunkini kullanma
            },
            complete: function (xhr, status) {
                //kendo.ui.progress($(self.Container), false); //progress Off
            },
            success: function (resultViewContent) {

                resultViewContent = mnApi.ReplaceAll(resultViewContent, opt.viewFileName, opt.viewName);

                if (fFindView(opt.viewName).length > 0) {
                    fFindView(opt.viewName).remove();
                }

                $(fGetTemp(opt, resultViewContent)).appendTo($(self.Container));

                var module = eval(opt.viewName); // burada önceden yüklenmiş olan js dosyası html e etki ediyor

                try {
                    module.prepare();
                }
                catch (err) {
                    console.log("mnPageView hata (" + opt.viewName + ") : ", err);
                }
                finally {
                    fShow(opt);
                }
            },
            error: function (xhr, status) {
                alert(xhr.status + " " + xhr.statusText + " " + opt.viewName);
            }
        });
    }

    function fShow(opt) {
        var module = eval(opt.viewName);

        //btnGeri event
        $(self.Container).find("#" + opt.viewName + ".mnPageView .mnPageViewHeader #btnGeri").off('click');
        $(self.Container).find("#" + opt.viewName + ".mnPageView .mnPageViewHeader #btnGeri").click(function () {
            if (opt.onClose !== undefined) {
                opt.onClose(module);
            }

            $(self.Container).find(".mnPageView:visible").hide();

            // ownerViewName değişkeninin generate den kaldırabilirsin.......
            if (module.opt.ownerViewName !== null && module.opt.ownerViewName !== undefined && module.opt.ownerViewName.length > 0) {
                $(self.Container).find("#" + module.opt.ownerViewName + ".mnPageView").show();
            } else {
                history.back();
            }
        });

        //açık olan formlar kapatılıyor
        $(self.Container).find(".mnPageView:visible").hide();
        var $pageView = $(self.Container).find("#" + opt.viewName + ".mnPageView");

        if (opt.header) {
            $pageView.find(".mnPageViewHeader").show();
        } else {
            $pageView.find(".mnPageViewHeader").hide();
        }

        if (opt.title.length === 0) {
            opt.title = module.title;
        }

        $pageView.find(".mnPageViewHeader #mainTitle").attr("data-langkey-text", opt.title);
        $pageView.find(".mnPageViewHeader #subTitle").attr("data-langkey-text", opt.subTitle);

        if (opt.actions.indexOf("backward") >= 0) {
            $pageView.find(".mnPageViewHeader #btnGeri").show();
        } else {
            $pageView.find(".mnPageViewHeader #btnGeri").hide();
        }

        if (opt.onShow !== undefined) {
            opt.onShow(module);
        }

        // Language
        mnLang.TranslateWithSelector($pageView);
        $pageView.show();
    }

    return self;
}();

