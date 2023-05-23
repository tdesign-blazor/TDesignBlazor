let util = {
    /**
     * 为指定的元素订阅js事件，该方法可支持保留原有的事件不被覆盖，并支持参数指定强制覆盖。
     * @param {Element} element 要注册的元素
     * @param {String} name 事件名称，如 "onscroll"
     * @param {Function} handler 事件方法
     * @param {Boolean} replace 是否覆盖原有事件，默认为false，需要覆盖时传入true
     */
    setEvent: function (element, name, handler, replace = false) {
        let functionStored = element[name];
        element[name] = function (e) {
            if (!replace && functionStored) {
                functionStored(e)
            }
            handler(e);
        }
    },
    focus: function (element, dotnetHelper) {
        if (element) {
            element.focus();
            if (dotnetHelper) {
                dotnetHelper.invokeMethodAsync("Invoke");
            }
        }
    }
}
export { util }