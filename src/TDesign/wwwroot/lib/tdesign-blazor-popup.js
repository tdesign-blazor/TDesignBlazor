import { createPopper } from './popper/popper.js'

/**
 * 弹出层
 * */
let popup = {
    /**
     * 创建一个新的弹出层引用
     * @param objRef 触发对象的引用
     * @param popupRef 弹出提示对象的引用
     * @param options 弹出层配置
     * @param optionDotNetHelper options 包装的 DotNetReference 对象
     * @returns popper 对象
     * */
    show: function (trigerElement, popupElement, options, optionDotNetReference,dotNetHelper) {

        options.onFirstUpdate = state => {
            optionDotNetReference.invokeMethodAsync("InvokeOnFirstUpdate", { placement: state.placement });
        }
        let popper = createPopper(trigerElement, popupElement, options);


        window.addEventListener("click", () => {
            dotNetHelper.invokeMethodAsync("Invoke");
        });

        document.body.appendChild(popupElement);
        popupElement.style.display = "";
        return popper;
    },
    hide: function (popper, popupElement, options) {
        if (popper) {
            popper.destroy();
        }
    }
}

export { popup }