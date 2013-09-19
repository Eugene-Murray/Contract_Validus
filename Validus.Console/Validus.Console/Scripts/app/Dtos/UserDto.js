var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.User = function () {
    this.Id = ko.observable(0);
    this.DomainLogon = ko.observable('');
    this.PrimaryOfficeList  = ko.observableArray();
    this.DefaultOrigOfficeList  = ko.observableArray();
    this.DefaultUWList = ko.observableArray();
    this.PrimaryOffice = ko.observable(new ConsoleApp.Office());
    this.DefaultOrigOffice = ko.observable(new ConsoleApp.Office());
    this.DefaultUW = ko.observable(new ConsoleApp.DefaultUnderwriter());

    this.OpenTabs = ko.observableArray();
    this.IsActive = ko.observable();
    this.UnderwriterId = ko.observable();

    this.TeamMemberships = ko.observableArray();
    this.FilterCOBs = ko.observableArray();
    this.FilterOffices = ko.observableArray();
    this.FilterMembers = ko.observableArray();
    this.AdditionalCOBs = ko.observableArray();
    this.AdditionalOffices = ko.observableArray();
    this.AdditionalUsers = ko.observableArray();

    this.AllTeamMemberships = ko.observableArray();
    this.AllFilterCOBs = ko.observableArray();
    this.AllFilterOffices = ko.observableArray();
    this.AllFilterMembers = ko.observableArray();
    this.AllAdditionalCOBs = ko.observableArray();
    this.AllAdditionalOffices = ko.observableArray();
    this.AllAdditionalUsers = ko.observableArray();
	
    this.IsCurrentMembership = ko.observable(false);
};