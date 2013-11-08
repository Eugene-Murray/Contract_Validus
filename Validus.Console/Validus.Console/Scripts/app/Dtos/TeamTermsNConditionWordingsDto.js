var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TeamTermsNConditionWordingsDto = function () {
    this.TeamId = ko.observable('');
    this.OfficeId = ko.observable('');
    this.TermsNConditionWordingSettingDtoList = ko.observableArray();
};