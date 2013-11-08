var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.QuoteTemplate = function () {
    this.Id = ko.observable(0);
    this.Name = ko.observable("");
    this.RdlPath = ko.observable("");
};