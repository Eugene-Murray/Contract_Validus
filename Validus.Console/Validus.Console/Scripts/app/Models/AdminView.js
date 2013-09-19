var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.View = function (title, templateName, data, active) {
    this.title = title;
    this.templateName = templateName;
    this.data = data;
    this.active = ko.observable(active);
};