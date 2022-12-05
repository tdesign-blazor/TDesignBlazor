window.hash = function (id) {
    let el = document.getElementById(id);
    let topSize = el.offsetTop - el.offsetHeight;
    el.parentNode.scrollTo({
        top: topSize,
        left: 0,
        behavior: 'smooth'
    })
}

/*
 anchor关联滚动容器滚动监听
 */
window.onAnchorScroll = function (dotNetHelper,id) {
    document.getElementById(id).onscroll = function (e) {
        dotNetHelper.invokeMethodAsync('OnScrollAnchorChangeAsync', e.srcElement.scrollTop);
        console.log(e.srcElement.scrollTop)
    }
}

/*
 获取元素的顶部偏移量
 */
window.getOffsetTop = function (id) {
    let el = document.getElementById(id);
    return el.offsetTop;
}

/*
 获取元素的偏移高度
 */
window.getOffsetHeight = function (id) {
    let el = document.getElementById(id);
    return el.offsetHeight;
}