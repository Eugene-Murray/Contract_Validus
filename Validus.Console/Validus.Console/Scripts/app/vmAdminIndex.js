$(function () {

    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

    ConsoleApp.vmAdminIndex = function () {
        var self = this;

        self.views = ko.observableArray([
            new ConsoleApp.View("Teams", "ManageTeams.Template", ConsoleApp.vmManageTeams, true),
            new ConsoleApp.View("Users", "ManageUsers.Template", ConsoleApp.vmManageUsers, false),
            new ConsoleApp.View("Links", "ManageLinks.Template", ConsoleApp.vmManageLinks, false),
            new ConsoleApp.View("QuoteTemplates", "ManageQuoteTemplates.Template", ConsoleApp.vmManageQuoteTemplates, false),
            new ConsoleApp.View("Accelerators", "ManageAccelerators.Template", ConsoleApp.vmManageAccelerators, false),
            new ConsoleApp.View("MarketWordings", "ManageMarketWordings.Template", ConsoleApp.vmManageMarketWordings, false),
            new ConsoleApp.View("TermsNConditionWordings", "ManageTermsNConditionWordings.Template", ConsoleApp.vmManageTermsNConditionWordings, false),
            new ConsoleApp.View("Subjectivities", "ManageSubjectToClauseWordings.Template", ConsoleApp.vmManageSubjectToClauseWordings, false),
            new ConsoleApp.View("UnderwriterSignature", "ManageUnderwriterSignature.Template", ConsoleApp.vmManageUnderwriterSignature, false)
        ]);

        self.selectedView = ko.observable(new ConsoleApp.View("Manage Teams", "ManageTeams.Template", ConsoleApp.vmManageTeams, true));
        

    	// Events
        self.click_GoToUserView = function (e) {
            if (e.LogonName != null) {
                self.selectedView(self.views()[1]); // Navigate to the ManageUser View
                ConsoleApp.vmManageUsers.Initialize({ pageMode: 'EDIT_FROM_TEAMPAGE', selectedUser: e.LogonName });
            }
        };

        // Subscribe
	    self.selectedView.subscribe(function(data)
	    {
	    	// TODO: Should not need to loop through every view, especially when active is already found
		    $.each(self.views(), function(key, view)
		    {
		    	// TODO: view.active(data.title === view.title);
			    
			    if (data.title == view.title)
			    {
				    view.active(true);
			    }
			    else
			    {
				    view.active(false);
			    }
		    });
	    });

        return self;
    };
});