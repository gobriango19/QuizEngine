require(["jquery", "quizEngine", "jquery-unob-ajax", "jquery-validate", "jquery-validate-unob", "bootstrap"], function($, qe) {
    window.bindEditQuestionTextArea = function() {
        qe.Widgets.textAreaCharCountdown(
        {
            formId: "editQuestionForm",
            textAreaId: "QuestionText",
            displayDivId: "questionTextAreaCharCount",
            maxLength: 512,
            placeholderMsg: "Maximum length of question is 512 characters",
            neutralPattn: "{0} characters left",
            warningPattn: "{0} characters exceeded",
            warningClasses: ["text-danger"]
        });
    }

    bindEditQuestionTextArea(); // initial binding

    window.displayEditQuestionSuccessMsg = function() {
        qe.Helpers.displayMsgInDiv(
        {
            displayDivId: "editQuestionFormMsgDiv",
            message: "The question has been successfully edited",
            setDisplayTo: null,
            displayClasses: ["text-success"]
        });

        $("#editQuestionFormDiv").click(function() {
            qe.Helpers.clearMsgDiv(
            {
                displayDivId: "editQuestionFormMsgDiv",
                setDisplayTo: null,
                displayClasses: ["text-success"]
            });
        });
    }

    qe.Widgets.textAreaCharCountdown(
    {
        formId: "addAnswerForm",
        textAreaId: "AnswerText",
        displayDivId: "answerTextAreaCharCount",
        maxLength: 512,
        placeholderMsg: "Maximum length of answer is 512 characters",
        neutralPattn: "{0} characters left",
        warningPattn: "{0} characters exceeded",
        warningClasses: ["text-danger"]
    });
});