var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TeamLinksDto = function () {
    this.TeamId = ko.observable('');
    this.TeamLinksIdList = ko.observableArray();
};