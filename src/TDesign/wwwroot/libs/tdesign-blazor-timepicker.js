let timepicker = {
    clickToScroll: function (element) {
        if (!element) {
            console.log('element is undifined');
            return;
        }
        let top = 0;
        let parent = element.offsetParent;
        let current = element;
        while (current != parent) {
            top += current.offsetTop;
            current = current.offsetParent;
        }

        top = top - parent.offsetTop - 100;//100 是根据 .t-time-picker__panel-body-active-mask 这个 div 猜的
        console.debug('top:' + top);

        parent.scrollTo({
            top: top,
            behavior:'smooth'
        })
    },
    wheelToScroll: function (e, element) {
        this.clickToScroll(element);
    }
}

export { timepicker }