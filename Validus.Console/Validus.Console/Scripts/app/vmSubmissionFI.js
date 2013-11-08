$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	ConsoleApp.vmSubmissionFI = function(id, domId, initialiseSelf, isReadOnly)
	{
		var self = new vmSubmission(id, domId, false, isReadOnly);

		self.Model.submissionType = ko.observable("FI Submission");
		self.Model.submissionTypeId = ko.observable("FI");

		self.QAddAdditional = function(vmQ)
		{
			vmQ.COBId = ko.observable("CF");
			vmQ.LineSizePctgAmt = ko.observable("%");
			vmQ.LineSize = ko.observable();
			vmQ.LineToStand = ko.observable(true);
			vmQ.RiskCodeId = ko.observable();
			vmQ.IsReinstatement = ko.observable(false);

			vmQ.RenewalRate = ko.observable();
			vmQ.RenewalConditions = ko.observable();
			vmQ.RenewalDeductibles = ko.observable();
			vmQ.RenewalExposure = ko.observable();
			vmQ.RenewalBase = ko.observable();
			vmQ.RenewalFull = ko.observable();
		};

		self.QSyncJSONAdditional = function(vmQ, jsonQ)
		{
			vmQ.LineSizePctgAmt(jsonQ.LineSizePctgAmt);
			vmQ.LineSize(jsonQ.LineSize);
			vmQ.LineToStand(jsonQ.LineToStand);
			vmQ.RiskCodeId(jsonQ.RiskCodeId);
			vmQ.IsReinstatement(jsonQ.IsReinstatement);

			vmQ.RenewalRate(jsonQ.RenewalRate);
			vmQ.RenewalConditions(jsonQ.RenewalConditions);
			vmQ.RenewalDeductibles(jsonQ.RenewalDeductibles);
			vmQ.RenewalExposure(jsonQ.RenewalExposure);
			vmQ.RenewalBase(jsonQ.RenewalBase);
			vmQ.RenewalFull(jsonQ.RenewalFull);
		};

		self.OVAddAdditional = function(vmOV)
		{
			vmOV.AreQuotesExpanded = ko.observable(false);

			vmOV.ExpandFIQuotes = function()
			{
				if (window.confirm("The current quote will be replicated for each risk code, resulting in a separate copy of the current quote for each risk code. Continue?"))
				{
					var currentQuote = vmOV.GetParent().CurrentQuote();

					if (currentQuote.COBId() !== "CF")
					{
						toastr.warning("Current quote is not CF.");
						return;
					}
					else
					{
						var currentSet = false,
						    riskCodesToDo = [];

						for (var i = 0; i < vmOV.GetParent().RiskCodesArray.length; i++)
						{
							if (vmOV.GetParent().RiskCodesArray[i].Selected() === true)
							{
								riskCodesToDo.push(vmOV.GetParent().RiskCodesArray[i].Code);
							}
						}

						if (riskCodesToDo.length == 0)
						{
							toastr.error("No risk codes entered.");
							return;
						}

						//  If this current quote risk code is one of the expansion risk codes, remove it from the todo list
						var existingIndex = $.inArray(currentQuote.RiskCodeId(), riskCodesToDo);
						
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

							var l = vmOV.AddQuote(null, null, false) - 1,
							    addedQuote = vmOV.Quotes()[l];

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

						vmOV.AreQuotesExpanded(true);
						vmOV.GetParent().GetParent().AttachValidation();
						//parent.GetParent().CanCreateQuoteSheet(false);	
					}
				}
			};
		};

		self.OAddAdditional = function(vmO)
		{
			vmO.RiskCodes = ko.observable("");
			vmO.RiskCodesArray = [];

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
						riskCodes = vmO.RiskCodes(),
					    riskCodesArray = riskCodes.split(",");
					
					copy.unshift({ Code: null, Name: null });
					self.RiskCodeOptionsDropDownList = copy;
					
					for (var i = 0; i < self.RiskCodeOptionsList.length; i++)
					{
						var currentCode = self.RiskCodeOptionsList[i].Code,
						    isChecked = $.inArray(currentCode, riskCodesArray) > -1;
						
						vmO.RiskCodesArray.push({
							Code: currentCode,
							Name: self.RiskCodeOptionsList[i].Name,
							Selected: ko.observable(isChecked)
						});
					}

					//  TODO: use join?
					vmO.RiskCodesButtonText = ko.computed(function()
					{
						var joined = "",
						    isFirst = true;
						
						for (var i = 0; i < vmO.RiskCodesArray.length; i++)
						{
							if (vmO.RiskCodesArray[i].Selected() === true)
							{
								joined += (isFirst === false ? "," : "") + vmO.RiskCodesArray[i].Code;
								isFirst = false;
							}
						}
						
						vmO.RiskCodes(joined);
						
						return "Risk Codes: " + joined;
					});
				}
			);

			vmO.OpenRiskCodesDialog = function()
			{
				$(".val-riskcodes-modal", self.Form).modal("show");
			};
		};

		self.OSyncJSONAdditional = function(vmO, jsonO)
		{
			vmO.RiskCodes(jsonO.RiskCodes);
			
			var riskCodes = jsonO.RiskCodes,
			    riskCodesArray = [];
			
			if (riskCodes !== undefined && riskCodes !== null)
			{
				riskCodesArray = riskCodes.split(",");
			}

			for (var i = 0; i < self.RiskCodeOptionsList.length; i++)
			{
				var currentCode = self.RiskCodeOptionsList[i].Code;
				
				vmO.RiskCodesArray[i].Selected($.inArray(currentCode, riskCodesArray) > -1);
			}
		};

		//  TODO: Why isn't this in the base?
		self.CreateFirstOption(self);
				self.Initialise(self);

		return self;
	};
});