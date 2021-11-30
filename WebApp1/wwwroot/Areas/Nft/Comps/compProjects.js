
function compProjects(_elm, _opt) {
    var self = {};

    self = $.extend({
        //input1: "" //input param
        onChange: null //dropdown seçim yapıldığında oluşan event
    }, _opt);

    self.$elm = $(_elm);

    function fnHtmlAppend() {
        var temp = `
            <div>
                <input id="ddProjects" class="w-100" />
            </div>
        `;
        self.$elm.append(temp);
    }

    function fnStyleAppend() {
        var style = `
            <style>
                `+ self.$elm.selector + ` div {

                }
            </style>
        `;
        self.$elm.append(style);
    }

    function fnCreateElements() {
        self.ddProjects = self.$elm.find('#ddProjects').kendoDropDownList({
            valuePrimitive: true,
            dataValueField: 'Name',
            dataTextField: 'Name',
            valueTemplate: `
                    <span class="d-block align-middle mr-2 rounded-circle" style="background-image: url('#:data.ImageUrl != undefined ? data.ImageUrl : "" #'); background-size: 100%; width: 48px; height: 48px;">
                    </span>
                    <span class="h3">#:data.Name#</span>
                `,
            template: `
                    <span class="k-state-default d-block align-top rounded-circle mr-2" style="background-image: url('#:data.ImageUrl#'); box-sizing: border-box; background-size: 100%; width: 48px; height: 48px;"></span>
                    <span class="k-state-default d-block align-top" style="box-sizing: border-box;" >
                        <h3>#: data.Name #</h3>
                    </span>
                `,
            change: function (e) {
                var dataItem = this.dataItem(e.item);
                if (dataItem) {
                    if (self.onChange) {
                        var prms = {
                            event: e,
                            name: dataItem.Name,
                            layers: dataItem.Layers
                        };
                        self.onChange(prms);
                    }
                }
            },
            dataBound: function (e) {
                $(e.sender.span).css("height", "60px");
                e.sender.select(0);
                e.sender.trigger("change");
            }
        }).getKendoDropDownList();

        $.ajax({
            url: "/api/Nft/GetProjectList",
            type: "GET", dataType: "json",
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
                    self.ddProjects.setDataSource(result.Data);
                } else {
                    mnNotification.warning(result.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }


    self.prepare = function (_opt) {
        self.opt = $.extend({}, _opt);
        fnHtmlAppend();
        fnStyleAppend();

        //create elements
        fnCreateElements();

    };

    return self;
};
