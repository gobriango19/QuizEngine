require(["jquery", "quizEngine", "jquery-unob-ajax", "jquery-validate", "jquery-validate-unob", "bootstrap"], function($, qe) {
    window.bindEditResultTextArea = function() {
        qe.Widgets.textAreaCharCountdown(
        {
            formId: "editResultForm",
            textAreaId: "Description",
            displayDivId: "resultDescTextAreaCharCount",
            maxLength: 512,
            placeholderMsg: "Maximum length of result is 512 characters",
            neutralPattn: "{0} characters left",
            warningPattn: "{0} characters exceeded",
            warningClasses: "warningMsg"
        });
    }

    bindEditResultTextArea(); // initial binding

    window.displayEditResultSuccessMsg = function() {
        qe.Helpers.displayMsgInDiv(
        {
            displayDivId: "editResultFormMsgDiv",
            message: "The result has been successfully edited",
            setDisplayTo: null,
            displayClasses: ["text-success"]
        });

        $("#editResultFormDiv").click(function() {
            qe.Helpers.clearMsgDiv(
            {
                displayDivId: "editResultFormMsgDiv",
                setDisplayTo: null,
                displayClasses: ["text-success"]
            });
        });
    }
});
