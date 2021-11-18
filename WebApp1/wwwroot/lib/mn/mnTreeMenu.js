/*
 * Tree Menu widget
 * Version: 0.1
 * Mutlu MUTLUTAN
*/

(function ($) {

    $.fn.mnTreeMenu = function (_opt) {

        var $this = $(this);
        var self = $this;
        self.opt = $.extend({
            dataSource: [],
            itemColor: "ghostwhite",
            itemBackgroundColor: "transparent",
            itemHoverColor: "#5684c8",
            itemHoverBackgroundColor: "white",
            itemActiveColor: "ghostwhite"
        }, _opt);

        function fAddStyle() {
            var style = '                                                                                     \
                <style>                                                                                       \
                    '+ $this.selector + '.mnTreeMenu {                                                        \
                        color: '+ self.opt.itemColor + ';                                                     \
                    }                                                                                         \
                                                                                                              \
                        '+ $this.selector + '.mnTreeMenu ul {                                                 \
                            padding: 0px;                                                                     \
                            margin: 0px;                                                                      \
                        }                                                                                     \
                                                                                                              \
                            '+ $this.selector + '.mnTreeMenu ul li {                                          \
                                padding-top: 6px;                                                             \
                                padding-bottom: 6px;                                                          \
                                padding-right: 6px;                                                           \
                            }                                                                                 \
                                                                                                              \
                                '+ $this.selector + '.mnTreeMenu ul li ul li {                                \
                                    padding-top: 3px;                                                         \
                                    padding-bottom: 3px;                                                      \
                                }                                                                             \
                                                                                                              \
                                '+ $this.selector + '.mnTreeMenu ul li ul li ul li {                          \
                                    padding-top: 1px;                                                         \
                                    padding-bottom: 1px;                                                      \
                                }                                                                             \
                                                                                                              \
                                '+ $this.selector + '.mnTreeMenu ul li a {                                    \
                                    text-decoration: none;                                                    \
                                    display: block;                                                           \
                                    padding-left: 0px;                                                      \
                                    padding-right: 10px;                                                      \
                                    margin: 0px;                                                              \
                                    font-weight: normal;                                                      \
                                }                                                                             \
                                                                                                              \
                                    '+ $this.selector + '.mnTreeMenu ul li a:hover {                          \
                                        cursor: pointer;                                                      \
                                        color: '+ self.opt.itemHoverColor + ' !important;                     \
                                        background-color: '+ self.opt.itemHoverBackgroundColor + ';           \
                                    }                                                                         \
                                                                                                              \
                                                                                                              \
                                    '+ $this.selector + '.mnTreeMenu ul li a.active {                         \
                                        color: '+ self.opt.itemActiveColor + ' !important;                    \
                                    }                                                                         \
                                                                                                              \
                                '+ $this.selector + '.mnTreeMenu ul li ul li a {                              \
                                    padding-left: 20px;                                                      \
                                    font-weight: 300;                                                         \
                                }                                                                             \
                                                                                                              \
                                '+ $this.selector + '.mnTreeMenu ul li ul li ul li a{                         \
                                    padding-left: 40px;                                                      \
                                    font-weight: 200;                                                         \
                                }                                                                             \
                                                                                                              \
                        '+ $this.selector + '.mnTreeMenu i.mnCollapseButton {                                 \
                            margin-top: 3px;                                                                  \
                        }                                                                                     \
                                                                                                              \
                        '+ $this.selector + '.mnTreeMenu [aria-expanded="false"] i.mnCollapseButton {         \
                            transition: 0.5s;                                                                 \
                        }                                                                                     \
                                                                                                              \
                        '+ $this.selector + '.mnTreeMenu [aria-expanded="true"] i.mnCollapseButton {          \
                            transform: rotate(-90deg);                                                        \
                            transition: 0.5s;                                                                 \
                        }                                                                                     \
                </style>';

            $(document.head).append(style);
        }

        function fGetMenuHtml() {
            var treeMenuUniqeName = "treeMenu_" + new Date().getTime();
            var menuHtmls = $('<ul id="' + treeMenuUniqeName + '" class="accordion" ></ul>');

            for (var i = 0; i < self.opt.dataSource.length; i++) {

                var level_0_name = mnApi.ReplaceAll(self.opt.dataSource[i].id, ".", "-");
                var level_0_item_length = (self.opt.dataSource[i].items === undefined ? 0 : self.opt.dataSource[i].items.length);
                var newLine = '';
                newLine += '<li>';
                newLine += '    <a class="mnMenuItem" name="' + level_0_name + '" data-item-length="' + level_0_item_length + '" data-parent="#' + treeMenuUniqeName + '" data-toggle="collapse" data-target="#' + level_0_name + '" aria-expanded="' + self.opt.dataSource[i].expanded + '">';
                newLine += '        <i class="' + self.opt.dataSource[i].cssClass + '"></i>';
                newLine += '        <span>' + self.opt.dataSource[i].text + '</span>';

                if (self.opt.dataSource[i].items) {
                    newLine += '        <i class="mnCollapseButton fa fa-angle-left float-right"></i>';
                }
                newLine += '    </a>';

                if (self.opt.dataSource[i].items) {
                    newLine += '<ul class="collapse ' + (self.opt.dataSource[i].expanded === true ? " show " : "") + '" id="' + level_0_name + '">';
                    for (var k = 0; k < self.opt.dataSource[i].items.length; k++) {
                        var level_1_name = mnApi.ReplaceAll(self.opt.dataSource[i].items[k].id, ".", "-");
                        var level_1_item_length = (self.opt.dataSource[i].items[k].items === undefined ? 0 : self.opt.dataSource[i].items[k].items.length);
                        newLine += '<li>';
                        newLine += '    <a class="mnMenuItem" name="' + level_1_name + '" data-item-length="' + level_1_item_length + '" data-parent="#' + level_0_name + '" data-toggle="collapse" data-target="#' + level_1_name + '" aria-expanded="' + self.opt.dataSource[i].items[k].expanded + '">';
                        newLine += '        <i class="' + self.opt.dataSource[i].items[k].cssClass + '"></i>';
                        newLine += '        <span>' + self.opt.dataSource[i].items[k].text + '</span>';
                        //--------level(2) için Collapse Button baş-----------------------
                        if (self.opt.dataSource[i].items[k].items) {
                            newLine += '        <i class="mnCollapseButton fa fa-angle-left float-right"></i>';
                        }
                        //-----------level(2) için Collapse Button bit--------------------
                        newLine += '    </a>';
                        //level(2) yeri...bas
                        if (self.opt.dataSource[i].items[k].items) {
                            newLine += '<ul class="collapse ' + (self.opt.dataSource[i].items[k].expanded === true ? " show " : "") + '" id="' + level_1_name + '">';
                            for (var j = 0; j < self.opt.dataSource[i].items[k].items.length; j++) {
                                var level_2_name = mnApi.ReplaceAll(self.opt.dataSource[i].items[k].items[j].id, ".", "-");
                                var level_2_item_length = (self.opt.dataSource[i].items[k].items[j].items === undefined ? 0 : self.opt.dataSource[i].items[k].items[j].items.length);
                                newLine += '<li>';
                                newLine += '    <a class="mnMenuItem" name="' + level_2_name + '" data-item-length="' + level_2_item_length + '" >';
                                newLine += '        <i class="' + self.opt.dataSource[i].items[k].items[j].cssClass + '"></i>';
                                newLine += '        <span>' + self.opt.dataSource[i].items[k].items[j].text + '</span>';
                                newLine += '    </a>';
                                newLine += '</li>';
                            }
                            newLine += '</ul>';
                        }
                        //level(2) yeri...bit

                        newLine += '</li>';
                    }
                    newLine += '</ul>';
                }

                newLine += '</li>';

                menuHtmls.append($(newLine));
            }

            return menuHtmls;
        }

        function fInit() {
            fAddStyle();
            $this.addClass("mnTreeMenu");

            $this.empty().append(fGetMenuHtml());

            $this.on("click", "a", function (e) {
                var id = $(this).attr("name");
                var itemLength = $(this).attr("data-item-length");
                id = mnApi.ReplaceAll(id, "-", ".");
                //event bind
                if (self.opt.onFullClick !== undefined) {
                    self.opt.onFullClick({ "id": id, "itemLength": itemLength });
                }
            });

            $this.on("click", "a[data-item-length=0]", function (e) {
                $this.find(".mnMenuItem.active").removeClass("active");
                $(this).addClass("active");
                var id = $(this).attr("name");
                id = mnApi.ReplaceAll(id, "-", ".");
                //event bind
                if (self.opt.onClick !== undefined) {
                    self.opt.onClick({ "id": id });
                }
            });
        };

        self.fRefresh = function (dataSource) {
            self.opt.dataSource = dataSource;
            $this.empty().append(fGetMenuHtml());
            //event bind
            if (self.opt.onRefresh !== undefined) {
                self.opt.onRefresh(self);
            }
        }

        self.fSetActive = function (name) {
            var name = mnApi.ReplaceAll(name, ".", "-");
            $this.find(".mnMenuItem[name='" + name + "']").click();
        }

        fInit();

        return self;
    };

})(jQuery);
