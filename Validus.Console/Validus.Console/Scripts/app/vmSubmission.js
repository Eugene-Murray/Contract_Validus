
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

/*
	TODO: Summary
	- Nest Submission view-model within a Home view-model (use Knockout binding handlers for tabs, etc)
	- Implement a User settings view-model and take advantage of its observables
	- Extract bulky AJAX calls and use a generic helper instead
	- Submission base to represent the model, not the DTO
	- Implement Select2 for ALL choice lists
	- Dynamically retrieve choice list data
	- Remove "display" observables
	- Use Knockout binding context ($parents, $parent), not custom GetParent functions
	- Simplify Sync (generic and recursive algorithm) and remove KOTrim
	- Reduce the number of observables, functions, members, etc and group them together
	- Attach Knockout validation
	- Remove commented-out-code (this file is big enough already!)
	- Comply with HTML5 standards
*/
function vmSubmission(id, domId, initialiseSelf, isReadOnly)
{
	var self = this;

	self.Id = id > 0 ? id : 0;
	self.Form = $(".form", $("#" + domId));
	self.DirtyFlag = null;

	self.OptionIndexToCopy = ko.observable(0);
	self.CanCreateQuoteSheet = ko.observable(false);
	self.CreatingQuoteSheet = ko.observable(false);
	self.IsInitialised = ko.observable(false);
	self.IsSaving = ko.observable(false);
	self.IsReadOnly = ko.observable(isReadOnly || false);
	self.IsSavingOrReadonly = ko.computed(function() // TODO: Data-bind attributes can handle multiple observables/functions/members, do we need extra computeds ?
	{
		return (self.IsReadOnly() || self.IsSaving());
	}, self);
    self.IsLoading = ko.observable(false);
	self.ValidationErrors = ko.observable("");

	self.Defaults = { // TODO: Use a user settings view-model
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

	self.Model = {
	    Id: ko.observable(0),
	    Timestamp: ko.observable(""),
	    Title: ko.observable("New Submission"),
	    UnderwriterNotes: ko.observable(""),
	    QuoteSheetNotes: ko.observable(""),

	    InsuredId: ko.observable(),
	    InsuredName: ko.observable(),
	    BrokerCode: ko.observable(),
	    BrokerSequenceId: ko.observable(),
	    BrokerPseudonym: ko.observable(),
	    BrokerGroupCode: ko.observable(),
	    BrokerContact: ko.observable(),
	    NonLondonBrokerCode: ko.observable(),
	    NonLondonBrokerName: ko.observable(),
	    UnderwriterCode: ko.observable(),
	    UnderwriterContactCode: ko.observable(),
	    LeaderNo: ko.observable(),
	    Leader: ko.observable(),
	    Domicile: ko.observable(),
	    Brokerage: ko.observable("").extend({ numeric: 9 }),
	    QuotingOfficeId: ko.observable(""),
	    
	    Options: ko.observableArray([]),
	    
	    SubmissionMarketWordingsList: ko.observableArray([]),
	    CustomSubmissionMarketWordingsList: ko.observableArray([]),
	    SubmissionTermsNConditionWordingsList: ko.observableArray([]),
	    CustomSubmissionTermsNConditionWordingsList: ko.observableArray([]),
	    SubmissionSubjectToClauseWordingsList: ko.observableArray([]),
	    CustomSubmissionSubjectToClauseWordingsList: ko.observableArray([]),
	    
	    NewBrokerContactName: '',
	    NewBrokerContactEmail: '',
	    NewBrokerContactPhoneNumber: '',
	    
	    AdditionalInsuredList: ko.observableArray([]),

		AuditTrails: []
	};

	self.Functions = {
		/*
			Bootstrap typeahead call-back for when a broker search term yield no results.
		*/
		InvalidBrokerSelected: function(element, options, koId, koValue, koDependents)
		{
			if (ko.isObservable(koId)) koId(-1);
			if (ko.isObservable(koValue)) koValue(-1);

			if (koDependents)
			{
				for (var koDependant in koDependents)
				{
					if (ko.isObservable(koDependents[koDependant]))
					{
						koDependents[koDependant](-1);
					}
				}
			}
		},
		/*
			Bootstrap typeahead call-back for when a currency search term yield no results.
		*/
		InvalidCurrencySelected: function(element, options, koId, koValue, koDependents)
		{
			if (ko.isObservable(koValue)) koValue(-1);
		},
		/*
			Bootstrap typeahead call-back for when a domicile search term yield no results.
		*/
		InvalidDomicileSelected: function(element, options, koId, koValue, koDependents)
		{
			if (ko.isObservable(koId)) koId(-1);
			if (ko.isObservable(koValue)) koValue(-1);
		},
		/*
			Bootstrap typeahead call-back for when a insured search term yield no results.
		*/
		InvalidInsuredSelected: function(element, options, koId, koValue, koDependents)
		{
			if (ko.isObservable(koId)) koId(0);
			if (ko.isObservable(koValue)) koValue($.trim($(element).val()).toUpperCase());
		},
		/*
			Bootstrap typeahead call-back for when a leader search term yield no results.
		*/
		InvalidLeaderSelected: function(element, options, koId, koValue, koDependents)
		{
			if (ko.isObservable(koValue)) koValue(-1);

			if (koDependents)
			{
				for (var koDependant in koDependents)
				{
					if (ko.isObservable(koDependents[koDependant]))
					{
			            koDependents[koDependant](-1);
			        }
			    }
			}
		},
		/*
			Bootstrap typeahead call-back for when a underwriter search term yield no results.
		*/
		InvalidUnderwriterSelected: function(element, options, koId, koValue, koDependents)
		{
			if (ko.isObservable(koValue)) koValue(-1);
		}
	};

	self.Subscriptions = function(model)
	{
		model.InsuredId.subscribe(function(value)
		{
			if (value === 0)
			{
				// TODO: Highlight and add tooltip for indication that a new Insured will be created
			}
		});
		
		model.InsuredName.subscribe(function(value) {
			self.SetInsuredTabLabel(value);
			self.SetInsuredLossRatios(value);
			self.SearchWorldCheck(value);
			self.CrossSellingCheck(value);
		});

		//model.BrokerCode.subscribe(function(value)
		//{
		//	self.Model.BrokerContact("");
		//});

		model.BrokerGroupCode.subscribe(function(value)
		{
			self.SetBrokerSummary(value);
			self.SetBrokerLossRatios(value);
		});
	};

	//#region MarketWording
	self.Model.QuotingOfficeId.subscribe(function(officeId)
	{
        if (self.IsLoading()) return;
	    var ajaxConfig = { Url: "/Admin/GetMarketWordingsForTeamOffice?teamId=" + self.Team().Id() + "&officeId=" + officeId, VerbType: "GET", ContentType: "application/json;charset=utf-8" };
	    var response = ConsoleApp.AjaxHelper(ajaxConfig);
	    self.Model.SubmissionMarketWordingsList.removeAll();
		response.success(function(data)
		{
			if (data != null && data.length > 0)
			{
	            var tmpSubmissionMarketWordingsList = [];
				$.each(data, function(key, value)
				{
	                tmpSubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                    .Id(value.MarketWording.Id)
	                    .DisplayOrder(value.DisplayOrder)
	                    .WordingRefNumber(value.MarketWording.WordingRefNumber)
	                    .Title(value.MarketWording.Title)
	                    
	                );
	            });
	            ko.utils.arrayPushAll(self.Model.SubmissionMarketWordingsList(), tmpSubmissionMarketWordingsList);
	            self.Model.SubmissionMarketWordingsList.valueHasMutated();
			} else
			{
                console.log("Team-Office(change)- No MarketWordings Found");
	        }
	    });
	});
	
	self.selectedMarketWording = ko.observable(new ConsoleApp.MarketWording());
	self.showImageProcessing_LoadingMarketWordings = ko.observable('block');
	self.onClickMarketWordingItem = function(item)
	{
		if (ko.isObservable(item))
		{
	        self.selectedMarketWording(item);
		} else
		{
	        self.selectedMarketWording(new ConsoleApp.MarketWording()
	            .Id(item.Id)
	            .WordingRefNumber(item.WordingRefNumber)
	            .Title(item.Title));
	    }
	};

	self.enableAddMarketWordingToSubmission = ko.computed(function()
	{
		if (self.selectedMarketWording() !== undefined && self.selectedMarketWording().Id() != 0)
		{
			return $.grep(self.Model.SubmissionMarketWordingsList(), function(i) { return i.Id() == self.selectedMarketWording().Id(); }).length == 0;
	    }
	    return false;
	}, self);
	self.click_AddMarketWordingToSubmission = function(e)
	{

		if (self.selectedMarketWording() !== undefined && self.selectedMarketWording().Id() != 0)
		{
			var result = $.grep(self.Model.SubmissionMarketWordingsList(), function(i) { return i.Id() == self.selectedMarketWording().Id(); });
			if (result.length == 0)
			{
	            self.Model.SubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                    .Id(self.selectedMarketWording().Id())
	                    .DisplayOrder(0)
	                    .WordingRefNumber(self.selectedMarketWording().WordingRefNumber())
	                    .Title(self.selectedMarketWording().Title())
	            );

	            self.selectedMarketWording(new ConsoleApp.MarketWording());
	        }
	        }
	};
	
	self.click_RemoveMarketWordingFromSubmission = function(e)
	{

		if (self.selectedSubmissionMarketWording() !== undefined && self.selectedSubmissionMarketWording().Id() != 0)
		{

			var removeItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function(item)
			{
	            return item.Id() == self.selectedSubmissionMarketWording().Id();
	        }); // TODO: Remove unused "removeItem" variable

			self.Model.SubmissionMarketWordingsList.remove(function(item) { return item.Id() == self.selectedSubmissionMarketWording().Id(); });
	        self.selectedSubmissionMarketWording(new ConsoleApp.MarketWordingSettingDto());
	    }
	};

	self.selectedSubmissionMarketWording = ko.observable(new ConsoleApp.MarketWordingSettingDto());

	self.enableRemoveMarketWordingFromSubmission = ko.computed(function()
	{
		if (self.selectedSubmissionMarketWording() !== undefined && self.selectedSubmissionMarketWording().Id() != 0)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);
	self.showImageProcessing_LoadingSubmissionMarketWordings = ko.observable('block'); // TODO: use true/false for states, not stylesheet values
	self.onClickSubmissionMarketWordingItem = function(item)
	{
	    self.selectedSubmissionMarketWording(item);
	};
	self.enableSelectedSubmissionMarketWordingMoveUp = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });
	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
	self.onClick_selectedSubmissionMarketWordingMoveUp = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });


	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        var array = self.Model.SubmissionMarketWordingsList();
	        self.Model.SubmissionMarketWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedSubmissionMarketWordingMoveDown = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });


	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.SubmissionMarketWordingsList().length != i + 1)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
    
	self.onClick_selectedSubmissionMarketWordingMoveDown = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionMarketWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionMarketWording().Id());
	    });


	    var i = self.Model.SubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.SubmissionMarketWordingsList().length != i + 1)
		{
	        var array = self.Model.SubmissionMarketWordingsList();
	        self.Model.SubmissionMarketWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};

	self.customMarketWordingRef = ko.observable("");
	self.customMarketWording = ko.observable("");
	
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_AddCustomMarketWordingToSubmission = function()
	{
		if (self.customMarketWordingRef() !== undefined && self.customMarketWordingRef() != "")
		{
			var result = $.grep(self.Model.CustomSubmissionMarketWordingsList(), function(i) { return i.WordingRefNumber() == self.customMarketWordingRef(); });
			if (result.length == 0)
			{
	            self.Model.CustomSubmissionMarketWordingsList.push(new ConsoleApp.MarketWordingSettingDto()
	                    .Id(0)
	                    .DisplayOrder(0)
	                    .WordingRefNumber(self.customMarketWordingRef())
	                    .Title(self.customMarketWording())
	            );
	            self.customMarketWordingRef("");
	            self.customMarketWording("");
	        }
	        }
	};
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_RemoveCustomMarketWordingToSubmission = function()
	{
		if (self.selectedCustomSubmissionMarketWording() !== undefined && self.selectedCustomSubmissionMarketWording().WordingRefNumber() != "")
		{
	        self.customMarketWordingRef(self.selectedCustomSubmissionMarketWording().WordingRefNumber());
	        self.customMarketWording(self.selectedCustomSubmissionMarketWording().Title());
			self.Model.CustomSubmissionMarketWordingsList.remove(function(item) { return item.WordingRefNumber() == self.selectedCustomSubmissionMarketWording().WordingRefNumber(); });

	        self.selectedCustomSubmissionMarketWording(new ConsoleApp.MarketWordingSettingDto());

	    }
	};
	self.enableAddCustomMarketWordingToSubmission = ko.computed(function()
	{
		if (self.customMarketWordingRef() !== undefined && self.customMarketWordingRef() != "")
		{
			return $.grep(self.Model.CustomSubmissionMarketWordingsList(), function(i) { return i.WordingRefNumber() == self.customMarketWordingRef(); }).length == 0;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);

	
	self.selectedCustomSubmissionMarketWording = ko.observable(new ConsoleApp.MarketWordingSettingDto());
	self.enableRemoveCustomMarketWordingFromSubmission = ko.computed(function()
	{
		if (self.selectedCustomSubmissionMarketWording() !== undefined && self.selectedCustomSubmissionMarketWording().WordingRefNumber() != "")
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);

	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_CustomSubmissionMarketWordingItem = function(item)
	{
	    self.selectedCustomSubmissionMarketWording(item);
	};
	self.enableSelectedCustomSubmissionMarketWordingMoveUp = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_selectedCustomSubmissionMarketWordingMoveUp = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        var array = self.Model.CustomSubmissionMarketWordingsList();
	        self.Model.CustomSubmissionMarketWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedCustomSubmissionMarketWordingMoveDown = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.CustomSubmissionMarketWordingsList().length != i + 1)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_selectedCustomSubmissionMarketWordingMoveDown = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionMarketWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionMarketWording().Title());
	    });


	    var i = self.Model.CustomSubmissionMarketWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.CustomSubmissionMarketWordingsList().length != i + 1)
		{
	        var array = self.Model.CustomSubmissionMarketWordingsList();
	        self.Model.CustomSubmissionMarketWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    //#endregion 
    
    //#region TermsNConditionWording
    
	self.Model.QuotingOfficeId.subscribe(function(officeId)
	{
        if (self.IsLoading()) return;
	    var ajaxConfig = { Url: "/Admin/GetTermsNConditionWordingsForTeamOffice?teamId=" + self.Team().Id() + "&officeId=" + officeId, VerbType: "GET", ContentType: "application/json;charset=utf-8" };
	    var response = ConsoleApp.AjaxHelper(ajaxConfig);
	    self.Model.SubmissionTermsNConditionWordingsList.removeAll();
		response.success(function(data)
		{
			if (data != null && data.length > 0)
			{
                var tempSubmissionTermsNConditionWordingsList = [];
				$.each(data, function(key, value)
				{
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
	          
			} else
			{
                console.log("Team-Office(change)- No TermsNConditionWordings Found");
	        }
	    });

	});

	self.selectedTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWording());
	self.showImageProcessing_LoadingTermsNConditionWordings = ko.observable('block'); // TODO: Use true/false for states, not stylesheet values
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClickCheckTermsNConditionWordingItem = function(item)
	{
		var result = $.grep(self.Model.SubmissionTermsNConditionWordingsList(), function(i)
		{
			return i.Id() == item.Id();
		});
		if (result.length == 1)
		{

			if (result[0].IsStrikeThrough() == true)
			{
                result[0].IsStrikeThrough(false);
				item.IsStrikeThrough = false;
			}
			else
                {
				{
                    result[0].IsStrikeThrough(true);
					item.IsStrikeThrough = true;
                }
            }
            self.Model.SubmissionTermsNConditionWordingsList.valueHasMutated();
        }
    };
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClickTermsNConditionWordingItem = function(item)
	{
		if (ko.isObservable(item))
		{
	        self.selectedTermsNConditionWording(item);
		} else
		{
	        self.selectedTermsNConditionWording(new ConsoleApp.TermsNConditionWording()
	            .Id(item.Id)
	            .WordingRefNumber(item.WordingRefNumber)
	            .Title(item.Title));
	    }
	};
	self.enableAddTermsNConditionWordingToSubmission = ko.computed(function()
	{
		if (self.selectedTermsNConditionWording() !== undefined && self.selectedTermsNConditionWording().Id() != 0)
		{
			return $.grep(self.Model.SubmissionTermsNConditionWordingsList(), function(i) { return i.Id() == self.selectedTermsNConditionWording().Id(); }).length == 0;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.click_AddTermsNConditionWordingToSubmission = function(e)
	{

		if (self.selectedTermsNConditionWording() !== undefined && self.selectedTermsNConditionWording().Id() != 0)
		{

			var result = $.grep(self.Model.SubmissionTermsNConditionWordingsList(), function(i) { return i.Id() == self.selectedTermsNConditionWording().Id(); });
			if (result.length == 0)
			{
	            self.Model.SubmissionTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWordingSettingDto()
	                    .Id(self.selectedTermsNConditionWording().Id())
	                    .DisplayOrder(0)
	                    .WordingRefNumber(self.selectedTermsNConditionWording().WordingRefNumber())
	                    .IsStrikeThrough(false)
	                    .Title(self.selectedTermsNConditionWording().Title())
	            );

	            self.selectedTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
	        }
	        }
	};
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.click_RemoveTermsNConditionWordingFromSubmission = function(e)
	{

		if (self.selectedSubmissionTermsNConditionWording() !== undefined && self.selectedSubmissionTermsNConditionWording().Id() != 0)
		{

			var removeItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function(item)
			{
	            return item.Id() == self.selectedSubmissionTermsNConditionWording().Id();
	        });

			self.Model.SubmissionTermsNConditionWordingsList.remove(function(item) { return item.Id() == self.selectedSubmissionTermsNConditionWording().Id(); });

	        self.selectedSubmissionTermsNConditionWording(new ConsoleApp.TermsNConditionWordingSettingDto());

	    }
		else
		{ // TODO: Redundant else statement
	    }

	};

	self.selectedSubmissionTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWordingSettingDto());
	self.enableRemoveTermsNConditionWordingFromSubmission = ko.computed(function()
	{
		if (self.selectedSubmissionTermsNConditionWording() !== undefined && self.selectedSubmissionTermsNConditionWording().Id() != 0)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);
	
	self.showImageProcessing_LoadingSubmissionTermsNConditionWordings = ko.observable('block'); // TODO: Use true/false for states, not stylesheet values

	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClickSubmissionTermsNConditionWordingItem = function(item)
	{
	    self.selectedSubmissionTermsNConditionWording(item);
	};
    
	self.enableSelectedSubmissionTermsNConditionWordingMoveUp = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });
	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	       return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_selectedSubmissionTermsNConditionWordingMoveUp = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });


	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        var array = self.Model.SubmissionTermsNConditionWordingsList();
	        self.Model.SubmissionTermsNConditionWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedSubmissionTermsNConditionWordingMoveDown = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });


	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.SubmissionTermsNConditionWordingsList().length != i + 1)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_selectedSubmissionTermsNConditionWordingMoveDown = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.SubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Id() == self.selectedSubmissionTermsNConditionWording().Id());
	    });


	    var i = self.Model.SubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.SubmissionTermsNConditionWordingsList().length != i + 1)
		{
	        var array = self.Model.SubmissionTermsNConditionWordingsList();
	        self.Model.SubmissionTermsNConditionWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    
	self.customTermsNConditionWordingRef = ko.observable("");
	self.customTermsNConditionWording = ko.observable("");
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClickCheckCustomTermsNConditionWordingItem = function(item)
	{
		var result = $.grep(self.Model.CustomSubmissionTermsNConditionWordingsList(), function(i) { return i.WordingRefNumber() == item.WordingRefNumber; });
		if (result.length == 1)
		{

			if (result[0].IsStrikeThrough() == true)
			{
                result[0].IsStrikeThrough(false);
                item.IsStrikeThrough = false;
			} else
                {
				{
                    result[0].IsStrikeThrough(true);
                    item.IsStrikeThrough = true;
                }
            }
            self.Model.CustomSubmissionTermsNConditionWordingsList.valueHasMutated();
        }
    };
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_AddCustomTermsNConditionWordingToSubmission = function()
	{
		if (self.customTermsNConditionWording() !== undefined && self.customTermsNConditionWording() != "")
		{

			var result = $.grep(self.Model.CustomSubmissionTermsNConditionWordingsList(), function(i) { return i.WordingRefNumber() == self.customTermsNConditionWordingRef(); });
			if (result.length == 0)
			{
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
	        }
	};
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_RemoveCustomTermsNConditionWordingToSubmission = function()
	{
		if (self.selectedCustomSubmissionTermsNConditionWording() !== undefined && self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber() != "")
		{
	        self.customTermsNConditionWordingRef(self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber());
	        self.customTermsNConditionWording(self.selectedCustomSubmissionTermsNConditionWording().Title());
			self.Model.CustomSubmissionTermsNConditionWordingsList.remove(function(item) { return item.WordingRefNumber() == self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber(); });

	        self.selectedCustomSubmissionTermsNConditionWording(new ConsoleApp.TermsNConditionWordingSettingDto());

	    }
	};
	self.enableAddCustomTermsNConditionWordingToSubmission = ko.computed(function()
	{
		if (self.customTermsNConditionWordingRef() !== undefined && self.customTermsNConditionWordingRef() != "")
		{
			return $.grep(self.Model.CustomSubmissionTermsNConditionWordingsList(), function(i) { return i.WordingRefNumber() == self.customTermsNConditionWordingRef(); }).length == 0;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);
	
	self.selectedCustomSubmissionTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWordingSettingDto());
	self.enableRemoveCustomTermsNConditionWordingFromSubmission = ko.computed(function()
	{
		if (self.selectedCustomSubmissionTermsNConditionWording() !== undefined && self.selectedCustomSubmissionTermsNConditionWording().WordingRefNumber() != "")
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_CustomSubmissionTermsNConditionWordingItem = function(item)
	{
	    self.selectedCustomSubmissionTermsNConditionWording(item);
	};

	self.enableSelectedCustomSubmissionTermsNConditionWordingMoveUp = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });


	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_selectedCustomSubmissionTermsNConditionWordingMoveUp = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });

	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        var array = self.Model.CustomSubmissionTermsNConditionWordingsList();
	        self.Model.CustomSubmissionTermsNConditionWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	// TODO: Lots of "enable...this" and "enable...that", surely there is a better way... ?
	self.enableSelectedCustomSubmissionTermsNConditionWordingMoveDown = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });
	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.CustomSubmissionTermsNConditionWordingsList().length != i + 1)
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	});
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_selectedCustomSubmissionTermsNConditionWordingMoveDown = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionTermsNConditionWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionTermsNConditionWording().Title());
	    });


	    var i = self.Model.CustomSubmissionTermsNConditionWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.CustomSubmissionTermsNConditionWordingsList().length != i + 1)
		{
	        var array = self.Model.CustomSubmissionTermsNConditionWordingsList();
	        self.Model.CustomSubmissionTermsNConditionWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    
    //#endregion 

    //#region SubjectToClauseWording
	self.Model.QuotingOfficeId.subscribe(function(officeId)
	{
        if (self.IsLoading()) return;
	    var ajaxConfig = { Url: "/Admin/GetSubjectToClauseWordingsForTeamOffice?teamId=" + self.Team().Id() + "&officeId=" + officeId, VerbType: "GET", ContentType: "application/json;charset=utf-8" };
	    var response = ConsoleApp.AjaxHelper(ajaxConfig);
	    self.Model.CustomSubmissionSubjectToClauseWordingsList.removeAll();
		response.success(function(data)
		{
			if (data != null && data.length > 0)
			{
	            var tmpCustomSubmissionSubjectToClauseWordingsList = [];
				$.each(data, function(key, value)
				{
	                tmpCustomSubmissionSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWordingSettingDto()
	                    .Id(0)
	                    .DisplayOrder(value.DisplayOrder)
	                    .IsStrikeThrough(value.IsStrikeThrough)
	                    .Title(value.SubjectToClauseWording.Title)
	                );
	            });
	            ko.utils.arrayPushAll(self.Model.CustomSubmissionSubjectToClauseWordingsList(), tmpCustomSubmissionSubjectToClauseWordingsList);
	            self.Model.CustomSubmissionSubjectToClauseWordingsList.valueHasMutated();
			} else
			{
                console.log("Team-Office(change)- No SubjectToClauseWordings Found");
	        }
	    });
	});
	
	self.selectedSubjectToClauseWording = ko.observable(new ConsoleApp.SubjectToClauseWording());
	self.showImageProcessing_LoadingSubjectToClauseWordings = ko.observable('block');
	self.customSubjectToClauseWording = ko.observable("");
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_AddCustomSubjectToClauseWordingToSubmission = function()
	{
		if (self.customSubjectToClauseWording() !== undefined && self.customSubjectToClauseWording() != "")
		{

			var result = $.grep(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function(i) { return i.Title() == self.customSubjectToClauseWording(); });
			if (result.length == 0)
			{
	            self.Model.CustomSubmissionSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWordingSettingDto()
	                    .Id(0)
	                    .DisplayOrder(0)
	                    .IsStrikeThrough(false)
	                    .Title(self.customSubjectToClauseWording())
	            );

	            self.customSubjectToClauseWording("");
	        }
	        }
	};
	self.onClick_RemoveCustomSubjectToClauseWordingToSubmission = function()
	{
		if (self.selectedCustomSubmissionSubjectToClauseWording() !== undefined && self.selectedCustomSubmissionSubjectToClauseWording().Title() != "")
		{

			self.Model.CustomSubmissionSubjectToClauseWordingsList.remove(function(item) { return item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title(); });

	        self.customSubjectToClauseWording(self.selectedCustomSubmissionSubjectToClauseWording().Title());
	        self.selectedCustomSubmissionSubjectToClauseWording(new ConsoleApp.SubjectToClauseWordingSettingDto());
	    }
	};
	self.enableAddCustomSubjectToClauseWordingToSubmission = ko.computed(function()
	{
		if (self.customSubjectToClauseWording() !== undefined && self.customSubjectToClauseWording() != "")
		{
			return $.grep(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function(i) { return i.Title() == self.customSubjectToClauseWording(); }).length == 0;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);
	self.selectedCustomSubmissionSubjectToClauseWording = ko.observable(new ConsoleApp.SubjectToClauseWordingSettingDto());
	self.enableRemoveCustomSubjectToClauseWordingFromSubmission = ko.computed(function()
	{
		if (self.selectedCustomSubmissionSubjectToClauseWording() !== undefined && self.selectedCustomSubmissionSubjectToClauseWording().Title() != "")
		{
	        return true;
	    }
	    return false; // TODO: Use conditional ternary operator
	}, self);

	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClickCheckCustomSubjectToClauseWordingItem = function(item)
	{
		var result = $.grep(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function(i) { return i.Title() == item.Title; });
		if (result.length == 1)
		{

			if (result[0].IsStrikeThrough() == true)
			{
	            result[0].IsStrikeThrough(false);
	            item.IsStrikeThrough = false;
			} else
	            {
				{
	                result[0].IsStrikeThrough(true);
	                item.IsStrikeThrough = true;
	            }
	        }
	        self.Model.CustomSubmissionSubjectToClauseWordingsList.valueHasMutated();
	    }
	};

	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_CustomSubmissionSubjectToClauseWordingItem = function(item)
	{
	    self.selectedCustomSubmissionSubjectToClauseWording(item);
	};
	self.enableSCustomSubmissionSubjectToClauseWordingMoveUp = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });
	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        return true;
	    }
	    return false;
	});
	self.onClick_selectedCustomSubmissionSubjectToClauseWordingMoveUp = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });


	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
		if (i >= 1)
		{
	        var array = self.Model.CustomSubmissionSubjectToClauseWordingsList();
	        self.Model.CustomSubmissionSubjectToClauseWordingsList.splice(i - 1, 2, array[i], array[i - 1]);
	    }
	};
	self.enableSelectedCustomSubmissionSubjectToClauseWordingMoveDown = ko.computed(function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });
	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.CustomSubmissionSubjectToClauseWordingsList().length != i + 1)
		{
	        return true;
	    }
	    return false;
	});
	// TODO: Remove all onClick functions where we do not care about the event (replace with generic functions)
	self.onClick_selectedCustomSubmissionSubjectToClauseWordingMoveDown = function()
	{
		var moveItem = ko.utils.arrayFirst(self.Model.CustomSubmissionSubjectToClauseWordingsList(), function(item)
		{
	        return (item.Title() == self.selectedCustomSubmissionSubjectToClauseWording().Title());
	    });


	    var i = self.Model.CustomSubmissionSubjectToClauseWordingsList.indexOf(moveItem);
		if (i >= 0 && self.Model.CustomSubmissionSubjectToClauseWordingsList().length != i + 1)
		{
	        var array = self.Model.CustomSubmissionSubjectToClauseWordingsList();
	        self.Model.CustomSubmissionSubjectToClauseWordingsList.splice(i, 2, array[i + 1], array[i]);
	    }
	};
    //#endregion

	self.Initialise = function(viewModel, syncCallback)
	{
		if (!domId)
		{
			toastr.error("No DOM Id specified, cannot initialise Submission view model");

			self.Cancel();
		}
		else
		{
			$(".val-worldcheck-matches", self.Form).html("Please provide an Insured Name");
			$(".val-related-loss-ratios", self.Form).html("Please provide an Insured Name");
			$(".val-cross-selling", self.Form).html("Please provide an Insured Name");

			self.SetCreateBrokerContact();
			self.PopAuditTrails();

			if (!viewModel) self.Model.Options().push(new Option(0, domId, self));

			self.Subscriptions(self.Model);
			// TODO: self.Computeds(self.Model);
			
			self.DirtyFlag = new ko.dirtyFlag(self);
			self.BindKO(viewModel);

			if (self.Id > 0)
			{
				setTimeout(function()
				{
					self.LoadSubmission("/api/submissionapi/getsubmission", syncCallback);
				}, 100);
			}
			else
			{
				setTimeout(function()
				{
					self.InitialiseForm();
				}, 100);
			}
		}
	};

	self.LoadSubmission = function(url, syncCallback)
	{
		$.ajax(
			{
				url: url,
				type: "GET",
				data: { id: self.Id },
				dataType: "json",
				contentType: "application/json",
				success: function(data, status, xhr)
				{
					self.IsLoading(true);
					self.CanCreateQuoteSheet(true);
					self.IsSaving(false);
					self.ValidationErrors("");

					toastr.success("Submission retrieved");

					if (data.Submission)
					{
						self.syncJSON(data.Submission);

						if (syncCallback) syncCallback(data.Submission);

						self.ShowAuditTrails(self.Model.Id());
						self.InitialiseForm();
						self.IsLoading(false);

						toastr.success("Submission synchronised");
					}
					else self.IsLoading(false);
				}
			});
	};

	self.InitialiseForm = function()
	{
		//console.log('InitialiseForm()');
		self.AttachValidation();
		self.InitialisePane();
		self.SetDocumentDetails();
		self.BindUnload();
		self.DirtyFlag.Reset();
		self.IsInitialised(true);
	};

	self.SetDefaultValuesAndQuoteTemplates = function()
	{
		$.when(self.GetTeamBySubmissionTypeId(self.Model.submissionTypeId()))
			.done(function()
			{
				self.GetQuoteTemplates(self.Team().Id());

				var defaultDomicile = $("input[type='hidden'][name='DefaultDomicile']", self.Form).val(),
				    defaultQuoteExpiry = $("input[type='hidden'][name='DefaultQuoteExpiry']", self.Form).val(),
				    defaultOffice = $("input[type='hidden'][name='DefaultOffice']", self.Form).val(),
				    defaultUnderwriter = $("input[type='hidden'][name='DefaultUnderwriter']", self.Form).val(),
				    defaultPolicyType = $("input[type='hidden'][name='DefaultPolicyType']", self.Form).val(),
				    defaultNonLondonBroker = $("input[type='hidden'][name='DefaultNonLondonBroker']", self.Form).val();

				if (defaultNonLondonBroker)
				{
					self.Model.NonLondonBrokerCode(defaultNonLondonBroker);
				}

				if (defaultDomicile)
				{
					self.Defaults.Domicile = defaultDomicile;

					self.Model.Domicile(defaultDomicile);
				}

				if (defaultUnderwriter)
				{
					self.Defaults.Underwriter = defaultUnderwriter;

					self.Model.UnderwriterCode(defaultUnderwriter);
					self.Model.UnderwriterContactCode(defaultUnderwriter);
				}

				if (defaultOffice)
				{
					self.Defaults.Office = defaultOffice;

					self.Model.QuotingOfficeId(defaultOffice);
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
					{
						self.Defaults.QuoteExpiry = quoteExpiryDate.format("DD MMM YYYY");
					}
				}
			});
	};

	self.SetDocumentDetails = function()
	{
		//console.log('SetDocumentDetails()');
		/* 
		TODO: Changing this computed to a subsription as well as SetInsuredDetails & SetBrokerDetails
    	$(".val-submission-documents", self.Form).html("");
	    */

		var policyIds = "";

		$.each(self.Model.Options(), function(optionIndex, optionData)
		{
			$.each(optionData.OptionVersions(), function(versionIndex, versionData)
			{
				$.each(versionData.Quotes(), function(quoteIndex, quoteData)
				{
					if (quoteData.RenPolId() !== "")
					{
						policyIds += quoteData.RenPolId() + ";";
					}

					if (quoteData.SubscribeReference() !== "")
					{
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
			success: function(data)
			{
				self.DocumentCount(self.CountIndication(data));

				var tableConfig = new BaseDataTable();

				tableConfig.bProcessing = false;
				tableConfig.aaSorting = [[2, "desc"]];

				$(".val-submission-documents", self.Form).html(data);
				$(".val-submission-documents .val-subscribe-policyid", self.Form).click(function(e)
				{
					OpenWebPolicy($(e.target).attr("href"));

					e.preventDefault();
				});
				$(".val-submission-documents .val-filenet-document", self.Form).click(function(e)
				{
					window.open($(e.target).attr("href"));

					e.preventDefault();
				});
				$(".val-submission-documents table", self.Form).dataTable(tableConfig);
			}
		});
	};
	
	self.GetTeamBySubmissionTypeId = function(submissionTypeId)
	{
		//console.log('GetTeamBySubmissionTypeId()');

		var ajaxConfig = { Url: "/Admin/GetTeamBySubmissionTypeId?submissionTypeId=" + submissionTypeId, VerbType: "GET" };

		var response = ConsoleApp.AjaxHelper(ajaxConfig);

		response.success(function(data)
		{
			self.Team(new ConsoleApp.Team(data.Id, data.Title));
			//console.log(self.Team());
		});
		
		return response;
    };

	self.PopAuditTrails = function()
	{
		$('.showAuditTrail').popover({
			html: true,
			content: function()
			{

				return '<div class="AuditTrailPoped">' + $('.val-submission-insuredName-audits', self.Form).html() + '</div>';

			},
			afterShowed: function()
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
		console.log('SetCreateBrokerContact()');

		var html;
		$.when(
			$.get('/Submission/_CreateBrokerContact', function(data) {
				html = data;
			})
		).done(function(data) {
			$('.addNewBrokerContact').popover({
				html: true,
				content: function()
				{
					// Note: this is not ideal, but I am being told to get this done fast and then fix other stuff...
					if (self.Model.NewBrokerContactName != '')
					{
						html = html.replace('<input class="span12 new-broker-contact-name" data-bind="value: Model.NewBrokerContactName" data-val="true" data-val-required="The Name field is required." name="NewBrokerContactName" type="text" value="" />', '<input class="span12 new-broker-contact-name" data-bind="value: Model.NewBrokerContactName" data-val="true" data-val-required="The Name field is required." name="NewBrokerContactName" type="text" value="' + self.Model.NewBrokerContactName + '" />');
						html = html.replace('<input class="span12 new-broker-contact-email" data-bind="value: Model.NewBrokerContactName, validate: IsInitialised" data-val="true" data-val-required="The Email field is required." name="NewBrokerContactEmail" type="text" value="" />', ' <input class="span12 new-broker-contact-email" data-bind="value: Model.NewBrokerContactName, validate: IsInitialised" data-val="true" data-val-required="The Email field is required." name="NewBrokerContactEmail" type="text" value="' + self.Model.NewBrokerContactEmail + '" />');
						html = html.replace('<input class="span12 new-broker-contact-phonenumber" data-bind="value: Model.NewBrokerContactPhoneNumber, validate: IsInitialised" data-val="true" data-val-required="The Phone Number field is required." name="NewBrokerContactPhoneNumber" type="text" value="" />', '<input class="span12 new-broker-contact-phonenumber" data-bind="value: Model.NewBrokerContactPhoneNumber, validate: IsInitialised" data-val="true" data-val-required="The Phone Number field is required." name="NewBrokerContactPhoneNumber" type="text" value="' + self.Model.NewBrokerContactPhoneNumber + '" />');
					}
					
					return html;
				},
				trigger: 'manual'
			});
		});

		 
		$(self.Form).on("click", '.createBrokerContact', function() {

				self.Model.NewBrokerContactName = $(".new-broker-contact-name", self.Form).val();
				self.Model.NewBrokerContactEmail = $(".new-broker-contact-email", self.Form).val();
				self.Model.NewBrokerContactPhoneNumber = $(".new-broker-contact-phonenumber", self.Form).val();

				if (self.Model.NewBrokerContactName === ''
					|| self.Model.NewBrokerContactEmail == ''
					|| self.Model.NewBrokerContactPhoneNumber === '') {
					toastr.error("New Broker Contact Name, Email and Phone Number are required fields");
					return;
				}

			self.Model.BrokerContact(self.Model.NewBrokerContactName);
				$('.addNewBrokerContact', self.Form).popover('hide');
				self.createBrokerContactAlreadyShown = false;

		});
	};
	
	self.createBrokerContactAlreadyShown = false;
	self.typeAheadCreateBrokerContact = function()
	{
		$(".new-broker-contact-name", self.Form).val('');
		$(".new-broker-contact-email", self.Form).val('');
		$(".new-broker-contact-phonenumber", self.Form).val('');
		
		if (self.Model.BrokerContact() !== undefined)
			self.Model.NewBrokerContactName = self.Model.BrokerContact();

		if (!self.createBrokerContactAlreadyShown) {
			self.createBrokerContactAlreadyShown = true;
			$('.addNewBrokerContact', self.Form).popover('show');
		}
	};

	self.showCreateBrokerContact = function() {
		$('.addNewBrokerContact', self.Form).popover('toggle');
	};

	self.GetQuoteTemplates = function(teamId)
	{
		self.TeamQuoteTemplatesList.removeAll();

		var ajaxConfig = { Url: "/Admin/GetQuoteTemplatesForTeam?teamId=" + teamId, VerbType: "GET" };

		var response = ConsoleApp.AjaxHelper(ajaxConfig);

		response.success(function(data)
		{

			if (data.length > 0)
			{
				$.each(data, function(key, value)
				{
					self.TeamQuoteTemplatesList.push(new ConsoleApp.QuoteTemplate()
						.Id(value.Id)
						.Name(value.Name)
						.RdlPath(value.RdlPath));
				});
			}
			else
			{
				toastr.info("No Quote Templates for Team");
			}

		});

		return response;
	};
	
    self.CreateFirstOption = function(vm) {
    	//console.log('CreateFirstOption()');
    	self.SetDefaultValuesAndQuoteTemplates();
		self.Model.Options.push(new Option(0, domId, self));
		var vmOption1 = self.Model.Options()[0];
		return vmOption1;
	};
	
	self.SetInsuredTabLabel = function(insuredName)
	{
		$("a[href$='" + domId + "']:first")
			.attr("title", insuredName).attr("alt", insuredName)
			.children("span")
			.text(!insuredName
				? "New Submission"
				: (insuredName.length > 14)
					? insuredName.substring(0, 12) + "..."
					: insuredName);

		$("a[href$='" + domId + "']:first button")
			.unbind("click")
			.bind("click", function()
			{
				self.Cancel();
			});
	};

	/*
		TODO: Cleanup

		We should really be using an InitPV_ function.
	*/
    self.SetInsuredLossRatios = function (insuredName) {

    	//console.log("SetInsuredLossRatios()");

    	$(".val-related-loss-ratios", self.Form).html('<p class="image_loading_LossRatios">'
				+ '<img style="width: 20px; height: 20px;" src="/content/images/animated_progress.gif">'
				+ 'Loading...'
				+ '</p>');

		$.ajax(
		{
			url: "/insured/_insureddetailsminimal",
			type: "GET",
			data: { insuredName: encodeURIComponent(insuredName) },
			dataType: "html",
			success: function(data)
			{
				self.RelatedCount(self.CountIndication(data));

				var tableConfig = new BaseDataTable();

				tableConfig.aaSorting = [[1, "desc"]];
				tableConfig.fnDrawCallback = function(oSettings)
				{
					$(oSettings.nTable).find('th').each(function()
					{
						if ($(this).children('span').is("[data-toggle]"))
						{
				            $(this).children('span').tooltip();
				        }
					});

					$(oSettings.nTable).find("td.val-insddet-lr").filter(function () {
					    return parseFloat($(this).text()) >= 100;
					}).addClass("text-warning");
				};

				tableConfig.aoColumnDefs = [
		        {
		            "aTargets": [2],
		            "sType": "numeric",
		            "mRender": function (iIn, t)
		            {                        
		                if (t === 'display') {		   
		                    return $.formatCurrency(iIn);
		                }
			            return iIn;
		            }
		        }];

				$(".val-related-loss-ratios", self.Form).html(data);
				$(".val-insured-minimal-datatable", self.Form).dataTable(tableConfig);
			}
		});
	};
    
	self.CountIndication = function(data)
	{
		var count = (data) ? data.match(/<tr>/gi).length - 1 : 0;

		return (count > 0) ? "(" + count + ")" : "";
	};

	/*
		TODO: Cleanup

		This was added quickly for a demonstration and has not been improved upon since.
		We should really be using an InitPV_ function.
	*/
	self.SetBrokerSummary = function(brokerGrpCd)
	{
		self.BrokerRating("");
		self.BrokerScore("");
		self.BrokerCreditLimit("");

		if (brokerGrpCd && brokerGrpCd !== -1)
		{
			$.ajaxLatest("/broker/getbrokersummarybyid",
			{
				data: { brokerCd: brokerGrpCd }, // TODO: Updated Broker controller to use proper variable name
				dataType: "json",
				success: function(data)
				{
					self.BrokerRating(data.BrokerRating ? data.BrokerRating : "");
					self.BrokerScore(data.BrokerScore ? data.BrokerScore : "");
					self.BrokerCreditLimit($.formatCurrency(data.BrokerCreditLimit));
				}
			});
		}
	};

	self.SetBrokerLossRatios = function(brokerGrpCd)
	{
		$(".val-broker-lossratio-sparkline", self.Form).html("");
		$(".val-broker-lossratio-graph", self.Form).html("");

		if (brokerGrpCd && brokerGrpCd !== -1)
		{
			$(".val-broker-lossratio-loadmessage", self.Form).hide();
			$(".val-broker-lossratio-loading", self.Form).show();

			$.ajaxLatest("/broker/getbrokerdevelopmentstatsbyid",
			{
				data: { brokerCd: brokerGrpCd }, // TODO: Updated Broker controller to use proper variable name
				success: function(data)
				{
					if (!data.Months)
					{
						$(".val-broker-lossratio-sparkline", self.Form).html("No Broker Loss Ratio's found");
					}
					else
					{
						var brokerSparklines = $(".val-broker-lossratio-sparkline", self.Form),
							brokerSeries = [];

						$.each(data.LossRatioForMonthList, function(key, item)
						{
							$('<br /><b><span class="item">' + item.Year + '</span></b>').appendTo(brokerSparklines);

							$('<span></span>').appendTo(brokerSparklines).kendoSparkline({ theme: "blueOpal", data: item.LossRatios });

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
					}
				},
				complete: function()
				{
					$(".val-broker-lossratio-loading", self.Form).hide();
				}
			});
		}
		else
		{
			$(".val-broker-lossratio-loadmessage", self.Form).show();
		}
	};

	self.CrossSellingCheck = function(insuredName)
	{
		// TODO: Add to HTML and use state driven visibility
		$(".val-cross-selling", self.Form)
			.html('<p class="image_loading_WorldCheck">'
				+ '<img style="width: 20px; height: 20px;" src="/content/images/animated_progress.gif">'
				+ 'Loading...'
				+ '</p>');

    	$.ajax(
		{
			url: "/submission/_crosssellingcheck",
			type: "GET",
			data: { insuredName: encodeURIComponent(insuredName), submissionId: self.Id },
			dataType: "html",
			success: function(data)
			{
				var count = (data.match(/<tr>/g) || []).length - 1;
				
				self.CrossSellingCount(count === 0 ? '' : '(' + count + ')');

				var tableConfig = new BaseDataTable();
				
				tableConfig.aaSorting = [[0, "desc"]];

				$(".val-cross-selling", self.Form).html(data);
				$(".val-cross-selling-datatable", self.Form).dataTable(tableConfig);

				if (count !== 0)
				{
					// TODO: Use lower case classes
					$(".crossSellingLink", self.Form)
						.effect("highlight", { color: "#A1EDA7" }, 5000, null);
				}

				$(".val-edit-submission-cross-selling", self.Form).click(SubmissionEditButton_Click);
			}
		});
    };

	self.SearchWorldCheck = function(insuredName)
	{
		// TODO: Why is this here ?
		$(".val-related-loss-ratios", self.Form).html("");
		
		// TODO: Add to HTML and use state driven visibility
    	$(".val-worldcheck-matches", self.Form)
			.html('<p class="image_loading_WorldCheck">'
				+ '<img style="width: 20px; height: 20px;" src="/content/images/animated_progress.gif">'
				+ 'Loading...'
				+ '</p>');

		$.ajax(
		{
			url: "/worldcheck/_worldchecksearchmatches",
			type: "GET",
			data: { insuredName: encodeURIComponent(insuredName) },
			dataType: "html",
			success: function(data)
			{
		    	var count = (data.match(/<tr>/g) || []).length - 1;

		    	self.WorldCheckCount(count === 0 ? '' : '(' + count + ')');

		    	var tableConfig = new BaseDataTable(null,
		    		"<p>For full functionality, click here: <a href='"
		    		+ window.WorldCheckUrl
		    		+ "' target='_blank'>World Check</a></p> <p>Please contact Compliance for any queries.</p>");

				tableConfig.aaSorting = [[0, "desc"]];
				tableConfig.aoColumnDefs = [{ "bSortable": false, "aTargets": [1] }];

				$(".val-worldcheck-matches", self.Form).html(data);
				$(".val-worldcheck-matches-datatable", self.Form).dataTable(tableConfig);

				if (count !== 0)
				{
					// TODO: Use lower case classes
					$(".worldCheckLink", self.Form)
						.effect("highlight", { color: "#EDA1B3" }, 5000, null);
			    }
		    }
		});
	    
		if (insuredName)
		{
			self.Model.AuditTrails.push(new ConsoleApp.AuditTrail(0, "Submission", "", "World Check",
				"World Check requested for insured name: " + insuredName));
        }
    };

	self.ShowLossRatioGraph = function()
	{
		if ($(".val-broker-lossratio-graph", self.Form).html().length > 0)
		{
			$(".val-broker-lossratio-modal", self.Form).modal("show");
		}
	};
	
	self.ShowAuditTrails = function(submissionId)
	{
        $(".val-submission-insuredName-audits", self.Form).html("");

        $.ajax(
		{
		    url: "/Submission/_WorldCheckAuditTrailForSubmission",
		    type: "GET",
		    data: { id: submissionId },
		    dataType: "html",
			success: function(data)
			{
		       // var tableConfig = new BaseDataTable();
		       // tableConfig.aaSorting = [[0, "desc"]];
		       // tableConfig.aoColumnDefs = [{ "bSortable": false, "aTargets": [1] }];
		        
		        $(".val-submission-insuredName-audits", self.Form).html(data);
		       // $(".val-worldcheck-auditTrails-datatable", self.Form).dataTable(tableConfig);
		    }
		});
      
    };
	
	self.BindKO = function(viewModel)
	{

		if (viewModel === undefined) // TODO: What about null values ?
			ko.applyBindings(self, document.getElementById(domId));
		else 
			ko.applyBindings(viewModel, document.getElementById(domId));
	};

	self.BindUnload = function()
	{
		/* TODO: Get this working for all browsers, including IE 9
		if ($.support.leadingWhitespace) // False for IE < 9
		{
			$(window).unbind("beforeunload." + domId)
				.bind("beforeunload." + domId, self.UnloadConfirmation);
		}*/
	};

	self.UnbindUnload = function()
	{
		$(window).unbind("beforeunload." + domId);
	};

	self.UnloadConfirmation = function(e)
	{
		var isDirty = (self.DirtyFlag) ? self.DirtyFlag.IsDirty() : false;

		if (e)
		{
			if (isDirty)
			{
				toastr.warning("Unsaved Submission changes detected");

				return "You have unsaved changes!";
			}

			return null;
		}
		else if (isDirty)
		{
			toastr.warning("Unsaved Submission changes detected");

			return window.confirm("You have unsaved changes!\n\rAre you sure you wish to leave ?");
		}

		return true;
	};

	self.AttachValidation = function()
	{
		/* TODO: Enable once we are ready
        $.validator.unobtrusive.parse("#" + domId + " .form");
		*/
	};

	self.click_AddAdditionalInsured = function() {
		console.log('Add');
		self.Model.AdditionalInsuredList.push({ Id: ko.observable(0), InsuredId: ko.observable(''), InsuredName: ko.observable(''), InsuredType: ko.observable('Additional'), InsuredTypes: ["Additional", "Cedent", "Interested Party", "Reinsured"] });
	};

	self.click_RemoveAdditionalInsured = function(item)
	{
		console.log('Remove');
		self.Model.AdditionalInsuredList.remove(item);
	};

	// Note: done at the inheriting submission level to add new properties at quote level etc...
	self.AddOption = function()
	{
		var length = self.Model.Options().length,
		    newOption = new Option(length, domId, self),
			submissionId = self.Model.Id();
	   
		newOption.SubmissionId(submissionId);

		length = self.Model.Options.push(newOption);

		self.AttachValidation();

		return length;
	};

	self.CopyOption = function()
	{
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

		$.each(newVersionData.Quotes, function(quoteIndex, quoteItem)
		{
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
			self.Save(element, e, function() { self.CreateQuoteSheet(element); });
		}
		else
		{
			self.CreateQuoteSheet(element);
		}
	};

	self.CreateQuoteSheet = function(element, e)
	{
		var quoteSheetTemplateId;
		if (self.TeamQuoteTemplatesList().length === 0)
		{
			toastr.warning("No Templates for quote sheet");
			return;
		}
		else if (self.TeamQuoteTemplatesList().length === 1)
		{
			quoteSheetTemplateId = self.TeamQuoteTemplatesList()[0].Id();
		}
		else
		{
			quoteSheetTemplateId = element.Id();
		}

		var optionList = [];
		$.each(self.Model.Options(), function(optionIndex, optionItem)
		{
			if (optionItem.AddToQuoteSheet())
			{
				optionList.push(
				{
					OptionId: optionItem.Id(),
					OptionVersionNumberList: [optionItem.CurrentVersion().VersionNumber()]
				});
			}
		});

		if (optionList.length === 0)
		{
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
			success: function(data, status, xhr)
			{
				toastr.info("Quote sheet created");

				self.SetDocumentDetails();

				if (data.Submission)
				{
					toastr.success("Submission updated");

					self.syncJSON(data.Submission);
					self.DirtyFlag.Reset();

					toastr.success("Submission synchronised");
				}

				var responseLocation = xhr.getResponseHeader("Location");

				if (responseLocation)
				{
					window.open(responseLocation, "_blank");
				}
			},
			complete: function(xhr, status)
			{
				self.CanCreateQuoteSheet(true);
				self.CreatingQuoteSheet(false);
			}
		});
	};

	self.ToggleComparison = function()
	{
		$(".val-optioncomparison-datatable", $("#" + domId)).each(function()
		{
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

	self.Cancel = function()
	{
		if (self.UnloadConfirmation(null))
		{
			self.UnbindUnload();

			// TODO: Kill view model ?

			Val_CloseTab(domId);
		}
	};

	self.Save = function(element, e, callback, url, syncCallback)
	{
		self.IsSaving(true);
		self.CanCreateQuoteSheet(false);

		//if (self.Form.valid()) TODO: Replace with Knockout Validation IsValid() check
		{
			var modelJSON = self.toJSON(),
			    isNew = (self.Id === 0);

			toastr.info("Saving Submission");

			$.ajax(
			{
					url: url ? url : (!isNew) ? "/api/submissionapi/editsubmission" : "/api/submissionapi/createsubmission",
					headers: { 'X-SubmissionType': self.Model.submissionTypeId() }, // TODO: I am guessing that this is not valid HTML5
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
						
						if (data.Submission)
						{
							self.syncJSON(data.Submission);

							if (syncCallback) syncCallback(data.Submission);

							self.ShowAuditTrails(self.Model.Id());
							
							toastr.success("Submission synchronised");
						}
						
						self.DirtyFlag.Reset();
						
						if (callback) callback(element, e);
						
						self.IsLoading(false);

					    if (isNew) {
					        if (self.Model.Options()[0].OptionVersions()[0].Quotes()[0].RenPolId() != "") {
					            $.pubsub.publish("policyRenewed",
					                {
					                    RenPolId: self.Model.Options()[0].OptionVersions()[0].Quotes()[0].RenPolId()
					                });

					        } else {
					            $.pubsub.publish("submissionCreated",
					                {
					                    SubscribeReference: self.Model.Options()[0].OptionVersions()[0].Quotes()[0].SubscribeReference()
					                });
					        }
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
		//else
		//{
		//	// TODO: This is probably not sufficient...
		//	toastr.warning("Submission validation failed");
		//}
	};

	self.CanCreateQuoteSheetCheck = ko.computed(function()
	{
		var canCreate = false;

		$.each(self.Model.Options(), function(optionIndex, optionItem)
		{
			$.each(optionItem.CurrentVersion().Quotes(), function(quoteIndex, quoteItem)
			{
				canCreate = /\S/g.test(quoteItem.SubscribeReference());

				return canCreate;
			});

			return canCreate;
		});

		self.CanCreateQuoteSheet(canCreate);
	}, self);
	
    self.CurrentOption = function()
    {
	    var optionTab = $("li.active a[data-toggle='tab'][data-target^='#" + domId + "-option']"),
	        optionIndex = optionTab.attr("data-target").match(/[0-9]{1,2}$/)[0];

    	return (optionIndex >= 0)
			    ? self.Model.Options()[optionIndex]
			    : self.Model.Options()[0];
    };
    
    self.Model.ActiveOptions = ko.computed(function () {
        return ko.utils.arrayFilter(self.Model.Options(), function (options) {
            return options.Id() >= 0;
        });
    }, self);

	self.InitialiseTabs = function(element) {
		//console.log('InitialiseTabs()');
		ConsoleApp.InitialiseTabs(element, domId, self);
	};

	self.InitialisePane = function(element)
	{
		ConsoleApp.InitialisePane(element, self);
	};

	if (initialiseSelf)
	{
		self.Initialise();
	}
}

function Option(optionIndex, domId, parent) {
    var self = this;
    self.TitleoptionNumber = optionIndex + 1;

	self.GetParent = function()
	{// TODO: Use knockout $parents
		return parent;
	};
	if (!!self.GetParent().Model.Options()[self.GetParent().Model.Options().length - 1])
	{
	    var prevTitleoptionNumber = Number(self.GetParent().Model.Options()[self.GetParent().Model.Options().length - 1].Title().replace("Option ", ""));
	    self.TitleoptionNumber = prevTitleoptionNumber + 1;
    }
    self.GetIndex = function()
	{
		return optionIndex;
	};

	self.Id = ko.observable(0);
	self.SubmissionId = ko.observable(0);
	self.Timestamp = ko.observable("");
	self.Title = ko.observable("Option " + self.TitleoptionNumber);
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

	self.SetVersionIndex = function(data)
	{
		//var versionNumber = ko.utils.peekObservable(element.VersionNumber()),
		//	versionCount = self.OptionVersions().length,
	    //	versionIndex = (versionCount - versionNumber) - 1;
	    var versionIndex = self.OptionVersions.indexOf(data); 

		self.VersionIndex(versionIndex);
	};

	self.AddOptionVersion = function()
	{
		var versionData = self.CurrentVersion().koTrim();

		parent.AttachValidation();

		return self.CopyOptionVersion(versionData);
	};

	self.CopyOptionVersion = function(versionData)
	{
		var length = self.OptionVersions().length,
			newVersion = new OptionVersion(length, domId, self);

		versionData.Title = newVersion.Title();
		versionData.OptionId = 0;
		versionData.VersionNumber = newVersion.VersionNumber();
		versionData.IsLocked = false;

		$.each(versionData.Quotes, function(quoteIndex, quoteItem)
		{
			quoteItem.Id = 0;
			quoteItem.OptionId = 0;
			quoteItem.VersionNumber = versionData.VersionNumber;
			quoteItem.Timestamp = "";
			quoteItem.SubmissionStatus = "SUBMITTED";

			if (quoteItem.IsSubscribeMaster === true)
			{
				if (self.CurrentVersion().Quotes()[quoteIndex].CorrelationToken() === quoteItem.CorrelationToken)
				{
					self.CurrentVersion().Quotes()[quoteIndex].IsSubscribeMaster(false);
				}
			}
		});

		newVersion.syncJSON(versionData);
	    
		length = self.OptionVersions.unshift(newVersion);

		self.VersionIndex(0);

		return length;
	};

	self.NavigateToOption = function(element, e)
	{
		$("a[data-toggle='tab'][data-target='#" + domId + "-option" + optionIndex + "']").tab("show");
	};

	self.NavigateToQuote = function(element, e)
	{
		var optionDomId = domId + "-option" + optionIndex,
			quoteIndex = parseInt($(e.target).text());

		if (isNaN(quoteIndex)) quoteIndex = 0;

		$("a[data-toggle='tab'][data-target='#" + optionDomId + "']").tab("show");
		$("#" + optionDomId + " .carousel").carousel(parseInt(quoteIndex));
	};

	self.SetMaster = function(element, e)
	{
		var optionDomId = domId + "-option" + optionIndex,
			quoteIndex = parseInt($(e.target).text());

		if (isNaN(quoteIndex))
			quoteIndex = $(e.target).parent("td").children("span:first");

		$("a[data-toggle='tab'][data-target='#" + optionDomId + "']").tab("show");
		$("#" + optionDomId + " .carousel").carousel(parseInt(quoteIndex));
	};

	self.VersionTitle = ko.computed(function()
	{
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

	self.VersionCount = ko.computed(function()
	{
		return self.OptionVersions().length;
	}, self);

	self.CurrentVersion = ko.computed(function()
	{
		return self.OptionVersions()[self.VersionIndex()];
	}, self);

	self.CurrentQuote = ko.computed(function()
	{
		var optionDomId = domId + "-option" + optionIndex,
		    quoteDomId = optionDomId + " .carousel .carousel-indicators li.active",
		    quoteIndex = $("#" + quoteDomId).index(),
		    currentQuote = (quoteIndex >= 0 && self.CurrentVersion().Quotes().length > quoteIndex)
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

    self.CanDelete = ko.computed(function () {
        var candelete = true;
        ko.utils.arrayForEach(this.OptionVersions(), function (optionVersion) {
            candelete = candelete && optionVersion.CanDeleteCheck();
        });
        candelete = candelete && (self.GetParent().Model.Options().length > 1);
        return candelete;
    }, self);
    
    self.DeleteOption = function (data,event) {
        toastr.info("Delete Option - " + self.Title());
        event.preventDefault();
        toastr.info("Delete Option - " + self.Title());
        toastr.info("Options in collection - " + self.GetParent().Model.Options().length);
        if (data.Id() === 0) {
            self.GetParent().Model.Options.remove(data);
        } else {
            ko.utils.arrayForEach(data.OptionVersions(), function (optionVersion) {
                ko.utils.arrayForEach(optionVersion.Quotes(), function (quote) {
                    quote.Id(-1 * quote.Id());
                });
                optionVersion.OptionId(-1 * optionVersion.OptionId());
            });
            
            data.Id(-1 * data.Id());
        }
        $("a[data-toggle='tab'][data-target^='#" + domId + "-option" + "']:first").tab("show");
        toastr.info("Deleted Option - " + self.Title());
        toastr.info("Options in collection - " + self.GetParent().Model.Options().length);
    };
    
    self.ActiveOptionVersions = ko.computed(function () {
        return ko.utils.arrayFilter(self.OptionVersions(), function (optionVersion) {
            return optionVersion.OptionId() >= 0;
        });
    }, self);
    
    //Todo: this is to correct the option index for copy
    self.CopyOption = function (data, event) {
        var optionIndexToCopy = -1;
        optionIndexToCopy = self.GetParent().Model.Options.indexOf(data);
        if(optionIndexToCopy>-1)
            self.GetParent().OptionIndexToCopy(optionIndexToCopy);
        self.GetParent().CopyOption();
    };

}

function OptionVersion(versionNumber, domId, parent)
{
	var self = this;

	self.TitleVersionNumber = versionNumber + 1;

	self.GetParent = function()
	{ // TODO: Use knockout $parents
		return parent;
	};

	if (!!self.GetParent().OptionVersions && !!self.GetParent().OptionVersions()[0])
	{
		self.TitleVersionNumber = self.GetParent().OptionVersions()[0].Title().replace("Version ", "") + 1;
	}

	self.GetVersionNumber = function()
	{
		return versionNumber;
	};

	self.OptionId = ko.observable(0);
	self.VersionNumber = ko.observable(self.TitleVersionNumber - 1);
	self.Timestamp = ko.observable("");
	self.Title = ko.observable("Version " + self.TitleVersionNumber);
    
	self.Comments = ko.observable("");
	self.IsExperiment = ko.observable(false); // TODO: Remove or implement ?
	self.IsLocked = ko.observable(false);

    //  Initialise additional observables defined in inheriting
	if (parent.GetParent().OVAddAdditional)
	    parent.GetParent().OVAddAdditional(self);

	self.Quotes = ko.observableArray([]);

	self.CanAddQuotes = ko.observable(versionNumber === 0);

	self.Initialise = function()
	{
		self.AddQuote();
	};

	self.AddQuote = function(quote, e, useNewQuoteWithExtraProperties)
    {
		var length; // TODO: Use conditional ternary operator
		if (useNewQuoteWithExtraProperties === true)
        {
			length = self.Quotes.push(quote);
		} else
		{
			length = self.Quotes.push(new Quote(domId, self));
		}
		
		parent.GetParent().AttachValidation();
		parent.GetParent().CanCreateQuoteSheet(false);

		return length;
	};

	self.VersionNumber.subscribe(function(value)
	{
		$.each(self.Quotes(), function(quoteIndex, quoteItem)
		{
			quoteItem.VersionNumber(value);
		});
	});

	self.CanDeleteCheck = function()
	{
	    if (this.IsLocked()) return false;

		var candelete = true;
		
		ko.utils.arrayForEach(this.Quotes(), function(quote)
		{
			candelete &= quote.CanDelete();
	    });

	    return candelete;
    };

	self.CanDelete = ko.computed(function()
	{
		return self.CanDeleteCheck() &&
			!!parent.OptionVersions &&
			parent.OptionVersions().length > 1;
	}, self);
    
	self.DeleteOptionVersion = function (data,event) {
	    toastr.info("Delete OptionVersion - " + self.Title());
	    toastr.info("OptionVersions in collection - " + self.GetParent().OptionVersions().length);
	    var newVersionIndex = 0;
	    if (data.OptionId() === 0) {
	        if (self.GetParent().VersionIndex() == self.GetParent().OptionVersions.indexOf(data)) {
	            self.GetParent().OptionVersions.remove(data);
	            self.GetParent().VersionIndex(0);
	        } else {
	            var currentVersiondata = self.GetParent().OptionVersions()[(self.GetParent().VersionIndex())];
	            var indexToDelete = self.GetParent().OptionVersions.indexOf(data);
	            self.GetParent().OptionVersions.valueWillMutate();
	            self.GetParent().OptionVersions().splice(indexToDelete, 1);
	            newVersionIndex = self.GetParent().OptionVersions.indexOf(currentVersiondata);
	            self.GetParent().VersionIndex(newVersionIndex);
	            self.GetParent().OptionVersions.valueHasMutated();
	            
	        }
	        
	    } else {
	        ko.utils.arrayForEach(data.Quotes(), function (quote) {
	            quote.Id(-1 * quote.Id());
	        });
	       
	        if (self.GetParent().VersionIndex() == self.GetParent().OptionVersions.indexOf(data)) {
	            data.OptionId(-1 * data.OptionId());
	            var activeOPtionVersions = ko.utils.arrayFilter(self.GetParent().OptionVersions(), function (optionVersion) {
	                return optionVersion.OptionId() >= 0;
	            });
	            newVersionIndex = self.GetParent().OptionVersions.indexOf(activeOPtionVersions[0]);
	            self.GetParent().VersionIndex(newVersionIndex);
	        } else {
	            data.OptionId(-1 * data.OptionId());
	        }
	    }
	    toastr.info("Delete OptionVersion - " + self.Title());
	    toastr.info("OptionVersions in collection - " + self.GetParent().OptionVersions().length);
	};

	self.ActiveQuotes = ko.computed(function () {
	    return ko.utils.arrayFilter(self.Quotes(), function (quote) {
	        return quote.Id() >= 0;
	    });
    }, self);

	self.Initialise();
}

function Quote(domId, parent)
{
	var self = this,
	    token = $.generateGuid();
	    
	self.GetParent = function()
	{// TODO: Use knockout $parents
		return parent;
	};

	// Identifiers
	self.Id = ko.observable();
	self.OptionId = ko.observable();
	self.VersionNumber = ko.observable();
	self.CorrelationToken = ko.observable();
	self.Timestamp = ko.observable();
	self.SubscribeTimestamp = ko.observable();

	// Descriptions
	self.Comment = ko.observable();
	self.Description = ko.observable();

	// Policy
	self.SubscribeReference = ko.observable();
	self.FacilityRef = ko.observable();
	self.RenPolId = ko.observable();

	self.AccountYear = ko.observable();
	self.COBId = ko.observable();
	self.DeclinatureComments = ko.observable();
	self.DeclinatureReason = ko.observable();
	self.EntryStatus = ko.observable();
	self.MOA = ko.observable();
	self.OriginatingOfficeId = ko.observable();
	self.PolicyType = ko.observable();
	self.SubmissionStatus = ko.observable();

	// Period
	self.InceptionDate = ko.observable();
	self.ExpiryDate = ko.observable();
	self.QuoteExpiryDate = ko.observable();

	// Limit
	self.LimitCCY = ko.observable();
	self.LimitAmount = ko.observable();

	// Excess
	self.ExcessCCY = ko.observable();
	self.ExcessAmount = ko.observable();

	// Pricing
	self.Currency = ko.observable();
	self.TechnicalPremium = ko.observable();
	self.BenchmarkPremium = ko.observable();
	self.TechnicalPricingBindStatus = ko.observable();
	self.TechnicalPricingMethod = ko.observable();
	self.TechnicalPricingPremiumPctgAmt = ko.observable();

	// Premium
	self.QuotedPremium = ko.observable();

	// Other
	self.IsInitialised = ko.observable();
	self.IsSubscribeMaster = ko.observable();
	self.ValidationErrors = ko.observable();

	// TODO: Remove
    // Initialise additional observables defined in inheriting
	if (parent.GetParent().GetParent().QAddAdditional)
	    parent.GetParent().GetParent().QAddAdditional(self);

	self.Functions = {
		Delete: function()
{
			var currentId = self.Id();

			if (!currentId) parent.Quotes.remove(self);
			else self.Id(-currentId);
		}, // TODO: Move out to OptionVersion
		SetSyncSlaveSubscriptions: function()
		{
			var slaveObservables = ["AccountYear",
				"COBId",
				"DeclinatureComments",
				"DeclinatureReason",
				"EntryStatus",
				"ExpiryDate",
				"FacilityRef",
				"InceptionDate",
				"IsSubscribeMaster",
				"MOA",
				"OriginatingOfficeId",
				"PolicyType",
				"SubmissionStatus"];

			for (var slaveIter in slaveObservables)
			{
				var slaveObservable = slaveObservables[slaveIter];

				if (ko.isObservable(self[slaveObservable]))
				{
					self.Functions.SetSyncSlaveSubscription(slaveObservable);
				}
			}
		},
		SetSyncSlaveSubscription: function(observableName)
		{
			self[observableName].subscribe(function(value)
			{
				self.Functions.SyncSlaveObservables(observableName, value);
			});
		},
		SetDefaults: function()
		{
			self.Id(0);
			self.OptionId(parent.OptionId());
			self.VersionNumber(parent.VersionNumber());

			self.CorrelationToken(token);
			self.EntryStatus("PARTIAL");
			self.IsSubscribeMaster(true);
			self.PolicyType("NONMARINE");
			self.SubmissionStatus("SUBMITTED");
			self.TechnicalPricingPremiumPctgAmt("%");
		},
		SetUserDefaults: function()
		{
	// TODO: Change to use a user settings view model
	var defaultQuoteExpiry = $("input[type='hidden'][name='DefaultQuoteExpiry']", self.Form).val(),
		defaultOffice = $("input[type='hidden'][name='DefaultOffice']", self.Form).val(),
		defaultPolicyType = $("input[type='hidden'][name='DefaultPolicyType']", self.Form).val();

    defaultQuoteExpiry = parseInt(defaultQuoteExpiry);

    if (!isNaN(defaultQuoteExpiry))
    {
        var quoteExpiryDate = moment().add("d", defaultQuoteExpiry);

	    if (quoteExpiryDate && quoteExpiryDate.isValid())
	    {
	    	self.QuoteExpiryDate(quoteExpiryDate.format("DD MMM YYYY"));
	    }
    }

	if (defaultPolicyType) self.PolicyType(defaultPolicyType);
			if (defaultOffice) self.OriginatingOfficeId(defaultOffice);
		},
		ShowDeclinatureDialog: function()
		{
			var vmOption = parent.GetParent(),
				optionDomId = domId + "-option" + vmOption.GetIndex(),
				quoteDomId = optionDomId + " .carousel .carousel-inner div.active",
				modalDomId = quoteDomId + " .val-declinature:first";
	
			$("#" + modalDomId).modal("show");
		}, // TODO: Refactor required ?
		SyncSlaveObservables: function(observableName, value)
	{
			if (self.IsSubscribeMaster())
			{
				var vmSubmission = parent.GetParent().GetParent();

				$.each(vmSubmission.Model.Options(), function(optionIndex, optionData)
	{
					$.each(optionData.OptionVersions(), function(versionIndex, versionData)
		{
						if (!versionData.IsLocked() || observableName === "IsSubscribeMaster")
						{
							$.each(versionData.Quotes(), function(quoteIndex, quoteData)
							{
								if (quoteData.CorrelationToken() === self.CorrelationToken() && quoteData !== self)
								{
									quoteData[observableName](value);
		}
	});
						}
	});
	});
			}
		}
	};

	self.Computeds = function(model)
    {
		model.CanDelete = ko.computed(function()
	{
			return !model.IsSubscribeMaster()
				&& !model.SubscribeReference()
				&& parent.Quotes().length > 1;
	});

		model.IsDeclinature = ko.computed(function()
	{
			return /^DECLINED$/i.test(model.SubmissionStatus());
	});

		model.IsQuoted = ko.computed(function()
	{
			return /^QUOTED$/i.test(model.SubmissionStatus());
	});

		model.IsLiveOrCancelled = ko.computed(function()
	{
			return /^LIVE|CANCELLED$/i.test(model.EntryStatus());
	});

		model.IsLocked = ko.computed(function()
	{
			return model.IsQuoted()
				|| model.IsLiveOrCancelled();
	});
	};

	self.Subscriptions = function(model)
	{
		model.FacilityRef.subscribe(function(value)
	{
			if (/(^[A-Z]{2})\S*(\d{2}$)/i.test(value))
		{
				model.COBId(RegExp.$1.toUpperCase());
		}
	});

		model.InceptionDate.subscribe(function(value)
		{
			var inceptionDate = moment(value);

			if (inceptionDate && inceptionDate.isValid())
			{
				model.AccountYear(inceptionDate.format("YYYY"));
				model.ExpiryDate(inceptionDate.add("y", 1).subtract("d", 1).format("DD MMM YYYY"));
		}
	});

		model.LimitCCY.subscribe(function(value)
	{
			if (value)
		{
				if (!model.ExcessCCY()) model.ExcessCCY(value);
				if (!model.Currency()) model.Currency(value);
		}
	});

		model.SubmissionStatus.subscribe(function(value)
	{
			if (value !== "DECLINED") // TODO: Use IsDeclinature computed ?
		{
				model.DeclinatureReason("");
				model.DeclinatureComments("");
		}

			// TODO: CanAddQuotes to computed
			if (parent.CanAddQuotes())
				parent.CanAddQuotes(value !== "QUOTED" && parent.VersionNumber() === 0);
	});
	};

	self.Initialise = function()
		{
		self.Functions.SetDefaults();
		self.Functions.SetUserDefaults();

		self.Computeds(self);
		self.Subscriptions(self);

		self.Functions.SetSyncSlaveSubscriptions();

		self.IsInitialised(true);
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

vmSubmission.prototype.syncJSON = function(submissionData)
{
	var self = this,
	    vmSubmission = this.Model;

	self.Id = submissionData.Id;
    vmSubmission.AuditTrails.length = 0;
	vmSubmission.Id(submissionData.Id);
	vmSubmission.Timestamp(submissionData.Timestamp);
	vmSubmission.Title(submissionData.Title);

    //  Call the additional sync tasks defined in inherited.
	if (self.SSyncJSONAdditional)
		self.SSyncJSONAdditional(vmSubmission, submissionData);

	vmSubmission.InsuredId(submissionData.InsuredId);
	vmSubmission.BrokerSequenceId(submissionData.BrokerSequenceId);
	vmSubmission.BrokerContact(submissionData.BrokerContact);
	vmSubmission.NonLondonBrokerCode(submissionData.NonLondonBrokerCode);
	vmSubmission.UnderwriterCode(submissionData.UnderwriterCode);
	vmSubmission.UnderwriterContactCode(submissionData.UnderwriterContactCode);
	vmSubmission.QuotingOfficeId(submissionData.QuotingOfficeId);
	vmSubmission.LeaderNo(submissionData.LeaderNo);
	vmSubmission.Leader(submissionData.Leader);
	vmSubmission.Domicile(submissionData.Domicile);
	vmSubmission.Brokerage(submissionData.Brokerage);
	vmSubmission.UnderwriterNotes(submissionData.UnderwriterNotes);
	
	if (submissionData.AdditionalInsuredList) {
		vmSubmission.AdditionalInsuredList.removeAll();
		$.each(submissionData.AdditionalInsuredList, function(index, item)
		{
			vmSubmission.AdditionalInsuredList.push(
			{
			    Id: ko.observable(item.Id),
				InsuredId: ko.observable(item.InsuredId),
				InsuredName: ko.observable(item.InsuredName),
				InsuredType: ko.observable(item.InsuredType),
				InsuredTypes: ["Additional", "Cedent", "Interested Party", "Reinsured"]
			});
		});
	}

	if (vmSubmission.Options().length !== submissionData.Options.length)
	{
    	while (vmSubmission.Options().length !== 0)
    	{// TODO: Don't use while loops
			vmSubmission.Options.pop();
		}
	}

	$.each(submissionData.Options, function(optionIndex, optionData)
	{
		var vmOption = vmSubmission.Options()[optionIndex];

		if (!vmOption)
		{
			var optionsLength = self.AddOption();

			vmOption = vmSubmission.Options()[optionsLength - 1];
		}

		vmSubmission.Options()[optionIndex] = vmOption.syncJSON(optionData);
	});

	vmSubmission.SubmissionMarketWordingsList.removeAll();
	
    var tempSubmissionMarketWordingsList = [];

    $.each(submissionData.MarketWordingSettings, function(index, value)
    {
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
	$.each(submissionData.TermsNConditionWordingSettings, function(index, value)
	{
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
	$.each(submissionData.CustomMarketWordingSettings, function(index, value)
	{
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
	$.each(submissionData.CustomTermsNConditionWordingSettings, function(index, value)
	{
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
	$.each(submissionData.CustomSubjectToClauseWordingSettings, function(index, value)
	{
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

Option.prototype.syncJSON = function(optionData)
{
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

	while (vmOption.OptionVersions().length > optionData.OptionVersions.length)
	{// TODO: Don't use while loops
		vmOption.OptionVersions().pop();
	}

    while (vmOption.OptionVersions().length < optionData.OptionVersions.length)
    {// TODO: Don't use while loops
		vmOption.AddOptionVersion();
	}

	optionData.OptionVersions.sort(function(versionA, versionB)
	{
		return (versionA.VersionNumber < versionB.VersionNumber)
			? 1 : (versionA.VersionNumber > versionB.VersionNumber)
				? -1 : 0;
	});

	$.each(optionData.OptionVersions, function(versionIndex, versionData)
	{
		var vmVersion = vmOption.OptionVersions()[versionIndex];

		if (!vmVersion)
			vmOption.CopyOptionVersion(versionData);
		else
			vmOption.OptionVersions()[versionIndex] = vmVersion.syncJSON(versionData);
	});

	return vmOption;
};

OptionVersion.prototype.syncJSON = function(versionData)
{
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
    
	while (vmVersion.Quotes().length > versionData.Quotes.length)
	{ // TODO: Don't use while loops
		vmVersion.Quotes.pop();
	}

	$.each(versionData.Quotes, function(quoteIndex, quoteData)
	{
		var vmQuote = vmVersion.Quotes()[quoteIndex];

		if (!vmQuote)
		{
			var quotesLength = vmVersion.AddQuote();

			vmQuote = vmVersion.Quotes()[quotesLength - 1];
		}

		vmVersion.Quotes()[quoteIndex] = vmQuote.syncJSON(quoteData);
	});

	return vmVersion;
};

Quote.prototype.syncJSON = function(quoteData)
{
	var vmQuote = this;

	// TODO: Remove date specifics after implementing a datepicker binding handler
	var inceptionDate = moment(quoteData.InceptionDate),
	    expiryDate = moment(quoteData.ExpiryDate),
	    quoteExpiryDate = moment(quoteData.QuoteExpiryDate);

	vmQuote.InceptionDate(inceptionDate.isValid() ? inceptionDate.format("DD MMM YYYY") : "");
	vmQuote.ExpiryDate(expiryDate.isValid() ? expiryDate.format("DD MMM YYYY") : "");
	vmQuote.QuoteExpiryDate(quoteExpiryDate.isValid() ? quoteExpiryDate.format("DD MMM YYYY") : "");

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
	vmQuote.FacilityRef(quoteData.FacilityRef || "");
	vmQuote.RenPolId(quoteData.RenPolId || "");

	vmQuote.AccountYear(quoteData.AccountYear);
	vmQuote.COBId(quoteData.COBId);
	vmQuote.MOA(quoteData.MOA);
	vmQuote.OriginatingOfficeId(quoteData.OriginatingOfficeId);
	vmQuote.EntryStatus(quoteData.EntryStatus);
	vmQuote.PolicyType(quoteData.PolicyType);

	vmQuote.LimitCCY(quoteData.LimitCCY);
	vmQuote.LimitAmount(quoteData.LimitAmount);

	vmQuote.ExcessCCY(quoteData.ExcessCCY);
	vmQuote.ExcessAmount(quoteData.ExcessAmount);

	vmQuote.Currency(quoteData.Currency);
	vmQuote.TechnicalPricingBindStatus(quoteData.TechnicalPricingBindStatus);
	vmQuote.TechnicalPricingPremiumPctgAmt(quoteData.TechnicalPricingPremiumPctgAmt || "%");
	vmQuote.TechnicalPricingMethod(quoteData.TechnicalPricingMethod || "MODEL");

	vmQuote.TechnicalPremium(quoteData.TechnicalPremium);
	vmQuote.BenchmarkPremium(quoteData.BenchmarkPremium);
	vmQuote.QuotedPremium(quoteData.QuotedPremium);

	vmQuote.Comment(quoteData.Comment);
	vmQuote.Description(quoteData.Description);

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
vmSubmission.prototype.toJSON = function(dirtyFlag)
{
	var vmSubmission = this.koTrim(dirtyFlag);

	return ko.toJSON(vmSubmission, dirtyFlag);
};

vmSubmission.prototype.koTrim = function(dirtyFlag)
{
	var vmSubmission = ko.toJS(this.Model);

	delete vmSubmission.Functions;
	delete vmSubmission.Subscriptions;

	// Front-End
	if (dirtyFlag)
	{
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

    delete vmSubmission.ActiveOptions;

	$.each(vmSubmission.Options, function(optionIndex, optionItem)
	{
		vmSubmission.Options[optionIndex] = optionItem.koTrim(optionItem);
	});

	return vmSubmission;
};

Option.prototype.toJSON = function()
{
	var vmOption = this.koTrim();

	return ko.toJSON(vmOption);
};

Option.prototype.koTrim = function(thisOption, dirtyFlag)
{
	var vmOption = (!thisOption) ? ko.toJS(this) : thisOption;

	delete vmOption.Functions;
	delete vmOption.Subscriptions;

    // Garbage
	delete vmOption.optionNumber;
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
    
	delete vmOption.CanDelete;
	delete vmOption.DeleteOption;
	delete vmOption.ActiveOptionVersions;
    delete vmOption.CopyOption;

	delete vmOption.NavigateToOption;
	delete vmOption.NavigateToQuote;

	delete vmOption.SetMaster;

	delete vmOption.koTrim;
	delete vmOption.syncJSON;
	delete vmOption.toJSON;

	$.each(vmOption.OptionVersions, function(versionIndex, versionItem)
	{
		vmOption.OptionVersions[versionIndex] = versionItem.koTrim(versionItem, dirtyFlag);
	});

	return vmOption;
};

OptionVersion.prototype.toJSON = function()
{
	var vmVersion = this.koTrim();

	return ko.toJSON(vmVersion);
};

OptionVersion.prototype.koTrim = function(thisVersion, dirtyFlag)
{
	var vmVersion = (!thisVersion) ? ko.toJS(this) : thisVersion;

	delete vmVersion.Functions;
	delete vmVersion.Subscriptions;

	// Garbage
	delete vmVersion.GetParent;
	delete vmVersion.GetVersionNumber;

	delete vmVersion.AddQuote;
	delete vmVersion.CanAddQuotes;
	delete vmVersion.Initialise;

	delete vmVersion.koTrim;
	delete vmVersion.syncJSON;
	delete vmVersion.toJSON;
    
	delete vmVersion.CanDeleteCheck;
	delete vmVersion.CanDelete;
	delete vmVersion.DeleteOptionVersion;
	delete vmVersion.ActiveQuotes;

	$.each(vmVersion.Quotes, function(quoteIndex, quoteItem)
	{
		vmVersion.Quotes[quoteIndex] = quoteItem.koTrim(quoteItem, dirtyFlag);
	});

	return vmVersion;
};

Quote.prototype.toJSON = function()
{
	var vmQuote = this.koTrim();

	return ko.toJSON(vmQuote);
};

Quote.prototype.koTrim = function(thisQuote, dirtyFlag)
{
	var vmQuote = thisQuote || ko.toJS(this);

	delete vmQuote.Functions;
	delete vmQuote.GetParent;
	delete vmQuote.Initialise;
	delete vmQuote.Subscriptions;
	delete vmQuote.ValidationErrors;

	delete vmQuote.koTrim;
	delete vmQuote.syncJSON;
	delete vmQuote.toJSON;

	return vmQuote;
};

// TODO: ko.dirtyFlag suggests it is generic, however, see Resynchronise()
// TODO: Is KOLite dirty-flag better ?
ko.dirtyFlag = function(koObject)
{
	var self = this;
	self.GetObjectState = function () {
	    return ko.toJSON(koObject.koTrim(), function (key, value) {
	        if (   key == "InsuredName"
	            || key == "AuditTrails"
	            || key == "BrokerCode"
	            || key == "BrokerPseudonym"
	            || key == "BrokerGroupCode"
	            || key == "NonLondonBrokerName"
	            || key == "_Domicile"
	            || key == "._Leader"
	            || key == "._NonLondonBroker"
	            || key == "._Underwriter"
	            || key == "._UnderwriterContact"
	            || key == ".Broker"
	            || key == ".QuotingOffice"
	            || key == "InsuredTypes"
	        
                || key == "DeclinatureReason" //Todo: this has to be investigated
                || key == "_COB"
	            || key ==  "_Currency"
	            || key ==  "_ExcessCCY"
	            || key ==  "_LimitCCY"
	            || key ==  "_MOA"
	            || key ==  "_OriginatingOffice"
                ) {
	            return null;
	        } else {
	            return value;
	        }
	    });
	};
	self.OriginalState = ko.observable(self.GetObjectState());
	//self.OriginalState = ko.observable(koObject.toJSON());
	self.IsDirty = ko.observable(false);
	self.RequiresReset = ko.observable(false);

	self.Evaluate = ko.computed(function()
	{
		if (self.RequiresReset()) {
		    self.OriginalState(self.GetObjectState());
		//	self.OriginalState(koObject.toJSON());
			self.IsDirty(false);
			self.RequiresReset(false);
		}
		else if (!self.IsDirty())
		{
		    self.IsDirty(self.OriginalState() !== self.GetObjectState());
			//self.IsDirty(self.OriginalState() !== koObject.toJSON());
		}
	}, self);

	self.Reset = function()
	{
		self.RequiresReset(true);
	};

	// TODO: Remove/re-think may be required as syncJSON is only used by Submission -> Quote VM's
	self.Resynchronise = function()
	{
		// TODO: Perhaps we could implement a something generic using hasOwnProperty() ?
		koObject.syncJSON(JSON.parse(self.OriginalState()));

		self.Reset();
	};
};