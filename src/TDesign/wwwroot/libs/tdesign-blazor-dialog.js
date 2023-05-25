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
    
    open: function (elementRef,dotNetHelper) {
        if (!elementRef) {
            console.error('dialog is undifiend');
            return;
        }

        elementRef.style.display = '';
        elementRef.classList.add(ANIMATION.ENTER, ANIMATION.ENTER_ACTIVE, ANIMATION.ENTER_TO);
        setTimeout(() => {
            elementRef.classList.remove(ANIMATION.ENTER, ANIMATION.ENTER_ACTIVE, ANIMATION.ENTER_TO);

            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync("OnOpened");
            }
        }, 400);

        let keyupFuc = function (e) {
            if (e.code == 'Escape') {
                dialog.close(elementRef, dotNetHelper);
                this.window.removeEventListener('keyup', keyupFuc);
            }
        }

        window.addEventListener("keyup", keyupFuc);
    },
    close: function (elementRef, dotNetHelper) {
        if (!elementRef) {
            console.error('dialog is undifiend');
            return;
        }

        elementRef.classList.add(ANIMATION.LEAVE, ANIMATION.LEAVE_ACTIVE, ANIMATION.LEAVE_TO);
        setTimeout(() => {
            elementRef.classList.remove(ANIMATION.LEAVE, ANIMATION.LEAVE_ACTIVE, ANIMATION.LEAVE_TO);
            elementRef.style.display = 'none';
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync("OnClosed", true);
            }
        }, 400);
    }
}

export { dialog }