window.downloadFile = async (fileName, base64) => {
    if ('showSaveFilePicker' in window) {
        try {
            const blob = await (await fetch("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64)).blob();

            const handle = await window.showSaveFilePicker({
                suggestedName: fileName,
                types: [{
                    description: 'Excel File',
                    accept: { 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet': ['.xlsx'] }
                }]
            });

            const writable = await handle.createWritable();
            await writable.write(blob);
            await writable.close();
            return;
        } catch (err) {
            if (err.name === 'AbortError') return;
            console.log(err);
        }
    }

    const link = document.createElement('a');
    link.download = fileName;
    link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}