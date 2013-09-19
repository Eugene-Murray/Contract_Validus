var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.TeamMembership = function () {
    this.Id = ko.observable(0);
    this.Team = ko.observable(new ConsoleApp.Team());
    this.StartDate = ko.observable('');
    this.EndDate = ko.observable('');
    this.IsCurrent = ko.observable(false);
    this.PrimaryTeamMembership = ko.observable(false);

    this.setIcon = ko.computed(function()
    {
    	return this.PrimaryTeamMembership() === true ? "icon-ok icon-black" : "icon-remove icon-black";
    }, this);
};