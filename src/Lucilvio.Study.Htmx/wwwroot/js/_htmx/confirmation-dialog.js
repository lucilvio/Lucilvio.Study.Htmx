htmx.on("htmx:confirm", (evt) => {
    const sourceElement = evt.detail.elt;

    if (!sourceElement.hasAttribute("hx-confirm"))
        return;

    evt.preventDefault();

    const confirmationDialog = document.querySelector("#confirmationDialog");
    confirmationDialog.querySelector("#confirmationText").innerText = evt.detail.question;

    const bsModel = new bootstrap.Modal(confirmationDialog);
    bsModel.show();

    const confirmButton = confirmationDialog.querySelector("#confirmButton");
    const cancelButton = confirmationDialog.querySelector("#cancelButton");

    function cleanUp() {
        confirmButton.removeEventListener("click", onConfirm);
        cancelButton.removeEventListener("click", onCancel);
    }

    function onConfirm() {
        cleanUp();
        bsModel.hide();

        evt.detail.issueRequest(true);
    }

    function onCancel() {
        cleanUp();
        bsModel.hide();
    }

    confirmButton.addEventListener("click", onConfirm);
    cancelButton.addEventListener("click", onCancel);
});