var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TeamSubjectToClauseWordingsDto = function () {
    this.TeamId = ko.observable('');
    this.OfficeId = ko.observable('');
    this.SubjectToClauseWordingSettingDtoList = ko.observableArray();
};