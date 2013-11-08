$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	ConsoleApp.vmSubmissionCA = function(id, domId, initialiseSelf, isReadOnly)
	{
		var self = new vmSubmission(id, domId, false, isReadOnly);

		self.Model.submissionType = ko.observable("CA Submission");
		self.Model.submissionTypeId = ko.observable("CA");

		self.OVAddAdditional = function(vmOV)
		{
			vmOV.TSICurrency = ko.observable();
			vmOV.TSIAmount = ko.observable();
		};

		self.OVSyncJSONAdditional = function(vmOV, jsonOV)
		{
			vmOV.TSICurrency(jsonOV.TSICurrency);
			vmOV.TSIAmount(jsonOV.TSIAmount);
		};

		self.QAddAdditional = function(vmQuote)
		{
			vmQuote.AmountOrOPL = ko.observable("");
			vmQuote.AmountOrONP = ko.observable("");
		    vmQuote.LineSize = ko.observable("").extend({ numeric: 9 });
			
			vmQuote.AmountOrOPL.subscribe(function(value)
			{
				if (value === "OPL")
				{
					vmQuote.LimitCCY("");
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