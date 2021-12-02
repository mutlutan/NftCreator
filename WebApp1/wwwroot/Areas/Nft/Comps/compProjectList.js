
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
                    <a name="btnAddProject" class="btn btn-sm btn-link " type="button">Add Project</a>
                </div>

                <div class="py-1"></div>

                <div id="divProjectList" class="">
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

        self.$elm.find("[name=btnAddProject]").click(function (e) {
            kendo.prompt("Please, enter a new value:", "").then(function (data) {
                fnAddProject(data);
            });
        });

        fnGetProjectList();
    }

    function fnGetProjectList() {
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
                    self.$elm.find('#divProjectList').empty();
                    //result.Data
                    for (const project of result.Data) {

                        var tempExportList = "";
                        for (const item of project.ExportList) {
                            var tempExport = `
                                <tr>
                                    <td>
                                        <a href="#/Files?p1=`+ item.DownloadUrl + `" class="btn btn-sm btn-link">` + item.DownloadFileName + `</a>
                                    </td>
                                    <td>
                                        `+ item.PlannedImageQuantity + `
                                    </td>
                                    <td>
                                        `+ item.CreatedImageQuantity + `
                                    </td>
                                    <td>
                                        `+ item.RemainderImageQuantity + `
                                    </td>
                                </tr>
                            `;
                            tempExportList += tempExport;
                        }

                        var temp = `
                            <div class="row border border-secondary m-2 p-1 ">
                                <div class="col-md-12">
                                    <table>
                                        <tr>
                                            <td>
                                                <img class="rounded-circle" src="` + project.ImageUrl + `" onerror="if (this.src != 'img/photo/noimage.png') this.src = 'img/photo/noimage.png';" style="width: 64px;">
                                            </td>
                                            <td class="w-100 pl-2">
                                                <span class="h4 text-truncate d-block">`+ project.Name + `</span>
                                                <a href="#/ProjectEditor?p1=`+ project.Name + `" class="btn btn-sm btn-link pl-0">Edit</a>
                                            </td>
                                            <td>
                                                <a name="btnImport" class="btn btn-sm btn-link" >Import</a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-md-12 p-1">
                                    <table class="table table-sm">
                                        <thead class="thead-light">
                                            <tr>
                                                <th class="font-weight-normal">Downloads</th>
                                                <th class="font-weight-normal">Planned</th>
                                                <th class="font-weight-normal">Created</th>
                                                <th class="font-weight-normal">Remainder</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            `+ tempExportList + `
                                        </tbody>
                                    </table>
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

    function fnAddProject(_projectName) {
        var _data = {
            projectName: _projectName
        };

        $.ajax({
            url: "/api/Nft/AddProject",
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
                    fnGetProjectList();
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
