var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.BasicUser = function () {
	this.Id = ko.observable(0);
	this.DomainLogon = ko.observable('');
	this.IsCurrentMembership = ko.observable(false);
};