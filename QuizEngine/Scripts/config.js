requirejs.config({
    baseUrl: "/Scripts",
    paths: {
        "jquery": "jquery-1.11.2",
        "jquery-unob-ajax": "jquery.unobtrusive-ajax",
        "jquery-validate": "jquery.validate",
        "jquery-validate-unob": "jquery.validate.unobtrusive",
        "bootstrap": "bootstrap",
        "quizEngine": "quizEngine"
    },
    shim: {
        "jquery-unob-ajax": ["jquery"],
        "jquery-validate": ["jquery"],
        "jquery-validate-unob": ["jquery", "jquery-validate"],
        "bootstrap": ["jquery"]
    }
});