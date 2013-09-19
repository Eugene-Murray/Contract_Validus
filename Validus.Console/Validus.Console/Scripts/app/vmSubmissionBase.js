/*
	Base Submission View Model

	Description;
		This is the view model for a single Submission that contains all properties of the 
		model and client-side behaviour.
		
	Structure;
		Submission.Model.Options()[].OptionVersions()[].Quotes()[]
		Submission.Model.Options()[].CurrentVersion().Quotes()[]

	Usage;
		var vmSubmission = new Submission(0, "Id");

		ko.applyBindings(vmSubmission);

	TODO: Refactor vmSubmission to a simpler structure (group together functions, subscriptions, computed's, DTO's, etc)
*/
function vmSubmissionBase(id, domId, initilizeSelf)
{
	var self = this;

	self.Id = (id > 0) ? id : 0;
	self.Form = $(".form", $("#" + domId));
	self.DirtyFlag = null;

	self.OptionIndexToCopy = ko.observable(0);
	self.CanCreateQuoteSheet = ko.observable(false);
	self.CreatingQuoteSheet = ko.observable(false);
	self.IsInitialised = ko.observable(false);
	self.IsSaving = ko.observable(false);
    self.IsLoading = ko.observable(false);
	self.ValidationErrors = ko.observable("");

	self.Defaults = {
		Domicile: null,
		QuoteExpiry: null,
		Office: null,
		Underwriter: null,
		PolicyType: null
	};

	self.TeamQuoteTemplatesList = ko.observableArray([]);
	self.Team = ko.observable(new ConsoleApp.Team());

	self.BrokerRating = ko.observable("");
	self.BrokerScore = ko.observable("");
	self.BrokerCreditLimit = ko.observable("");
	self.RelatedCount = ko.observable("");
	self.DocumentCount = ko.observable("");
	self.WorldCheckCount = ko.observable("");
	self.CrossSellingCount = ko.observable("");
	
	self.NewBrokerContact = ko.observable("");
    
	self.Model = {
	    Id: ko.observable(0),
	    Timestamp: ko.observable(""),
	    Title: ko.observable("New Submission"),
	    UnderwriterNotes: ko.observable(""),
	    QuoteSheetNotes: ko.observable(""),
	    InsuredName: ko.observable(""),
	    InsuredId: ko.observable(""),
	    Broker: ko.observable(""),
	    BrokerCode: ko.observable(""),
	    BrokerSequenceId: ko.observable(""),
	    BrokerPseudonym: ko.observable(""),
	    BrokerContact: ko.observable(""),
	    Description: ko.observable(""),
	    _NonLondonBroker: ko.observable(""),
	    NonLondonBrokerCode: ko.observable(""),
	    NonLondonBrokerName: ko.observable(""),
	    _Underwriter: ko.observable(""),
	    UnderwriterCode: ko.observable(""),
	    _UnderwriterContact: ko.observable(""),
	    UnderwriterContactCode: ko.observable(""),
	    _Leader: ko.observable(""),
	    Leader: ko.observable(""),
	    _Domicile: ko.observable(""),
	    Domicile: ko.observable(""),
	    Brokerage: ko.observable(""),
	    _QuotingOffice: ko.observable(""),
	    QuotingOfficeId: ko.observable(""),
	    QuotingOffice: ko.observable(""),
	    Options: ko.observableArray([]),
	    SubmissionMarketWordingsList: ko.observableArray([]),
	    CustomSubmissionMarketWordingsList: ko.observableArray([]),
	    SubmissionTermsNConditionWordingsList: ko.observableArray([]),
	    CustomSubmissionTermsNConditionWordingsList: ko.observableArray([]),
	    SubmissionSubjectToClauseWordingsList: ko.observableArray([]),
	    CustomSubmissionSubjectToClauseWordingsList: ko.observableArray([]),
        AuditTrails:[]
	};

	//#region MarketWording
	self.Model.QuotingOfficeId.subscribe(function (officeId) {
        if (self.IsLoading()) return;
	    var ajaxConfig = { Url: "/Admin/GetMarketWordingsForTeamOffice?teamId=" + self.Team().Id() + "&officeId=" + officeId, VerbType: "GET", ContentType: "application/json;charset=utf-8" };
	    var response = ConsoleApp.AjaxHelper(ajaxConfig);
	    self.Model.SubmissionMarketWordingsList.removeAll();
	    response.success(function (data) {
	        if (data != null && data.length > 0) {
	            var tmpSubmissionMarketWordingsList = [];
	            $.each(data, function (key, value) {
	                tmpSubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                    .Id(value.MarketWording.Id)
	                    .DisplayOrder(value.DisplayOrder)
	                    .WordingRefNumber(value.MarketWording.WordingRefNumber)
	                    .Title(value.MarketWording.Title)
	                    
	                );
	            });
	            ko.utils.arrayPushAll(self.Model.SubmissionMarketWordingsList(), tmpSubmissionMarketWordingsList);
	            self.Model.SubmissionMarketWordingsList.valueHasMutated();
	        } else {
                console.log("Team-Office(change)- No MarketWordings Found");
	        }
	    });

	});

	
	self.selectedMarketWording = ko.observable(new ConsoleApp.MarketWording());
	self.showImageProcessing_LoadingMarketWordings = ko.observable('block');
	self.onClickMarketWordingItem = function (item) {
	    if (ko.isObservable(item)) {
	        self.selectedMarketWording(item);
	    } else {
	        self.selectedMarketWording(new ConsoleApp.MarketWording()
	            .Id(item.Id)
	            .WordingRefNumber(item.WordingRefNumber)
	            .Title(item.Title));
	    }
	};

	self.enableAddMarketWordingToSubmission = ko.computed(function () {
	    if (self.selectedMarketWording() !== undefined && self.selectedMarketWording().Id() != 0) {
	        return $.grep(self.Model.SubmissionMarketWordingsList(), function (i) { return i.Id() == self.selectedMarketWording().Id(); }).length == 0;
	    }
	    return false;
	}, self);
	self.click_AddMarketWordingToSubmission = function (e) {

	    if (self.selectedMarketWording() !== undefined && self.selectedMarketWording().Id() != 0) {
	        var result = $.grep(self.Model.SubmissionMarketWordingsList(), function (i) { return i.Id() == self.selectedMarketWording().Id(); });
	        if (result.length==0) {
	            self.Model.SubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                    .Id(self.selectedMarketWording().Id())
	                    .DisplayOrder(0)
	                    .WordingRefNumber(self.selectedMarketWording().WordingRefNumber())
	                    .Title(self.selectedMarketWording().Title())
	            );

	            self.selectedMarketWording(new ConsoleApp.MarketWording());
	        }
	        else {
	        }
	    }
	    else {
	    }

	};
	
	self.click_RemoveMarketWordingFromSubmission = function (e) {

	    if (self.selectedSubmissionMarketWording() !== undefined && self.selectedSubmissionMarketWording().Id() != 0) {

	        var removeItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function (item) {
	            return item.Id() == self.selectedSubmissionMarketWording().Id();
	        });

	        self.Model.SubmissionMarketWordingsList.remove(function (item) { return item.Id() == self.selectedSubmissionMarketWording().Id(); });
	        self.selectedSubmissionMarketWording(new ConsoleApp.MarketWordingSettingDto());

	    }
	    else {
	       
	    }

	};

	self.selectedSubmissionMarketWording = ko.observable(new ConsoleApp.MarketWordingSettingDto());

	self.enableRemoveMarketWordingFromSubmission = ko.computed(function () {
	    if (self.selectedSubmissionMarketWording() !== undefined && self.selectedSubmissionMarketWording().Id() != 0) {
	        return true;
	    }
	    return false;
	}, self);
	self.showImageProcessing_LoadingSubmissionMarketWordings = ko.observable('block');
	self.onClickSubmissionMarketWordingItem = function (item) {
	    self.selectedSubmissionMarketWording(item);
	};
	self.enableSelectedSubmissionMarketWordingMoveUp = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });
	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedSubmissionMarketWordingMoveUp = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });


	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        var array = self.Model.SubmissionMarketWordingsList();
	        self.Model.SubmissionMarketWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedSubmissionMarketWordingMoveDown = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });


	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.SubmissionMarketWordingsList().length != i + 1) {
	        return true;
	    }
	    return false;
	});
    
	self.onClick_selectedSubmissionMarketWordingMoveDown = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });


	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.SubmissionMarketWordingsList().length != i + 1) {
	        var array = self.Model.SubmissionMarketWordingsList();
	        self.Model.SubmissionMarketWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};

	self.customMarketWordingRef = ko.observable("");
	self.customMarketWording = ko.observable("");
	self.onClick_AddCustomMarketWordingToSubmission = function () {
	    if (self.customMarketWordingRef() !== undefined && self.customMarketWordingRef() != "") {
	        var result = $.grep(self.Model.CustomSubmissionMarketWordingsList(), function (i) { return i.WordingRefNumber() == self.customMarketWordingRef(); });
	        if (result.length==0) {
	            self.Model.CustomSubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                    .Id(0)
	                    .DisplayOrder(0)
	                    .WordingRefNumber(self.customMarketWordingRef())
	                    .Title(self.customMarketWording())
	            );
	            self.customMarketWordingRef("");
	            self.customMarketWording("");
	        }
	        else {
	        }
	    }
	    else {
	    }
	};
	self.onClick_RemoveCustomMarketWordingToSubmission = function () {
	    if (self.selectedCustomSubmissionMarketWording() !== undefined && self.selectedCustomSubmissionMarketWording().WordingRefNumber() != "") {
	        self.customMarketWordingRef(self.selectedCustomSubmissionMarketWording().WordingRefNumber());
	        self.customMarketWording(self.selectedCustomSubmissionMarketWording().Title());
	        self.Model.CustomSubmissionMarketWordingsList.remove(function (item) { return item.WordingRefNumber() == self.selectedCustomSubmissionMarketWording().WordingRefNumber(); });

	        self.selectedCustomSubmissionMarketWording(new ConsoleApp.MarketWordingSettingDto());

	    }
	    else {
	    }
	};
	self.enableAddCustomMarketWordingToSubmission = ko.computed(function () {
	    if (self.customMarketWordingRef() !== undefined && self.customMarketWordingRef() != "") {
	        return $.grep(self.Model.CustomSubmissionMarketWordingsList(), function (i) { return i.WordingRefNumber() == self.customMarketWordingRef(); }).length==0;
	    }
	    return false;
	}, self);

	
	self.selectedCustomSubmissionMarketWording = ko.observable(new ConsoleApp.MarketWordingSettingDto());
	self.enableRemoveCustomMarketWordingFromSubmission = ko.computed(function () {
	    if (self.selectedCustomSubmissionMarketWording() !== undefined && self.selectedCustomSubmissionMarketWording().WordingRefNumber() != "") {
	        return true;
	    }
	    return false;
	}, self);

	self.onClick_CustomSubmissionMarketWordingItem = function (item) {
	    self.selectedCustomSubmissionMarketWording(item);
	};
	self.enableSelectedCustomSubmissionMarketWordingMoveUp = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedCustomSubmissionMarketWordingMoveUp = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        var array = self.Model.CustomSubmissionMarketWordingsList();
	        self.Model.CustomSubmissionMarketWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedCustomSubmissionMarketWordingMoveDown = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.CustomSubmissionMarketWordingsList().length != i + 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedCustomSubmissionMarketWordingMoveDown = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.CustomSubmissionMarketWordingsList().length != i + 1) {
	        var array = self.Model.CustomSubmissionMarketWordingsList();
	        self.Model.CustomSubmissionMarketWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    //#endregion 
    
    //#region TermsNConditionWording
    
	self.Model.QuotingOfficeId.subscribe(function (officeId) {
        if (self.IsLoading()) return;
	    var ajaxConfig = { Url: "/Admin/GetTermsNConditionWordingsForTeamOffice?teamId=" + self.Team().Id() + "&officeId=" + officeId, VerbType: "GET", ContentType: "application/json;charset=utf-8" };
	    var response = ConsoleApp.AjaxHelper(ajaxConfig);
	    self.Model.SubmissionTermsNConditionWordingsList.removeAll();
        response.success(function (data) {
            if (data != null && data.length > 0) {
                var tempSubmissionTermsNConditionWordingsList = [];
                $.each(data, function (key, value) {
                    tempSubmissionTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWordingSettingDto()
	                    .Id(value.TermsNConditionWording.Id)
	                    .DisplayOrder(value.DisplayOrder)
                        .WordingRefNumber(value.TermsNConditionWording.WordingRefNumber)
	                    .IsStrikeThrough(value.IsStrikeThrough)
	                    .Title(value.TermsNConditionWording.Title)
	                );
                });
                ko.utils.arrayPushAll(self.Model.SubmissionTermsNConditionWordingsList(), tempSubmissionTermsNConditionWordingsList);
                self.Model.SubmissionTermsNConditionWordingsList.valueHasMutated();
	          
	        } else {
                console.log("Team-Office(change)- No TermsNConditionWordings Found");
	        }
	    });

	});

	self.selectedTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWording());
	self.showImageProcessing_LoadingTermsNConditionWordings = ko.observable('block');
    self.onClickCheckTermsNConditionWordingItem = function(item) {
        var result = $.grep(self.Model.SubmissionTermsNConditionWordingsList(), function(i) { return i.Id() == item.Id(); });
        if (result.length == 1) {

            if (result[0].IsStrikeThrough() == true) {
                result[0].IsStrikeThrough(false);
                item.IsStrikeThrough = false
            } else {
                {
                    result[0].IsStrikeThrough(true);
                    item.IsStrikeThrough = true
                }
            }
            self.Model.SubmissionTermsNConditionWordingsList.valueHasMutated();
        }
    };
	self.onClickTermsNConditionWordingItem = function (item) {
	    if (ko.isObservable(item)) {
	        self.selectedTermsNConditionWording(item);
	    } else {
	        self.selectedTermsNConditionWording(new ConsoleApp.TermsNConditionWording()
	            .Id(item.Id)
	            .WordingRefNumber(item.WordingRefNumber)
	            .Title(item.Title));
	    }
	};
	self.enableAddTermsNConditionWordingToSubmission = ko.computed(function () {
	    if (self.selectedTermsNConditionWording() !== undefined && self.selectedTermsNConditionWording().Id() != 0) {
	        return $.grep(self.Model.SubmissionTermsNConditionWordingsList(), function (i) { return i.Id() == self.selectedTermsNConditionWording().Id(); }).length == 0;
	    }
	    return false;
	}, self);
	self.click_AddTermsNConditionWordingToSubmission = function (e) {

	    if (self.selectedTermsNConditionWording() !== undefined && self.selectedTermsNConditionWording().Id() != 0) {

	            var result = $.grep(self.Model.SubmissionTermsNConditionWordingsList(), function (i) { return i.Id() == self.selectedTermsNConditionWording().Id(); });
	            if (result.length==0) {
	            self.Model.SubmissionTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWordingSettingDto()
	                    .Id(self.selectedTermsNConditionWording().Id())
	                    .DisplayOrder(0)
	                    .WordingRefNumber(self.selectedTermsNConditionWording().WordingRefNumber())
	                    .IsStrikeThrough(false)
	                    .Title(self.selectedTermsNConditionWording().Title())
	            );

	            self.selectedTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
	        }
	        else {
	        }
	    }
	    else {
	    }

	};
	self.click_RemoveTermsNConditionWordingFromSubmission = function (e) {

	    if (self.selectedSubmissionTermsNConditionWording() !== undefined && self.selectedSubmissionTermsNConditionWording().Id() != 0) {

	        var removeItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function (item) {
	            return item.Id() == self.selectedSubmissionTermsNConditionWording().Id();
	        });

	        self.Model.SubmissionTermsNConditionWordingsList.remove(function (item) { return item.Id() == self.selectedSubmissionTermsNConditionWording().Id(); });

	        self.selectedSubmissionTermsNConditionWording(new ConsoleApp.TermsNConditionWordingSettingDto());

	    }
	    else {
	    }

	};

	self.selectedSubmissionTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWordingSettingDto());
	self.enableRemoveTermsNConditionWordingFromSubmission = ko.computed(function () {
	    if (self.selectedSubmissionTermsNConditionWording() !== undefined && self.selectedSubmissionTermsNConditionWording().Id() != 0) {
	        return true;
	    }
	    return false;
	}, self);
	self.showImageProcessing_LoadingSubmissionTermsNConditionWordings = ko.observable('block');
	self.onClickSubmissionTermsNConditionWordingItem = function (item) {
	    self.selectedSubmissionTermsNConditionWording(item);
	};
    
	self.enableSelectedSubmissionTermsNConditionWordingMoveUp = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });
	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	       return true;
	    }
	    return false;
	});
	self.onClick_selectedSubmissionTermsNConditionWordingMoveUp = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });


	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        var array = self.Model.SubmissionTermsNConditionWordingsList();
	        self.Model.SubmissionTermsNConditionWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedSubmissionTermsNConditionWordingMoveDown = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });


	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.SubmissionTermsNConditionWordingsList().length != i + 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedSubmissionTermsNConditionWordingMoveDown = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });


	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.SubmissionTermsNConditionWordingsList().length != i + 1) {
	        var array = self.Model.SubmissionTermsNConditionWordingsList();
	        self.Model.SubmissionTermsNConditionWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    
	self.customTermsNConditionWordingRef = ko.observable("");
	self.customTermsNConditionWording = ko.observable("");
    self.onClickCheckCustomTermsNConditionWordingItem = function(item) {
        var result = $.grep(self.Model.CustomSubmissionTermsNConditionWordingsList(), function (i) { return i.WordingRefNumber() == item.WordingRefNumber; });
        if (result.length == 1) {

            if (result[0].IsStrikeThrough() == true) {
                result[0].IsStrikeThrough(false);
                item.IsStrikeThrough = false;
            } else {
                {
                    result[0].IsStrikeThrough(true);
                    item.IsStrikeThrough = true;
                }
            }
            self.Model.CustomSubmissionTermsNConditionWordingsList.valueHasMutated();
        }
    };
	self.onClick_AddCustomTermsNConditionWordingToSubmission = function () {
	    if (self.customTermsNConditionWording() !== undefined && self.customTermsNConditionWording() != "") {

	        var result = $.grep(self.Model.CustomSubmissionTermsNConditionWordingsList(), function (i) { return i.WordingRefNumber() == self.customTermsNConditionWordingRef(); });
	        if (result.length==0) {
	            self.Model.CustomSubmissionTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWordingSettingDto()
	                    .Id(0)
	                    .DisplayOrder(0)
	                    .IsStrikeThrough(false)
	                    .WordingRefNumber(self.customTermsNConditionWordingRef())
	                    .Title(self.customTermsNConditionWording())
	            );
	            self.customTermsNConditionWordingRef("");
	            self.customTermsNConditionWording("");
	        }
	        else {
	        }
	    }
	    else {
	    }
	};
	self.onClick_RemoveCustomTermsNConditionWordingToSubmission = function () {
	    if (self.selectedCustomSubmissionTermsNConditionWording() !== undefined && self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber() != "") {
	        self.customTermsNConditionWordingRef(self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber());
	        self.customTermsNConditionWording(self.selectedCustomSubmissionTermsNConditionWording().Title());
	        self.Model.CustomSubmissionTermsNConditionWordingsList.remove(function (item) { return item.WordingRefNumber() == self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber(); });

	        self.selectedCustomSubmissionTermsNConditionWording(new ConsoleApp.TermsNConditionWordingSettingDto());

	    }
	    else {
	    }
	};
	self.enableAddCustomTermsNConditionWordingToSubmission = ko.computed(function () {
	    if (self.customTermsNConditionWordingRef() !== undefined && self.customTermsNConditionWordingRef() != "") {
	        return $.grep(self.Model.CustomSubmissionTermsNConditionWordingsList(), function (i) { return i.WordingRefNumber() == self.customTermsNConditionWordingRef(); }).length == 0;
	    }
	    return false;
	}, self);
	
	self.selectedCustomSubmissionTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWordingSettingDto());
	self.enableRemoveCustomTermsNConditionWordingFromSubmission = ko.computed(function () {
	    if (self.selectedCustomSubmissionTermsNConditionWording() !== undefined && self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber() != "") {
	        return true;
	    }
	    return false;
	}, self);
	self.onClick_CustomSubmissionTermsNConditionWordingItem = function (item) {
	    self.selectedCustomSubmissionTermsNConditionWording(item);
	};

	self.enableSelectedCustomSubmissionTermsNConditionWordingMoveUp = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });


	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedCustomSubmissionTermsNConditionWordingMoveUp = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });


	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        var array = self.Model.CustomSubmissionTermsNConditionWordingsList();
	        self.Model.CustomSubmissionTermsNConditionWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedCustomSubmissionTermsNConditionWordingMoveDown = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });
	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.CustomSubmissionTermsNConditionWordingsList().length != i + 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedCustomSubmissionTermsNConditionWordingMoveDown = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });


	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.CustomSubmissionTermsNConditionWordingsList().length != i + 1) {
	        var array = self.Model.CustomSubmissionTermsNConditionWordingsList();
	        self.Model.CustomSubmissionTermsNConditionWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    
    //#endregion 

    //#region SubjectToClauseWording
	self.Model.QuotingOfficeId.subscribe(function (officeId) {
        if (self.IsLoading()) return;
	    var ajaxConfig = { Url: "/Admin/GetSubjectToClauseWordingsForTeamOffice?teamId=" + self.Team().Id() + "&officeId=" + officeId, VerbType: "GET", ContentType: "application/json;charset=utf-8" };
	    var response = ConsoleApp.AjaxHelper(ajaxConfig);
	    self.Model.CustomSubmissionSubjectToClauseWordingsList.removeAll();
	    response.success(function (data) {
	        if (data != null && data.length > 0) {
	            var tmpCustomSubmissionSubjectToClauseWordingsList = [];
	            $.each(data, function (key, value) {
	                tmpCustomSubmissionSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWordingSettingDto()
	                    .Id(0)
	                    .DisplayOrder(value.DisplayOrder)
	                    .IsStrikeThrough(value.IsStrikeThrough)
	                    .Title(value.SubjectToClauseWording.Title)
	                );
	            });
	            ko.utils.arrayPushAll(self.Model.CustomSubmissionSubjectToClauseWordingsList(), tmpCustomSubmissionSubjectToClauseWordingsList);
	            self.Model.CustomSubmissionSubjectToClauseWordingsList.valueHasMutated();
	        } else {
                console.log("Team-Office(change)- No SubjectToClauseWordings Found");
	        }
	    });
	});


	
	self.selectedSubjectToClauseWording = ko.observable(new ConsoleApp.SubjectToClauseWording());
	self.showImageProcessing_LoadingSubjectToClauseWordings = ko.observable('block');
	self.customSubjectToClauseWording = ko.observable("");
	self.onClick_AddCustomSubjectToClauseWordingToSubmission = function () {
	    if (self.customSubjectToClauseWording() !== undefined && self.customSubjectToClauseWording() != "") {

	            var result = $.grep(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function (i) { return i.Title() == self.customSubjectToClauseWording(); });
	            if (result.length==0) {
	            self.Model.CustomSubmissionSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWordingSettingDto()
	                    .Id(0)
	                    .DisplayOrder(0)
	                    .IsStrikeThrough(false)
	                    .Title(self.customSubjectToClauseWording())
	            );

	            self.customSubjectToClauseWording("");
	        }
	        else {
	        }
	    }
	    else {
	    }
	};
	self.onClick_RemoveCustomSubjectToClauseWordingToSubmission = function () {
	    if (self.selectedCustomSubmissionSubjectToClauseWording() !== undefined && self.selectedCustomSubmissionSubjectToClauseWording().Title() != "") {

	        self.Model.CustomSubmissionSubjectToClauseWordingsList.remove(function (item) { return item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title(); });

	        self.customSubjectToClauseWording(self.selectedCustomSubmissionSubjectToClauseWording().Title());
	        self.selectedCustomSubmissionSubjectToClauseWording(new ConsoleApp.SubjectToClauseWordingSettingDto());
	    }
	    else {
	    }
	};
	self.enableAddCustomSubjectToClauseWordingToSubmission = ko.computed(function () {
	    if (self.customSubjectToClauseWording() !== undefined && self.customSubjectToClauseWording() != "") {
	        return $.grep(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function (i) { return i.Title() == self.customSubjectToClauseWording(); }).length == 0;
	    }
	    return false;
	}, self);
	self.selectedCustomSubmissionSubjectToClauseWording = ko.observable(new ConsoleApp.SubjectToClauseWordingSettingDto());
	self.enableRemoveCustomSubjectToClauseWordingFromSubmission = ko.computed(function () {
	    if (self.selectedCustomSubmissionSubjectToClauseWording() !== undefined && self.selectedCustomSubmissionSubjectToClauseWording().Title() != "") {
	        return true;
	    }
	    return false;
	}, self);

	self.onClickCheckCustomSubjectToClauseWordingItem = function (item) {
	    var result = $.grep(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function (i) { return i.Title() == item.Title; });
	    if (result.length == 1) {

	        if (result[0].IsStrikeThrough() == true) {
	            result[0].IsStrikeThrough(false);
	            item.IsStrikeThrough = false;
	        } else {
	            {
	                result[0].IsStrikeThrough(true);
	                item.IsStrikeThrough = true;
	            }
	        }
	        self.Model.CustomSubmissionSubjectToClauseWordingsList.valueHasMutated();
	    }
	};

	self.onClick_CustomSubmissionSubjectToClauseWordingItem = function (item) {
	    self.selectedCustomSubmissionSubjectToClauseWording(item);
	};
	self.enableSCustomSubmissionSubjectToClauseWordingMoveUp = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });
	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedCustomSubmissionSubjectToClauseWordingMoveUp = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });


	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
	    if (i >= 1) {
	        var array = self.Model.CustomSubmissionSubjectToClauseWordingsList();
	        self.Model.CustomSubmissionSubjectToClauseWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedCustomSubmissionSubjectToClauseWordingMoveDown = ko.computed(function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });
	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.CustomSubmissionSubjectToClauseWordingsList().length != i + 1) {
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedCustomSubmissionSubjectToClauseWordingMoveDown = function () {
	    var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function (item) {
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });


	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
	    if (i >= 0 && self.Model.CustomSubmissionSubjectToClauseWordingsList().length != i + 1) {
	        var array = self.Model.CustomSubmissionSubjectToClauseWordingsList();
	        self.Model.CustomSubmissionSubjectToClauseWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    //#endregion



	// Events
	
    self.Initialise = function (viewModel, syncCallback) {
		console.log('Initialise - VM');
		console.log(viewModel);

		if (!domId) {
		    toastr.error("No DOM Id specified, cannot initialise Submission view model");

		    self.Cancel();
		} else {
		    self.SearchWorldCheck("");
		    self.SetInsuredLossRatios("");
		    self.SetBrokerLossRatios("");
		    self.SetCreateBrokerContact();
		    self.PopAuditTrails();
		    
			$.when(self.GetTeamBySubmissionTypeId(self.Model.submissionTypeId())).done(function () {
		                self.GetQuoteTemplates(self.Team().Id());
			});
		                        		self.SetDefaultValues();
		    if (!viewModel) {
		                                self.Model.Options().push(new Option(0, domId, self));
		                            }
		                            self.DirtyFlag = new ko.dirtyFlag(self);
		                            self.BindKO(viewModel);
		                            if (self.Id > 0) {
		                                setTimeout(function () { self.LoadSubmission("/submission/GetSubmission", syncCallback); },100);

		                            } else {
		                                setTimeout(function () { self.InitialiseForm(); },100);
		        };
		      
		}
    };

    self.LoadSubmission = function (url, syncCallback) {
		$.ajax(
		{
			url: url,
			type: "GET",
			data: { id: self.Id },
			dataType: "json",
			contentType: "application/json",
		    success: function (data, status, xhr) {
		        self.IsLoading(true);
				self.CanCreateQuoteSheet(true);
				self.IsSaving(false);
				self.ValidationErrors("");

				toastr.success("Submission retrieved");

		        if (data.Submission) {
				    self.syncJSON(data.Submission);
					if (syncCallback)
						syncCallback(data.Submission);
		                self.ShowAuditTrails(self.Model.Id());
					    self.InitialiseForm();
		                self.IsLoading(false);
					toastr.success("Submission synchronised");
		        } else {
		            self.IsLoading(false);
			    }
		    }
			//error: function... TODO: Implement data.ErrorMessages ...Not found ?
		});
	};

    self.InitialiseForm = function () {
		self.AttachValidation();
		self.InitialisePane();
		self.SetDocumentDetails();
		self.BindUnload();
		self.DirtyFlag.Reset();
		self.IsInitialised(true);
	};

    self.SetDefaultValues = function () {
    	$.when(self.GetTeamBySubmissionTypeId(self.Model.submissionTypeId()))
    		.done(function()
    		{
    			
    		var defaultDomicile = $("input[type='hidden'][name='DefaultDomicile']", self.Form).val(),
             defaultQuoteExpiry = $("input[type='hidden'][name='DefaultQuoteExpiry']", self.Form).val(),
             defaultOffice = $("input[type='hidden'][name='DefaultOffice']", self.Form).val(),
             defaultUnderwriter = $("input[type='hidden'][name='DefaultUnderwriter']", self.Form).val(),
             defaultPolicyType = $("input[type='hidden'][name='DefaultPolicyType']", self.Form).val();


            if (defaultDomicile) {
                self.Defaults.Domicile = defaultDomicile;

                self.Model._Domicile(defaultDomicile);
            }

            if (defaultUnderwriter) {
                self.Defaults.Underwriter = defaultUnderwriter;

                self.Model._Underwriter(defaultUnderwriter);
                self.Model._UnderwriterContact(defaultUnderwriter);
            }

            if (defaultOffice)
            {
                self.Defaults.Office = defaultOffice;

                self.Model._QuotingOffice(defaultOffice);
            }

            if (defaultPolicyType)
            {
                self.Defaults.PolicyType = defaultPolicyType;
            }

            defaultQuoteExpiry = parseInt(defaultQuoteExpiry);

            if (!isNaN(defaultQuoteExpiry))
            {
                var quoteExpiryDate = moment().add("d", defaultQuoteExpiry);

                if (quoteExpiryDate && quoteExpiryDate.isValid())
                    self.Defaults.QuoteExpiry = quoteExpiryDate.format("YYYY-MM-DD");
            }

        });
        
//$(function () {
//    function checkPendingRequest() {
//        if ($.active > 0) {
//            window.setTimeout(checkPendingRequest, 1000);
//        } else {

		
                    
//            }
//    };
//    window.setTimeout(checkPendingRequest, 1000);
//});
	
	};

    self.SetDocumentDetails = function () {
		/* 
		TODO: Changing this computed to a subsription as well as SetInsuredDetails & SetBrokerDetails
    	$(".val-submission-documents", self.Form).html("");
	    */

		var policyIds = "";

        $.each(self.Model.Options(), function (optionIndex, optionData) {
            $.each(optionData.OptionVersions(), function (versionIndex, versionData) {
                $.each(versionData.Quotes(), function (quoteIndex, quoteData) {
                    if (quoteData.RenPolId() !== "") {
						policyIds += quoteData.RenPolId() + ";";
					}

                    if (quoteData.SubscribeReference() !== "") {
						policyIds += quoteData.SubscribeReference() + ";";
					}
				});
			});
		});

		$.ajax(
		{
			url: "/uwdocument/_search",
			type: "GET",
			data: { term: encodeURIComponent(policyIds) },
			dataType: "html",
		    success: function (data) {
				self.DocumentCount(self.CountIndication(data));

				var tableConfig = new BaseDataTable();

				tableConfig.bProcessing = false;
				tableConfig.aaSorting = [[2, "desc"]];

				$(".val-submission-documents", self.Form).html(data);
		        $(".val-submission-documents .val-subscribe-policyid", self.Form).click(function (e) {
					OpenWebPolicy($(e.target).attr("href"));

					e.preventDefault();
				});
		        $(".val-submission-documents .val-filenet-document", self.Form).click(function (e) {
					window.open($(e.target).attr("href"));

					e.preventDefault();
				});
				$(".val-submission-documents table", self.Form).dataTable(tableConfig);
			}
		});
	};
	
    self.GetTeamBySubmissionTypeId = function (submissionTypeId) {

		var ajaxConfig = { Url: "/Admin/GetTeamBySubmissionTypeId?submissionTypeId=" + submissionTypeId, VerbType: "GET" };

		var response = ConsoleApp.AjaxHelper(ajaxConfig);

        response.success(function (data) {
			self.Team(new ConsoleApp.Team(data.Id, data.Title));
			console.log(self.Team());
		});

        response.fail(function (jqXhr, textStatus) {
			toastr["error"]("An Error Has Occurred!");
			console.log(jqXhr + " " + textStatus);
		});

		return response;
    };
    
    self.PopAuditTrails = function () {
        $('.showAuditTrail').popover({
            html: true,
            content: function() {

                return '<div class="AuditTrailPoped">' + $('.val-submission-insuredName-audits', self.Form).html() + '</div>';

            },
            afterShowed:function()
            {
                var tableConfig = new BaseDataTable();
                tableConfig.bSort = false;
                $(".val-worldcheck-auditTrails-datatable", $(".AuditTrailPoped", self.Form)).dataTable(tableConfig);
            },
            trigger: 'click',
            placement: 'bottom',
            template: '<div class="popover" style="width:400px"><div class="arrow"></div><div class="popover-inner" style="width:400px"><div class="popover-content"><p></p></div></div></div>'
           
    });

    };

	self.SetCreateBrokerContact = function() {
		$('.addNewBrokerContact', self.Form).popover({
			html: true,
			content: function()
			{
				return $('.popover_content_wrapper', self.Form).html();
			},
			trigger: 'click'
		});
		
		$(document).on("click", ".createBrokerContact", function()
		{
			var brokerContact = $(".newBrokerContact", self.Form).val();
			self.CreateBrokerContact(brokerContact);
		});
	};

    self.CreateBrokerContact = function(brokerContact)
    {
    	console.log(brokerContact);

    	var ajaxConfig = { Url: "/Broker/CreateBrokerContact", VerbType: "POST", Data: ko.toJSON({ NewBrokerContact: brokerContact }) };

		var response = ConsoleApp.AjaxHelper(ajaxConfig);

		response.success(function(data)
		{
			console.log(data);
			toastr["info"]("Broker Contact created");
			self.Model.BrokerContact(brokerContact);
			$('.addNewBrokerContact').popover('hide');
		});

		response.fail(function(jqXhr, textStatus)
		{
			toastr["error"]("An Error Has Occurred!");
			console.log(jqXhr + " " + textStatus);
		});
	};

    self.GetQuoteTemplates = function (teamId) {
		self.TeamQuoteTemplatesList.removeAll();

		var ajaxConfig = { Url: "/Admin/GetQuoteTemplatesForTeam?teamId=" + teamId, VerbType: "GET" };

		var response = ConsoleApp.AjaxHelper(ajaxConfig);

        response.success(function (data) {

            if (data.length > 0) {
                $.each(data, function (key, value) {
					self.TeamQuoteTemplatesList.push(new ConsoleApp.QuoteTemplate()
						.Id(value.Id)
						.Name(value.Name)
						.RdlPath(value.RdlPath));
				});
			}
            else {
				toastr["info"]("No Quote Templates for Team");
			}

		});

        response.fail(function (jqXhr, textStatus) {
			toastr["error"]("An Error Has Occurred!");
			console.log(jqXhr + " " + textStatus);
		});

		return response;
	};
	
    self.CreateFirstOption = function(vm)
    {
    	self.SetDefaultValues();
		self.Model.Options().push(new Option(0, domId, self));
		var vmOption1 = self.Model.Options()[0];
		return vmOption1;
	};
	
    self.SetInsuredTabLabel = function (insuredName) {
		$("a[href$='" + domId + "']:first")
			.attr("title", insuredName).attr("alt", insuredName)
			.children("span")
			.text((!insuredName)
				? "New Submission"
				: (insuredName.length > 14)
					? insuredName.substring(0, 12) + "..."
					: insuredName);

		$("a[href$='" + domId + "']:first button")
			.unbind("click")
			.bind("click", function () {
				self.Cancel();
			});
	};

	/*
		TODO: Cleanup

		We should really be using an InitPV_ function.
	*/
    self.SetInsuredLossRatios = function (insuredName) {
		$(".val-worldcheck-matches", self.Form).html("");

		$.ajax(
		{
			url: "/insured/_insureddetailsminimal",
			type: "GET",
			data: { insuredName: encodeURIComponent(insuredName) },
			dataType: "html",
		    success: function (data) {
				self.RelatedCount(self.CountIndication(data));

				var tableConfig = new BaseDataTable();

				tableConfig.aaSorting = [[1, "desc"]];

				$(".val-related-loss-ratios", self.Form).html(data);
				$(".val-insured-minimal-datatable", self.Form).dataTable(tableConfig);
			}
		});
	};
    
    self.CountIndication = function (data) {
		var count = (data) ? data.match(/<tr>/gi).length - 1 : 0;

		return (count > 0) ? "(" + count + ")" : "";
	};

	/*
		TODO: Cleanup

		This was added quickly for a demonstration and has not been improved upon since.
		We should really be using an InitPV_ function.
	*/
    self.SetBrokerSummary = function (brokerGroupCode) {
		self.BrokerRating('');
		self.BrokerScore('');
		self.BrokerCreditLimit('');

		if (brokerGroupCode === "") return;

		console.log(brokerGroupCode);
		$.ajax(
			{
				url: "/broker/getBrokerSummaryById",
				type: "GET",
				data: { brokerCd: brokerGroupCode },
				dataType: "json",
				contentType: "application/json",
			    success: function (data) {
					console.log(data.BrokerRating);
					console.log(data.BrokerScore);
					console.log(data.BrokerCreditLimit);
					self.BrokerRating(data.BrokerRating ? data.BrokerRating : '');
					self.BrokerScore(data.BrokerScore ? data.BrokerScore : '');
					self.BrokerCreditLimit(data.BrokerCreditLimit ? data.BrokerCreditLimit : '');
				},
			    error: function (xhr, status, error) {
					toastr.error("Error: " + error);
				}
			});
	};

    self.click_SetBrokerLossRatios = function () {
        self.SetBrokerLossRatios(self.Model.BrokerPseudonym());
    };
    self.SetBrokerLossRatios = function (brokerGroupCode) {
		$(".val-broker-lossratio-sparkline", self.Form).html("");
		$(".val-broker-lossratio-graph", self.Form).html("");

        if (brokerGroupCode !== "") {
			$(".val-broker-lossratio-loading", self.Form).show();

			$.ajax(
			{
				url: "/broker/getbrokerdevelopmentstatsbyid",
				type: "GET",
				data: { brokerCd: brokerGroupCode },
				dataType: "json",
				contentType: "application/json",
			    success: function (data) {
					$(".val-broker-lossratio-loading", self.Form).hide();
					$(".val-broker-lossratio-sparkline", self.Form).html("");
					$(".val-broker-lossratio-graph", self.Form).html("");

					if (!data.Months)
					{
						$(".val-broker-lossratio-sparkline", self.Form).html("No Broker Loss Ratio's found");
					    return;
				    }

				    var brokerSparklines = $(".val-broker-lossratio-sparkline", self.Form),
						brokerSeries = [];

			        $.each(data.LossRatioForMonthList, function (key, item) {
						$('<br /><b><span>' + item.Year + '</span></b>').appendTo(brokerSparklines);

						$('<span></span>').appendTo(brokerSparklines).kendoSparkline({ type: "area", theme: "blueOpal", data: item.LossRatios });

						brokerSeries.push({ name: item.Year, data: item.LossRatios });
					});

					$(".val-broker-lossratio-graph", self.Form).kendoChart(
					{
						theme: "blueOpal",
						title: { text: "Loss Ratio Per Month", visible: false },
						legend: { position: "bottom" },
						chartArea: { background: "" },
						seriesDefaults: { type: "line" },
						series: brokerSeries,
						valueAxis:
						{
							labels: { format: "{0}" },
							line: { visible: false },
							axisCrossingValue: -10
						},
						categoryAxis:
						{
							categories: data.Months,
							majorGridLines: { visible: false }
						},
						tooltip:
						{
							format: "{0}%",
							template: "#= series.name #: #= value #",
							visible: true
						}
					});

					$(".val-broker-lossratio-loading", self.Form).hide();
				},
			    error: function (xhr, status, error) {
					toastr.error("Error: " + error);

					$(".val-broker-lossratio-loading", self.Form).hide();
				}
			});
		}
        else {
			$(".val-broker-lossratio-loading", self.Form).hide();
		}
	};

    self.CrossSellingCheck = function(insuredName) {
    	var loadingImage = "<p class='image_loading_WorldCheck'><img style='width: 20px; height: 20px;' src='/Content/images/animated_progress.gif'> Loading... </p>";
    	$(".val-cross-selling", self.Form).html(loadingImage);

    	$.ajax(
		{
			url: "/submission/_CrossSellingCheck",
			type: "GET",
			data: { insuredName: encodeURIComponent(insuredName), thisSubmissionId: self.Id },
			dataType: "html",
			success: function(data) {
				
				var count = ((data.match(/<tr>/g) || []).length - 1);

				self.CrossSellingCount((count === 0) ? '' : '(' + count + ')');

				var tableConfig = new BaseDataTable();
				tableConfig.aaSorting = [[0, "desc"]];

				$(".val-cross-selling", self.Form).html(data);
				$(".val-cross-selling-datatable", self.Form).dataTable(tableConfig);

				if (count !== 0) {
					$('.crossSellingLink', self.Form).effect('pulsate', null, 200, null);
					$('.crossSellingLink', self.Form).effect('highlight', { color: '#a1eda7' }, 5000, null);
				}

				SetupEditSubmissionBtn(".val-edit-submission-cross-selling");
			}
		});
    };

    self.SearchWorldCheck = function (insuredName) {
    	$(".val-related-loss-ratios", self.Form).html("");

    	var loadingImage = "<p class='image_loading_WorldCheck'><img style='width: 20px; height: 20px;' src='/Content/images/animated_progress.gif'> Loading... </p>";
    	$(".val-worldcheck-matches", self.Form).html(loadingImage);

		$.ajax(
		{
			url: "/worldcheck/_worldchecksearchmatches",
			type: "GET",
			data: { insuredName: encodeURIComponent(insuredName) },
			dataType: "html",
		    success: function (data) {
		    	
		    	var count = ((data.match(/<tr>/g) || []).length - 1);

		    	self.WorldCheckCount((count === 0) ? '' : '(' + count + ')');

		    	var tableConfig = new BaseDataTable(null, "<p>For full functionality, click here: <a href=" + WorldCheckURL + "  target='_blank'>World Check</a></p> <p>Please contact Compliance for any queries.</p>");

				tableConfig.aaSorting = [[0, "desc"]];
				tableConfig.aoColumnDefs = [{ "bSortable": false, "aTargets": [1] }];

				$(".val-worldcheck-matches", self.Form).html(data);
				$(".val-worldcheck-matches-datatable", self.Form).dataTable(tableConfig);

			    if (count !== 0) {
				    $('.worldCheckLink', self.Form).effect('pulsate', null, 200, null);
				    $('.worldCheckLink', self.Form).effect('highlight', { color: '#eda1b3' }, 5000, null);
			    }
		    }
		});
	    
        if (insuredName != null) {
            self.Model.AuditTrails.push(new ConsoleApp.AuditTrail(0, "Submission", "", "World Check", "World Check requested for insured name: " + insuredName));
        }
    };

    self.ShowLossRatioGraph = function () {
        if ($(".val-broker-lossratio-graph", self.Form).html().length > 0) {
			$(".val-broker-lossratio-modal", self.Form).modal("show");
		}
	};
    self.ShowAuditTrails = function (submissionId) {
        $(".val-submission-insuredName-audits", self.Form).html("");

        $.ajax(
		{
		    url: "/Submission/_WorldCheckAuditTrailForSubmission",
		    type: "GET",
		    data: { id: submissionId },
		    dataType: "html",
		    success: function (data) {
		       // var tableConfig = new BaseDataTable();
		       // tableConfig.aaSorting = [[0, "desc"]];
		       // tableConfig.aoColumnDefs = [{ "bSortable": false, "aTargets": [1] }];
		        
		        $(".val-submission-insuredName-audits", self.Form).html(data);
		       // $(".val-worldcheck-auditTrails-datatable", self.Form).dataTable(tableConfig);
		    }
		});
      
    };
    self.BindKO = function (viewModel) {

		if (viewModel === undefined)
			ko.applyBindings(self, document.getElementById(domId));
		else 
			ko.applyBindings(viewModel, document.getElementById(domId));
	};

    self.BindUnload = function () {
		/* TODO: Get this working for all browsers, including IE 9
		if ($.support.leadingWhitespace) // False for IE < 9
		{
			$(window).unbind("beforeunload." + domId)
				.bind("beforeunload." + domId, self.UnloadConfirmation);
		}*/
	};

    self.UnbindUnload = function () {
		$(window).unbind("beforeunload." + domId);
	};

    self.UnloadConfirmation = function (e) {
		var isDirty = (self.DirtyFlag) ? self.DirtyFlag.IsDirty() : false;

        if (e) {
            if (isDirty) {
				toastr.warning("Unsaved Submission changes detected");

				return "You have unsaved changes!";
			}

			return null;
		}
        else if (isDirty) {
			toastr.warning("Unsaved Submission changes detected");

			return window.confirm("You have unsaved changes!\n\rAre you sure you wish to leave ?");
		}

		return true;
	};

    self.AttachValidation = function () {
		/* TODO: Enable once we are ready
        $.validator.unobtrusive.parse("#" + domId + " .form");
		*/
	};

	// Note: done at the inheriting submission level to add new properties at quote level etc...
    self.AddOption = function () {
		var length = self.Model.Options().length,
		    newOption = new Option(length, domId, self),
			submissionId = self.Model.Id();

		newOption.SubmissionId(submissionId);

		length = self.Model.Options.push(newOption);

		self.AttachValidation();

		return length;
	};

    self.CopyOption = function () {
		var length = self.Model.Options().length,
		    optionIndex = self.OptionIndexToCopy(),
		    originalOption = self.Model.Options()[optionIndex],
		    originalVersion = originalOption.CurrentVersion(),
		    newOptionData = originalOption.koTrim(),
		    newVersionData = originalVersion.koTrim(),
			newOption = new Option(length, domId, self),
	        newOptionTitle = newOption.Title();

		newOptionData.Id = 0;
		newOptionData.Title = newOptionTitle;

		newVersionData.OptionId = 0;
		newVersionData.VersionNumber = 0;
		newVersionData.Title = "Version 1";
		newVersionData.IsLocked = false;

        $.each(newVersionData.Quotes, function (quoteIndex, quoteItem) {
			quoteItem.Id = 0;
			quoteItem.OptionId = 0;
			quoteItem.VersionNumber = 0;
			quoteItem.SubmissionStatus = "SUBMITTED";
			quoteItem.IsSubscribeMaster = false;

			quoteItem.BenchmarkPremium = "";
			quoteItem.TechnicalPremium = "";
			quoteItem.QuotedPremium = "";
			quoteItem.LimitAmount = "";
			quoteItem.ExcessAmount = "";
		});

		newOptionData.OptionVersions = [newVersionData];

		newOption.syncJSON(newOptionData);

		length = self.Model.Options.push(newOption);

		self.AttachValidation();

		return length;
	};

	self.QuoteSheetCreationCheck = function(element, e)
	{
		if (self.DirtyFlag.IsDirty())
		{
		    self.Save(element, e, function () { self.CreateQuoteSheet(element); });
		}
		else
		{
			self.CreateQuoteSheet(element);
		}
	};

    self.CreateQuoteSheet = function (element, e) {
		var quoteSheetTemplateId;
        if (self.TeamQuoteTemplatesList().length === 0) {
			toastr.warning("No Templates for quote sheet");
			return;
		}
        else if (self.TeamQuoteTemplatesList().length === 1) {
			quoteSheetTemplateId = self.TeamQuoteTemplatesList()[0].Id();
		}
        else {
			quoteSheetTemplateId = element.Id();
		}

		var optionList = [];
        $.each(self.Model.Options(), function (optionIndex, optionItem) {
            if (optionItem.AddToQuoteSheet()) {
				optionList.push(
				{
					OptionId: optionItem.Id(),
					OptionVersionNumberList: [optionItem.CurrentVersion().VersionNumber()]
				});
			}
		});

        if (optionList.length === 0) {
			toastr.warning("No options selected for quote sheet");
			return;
		}

		self.CanCreateQuoteSheet(false);
		self.CreatingQuoteSheet(true);

		$.ajax(
		{
			url: "/quotesheet/CreateQuote",
			type: "POST",
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify(
			{
				QuoteSheetTemplateId: quoteSheetTemplateId,
				SubmissionId: self.Model.Id(),
				OptionList: optionList
			}),
		    success: function (data, status, xhr) {
				toastr.info("Quote sheet created");

				self.SetDocumentDetails();

		        if (data.Submission) {
					toastr.success("Submission updated");

					self.syncJSON(data.Submission);
					self.DirtyFlag.Reset();

					toastr.success("Submission synchronised");
				}

				var responseLocation = xhr.getResponseHeader("Location");

		        if (responseLocation) {
					window.open(responseLocation, "_blank");
				}
			},
		    complete: function (xhr, status) {
				self.CanCreateQuoteSheet(true);
				self.CreatingQuoteSheet(false);
			}
		});
	};

    self.ToggleComparison = function () {
        $(".val-optioncomparison-datatable", $("#" + domId)).each(function () {
			if ($(this).is(":visible")) $(this).hide();
			else $(this).show();
		});
	};

	/* TODO: Not a simple task to get working
		
		Biggest problem is manipulating the ko.bindingHandlers for the Bootstrap 
		Carousel and Tabs.

	self.Reset = function()
	{
		if (self.DirtyFlag.IsDirty())
		{
			self.DirtyFlag.Resynchronise();
			
			toastr.info("Submission resynchronised");
		}
		else toastr.info("No changes to Submission");
	};
	*/

    self.Cancel = function () {
        if (self.UnloadConfirmation(null)) {
			self.UnbindUnload();

			// TODO: Kill view model ?

			Val_CloseTab(domId);
		}
	};

	self.Save = function(element, e, callback, url, syncCallback)
	{
		self.IsSaving(true);
		self.CanCreateQuoteSheet(false);

		if (self.Form.valid())
		{
			var modelJSON = self.toJSON(),
			    isNew = (self.Id === 0);

			toastr.info("Saving Submission");

			$.ajax(
			{
					url: url ? url : (!isNew) ? "/api/submissionapi/EditSubmission" : "/api/submissionapi/CreateSubmission",
					headers: { 'X-SubmissionType': self.Model.submissionTypeId() },
					type: (!isNew) ? "PUT" : "POST",
					data: modelJSON,
					dataType: "json",
					contentType: "application/json",
					success: function(data, status, xhr)
					{
						self.CanCreateQuoteSheet(true);
						self.IsSaving(false);
						self.ValidationErrors("");

						$.each(self.Model.Options(), function(optionIndex, optionItem)
						{
							$.each(optionItem.OptionVersions(), function(versionIndex, versionItem)
							{
								$.each(versionItem.Quotes(), function(quoteIndex, quoteItem)
								{
									quoteItem.ValidationErrors("");
								});
							});
						});

						if (isNew) toastr.success("Submission created");
						else toastr.success("Submission edited");
						
						self.IsLoading(true);
						
						if (data)
						{
							self.syncJSON(data);
							if (syncCallback) syncCallback(data);
							self.ShowAuditTrails(self.Model.Id());
							toastr.success("Submission synchronised");
						}
						
						self.DirtyFlag.Reset();
						
						if (callback) callback(element, e);
						
						self.IsLoading(false);

						if (self.Model.Options()[0].OptionVersions()[0].Quotes()[0].RenPolId() != "")
						{
							$.pubsub.publish("policyRenewed",
							{
								RenPolId: self.Model.Options()[0].OptionVersions()[0].Quotes()[0].RenPolId()
							});

						}
					},
					error: function(xhr, status, error)
					{
						self.IsSaving(false);

						toastr.error("Saving Submission failed");

						var jsonData = JSON.parse(xhr.responseText),
						    errorHTML = "";

						if (xhr.status === 400)
						{
							if (jsonData.Error)
							{
								$.each(self.Model.Options(), function(optionIndex, optionItem)
								{
								    var optionPattern = "submission.Options[\\[]" + optionIndex + "[\\]]";

									$.each(optionItem.OptionVersions(), function(versionIndex, versionItem)
									{
										var versionPattern = optionPattern + "[.]OptionVersions[\\[]" + versionIndex + "[\\]]";

										$.each(versionItem.Quotes(), function(quoteIndex, quoteItem)
										{
											var quotePattern = versionPattern + "[.]Quotes[\\[]" + quoteIndex + "[\\]]",
											    errorPattern = quotePattern + "[^\"]*",
											    errorRegEx = new RegExp(errorPattern, "g"),
											    errorMatches = xhr.responseText.match(errorRegEx);

											errorHTML = "";

											$(errorMatches).each(function(matchIndex, errorMatch)
											{
												$(jsonData.Error[errorMatch]).each(function(errorIndex, errorMessage)
												{
													errorHTML += "<li>" + errorMessage + "</li>";

													// We do not want to display these errors again
													delete jsonData.Error[errorMatch]; // TODO: Can we delete in the same loop ?
												});
											});

											quoteItem.ValidationErrors(errorHTML);
										});
									});
								});

								errorHTML = "";

								$.each(jsonData.Error, function(propertyIndex)
								{
									$(jsonData.Error[propertyIndex]).each(function(errorIndex, errorMessage)
									{
										/* 
											Bug fix 309
											Hide other broker errors as these are not displayed on the UI and may
											confuse the users.
										*/
										var brokerError1 = /BrokerSequenceId/gi,
										    brokerError2 = /BrokerPseudonym/gi;

										if (!brokerError1.test(propertyIndex) && !brokerError2.test(propertyIndex))
										{
											errorHTML += "<li>" + errorMessage + "</li>";
										}
									});
								});
							}
							else
							{
								errorHTML += "<li>No errors returned, please contact the administrator</li>";
							}

							self.ValidationErrors(errorHTML);
						}
					}
				});
		}
		else
		{
			// TODO: This is probably not sufficient...
			toastr.warn("Submission validation failed");
		}
	};

    self.CanCreateQuoteSheetCheck = ko.computed(function () {
		var canCreate = false;

        $.each(self.Model.Options(), function (optionIndex, optionItem) {
            $.each(optionItem.CurrentVersion().Quotes(), function (quoteIndex, quoteItem) {
				canCreate = /\S/g.test(quoteItem.SubscribeReference());

				return canCreate;
			});

			return canCreate;
		});

		self.CanCreateQuoteSheet(canCreate);
	}, self);

	// Subscriptions / Computed
    self.Model.InsuredName.subscribe(function (value) {
		var values = (value) ? value.split(":") : [],
            insuredName = (values[0]) ? values[0].trim() : "",
            insuredId = (values[1]) ? values[1].trim() : "0";

		self.Model.InsuredId(insuredId);

		self.SetInsuredLossRatios(insuredName);
		self.SearchWorldCheck(insuredName);
		self.CrossSellingCheck(insuredName);
		self.SetInsuredTabLabel(insuredName);
	});

    self.Model.Broker.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.Model.BrokerCode((values[0]) ? values[0].trim() : "");
		self.Model.BrokerPseudonym((values[1]) ? values[1].trim() : "");
		self.Model.BrokerSequenceId((values[3]) ? values[3].trim() : "");

		var brokerCd = (values[4]) ? values[4].trim() : "";
		self.SetBrokerLossRatios(brokerCd);
		self.SetBrokerSummary(brokerCd);
	});

    self.Model._NonLondonBroker.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.Model.NonLondonBrokerCode((values[0]) ? values[0].trim() : "");
		self.Model.NonLondonBrokerName((values[1]) ? values[1].trim() : "");
	});

    self.Model._Underwriter.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.Model.UnderwriterCode((values[0]) ? values[0].trim() : "");
	});

    self.Model._UnderwriterContact.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.Model.UnderwriterContactCode((values[0]) ? values[0].trim() : "");
	});

    self.Model._Leader.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.Model.Leader((values[0]) ? values[0].trim() : "");
	});

    self.Model._Domicile.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.Model.Domicile((values[0]) ? values[0].trim() : "");
	});

    self.Model._QuotingOffice.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.Model.QuotingOfficeId((values[0]) ? values[0].trim() : "");
    });

    self.CurrentOption = function()
    {
	    var optionTab = $("li.active a[data-toggle='tab'][data-target^='#" + domId + "-option']"),
	        optionIndex = optionTab.attr("data-target").match(/[0-9]{1,2}$/)[0];

    	return (optionIndex >= 0)
			    ? self.Model.Options()[optionIndex]
			    : self.Model.Options()[0];
    };

	// Initialize Methods

	self.InitialiseTabs = function(element)
	{
		ConsoleApp.InitialiseTabs(element, domId, self);
	};

	self.InitialisePane = function(element)
	{
		ConsoleApp.InitialisePane(element, self);
	};

	// Setup
	if (initilizeSelf === true) {
		console.log(initilizeSelf);
		self.Initialise();
	}

}

