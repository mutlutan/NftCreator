
function compProjectList(_elm, _opt) {
    var self = {};

    self = $.extend({
        //input1: "" //input param
        onChange: null //dropdown seçim yapıldığında oluşan event
    }, _opt);

    self.$elm = $(_elm);
    self.area = 'Nft';
    self.primaryKey = 'Id';
    self.tableName = 'NftProje';
    self.apiUrlPrefix = '/' + self.area + '/Dwa' + self.tableName;

    function fnHtmlAppend() {
        var temp = `
            <div>
                <div>
                    <a name="btnAddProject" class="btn btn-sm btn-link " type="button">AddProject</a>
                </div>
                <div id="divProjectList" class="border p-1">
                </div>
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

                    //result.Data
                    for (const item of result.Data) {
                        var temp = `
                            <div class="row">
                                <div class="col-md-6">
                                    <img class="rounded-circle" src="` + item.ImageUrl + `" style="width: 48px; height: 48px;">
                                    <span>`+ item.Name + `</span>
                                </div>
                                <div class="col-md-6">
                                    <a name="btnNameEdit" class="btn btn-sm btn-link " type="button">Edit</a>
                                    <a name="btnImageUpload" class="btn btn-sm btn-link " type="button">Upload</a>
                                </div>
                            </div>
                        `;
                        self.$elm.find('#divProjectList').append(temp);
                    }

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
