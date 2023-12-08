

export const downloadFile = file => {
    const link = document.createElement('a')
    const url = URL.createObjectURL(file);
    link.href = url;
    link.download = file.name;
    link.click();
    window.URL.revokeObjectURL(url)
};

