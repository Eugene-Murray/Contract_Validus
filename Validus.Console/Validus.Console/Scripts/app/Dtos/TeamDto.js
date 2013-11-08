var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.Team = function (id, title, defaultMOA, defaultDomicile, quoteExpiryDaysDefault, users, links, relatedCOBs, relatedOffices, allUsers, allLinks, allRelatedCOBs, allRelatedOffices, defaultPolicyType)
{

	this.Id = ko.observable(id);
	this.Title = ko.observable(title);
	this.DefaultMOA = ko.observable(defaultMOA);
	this.DefaultDomicile = ko.observable(defaultDomicile);
	this.QuoteExpiryDaysDefault = ko.observable(quoteExpiryDaysDefault);
	this.DefaultPolicyType = ko.observable(defaultPolicyType);

	this.Users = (users !== undefined && users !== null) ? ko.observableArray(users) : ko.observableArray([]);
	this.Links = (links !== undefined && links !== null) ? ko.observableArray(links) : ko.observableArray([]);  //ko.observableArray(links);
	this.RelatedCOBs = (relatedCOBs !== undefined && relatedCOBs !== null) ? ko.observableArray(relatedCOBs) : ko.observableArray([]); //ko.observableArray(relatedCOBs);
	this.RelatedOffices = (relatedOffices !== undefined && relatedOffices !== null) ? ko.observableArray(relatedOffices) : ko.observableArray([]); //ko.observableArray(relatedOffices);

	this.AllUsers = (allUsers !== undefined && allUsers !== null) ? ko.observableArray(allUsers) : ko.observableArray([]); //ko.observableArray(allUsers);
	this.AllLinks = (allLinks !== undefined && allLinks !== null) ? ko.observableArray(allLinks) : ko.observableArray([]); //ko.observableArray(allLinks);
	this.AllRelatedCOBs = (allRelatedCOBs !== undefined && allRelatedCOBs !== null) ? ko.observableArray(allRelatedCOBs) : ko.observableArray([]); //ko.observableArray(allRelatedCOBs);
	this.AllRelatedOffices = (allRelatedOffices !== undefined && allRelatedOffices !== null) ? ko.observableArray(allRelatedOffices) : ko.observableArray([]); //ko.observableArray(allRelatedOffices);
	
    this.dirtyFlag = new ko.TeamDirtyFlag(this);
};
