var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TermsNConditionWordingSettingDto = function () {
    this.SettingId = ko.observable(0);
    this.Id = ko.observable(0);
    this.DisplayOrder = ko.observable(0);
    this.IsStrikeThrough = ko.observable(false);
    this.WordingRefNumber = ko.observable("");
    this.Title = ko.observable("");
};