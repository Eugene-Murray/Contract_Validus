$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	ConsoleApp.vmSubmissionME = function(id, domId, initilizeSelf, isReadOnly)
	{
		var self = new vmSubmissionBase(id, domId, false, isReadOnly);

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
		};
		
		self.QSyncJSONAdditional = function(vmQuote, quoteJSON)
		{
			vmQuote.AmountOrOPL(quoteJSON.AmountOrOPL);
			vmQuote.AmountOrONP(quoteJSON.AmountOrONP);
			vmQuote.LineSize(quoteJSON.LineSize);
		};

		self.CreateFirstOption(self); // TODO: Move to the base
		self.Initialise(self);

		return self;
	};
});