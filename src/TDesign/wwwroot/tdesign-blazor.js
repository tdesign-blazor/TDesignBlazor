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