$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	ConsoleApp.vmSubmissionME = function(id, domId, initilizeSelf, isReadOnly)
	{
		var self = new vmSubmission(id, domId, false, isReadOnly);

		self.Model.submissionType = ko.observable("ME Submission");
		self.Model.submissionTypeId = ko.observable("ME");

		self.QAddAdditional = function(vmQuote)
		{
			vmQuote.AmountOrOPL = ko.observable("");
			vmQuote.AmountOrONP = ko.observable("");
		    vmQuote.LineSize = ko.observable("").extend({ numeric: 9 });
			
			vmQuote.AmountOrOPL.subscribe(function(value)
			{
				if (value === "OPL")
				{
					vmQuote._LimitCCY("");
					vmQuote.LimitAmount("");
				}
			});
			
			vmQuote.RenewalRate = ko.observable();
			vmQuote.RenewalConditions = ko.observable();
			vmQuote.RenewalDeductibles = ko.observable();
			vmQuote.RenewalExposure = ko.observable();
			vmQuote.RenewalBase = ko.observable();
			vmQuote.RenewalFull = ko.observable();
		};
		
		self.QSyncJSONAdditional = function(vmQuote, quoteJSON)
		{
			vmQuote.AmountOrOPL(quoteJSON.AmountOrOPL);
			vmQuote.AmountOrONP(quoteJSON.AmountOrONP);
			vmQuote.LineSize(quoteJSON.LineSize);
			
			vmQuote.RenewalRate(quoteJSON.RenewalRate);
			vmQuote.RenewalConditions(quoteJSON.RenewalConditions);
			vmQuote.RenewalDeductibles(quoteJSON.RenewalDeductibles);
			vmQuote.RenewalExposure(quoteJSON.RenewalExposure);
			vmQuote.RenewalBase(quoteJSON.RenewalBase);
			vmQuote.RenewalFull(quoteJSON.RenewalFull);
		};

		self.CreateFirstOption(self); // TODO: Move to the base
		self.Initialise(self);

		return self;
	};
});