function Option(optionIndex, domId, parent) {
	var self = this,
		optionNumber = optionIndex + 1;

    self.GetParent = function () {
		return parent;
	};

    self.GetIndex = function () {
		return optionIndex;
	};

	self.Id = ko.observable(0);
	self.SubmissionId = ko.observable(0);
	self.Timestamp = ko.observable("");
	self.Title = ko.observable("Option " + optionNumber);
	self.Comments = ko.observable("");
	self.VersionIndex = ko.observable(0);
	self.OptionVersions = ko.observableArray([new OptionVersion(0, domId, self)]);

	self.CanCopyOption = ko.observable(false);
	self.AddToQuoteSheet = ko.observable(true);
	self.EnableAddToQuoteSheet = ko.observable(true);
	self.IsCurrentQuoteQuoted = ko.observable(false);

    //  Initialise additional observables defined in inheriting
	if (parent.OAddAdditional)
	    parent.OAddAdditional(self);

    self.SetVersionIndex = function (element) {
		var versionNumber = ko.utils.peekObservable(element.VersionNumber()),
			versionCount = self.OptionVersions().length,
			versionIndex = (versionCount - versionNumber) - 1;

		self.VersionIndex(versionIndex);
	};

    self.AddOptionVersion = function () {
		var versionData = self.CurrentVersion().koTrim();

		parent.AttachValidation();

		return self.CopyOptionVersion(versionData);
	};

    self.CopyOptionVersion = function (versionData) {
		var length = self.OptionVersions().length,
			newVersion = new OptionVersion(length, domId, self);

		versionData.Title = newVersion.Title();
		versionData.OptionId = 0;
		versionData.VersionNumber = newVersion.VersionNumber();
		versionData.IsLocked = false;

        $.each(versionData.Quotes, function (quoteIndex, quoteItem) {
			quoteItem.Id = 0;
			quoteItem.OptionId = 0;
			quoteItem.VersionNumber = versionData.VersionNumber;
			quoteItem.Timestamp = "";
			quoteItem.SubmissionStatus = "SUBMITTED";

            if (quoteItem.IsSubscribeMaster === true) {
                if (self.CurrentVersion().Quotes()[quoteIndex].CorrelationToken() === quoteItem.CorrelationToken) {
					self.CurrentVersion().Quotes()[quoteIndex].IsSubscribeMaster(false);
				}
			}
		});

		newVersion.syncJSON(versionData);

		length = self.OptionVersions.unshift(newVersion);

		self.VersionIndex(0);

		return length;
	};

    self.NavigateToOption = function (element, e) {
		$("a[data-toggle='tab'][data-target='#" + domId + "-option" + optionIndex + "']").tab("show");
	};

    self.NavigateToQuote = function (element, e) {
		var optionDomId = domId + "-option" + optionIndex,
			quoteIndex = parseInt($(e.target).text());

		if (isNaN(quoteIndex)) quoteIndex = 0;

		$("a[data-toggle='tab'][data-target='#" + optionDomId + "']").tab("show");
		$("#" + optionDomId + " .carousel").carousel(parseInt(quoteIndex));
	};

    self.SetMaster = function (element, e) {
		var optionDomId = domId + "-option" + optionIndex,
			quoteIndex = parseInt($(e.target).text());

		if (isNaN(quoteIndex))
			quoteIndex = $(e.target).parent("td").children("span:first");

		$("a[data-toggle='tab'][data-target='#" + optionDomId + "']").tab("show");
		$("#" + optionDomId + " .carousel").carousel(parseInt(quoteIndex));
	};

    self.VersionTitle = ko.computed(function () {
		var optionTitle = self.Title(),
			versionIndex = self.VersionIndex(),
	        versionTitle = (self.OptionVersions()[versionIndex])
		        ? self.OptionVersions()[versionIndex].Title() : "";

		// Always show option as well - easier to debug
		return optionTitle + " " + versionTitle.replace(/ersion /gi, "").toLowerCase();

		//return (versionIndex > 0)
		//	? optionTitle + " " + versionTitle.replace(/ersion /gi, "").toLowerCase()
		//	: optionTitle;
	}, self);

    self.VersionCount = ko.computed(function () {
		return self.OptionVersions().length;
	}, self);

    self.CurrentVersion = ko.computed(function () {
		return self.OptionVersions()[self.VersionIndex()];
	}, self);

    self.CurrentQuote = ko.computed(function () {
		var optionDomId = domId + "-option" + optionIndex,
		    quoteDomId = optionDomId + " .carousel .carousel-indicators li.active",
		    quoteIndex = $("#" + quoteDomId).index(),
		    currentQuote = (quoteIndex >= 0)
			    ? self.CurrentVersion().Quotes()[quoteIndex]
			    : self.CurrentVersion().Quotes()[0];

		self.IsCurrentQuoteQuoted(currentQuote.SubmissionStatus() === "QUOTED");

		return currentQuote;
	}, self);

    self.RequiredFieldsCheck = ko.computed(function()
    {
    	var isValid = true;

    	$.each(self.CurrentVersion().Quotes(), function(quoteIndex, quoteItem)
    	{
    		isValid &= /\S/g.test(quoteItem.COBId())
		        && /\S/g.test(quoteItem.MOA())
		        && /\S/g.test(quoteItem.OriginatingOfficeId())
		        && /\S/g.test(quoteItem.AccountYear());

    		return isValid;
    	});

    	self.CanCopyOption(isValid);

    	return isValid;
    }, self);
}

