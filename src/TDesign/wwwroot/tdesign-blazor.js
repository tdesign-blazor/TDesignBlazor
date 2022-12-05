import { createPopper } from './js/popper/popper.js'

 */
        }
    },
    /**
     * 获取组件当前位置距离窗口顶端的高度值，offsetTop
     * @param {String} 组件的元素id
     * @return {Number} offsetTop
     * */
    positionY: function (id) {
        return parseInt(document.getElementById(id).getBoundingClientRect().y);
    },
}

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

export { affix, popup }
