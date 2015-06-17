QuizEngine.module("QuizApp.Show", function(Show, QuizEngine, Backbone, Marionette, $, _) {
    Show.Controller = {
        showQuiz: function(quizId) {
            var loadingView = new QuizEngine.Common.MessagingView({
                message: "Loading quiz..."
            });
            QuizEngine.regions.main.show(loadingView);

            var showClientView = function(clientView) {
                loadingView.$el.fadeOut(function() {
                    clientView.$el.fadeIn();
                    QuizEngine.regions.main.show(clientView);
                });
            }

            var quizPromise = QuizEngine.request("entities:quiz", quizId);
            $.when(quizPromise).done(function(status, model) {
                var clientView = undefined;
                if(status === 200) {
                    var currentQuiz = model;

                    var questionsPromise = QuizEngine.request("entities:questions", quizId);
                    $.when(questionsPromise).done(function(status, collection) {
                        if(status === 200 && collection.length !== 0) {
                            var allQuestions = collection;
                            var currentQuestionIndex = 0;

                            var firstQuestion = collection.at(currentQuestionIndex);

                            clientView = new Show.QuizLayoutView();
                            var quizView = new Show.QuizView({ model: currentQuiz });
                            var questionView = new Show.QuestionView({ model: firstQuestion });
                            var answersView = new Show.AnswersView({
                                collection: firstQuestion.get("answers")
                            });

                            var totalScore = 0;
                            answersView.on("childview:answer:selected", function(args) {
                                var selectedAnswer = args.model;
                                totalScore += selectedAnswer.get("score");

                                console.log("selected score: " + selectedAnswer.get("score"));
                                console.log("total score: " + totalScore);

                                currentQuestionIndex += 1;
                                var nextQuestion = allQuestions.at(currentQuestionIndex)
                                if(nextQuestion) {
                                    questionView.model = nextQuestion;
                                    questionView.render();
                                    answersView.collection.reset(nextQuestion.get("answers").models);
                                } else {
                                    Show.Controller.showResult(quizId, totalScore, clientView);
                                }
                            });

                            clientView.on("show", function() {
                                clientView.quizRegion.show(quizView);
                                clientView.questionRegion.show(questionView);
                                clientView.answersRegion.show(answersView);
                            });
                        } else {
                            clientView = new QuizEngine.Common.MessagingView({
                                message: "The quiz cannot be loaded; it contains no questions."
                            });
                        }
                        showClientView(clientView)
                    });
                } else if(status === 404) {
                    clientView = new QuizEngine.Common.MessagingView({
                        message: "The quiz cannot be loaded; it was not found in the system."
                    });
                    showClientView(clientView);
                } else {
                    clientView = new QuizEngine.Common.MessagingView({
                        message: "An error has occurred, and the quiz cannot be loaded."
                    });
                    showClientView(clientView);
                }
            });
        },
        showResult: function(quizId, score, layoutView) {
            layoutView.questionRegion.empty();
            layoutView.answersRegion.empty();

            var loadingView = new QuizEngine.Common.MessagingView({
                message: "Loading result..."
            });
            layoutView.resultRegion.show(loadingView);

            var resultsPromise = QuizEngine.request("entities:results", quizId);
            $.when(resultsPromise).done(function(status, collection) {
                var resultView = undefined;
                if(status === 200 && collection.length !== 0) {
                    var result = _.find(collection.models, function(model) {
                        return (score >= model.get("minScore") && score <= model.get("maxScore"));
                    });
                    if(result) {
                        resultView = new Show.ResultView({ model: result });

                        resultView.on("quiz:playagain", function(args) {
                            QuizEngine.trigger("quiz:show", quizId)
                        });
                        resultView.on("quizzes:playanother", function(args) {
                            QuizEngine.trigger("quizzes:list");
                        });
                    } else {
                        resultView = new QuizEngine.Common.MessagingView({
                            message: "The quiz cannot be completed; no results matches your score."
                        });
                    }
                } else {
                    resultView = new QuizEngine.Common.MessagingView({
                        message: "The quiz cannot be completed; it contains no results."
                    });
                }
                loadingView.$el.fadeOut(function() {
                    resultView.$el.fadeIn();
                    layoutView.resultRegion.show(resultView);
                });
            });
        }
    }
});