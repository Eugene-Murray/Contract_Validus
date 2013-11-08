$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

	ConsoleApp.vmSubmissionHM = function(id, domId, initilizeSelf, isReadOnly)
	{
		var self = new vmSubmission(id, domId, false, isReadOnly);

		self.Model.submissionType = ko.observable("HM Submission");
		self.Model.submissionTypeId = ko.observable("HM");

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
			vmQuote.AmountOrOPL = ko.observable();
			vmQuote.AmountOrONP = ko.observable();
			vmQuote.VesselTopLimitCurrency = ko.observable();
			vmQuote.VesselTopLimitAmount = ko.observable();

			vmQuote.RenewalRate = ko.observable();
			vmQuote.RenewalConditions = ko.observable();
			vmQuote.RenewalDeductibles = ko.observable();
			vmQuote.RenewalExposure = ko.observable();
			vmQuote.RenewalBase = ko.observable();
			vmQuote.RenewalFull = ko.observable();

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
			vmQuote.VesselTopLimitCurrency(quoteJSON.VesselTopLimitCurrency);
			vmQuote.VesselTopLimitAmount(quoteJSON.VesselTopLimitAmount);
		    
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