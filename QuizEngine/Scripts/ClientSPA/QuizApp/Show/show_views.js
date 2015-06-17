QuizEngine.module("QuizApp.Show", function(Show, QuizEngine, Backbone, Marionette, $, _) {
    Show.QuizLayoutView = Marionette.LayoutView.extend({
        template: "#quiz-show-layout",
        regions: {
            "quizRegion": "#quiz-region",
            "questionRegion": "#question-region",
            "answersRegion": "#answers-region",
            "resultRegion": "#result-region"
        }
    });

    Show.QuizView = Marionette.ItemView.extend({
        template: "#quiz-show"
    });

    Show.QuestionView = Marionette.ItemView.extend({
        template: "#question-show"
    });

    Show.AnswerView = Marionette.ItemView.extend({
        template: "#answers-show-individual",
        className: "selectable",
        triggers: {
            "click": "answer:selected"
        },
        onRender: function() {
            var imageUrl = this.model.get("imageUrl");
            if(imageUrl) {
                $inner = this.$el.find(".selectable-inner");
                $inner.css("background-image", "url(" + imageUrl + ")");
            }
        }
    });

    Show.AnswersView = Marionette.CompositeView.extend({
        template: "#answers-show",
        childView: Show.AnswerView,
        className: "wrap"
    });

    Show.ResultView = Marionette.ItemView.extend({
        template: "#result-show",
        triggers: {
            "click a.js-play-again-link": "quiz:playagain",
            "click a.js-play-another-link": "quizzes:playanother"
        },
        onRender: function() {
            var imageUrl = this.model.get("imageUrl");
            if(imageUrl) {
                $resultImage = this.$el.find(".result-image");
                var image = $("<img>", { src: imageUrl, class: "responsive-image" });
                $resultImage.append(image);
            }
        }
    });
});