function OptionVersion(versionNumber, domId, parent) {
	var self = this;

    self.GetParent = function () {
		return parent;
	};

    self.GetVersionNumber = function () {
		return versionNumber;
	};

	self.OptionId = ko.observable(0);
	self.VersionNumber = ko.observable(versionNumber);
	self.Timestamp = ko.observable("");
	self.Title = ko.observable("Version " + (versionNumber + 1));
	self.Comments = ko.observable("");
	self.IsExperiment = ko.observable(false); // TODO: Remove or implement ?
	self.IsLocked = ko.observable(false);

    //  Initialise additional observables defined in inheriting
	if (parent.GetParent().OVAddAdditional)
	    parent.GetParent().OVAddAdditional(self);

	self.Quotes = ko.observableArray([]);

	self.CanAddQuotes = ko.observable(versionNumber === 0);

    self.Initialise = function () {
		self.AddQuote();
	};

    self.AddQuote = function (quote, e, useNewQuoteWithExtraProperties)
    {
		var length;
        if (useNewQuoteWithExtraProperties === true) {
			length = self.Quotes.push(quote);
		} else {
			length = self.Quotes.push(new Quote(domId, self));
		}
		
		parent.GetParent().AttachValidation();
		parent.GetParent().CanCreateQuoteSheet(false);

		return length;
	};

    self.VersionNumber.subscribe(function (value) {
        $.each(self.Quotes(), function (quoteIndex, quoteItem) {
			quoteItem.VersionNumber(value);
		});
	});

	self.Initialise();
}

