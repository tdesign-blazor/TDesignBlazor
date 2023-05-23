let tagInput = {
    pressKey: function (element, dotNetObject) {

        let inputValue;
        element.addEventListener('input', (e) => {
            inputValue = e.target.value;
        });

        element.addEventListener('keyup', e => {
            //13: 回车键(Enter)
            if (e.keyCode == 13) {
                dotNetObject.invokeMethodAsync("Invoke", e.keyCode, inputValue);
                element.value = inputValue = '';
                element.focus();
            }
            //8: 退格键(Backspace)
            if (e.keyCode == 8) {
                dotNetObject.invokeMethodAsync("Invoke", e.keyCode, "");
                element.focus();
            }
        })
    },
}

export { tagInput }