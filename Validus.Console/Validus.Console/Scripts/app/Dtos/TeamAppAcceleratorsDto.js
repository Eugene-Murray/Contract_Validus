var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TeamAppAcceleratorsDto = function () {
    this.TeamId = ko.observable('');
    this.TeamAppAcceleratorsIdList = ko.observableArray();
};