require(["jquery", "quizEngine", "jquery-unob-ajax", "jquery-validate", "jquery-validate-unob", "bootstrap"], function($, qe) {
    qe.Widgets.textAreaCharCountdown(
    {
        formId: "addQuizForm",
        textAreaId: "Description",
        displayDivId: "descTextAreaCharCount",
        maxLength: 512,
        placeholderMsg: "Maximum length of description is 512 characters",
        neutralPattn: "{0} characters left",
        warningPattn: "{0} characters exceeded",
        warningClasses: ["text-danger"]
    });
});
