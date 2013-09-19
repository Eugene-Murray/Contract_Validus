var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TeamQuoteTemplatesDto = function () {
    this.TeamId = ko.observable('');
    this.TeamQuoteTemplatesIdList = ko.observableArray();
};