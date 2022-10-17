window.tdesign = {
    popup: function (reference, content, placement) {        
        Popper.createPopper(reference, content, {
            placement: placement
        });
    }
}