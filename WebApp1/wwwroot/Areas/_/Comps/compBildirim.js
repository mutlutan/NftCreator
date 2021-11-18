
function compBildirim(_elm, _opt) {
    var self = {};

    self = $.extend({
        //input1: "" //input param
    }, _opt);

    self.$elm = $(_elm);

    function fnHtmlAppend() {
        var temp = ``;
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

    }

    function fnBildirimGoster() {
        var temp = `
            <div name="divBildirimWindow" >
                Bildirimler Burada...
            </div>
        `;

        let $temp = $(temp);

        var popup = $temp.kendoWindow({
            modal: true,
            actions: ["Close"],
            title: "Bildirimler",
            close: function (e) {
                e.sender.destroy();
            },
            open: function (e) {

            }
        }).getKendoWindow();

        popup.center().open();
    }

    function fnBildirimGetir() {
        self.$elm.addClass("animated faa-ring");
    }


    self.prepare = function (_opt) {
        self.opt = $.extend({}, _opt);

        fnHtmlAppend();
        fnStyleAppend();

        //create elements
        fnCreateElements();

        self.$elm.click(function (e) {
            self.$elm.removeClass("animated faa-ring");
            fnBildirimGoster();
        });

        fnBildirimGetir();

    };

    return self;
};
