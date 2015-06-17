QuizEngine.module("QuizApp.List", function(List, QuizEngine, Backbone, Marionette, $, _) {
    List.Controller = {
        listQuizzes: function() {
            var loadingView = new QuizEngine.Common.MessagingView({
                message: "Loading quizzes..."
            });
            QuizEngine.regions.main.show(loadingView);

            var quizzesPromise = QuizEngine.request("entities:quizzes");
            $.when(quizzesPromise).done(function(status, collection) {
                var listView = undefined;
                if(status === 200) {
                    listView = new List.QuizzesView({
                        collection: collection
                    });
                    listView.on("childview:quiz:show", function(args) {
                        QuizEngine.trigger("quiz:show", args.model.get("quizId"));
                    });
                } else if(status === 404) {
                    listView = new QuizEngine.Common.MessagingView({
                        message: "No quizzes can be loaded; there are no quizzes in the system."
                    });
                } else {
                    listView = new QuizEngine.Common.MessagingView({
                        message: "An error has occurred, and no quizzes can be loaded."
                    });
                }

                loadingView.$el.fadeOut(function() {
                    listView.$el.fadeIn();
                    QuizEngine.regions.main.show(listView);
                });
            });
        }
    }
});