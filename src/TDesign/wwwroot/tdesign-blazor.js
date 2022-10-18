export function createPopper(reference, popper, options, objRef) {
    options.onFirstUpdate = (state) => {
        console.log(state);
        const stateCopy = {
            placement: state.placement
        }
        objRef.invokeMethodAsync('CallOnFirstUpdate', stateCopy)
    };

    return Popper.createPopper(reference, popper, {
        placement:'buttom'
    });
}

export function getStateOfInstance(instance) {
    var state = instance.state;
    return {
        placement: state.placement
    }
}

export function updateOnInstance(instance) {
    return instance.update().then(state => ({ placement: state.placement }));
}

export function setOptionsOnInstance(instance, options, objRef) {
    options.onFirstUpdate = (state) => {
        const stateCopy = {
            placement: state.placement
        }
        objRef.invokeMethodAsync('CallOnFirstUpdate', stateCopy)
    };
    return instance.setOptions(options).then(state => ({ placement: state.placement }));
}