function Quote(domId, parent) {
	var self = this,
	    token = $.generateGuid(),
        submissionStatuses = ["SUBMITTED", "QUOTED", "FIRM ORDER", "DECLINED"], // TODO: Dynamically retrieve list from Validus.Services
        entryStatuses = ["NTU", "PARTIAL"], // TODO: Dynamically retrieve list from Validus.Services
        //policyTypes = ["AVIATION", "FAC R/I", "INPROP TTY", "INWARD X/L", "MARINE", "NONMARINE", "PACKAGE", "PROP TTY", "X/L R/I"],
        policyTypes = [{ id: 1, value: 'MARINE' }, { id: 2, value: 'NONMARINE' }],
		//technicalPricingBindStatusList = [""], // TODO: ???
		//technicalPricingPremiumPctgAmtList = [""], // TODO: ???
	    declinatureReasons = ["", // TODO: Dynamically retrieve list from Validus.Services
		    "Attachment point too low",
		    "Broker did not win the account",
		    "Cat Exposed",
		    "Cedent did not win the account",
		    "Ceding Company declined to write",
		    "Ceding Company security",
		    "Class of Business",
		    "Clearance - max line already out",
		    "Excluded class",
		    "Incumbent Loyalty",
		    "Lack of information",
		    "Loss experience",
		    "Never materialised",
		    "Occupancy",
		    "Other",
		    "Price",
		    "Pricing inadequate",
		    "Production source",
		    "Quota Share",
		    "Risk quality",
		    "Source",
		    "TBA",
		    "Terrorism",
		    "Unable to offer sufficient capacity",
		    "Unacceptable terms and conditions"];

	self.GetParent = function()
	{
		return parent;
	};

	self.Id = ko.observable(0);
	self.OptionId = ko.observable(parent.OptionId());
	self.VersionNumber = ko.observable(parent.VersionNumber());

	self.SubmissionStatusList = ko.observableArray(submissionStatuses);
	self.SubmissionStatus = ko.observable("SUBMITTED");
	self.SubscribeTimestamp = ko.observable("");
	self.Timestamp = ko.observable("");

	self.SubscribeReference = ko.observable("");
	self.RenPolId = ko.observable("");

	self._FacilityRef = ko.observable("");
	self.FacilityRef = ko.observable("");

	self.PolicyTypeList = ko.observableArray(policyTypes);
	self.SelectedPolicyType = ko.observable("");
	self.PolicyType = ko.observable('NONMARINE');

	self.EntryStatus = ko.observable("PARTIAL");
	self.EntryStatusList = ko.observableArray(entryStatuses);

	self._COB = ko.observable("");
	self.COBId = ko.observable("");
	self.COB = ko.observable("");

	self._OriginatingOffice = ko.observable("");
	self.OriginatingOfficeId = ko.observable("");
	self.OriginatingOffice = ko.observable("");

	self._MOA = ko.observable("");
	self.MOA = ko.observable("");

	self.InceptionDate = ko.observable("");
	self.ExpiryDate = ko.observable("");
	self.QuoteExpiryDate = ko.observable("");
	self.AccountYear = ko.observable("");

	self.TechnicalPricingBindStatus = ko.observable("");
	self.TechnicalPricingBindStatusList = ko.observableArray(["", "Pre", "Post"]);

	self.TechnicalPricingPremiumPctgAmt = ko.observable("%");
	self.TechnicalPricingPremiumPctgAmtList = ko.observableArray(["%", "Amt"]);

	self.TechnicalPricingMethod = ko.observable("");
	self.TechnicalPricingMethodList = ko.observableArray(["", "Actuary", "Model", "UW"]); // TODO: Dynamically retrieve list from Validus.Services

	self.TechnicalPremium = ko.observable("");
	self.BenchmarkPremium = ko.observable("");
	self.QuotedPremium = ko.observable("");
	self.LimitAmount = ko.observable("");
	self.ExcessAmount = ko.observable("");

	self.Currency = ko.observable("");
	self.LimitCCY = ko.observable("");
	self.ExcessCCY = ko.observable("");
	self._Currency = ko.observable("");
	self._LimitCCY = ko.observable("");
	self._ExcessCCY = ko.observable("");

	self.Comment = ko.observable("");
	self.DeclinatureReason = ko.observable("");
	self.DeclinatureComments = ko.observable("");
	self.DeclinatureReasonList = ko.observableArray(declinatureReasons);

	self.ValidationErrors = ko.observable("");

	self.CreatedOn = ko.observable("0001-01-01T00:00:00.00");
	self.CreatedBy = ko.observable("");

	self.ModifiedOn = ko.observable("0001-01-01T00:00:00.00");
	self.ModifiedBy = ko.observable("");

	self.IsSubscribeMaster = ko.observable(true);
	self.CorrelationToken = ko.observable(token);

    //  Initialise additional observables defined in inheriting
	if (parent.GetParent().GetParent().QAddAdditional)
	    parent.GetParent().GetParent().QAddAdditional(self);

	self.Initialise = function() {
		console.log('Quote Initialise');

        var defaultQuoteExpiry = $("input[type='hidden'][name='DefaultQuoteExpiry']", self.Form).val(),
             defaultOffice = $("input[type='hidden'][name='DefaultOffice']", self.Form).val(),
             defaultPolicyType = $("input[type='hidden'][name='DefaultPolicyType']", self.Form).val();

        defaultQuoteExpiry = parseInt(defaultQuoteExpiry);

        if (!isNaN(defaultQuoteExpiry))
        {
        	var quoteExpiryDate = moment().add("d", defaultQuoteExpiry);

        	if (quoteExpiryDate && quoteExpiryDate.isValid())
        		quoteExpiryDate = quoteExpiryDate.format("YYYY-MM-DD");
        }

        if (defaultQuoteExpiry) self.QuoteExpiryDate(quoteExpiryDate);
		
	    if (defaultPolicyType) {
	    	self.PolicyType(self.PolicyTypeList.find('value', { value: defaultPolicyType }));
		}

		if (defaultOffice) self._OriginatingOffice(defaultOffice);
	};

	self.SelectedPolicyType.subscribe(function(newVal) {
		self.PolicyType(newVal.value);
	});

    self.SubmissionStatus.subscribe(function (value) {
        if (value !== "DECLINED") {
			self.DeclinatureReason("");
			self.DeclinatureComments("");
		}

		if (parent.CanAddQuotes())
			parent.CanAddQuotes(value !== "QUOTED" && parent.VersionNumber() === 0);

		self.SyncSlaveObservables("SubmissionStatus", value);
	});

    self.DeclinatureReason.subscribe(function (value) {

		self.SyncSlaveObservables("DeclinatureReason", value);
	});

    self.DeclinatureComments.subscribe(function (value) {
		self.SyncSlaveObservables("DeclinatureComments", value);
	});

    self._FacilityRef.subscribe(function(value)
    {
	    var facilityValues = value ? value.split(":") : [];

	    self.FacilityRef(facilityValues[0] ? facilityValues[0].trim() : "");
    });
	
	self.FacilityRef.subscribe(function(value)
	{
		self.SyncSlaveObservables("FacilityRef", value);
	});

    self._OriginatingOffice.subscribe(function (value) {
    	var values = (value) ? value.split(":") : [];

		self.OriginatingOfficeId((values[0]) ? values[0].trim() : "");

		self.SyncSlaveObservables("_OriginatingOffice", value);
	});

    self._COB.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.COBId((values[0]) ? values[0].trim() : "");

		self.SyncSlaveObservables("_COB", value);
	});

    self._MOA.subscribe(function (value) {
		var values = (value) ? value.split(":") : [];

		self.MOA((values[0]) ? values[0].trim() : "");

		self.SyncSlaveObservables("_MOA", value);
	});

    self.PolicyType.subscribe(function (value) {
		self.SyncSlaveObservables("PolicyType", value);
	});

    self.EntryStatus.subscribe(function (value) {
		self.SyncSlaveObservables("EntryStatus", value);
	});

    self.InceptionDate.subscribe(function (value) {
        if ((value !== undefined) && (value !== null)) {
			var inceptionDate = moment(value);

            if (inceptionDate && inceptionDate.isValid()) {
				self.AccountYear(inceptionDate.format("YYYY"));
				self.ExpiryDate(inceptionDate.add("y", 1).subtract("d", 1).format("YYYY-MM-DD"));
			}
		}

		self.SyncSlaveObservables("InceptionDate", value);
	});

    self.ExpiryDate.subscribe(function (value) {
        if ((value !== undefined) && (value !== null)) {
			var expiryDate = moment(value);

            if (expiryDate && expiryDate.isValid()) {
				self.SyncSlaveObservables("ExpiryDate", expiryDate.format("YYYY-MM-DD"));
			}
		}
	});

    self.AccountYear.subscribe(function (value) {
		self.SyncSlaveObservables("AccountYear", value);
	});

    self.IsSubscribeMaster.subscribe(function (value) {
		self.SyncSlaveObservables("IsSubscribeMaster", false);
	});

    self.TechnicalPricingMethodList.subscribe(function (value) {
        if ((value !== undefined) && (value !== null)) {
			self.TechnicalPricingMethod(value);
		}
	});

    self._Currency.subscribe(function (value) {
		var currencyValues = (value) ? value.split(":") : [],
			currencyPsu = (currencyValues[0]) ? currencyValues[0].trim() : "";

		self.Currency(currencyPsu);
        		
		if (currencyValues.length > 1)
		{
		    if (self._LimitCCY() == '')
		        self._LimitCCY(self._Currency());
		    if (self._ExcessCCY() == '')
		        self._ExcessCCY(self._Currency());
		}
	});

    self._LimitCCY.subscribe(function (value) {
		var currencyValues = (value) ? value.split(":") : [],
			currencyPsu = (currencyValues[0]) ? currencyValues[0].trim() : "";

		self.LimitCCY(currencyPsu);
	});

    self._ExcessCCY.subscribe(function (value) {
		var currencyValues = (value) ? value.split(":") : [],
			currencyPsu = (currencyValues[0]) ? currencyValues[0].trim() : "";

		self.ExcessCCY(currencyPsu);
	});

    self.EnableSubscribeMaster = ko.computed(function () {
		return !self.IsSubscribeMaster();
	}, self);

    self.IsLiveOrCancelled = ko.computed(function () {
		var entryStatus = self.EntryStatus();

		return ((entryStatus === "LIVE") || (entryStatus === "CANCELLED"));
	}, self);

    self.IsLocked = ko.computed(function () {
		return self.SubmissionStatus() === "QUOTED"
    		|| self.IsLiveOrCancelled();
	}, self);

    self.ShowDeclinatureDialog = function () {
		var vmOption = parent.GetParent(),
		    optionDomId = domId + "-option" + vmOption.GetIndex(),
		    quoteDomId = optionDomId + " .carousel .carousel-inner div.active",
		    modalDomId = quoteDomId + " .val-declinature:first";

		$("#" + modalDomId).modal("show");
	};

    self.SyncSlaveObservables = function (observableName, value) {
        if (self.IsSubscribeMaster()) {
			var vmSubmission = parent.GetParent().GetParent();

            $.each(vmSubmission.Model.Options(), function (optionIndex, optionData) {
                $.each(optionData.OptionVersions(), function (versionIndex, versionData) {
                    if (!versionData.IsLocked() || observableName === "IsSubscribeMaster") {
                        $.each(versionData.Quotes(), function (quoteIndex, quoteData) {
							if (quoteData.CorrelationToken() === self.CorrelationToken()
								&& quoteData !== self) {
								quoteData[observableName](value);
							}
						});
					}
				});
			});
		}
    };
	
	self.Initialise();
}
	
