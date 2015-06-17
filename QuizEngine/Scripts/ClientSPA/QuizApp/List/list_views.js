QuizEngine.module("QuizApp.List", function(List, QuizEngine, Backbone, Marionette, $, _) {
    List.QuizView = Marionette.ItemView.extend({
        template: "#quizzes-list-individual",
        className: "list-group-item",
        trigger: {
            "click .js-name a.js-name-link": "quiz:show"
        }
    });

    List.QuizzesView = Marionette.CompositeView.extend({
        template: "#quizzes-list",
        className: "list-group",
        childView: List.QuizView
    });
});