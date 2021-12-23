
/**
 * @authors 工具包对象 | 依赖于 jquery.js
 * @date    2015-08-26 15:55:16
 * @version 谭子良
 */

/* 
 * *************************************************************
 * 
 * 下面为低版本浏览器提供高版本浏览器通用函数支持
 * 
 * *************************************************************
 */

// indexOf
if (!Array.indexOf) {
    Array.prototype.indexOf = function (obj) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == obj) {
                return i;
            }
        }
        return -1;
    }
}

// forEach
if (!Array.prototype.forEach) {
    Array.prototype.forEach = function (fun /*, thisp*/) {
        var len = this.length;
        if (typeof fun != "function")
            throw new TypeError();
        var thisp = arguments[1];
        for (var i = 0; i < len; i++) {
            if (i in this)
                fun.call(thisp, this[i], i, this);
        }
    };
}

/* 
 * *************************************************************
 * 
 * 下面为 Json2.js 源码
 * 
 * *************************************************************
 */

if (typeof JSON !== 'object') {
    JSON = {};
}

(function () {
    'use strict';

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {
        Date.prototype.toJSON = function () {
            return isFinite(this.valueOf()) ? this.getUTCFullYear() + '-' +
                f(this.getUTCMonth() + 1) + '-' +
                f(this.getUTCDate()) + 'T' +
                f(this.getUTCHours()) + ':' +
                f(this.getUTCMinutes()) + ':' +
                f(this.getUTCSeconds()) + 'Z' : null;
        };
        String.prototype.toJSON =
            Number.prototype.toJSON =
            Boolean.prototype.toJSON = function () {
                return this.valueOf();
            };
    }

    var cx,
        escapable,
        gap,
        indent,
        meta,
        rep;

    function quote(string) {
        // If the string contains no control characters, no quote characters, and no
        // backslash characters, then we can safely slap some quotes around it.
        // Otherwise we must also replace the offending characters with safe escape
        // sequences.
        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string' ? c : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }

    function str(key, holder) {
        // Produce a string from holder[key].
        var i, // The loop counter.
            k, // The member key.
            v, // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];
        // If the value has a toJSON method, call it to obtain a replacement value.
        if (value && typeof value === 'object' &&
            typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }
        // If we were called with a replacer function, then call the replacer to
        // obtain a replacement value.
        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }
        // What happens next depends on the value's type.
        switch (typeof value) {
            case 'string':
                return quote(value);
            case 'number':
                // JSON numbers must be finite. Encode non-finite numbers as null.
                return isFinite(value) ? String(value) : 'null';
            case 'boolean':
            case 'null':
                // If the value is a boolean or null, convert it to a string. Note:
                // typeof null does not produce 'null'. The case is included here in
                // the remote chance that this gets fixed someday.
                return String(value);
            // If the type is 'object', we might be dealing with an object or an array or
            // null.
            case 'object':
                // Due to a specification blunder in ECMAScript, typeof null is 'object',
                // so watch out for that case.
                if (!value) {
                    return 'null';
                }
                // Make an array to hold the partial results of stringifying this object value.
                gap += indent;
                partial = [];
                // Is the value an array?
                if (Object.prototype.toString.apply(value) === '[object Array]') {
                    // The value is an array. Stringify every element. Use null as a placeholder
                    // for non-JSON values.
                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || 'null';
                    }
                    // Join all of the elements together, separated with commas, and wrap them in
                    // brackets.
                    v = partial.length === 0 ? '[]' : gap ? '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' : '[' + partial.join(',') + ']';
                    gap = mind;
                    return v;
                }
                // If the replacer is an array, use it to select the members to be stringified.
                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        if (typeof rep[i] === 'string') {
                            k = rep[i];
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                } else {
                    // Otherwise, iterate through all of the keys in the object.
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                }
                // Join all of the member texts together, separated with commas,
                // and wrap them in braces.
                v = partial.length === 0 ? '{}' : gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' : '{' + partial.join(',') + '}';
                gap = mind;
                return v;
        }
    }

    // If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g;
        meta = { // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"': '\\"',
            '\\': '\\\\'
        };
        JSON.stringify = function (value, replacer, space) {

            // The stringify method takes a value and an optional replacer, and an optional
            // space parameter, and returns a JSON text. The replacer can be a function
            // that can replace values, or an array of strings that will select the keys.
            // A default replacer method can be provided. Use of the space parameter can
            // produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

            // If the space parameter is a number, make an indent string containing that
            // many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

                // If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

            // If there is a replacer, it must be a function or an array.
            // Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

            // Make a fake root object containing our value under the key of ''.
            // Return the result of stringifying the value.

            return str('', {
                '': value
            });
        };
    }

    // If the JSON object does not yet have a parse method, give it one.
    if (typeof JSON.parse !== 'function') {
        cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g;
        JSON.parse = function (text, reviver) {

            // The parse method takes a text and an optional reviver function, and returns
            // a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

                // The walk method is used to recursively walk the resulting structure so
                // that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


            // Parsing happens in four stages. In the first stage, we replace certain
            // Unicode characters with escape sequences. JavaScript handles many characters
            // incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

            // In the second stage, we run the text against regular expressions that look
            // for non-JSON patterns. We are especially concerned with '()' and 'new'
            // because they can cause invocation, and '=' because it can cause mutation.
            // But just to be safe, we want to reject all unexpected forms.

            // We split the second stage into 4 regexp operations in order to work around
            // crippling inefficiencies in IE's and Safari's regexp engines. First we
            // replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
            // replace all simple value tokens with ']' characters. Third, we delete all
            // open brackets that follow a colon or comma or that begin the text. Finally,
            // we look to see that the remaining characters are only whitespace or ']' or
            // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                    .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                    .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                // In the third stage we use the eval function to compile the text into a
                // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                // in JavaScript: it can begin a block or an object literal. We wrap the text
                // in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

                // In the optional fourth stage, we recursively walk the new structure, passing
                // each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function' ? walk({
                    '': j
                }, '') : j;
            }

            // If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }

}());

/* 
 * *************************************************************
 * 
 * 下面为通用工具包函数 window.T 对象
 * 
 * *************************************************************
 */