/*
	Submission / Option / Option Version / Quote Sync-JSON Extensions

	Description;
		These are functions that map JSON data to theSubmission / Option / Option Version / Quote 
		view models.
		
	Usage;
		thisSubmission.syncJSON(jsonData);
*/

vmSubmissionBase.prototype.syncJSON = function (submissionData) {
	var self = this,
	    vmSubmission = this.Model;

	self.Id = submissionData.Id;
    vmSubmission.AuditTrails.length = 0;
	vmSubmission.Id(submissionData.Id);
	vmSubmission.Timestamp(submissionData.Timestamp);

    //  Call the additional sync tasks defined in inherited.
	if (self.SSyncJSONAdditional)
	    self.SSyncJSONAdditional(vmSubmission, submissionData);
    
	/*
		TODO: Remove the need for display values
	*/
	if ((submissionData.Broker != undefined) && (submissionData.Broker != null))
		vmSubmission.Broker(submissionData.Broker);
    else {
	    $.getJSON(window.ValidusServicesUrl + "Broker", { id: submissionData.BrokerSequenceId }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmSubmission.Broker(item.Code
							+ " : " + item.Psu
							+ " : " + item.Name
							+ " : " + item.Id
							+ " : " + item.GrpCd);

				return false;
			});
		});
	}

	if ((submissionData._Underwriter != undefined) && (submissionData._Underwriter != null))
		vmSubmission._Underwriter(submissionData._Underwriter);
    else {
        $.getJSON(window.ValidusServicesUrl + "Underwriter", { code: submissionData.UnderwriterCode }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmSubmission._Underwriter(item.Code + " : " + item.Name);

				return false;
			});
		});
	}

	if ((submissionData._UnderwriterContact != undefined) && (submissionData._UnderwriterContact != null))
		vmSubmission._UnderwriterContact(submissionData._UnderwriterContact);
    else {
        $.getJSON(window.ValidusServicesUrl + "Underwriter", { code: submissionData.UnderwriterContactCode }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmSubmission._UnderwriterContact(item.Code + " : " + item.Name);

				return false;
			});
		});
	}

	vmSubmission._Leader(submissionData.Leader);

	if ((submissionData._Domicile != undefined) && (submissionData._Domicile != null))
		vmSubmission._Domicile(submissionData._Domicile);
    else {
        $.getJSON(window.ValidusServicesUrl + "Domicile", { code: submissionData.Domicile }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmSubmission._Domicile(item.Code + " : " + item.Name);

				return false;
			});
		});
	}

	if ((submissionData._QuotingOffice != undefined) && (submissionData._QuotingOffice != null))
		vmSubmission._QuotingOffice(submissionData._QuotingOffice);
    else {
        $.getJSON(window.ValidusServicesUrl + "Office", { code: submissionData.QuotingOfficeId }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmSubmission._QuotingOffice(item.Code + " : " + item.Name);

				return false;
			});
		});
	}

	if ((submissionData._NonLondonBroker != undefined) && (submissionData._NonLondonBroker != null))
	    vmSubmission._NonLondonBroker(submissionData._NonLondonBroker);
    else if (submissionData.NonLondonBrokerCode) {
	    $.getJSON(window.ValidusServicesUrl + "NonLondonBroker", { code: submissionData.NonLondonBrokerCode }, function (jsonData) {
	        $(jsonData).each(function (index, item) {
	            vmSubmission._NonLondonBroker(item.Code + " : " + item.Name);

	            return false;
	        });
	    });
	}
	/*
		End of display values
	*/

	// TODO: vmSubmission._NonLondonBroker(submissionData.???);
	// TODO: vmSubmission.NonLondonBroker(submissionData.???);

	vmSubmission.QuotingOfficeId(submissionData.QuotingOfficeId);
	vmSubmission.QuotingOffice(submissionData.QuotingOffice); // TODO: Is this ever used ?
    if (submissionData.NonLondonBrokerCode) {
	    vmSubmission.NonLondonBrokerCode(submissionData.NonLondonBrokerCode);
	}

	vmSubmission.Title(submissionData.Title);
	vmSubmission.UnderwriterNotes(submissionData.UnderwriterNotes);
	vmSubmission.Description(submissionData.Description);

	vmSubmission.InsuredName(submissionData.InsuredName + " : " + submissionData.InsuredId);
	vmSubmission.InsuredId(submissionData.InsuredId);

	vmSubmission.BrokerCode(submissionData.BrokerCode);
	vmSubmission.BrokerPseudonym(submissionData.BrokerPseudonym);
	vmSubmission.BrokerSequenceId(submissionData.BrokerSequenceId);
	vmSubmission.BrokerContact(submissionData.BrokerContact);
	vmSubmission.UnderwriterCode(submissionData.UnderwriterCode);
	vmSubmission.UnderwriterContactCode(submissionData.UnderwriterContactCode);
	vmSubmission.Leader(submissionData.Leader);
	vmSubmission.Domicile(submissionData.Domicile);
	vmSubmission.Brokerage(submissionData.Brokerage);

    if (vmSubmission.Options().length !== submissionData.Options.length) {
        while (vmSubmission.Options().length !== 0) {
			vmSubmission.Options.pop();
		}
	}

    $.each(submissionData.Options, function (optionIndex, optionData) {
		var vmOption = vmSubmission.Options()[optionIndex];

        if (!vmOption) {
			var optionsLength = self.AddOption();

			vmOption = vmSubmission.Options()[optionsLength - 1];
		}

		vmSubmission.Options()[optionIndex] = vmOption.syncJSON(optionData);
	});

    vmSubmission.SubmissionMarketWordingsList.removeAll();
    var tempSubmissionMarketWordingsList = [];
	$.each(submissionData.MarketWordingSettings, function (index, value) {
	    tempSubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                    .SettingId(value.Id)
	                    .Id(value.MarketWording.Id)
	                    .DisplayOrder(value.DisplayOrder)
	                    .WordingRefNumber(value.MarketWording.WordingRefNumber)
	                    .Title(value.MarketWording.Title)
	                );
	});
	ko.utils.arrayPushAll(vmSubmission.SubmissionMarketWordingsList(), tempSubmissionMarketWordingsList);
	vmSubmission.SubmissionMarketWordingsList.valueHasMutated();

    vmSubmission.SubmissionTermsNConditionWordingsList.removeAll();
    var tempSubmissionTermsNConditionWordingsList = [];
	$.each(submissionData.TermsNConditionWordingSettings, function (index, value) {
	    tempSubmissionTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWordingSettingDto()
	                    .SettingId(value.Id)
                        .Id(value.TermsNConditionWording.Id)
	                    .DisplayOrder(value.DisplayOrder)
	                    .IsStrikeThrough(value.IsStrikeThrough)
	                    .WordingRefNumber(value.TermsNConditionWording.WordingRefNumber)
	                    .Title(value.TermsNConditionWording.Title)
                  );
	});
	ko.utils.arrayPushAll(vmSubmission.SubmissionTermsNConditionWordingsList(), tempSubmissionTermsNConditionWordingsList);
	vmSubmission.SubmissionTermsNConditionWordingsList.valueHasMutated();

	vmSubmission.CustomSubmissionMarketWordingsList.removeAll();
    var tempCustomSubmissionMarketWordingsList = [];
	$.each(submissionData.CustomMarketWordingSettings, function (index, value) {
	    tempCustomSubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                 .SettingId(value.Id)
                     .Id(value.MarketWording.Id)
                     .DisplayOrder(value.DisplayOrder)
	                 .WordingRefNumber(value.MarketWording.WordingRefNumber)
                     .Title(value.MarketWording.Title)
                 );
	});
	ko.utils.arrayPushAll(vmSubmission.CustomSubmissionMarketWordingsList(), tempCustomSubmissionMarketWordingsList);
	vmSubmission.CustomSubmissionMarketWordingsList.valueHasMutated();

    vmSubmission.CustomSubmissionTermsNConditionWordingsList.removeAll();
    var tempCustomSubmissionTermsNConditionWordingsList = [];
	$.each(submissionData.CustomTermsNConditionWordingSettings, function (index, value) {
	    tempCustomSubmissionTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWordingSettingDto()
	                  .SettingId(value.Id)
                      .Id(value.TermsNConditionWording.Id)
                      .DisplayOrder(value.DisplayOrder)
                      .IsStrikeThrough(value.IsStrikeThrough)
	                  .WordingRefNumber(value.TermsNConditionWording.WordingRefNumber)
                      .Title(value.TermsNConditionWording.Title)
                );
	});
	ko.utils.arrayPushAll(vmSubmission.CustomSubmissionTermsNConditionWordingsList(), tempCustomSubmissionTermsNConditionWordingsList);
	vmSubmission.CustomSubmissionTermsNConditionWordingsList.valueHasMutated();

    vmSubmission.CustomSubmissionSubjectToClauseWordingsList.removeAll();
    var tempCustomSubmissionSubjectToClauseWordingsList = [];
	$.each(submissionData.CustomSubjectToClauseWordingSettings, function (index, value) {
	    tempCustomSubmissionSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWordingSettingDto()
	                 .SettingId(value.Id)
                     .Id(value.SubjectToClauseWording.Id)
                      .DisplayOrder(value.DisplayOrder)
                      .IsStrikeThrough(value.IsStrikeThrough)
                      .Title(value.SubjectToClauseWording.Title)
              );
	});
	ko.utils.arrayPushAll(vmSubmission.CustomSubmissionSubjectToClauseWordingsList(), tempCustomSubmissionSubjectToClauseWordingsList);
    vmSubmission.CustomSubmissionSubjectToClauseWordingsList.valueHasMutated();
	return vmSubmission;
};

