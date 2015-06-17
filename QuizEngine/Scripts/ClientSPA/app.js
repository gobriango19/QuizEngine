var QuizEngine = new Marionette.Application();

QuizEngine.navigate = function(route, options) {
    options || (options = {});
    Backbone.history.navigate(route, options);
};

QuizEngine.getCurrentRoute = function() {
    return Backbone.history.fragment
};

QuizEngine.on("before:start", function() {
    var RegionContainer = Marionette.LayoutView.extend({
        //el: "#app-container",
        el: "body",
        regions: {
            main: "#main-region"
        }
    });

    QuizEngine.regions = new RegionContainer();
});

QuizEngine.on("start", function() {
    if(Backbone.history) {
        Backbone.history.start();
    }

    if(this.getCurrentRoute() === "") {
        QuizEngine.trigger("quizzes:list");
    }
});