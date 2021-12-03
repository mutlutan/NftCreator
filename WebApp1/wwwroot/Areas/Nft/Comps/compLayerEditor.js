
function compLayerEditor(_elm, _opt) {
    var self = {};

    self = $.extend({
        //input1: "" //input param
        onChange: null //dropdown seçim yapıldığında oluşan event
    }, _opt);

    self.$elm = $(_elm);

    function fnHtmlAppend() {
        var temp = `
            <div class="row">
                <div class="col">
                    <div style="width:500px;">
                        <label>Total Percentage : %</label>
                        <span id="spanTotalUsagePercentageValue"></span>
                        <span id="spanTotalUsagePercentageMessage"></span>

                        <a name="btnEqualPercentage" class="btn btn-sm btn-link pull-right">Equal %</a>
                    </div>
                </div>
                <div class="col"></div>
            </div>
            <div class="row">
                <div class="col">
                    <div id="divLayer" style="width:500px; height:450px"></div>
                </div>
                <div class="col">
                    <div>
                        <img name="imgPreview" class="img-thumbnail" style="width:150px; height:150px;" />
                    </div>
                    <div>
                        <label name="labelImageName"></label>
                    </div>
                    <div>
                        <label>Width : </label> <label name="labelImageWidth"></label>
                    </div>
                    <div>
                        <label>Height : </label> <label name="labelImageHeight"></label>
                    </div>
                </div>
            </div>
        `;
        self.$elm.append(temp);
    }

    function fnStyleAppend() {
        var style = `
            <style>
                ${self.$elm.selector} div {

                }
            </style>
        `;
        self.$elm.append(style);
    }

    function fnCreateElements() {
        self.$elm.find("[name=btnEqualPercentage]").click(function (e) {

            let itemLength = self.lbSelectImages.wrapper.find(".itemImage").length;
            let percentageValue = parseInt(100 / itemLength);
            let differencePercentage = 100 - (percentageValue * itemLength);

            self.lbSelectImages.wrapper.find(".itemImage").each(function () {
                $(this).find("input[name=UsagePercentage]").val(percentageValue);
            });
            
            $(self.lbSelectImages.wrapper.find(".itemImage").get(0)).find("input[name=UsagePercentage]").val(percentageValue + differencePercentage);

            fnSetLayerInfo();
        });
    }

    function fnGetLayerInfo() {

        var _data = {
            projectName: self.opt.projectName,
            layerName: self.opt.layerName,
        };

        $.ajax({
            url: "/api/Nft/GetLayerInfo",
            data: JSON.stringify(_data),
            type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
            beforeSend: function (jqXHR, settings) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, true); //progress On
                });
            },
            complete: function (jqXHR, textStatus) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, false); //progress Off
                });
            },
            success: function (result, textStatus, jqXHR) {
                if (result.Success == true) {
                    fnShowLayerImage(result.Data);
                } else {
                    mnNotification.warning(result.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });

    }

    function fnShowLayerImage(layerInfo) {
        var temp = `
                <div name="layerItem" >
                    <select id="selectImages" class="w-100" style="height:433px" ></select>
                </div>
            `;

        self.$elm.find("#divLayer").html(temp);

        //Image
        self.lbSelectImages = self.$elm.find("#divLayer").find("#selectImages").kendoListBox({
            dataSource: layerInfo.ImageList,
            dataTextField: "Name",
            dataValueField: "Name",
            template: function (d) {
                var attributes = d.Status == true ? "checked" : "";

                var temp = `
                    <div class="itemImage w-100" data-image-name="${d.Name}" data-image-url="${encodeURIComponent(d.ImageUrl.toString())}">
                        <input name="Name" type="hidden" value="${d.Name}">
                        <input name="ImageWidth" type="hidden" value="${d.ImageWidth}">
                        <input name="ImageHeight" type="hidden" value="${d.ImageHeight}">
                        <input name="cbStatus" type="checkbox" class="k-checkbox" ${attributes} />
                        <label>${d.Name}</label>
                        <label class="pl-2">%</label>
                        <input class="form-control d-inline form-control-sm" name="UsagePercentage" type="number" value="${d.UsagePercentage}" style="width:60px;" min="0" max="100">
                        <button name="btnRename" class="btn btn-sm btn-link " type="button">Rename</button>
                    </div>
                `;
                return temp;
            },
            change: function (e) {
                var $item = e.sender.select().find(".itemImage");
                let imageUrl = $item.attr("data-image-url");
                self.$elm.find("[name=imgPreview]").attr("src", decodeURIComponent(imageUrl));
                self.$elm.find("[name=labelImageName]").html($item.find("input[name=Name]").val());
                self.$elm.find("[name=labelImageWidth]").html($item.find("input[name=ImageWidth]").val());
                self.$elm.find("[name=labelImageHeight]").html($item.find("input[name=ImageHeight]").val());
            },
            dataBound: function (e) {
                e.sender.wrapper.on("click", ".itemImage [name=cbStatus]", function () {
                    fnSetLayerInfo();
                });

                e.sender.wrapper.on("change", ".itemImage [name=UsagePercentage]", function () {
                    fnSetLayerInfo();
                });

                e.sender.wrapper.on("click", ".itemImage [name=btnRename]", function (e) {
                    let $itemLayer = $(e.currentTarget).closest(".itemImage");
                    let oldName = $itemLayer.find("input[name=Name]").val();

                    kendo.prompt("Please, enter a new value:", oldName).then(function (data) {
                        fnChangeImageName(oldName, data);
                    });

                });

                e.sender.select(e.sender.items().first());
            }
        }).getKendoListBox();

        //yüzde toplamı göster
        fnShowUsagePercentage();
    }

    function fnChangeImageName(_oldName, _newName) {

        var _data = {
            projectName: self.opt.projectName,
            layerName: self.opt.layerName,
            oldImageName: _oldName,
            newImageName: _newName
        };

        $.ajax({
            url: "/api/Nft/ChangeImageName",
            data: JSON.stringify(_data),
            type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
            beforeSend: function (jqXHR, settings) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, true); //progress On
                });
            },
            complete: function (jqXHR, textStatus) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, false); //progress Off
                });
            },
            success: function (result, textStatus, jqXHR) {
                if (result.Success != true) {
                    alert(result.Message);
                }
                fnGetLayerInfo();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }

    function fnSetLayerInfo() {
        let _images = [];
        self.lbSelectImages.wrapper.find(".itemImage").each(function () {
            let elmCheckbox = $(this).find("input[type=checkbox]");
            let status = elmCheckbox.is(':checked');
            let imageName = elmCheckbox.closest(".itemImage").find("input[name=Name]").val();
            let imageUrl = elmCheckbox.closest(".itemImage").attr("data-image-url");
            let imageWidth = elmCheckbox.closest(".itemImage").find("input[name=ImageWidth]").val();
            let imageHeight = elmCheckbox.closest(".itemImage").find("input[name=ImageHeight]").val();
            let usagePercentage = $(this).find("input[name=UsagePercentage]").val();

            _images.push(
                {
                    "LayerName": self.opt.layerName,
                    "Status": status,
                    "Name": imageName,
                    "UsagePercentage": usagePercentage,
                    "ImageUrl": imageUrl,
                    "ImageWidth": imageWidth,
                    "ImageHeight": imageHeight
                }
            );
        });

        var _data = {
            ProjectName: self.opt.projectName,
            LayerName: self.opt.layerName,
            ImageList: _images
        };

        $.ajax({
            url: "/api/Nft/SetLayerInfo",
            data: JSON.stringify(_data),
            type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
            beforeSend: function (jqXHR, settings) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, true); //progress On
                });
            },
            complete: function (jqXHR, textStatus) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, false); //progress Off
                });
            },
            success: function (result, textStatus, jqXHR) {
                if (result.Success != true) {
                    alert(result.Message);
                    fnGetLayerInfo();
                }
                fnShowUsagePercentage();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }

    function fnSumUsagePercentage() {
        var totalUsagePercentage = 0;
        self.lbSelectImages.wrapper.find(".itemImage").each(function () {
            if ($(this).find("input[type=checkbox]").is(':checked')) {
                let elmUsagePercentage = $(this).find("[name=UsagePercentage]");
                totalUsagePercentage += parseFloat(elmUsagePercentage.val());
            }
        });

        return totalUsagePercentage;
    }

    function fnShowUsagePercentage() {
        var total = fnSumUsagePercentage();
        var message = "";
        self.$elm.find("#spanTotalUsagePercentageValue").html(total);
        if (total == 100) {
            message = '<span class="text-success"> Ok!<span>';
        } else {
            message = '<span class="text-danger"> Not Ok!<span>';
        }
        self.$elm.find("#spanTotalUsagePercentageMessage").html(message);
    }

    self.prepare = function (_opt) {
        self.opt = $.extend({}, _opt);

        fnHtmlAppend();
        fnStyleAppend();

        fnGetLayerInfo();

        //create elements
        fnCreateElements();
    };

    return self;
};
