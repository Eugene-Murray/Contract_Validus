var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.Accelerator = function () {

    this.Id = ko.observable("");

    this.HomepageUrl = ko.observable("");
    this.DisplayName = ko.observable("");
    this.DisplayIcon = ko.observable("");
    this.ActivityCategory = ko.observable("");
    this.ActivityActionPreview = ko.observable("");
    this.ActivityActionExecute = ko.observable("");
};