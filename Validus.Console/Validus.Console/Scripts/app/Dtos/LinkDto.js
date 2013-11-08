var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.Link = function () {
    this.Id = ko.observable(0);
    this.Url = ko.observable("");
    this.Title = ko.observable("");
    this.Category = ko.observable("");
};