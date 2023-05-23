
let tdesign = {
    /**
     * 为指定的元素订阅js事件，该方法可支持保留原有的事件不被覆盖，并支持参数指定强制覆盖。
     * @param {Element} element 要注册的元素
     * @param {String} name 事件名称，如 "onscroll"
     * @param {Function} handler 事件方法
     * @param {Boolean} replace 是否覆盖原有事件，默认为false，需要覆盖时传入true
     */
    setEvent: function(element, name, handler, replace = false) {
        let functionStored = element[name];
        element[name] = function(e) {
            if (!replace && functionStored) {
                functionStored(e)
            }
            handler(e);
        }
    },
    focus: function (element,dotnetHelper) {
        if (element) {
            element.focus();
            if (dotnetHelper) {
                dotnetHelper.invokeMethodAsync("Invoke");
            }
        }
    }
}
///**
// * @description 组件 affix 用到的js对象。
// */
//let affix = {
//    /**
//     * 组件初始化方法，用于给指定的元素绑定onscroll事件。
//     * @function affix.init
//     * @param {String} container HTML元素id，如果为空则使用document.body 
//     * @param {DotNetObjectReference<TAffix>} dotnetRef
//     */
//    init: function (container, dotnetRef) {
//        let el = container ? document.getElementById(container) : document.body
//        let scroll = function () {
//            let boundingClientRect = el.getBoundingClientRect()
//            let containerScrollTop = el.scrollTop
//            let containerY = parseInt(boundingClientRect.y)
//            let containerHeight = el.clientHeight
//            dotnetRef.invokeMethodAsync("OnScrollChanged", containerScrollTop, containerY, containerHeight)
//        }
//        tdesign.setEvent(el, "onscroll", scroll);
//    },
//    /**
//     * 获取组件当前位置距离窗口顶端的高度值，offsetTop
//     * @param {String} 组件的元素id
//     * @return {Number} offsetTop
//     * */
//    positionY: function (id) {
//        return parseInt(document.getElementById(id).getBoundingClientRect().y);
//    },
//}





/**
 * 暗黑模式
 * */
let theme = {
    /**
     * 浅色模式
     * */
    light: function () {
        document.documentElement.removeAttribute('theme-mode');
    },
    /**
     * 暗色模式
     * */
    dark: function () {
        document.documentElement.setAttribute('theme-mode', 'dark');
    }
}

export { tdesign, affix, anchor, theme, tagInput }
