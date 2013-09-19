var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.DefaultUnderwriter = function () {

    this.Id = ko.observable(0);
    this.DomainLogon = ko.observable('');

};