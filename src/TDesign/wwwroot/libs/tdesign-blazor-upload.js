let upload = {
    _files: null,
    showDialog: function (inputElement, dotNetHelper) {
        if (!inputElement) {
            return;
        }
        inputElement.click();

        inputElement.addEventListener('change', e => {
            this._files = e.target.files;

            let files = this._files;
            let fileInfoList = new Array(files.length); //对应代码中的 UploadFileInfo 对象
            for (var i = 0; i < files.length; i++) {
                let file = files[i];
                fileInfoList.push({
                    name: file.name,
                    size: file.size,
                    contentType: file.type,
                    Url: URL.createObjectURL(file)
                });
            }

            dotNetHelper.invokeMethodAsync("Invoke", fileInfoList);
        })
    },
    uploadFile: function (parameters, dotNetHelper) {
        let file = this._files[parameters.index];
        if (!file) {
            throw 'The file from "_files" variable with index' + parameters.index + ' was not found';
        }

        let size = file.size;

        let formData = new FormData();
        formData.append(parameters.name, file);

        if (parameters.data) {
            for (let key in parameters.data) {
                formData.append(key, parameters.data[key]);
            }
        }

        const xhr = new XMLHttpRequest();

        xhr.onreadystatechange = (request, e) => {
            if (xhr.readyState == 4) {
                if (xhr.status == 200) {
                    dotNetHelper.invokeMethodAsync("onSuccess", parameters.fileId, xhr.responseText);
                } else {
                    dotNetHelper.invokeMethodAsync("onFailure", parameters.fileId, xhr.responseText);
                }
            }
        };
        xhr.upload.onprogress = (e) => {
            var percent = Math.floor(e.loaded / size * 100);
            dotNetHelper.invokeMethodAsync("onProgress", parameters.fileId, percent);
        };
        xhr.onerror = e => {
            dotNetHelper.invokeMethodAsync("onFailure", parameters.fileId, xhr.responseText);
        }
        if (!parameters.method) {
            parameters.method = "POST";
        }
        if (!parameters.actionUrl) {
            throw '必须提供上传服务端的 URL 地址';
        }

        xhr.open(parameters.method, parameters.actionUrl, true);//异步上传

        if (parameters.headers) {
            for (let key in parameters.headers) {
                xhr.setRequestHeader(key, parameters.headers[key]);
            }
        }

        xhr.send(formData);//发送请求到后端
    }
}

export { upload }