QuizEngine.module("QuizApp", function(QuizApp, QuizEngine, Backbone, Marionette, $, _) {
    QuizApp.Router = Marionette.AppRouter.extend({
        appRoutes: {
            "quizzes": "listQuizzes",
            "quizzes/:quizId": "showQuiz"
        }
    });

    var API = {
        listQuizzes: function() {
            QuizEngine.QuizApp.List.Controller.listQuizzes();
        },
        showQuiz: function(quizId) {
            QuizEngine.QuizApp.Show.Controller.showQuiz(quizId);
        }
    }

    QuizEngine.on("quizzes:list", function() {
        QuizEngine.navigate("quizzes");
        API.listQuizzes();
    });

    QuizEngine.on("quiz:show", function(quizId) {
        QuizEngine.navigate("quizzes/" + quizId);
        API.showQuiz(quizId);
    });

    QuizApp.on("start", function() {
        new QuizApp.Router({
            controller: API
        });
    });
});