$(function()
{
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

	ConsoleApp.vmSubmissionCA = function(id, domId, initialiseSelf, isReadOnly)
	{
		var self = new vmSubmissionBase(id, domId, false, isReadOnly);

		self.Model.submissionType = ko.observable("CA Submission");
		self.Model.submissionTypeId = ko.observable("CA");

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
			
			if (jsonOV._TSICurrency != undefined && jsonOV._TSICurrency != null)
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