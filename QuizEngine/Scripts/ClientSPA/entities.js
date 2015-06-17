QuizEngine.module("Entities", function(Entities, QuizEngine, Backbone, Marionette, $, _) {
    Entities.Quiz = Backbone.Model.extend({
        urlRoot: "/api/quizzes",
        idAttribute: "quizId"
    });

    Entities.Quizzes = Backbone.Collection.extend({
        url: "/api/quizzes",
        model: Entities.Quiz
    });

    Entities.Question = Backbone.RelationalModel.extend({
        idAttribute: "questionId",
        relations: [
            {
                type: Backbone.HasMany,
                key: "answers",
                relatedModel: "QuizEngine.Entities.Answer",
                collectionType: "QuizEngine.Entities.Answers"
            }
        ]
    });

    Entities.Questions = Backbone.Collection.extend({
        initialize: function(options) {
            this.quizId = options.quizId;
        },
        url: function() {
            return "/api/quizzes/" + this.quizId + "/questions"
        },
        model: Entities.Question,
        comparator: "sequence"
    });


    Entities.Answer = Backbone.RelationalModel.extend({
        idAttribute: "answerId"
    });

    Entities.Answers = Backbone.Collection.extend({
        model: Entities.Answer,
        comparator: "sequence"
    });

    Entities.Result = Backbone.Model.extend({
        idAttribute: "resultId"
    });

    Entities.Results = Backbone.Collection.extend({
        initialize: function(options) {
            this.quizId = options.quizId;
        },
        url: function() {
            return "/api/quizzes/" + this.quizId + "/results"
        },
        model: Entities.Result
    });

    var API = {
        getQuizzes: function() {
            var quizzes = new Entities.Quizzes();
            var defer = $.Deferred();
            quizzes.fetch({
                success: function(collection, response, options) {
                    defer.resolve(options.xhr.status, collection);
                },
                error: function(collection, response, options) {
                    defer.resolve(options.xhr.status, collection);
                }
            });

            return defer.promise();
        },
        getQuiz: function(quizId) {
            var quiz = new Entities.Quiz({ quizId: quizId });
            var defer = $.Deferred();
            quiz.fetch({
                success: function(model, response, options) {
                    defer.resolve(options.xhr.status, model);
                },
                error: function(model, response, options) {
                    defer.resolve(options.xhr.status, model);
                }
            });
            return defer.promise();
        },
        getQuestions: function(quizId) {
            var questions = new Entities.Questions({ quizId: quizId });
            var defer = $.Deferred();
            questions.fetch({
                success: function(collection, response, options) {
                    defer.resolve(options.xhr.status, collection);
                },
                error: function(collection, response, options) {
                    defer.resolve(options.xhr.status, collection);
                }
            });
            return defer.promise();
        },
        getResults: function(quizId) {
            var results = new Entities.Results({ quizId: quizId });
            var defer = $.Deferred();
            results.fetch({
                success: function(collection, response, options) {
                    defer.resolve(options.xhr.status, collection);
                },
                error: function(collection, response, options) {
                    defer.resolve(options.xhr.status, collection);
                }
            });
            return defer.promise();
        }
    }

    QuizEngine.reqres.setHandler("entities:quizzes", function() {
        return API.getQuizzes();
    });

    QuizEngine.reqres.setHandler("entities:quiz", function(quizId) {
        return API.getQuiz(quizId);
    });

    QuizEngine.reqres.setHandler("entities:questions", function(quizId) {
        return API.getQuestions(quizId);
    });

    QuizEngine.reqres.setHandler("entities:results", function(quizId) {
        return API.getResults(quizId);
    });
});