// global downloadFileFromStream
window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement("a");
    anchorElement.href = url;
    anchorElement.download = fileName ?? "";
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
};

window.openPdfFromStream = async (fileName, contentStreamReference) => {
    const previewWindow = window.open("about:blank", "_blank");
    if (!previewWindow) return false;

    try {
        const arrayBuffer = await contentStreamReference.arrayBuffer();
        const blob = new Blob([arrayBuffer], { type: "application/pdf" });
        const url = URL.createObjectURL(blob);
        previewWindow.document.title = fileName ?? "Document preview";
        previewWindow.location.replace(url);
        window.setTimeout(() => URL.revokeObjectURL(url), 60_000);
        return true;
    } catch (error) {
        previewWindow.close();
        throw error;
    }
};
