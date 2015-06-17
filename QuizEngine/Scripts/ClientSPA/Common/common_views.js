QuizEngine.module("Common", function(Common, QuizEngine, Backbone, Marionette, $, _) {
    Common.MessagingView = Marionette.ItemView.extend({
        template: "#general-messaging",
        //message: "",
        serializeData: function() {
            return {
                message: Marionette.getOption(this, "message")
            }
        }

    });
});