Option.prototype.syncJSON = function (optionData) {
	var vmOption = this;

	vmOption.Id(optionData.Id);
	vmOption.SubmissionId(optionData.SubmissionId);
	vmOption.Timestamp(optionData.Timestamp);

	vmOption.Title(optionData.Title);
	vmOption.Comments(optionData.Comments);
    // TODO: vmOption.VersionIndex(???);

    //  Call the additional sync tasks defined in inherited.
	if (vmOption.GetParent().OSyncJSONAdditional)
	    vmOption.GetParent().OSyncJSONAdditional(vmOption, optionData);

    while (vmOption.OptionVersions().length > optionData.OptionVersions.length) {
		vmOption.OptionVersions.pop();
	}

    while (vmOption.OptionVersions().length < optionData.OptionVersions.length) {
		vmOption.AddOptionVersion();
	}

    optionData.OptionVersions.sort(function (versionA, versionB) {
		return (versionA.VersionNumber < versionB.VersionNumber)
			? 1 : (versionA.VersionNumber > versionB.VersionNumber)
				? -1 : 0;
	});

    $.each(optionData.OptionVersions, function (versionIndex, versionData) {
		var vmVersion = vmOption.OptionVersions()[versionIndex];

		if (!vmVersion)
			vmOption.CopyOptionVersion(versionData);
		else
			vmOption.OptionVersions()[versionIndex] = vmVersion.syncJSON(versionData);
	});

	return vmOption;
};