; (function (window, document, undefined) {

    window.T = {};
    var _HOSTNAME = location.hostname,
        _PORT = location.port,
        _DOMAIN = "";
    if (_HOSTNAME == 'localhost' && _PORT != '') {
        _HOSTNAME = _HOSTNAME + ":" + _PORT;
    }
    else {
        _DOMAIN = _HOSTNAME.slice(parseInt(_HOSTNAME.lastIndexOf(".") - 5));
        // 跨域配置 设置跨域的基域名称必须与后台一致
        document.domain = T.DOMAIN.DOMAIN;
    }

    // 域名配置
    T.DOMAIN = {
        /* ACTION: 'http://action.' + _DOMAIN + '/',*/
        WWW: 'http://' + _HOSTNAME + '/',
        /* BASE: 'http://base.' + _HOSTNAME + '/',*/
        DOMAIN: _DOMAIN
    };

    /* 
     * 用途：设置cookie 依赖 jquery.cookie.js
     */
    T.cookie = function (key, value, day) {
        //?替换成分钟数如果为60分钟则为 60 * 60 *1000
        if (typeof (key) != 'undefined' && typeof (value) != 'undefined') {
            if (day) {
                $.cookie(key, value, { expires: day, path: '/', domain: T.DOMAIN.DOMAIN });
            } else {
                $.cookie(key, value, { path: '/' });
            }
        } else if (typeof (key) != 'undefined') {
            return $.cookie(key);
        }
    };

    // 清除 cookie 设置过期时间
    T.uncookie = function () {
        $.cookie("__asid", "", { expires: -1, path: '/', domain: T.DOMAIN.DOMAIN });
        //$.cookie("_type", "", { expires: -1, path: '/' });
    };

    /* 
     * 用途：获取浏览器版本 
     * 返回：{os:**,engine:**,browser:**,version:**}
     */
    T.getBrowser = function () {
        var ua = navigator.userAgent.toLowerCase(),
            re_msie = /\b(?:msie |ie |trident\/[0-9].*rv[ :])([0-9.]+)/;

        function toString(object) {
            return Object.prototype.toString.call(object);
        }

        function isString(object) {
            return toString(object) === "[object String]";
        }


        var ENGINE = [
            ["trident", re_msie],
            ["webkit", /\bapplewebkit[\/]?([0-9.+]+)/],
            ["gecko", /\bgecko\/(\d+)/],
            ["presto", /\bpresto\/([0-9.]+)/]
        ];

        var BROWSER = [
            ["ie", re_msie],
            ["firefox", /\bfirefox\/([0-9.ab]+)/],
            ["opera", /\bopr\/([0-9.]+)/],
            ["chrome", / (?:chrome|crios|crmo)\/([0-9.]+)/],
            ["safari", /\bversion\/([0-9.]+(?: beta)?)(?: mobile(?:\/[a-z0-9]+)?)? safari\//]
        ];

        // 操作系统信息识别表达式
        var OS = [
            ["windows", /\bwindows nt ([0-9.]+)/],
            ["ipad", "ipad"],
            ["ipod", "ipod"],
            ["iphone", /\biphone\b|\biph(\d)/],
            ["mac", "macintosh"],
            ["linux", "linux"]
        ];

        var IE = [
            [6, 'msie 6.0'],
            [7, 'msie 7.0'],
            [8, 'msie 8.0'],
            [9, 'msie 9.0'],
            [10, 'msie 10.0']
        ];

        var detect = function (client, ua) {
            for (var i in client) {
                var name = client[i][0],
                    expr = client[i][1],
                    isStr = isString(expr),
                    info;
                if (isStr) {
                    if (ua.indexOf(expr) !== -1) {
                        info = name;
                        return info
                    }
                } else {
                    if (expr.test(ua)) {
                        info = name;
                        return info;
                    }
                }
            }
            return 'unknow';
        };

        return {
            os: detect(OS, ua),
            browser: detect(BROWSER, ua),
            engine: detect(ENGINE, ua),
            //只有IE才检测版本，否则意义不大
            version: re_msie.test(ua) ? detect(IE, ua) : ''
        };
    };

    T.GoBack = function pageGoBack() {
        if (document.referrer) {
            window.location.href = document.referrer;
        } else {
            window.history.back(-1);
        }
    }

    /* 
     * 用途：基于时间戳生成20位全局唯一标识（每一毫秒只对应一个唯一的标识，适用于生成DOM节点ID） 
     */
    T.UUID = function (len) {
        var timestamp = new Date().getTime() || 0,
            chars = 'abcdefghijklmnopqrstuvwxyz',
            uuid = '';
        this.timestamp = this.timestamp == timestamp ? timestamp + 1 : timestamp;
        timestamp = '' + this.timestamp;
        len = len || 20;
        for (var i = 0; i < len; i++) {
            var k = timestamp.charAt(i);
            if (k == '') {
                k = Math.floor(Math.random() * 26);
            }
            uuid += chars.charAt(k) || 'x';
        }
        return uuid;
    };

    /*
     * 用途：生成GUID的方法
     */
    T.Guid = function () {
        var guid = "";
        for (var i = 1; i <= 32; i++) {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) guid += "-"
        }
        return guid
    };

    /*
     * 用途：为页面增加一段 cssText到头部
     * @param {string} [css_Str] css样式字符串
     * @param {string} [cssID] 添加到指定的styleID区块
     * @param {string} [target] 是否添加到父页面
     */
    T.addCssText = function (css_Str, cssID, target) {
        if (css_Str) {
            var css_ID = cssID ? cssID : 'costomCssTest';
            var BRO = T.getBrowser();
            if (BRO.browser == 'ie' && BRO.version < 9) {
                // 如果是IE浏览器 9.0以下则需要使用以下方式增加
                // var ArrSheet = document.styleSheets; 样式表
                if (target == 'parent') {
                    var ArrSheetNew = window.parent.document.createStyleSheet();
                    ArrSheetNew.cssText = css_Str;
                    ArrSheetNew.id = css_ID;
                } else {
                    var ArrSheetNew = document.createStyleSheet();
                    ArrSheetNew.cssText = css_Str;
                    ArrSheetNew.id = css_ID;
                }
            } else {
                if (target == 'parent') {
                    var Dom_Style = window.parent.document.getElementById(css_ID)
                } else {
                    var Dom_Style = document.getElementById(css_ID)
                }
                if (Dom_Style) {
                    //如果对应ID的节点已经存在
                    var CssContent = Dom_Style.textContent;
                    Dom_Style.textContent = (CssContent + css_Str);
                } else {
                    var style = document.createElement("style");
                    style.id = css_ID; //指定ID
                    style.type = "text/css";
                    style.textContent = css_Str;
                    if (target == 'parent') {
                        window.parent.document.getElementsByTagName("HEAD").item(0).appendChild(style);
                    } else {
                        document.getElementsByTagName("HEAD").item(0).appendChild(style);
                    }
                }
            }
        }
    }

    /*
     * 用途：动态加载 link css文件
     * @param {string} [url] css样式地址
     * @param {string} [cssID] css link ID
     * @param {string} [target] 是否添加到父页面
     */
    T.addCssLink = function (url, cssID, target) {
        if (url) {
            var css_ID = cssID ? cssID : "";
            var link = document.createElement('link');
            link.id = css_ID;
            link.href = url + (url.indexOf('?') < 0 ? '?' : '&') + 'rand=' + T.UUID();
            link.rel = 'stylesheet';
            link.type = 'text/css';
            if (target == 'parent') {
                window.parent.document.getElementsByTagName("HEAD").item(0).appendChild(link);
            } else {
                document.getElementsByTagName("HEAD").item(0).appendChild(link);
            }
        }
    }

    /*
     * 用途：获取地址栏中某一个参数的值 alert(T.getUrlParam("name"));
     * @param {string} [name] 参数key值
     */
    T.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            // return unescape(r[2]);
            return decodeURI(r[2]);
        }
        return null;
    }

    /*
     * 用途：得到地址栏中的GET参数数组
     * alert(requestArr['参数']);
     */
    T.getUrlParamJson = function () {
        // 获取url中"?"符后的字串
        var url = location.search;
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }

    /**
     * @得到数据类型
     * @method Typeof
     * @param {Object} [obj] 必选，数据
     * @param {RegExp} [type] 可选，数据类型正则表达式
     * Return {Boolean|String} 传入数据类型正则，则返回Boolean，否则返回数据类型String
     */
    T.Typeof = function (obj, type) {
        var oType = {
            '[object Boolean]': 'Boolean',
            '[object Number]': 'Number',
            '[object String]': 'String',
            '[object Function]': 'Function',
            '[object Array]': 'Array',
            '[object Date]': 'Date',
            '[object RegExp]': 'RegExp',
            '[object Object]': 'Object'
        },
            ret = obj == null ? String(obj) : oType[Object.prototype.toString.call(obj)] || 'Unknown';
        return type ? type.test(ret) : ret;
    };

    /**
     * @对象遍历
     * @method Each
     * @param {Object} [obj] 必选，对象
     * @param {Function} [callback] (key,value) 必选，回调函数
     * Return {Object} 对象
     */
    T.Each = function (obj, callback) {
        if (!obj) return obj;
        if (!T.Typeof(callback, /Function/)) return obj;
        if (T.Typeof(obj, /Array|NodeList/)) {
            for (var l = obj.length, i = 0; i < l; i++)
                if (callback(i, obj[i]) === false) break;
        } else if (T.Typeof(obj, /Object/)) {
            for (var o in obj)
                if (obj.hasOwnProperty && obj.hasOwnProperty(o))
                    if (callback(o, obj[o]) === false) break;
        }
        return obj;
    };

    /**
     * create 方法
     * @param {String} [tag] 标签类型
     * @param {Object} [attrs] 属性集合
     */
    T.element = function (tag, attrs) {
        tag = tag || 'div';
        attrs = attrs || {};
        var node = document.createElement(tag);
        T.Each(attrs, function (k, v) {
            if (k == 'class') node.className = v;
            else if (k == 'id') node.id = v;
            else if (k == 'name') node.name = v;
            else if (k == 'style') node.style.cssText = v;
            else $(node).attr(k, v);
        });
        return node;
    };

    /**
     * @将参数对象转换为URL参数字符串
     * @method ConvertToQueryString
     * @param {Object} [options] 必选，参数对象
     * Return {String} URL参数字符串
     */
    T.ConvertToQueryString = function (options) {
        var params = [];
        for (var o in options)
            if (options.hasOwnProperty(o))
                params.push(o + '=' + encodeURIComponent(typeof options[o] === 'object' ? JSON.stringify(options[o]) : options[o]));
        return params.join('&');
    };

    /**
     * GET 方法
     * @param {Object} options 请求数据
     * @param {String} options.action 请求API
     * @param {Object} options.params 请求参数
     * @param {Function} options.success 请求成功后回调
     * @param {Function} options.failure 请求失败后回调（逻辑错误）
     * @param {Function} options.error 请求出错后回调（系统错误）
     */
    T.GET = function (options, _failure, _error) {
        if (!options || !options.action) return;
        var params = options.params || {};
        var jsoncallback = params.jsoncallback || T.UUID().toUpperCase(); //产生随机函数名
        if (!/^http/.test(options.action)) {
            options.action = T.DOMAIN.ACTION + options.action;
        }
        params.jsoncallback = jsoncallback;
        var _params = params;
        options.action += (options.action.indexOf('?') < 0 ? '?' : '&') + T.ConvertToQueryString(_params);
        var script = document.createElement('script');
        script.defer = 'defer';
        script.async = 'async';
        script.src = options.action;
        document.documentElement.appendChild(script);
        // 定义回调函数
        window[jsoncallback] = function (response) {
            //定义被脚本执行的回调函数
            try {
                options.data = response;
                T.callback(options);
            } catch (e) {
                // alert(e);
            } finally {
                //最后删除该函数与script元素
                if (script && script.parentNode && script.parentNode.removeChild) {
                    script.parentNode.removeChild(script);
                }
                script = null;
                //加入堆栈
                setTimeout(function () {
                    window[jsoncallback] = null;
                }, 10);
            }
        };
        script.onerror = function () {
            if (!script) return;
            // 报错
            window[jsoncallback] = null;
            if (typeof options.error === 'function') {
                options.error();
            }
            // 最后删除该函数与script元素
            if (script && script.parentNode && script.parentNode.removeChild) {
                script.parentNode.removeChild(script);
            }
            script = null;
            alert("服务器系统繁忙，请稍后再试！");
        };
    };

    /**
     * POST 方法
     * @param {Object} options 请求数据
     * @param {String} options.action 请求API
     * @param {Object} options.params 请求参数
     * @param {Function} options.success 请求成功后回调
     * @param {Function} options.failure 请求失败后回调（逻辑错误）
     * @param {Function} options.error 请求出错后回调（系统错误）
     */
    T.POST = function (options, _error) {

        if (!options || !options.action) return;
        T.POST.zIndex = (T.POST.zIndex || 0) + 1;
        options.params = options.params || {};
        if (!/^http/.test(options.action)) {
            options.action = T.DOMAIN.ACTION + options.action;
        }
        // 添加唯一随机ID
        options.action += (options.action.indexOf('?') > 0 ? '&' : '?') + 'ccdfs=' + T.UUID().toUpperCase(); //Math.random();

        // 创建表单
        var form = T.element('form', {
            target: 'piframe_' + T.POST.zIndex,
            action: options.action,
            method: 'post',
            style: 'display:none'
        });

        // 创建 iframe 页面
        var iframe;
        try { // for I.E.
            iframe = document.createElement('<iframe name="piframe_' + T.POST.zIndex + '">');
        } catch (ex) { //for other browsers, an exception will be thrown
            iframe = T.element('iframe', {
                name: 'piframe_' + T.POST.zIndex,
                src: '#',
                //'about:blank'
                style: 'display:none'
            });
        }
        if (!iframe) return;
        iframe.style.display = 'none';
        document.body.appendChild(iframe);
        document.body.appendChild(form);

        // 对数组参数进行特殊处理
        var arrParamHandle = function (name, arr) {
            for (var i = 0, len = arr.length; i < len; i++) {
                form.appendChild(T.element('input', {
                    type: 'hidden',
                    name: name,
                    value: (typeof arr[i] === 'object' ? JSON.stringify(arr[i]) : arr[i])
                }));
            };
        }

        // 表单赋值
        var formdata = options.params;
        T.Each(formdata, function (k, v) {
            // 一级数组对象处理
            if (T.Typeof(v, /Array/)) {
                arrParamHandle(k, v)
            } else {
                // if (/password$|pwd$/i.test(k)) v = window.cipher(window.cipher(v));
                form.appendChild(T.element('input', {
                    type: 'hidden',
                    name: k,
                    value: (typeof v === 'object' ? JSON.stringify(v) : v)
                }));
            }
        });

        // console.log(JSON.stringify(options.params));
        // return;

        // frameElement.callback({0})
        iframe.callback = function (o) {
            // console.log(o);
            iframe.parentNode.removeChild(iframe);
            form.parentNode.removeChild(form);
            iframe = form = null;
            options.data = o;
            T.callback(options);
        };
        /*iframe.onload = function(){
            if(!iframe)return;
            iframe.parentNode.removeChild(iframe), form.parentNode.removeChild(form), iframe = form = null;
            _error&&_error();
            T.alt(T.TIPS.DEF, function(_this){
                _this.remove();
                //location.reload();
            }, function(){
                //location.reload();
            });
        };*/
        if (options.before) {
            options.before(form, iframe);
        }
        form.submit();
    };

    /**
     * 用途：T.GET 数据成功后回调处理函数
     * @param {Object} options 请求数据
     * @param {String} options.action 请求API
     * @param {Object} options.params 请求参数
     * @param {Function} options.success 请求成功后回调
     * @param {Function} options.failure 请求失败后回调（逻辑错误）
     * @param {Function} options.error 请求出错后回调（系统错误）
     */

    T.callback = function (options) {
        // 有JSON数据返回
        if (options && T.Typeof(options.data, /Object/)) {
            // console.log('==========>', options.action.replace(/\?.*/, ''), '    ', options.data, '    ', '路由');
            // 服务器返回JSON数据==正常返回
            if (options.data.errno == 0 && T.Typeof(options.data.data, /Object/)) {
                if (options.data.data.errno == 0) {

                    if (options.success) {

                        options.success(options.data.data);
                    }
                } else if (options.data.data.errno > 0) {
                    if (options.failure) {
                        options.failure(options.data.data);
                    }
                } else {
                    if (options.error) {
                        options.error(options.data.data);
                    }
                }

            } else if (options.data.errno > 0) {
                if (options.failure) {
                    //路由系统逻辑错误
                    options.failure(options.data);
                }
            } else {
                if (options.error) {
                    //路由系统异常
                    options.error(options.data);
                }
            }
        } else {
            if (options.error) {
                options.error("系统繁忙，请稍后再试");
            }
        }
    };


    //T.callback = function(options) {
    //    // 有JSON数据返回
    //    if (options && T.Typeof(options.data, /Object/)) {
    //        // console.log('==========>', options.action.replace(/\?.*/, ''), '    ', options.data, '    ', '路由');
    //        // 服务器返回JSON数据==正常返回
    //        if (options.data.errno == 0 && T.Typeof(options.data.data, /Object/)) {
    //            if (options.success) {
    //                options.success(options.data.data);
    //            }
    //        // 数据逻辑错误
    //        }else if(options.data.errno !== 0){
    //            if (options.failure) {
    //                options.failure(options.data);
    //            }
    //        // options.data.data 不是一个Json对象 [系统错误]
    //        }else{
    //            if (options.error) {
    //                options.error("系统错误");
    //            }
    //        }
    //    // options.data有可能不是一个Json对象 [系统错误]
    //    } else {
    //        if (options.error) {
    //            options.error("系统错误");
    //        }
    //    }
    //};

    /**
     * 用途：表单验证配置参数
     */
    T.validConfigs = {

        // 验证用的input属性
        flag: "test",

        // 验证规划与提示语
        valid: {
            required: {
                pattern: '',
                msg: '必填'
            },
            email: {
                pattern: /^[\w\-\.]+@[\w\-\.]+(\.\w+)+$/,
                msg: '必须是正确的邮件地址'
            },
            plusdecimal: {
                pattern: /(^\d+)(\.\d+)?$/,
                msg: '必须是数字(正整数[小数])'
            },
            plusnumber: {
                pattern: /^[0-9]*$/,
                msg: '必须是数字(正整数)'
            },
            decimal: {
                pattern: /(^-?\d+)(\.\d+)?$/,
                msg: '必须是数字(正负(整数)小数)'
            },
            number: {
                pattern: /^[+-]?[0-9]*$/,
                msg: '必须是数字(正负整数)'
            },
            username: {
                pattern: '/^\w{3,16}$/',
                msg: '长度不对或者含有特殊字符'
            },
            url: {
                pattern: "^((https|http|ftp|rtsp|mms)?://)" + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" + "(([0-9]{1,3}.){3}[0-9]{1,3}" + "|" + "([0-9a-z_!~*'()-]+.)*" + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]." + "[a-z]{2,6})" + "(:[0-9]{1,4})?" + "((/?)|" + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$",
                msg: '网址输入有误'
            },
            mobile: {
                pattern: /^1[3,5,8]{1}[0-9]{1}[0-9]{8}$/g,
                msg: '手机号码错误'
            },
            tel: {
                pattern: /^(((?:[\+0]\d{1,3}-[1-9]\d{1,2})|\d{3,4})-)?\d{5,8}$/,
                msg: '电话号码格式不对'
            }
        },

        // input select textarea 报错在自己的后面

        // input 错误样式className
        errorInputCss: 'error',
        // input 报错样式className
        errorTipNodes: 'errorMsg',
        // input 报错内容 node innerHTML
        errorTipHtml: '<cite class="errorMsg"></cite>',

        // checkbox radio 报错位置放在自己父容器（td）标签内的最后面
        errorCheckNode: 'td',
        noCheckErrorTip: '<p class="errorMsg">必选项不能为空</p>'

    };

    /**
     * 用途：禁用文本框工具
     * T.formDisabled(node);    htmlnode：input | select | textarea 
     * @param {string} [node] 文档节点范围
     */
    T.formDisabled = function (node) {
        $(node).each(function () {
            if ($(this).is(":visible") && ($(this).attr('readonly') || $(this).attr('disabled'))) {
                if ($(this)[0].tagName.toUpperCase() == "SELECT") {
                    $(this).removeAttr('readonly').removeAttr('disabled').addClass('disabled').unbind()
                        .bind('change', function () {
                            this.selectedIndex = this.defOpt;
                        })
                        .bind('focus', function () {
                            this.defOpt = this.selectedIndex;
                        });
                } else {
                    // .removeAttr('readonly').removeAttr('disabled')
                    $(this).addClass('disabled').bind('change', function () {
                        return false;
                    }).bind('contextmenu', function (evt) {
                        return false;
                    }).bind('keydown', function (evt) {
                        return false;
                    });
                }
            }
        });
    }

    /*
     * 用途：注册表单验证
     * @param {string} [nodeID] 文档节点范围
     * @param {object} [options] 为T.validConfigs增加新的配置验证规则
     * 注册验证事件 会自动搜索出所有需要验证的表单项
     * T.registerFileds()
     */
    T.registerFileds = function (nodeID, options) {

        if ($.isPlainObject(options)) {
            T.validConfigs = $.extend(T.validConfigs, options);
        }

        // 解禁锁定域
        $("input,select,textarea").bind('disabled', function () {
            T.formDisabled(this);
        });
        T.formDisabled("input,select,textarea");

        // 内部变量
        var _self = this;
        var node = nodeID ? $(nodeID)[0] : null;
        if (node) {
            var els = $("[" + T.validConfigs.flag + "]", node);
        } else {
            var els = $("[" + T.validConfigs.flag + "]");
        }
        // 注册绑定
        $.each(els, function (k, v) {
            if (v.type == "radio" || v.type == "checkbox") {
                $(v).bind('validSubmit', function () {
                    _self.pickCheck(v.name);
                }).bind('change', function () {
                    _self.pickCheck(v.name);
                });
            } else {
                var el = $(v).attr(T.validConfigs.flag).split(" ");
                $.each(el, function (i, n) {
                    $(v).bind('validSubmit', function () {
                        bFlag = _self.pickValue(n, this);
                        if (!bFlag && $(this).next("." + T.validConfigs.errorTipNodes).length == 0) {
                            _self.tipValue($(this), n);
                            $(this).focus();
                        }
                    }).bind('change', function () {
                        bFlag = _self.pickValue(n, this);
                        if (!bFlag && $(this).next("." + T.validConfigs.errorTipNodes).length == 0) {
                            _self.tipValue($(this), n);
                            $(this).focus();
                        } else {
                            $(this).removeClass(T.validConfigs.errorInputCss);
                            $(this).next("." + T.validConfigs.errorTipNodes).remove();
                        }
                    });
                });
            }
            if (k == 0) {
                $(v).closest("form").bind("submit", function () {
                    var a = T.validFileds(this);
                    if (a === false) {
                        return false;
                    }
                });
            }
        });

        // 报错
        this.tipValue = function (inputs, n) {
            inputs.addClass(T.validConfigs.errorInputCss);
            var tip = $(T.validConfigs.errorTipHtml);
            // 如果没有 n 这条验证规则
            if (!T.validConfigs.valid[n]) {
                tip.html("验证规则出错：" + n);
            } else {
                tip.html(T.validConfigs.valid[n].msg);
            }
            if (inputs[0].tagName.toUpperCase() == "TEXTAREA") {
                // 若是文本域
                inputs.after(tip);
            } else {
                // 其它
                inputs.after(tip);
            }
        }

        // 验证
        this.pickValue = function (testName, inputer) {
            var oValue = $(inputer).val(), enumType;
            if (testName == 'required') {
                return $.trim(oValue) != "";
            } else {
                if (oValue == "") {
                    return true;
                }
                // 如果没有 n 这条验证规则
                if (T.validConfigs["valid"][testName]) {
                    // 进行正则检测
                    return T.validConfigs["valid"][testName]["pattern"].test(oValue);
                } else {
                    // console.log("没有这个验证规则！")
                    return false;
                }
            }
        };

        // 验证多选，单选
        this.pickCheck = function (name) {
            var cks = $("[name=" + name + "]"), che = false;
            $.each(cks, function (k, v) {
                if (v.checked) {
                    che = true;
                    return false;
                }
            });
            if (!che && cks.last().parents(T.validConfigs.errorCheckNode).find("." + T.validConfigs.errorTipNodes).length == 0) {
                var tip = $(T.validConfigs.noCheckErrorTip);
                cks.last().parents(T.validConfigs.errorCheckNode).append(tip);
            } else {
                cks.last().parents(T.validConfigs.errorCheckNode).find("." + T.validConfigs.errorTipNodes).remove();
            }
        };

    };

    /*
     * 用途：对当前传入ID对象下面的所有表单进行验证
     * T.validFileds(rangeID)
     * @param {string} [rangeID] 范围指定
     */
    T.validFileds = function (rangeID) {
        // 传入当前对象为搜索范围
        var range = $(rangeID)[0];
        // 移除所有报错
        $("." + T.validConfigs.errorInputCss).removeClass(T.validConfigs.errorInputCss);
        $("." + T.validConfigs.errorTipNodes).remove();
        var bFlag = true;
        // 遍历
        var els = $("[" + T.validConfigs.flag + "]", range);
        if (els.length > 0) {
            $.each(els, function (i, n) {
                // 触发回调 如果有一处错误 则返回 false
                $(n).trigger('validSubmit');
                bFlag = $("." + T.validConfigs.errorTipNodes).length == 0;
                if (!bFlag) {
                    return false;
                }
            });
        }
        // 回传
        return bFlag;
    }

    /**
     * 用途：获得当前传入节点范围下面的所有表单JSON
     * T.formToJSON("#formID",function(json){});
     * 参数说明
     * @param {string} [rangeID] 指定范围内表单转成JSON
     * @param {function} [callback] 对返回实体回调操作
     */
    T.formToJSON = function (rangeID, callback) {
        var oEntity = {};
        if ($(rangeID).length == 0) {
            alert("查找panel,form元素失败");
            return false;
        }
        var a = T.validFileds(rangeID);
        if (a === false) { return false; }
        // 在当前范围内查找 input,select,textarea
        var els = $("input,select,textarea", $(rangeID)[0]);
        for (var i = 0, max = els.length; i < max; i++) {
            // checkbox radio 没选中的略过
            var aType = els[i].type;
            if (aType == 'radio' || aType == 'checkbox') {
                if (!els[i].checked) {
                    continue;
                }
            }
            // name 为空 ，值为空，略过
            var aName = els[i].name;
            var aValue = $(els[i]).val();
            if (aName == '' || aName == 'undefined' || aValue === null || typeof aValue == 'undefined' || aValue == '' || ($.isArray(aValue) && !aValue.length)) {
                continue;
            }
            aValue = $.trim(aValue);
            var aCondition = els[i].name.split('__');
            // 如果是 name__subname 则返回 name["subname1"] = value, name["subname2"] = value
            if (aCondition.length == 2) {
                if (!oEntity[aCondition[0]]) {
                    oEntity[aCondition[0]] = {}
                }
                oEntity[aCondition[0]][aCondition[1]] = (!oEntity[aCondition[0]][aCondition[1]]) ? aValue : oEntity[aCondition[0]][aCondition[1]] + "," + aValue;
            } else {
                oEntity[els[i].name] = (!oEntity[els[i].name]) ? aValue : oEntity[els[i].name] + "," + aValue;
            }
        }
        if (callback && $.isFunction(callback)) {
            oEntity = callback(oEntity);
        }
        return $.isPlainObject(oEntity) ? oEntity : false;
    }

    /**
     * 用途：获取数据生成页面列表及分页 具体参数见函数内部说明
     * @param {object} 
     * 特别说明：回来的数据必须是如下规则，若不是，必须转成如下格式
       configs.initData = data.data: {
            ResultList:[{"userid": 10602,"nick":"张三"},{"userid": 10602,"nick":"五武"}],
            "AllowPaging": true,
            "PageIndex": 1,
            "PageSize": 15,
            "TotalRecord": 263,
            "TotalPage": 18
       }
     *
     */
    T.getRequest = function (options) {

        // 预定基本数据
        var configs = {
            url: null,                              // 请求数据地址
            tmpl: null,                             // 预设JS模板 script tpl ID
            target: null,                           // 煊览容器 node ID
            pageBar: null,                           // 分页容器 node ID
            pageBarShow: true,                      // 不分页 | 分页
            page: {                                 // 分页对象字段如下 前端必须与后台数据一致
                PageIndex: 1,                       // 当前第几页
                PageSize: 15,                       // 每页多少条数据
                TotalRecord: 0,                     // 总记录条数
                TotalPage: 0                        // 共多少页
            },
            formId: null,                            // 指定表单ID，或nodeID下面的json数据
            onRequest: null,                        // 数据请求之前 执行的函数并传入 form toJson 对象
            onResponse: null,                       // 数据请求成功之后 对参数值特殊处理的函数
            onComplete: null,                       // 数据煊览完成之后
            onError: null,                          // 数据请求失败所执行的方法
            initData: null                          // 数据请求完成之后接收的服务器数据
        }
        // 分页配置
        options.page = $.extend({}, configs.page, options.page);
        // 参数叠加
        configs = $.extend(configs, options);
        // 如果存在搜索参数form表单 获取参数JSON
        var param = {}
        if (configs.formId) {
            param = $(configs.formId).formToJSON();
        }
        // form表单 请求参数JSON 提交前处理函数
        if (configs.onRequest) {
            param = configs.onRequest(param);
            if (param === null) {
                return
            }
        }
        // 发送给服务器的参数
        var jsonData = null;
        if (configs.initData == null) {
            jsonData = serializeRequest(configs, param);
        }
        // console.log(jsonData);
        // var infoTip = new Tip('数据加载中...'),
        //     _errorInfo = new Tip('服务器端出错'),
        //     _jq = jQuery;

        // T.GET 请求参数及回调
        var ajaxOptions = {
            requestConfigs: configs,    // 所有配置参数
            requestParam: param,        // 发送给服务器的参数
            action: configs.url,       // 请求地址
            params: jsonData,           // 发送数据
            // 出错时
            error: function (msg) {
                requestConfigs = this.requestConfigs;
                // 如果传入了报错函数
                if (requestConfigs.onError) {
                    requestConfigs.onError(msg);
                    // 调用默认报错函数
                } else {
                    // _errorInfo.show();
                    // _jq("#tip16").css("backgroundColor", "#fff").find("img").hide();
                }
            },
            // 回调函数
            success: function (data) {
                // 如果后台数据符合前端规则 即无需预处理
                configs.initData = data.data;
                // 否则存在预处理函数 则先对数据进行处理
                if (configs.onResponse && $.isFunction(configs.onResponse)) {
                    configs.initData = configs.onResponse(data);
                }
                // 显示列表
                // var targetTag = configs.target[0].tagName.toLowerCase();  == tbody
                if (configs.initData.ResultList && configs.initData.ResultList.length > 0) {
                    $(configs.target).html($(configs.tmpl).tmpl(configs.initData.ResultList));
                    $(configs.target).children(":odd").addClass("odd");
                    $(configs.target).children().hover(function () {
                        $(this).addClass("over");
                    }, function () {
                        $(this).removeClass("over");
                    });
                    $(configs.target).on("click", "tr", function () {
                        $(this).addClass("current").siblings().removeClass("current");
                    });
                } else {
                    // 没有数据！
                    $(configs.target).html("");
                    $(configs.pageBar).html('<p class="noDataBack">暂未查询到任何数据,请更改查询条件后重试！</p>');
                }
                // 显示分页条
                if (configs.initData.TotalRecord > 0) {
                    var pageBar = createPageBar(configs.initData.TotalRecord, configs.initData.PageSize, configs.initData.PageIndex)
                    $(configs.pageBar).html(pageBar);
                    // 绑定事件
                    $(configs.pageBar).find("a").off("click").on("click", function () {
                        var params = options;
                        params.page.PageIndex = $(this).attr("data-pn");
                        T.getRequest(params);
                    });
                }
            }
        }
        // 如果已经获取数据
        if (configs.initData != null) {
            ajaxOptions.success(configs.initData);
        } else {
            // infoTip.show();
            // $.ajax(ajaxOptions);
            if (window.T.GET && typeof (window.T.GET) == 'function') {
                T.GET(ajaxOptions);
            } else {
                console.log("error:T.GET");
            }
        }
        // 序列化请求数据 附加请求页码，每页多少条
        function serializeRequest(config, param) {
            var request = {
                PageIndex: config.page.PageIndex,
                PageSize: configs.page.PageSize
            }
            for (el in param) {
                request[el] = param[el];
            }
            // return $.param(request);
            return request;
        }
        // 分页函数
        function createPageBar(_total, _per, _pn) {
            var _per = _per ? _per : 10;    // 当前页显示条数
            var _pn = _pn ? _pn : 1;        // 当前为第几页
            if (_total <= 0) {
                return '';
            }
            var _pagetotal = Math.ceil(_total / _per);   //总共多少页
            var $str = '<div class="m-pageBar box"><span class="count">共<i>' + _total + '</i>条记录，分<i>' + Math.ceil(_total / _per) + '</i>页显示</span>';
            // 上一页
            if (_pn == 1) {
                $str += '<a href="javascript:;" class="disabled">上一页</a>';
            } else {
                $str += '<a href="javascript:;" data-pn="' + (_pn - 1) + '">上一页</a>';
            }
            // 如果当前页码靠近总数，总数在9以上，要在 6789页前追加 2345
            if (_pn > _pagetotal - 4 && _pn > 5) {
                var _beforenumber = 4 - (_pagetotal - _pn);
                for (var i = _pn - 4 - _beforenumber; i <= _pn - 5; i++) {
                    if (i > 0) {
                        $str += '<a href="javascript:;" data-pn="' + i + '">' + i + '</a>';
                    }
                }
            }
            //前4页
            for (var i = _pn - 4; i < _pn; i++) {
                if (i > 0) {
                    $str += '<a href="javascript:;" data-pn="' + i + '">' + i + '</a>';
                }
            }
            //当前页
            $str += '<a href="javascript:;" class="selected">' + _pn + '</a>';
            //后4页
            for (var i = _pn + 1; i <= _pn + 4; i++) {
                if (i <= _pagetotal) {
                    $str += '<a href="javascript:;" data-pn="' + i + '">' + i + '</a>';
                }
            }
            //如果当前小于5，但实际页码，在9以上，要在4之后加上56789 追加显示到第9页
            if (_pn < 5 && _pagetotal > _pn + 4) {
                for (var i = _pn + 5; i <= 9; i++) {
                    if (i <= _pagetotal) {
                        $str += '<a href="javascript:;" data-pn="' + i + '">' + i + '</a>';
                    }
                }
            }
            if (_pn < _pagetotal) {
                $str += '<a href="javascript:;" data-pn="' + (_pn + 1) + '">下一页</a>';
            } else {
                $str += '<a href="javascript:;" class="disabled">下一页</a>';
            }
            $str += '</div>';
            return $str;
        }
    }

    /**
     * 用途：创建 radio | checkbox | select 页面元素集
     * @param {string} [options.nodeID]         // 插入到页面位置的nodeID|为null返回HtmlString
     * @param {string} [options.type]           // 类型：radio | checkbox | select 
     * @param {string} [options.name]           // name值
     * @param {string} [options.id]             // select 可以附加ID
     * @param {boolean} [options.required]      // 是否需要必填必选 依赖于 T.registerFileds
     * @param {object} [options.model]          // 显示及值{text:"text",value:"value"} 
                                                // 数据模型比对[相当于设置 mode.text = data.text]
     * @param {object} [options.data]           // 必须符合对象数组 [{},{},{}]
     * @param {array} [options.customAttribute] // 将数据对象中的自定义属性附加在元素上
     */
    T.createrFormElement = function (options) {
        if (!$.isPlainObject(options)) {
            return;
        }
        var nodeBox = options.nodeID ? options.nodeID : null, HTML = '';
        if (options.type && options.data.length > 0 && options.model) {
            var test = options.required ? 'test="required"' : '';
            switch (options.type) {
                case "radio":
                    ;
                case "checkbox":
                    // 对象模型: test:options.data[i].test | value:options.data[i].value 
                    for (var i = 0, end = options.data.length; i < end; i++) {
                        // 拼接自定义属性
                        var attributeHtml = '';
                        if (options.customAttribute && options.customAttribute.length > 0) {
                            for (var k = 0, attEnd = options.customAttribute.length; k < attEnd; k++) {
                                attributeHtml += ' ' + options.customAttribute[k] + '="' + (options.data[i][options.customAttribute[k]] ? options.data[i][options.customAttribute[k]] : '') + '"';
                            };
                        }
                        HTML += '<label class="ui-label"><input type="' + options.type + '" name="' + options.name + '" value="' + options.data[i][options.model.value] + '" ' + test + attributeHtml + '><em>' + options.data[i][options.model.text] + '</em></label>';
                    };
                    break;
                case "select":
                    HTML = '<select name="money" id="' + (options.id ? options.id : '') + '" class="ui-select" ' + test + '><option value="">请选择</option>';
                    for (var i = 0, end = options.data.length; i < end; i++) {
                        // 拼接自定义属性
                        var attributeHtml = '';
                        if (options.customAttribute && options.customAttribute.length > 0) {
                            for (var k = 0, attEnd = options.customAttribute.length; k < attEnd; k++) {
                                attributeHtml += ' ' + options.customAttribute[k] + '="' + (options.data[i][options.customAttribute[k]] ? options.data[i][options.customAttribute[k]] : '') + '"';
                            };
                        }
                        HTML += '<option value="' + options.data[i][options.model.value] + '"' + attributeHtml + '>' + options.data[i][options.model.text] + '</option>';
                    };
                    HTML += '</select>';
                    break;
                default: break;
            }
            if (nodeBox) {
                $(nodeBox).append(HTML);
            } else {
                return HTML;
            }
        }
    }

    /**
     * 用途：为表单填充数据
     * @param {string} [rangeID] 文档节点范围指定在这个ID内部
     * @param {object} [fillParam] 后台数据模型
     * @param {function} [callback] 对fillParam对象的回调处理
     */
    T.fillForm = function (rangeID, fillParam, callback) {

        // $("#dialog_overlay").append("<div class='loading'></div>").show();
        var me = $(rangeID);
        me.each(function () {
            var els = $("input,textarea,select,span", me[0]);
            for (var i = 0, max = els.length; i < max; i++) {
                var sValue = fillParam[$(els[i]).attr("name")];
                // 略过数据空值
                if ((typeof sValue == 'undefined') || (typeof sValue == 'object') || sValue == '' || sValue == null) { continue; }

                // checkbox
                if (els[i].type && els[i].type == "checkbox") {
                    // 布尔传值
                    if (typeof (sValue) == "boolean") {
                        if (sValue) {
                            els[i].checked = true;
                        } else {
                            els[i].checked = false;
                        }
                    } else {
                        if (sValue.toString().indexOf(els[i].value.toString()) != -1) {
                            els[i].checked = true;
                        } else {
                            els[i].checked = false;
                        }
                    }
                    // radio
                } else if (els[i].type && els[i].type == "radio") {
                    // 布尔传值 或 数字传值
                    if (typeof (sValue) == "boolean") {
                        if (sValue) {
                            els[i].checked = true;
                        } else {
                            els[i].checked = false;
                        }
                    } else {
                        if (sValue.toString().indexOf(els[i].value.toString()) != -1) {
                            els[i].checked = true;
                        } else {
                            els[i].checked = false;
                        }
                    }
                    // span
                } else if (els[i].nodeName && els[i].nodeName.toLowerCase() == "span") {
                    $(els[i]).html(sValue);
                } else {
                    $(els[i]).val(sValue).trigger("change");
                }
            }
        });
        // 启用回调处理不能绑定的数据
        var t = setTimeout(function () {
            if ($.isFunction(callback)) {
                callback(fillParam);
            }
            t = null;
            // $("#dialog_overlay").html("").hide();
        }, 300);
    };

    /**
     * 用途：点击nodesID全选name="**"的input 
     * @param {string} [nodes] 事件绑定ID或className
     * @param {object} [options] 全选处理参数
     */
    T.checkAll = function (nodes, options) {
        var defaults = {
            type: 'all',                    // 全选-all; 反选-invert
            name: '',                       // 默认name
            callback: function () { }          // 回调函数
        };
        var opts = $.extend(defaults, options);
        // 取得选中的数组
        var getElArr = function (para) {
            var elArr = [];
            $('input[name="' + para.name + '"]').each(function () {
                if (this.checked) {
                    elArr.push(this.value);
                }
            });
            if (elArr && elArr.length > 0) {
                return elArr;
            } else {
                return [];
            }
        }
        // 是否全选中了
        var ischeckAll = function (o, para) {
            var isFalse = null;
            $('input[name="' + para.name + '"]').each(function (i) {
                if (this.checked == false) {
                    isFalse = true;
                    return false;
                }
            });
            // 遍历完后返回 true 则全选中
            if (isFalse) {
                o.checked = false;
            } else {
                o.checked = true;
            }
        }
        // 初始化
        var init = function (o) {
            $(o).on('click', function () {
                var check = this.checked;
                $('input[name="' + opts.name + '"]').each(function () {
                    // 全选/全不选
                    if (opts.type === 'all') {
                        this.checked = check;
                    }
                    // 反选
                    else {
                        if (this.checked) {
                            this.checked = false;
                        } else {
                            this.checked = true;
                        }
                    }
                });
                // 回调函数：返回所选的input dom 数组
                if (opts.callback) {
                    var elArrData = getElArr(opts);
                    opts.callback(elArrData);
                }
            });
            // 如果有一个没有选中，则去掉全选
            if (opts.type == 'all') {
                $("body").on('click', 'input[name="' + opts.name + '"]', function () {
                    if (this.checked == false) {
                        // 当前取消选中，则取消全选
                        o.checked = false;
                    } else {
                        ischeckAll(o, opts);
                    }
                });
            }
        };
        return $(nodes).each(function () {
            init(this);
        });
    };

    /**
     * 用途：获得选中的 checkbox
     * 注意：默认为input数组
     *       如果要得到其中属性数组，需传入属性名 | value | id | 其它属性数组 ]
     * @param {string} [name] checkbox的name值
     * @param {string} [attribute] checkbox的value、id、title或其它属性
     */
    T.getCheckedArr = function (name, attribute) {
        var elArr = [];
        $('input[name="' + name + '"]').each(function () {
            if (this.checked) {
                if (attribute) {
                    if (attribute == "value") {
                        elArr.push(this.value);
                    } else {
                        elArr.push($(this).attr(attribute));
                    }
                } else {
                    elArr.push(this);
                }
            }
        });
        if (elArr && elArr.length > 0) {
            return elArr;
        } else {
            return [];
        }
    }

    /**
     * 用途：弹层基础控件
     * @param {object} [options] 传参对象请对照内部默认参数
     */
    T.popup = function (options) {

        // 默认参数
        var defaults = {                        // 创建蒙板可更改参数
            width: 600,                         // 指定弹窗宽度
            height: 450,                        // 指定弹窗高度
            setTimeout: null,                    // 多少毫秒后自动关闭
            beforeShow: null,                   // 弹出前函数
            afterShow: null,                    // 弹出后函数
            overlayZindex: 99,                   // 遮罩层显示层级
            overlay: "popup-overlay",           // 遮罩class name
            showed: "popup-showed",             // 打开的记录
            closeEl: '.close',                  // 关闭class
            mTop: 'auto',                       // auto | 20px
            overlayBg: "#000",                  // 遮罩层背景色
            overlayOpacity: 5,                   // 遮罩层透明度 1~9 (0.1~0.9)
            docClick: true,                     // 点击弹窗之外时是否关闭
            type: null,                          // 弹层类型 alert | confirm | prompt | iframe | otherID
            title: 'null',                       // 提示标题 alert
            text: 'null',                        // 提示文字 alert | confirm
            promptValue: '',                     // prompt defaultText value
            url: null,                           // iframe url地址
            toTarget: null,                      // current 当前页面 | 'parent' 父窗口  | 'top' 顶级窗口
            moveHander: '.hd',                   // 移动弹层的绑定节点
            modeAlertHTML: '<div class="popup popAlert"><header class="hd">提示</header><section class="ct"></section><a href="javascript:;" class="close">×</a></div>',
            modeConfirmHTML: '<div class="popup popConfirm"><header class="hd">提示</header><section class="ct">说明文字提示区</section><footer class="ft"><a class="btnTrue" href="javascript:;">确定</a>&nbsp;&nbsp;&nbsp;&nbsp;<a class="btnFalse popup-close" href="javascript:;">取消</a></footer><a href="javascript:;" class="close">×</a></div>',
            modePromptHTML: '<div class="popup popPromp"><header class="hd">提示</header><section class="ct"><h1>说明文字提示区</h1><p><input type="text" value=""></p></section><footer class="ft"><a class="btnTrue" href="javascript:;">确定</a>&nbsp;&nbsp;&nbsp;&nbsp;<a class="btnFalse popup-close" href="javascript:;">取消</a></footer><a href="javascript:;" class="close">×</a></div>',
            modeIframeHTML: '<div class="popup popIframe"><header class="hd">提示标题</header><section class="ct"><iframe src="#" frameborder="0" style="background:#fff;"></iframe></section><a href="javascript:;" class="close">×</a></div>',
            baseCssLink: 'http://base.ccdfs.cc/manager/css/public/m-popup.css',
            htmlString: '',                      // 弹层ID不存在 则以此启用创建弹层
            htmlID: null,                        // 弹层ID
            htmlCssFlag: null,                   // 附加 css block style ID
            htmlCssLink: null                    // 附加 cssText link
        };
        var $pop, $mengban, oLeft, oTop, moveWidth, moveHeight, docHeight,
            hasTop = window.parent.length > 0 ? true : false,
            opts = $.extend(defaults, options);

        // 关闭弹层
        var hidepop = function () {
            if ($pop && $mengban) {
                $mengban.off("dblclick");
                $pop.remove();
                $mengban.remove();
                if (opts.toTarget == "parent") {
                    $(window.parent).off("resize", resizeWin);
                } else if (opts.toTarget == "top") {
                    $(window.top).off("resize", resizeWin);
                } else if (opts.toTarget == "current") {
                    $(window).off("resize", resizeWin);
                } else {
                    if (hasTop) {
                        $(window.top).off("resize", resizeWin);
                    } else {
                        $(window).off("resize", resizeWin);
                    }
                }
                // 执行弹窗后事件函数
                if (opts.afterShow) { opts.afterShow($pop); }
            }
        };

        // 关闭所有弹层
        var hideAllpop = function () {
            // $pop.remove();
            // $mengban.remove();
            if (opts.toTarget == "parent") {
                window.parent.$("." + opts.showed).remove();
                window.parent.$("." + opts.overlay).remove();
                $(window.parent).off("resize", resizeWin);
            } else if (opts.toTarget == "top") {
                window.top.$("." + opts.showed).remove();
                window.top.$("." + opts.overlay).remove();
                $(window.top).off("resize", resizeWin);
            } else if (opts.toTarget == "current") {
                $("." + opts.showed).remove();
                $("." + opts.overlay).remove();
                $(window).off("resize", resizeWin);
            } else {
                if (hasTop) {
                    window.top.$("." + opts.showed).remove();
                    window.top.$("." + opts.overlay).remove();
                    $(window.top).off("resize", resizeWin);
                } else {
                    $("." + opts.showed).remove();
                    $("." + opts.overlay).remove();
                    $(window).off("resize", resizeWin);
                }
            }
            if ($pop && $mengban) {
                $mengban.off("dblclick");
                // 执行弹窗后事件函数
                if (opts.afterShow) { opts.afterShow($pop); }
            }
        };

        // 窗口变化
        var resizeWin = function () {
            // 是否有滚动条出现 真实网页高度
            /*
            var mh ;
            var win_h = $(window).height();
            var doc_h = $(document).height();
            if ( doc_h > win_h ){
                mh = Math.floor(doc_h + 16);
            }else {
                mh = doc_h;
            }
            */
            // 定位数值
            // var scr_height = $(document).scrollTop();   //  距顶部高度
            if (opts.toTarget == "parent") {
                oLeft = Math.floor(($(window.parent).width() - opts.width) / 2);
                oTop = Math.floor(($(window.parent).height() - opts.height) / 2);
                moveWidth = $(window.parent).width();
                moveHeight = $(window.parent).height();
                docHeight = $(window.parent.document).height();
            } else if (opts.toTarget == "top") {
                oLeft = Math.floor(($(window.top).width() - opts.width) / 2);
                oTop = Math.floor(($(window.top).height() - opts.height) / 2);
                moveWidth = $(window.top).width();
                moveHeight = $(window.top).height();
                docHeight = $(window.top.document).height();
            } else if (opts.toTarget == "current") {
                oLeft = Math.floor(($(window).width() - opts.width) / 2);
                oTop = Math.floor(($(window).height() - opts.height) / 2);
                moveWidth = $(window).width();
                moveHeight = $(window).height();
                docHeight = $(document).height();
            } else {
                if (hasTop) {
                    oLeft = Math.floor(($(window.top).width() - opts.width) / 2);
                    oTop = Math.floor(($(window.top).height() - opts.height) / 2);
                    moveWidth = $(window.top).width();
                    moveHeight = $(window.top).height();
                    docHeight = $(window.top.document).height();
                } else {
                    oLeft = Math.floor(($(window).width() - opts.width) / 2);
                    oTop = Math.floor(($(window).height() - opts.height) / 2);
                    moveWidth = $(window).width();
                    moveHeight = $(window).height();
                    docHeight = $(document).height();
                }
            }
            // 弹层位置
            $pop.css({
                'position': 'fixed',         // IE6 position: absolute; background: #fff;
                'width': opts.width,
                'height': opts.height,
                'left': oLeft,
                "top": oTop
            });
            if (opts.mTop != "auto") {
                $pop.css({
                    "top": opts.mTop
                });
            }

            if ($mengban) {
                $mengban.css({
                    'width': '100%',
                    'height': docHeight
                });
            }
        }

        // 创建蒙板
        var createMengban = function (toTarget) {
            // 添加蒙板
            function mb(isHasMengban, mengbanHeight) {
                // 是否创建透明蒙板
                var opacityHTML = isHasMengban ? 'opacity:0;filter:alpha(opacity=0);' : 'opacity:0.' + opts.overlayOpacity + ';filter:alpha(opacity=' + opts.overlayOpacity + '0);';
                // 判断是否已经有蒙版
                var mengbanHTML = $('<div class="' + opts.overlay + '" style="display:none;position:absolute;left:0px; top:0px;' + opacityHTML + 'width:100%; height:' + mengbanHeight + 'px; background:' + opts.overlayBg + '; z-index:' + opts.overlayZindex + ';"></div>');
                return mengbanHTML;
            }
            // 分支处理
            if (toTarget == "parent") {
                var isHasMengban = window.parent.$("body").find('.' + opts.showed).length > 0 ? true : false;
                moveWidth = $(window.parent).width();
                moveHeight = $(window.parent).height();
                docHeight = $(window.parent.document).height();
                $mengban = mb(isHasMengban, docHeight);
                window.parent.$("body").append($mengban);
            } else if (toTarget == "top") {
                var isHasMengban = window.top.$("body").find('.' + opts.showed).length > 0 ? true : false;
                moveWidth = $(window.top).width();
                moveHeight = $(window.top).height();
                docHeight = $(window.top.document).height();
                $mengban = mb(isHasMengban, docHeight);
                window.top.$("body").append($mengban);
            } else if (toTarget == "current") {
                var isHasMengban = $("body").find('.' + opts.showed).length > 0 ? true : false;
                moveWidth = $(window).width();
                moveHeight = $(window).height();
                docHeight = $(document).height();
                $mengban = mb(isHasMengban, docHeight);
                $("body").append($mengban);
            } else {
                if (hasTop) {
                    var isHasMengban = window.top.$("body").find('.' + opts.showed).length > 0 ? true : false;
                    moveWidth = $(window.top).width();
                    moveHeight = $(window.top).height();
                    docHeight = $(window.top.document).height();
                    $mengban = mb(isHasMengban, docHeight);
                    window.top.$("body").append($mengban);
                } else {
                    var isHasMengban = $("body").find('.' + opts.showed).length > 0 ? true : false;
                    moveWidth = $(window).width();
                    moveHeight = $(window).height();
                    docHeight = $(document).height();
                    $mengban = mb(isHasMengban, docHeight);
                    $("body").append($mengban);
                }
            }
        }

        // 入口函数
        var init = function () {

            // 弹层内容预处理
            var modeHTML = '';
            switch (opts.type) {
                case 'alert':
                    $pop = $(opts.modeAlertHTML);
                    $pop.find(".ct").html(opts.text);
                    break;
                case 'confirm':
                    $pop = $(opts.modeConfirmHTML);
                    $pop.confirmCallback = null;
                    $pop.find(".btnTrue").on("click", function () {
                        $pop.confirmCallback = true;
                        hidepop();
                    });
                    $pop.find(".btnFalse").on("click", function () {
                        $pop.confirmCallback = false;
                        hidepop();
                    });
                    $pop.find(".ct").html(opts.text);
                    break;
                case 'prompt':
                    $pop = $(opts.modePromptHTML);
                    $pop.find(".ct h1").html(opts.text);
                    $pop.find(".ct input").val(opts.promptValue);
                    $pop.promptValue = null;
                    $pop.find(".btnTrue").on("click", function () {
                        $pop.promptValue = $pop.find("input").val();
                        hidepop();
                    });
                    $pop.find(".btnFalse").on("click", function () {
                        $pop.promptValue = false;
                        hidepop();
                    });
                    break;
                case 'iframe':
                    $pop = $(opts.modeIframeHTML);
                    if (opts.iframeid) {
                        $pop.attr("id", opts.iframeid);
                    }
                    $pop.find(".hd").html(opts.title);
                    $pop.find("iframe").css({ "width": opts.width, "height": opts.iframeHeight });
                    $pop.find("iframe").attr("src", opts.url);
                    break;
                default:
                    var popContent = opts.htmlID ? $(opts.htmlID).html() : opts.htmlString;
                    if (popContent != '') {
                        $pop = $(popContent);
                    } else {
                        return;
                    }
            }

            // 弹窗前预置函数
            if (opts.beforeShow) { opts.beforeShow($pop); }

            // 创建蒙板
            createMengban(opts.toTarget);
            // 弹层内容
            if (opts.toTarget == "parent") {
                $pop.appendTo(window.parent.document.body);
                // 弹窗定位数值计算
                oLeft = Math.floor(($(window.parent).width() - opts.width) / 2);
                oTop = Math.floor(($(window.parent).height() - opts.height) / 2);
            } else if (opts.toTarget == "top") {
                $pop.appendTo(window.top.document.body);
                // 弹窗定位数值计算
                oLeft = Math.floor(($(window.top).width() - opts.width) / 2);
                oTop = Math.floor(($(window.top).height() - opts.height) / 2);
            } else if (opts.toTarget == "current") {
                $pop.appendTo("body");
                // 弹窗定位数值计算
                oLeft = Math.floor(($(window).width() - opts.width) / 2);
                oTop = Math.floor(($(window).height() - opts.height) / 2);
            } else {
                if (hasTop) {
                    $pop.appendTo(window.top.document.body);
                    // 弹窗定位数值计算
                    oLeft = Math.floor(($(window.top).width() - opts.width) / 2);
                    oTop = Math.floor(($(window.top).height() - opts.height) / 2);
                } else {
                    $pop.appendTo("body");
                    // 弹窗定位数值计算
                    oLeft = Math.floor(($(window).width() - opts.width) / 2);
                    oTop = Math.floor(($(window).height() - opts.height) / 2);
                }
            }

            // 是否有滚动条出现 真实网页高度
            /*
            var mh ;
            var win_h = $(window).height();
            var doc_h = $(document).height();
            if ( doc_h > win_h ){
                mh = Math.floor(doc_h + 16);
            }else {
                mh = doc_h;
            }
            */

            // 弹窗定位赋值
            $pop.css({
                'position': 'fixed',         // IE6 position: absolute; background: #fff;
                'width': opts.width,
                'height': opts.height,
                'left': oLeft,
                "top": oTop,
                "z-index": opts.overlayZindex + 1
            });

            // 如果有传入顶部高度
            if (opts.mTop != "auto") {
                $pop.css({
                    "top": opts.mTop
                });
            }

            // IE 系列判断 并执行下面函数
            /*
            if(navigator.userAgent.indexOf("MSIE 6.0") > 0){ 
                $pop.css({
                    "position": "absolute",
                    "top": _top + scr_height
                });
            }
            */

            // 显示蒙板
            $mengban.fadeIn("fast");
            // 显示弹层
            $pop.addClass(opts.showed).css("display", "block");

            // 移动弹层
            $pop.find(opts.moveHander).on("mousedown", function (event) {
                // 移动开始
                var startX = event.pageX || window.event.pageX,
                    startY = event.pageY || window.event.pageY,
                    offset = $pop.offset();
                var disX = startX - offset.left;
                var disY = startY - offset.top;
                // console.log(moveWidth);
                // console.log(moveHeight);
                // 禁止冒泡
                if (event.stopPropagation) {
                    event.stopPropagation();    //其它
                } else {
                    event.cancelBubble = true;//IE
                }
                $pop.closest("body").on("mousemove", function (event) {
                    // 正在移动
                    var _x = event.pageX || window.event.pageX,
                        _y = event.pageY || window.event.pageY;
                    // 边界锁定范围
                    if (_x - disX < 0 || _y - disY < 0 || _x - disX > moveWidth - opts.width || _y - disY > moveHeight - opts.height) {
                        return;
                    }
                    $pop.css({
                        "left": _x - disX,
                        "top": _y - disY,
                        "cursor": "move"
                    });
                }).on("mouseup", function () {
                    // 移动结束 
                    $pop.css({
                        "cursor": "default"
                    });
                    $pop.closest("body").off('mousemove');
                });
            });

            // 关闭事件
            $pop.find(opts.closeEl).on('click', function () {
                hidepop();
                return false;
            });

            // 双击蒙板事件
            if (opts.docClick) {
                $mengban.on('dblclick', hideAllpop);
            }

            // 窗口变化事件
            if (opts.toTarget == "parent") {
                $(window.parent).on("resize", resizeWin);
            } else if (opts.toTarget == "top") {
                $(window.top).on("resize", resizeWin);
            } else if (opts.toTarget == "current") {
                $(window).on("resize", resizeWin);
            } else {
                if (hasTop) {
                    $(window.top).on("resize", resizeWin);
                } else {
                    $(window).on("resize", resizeWin);
                }
            }

            // 多少毫秒后自动关闭弹窗
            if (opts.setTimeout > 10) {
                window.setTimeout(function () {
                    hidepop();
                    return false;
                }, opts.setTimeout);
            }

        };

        // 提前预加载 基础样式
        if (opts.baseCssLink) {
            if (opts.toTarget == "parent") {
                if (!window.parent.customPopupCssLink) {
                    window.parent.customPopupCssLink = true;
                    T.addCssLink(opts.baseCssLink, 'customPopupCssLink', "parent");     // 预加载样式
                }
            } else if (opts.toTarget == "top") {
                if (!window.top.customPopupCssLink) {
                    window.top.customPopupCssLink = true;
                    T.addCssLink(opts.baseCssLink, 'customPopupCssLink', "parent");     // 预加载样式
                }
            } else if (opts.toTarget == "current") {
                if (!window.customPopupCssLink) {
                    window.customPopupCssLink = true;
                    T.addCssLink(opts.baseCssLink, 'customPopupCssLink');             // 预加载样式
                }
            } else {
                if (hasTop) {
                    if (!window.top.customPopupCssLink) {
                        window.top.customPopupCssLink = true;
                        T.addCssLink(opts.baseCssLink, 'customPopupCssLink', "parent");     // 预加载样式
                    }
                } else {
                    if (!window.customPopupCssLink) {
                        window.customPopupCssLink = true;
                        T.addCssLink(opts.baseCssLink, 'customPopupCssLink');             // 预加载样式
                    }
                }
            }
        }

        // 提前预加载 附加样式
        if (opts.htmlCssFlag && opts.htmlCssLink) {
            if (opts.toTarget == "parent") {
                if (!window.parent[opts.htmlCssFlag]) {
                    window.parent[opts.htmlCssFlag] = true;
                    T.addCssLink(opts.htmlCssLink, opts.htmlCssFlag, "parent");     // 预加载样式
                }
            } else if (opts.toTarget == "top") {
                if (!window.top[opts.htmlCssFlag]) {
                    window.top[opts.htmlCssFlag] = true;
                    T.addCssLink(opts.htmlCssLink, opts.htmlCssFlag, "parent");     // 预加载样式
                }
            } else if (opts.toTarget == "current") {
                if (!window[opts.htmlCssFlag + "_flag"]) {
                    window[opts.htmlCssFlag + "_flag"] = true;
                    T.addCssLink(opts.htmlCssLink, opts.htmlCssFlag);             // 预加载样式
                }
            } else {
                if (hasTop) {
                    if (!window.top[opts.htmlCssFlag]) {
                        window.top[opts.htmlCssFlag] = true;
                        T.addCssLink(opts.htmlCssLink, opts.htmlCssFlag, "parent");     // 预加载样式
                    }
                } else {
                    if (!window[opts.htmlCssFlag + "_flag"]) {
                        window[opts.htmlCssFlag + "_flag"] = true;
                        T.addCssLink(opts.htmlCssLink, opts.htmlCssFlag);             // 预加载样式
                    }
                }
            }
        }

        // 初始化入口函数
        init();

    };

    /**
     * 用途：普通提示
     * @param {string} [message] 提示文字
     * @param {number} [time] N毫秒后自动关闭
     * @param {function} [callback] 关闭后的回调函数
     */
    T.alert = function (message, time, callback) {
        T.popup({
            type: 'alert',
            width: 360,
            height: 160,
            text: message,
            setTimeout: (time ? time : null),
            afterShow: function () {
                if (callback) {
                    callback();
                }
            }
        });
    }

    /**
     * 用途：二次确认提示
     * @param {string} [message] 提示文字
     * @param {function} [callback(boolean)] 回调处理
     */
    T.confirm = function (message, callback) {
        T.popup({
            type: 'confirm',
            width: 360,
            height: 190,
            text: message,
            afterShow: function (o) {
                if (callback) {
                    callback(o.confirmCallback);
                }
            }
        });
    }

    /**
     * 用途：获取输入文字
     * @param {string} [text] 标题文字
     * @param {string} [defaultText] 默认值
     * @param {function} [callback(inputText)] 回调处理
     */
    T.prompt = function (text, defaultText, callback) {
        T.popup({
            type: 'prompt',
            width: 360,
            height: 240,
            text: text,
            promptValue: (defaultText ? defaultText : ''),
            afterShow: function (o) {
                if (callback) {
                    callback(o.promptValue);
                }
            }
        });
    }

    /**
     * 用途：打开新窗口链接
     * @param {object} [options] 参数说明
     * @param {number} [options.popId] 弹出层ID 用来操作关闭，及交互
     * @param {string} [options.title] 标题文字
     * @param {number} [options.width] 宽度
     * @param {number} [options.height] 高度
     * @param {string} [options.url] 地址
     * @param {number} [options.setTimeout] 多少毫秒后可以关闭
     * @param {function} [options.beforeShow] 弹出前执行函数
     * @param {function} [options.afterShow] 关闭后执行函数
     */
    T.openIframe = function (options) {
        if (options.url) {
            T.popup({
                type: 'iframe',
                toTarget: options.toTarget ? options.toTarget : null,
                iframeid: (options.id ? options.id : null),
                title: (options.title ? options.title : '新建文档'),
                width: (options.width ? options.width : 800),
                height: (options.height ? (options.height + 40) : 540),
                iframeHeight: (options.height ? options.height : 500),
                url: options.url + (options.url.indexOf('?') < 0 ? '?rand=' : '&rand=') + (new Date().getTime()),
                setTimeout: options.setTimeout,
                beforeShow: options.beforeShow,
                afterShow: options.afterShow
            });
        } else {
            T.alert("参数中缺少请求地址！");
        }
    }

    // 截取年月日 2013-02-04 15:01:55.910 => 2013-02-04
    T.parseTimeString = function (timeString, type) {
        if (type == 'YMD' || type == 'ymd') {
            return timeString.substring(0, 10);
        }
    }


}(window, document));