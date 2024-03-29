/*
NumBox jQuery Plugin v1.1.0 (C) Copyright 2012-2014 TimeApps.com.
See http://www.numbox.org/ for API & licensing.
*/
(function (d) {
    function k(b, a) {
        try {
            b.type = a
        } catch (d) { }
    }
    var g = "undefined" !== typeof window.orientation,
        n = /; Android 2\./.test(navigator.userAgent),
        l = -1 < Object.prototype.toString.call(window.HTMLElement).indexOf("Constructor"),
        f = {
            setup: function (b) {
                "undefined" === typeof b && (b = {});
                var a = {
                    type: "currency",
                    places: 2,
                    min: 0,
                    max: 999999.99,
                    fixedLength: !1,
                    step: "min",
                    symbol: "$",
                    location: "l",
                    separator: ",",
                    grouping: 3,
                    autoScroll: g,
                    onFocus: "",
                    onBlur: "",
                    onKeydown: "",
                    onInput: "",
                    onPaste: ""
                };
                if ("undefined" !== typeof b.type) switch (b.type.toLowerCase()) {
                    case "ccard":
                        a.places =
                        0;
                        a.max = "9999999999999999";
                        a.fixedLength = !0;
                        a.step = "1";
                        a.symbol = "";
                        a.separator = "-";
                        a.grouping = 4;
                        break;
                    case "ccard-15":
                        a.places = 0;
                        a.max = "999999999999999";
                        a.fixedLength = !0;
                        a.step = "1";
                        a.symbol = "";
                        a.separator = "-";
                        break;
                    case "ccard-16":
                        a.places = 0;
                        a.max = "9999999999999999";
                        a.fixedLength = !0;
                        a.step = "1";
                        a.symbol = "";
                        a.separator = "-";
                        a.grouping = 4;
                        break;
                    case "decimal":
                        a.symbol = "";
                        break;
                    case "ein":
                        a.places = 0;
                        a.max = "999999999";
                        a.fixedLength = !0;
                        a.step = "1";
                        a.symbol = "";
                        a.separator = "-";
                        break;
                    case "integer":
                        a.places =
                        0;
                        a.step = "1";
                        a.symbol = "";
                        break;
                    case "percent":
                        a.places = 0;
                        a.max = 100;
                        a.step = "1";
                        a.symbol = "%";
                        a.location = "r";
                        break;
                    case "ssn":
                        a.places = 0;
                        a.max = "999999999";
                        a.fixedLength = !0;
                        a.step = "1";
                        a.symbol = "";
                        a.separator = "-";
                        break;
                    case "zip":
                    case "zip5":
                        a.places = 0;
                        a.max = "99999";
                        a.fixedLength = !0;
                        a.step = "1";
                        a.symbol = "";
                        break;
                    case "zip9":
                        a.places = 0, a.max = "999999999", a.fixedLength = !0, a.step = "1", a.symbol = "", a.separator = "-"
                }
                a = d.extend(a, b);
                a.type = a.type.toLowerCase();
                "min" == a.step.toLowerCase() && (a.step = 0 == a.places ? 1 : 1 /
                    Math.pow(10, a.places));
                "auto" == a.autoScroll.toString().toLocaleLowerCase() && (a.autoScroll = g);
                this.data("type", a.type);
                this.data("places", a.places);
                //g && l ? 1 > a.places ? this.attr("pattern", "[0-9]*") : this.removeAttr("pattern") : this.attr("pattern", 1 > a.places ? "^[0-9]*$" : "^[0-9]*([0-9]+[.]?[0-9]*)?$");
                this.attr("min", a.min);
                this.attr("max", a.max);
                this.data("fixed-length", a.fixedLength.toString().toLowerCase());
                this.attr("step", a.step);
                this.data("symbol", a.symbol);
                this.data("location", a.location);
                this.data("separator",
                    a.separator);
                this.data("grouping", a.grouping);
                this.data("autoscroll", a.autoScroll.toString().toLowerCase());
                this.data("onfocus", a.onFocus);
                this.data("onblur", a.onBlur);
                this.data("onkeydown", a.onKeydown);
                this.data("oninput", a.onInput);
                this.data("onpaste", a.onPaste);
                this.off("focus").on("focus", function (a) {
                    var b = d(this).data("onfocus");
                    this.value = f.getRaw(this);
                    l && !g || n || k(this, "text");
                    "setSelectionRange" in this && this.setSelectionRange(this.value.length, this.value.length);
                    if ("createTextRange" in this) {
                        var c =
                            this.createTextRange();
                        c.moveStart("character", this.value.length);
                        c.collapse();
                        c.select()
                    }
                    0 < b.length && (new Function(b + "(event)"))(a);
                    "true" == d(this).data("autoscroll") && setTimeout(function () {
                        var a = d('div[data-role="content"]');
                        0 == a.length && (a = d("#content"));
                        1 == a.length && window.scrollTo(0, d("input:focus").offset().top - d(a).offset().top - 45)
                    }, 250)
                });
                this.off("blur").on("blur", function (a) {
                    var b = d(this).data("onblur");
                    k(this, "text");
                    this.value = f.getFormatted(this);
                    d(this).hasClass("NumBox") && 0 < b.length &&
                        (new Function(b + "(event)"))(a)
                });
                this.off("keydown").on("keydown", function (a) {
                    var b = d(this).data("onkeydown"),
                        c = window.event ? a.which : a.keyCode,
                        e = !1;
                    if (16 == c || 17 == c) e = !0;
                    a.shiftKey && !a.altKey && (37 <= c && 40 >= c || -1 < d.inArray(c, [8, 9, 13, 27, 35, 36, 45, 46, 144])) && (e = !0);
                    a.ctrlKey && !a.shiftKey && !a.altKey && (37 <= c && 40 >= c || -1 < d.inArray(c, [8, 9, 13, 27, 35, 36, 45, 46, 67, 144])) && (e = !0);
                    a.ctrlKey || a.shiftKey || a.altKey || !(48 <= c && 57 >= c || 37 <= c && 40 >= c || 96 <= c && 105 >= c || 112 <= c && 123 >= c || 109 == c || 189 == c || -1 < d.inArray(c, [8, 9, 13, 27, 35, 36, 45, 46, 144, 190, 110])) || (e = !0);
                    190 == c && -1 < this.value.indexOf(".") && (e = !1);
                    (109 == c || 189 == c) && (-1 < this.value.indexOf("-") || -1 < parseFloat(d(this).attr("min"))) && (e = !1);
                    if (!e) return a.preventDefault(), a.stopPropagation(), !1;
                    13 == c ? (a.preventDefault(), a.stopPropagation(), d(this).blur()) : (d(this).data("last-value", this.value), d(this).data("this-press", c), "selectionEnd" in this ? d(this).data("last-caret", this.selectionEnd) : (c = document.selection.createRange().duplicate(), c.moveStart("character", 0 - this.value.length), d(this).data("last-caret", c.text.length)));
                    0 < b.length && (new Function(b + "(event)"))(a)
                });
                this.off("input propertychange").on("input propertychange",
                    function (a) {
                        if (null != d(this).data("last-value")) {
                            a.stopImmediatePropagation();
                            a.preventDefault();
                            var b = d(this).data("last-value");
                            d(this).removeData("last-value");
                            var c = d(this),
                                e = !1,
                                f = !1,
                                g = !1,
                                k = parseInt("0" + c.data("last-caret")),
                                l = c.val().match(/\./g),
                                m = d(this).data("oninput");
                            this.validity && (this.validity.valid || (e = !0));
                            "0" == d(c).data("places") ? !1 == /^[0-9]+$/.test(c.val()) && (e = !0) : (!1 == /^[\-]?[0-9]*[\.]?[0-9]*$/.test(c.val()) && (e = !0), e || null == l || (1 < l.length ? e = !0 : c.val().indexOf(".") < c.val().length - 1 -
                                c.data("places") && (e = !0)));
                            if ((isNaN(c.val()) && "-" != c.val()) || "" == c.val() || "." == c.val()) e = !0;
                            e || (parseFloat(c.val()) < parseFloat(d(c).attr("min")) && (e = !0), parseFloat(c.val()) > parseFloat(d(c).attr("max")) && (e = !0));
                            if (!e && "true" == d(c).data("fixed-length")) switch (c.val().length > d(c).attr("max").length && (e = !0), d(c).data("type")) {
                                case "ccard-15":
                                    1 == c.val().length ? "3" != c.val().substr(0, 1) && (f = !0) : "34" != c.val().substr(0, 2) && "37" != c.val().substr(0, 2) && (f = !0);
                                    break;
                                case "ccard-16":
                                    1 == c.val().length ? "4" != c.val().substr(0, 1) &&
                                    "5" != c.val().substr(0, 1) && (f = !0) : "4" != c.val().substr(0, 1) && ("51" > c.val().substr(0, 2) || "55" < c.val().substr(0, 2)) && (f = !0)
                            }
                            f || e && -1 == d.inArray(d(c).data("this-press"), [8, 46]) ? (c.val(b), "setSelectionRange" in this && this.setSelectionRange(k, k), "createTextRange" in this && (b = this.createTextRange(), b.moveStart("character", k), b.collapse(), b.select())) : g = !0;
                            d(c).removeData("last-caret");
                            d(c).removeData("this-press");
                            0 < m.length && (new Function(m + "(event, " + g.toString() + ")"))(a, g)
                        }
                    });
                this.off("paste").on("paste",
                    function (a) {
                        var b = d(this).data("onpaste");
                        alert(b)
                        a.preventDefault();
                        a.stopPropagation();
                        d(this).hasClass("NumBox") && 0 < b.length && (new Function(b + "(event)"))(a);
                        return !1
                    });
                this.each(function (a) {
                    this.value = f.getRaw(this)
                });
                this.blur();
                this.removeClass("NumBox").addClass("NumBox");
                return this
            },
            destroy: function () {
                this.each(function () {
                    d(this).off("focus blur keydown input propertychange paste");
                    this.value = f.getRaw(this);
                    k(this, "number");
                    d(this).removeData("type places fixed-length symbol location separator grouping autoscroll onfocus onblur onkeydown oninput onpropertychange onpaste");
                    d(this).removeClass("NumBox")
                });
                return this
            },
            getFormatted: function (b) {
                var a = b;
                void 0 === a && (a = this[0]);
                //var h = void 0 === b ? f.getRaw(a) : a.value;
                var h = f.getRaw(a);
                b = "";
                var g = parseInt(d(a).data("grouping")) || 3,
                    c = d(a).data("separator");
                if ("true" != d(a).data("fixed-length")) {
                    var ng = "";
                    if (h.toString().substr(0, 1) == "-" && !isNaN(h)) {
                        if (parseFloat("0" + h.toString().substr(1)) == 0) {
                            h = "-";
                        } else {
                            h = h.toString().substr(1);
                            ng = "-";
                        }
                    }
                    h = parseFloat("0" + h).toFixed(Number(d(a).data("places"))).split(".");
                    h[0] = h[0].split("").reverse().join("");
                    for (var e = 1; e < h[0].length + 1; e++) b += h[0].charAt(e - 1), 0 == e % g && e != h[0].length && (b += c);
                    b = b.split("").reverse().join("");
                    b = ng + b;
                    0 < Number(d(a).data("places")) &&
                        (b += "." + h[1])
                } else
                    for (e = 1; e < h.length + 1; e++) switch (b += h.charAt(e - 1), d(a).data("type")) {
                        case "ccard-15":
-1 < d.inArray(e, [4, 10]) && (b += c);
                            break;
                        case "ein":
-1 < d.inArray(e, [2]) && (b += c);
                            break;
                        case "ssn":
-1 < d.inArray(e, [3, 5]) && (b += c);
                            break;
                        case "zip":
                        case "zip5":
                            break;
                        case "zip9":
-1 < d.inArray(e, [5]) && (b += c);
                            break;
                        default:
                            0 == e % g && e != h.length && (b += c)
                    }
                return b = "r" == d(a).data("location") ? b + d(a).data("symbol") : d(a).data("symbol") + b
            },
            getRaw: function (b) {
                void 0 === b && (b = this[0]);
                var a = b.value,
                    f = d(b).data("symbol"),
                    g = d(b).data("separator");
                0 < f.length && (a = a.replace(f, ""));
                0 < g.length && (a = a.replace(RegExp(g, "g"), ""));
                0 == Number(a) && "true" != d(b).data("fixed-length") ? a = "" : "true" != d(b).data("fixed-length") && (a = parseFloat(a));
                return a
            },
            setRaw: function (b) {
                this.each(function (a) {
                    this.value = b;
                    this.value = f.getFormatted(this)
                });
                return this
            },
            autoScroll: function (b) {
                "auto" == b.toString().toLowerCase() && (b = g);
                this.each(function () {
                    d(this).data("autoscroll", (!0 == b).toString().toLowerCase())
                });
                return this
            },
            onFocus: function (b) {
                this.data("onfocus",
                    b.toString());
                return this
            },
            onBlur: function (b) {
                this.data("onblur", b.toString());
                return this
            },
            onKeydown: function (b) {
                this.data("onkeydown", b.toString());
                return this
            },
            onInput: function (b) {
                this.data("oninput", b.toString());
                return this
            },
            onPaste: function (b) {
                this.data("onpaste", b.toString());
                return this
            }
        };
    d.fn.NumBox = function (b) {
        if (b && "object" !== typeof b) {
            if (f[b]) return f[b].apply(this, Array.prototype.slice.call(arguments, 1));
            d.error("NumBox does not have a " + b + " method.");
            return this
        }
        return f.setup.apply(this,
            arguments)
    }
})(jQuery);