OptionVersion.prototype.syncJSON = function (versionData) {
	var vmVersion = this;

	vmVersion.OptionId(versionData.OptionId);
	vmVersion.VersionNumber(versionData.VersionNumber);
	vmVersion.Timestamp(versionData.Timestamp);

	vmVersion.Title(versionData.Title);
	vmVersion.Comments(versionData.Comments);
	vmVersion.IsExperiment(versionData.IsExperiment);
	vmVersion.IsLocked(versionData.IsLocked);

	if (vmVersion.GetParent().GetParent().OVSyncJSONAdditional)
	    vmVersion.GetParent().GetParent().OVSyncJSONAdditional(vmVersion, versionData);
    
    while (vmVersion.Quotes().length > versionData.Quotes.length) {
		vmVersion.Quotes.pop();
	}

    $.each(versionData.Quotes, function (quoteIndex, quoteData) {
		var vmQuote = vmVersion.Quotes()[quoteIndex];

        if (!vmQuote) {
			var quotesLength = vmVersion.AddQuote();

			vmQuote = vmVersion.Quotes()[quotesLength - 1];
		}

		vmVersion.Quotes()[quoteIndex] = vmQuote.syncJSON(quoteData);
	});

	return vmVersion;
};

Quote.prototype.syncJSON = function (quoteData) {
	var vmQuote = this;

	vmQuote.Id(quoteData.Id);
	vmQuote.OptionId(quoteData.OptionId);
	vmQuote.VersionNumber(quoteData.VersionNumber);

	/*
		Declinature reason must be synchronised first due to a subscription
		in the Quote view model that overwrites the Submission Status
	*/
	vmQuote.DeclinatureReason(quoteData.DeclinatureReason);
	vmQuote.DeclinatureComments(quoteData.DeclinatureComments);
	vmQuote.SubmissionStatus(quoteData.SubmissionStatus || "SUBMITTED");

	vmQuote.SubscribeTimestamp(quoteData.SubscribeTimestamp);
	vmQuote.Timestamp(quoteData.Timestamp || "");

	vmQuote.SubscribeReference(quoteData.SubscribeReference || "");
	vmQuote.RenPolId(quoteData.RenPolId || "");

	vmQuote.PolicyType(quoteData.PolicyType);
	vmQuote.EntryStatus(quoteData.EntryStatus);

	vmQuote.FacilityRef(quoteData.FacilityRef || "");
	/*
		TODO: Remove the need for display values
	*/
	if (quoteData._FacilityRef != undefined && quoteData._FacilityRef != null)
		vmQuote._FacilityRef(quoteData._FacilityRef);
	else if (quoteData.FacilityRef)
	{
		$.getJSON(window.ValidusServicesUrl + "Facility", { reference: quoteData.FacilityRef }, function(jsonData)
		{
			$(jsonData).each(function(index, item)
			{
				vmQuote._FacilityRef(item.Reference + " : " + item.Description);

				return false;
			});
		});
	}
	
	if ((quoteData._COB != undefined) && (quoteData._COB != null))
		vmQuote._COB(quoteData._COB);
    else {
        $.getJSON(window.ValidusServicesUrl + "COB", { code: quoteData.COBId }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmQuote._COB(item.Code + " : " + item.Name);

				return false;
			});
		});
	}

	if ((quoteData._MOA != undefined) && (quoteData._MOA != null))
		vmQuote._MOA(quoteData._MOA);
    else {
        $.getJSON(window.ValidusServicesUrl + "MOA", { code: quoteData.MOA }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmQuote._MOA(item.Code + " : " + item.Description);

				return false;
			});
		});
	}

	if ((quoteData._OriginatingOffice != undefined) && (quoteData._OriginatingOffice != null))
		vmQuote._OriginatingOffice(quoteData._OriginatingOffice);
    else {
        $.getJSON(window.ValidusServicesUrl + "Office", { code: quoteData.OriginatingOfficeId }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmQuote._OriginatingOffice(item.Code + " : " + item.Name);

				return false;
			});
		});
	}

	if ((quoteData._Currency != undefined) && (quoteData._Currency != null))
		vmQuote._Currency(quoteData._Currency);
    else if (quoteData.Currency) {
        $.getJSON(window.ValidusServicesUrl + "Currency", { psu: quoteData.Currency }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmQuote._Currency(item.Psu + " : " + item.Name);

				return false;
			});
		});
	}

	if ((quoteData._LimitCCY != undefined) && (quoteData._LimitCCY != null))
		vmQuote._LimitCCY(quoteData._LimitCCY);
    else if (quoteData.LimitCCY) {
        $.getJSON(window.ValidusServicesUrl + "Currency", { psu: quoteData.LimitCCY }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmQuote._LimitCCY(item.Psu + " : " + item.Name);

				return false;
			});
		});
	}
	else vmQuote._LimitCCY("");

	if ((quoteData._ExcessCCY != undefined) && (quoteData._ExcessCCY != null))
		vmQuote._ExcessCCY(quoteData._ExcessCCY);
    else if (quoteData.ExcessCCY) {
        $.getJSON(window.ValidusServicesUrl + "Currency", { psu: quoteData.ExcessCCY }, function (jsonData) {
            $(jsonData).each(function (index, item) {
				vmQuote._ExcessCCY(item.Psu + " : " + item.Name);

				return false;
			});
		});
	}
	else vmQuote._ExcessCCY("");
	/*
		End of display values
	*/
	
	vmQuote.COBId(quoteData.COBId);
	vmQuote.COB(quoteData.COB); // TODO: Is this ever used ? (It is required by the Model)

	vmQuote.OriginatingOfficeId(quoteData.OriginatingOfficeId);
	vmQuote.OriginatingOffice(quoteData.OriginatingOffice); // TODO: Is this ever used ?

	vmQuote.MOA(quoteData.MOA);

	vmQuote.Currency(quoteData.Currency);
	vmQuote.LimitCCY(quoteData.LimitCCY);
	vmQuote.ExcessCCY(quoteData.ExcessCCY);

	var inceptionDate = moment(quoteData.InceptionDate),
	    expiryDate = moment(quoteData.ExpiryDate),
	    quoteExpiryDate = moment(quoteData.QuoteExpiryDate);

	if (inceptionDate && inceptionDate.isValid())
		vmQuote.InceptionDate(inceptionDate.format("YYYY-MM-DD"));
	else
		vmQuote.InceptionDate("");

	if (expiryDate && expiryDate.isValid())
		vmQuote.ExpiryDate(expiryDate.format("YYYY-MM-DD"));
	else vmQuote.ExpiryDate("");

	if (quoteExpiryDate && quoteExpiryDate.isValid())
		vmQuote.QuoteExpiryDate(quoteExpiryDate.format("YYYY-MM-DD"));
	else vmQuote.QuoteExpiryDate("");

	vmQuote.AccountYear(quoteData.AccountYear);

	vmQuote.TechnicalPricingBindStatus(quoteData.TechnicalPricingBindStatus || "PRE");
	vmQuote.TechnicalPricingPremiumPctgAmt(quoteData.TechnicalPricingPremiumPctgAmt || "%");
	vmQuote.TechnicalPricingMethod(quoteData.TechnicalPricingMethod || "MODEL");

	vmQuote.TechnicalPremium(quoteData.TechnicalPremium);
	vmQuote.BenchmarkPremium(quoteData.BenchmarkPremium);
	vmQuote.QuotedPremium(quoteData.QuotedPremium);
	vmQuote.LimitAmount(quoteData.LimitAmount);
	vmQuote.ExcessAmount(quoteData.ExcessAmount);

	vmQuote.Comment(quoteData.Comment);

	vmQuote.CreatedOn(quoteData.CreatedOn);
	vmQuote.CreatedBy(quoteData.CreatedBy);

	vmQuote.ModifiedOn(quoteData.ModifiedOn);
	vmQuote.ModifiedBy(quoteData.ModifiedBy);

	vmQuote.IsSubscribeMaster(quoteData.IsSubscribeMaster);
	vmQuote.CorrelationToken(quoteData.CorrelationToken);

    if (vmQuote.GetParent().GetParent().GetParent().QSyncJSONAdditional)
	    vmQuote.GetParent().GetParent().GetParent().QSyncJSONAdditional(vmQuote, quoteData);

	return vmQuote;
};

