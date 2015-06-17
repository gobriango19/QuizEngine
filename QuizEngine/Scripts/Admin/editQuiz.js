require(["jquery", "quizEngine", "jquery-unob-ajax", "jquery-validate", "jquery-validate-unob", "bootstrap"], function($, qe) {
    window.bindEditQuizTextArea = function() {
        qe.Widgets.textAreaCharCountdown(
        {
            formId: "editQuizForm",
            textAreaId: "Description",
            displayDivId: "quizDescTextAreaCharCount",
            maxLength: 512,
            placeholderMsg: "Maximum length of description is 512 characters",
            neutralPattn: "{0} characters left",
            warningPattn: "{0} characters exceeded",
            warningClasses: ["text-danger"]
        });
    }

    bindEditQuizTextArea(); // initial binding

    window.displayEditQuizSuccessMsg = function() {
        qe.Helpers.displayMsgInDiv(
        {
            displayDivId: "editQuizFormMsgDiv",
            message: "The quiz has been sucessfully edited",
            setDisplayTo: null,
            displayClasses: ["text-success"]
        });

        $("#editQuizFormDiv").click(function() {
            qe.Helpers.clearMsgDiv({
                displayDivId: "editQuizFormMsgDiv",
                setDisplayTo: null,
                displayClasses: ["text-success"]
            });
        });
    }


    qe.Widgets.textAreaCharCountdown(
    {
        formId: "addQuestionForm",
        textAreaId: "QuestionText",
        displayDivId: "questionTextAreaCharCount",
        maxLength: 512,
        placeholderMsg: "Maximum length of question is 512 characters",
        neutralPattn: "{0} characters left",
        warningPattn: "{0} characters exceeded",
        warningClasses: ["text-danger"]
    });

    qe.Widgets.textAreaCharCountdown(
    {
        formId: "addResultForm",
        textAreaId: "Description",
        displayDivId: "resultDescTextAreaCharCount",
        maxLength: 512,
        placeholderMsg: "Maximum length of description is 512 characters",
        neutralPattn: "{0} characters left",
        warningPattn: "{0} characters exceeded",
        warningClasses: ["text-danger"]
    });
});