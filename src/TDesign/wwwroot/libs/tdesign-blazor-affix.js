import { util } from './tdesign-blazor-util.js';

/** TAffix 组件 */

let affix = {
    /**
     * 组件初始化方法，用于给指定的元素绑定onscroll事件。
     * @function affix.init
     * @param {String} container HTML元素id，如果为空则使用document.body 
     * @param {DotNetObjectReference<TAffix>} dotnetRef
     */
    init: function (containerId, dotnetRef) {
        let el = containerId? document.getElementById(containerId) : document.body;
        let scroll = function () {
            let boundingClientRect = el.getBoundingClientRect()
            let containerScrollTop = el.scrollTop;
            let containerY = boundingClientRect.y;
            let containerHeight = el.clientHeight;

            console.log("Top:" + containerScrollTop);
            dotnetRef.invokeMethodAsync("Invoke", containerScrollTop, containerY, containerHeight)
        }
        util.setEvent(el, "onscroll", scroll);
    },
    /**
     * 获取组件当前位置距离窗口顶端的高度值，offsetTop
     * @param {String} 组件的元素id
     * @return {Number} offsetTop
     * */
    positionY: function (containerId) {
        let el = containerId ? document.getElementById(containerId) : document.body;
        return el.getBoundingClientRect().y;
    },
}

export { affix };