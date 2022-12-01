/**
 * @description 组件 affix 用到的js对象。
 */
let affix = {
    /**
     * 组件初始化方法，用于给指定的元素绑定onscroll事件。
     * @function affix.init
     * @param {String} id 组件Affix div 元素的id
     * @param {String} container HTML元素id，如果为空则使用document.body 
     * @param {DotNetObjectReference<TAffix>} dotnetRef
     */
    init: function (id, container, dotnetRef) {
        let affixDiv = document.getElementById(id)
        let el = container ? document.getElementById(container) : document.body      
        el.onscroll = function () {
            let boundingClientRect = el.getBoundingClientRect()
            let containerScrollTop = el.scrollTop
            let containerY = parseInt(boundingClientRect.y)
            let containerHeight = el.clientHeight
            dotnetRef.invokeMethodAsync("OnScrollChanged", containerScrollTop, containerY, containerHeight)
        }
    },
    /**
     * 获取组件当前位置距离窗口顶端的高度值，offsetTop
     * @param {String} 组件的元素id
     * @return {Number} offsetTop
     * */
    positionY: function (id) {
        return parseInt(document.getElementById(id).getBoundingClientRect().y);
    },
    show: function (msg) {
        console.log(msg)
    },
}

export { affix }

import { createPopper } from './js/popper/popper.js'

/**
 * 弹出层
 * */
let popup = {
    /**
     * 创建一个新的弹出层引用
     * @param objRef 触发对象的引用
     * @param popupRef 弹出提示对象的引用
     * @param options 配置对象
     * @returns popper instance
     * */
    create: function (objRef, popupRef, options, optionRef) {
        options.onFirstUpdate = state => {
            optionRef.invokeMethodAsync("CallOnFirstUpdate", { placement: state.placement });
        }

        return createPopper(objRef, popupRef, options);
    },
    /**
     * 获取状态
     * @param popperInstance popper 实例
     * */
    getState: function (popperInstance) {
        return {
            placement: popperInstance.state
        };
    }
}

//export function updateOnInstance(instance) {
//    return instance.update().then(state => ({ placement: state.placement }));
//}

//export function setOptionsOnInstance(instance, options, objRef) {
//    options.onFirstUpdate = (state) => {
//        const stateCopy = {
//            placement: state.placement
//        }
//        objRef.invokeMethodAsync('CallOnFirstUpdate', stateCopy)
//    };
//    return instance.setOptions(options).then(state => ({ placement: state.placement }));
//}

export { popup }
