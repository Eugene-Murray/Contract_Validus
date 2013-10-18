$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

	ConsoleApp.vmSubmissionHM = function(id, domId, initilizeSelf, isReadOnly)
	{
		var self = new vmSubmissionBase(id, domId, false, isReadOnly);

		self.Model.submissionType = ko.observable("HM Submission");
		self.Model.submissionTypeId = ko.observable("HM");

		self.OVAddAdditional = function(vmOV)
		{
			vmOV.TSICurrency = ko.observable("");
			vmOV._TSICurrency = ko.observable("");
			vmOV._TSICurrency.subscribe(function(value)
			{
				var currencyValues = (value) ? value.split(":") : [],
                    currencyPsu = (currencyValues[0]) ? $.trim(currencyValues[0]) : "";

				vmOV.TSICurrency(currencyPsu);
			});

			vmOV.TSIAmount = ko.observable("");
		};

		self.OVSyncJSONAdditional = function(vmOV, jsonOV)
		{
			vmOV.TSICurrency(jsonOV.TSICurrency);
			if ((jsonOV._TSICurrency != undefined) && (jsonOV._TSICurrency != null))
				vmOV._TSICurrency(jsonOV._TSICurrency);
			else if (jsonOV.TSICurrency)
			{
				$.getJSON(window.ValidusServicesUrl + "Currency", { psu: jsonOV.TSICurrency }, function(jsonData)
				{
					$(jsonData).each(function(index, item)
					{
						vmOV._TSICurrency(item.Psu + " : " + item.Name);

						return false;
					});
				});
			}

			vmOV.TSIAmount(jsonOV.TSIAmount);
		};

		self.QAddAdditional = function(vmQuote)
		{
			vmQuote.AmountOrOPL = ko.observable("");
			vmQuote.AmountOrONP = ko.observable("");
			vmQuote.VesselTopLimitCurrency = ko.observable("");
			vmQuote._VesselTopLimitCurrency = ko.observable("");
			vmQuote.VesselTopLimitAmount = ko.observable("");

			vmQuote.AmountOrOPL.subscribe(function(value)
			{
				if (value === "OPL")
				{
					vmQuote._LimitCCY("");
					vmQuote.LimitAmount("");
				}
			});

			vmQuote._VesselTopLimitCurrency.subscribe(function(value)
			{
				var currencyValues = (value) ? value.split(":") : [],
                    currencyPsu = (currencyValues[0]) ? $.trim(currencyValues[0]) : "";

				vmQuote.VesselTopLimitCurrency(currencyPsu);
			});
		};

		self.QSyncJSONAdditional = function(vmQuote, quoteJSON)
		{
			vmQuote.AmountOrOPL(quoteJSON.AmountOrOPL);
			vmQuote.AmountOrONP(quoteJSON.AmountOrONP);

			vmQuote.VesselTopLimitCurrency(quoteJSON.VesselTopLimitCurrency);
			if ((quoteJSON._VesselTopLimitCurrency != undefined) && (quoteJSON._VesselTopLimitCurrency != null))
				vmQuote._VesselTopLimitCurrency(quoteJSON._VesselTopLimitCurrency);
			else if (quoteJSON.VesselTopLimitCurrency)
			{
				$.getJSON(window.ValidusServicesUrl + "Currency", { psu: quoteJSON.VesselTopLimitCurrency }, function(jsonData)
				{
					$(jsonData).each(function(index, item)
					{
						vmQuote._VesselTopLimitCurrency(item.Psu + " : " + item.Name);

						return false;
					});
				});
			}


			vmQuote.VesselTopLimitAmount(quoteJSON.VesselTopLimitAmount);
		};

		self.CreateFirstOption(self); // TODO: Move to the base
		self.Initialise(self);

		return self;
	};
});