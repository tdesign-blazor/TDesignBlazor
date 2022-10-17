window.tdesign = {
    popup: {        
        show: function (element, tip, placement) {
            let instance= Popper.createPopper(element, tip, {
                placement: placement
            });
            return instance;
        },
        destroy: function (instance) {
            instance.destroy();
        }
    }
}