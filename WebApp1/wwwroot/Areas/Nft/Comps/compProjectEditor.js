
function compProjectEditor(_elm, _opt) {
    var self = {};

    self = $.extend({
        //input1: "" //input param
        onChange: null //dropdown seçim yapıldığında oluşan event
    }, _opt);

    self.$elm = $(_elm);

    function fnHtmlAppend() {
        var temp = `
            <div class="row">
                <div class="col-md-6">
                    <div id="divProject"></div>
                </div>
                <div class="col-md-6">
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

    }

    function fnGetProjectInfo() {
        var _data = {
            projectName: self.projectName
        };

        $.ajax({
            url: "/api/Nft/GetProjectInfo",
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
                    fnShowProjectLayer(result.Data);
                } else {
                    mnNotification.warning(result.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }

    function fnChangeLayerName(oldName, newName) {

        var _data = {
            projectName: self.projectName,
            oldLayerName: oldName,
            newLayerName: newName
        };

        $.ajax({
            url: "/api/Nft/ChangeLayerName",
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
                fnGetProjectInfo();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }

    function fnSetProjectInfo() {
        let _layers = [];
        self.lbSelectLayers.wrapper.find(".itemLayer").each(function () {
            let elmCheckbox = $(this).find("input[type=checkbox]");
            let status = elmCheckbox.is(':checked');
            let layerName = $(this).closest(".itemLayer").find("input[name=Name]").val();
            _layers.push({ "Status": status, "Name": layerName });
        });

        var _data = {
            ProjectName: self.projectName,
            LayerList: _layers
        };

        $.ajax({
            url: "/api/Nft/SetProjectInfo",
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
                fnGetProjectInfo();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("(" + jqXHR.status + ") " + jqXHR.statusText + "\n" + this.url);
            }
        });
    }

    function fnShowProjectLayer(projectInfo) {
        var temp = `
                <div>
                    <div class="d-flex flex-row p-2">
                        <div>
                            <a name="btnImport" class="btn btn-sm btn-link d-none" >Import</a>
                        </div>
                    </div>
                    <div>
                        <select id="selectLayers" class="w-100" style="height:450px" ></select>
                    </div>
                </div>
            `;

        self.$elm.find("#divProject").html(temp);

        //header elements
        self.$elm.find("#divProject").find("[name=btnImport]").click(function (e) {

        });

        //layer
        self.lbSelectLayers = self.$elm.find("#divProject").find("#selectLayers").kendoListBox({
            dataSource: projectInfo.LayerList,
            dataTextField: "Name",
            dataValueField: "Name",
            draggable: {
                hint: function (element) {
                    return element.clone().hide();
                },
                placeholder: function (element) {
                    return element.clone().css({
                        "opacity": 0.5,
                        "border": "1px dashed red"
                    });
                }
                //placeholder: function (element) {
                //    return element.clone().text("drop here");
                //}
            },
            dragend: function (e) {
                fnSetProjectInfo();
            },
            drop: function (e) {
                //cancel istediğinde burdan yaparsın 
                //e.preventDefault();
            },
            template: function (d) {
                var attributes = d.Status == true ? "checked" : "";

                var temp = `
                    <div class="itemLayer" data-layer-name="${d.Name}">
                        <input name="cbStatus" type="checkbox" class="k-checkbox" ${attributes}/>
                        <input name="Name" type="hidden" value="${d.Name}" />
                        <label>${d.Name}</label>
                        <button name="btnRename" class="btn btn-sm btn-link " type="button">Rename</button>
                        <button name="btnDetail" class="btn btn-sm btn-link " type="button">Detail</button>
                    </div>
                `;
                return temp;
            },
            change: function (e) {
                e.sender.clearSelection();
            },
            dataBound: function (e) {
                e.sender.wrapper.find(".k-list .k-item").css("box-shadow", "none");
                e.sender.wrapper.on("click", ".itemLayer [name=cbStatus]", function () {
                    fnSetProjectInfo();
                });
                e.sender.wrapper.on("click", ".itemLayer [name=btnRename]", function (e) {
                    let $itemLayer = $(e.currentTarget).closest(".itemLayer");
                    let oldName = $itemLayer.find("input[name=Name]").val();

                    kendo.prompt("Please, enter a new value:", oldName).then(function (data) {
                        fnChangeLayerName(oldName, data);
                    });

                });
                e.sender.wrapper.on("click", ".itemLayer [name=btnDetail]", function (e) {
                    let layerName = $(e.currentTarget).closest(".itemLayer").find("input[name=Name]").val();
                    fnShowLayerImage(layerName);
                });
            }
        }).getKendoListBox();

    }

    function fnShowLayerImage(_layerName) {
        let $elm = $('<div name="div' + _layerName + '" ></div>');
        let layerEditor1 = compLayerEditor($elm.get(0));
        layerEditor1.prepare({ projectName: self.projectName, layerName: _layerName });

        var popup = $elm.kendoWindow({
            modal: true,
            actions: ["Close"],
            title: _layerName + " details",
            close: function (e) {
                e.sender.destroy();
            }
        }).getKendoWindow();

        popup.center().open();
    }

    self.ShowProjectLayers = function (_projectName) {
        self.projectName = _projectName;
        fnGetProjectInfo();
    };

    self.prepare = function (_opt) {
        self.opt = $.extend({}, _opt);

        fnHtmlAppend();
        fnStyleAppend();

        //create elements
        fnCreateElements();
    };

    return self;
};
