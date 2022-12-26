import { left } from './js/popper/enums.js'
import { createPopper } from './js/popper/popper.js'

/**
 * @description 组件 affix 用到的js对象。
 */
let affix = {
    /**
     * 组件初始化方法，用于给指定的元素绑定onscroll事件。
     * @function affix.init
     * @param {String} container HTML元素id，如果为空则使用document.body 
     * @param {DotNetObjectReference<TAffix>} dotnetRef
     */
    init: function (container, dotnetRef) {
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
}

/**
 * 弹出层
 * */
let popup = {
    popper: null,
    /**
     * 创建一个新的弹出层引用
     * @param objRef 触发对象的引用
     * @param popupRef 弹出提示对象的引用
     * @param options 弹出层配置
     * @param optionDotNetHelper options 包装的 DotNetReference 对象
     * @returns popper 对象
     * */
    create: function (objRef, popupRef, options, optionDotNetReference) {

        options.onFirstUpdate = state => {
            optionDotNetReference.invokeMethodAsync("InvokeOnFirstUpdate", { placement: state.placement });
        }

        popup.popper = createPopper(objRef, popupRef, options);

        document.body.appendChild(popupRef);

        return popup.popper;
    }
}
/**
 * 锚点
 * */
let anchor = {

    /**
     * anchor垂直平滑滚动
     * @param nodeId 元素id
     * */
    hash: function (nodeId, parentNodeId) {

        let anchor = document.getElementById(nodeId);
        let scrollContainer = document.getElementById(parentNodeId);
        let top =0;
        if(scrollContainer==null){
          scrollContainer=anchor.offsetParent;
          top=anchor.offsetTop;
        }else{
            let curr=anchor
            while(scrollContainer!=curr && curr!=null){
              top+=curr.offsetTop;
              curr=curr.offsetParent;
            }
        }
        top=top-scrollContainer.offsetTop;
            console.log(top);
            console.log(scrollContainer);
        // let test = document.getElementById("layout-body");

        scrollContainer.scrollTo({
            top: top,
            left: 0,
            behavior: 'smooth'
        })
    },
    /**
    * anchor关联滚动容器滚动监听
    * @param dotNetHelper anchor 实例
    * @param id 元素id
    * */
    onAnchorScroll: function (dotNetHelper, id) {
        document.getElementById(id).onscroll = function (e) {
            dotNetHelper.invokeMethodAsync('OnScrollAnchorChangeAsync', e.srcElement.scrollTop);
        }
    },


    /**
    * 获取元素的顶部偏移量
    * @param id 元素id
    * */
    getOffsetTop: function (id) {
        let el = document.getElementById(id);
        return el.offsetTop;
    },


    /**
    * 获取元素的偏移高度
    * @param id 元素id
    * */
    getOffsetHeight: function (id) {
        let el = document.getElementById(id);
        return el.offsetHeight;
    },
}


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

export { affix, popup, anchor, theme }
