function copyToClipboard(text) {
    // Use the modern Clipboard API if available and in a secure context
    if (navigator.clipboard && window.isSecureContext) {
        return navigator.clipboard.writeText(text);
    } else {
        // Fallback for older browsers or non-secure contexts (HTTP)
        let textArea = document.createElement("textarea");
        textArea.value = text;
        textArea.style.position = "fixed";
        textArea.style.left = "-9999px";
        textArea.style.top = "-9999px";
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();
        return new Promise((res, rej) => {
            try {
                document.execCommand('copy') ? res() : rej();
            } catch (error) {
                rej(error);
            }
            document.body.removeChild(textArea);
        });
    }
}