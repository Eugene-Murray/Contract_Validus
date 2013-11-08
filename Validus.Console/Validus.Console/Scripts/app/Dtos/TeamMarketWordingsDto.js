var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TeamMarketWordingsDto = function () {
    this.TeamId = ko.observable('');
    this.OfficeId = ko.observable('');
    this.MarketWordingSettingDtoList = ko.observableArray();
};