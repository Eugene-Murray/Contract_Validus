$(function()
{
    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};
     
	ConsoleApp.vmSubmissionPV = function(id, domId, initialiseSelf, isReadOnly)
	{
    	var self = new vmSubmissionBase(id, domId, false, isReadOnly);

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

			vmOV.UpdateTSITotal = function()
			{
				var tsiBi = parseFloat(vmOV.TSIBI()), // KO dependency for TSIBI needed here
				    tsiPd = parseFloat(vmOV.TSIPD()), // KO dependency for TSIPD needed here
				    tsiTotal = ko.utils.peekObservable(vmOV.TSITotal);

				if (tsiTotal === "")
				{
					tsiTotal = !isNaN(tsiBi) && !isNaN(tsiPd) ? Math.round(tsiBi + tsiPd).toString() : "";
				}

				return vmOV.TSITotal(tsiTotal ? tsiTotal.replace(/[^\d]/g, "") : "");
			};

			vmOV.TSIPD = ko.observable("");
			vmOV.TSIBI = ko.observable("");
			vmOV.TSITotal = ko.observable("");

			vmOV.TSIPD.subscribe(vmOV.UpdateTSITotal);
			vmOV.TSIBI.subscribe(vmOV.UpdateTSITotal);
		};

		self.QAddAdditional = function(vmQ)
		{
			vmQ.PDPctgAmt = ko.observable("");
			vmQ.BIPctgAmtDays = ko.observable("");

			vmQ.PDExcessAmount = ko.observable("");
			vmQ.BIExcessAmount = ko.observable("");

			vmQ.LineSize = ko.observable("");
			vmQ.LineToStand = ko.observable(true);

			vmQ.UpdateExcess = function()
			{
				var pdAmount = parseFloat(vmQ.PDExcessAmount()),
					biAmount = parseFloat(vmQ.BIExcessAmount()),
					pdType = vmQ.PDPctgAmt(),
					biType = vmQ.BIPctgAmtDays(),
					value = pdType === "Amt" && pdAmount && !isNaN(pdAmount)
						? biType === "Amt" && biAmount && !isNaN(biAmount)
							? Math.round(pdAmount + biAmount)
							: Math.round(pdAmount)
						: "";

				vmQ.ExcessAmount(value);
			};

			vmQ.PDExcessAmount.subscribe(vmQ.UpdateExcess);
			vmQ.BIExcessAmount.subscribe(vmQ.UpdateExcess);
			vmQ.PDPctgAmt.subscribe(vmQ.UpdateExcess);
			vmQ.BIPctgAmtDays.subscribe(vmQ.UpdateExcess);
		};

		self.SSyncJSONAdditional = function(vmS, jsonS)
		{
			vmS.Industry(jsonS.Industry);
			vmS.Situation(jsonS.Situation);
			vmS.Order(jsonS.Order);
			vmS.EstSignPctg(jsonS.EstSignPctg);
		};

		self.OVSyncJSONAdditional = function(vmOV, jsonOV)
		{		    
			vmOV.TSICurrency(jsonOV.TSICurrency);

			if (jsonOV._TSICurrency !== undefined && jsonOV._TSICurrency !== null)
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

			vmOV.TSIPD(jsonOV.TSIPD);
			vmOV.TSIBI(jsonOV.TSIBI);
			vmOV.TSITotal(jsonOV.TSITotal);
		};

		self.QSyncJSONAdditional = function(vmQ, jsonQ)
		{
			vmQ.PDPctgAmt(jsonQ.PDPctgAmt);
			vmQ.PDExcessAmount(jsonQ.PDExcessAmount);

			vmQ.BIPctgAmtDays(jsonQ.BIPctgAmtDays);
			vmQ.BIExcessAmount(jsonQ.BIExcessAmount);

			vmQ.LineSize(jsonQ.LineSize);
			vmQ.LineToStand(jsonQ.LineToStand);
		};

		self.Model.submissionType = ko.observable("PV Submission");
		self.Model.submissionTypeId = ko.observable("PV");

		self.Model.Industry = ko.observable("");
		
		self.Model.Situation = ko.observable("");
		self.Model.Order = ko.observable("");
		self.Model.EstSignPctg = ko.observable(100);

		self.PDPctgAmtList = ko.observableArray(["%", "Amt"]);
		self.BIPctgAmtDaysList = ko.observableArray(["%", "Amt", "Days"]);		

		self.CreateFirstOption(self); // TODO: Move to the base

		$.when($.ajax(
		{
			type: 'GET',
			url: window.ValidusServicesUrl + 'interest',
			dataType: 'json'
		})).done(function(data)
		{
			data.unshift({ Code: "", Description: "" });
			self.InterestsList = data;

			self.Initialise(self);
		});

		return self;
	};
});