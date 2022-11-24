/**
 * @description 组件 affix 用到的js对象。
 */
let affix = {
    /**
     * 组件初始化方法，用于给指定的元素绑定onscroll事件。
     * @function affix.init
     * @param {String} id HTML元素id，如果为空则使用document.body 
     */
    init: function (id, dotnetRef) {
        console.log('[info] affix.init ', id)
        let el = id ? document.getElementById(id) : document.body
        el.onscroll = function () { 
            let top = el.scrollTop
            let height = el.scrollHeight
            let bottom = height - top - el.clientHeight
            dotnetRef.invokeMethodAsync("OnScrollChanged", top, bottom, height)
        }
        
    },
    show: function(top, bottom, height){
        console.log(`[info]affix.box top:${top}, bottom:${bottom}, height:${height}`)
    },
}

export { affix }