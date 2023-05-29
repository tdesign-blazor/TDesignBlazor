import { util } from './tdesign-blazor-util.js';

const ANIMATION = {
    ENTER: 't-dialog-zoom__vue-enter',
    ENTER_ACTIVE: 't-dialog-zoom__vue-enter-active',
    ENTER_TO: 't-dialog-zoom__vue-enter-to',
    LEAVE: 't-dialog-zoom__vue-leave',
    LEAVE_ACTIVE: 't-dialog-zoom__vue-leave-active',
    LEAVE_TO:'t-dialog-zoom__vue-leave-to'
};

let dialog = {
    
    open: function (dialogId) {

        let elementRef = dialog.element(dialogId);

            elementRef.style.display = '';
            elementRef.classList.add(ANIMATION.ENTER, ANIMATION.ENTER_ACTIVE, ANIMATION.ENTER_TO);
        setTimeout(() => {
            elementRef.classList.remove(ANIMATION.ENTER, ANIMATION.ENTER_ACTIVE, ANIMATION.ENTER_TO);
        }, 200);

        let keyupFuc = function (e) {
            if (e.code == 'Escape') {
                dialog.close(elementRef);
                this.window.removeEventListener('keyup', keyupFuc);
            }
        }

        window.addEventListener("keyup", keyupFuc);
    },
    close: function (dialogId) {

        let elementRef = dialog.element(dialogId);
        elementRef.classList.add(ANIMATION.LEAVE, ANIMATION.LEAVE_ACTIVE, ANIMATION.LEAVE_TO);
        setTimeout(() => {
            elementRef.classList.remove(ANIMATION.LEAVE, ANIMATION.LEAVE_ACTIVE, ANIMATION.LEAVE_TO);
            //elementRef.style.display = 'none';
        }, 200);
    },
    element: function (id) {
        var dialog = document.getElementById(id);
        if (!dialog) {
            throw 'dialog for id(' + id + ') cannot be not found';
        }
        return dialog;
    }
}

export { dialog }