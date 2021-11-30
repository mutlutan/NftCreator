
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
                <div id="divGrid"></div>
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
