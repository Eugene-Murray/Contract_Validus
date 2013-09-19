$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	ConsoleApp.vmSubmissionEN = function(id, domId)
	{
		var self = new vmSubmissionBase(id, domId, false);

		self.Model.submissionType = ko.observable("EN Submission");
		self.Model.submissionTypeId = ko.observable("EN");
		
		self.QAddAdditional = function(vmQuote)
		{
			vmQuote.AmountOrOPL = ko.observable("");
			vmQuote.AmountOrONP = ko.observable("");
			vmQuote.QuoteComments = ko.observable("");

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
			vmQuote.QuoteComments(quoteJSON.QuoteComments);
		};

		self.CreateFirstOption(self); // TODO: Move to the base
		self.Initialise(self);

		return self;
	};
});