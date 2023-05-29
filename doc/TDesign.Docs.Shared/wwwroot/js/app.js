function changeDark(dark) {
    if (dark) {
        document.documentElement.setAttribute('theme-mode', 'dark');
    }
    else {
        document.documentElement.removeAttribute('theme-mode');
    }
}
window.highlight = () => {
    hljs.highlightAll();
}

highlight();