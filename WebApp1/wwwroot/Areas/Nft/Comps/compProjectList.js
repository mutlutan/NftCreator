
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
            <div >
                <div class="row">
                    <div class="col-md-6">
                        <a name="btnAddProject" class="btn btn-sm btn-link">Add Project</a>

                        <a name="btnRefreshProjects" class="btn btn-sm btn-link pull-right" title="refresh"> <i class="fa fa-refresh"></i> </a>
                    </div>
                    <div class="col-md-6">
                        
                    </div>
                </div>

                <div class="py-1"></div>

                <div class="row">
                    <div class="col-md-6">
                        <div id="divProjectList"></div>
                    </div>
                    <div class="col-md-6">
                        <div id="divProjectPreview" class="row"></div>
                    </div>
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

        self.$elm.find("[name=btnRefreshProjects]").click(function (e) {
            self.refresh();
        });

        //imgProjectIcon
        self.$elm.find("#divProjectList").on("click", "[name=imgProjectIcon]", function (e) {
            var $elm = $(e.currentTarget).closest("[name=projectItem]");
            var projectName = $elm.attr("data-project-name");
            var img = $elm.find("[name=imgProjectIcon]");

            var _data = {
                projectName: projectName
            };

            $.ajax({
                url: "/api/Nft/SetProjectIcon",
                data: JSON.stringify(_data),
                type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
                beforeSend: function (jqXHR, settings) {
                    kendo.ui.progress(self.$elm, true); //progress On
                },
                success: function (result, textStatus, jqXHR) {
                    if (result.Success == true) {
                        let imgUrl = $elm.find("[name=imgProjectIcon]").attr("data-image-url");
                        $elm.find("[name=imgProjectIcon]").attr("src", imgUrl +"?"+ new Date());
                    } else {
                        alert(result.Message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
                },
                complete: function (jqXHR, textStatus) {
                    setTimeout(function () {
                        kendo.ui.progress(self.$elm, false); //progress Off
                    });
                }
            });                        
        });

        self.$elm.find("#divProjectList").on("click", "[name=btnPreview]", function (e) {
            var $elm = $(e.currentTarget).closest("[name=projectItem]");
            var projectName = $elm.attr("data-project-name");
            var quantity = $elm.find("[name=quantity]").val();
            if (quantity > 0 && quantity <= 1000) {
                fnPreviewImage(projectName, quantity);
            } else {
                alert("You can create a maximum of 1000 images while previewing.");
            }
        });

        self.$elm.find("#divProjectList").on("click", "[name=btnGenerate]", function (e) {
            var $elm = $(e.currentTarget).closest("[name=projectItem]");
            var projectName = $elm.attr("data-project-name");
            var quantity = $elm.find("[name=quantity]").val();
            if (quantity > 0 && quantity <= 10000) {
                fnGenerateImages(projectName, quantity);
            } else {
                alert("You can create a maximum of 10k images while previewing.");
            }
        });

        //btnDelete
        self.$elm.find("#divProjectList").on("click", "[name=btnDelete]", function (e) {
            var $elm = $(e.currentTarget).closest("[name=projectItem]");
            var projectName = $elm.attr("data-project-name");
            var directoryName = $(e.currentTarget).attr("data-directory-name");

            kendo.confirm("Do you want to delete the export directory?").then(function () {
                fnDeleteExportDirectory(projectName, directoryName);
            });
        });
    }

    function fnRenderProjectList(_data) {
        self.$elm.find('#divProjectList').empty();
        for (const project of _data) {
            var tempExportList = "";
            for (const item of project.ExportList) {

                let visibleClass = item.DownloadUrl == "" ? "d-none" : "";

                var tempExport = `
                                <tr>
                                    <td>
                                        ` + item.DirectoryName + `
                                    </td>
                                    <td>
                                        `+ item.PlannedImageQuantity + `
                                    </td>
                                    <td>
                                        `+ item.CreatedImageQuantity + `
                                    </td>
                                    <td>
                                        <a href="#/Files?p1=`+ item.DownloadUrl + `" class="btn btn-sm btn-link ` + visibleClass + `" title="Download"> <i class="fa fa-download fa-fw"></i> </a>
                                    </td>
                                    <td>
                                        <a name="btnDelete" data-directory-name="` + item.DirectoryName + `" class="btn btn-sm btn-link" title="Delete"> <i class="fa fa-trash-o fa-fw"></i> </a>
                                    </td>
                                </tr>
                            `;
                tempExportList += tempExport;
            }

            var temp = `
                            <div name="projectItem" class="row border border-secondary mb-3 p-1 " data-project-name="`+ project.Name + `">
                                <div class="col-md-12">
                                    <table>
                                        <tr>
                                            <td>
                                                <img name="imgProjectIcon" class="" src="` + project.ImageUrl + "?" + new Date() + `" data-image-url="` + project.ImageUrl + `" onerror="if (this.src != 'img/photo/noimage.png') this.src = 'img/photo/noimage.png';" style="width: 90px; cursor: pointer;">
                                            </td>
                                            <td class="w-100 pl-2">
                                                <span class="h4 text-truncate d-block" style="max-width:200px;" title="`+ project.Name + `">`+ project.Name + `</span>
                                                <a href="#/ProjectEditor?p1=`+ project.Name + `" class="btn btn-sm btn-link pl-0"> Project Detail</a>
                                            </td>
                                            <td>
                                                <input name="quantity" class="form-control form-control-sm" type="text" placeholder="Quantity..." style="width:90px;" />
                                                <a name="btnPreview" class="btn btn-sm btn-link" title="Preview Images">Preview</a>
                                                <a name="btnGenerate" class="btn btn-sm btn-link" title="Generate Images">Generate</a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-md-12">
                                    <table name="tableExport" class="table table-sm">
                                        <thead class="thead-light">
                                            <tr>
                                                <th class="font-weight-normal">Export Name</th>
                                                <th class="font-weight-normal">Planned</th>
                                                <th class="font-weight-normal">Created</th>
                                                <th class="font-weight-normal"></th>
                                                <th class="font-weight-normal"></th>
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

            //kendo numeric edit
            //self.$elm.find("#divProjectList").find("[name=quantity]").kendoNumericTextBox({
            //    format: "n0",
            //    value: 1,
            //    min: 1,
            //    max: 10000
            //});
        }
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
                    fnRenderProjectList(result.Data)
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

    function fnPreviewImage(projectName, quantity) {

        var _data = {
            projectName: projectName,
            quantity: quantity
        };

        $.ajax({
            url: "/api/Nft/PreviewGenerateImages",
            data: JSON.stringify(_data),
            type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
            beforeSend: function (jqXHR, settings) {
                kendo.ui.progress(self.$elm, true); //progress On
            },
            success: function (result, textStatus, jqXHR) {
                if (result.Success == true) {
                    var imagesContainer = self.$elm.find("#divProjectPreview");
                    imagesContainer.empty();

                    for (const imgsrc of result.Data.Images) {
                        var temp = `
                            <div class="col-md-6 p-2">
                                <img src="`+ imgsrc + `" style="width:250px;">
                            <div>
                        `;
                        imagesContainer.append(temp);
                    }
                } else {
                    alert(result.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            },
            complete: function (jqXHR, textStatus) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, false); //progress Off
                });
            }
        });
    }

    function fnGenerateImages(projectName, quantity) {
        var _data = {
            projectName: projectName,
            quantity: quantity
        };

        $.ajax({
            url: "/api/Nft/StartGenerateImages",
            data: JSON.stringify(_data),
            type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
            beforeSend: function (jqXHR, settings) {
                kendo.ui.progress(self.$elm, true); //progress On
            },
            success: function (result, textStatus, jqXHR) {
                if (result.Success == true) {
                    kendo.alert(result.Message);
                } else {
                    kendo.alert(result.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            },
            complete: function (jqXHR, textStatus) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, false); //progress Off
                });
            }
        });
    }

    function fnDeleteExportDirectory(projectName, directoryName) {
        var _data = {
            projectName: projectName,
            directoryName: directoryName
        };

        $.ajax({
            url: "/api/Nft/DeleteExportDirectory",
            data: JSON.stringify(_data),
            type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
            beforeSend: function (jqXHR, settings) {
                kendo.ui.progress(self.$elm, true); //progress On
            },
            success: function (result, textStatus, jqXHR) {
                if (result.Success == true) {
                    self.refresh();
                } else {
                    alert(result.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            },
            complete: function (jqXHR, textStatus) {
                setTimeout(function () {
                    kendo.ui.progress(self.$elm, false); //progress Off
                });
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

    self.refresh = function () {
        fnGetProjectList();
    }

    return self;
};
