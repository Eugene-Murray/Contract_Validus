var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.MarketWording = function () {

    this.Id = ko.observable(0);

    this.Key = ko.observable("00000000-0000-0000-0000-000000000000");
    this.WordingRefNumber = ko.observable("");
    this.Title = ko.observable("");
};