/*
	Submission / Option / Option Version / Quote To-JSON & KO-Trim Extensions

	Description;
		These are functions that clean up the Submission / Option / Option Version / Quote 
		view models in preparation to be sent to the server or copied over to another instance.

	Descriptions;
		Front-End;
			Display user-friendly information and populate back-end observables with the 
			actual data required by the server.

		Entity Framework;
			Must not send back to server due to a problem with the entity framework
			handling objects, so use their equivalent Id observables instead.

		Garbage;
			Computed functions and other objects that are garbage to the Submission model

	Usage;
		var jsonData = thisSubmission.toJSON();
		var koData = thisSubmission.koTrim();

	Refactor Possibilities;
		A generic method of synchronising any Knockout view model with JSON.

		ko.syncJSON = function(source, target)
		{
			target = target || this;

			for (var property in source)
			{
				if (!target[property]) continue;

				if (ko.isObservableArray(target[property]))
				{
					for (var sourceIndex in source[property])
					{
						self.syncJSON(source[property][sourceIndex], target[property]);
					}
				}
				else if (ko.isObservable(target[property]))
				{
					target[property](source[property]);
				}
				else target[property] = source[property];
			}
		};
*/
vmSubmissionBase.prototype.toJSON = function (dirtyFlag) {
	var vmSubmission = this.koTrim(dirtyFlag),
	    insuredValues = (vmSubmission.InsuredName)
		    ? vmSubmission.InsuredName.split(":") : [];

	vmSubmission.InsuredName = (insuredValues[0]) ? insuredValues[0].trim() : "";
	vmSubmission.InsuredId = (insuredValues[1]) ? insuredValues[1].trim() : "0";

	return ko.toJSON(vmSubmission, dirtyFlag);
};

vmSubmissionBase.prototype.koTrim = function (dirtyFlag) {
	var vmSubmission = ko.toJS(this.Model);

	// Front-End
    if (dirtyFlag) {
		delete vmSubmission._Domicile;
		delete vmSubmission._Leader;
		delete vmSubmission._NonLondonBroker;
		delete vmSubmission._Underwriter;
		delete vmSubmission._UnderwriterContact;
		delete vmSubmission.Broker;
		delete vmSubmission.QuotingOffice;
	}

	// Garbage
	delete vmSubmission.koTrim;
	delete vmSubmission.syncJSON;
	delete vmSubmission.toJSON;

    $.each(vmSubmission.Options, function (optionIndex, optionItem) {
		vmSubmission.Options[optionIndex] = optionItem.koTrim(optionItem);
	});

	return vmSubmission;
};

Option.prototype.toJSON = function () {
	var vmOption = this.koTrim();

	return ko.toJSON(vmOption);
};

Option.prototype.koTrim = function (thisOption, dirtyFlag) {
	var vmOption = (!thisOption) ? ko.toJS(this) : thisOption;

	// Garbage
	delete vmOption.GetParent;
	delete vmOption.GetIndex;

	delete vmOption.VersionIndex; // TODO: Should we keep this ?

	delete vmOption.AddToQuoteSheet;
	delete vmOption.EnableAddToQuoteSheet;

	delete vmOption.VersionTitle;
	delete vmOption.VersionCount;

	delete vmOption.CurrentVersion;
	delete vmOption.CurrentQuote;

	delete vmOption.SetVersionIndex;

	delete vmOption.AddOptionVersion;
	delete vmOption.CopyOptionVersion;
	delete vmOption.CanCopyOption;
	delete vmOption.IsCurrentQuoteQuoted;
	delete vmOption.RequiredFieldsCheck;

	delete vmOption.NavigateToOption;
	delete vmOption.NavigateToQuote;

	delete vmOption.SetMaster;

	delete vmOption.koTrim;
	delete vmOption.syncJSON;
	delete vmOption.toJSON;

    $.each(vmOption.OptionVersions, function (versionIndex, versionItem) {
		vmOption.OptionVersions[versionIndex] = versionItem.koTrim(versionItem, dirtyFlag);
	});

	return vmOption;
};

OptionVersion.prototype.toJSON = function () {
	var vmVersion = this.koTrim();

	return ko.toJSON(vmVersion);
};

OptionVersion.prototype.koTrim = function (thisVersion, dirtyFlag) {
	var vmVersion = (!thisVersion) ? ko.toJS(this) : thisVersion;

	// Garbage
	delete vmVersion.GetParent;
	delete vmVersion.GetVersionNumber;

	delete vmVersion.AddQuote;
	delete vmVersion.CanAddQuotes;
	delete vmVersion.Initialise;

	delete vmVersion.koTrim;
	delete vmVersion.syncJSON;
	delete vmVersion.toJSON;

    $.each(vmVersion.Quotes, function (quoteIndex, quoteItem) {
		vmVersion.Quotes[quoteIndex] = quoteItem.koTrim(quoteItem, dirtyFlag);
	});

	return vmVersion;
};

Quote.prototype.toJSON = function () {
	var vmQuote = this.koTrim();

	return ko.toJSON(vmQuote);
};

Quote.prototype.koTrim = function (thisQuote, dirtyFlag) {
	var vmQuote = (!thisQuote) ? ko.toJS(this) : thisQuote;

	// Front-End 
    if (dirtyFlag) {
		delete vmQuote._COB;
		delete vmQuote._Currency;
		delete vmQuote._ExcessCCY;
		delete vmQuote._LimitCCY;
		delete vmQuote._MOA;
		delete vmQuote._OriginatingOffice;
	}

	// Choice Lists
	delete vmQuote.DeclinatureReasonList;
	delete vmQuote.EntryStatusList;
	delete vmQuote.PolicyTypeList;
	delete vmQuote.SubmissionStatusList;
	delete vmQuote.TechnicalPricingMethodList;
	delete vmQuote.TechnicalPricingBindStatusList;
	delete vmQuote.TechnicalPricingPremiumPctgAmtList;

	// Entity Framework
	delete vmQuote.COB;
	delete vmQuote.OriginatingOffice;
	delete vmQuote.CreatedOn;
	delete vmQuote.CreatedBy;
	delete vmQuote.ModifiedOn;
	delete vmQuote.ModifiedBy;

	// Garbage
	delete vmQuote.DeclinatureEditable;
	delete vmQuote.EnableSubscribeMaster;
	delete vmQuote.Initialise;
	delete vmQuote.IsLiveOrCancelled;
	delete vmQuote.SetMOADetails;
	delete vmQuote.SetOfficeDetails;
	delete vmQuote.ShowDeclinatureDialog;
	delete vmQuote.ValidationErrors;
	delete vmQuote.IsLocked;

	delete vmQuote.koTrim;
	delete vmQuote.syncJSON;
	delete vmQuote.toJSON;

	return vmQuote;
};

// TODO: ko.dirtyFlag suggests it is generic, however, see Resynchronise()
//var ConsoleIter = 0;
ko.dirtyFlag = function (koObject) {
	var self = this;

	self.OriginalState = ko.observable(koObject.toJSON());
	self.IsDirty = ko.observable(false);
	self.RequiresReset = ko.observable(false);

    self.Evaluate = ko.computed(function () {
		//console.log((ConsoleIter++).toString() + " - Submission Pre-Dirty: " + self.IsDirty());
		//console.log(JSON.parse(self.OriginalState()));

        if (self.RequiresReset()) {
			//console.log("Resetting Dirty Flag");

			self.OriginalState(koObject.toJSON());
			self.IsDirty(false);
			self.RequiresReset(false);
		}
        else if (!self.IsDirty()) {
			//console.log("Checking Dirty Flag");

			self.IsDirty(self.OriginalState() !== koObject.toJSON());
		}

		//console.log((ConsoleIter++).toString() + " - Submission Post-Dirty: " + self.IsDirty());
		//console.log(JSON.parse(self.OriginalState()));
	}, self);

    self.Reset = function () {
		self.RequiresReset(true);
	};

	// TODO: Remove/re-think may be required as syncJSON is only used by Submission -> Quote VM's
    self.Resynchronise = function () {
		// TODO: Perhaps we could implement a something generic using hasOwnProperty() ?
		koObject.syncJSON(JSON.parse(self.OriginalState()));

		self.Reset();
	};
};
