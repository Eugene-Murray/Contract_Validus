$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	ConsoleApp.vmSubmissionFI = function(id, domId, initialiseSelf, isReadOnly)
	{
		var self = new vmSubmissionBase(id, domId, false, isReadOnly);

		self.QAddAdditional = function(a)
		{
			a.COB = ko.observable("CF");
			a.COBId = ko.observable("CF");
			a._COB = ko.observable("CF : Direct - Casualty - Financial Institutions");

			a.LineSize = ko.observable("");
			a.LineToStand = ko.observable(true);
			a.RiskCodeId = ko.observable("");
			a.IsReinstatement = ko.observable(false);
			a.AmountOrOPL = ko.observable("Amt");
			a.AmountOrONP = ko.observable("%");
		};

		self.QSyncJSONAdditional = function(a, b)
		{
			a.LineSize(b.LineSize);
			a.LineToStand(b.LineToStand);
			a.RiskCodeId(b.RiskCodeId);
			a.IsReinstatement(b.IsReinstatement);

			a.AmountOrOPL(b.AmountOrOPL);
			a.AmountOrONP(b.AmountOrONP);
		};

		self.OVAddAdditional = function(ov)
		{
			ov.AreQuotesExpanded = ko.observable(false);

			ov.ExpandFIQuotes = function()
			{
				if (window.confirm("The current quote will be replicated for each risk code, resulting in a separate copy of the current quote for each risk code. Continue?"))
				{
					var currentQuote = ov.GetParent().CurrentQuote();

					if (currentQuote.COBId() != "CF")
					{
						alert("Current quote is not CF.");
						return;
					}
					else
					{
						var currentSet = false,
						    riskCodesToDo = [];

						for (var i = 0; i < ov.GetParent().RiskCodesArray.length; i++)
						{
							if (ov.GetParent().RiskCodesArray[i].Selected() === true)
							{
								riskCodesToDo.push(ov.GetParent().RiskCodesArray[i].Code);
							}
						}

						if (riskCodesToDo.length == 0)
						{
							alert("No risk codes entered.");
							return;
						}

						//  If this current quote risk code is one of the expansion risk codes, remove it from the todo list
						var existingIndex = jQuery.inArray(currentQuote.RiskCodeId(), riskCodesToDo);
						
						if (existingIndex > -1)
						{
							currentSet = true;
							riskCodesToDo.splice(existingIndex, 1);
						}

						for (var i = 0; i < riskCodesToDo.length; i++)
						{
							if (!currentSet)
							{
								currentQuote.RiskCodeId(riskCodesToDo[i]);
								currentSet = true;
								continue;
							}

							var l = ov.AddQuote(null, null, false) - 1,
							    addedQuote = ov.Quotes()[l];

							//  Copy all observable properties from current to target
							for (var property in currentQuote)
							{
								if (!addedQuote[property]) continue;

								if (ko.isObservableArray(addedQuote[property]))
								{
									//for (var sourceIndex in source[property])
									//{
									//    self.syncJSON(source[property][sourceIndex], target[property]);
									//}
								}
								else if (ko.isWriteableObservable(addedQuote[property]))
								{
									addedQuote[property](currentQuote[property]());
								}
								//else addedQuote[property](currentQuote[property]());
							}

							addedQuote.Id(0);
							addedQuote.Timestamp(null);
							addedQuote.SubscribeReference("");
							addedQuote.SubscribeTimestamp("");
							addedQuote.CorrelationToken($.generateGuid());
							addedQuote.RiskCodeId(riskCodesToDo[i]);
						}

						ov.AreQuotesExpanded(true);
						ov.GetParent().GetParent().AttachValidation();
						//parent.GetParent().CanCreateQuoteSheet(false);	
					}
				}
			};
		};

		self.OAddAdditional = function(option)
		{
			option.RiskCodes = ko.observable("");
			option.RiskCodesArray = [];

			$.when(
				$.ajax({
					type: 'GET',
					url: '/riskcode/GetBySubmissionTypeId?submissionTypeId=FI',
					dataType: 'json',
					async: false
				})
			).done(
				function(data3)
				{
					self.RiskCodeOptionsList = data3;

					var copy = data3.slice(0),
						riskCodes = option.RiskCodes(),
					    riskCodesArray = riskCodes.split(",");
					
					copy.unshift({ Code: "", Name: "" });
					self.RiskCodeOptionsDropDownList = copy;
					
					for (var i = 0; i < self.RiskCodeOptionsList.length; i++)
					{
						var currentCode = self.RiskCodeOptionsList[i].Code,
						    isChecked = jQuery.inArray(currentCode, riskCodesArray) > -1;
						
						option.RiskCodesArray.push({
							Code: currentCode,
							Name: self.RiskCodeOptionsList[i].Name,
							Selected: ko.observable(isChecked)
						});
					}

					//  TODO: use join?
					option.RiskCodesButtonText = ko.computed(function()
					{
						var joined = "",
						    isFirst = true;
						
						for (var i = 0; i < option.RiskCodesArray.length; i++)
						{
							if (option.RiskCodesArray[i].Selected() === true)
							{
								joined += (isFirst === false ? "," : "") + option.RiskCodesArray[i].Code;
								isFirst = false;
							}
						}
						
						option.RiskCodes(joined);
						
						return "Risk Codes: " + joined;
					});
				}
			);

			option.OpenRiskCodesDialog = function()
			{
				$(".val-riskcodes-modal", self.Form).modal("show");
			};
		};

		self.OSyncJSONAdditional = function(a, b)
		{
			a.RiskCodes(b.RiskCodes);
			var riskCodes = b.RiskCodes,
			    riskCodesArray = [];
			if (riskCodes !== undefined && riskCodes !== null)
			{
				riskCodesArray = riskCodes.split(",");
			}

			for (var i = 0; i < self.RiskCodeOptionsList.length; i++)
			{
				var currentCode = self.RiskCodeOptionsList[i].Code
				a.RiskCodesArray[i].Selected(jQuery.inArray(currentCode, riskCodesArray) > -1);
				//a.RiskCodesArray.push({
				//    Code: currentCode,
				//    Name: self.RiskCodeOptionsList[i].Name,
				//    Selected: ko.observable(jQuery.inArray(currentCode, riskCodesArray))
				//});
			}
		};

		self.Model.submissionType = ko.observable('FI Submission');
		self.Model.submissionTypeId = ko.observable('FI');

		//self.RiskCodeOptionsList = [{ Code: "BB", Name: "Fidelity, comp crime and bankers' policies" },
		//    { Code: "DO", Name: "Directors and officers liability" }, { Code: "PI", Name: "Errors and omissions/professional indemnity" }];

		//self.RiskCodeOptionsList = [];
		//self.RiskCodeOptionsDropDownList = [];

		//  TODO: Why isn't this in the base?
		self.CreateFirstOption(self);

		$.when(
			$.ajax({
				type: 'GET',
				url: window.ValidusServicesUrl + 'interest',
				dataType: 'json'
			}), $.ajax({
				type: 'GET',
				url: window.ValidusServicesUrl + 'currency',
				dataType: 'json'
			})
		).done(
			function(data, data2, data3)
			{
				data[0].unshift({ Code: "", Description: "" });
				self.InterestsList = data[0];

				data2[0].unshift({ Psu: "", Name: "" });
				self.CurrenciesList = data2[0];

				self.Initialise(self);
			}
		);

		return self;
	};
});