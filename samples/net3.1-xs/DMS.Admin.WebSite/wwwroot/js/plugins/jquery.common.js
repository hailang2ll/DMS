/**
 * @authors jquery 通用插件扩展
 * @date    2015-08-26 15:55:16
 * @version 谭子良
 */


/*
 * 重置表单清空
 */
$.fn.clearForm = function () {
    return this.each(function () {
        $('input,select,textarea', this).clearFields();
    });
};


/*
 * 重置表单清空
 */
$.fn.clearFields = $.fn.clearInputs = function () {
    return this.each(function () {
        var t = this.type, tag = this.tagName.toLowerCase();
        if (t == 'text' || t == 'hidden' || t == 'password' || tag == 'textarea')
            this.value = '';
        else if (t == 'checkbox' || t == 'radio')
            this.checked = false;
        else if (tag == 'select')
            this.selectedIndex = 0;
    });
};


/*
 * 选项卡插件
 */
$.fn.tab = function (options) {
    var defaults = {
        type: 'click',                // 触发方式
        title: '.tabTitle',            // tab标题class
        cnt: '.tabConter',           // tab内容class
        titleActive: 'selected',            // 标题激活样式
        active: 0,                           // 默认激活项（从1开始）
        beforeChange: null,                  // 触发前函数
        afterChange: null                   // 触发后函数
    };
    var opts = $.extend(defaults, options);
    // 切换方法
    var switchTab = function (o, idx) {
        o.children(opts.title).find("li").removeClass(opts.titleActive).eq(idx).addClass(opts.titleActive);
        o.children(opts.cnt).children().eq(idx).show().siblings().hide();
        if (opts.afterChange) opts.afterChange(o);
    };
    // 初始化
    var init = function (o) {
        o.children(opts.title).on(opts.type, "li", function () {
            var idx = $(this).index();
            if (opts.beforeChange) { opts.beforeChange(o); }
            switchTab(o, idx);
        });
    };
    return this.each(function () {
        var o = $(this);
        switchTab(o, opts.active);
        init(o);
    });
};


/*
* @constructor $.fn.bindSelect 
* @description 提示信息集合
* @example $("#ddlUserType").bindSelect(true,enumClass.EnumMemUserType,function(item){return true;});
* @param 参数 empty:是否加上请选择,data:enumClass对象参数, predicate:过滤方法,可以为空
*/
$.fn.bindSelect = function (empty, data, predicate) {
    var query = $(this).html("");
    if (empty) {
        query.html("<option value=\"\">--请选择--</option>");
    }
    if (data) {
        $.each(data, function (i, item) {
            if (predicate && $.isFunction(predicate)) {
                if (predicate(item)) {
                    query.append("<option value=\"" + item["n"] + "\">" + item["v"] + "</option>");
                }
            } else {
                query.append("<option value=\"" + item["n"] + "\">" + item["v"] + "</option>");
            }
        });
    }
}
/*
绑定分类
*/
$.fn.bindCategory = function (empty, data, predicate, predicateKey) {

    var query = $(this).html("");
    if (empty) {
        query.html("<option value=\"\">--请选择--</option>");
    }
    function getFiex(count) {
        if (count == 2) {
            return "&nbsp;&nbsp;";
        }
        if (count == 3) {
            return "&nbsp;&nbsp;&nbsp;|--";
        }
        if (count == 4) {
            return "&nbsp;&nbsp;&nbsp;&nbsp;|--";
        }
        if (count == 5) {
            return "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|--";
        }
        if (count == 6) {
            return "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|--";
        }
        if (count == 7) {
            return "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|--";
        }
        return "";
    }
    function forCategory(data) {
        $.each(data, function (i, item) {
            var itemVal = "";
            if (predicateKey && $.isFunction(predicateKey)) {
                itemVal = predicateKey(item);
            } else {
                //itemVal = item["id"] + "|" + item["code"] + "|" + item["name"] + "|" + item["codepath"] + "|" + item["id"] + "|" + item["namepath"] + "|" + item["level"];
                itemVal = item["id"] + "|" + item["code"] + "|" + item["name"] + "|" + item["codepath"] + "|" + item["namepath"] + "|" + item["level"];
            }
            if (predicate && $.isFunction(predicate)) {
                if (predicate(item)) {
                    query.append("<option value=\"" + itemVal + "\">" + getFiex(item["level"]) + item["name"] + "</option>");
                }
            } else {
                query.append("<option value=\"" + itemVal + "\">" + getFiex(item["level"]) + item["name"] + "</option>");
            }
            if (item["categoryList"] && item["categoryList"].length > 0) {
                forCategory(item["categoryList"]);
            }
        });
    }
    if (data) {
        forCategory(data);
    }
}

Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) { return false; }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n++] = this[i]
        }
    }
    this.length -= 1
}

var getEnumValue = function (type, enumObj) {
    var result = "";
    $.each(enumObj, function (i, item) {
        if (item["n"] == type) {
            result = item["v"];
        }
    });
    return result;

}
var parseDateString = function (timeString, timeFormat) {
    if (timeString.indexOf('T') > 0) {
        timeString = timeString.replace('T', ' ');
    }
    if (timeFormat.toLowerCase() == 'ymd') {
        return timeString.substring(0, 10);
    }
    else if (timeFormat.toLowerCase() == 'ymd hm') {
        return timeString.substring(0, 16);
    }
}

