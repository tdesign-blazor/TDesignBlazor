import { createPopper } from './js/popper/popper.js'

/**
 * 弹出层
 * */
let popup = {
    /**
     * 创建一个新的弹出层引用
     * @param objRef 触发对象的引用
     * @param popupRef 弹出提示对象的引用
     * @param options 配置对象
     * @returns popper instance
     * */
    create: function (objRef, popupRef, options, optionRef) {
        options.onFirstUpdate = state => {
            optionRef.invokeMethodAsync("CallOnFirstUpdate", { placement: state.placement });
        }

        return createPopper(objRef, popupRef, options);
    },
    /**
     * 获取状态
     * @param popperInstance popper 实例
     * */
    getState: function (popperInstance) {
        return {
            placement: popperInstance.state
        };
    }
}

//export function updateOnInstance(instance) {
//    return instance.update().then(state => ({ placement: state.placement }));
//}

//export function setOptionsOnInstance(instance, options, objRef) {
//    options.onFirstUpdate = (state) => {
//        const stateCopy = {
//            placement: state.placement
//        }
//        objRef.invokeMethodAsync('CallOnFirstUpdate', stateCopy)
//    };
//    return instance.setOptions(options).then(state => ({ placement: state.placement }));
//}

export { popup }