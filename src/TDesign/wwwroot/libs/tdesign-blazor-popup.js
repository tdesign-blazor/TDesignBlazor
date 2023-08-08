import { createPopper } from './popper/popper.js'

/**
 * 弹出层
 * */
let popup = {
    animationClass: {
        ANIMATION_ENTER: 't-popup--animation-enter',
        ANIMATION_ENTER_FROM: 't-popup--animation-enter-from',
        ANIMATION_EXITING: 't-popup--animation-exiting',
        ANIMATION_LEAVE_TO: 't-popup--animation-leave-to',
        ANIMATION_ENTER_TO: 't-popup--animation-enter-to',
        ANIMATION_ENTERING: 't-popup--animation-entering',
        ANIMATION_LEAVE_FROM: 't-popup--animation-leave-from',
        ANIMATION_LEAVE: 't-popup--animation-leave',
        ANIMATION_ENTER_ACTIVE: 't-popup--animation-enter-active',
        ANIMATION_LEAVE_ACTIVE: 't-popup--animation-leave-active',
    },
    popperList: [],
    /**
     * 创建一个新的弹出层引用
     * @param trigerElement 触发对象的引用
    * @param popupElement 弹出提示对象的引用
     * @param options 弹出层配置
     * @param optionDotNetHelper options 包装的 DotNetReference 对象
     * @returns popper 对象
    * */
    show: function (trigerElement, popupElement, options, dotNetHelper) {

        let popper;
        options.onFirstUpdate = state => {
            console.debug('placement:' + state.placement);
            //optionDotNetReference.invokeMethodAsync("InvokeOnFirstUpdate", { placement: state.placement });
        }
        setTimeout(() => {
            popper = createPopper(trigerElement, popupElement, options);
            document.body.appendChild(popupElement);

            window.addEventListener("click", () => {
                this.hide(popupElement, dotNetHelper);
            }, {once:true});

            //popupElement.style.display = "";
            popupElement.style.display = "";
            popupElement.classList.remove(this.animationClass.ANIMATION_ENTER, this.animationClass.ANIMATION_LEAVE_ACTIVE);
            popupElement.classList.add(this.animationClass.ANIMATION_ENTER_ACTIVE, this.animationClass.ANIMATION_LEAVE);
            dotNetHelper.invokeMethodAsync('onShown');

            this.popperList[popupElement] = popper;
        }, 400);
        //return popper;
    },
    hide: function (popupElement, dotNetHelper) {
        if (popupElement) {

            const popper = this.popperList[popupElement];
            if (!popper) {
                return;
            }
            popupElement.classList.remove(this.animationClass.ANIMATION_LEAVE, this.animationClass.ANIMATION_ENTER_ACTIVE);
            popupElement.classList.add(this.animationClass.ANIMATION_LEAVE_ACTIVE, this.animationClass.ANIMATION_ENTER);

            setTimeout(() => {
                popper.destroy();
                popupElement.style.display = 'none';
                dotNetHelper.invokeMethodAsync('onHidden');
            },400)
        }
    },
    clear: () => {
        this.popperList.clear();
    }
}

export { popup }