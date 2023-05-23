import { util } from './tdesign-blazor-util.js';
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
        let top = 0;
        if (scrollContainer == null) {
            scrollContainer = anchor.offsetParent;
            top = anchor.offsetTop;
        } else {
            let curr = anchor
            while (scrollContainer != curr && curr != null) {
                top += curr.offsetTop;
                curr = curr.offsetParent;
            }
        }
        top = top - scrollContainer.offsetTop;
        //console.log(top);
        // console.log(scrollContainer);
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
        let scroll = function (e) {
            dotNetHelper.invokeMethodAsync('OnScrollAnchorChangeAsync', e.srcElement.scrollTop);
        }
        util.setEvent(document.getElementById(id), "onscroll", scroll)
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

export { anchor }