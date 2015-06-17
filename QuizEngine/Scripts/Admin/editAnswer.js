require(["jquery", "quizEngine", "jquery-unob-ajax", "jquery-validate", "jquery-validate-unob", "bootstrap"], function($, qe) {
    window.bindEditAnswerTextArea = function() {
        qe.Widgets.textAreaCharCountdown(
        {
            formId: "editAnswerForm",
            textAreaId: "AnswerText",
            displayDivId: "answerTextAreaCharCount",
            maxLength: 512,
            placeholderMsg: "Maximum length of answer is 512 characters",
            neutralPattn: "{0} characters left",
            warningPattn: "{0} characters exceeded",
            warningClasses: ["text-danger"]
        });
    }

    bindEditAnswerTextArea(); // initial binding

    window.displayEditAnswerSuccessMsg = function() {
        qe.Helpers.displayMsgInDiv(
        {
            displayDivId: "editAnswerFormMsgDiv",
            message: "The answer has been successfully edited",
            setDisplayTo: null,
            displayClasses: ["text-success"]
        });

        $("#editAnswerFormDiv").click(function() {
            qe.Helpers.clearMsgDiv(
            {
                displayDivId: "editAnswerFormMsgDiv",
                setDisplayTo: null,
                displayClasses: ["text-success"]
            });
        });
    }
});