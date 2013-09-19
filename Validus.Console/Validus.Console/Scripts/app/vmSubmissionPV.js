$(function()
{
    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};
    
    //ConsoleApp.Cache = {};

    //ConsoleApp.getData = function (val)
    //{
    //    // return either the cached value or an
    //    // jqXHR object (which contains a promise)
    //    return ConsoleApp.Cache[val] ||
    //        $.ajax({
	//	        type: 'GET',
	//	        url: window.ValidusServicesUrl + val,
	//	        dataType: 'json',
	//	        success: function (data)
	//	        {
	//	            ConsoleApp.Cache[val] = data;
	//	        }		        
	//	    });
    //    //    $.ajax('/foo/', {
    //    //    data: { value: val },
    //    //    dataType: 'json',
    //    //    success: function (resp) {
    //    //        cache[val] = resp;
    //    //    }
    //    //});
    //}
        
	ConsoleApp.vmSubmissionPV = function(id, domId)
	{
	    var self = new vmSubmissionBase(id, domId, false);

	    self.UpdateTSITotal = function (ov)
	    {
	        var tsitotal = parseFloat(ov.TSIPD()) + parseFloat(ov.TSIBI());
	        if (ov.TSITotal() == "")
	            ov.TSITotal(isNaN(tsitotal) ? "" : tsitotal);
	    }
      
	    self.OVAddAdditional = function (a)
	    {
	        a.TSICurrency = ko.observable("");
	        a._TSICurrency = ko.observable("");
	        a._TSICurrency.subscribe(function (value)
	        {
	            var currencyValues = (value) ? value.split(":") : [],
                    currencyPsu = (currencyValues[0]) ? currencyValues[0].trim() : "";

	            a.TSICurrency(currencyPsu);
	        });

	        a.TSIPD = ko.observable("");
	        a.TSIPD.subscribe(function() { self.UpdateTSITotal(a); });

			a.TSIBI = ko.observable("");
			a.TSIBI.subscribe(function () { self.UpdateTSITotal(a); });

			a.TSITotal = ko.observable("");
		};


	    self.UpdateExcess = function (q)
	    {
	        var pd = q.PDExcessAmount();
	        var bi = q.BIExcessAmount();
	        if (q.BIPctgAmtDays() != "Days" && pd!= null && bi!= null && pd != "" && bi != "" && !isNaN(pd) && !isNaN(bi))
	            q.ExcessAmount(parseFloat(q.PDExcessAmount()) + parseFloat(q.BIExcessAmount()));
	        else
	            q.ExcessAmount(0);
	    }

		self.QAddAdditional = function (a)
		{
		    a.PDPctgAmt = ko.observable("");
		    a.PDExcessCurrency = ko.observable("");
		    a._PDExcessCurrency = ko.observable("");
		    a._PDExcessCurrency.subscribe(function (value) {
		        var currencyValues = (value) ? value.split(":") : [],
                    currencyPsu = (currencyValues[0]) ? currencyValues[0].trim() : "";

		        a.PDExcessCurrency(currencyPsu);
		    });

			a.PDExcessAmount = ko.observable("");
			a.BIPctgAmtDays = ko.observable("");

			a.BIExcessCurrency = ko.observable("");
			a._BIExcessCurrency = ko.observable("");
			a._BIExcessCurrency.subscribe(function (value) {
			    var currencyValues = (value) ? value.split(":") : [],
                    currencyPsu = (currencyValues[0]) ? currencyValues[0].trim() : "";

			    a.BIExcessCurrency(currencyPsu);
			});
			
			a.BIExcessAmount = ko.observable("");
			a.LineSize = ko.observable("");
			a.LineToStand = ko.observable(true);

			a.PDExcessAmount.subscribe(function () { self.UpdateExcess(a); });
			a.BIExcessAmount.subscribe(function () { self.UpdateExcess(a); });
		};

		self.SSyncJSONAdditional = function(a, b) {
			a.Industry(b.Industry);
			a.Situation(b.Situation);
			a.Order(b.Order);
			a.EstSignPctg(b.EstSignPctg);
		};

		self.OSyncJSONAdditional = function(a, b) {
		};

		self.OVSyncJSONAdditional = function (a, b)
		{		    
		    a.TSICurrency(b.TSICurrency);
		    if ((b._TSICurrency != undefined) && (b._TSICurrency != null))
		        a._TSICurrency(b._TSICurrency);
		    else if (b.TSICurrency)
		    {
		        $.getJSON(window.ValidusServicesUrl + "Currency", { psu: b.TSICurrency }, function (jsonData)
		        {
		            $(jsonData).each(function (index, item)
		            {
		                a._TSICurrency(item.Psu + " : " + item.Name);

		                return false;
		            });
		        });
		    }

			a.TSIPD(b.TSIPD);
			a.TSIBI(b.TSIBI);
			a.TSITotal(b.TSITotal);
		};

		self.QSyncJSONAdditional = function(a, b) {
			a.PDPctgAmt(b.PDPctgAmt);
			a.PDExcessCurrency(b.PDExcessCurrency);
			if ((b._PDExcessCurrency != undefined) && (b._PDExcessCurrency != null))
			    a._PDExcessCurrency(b._PDExcessCurrency);
			else if (b.PDExcessCurrency) {
			    $.getJSON(window.ValidusServicesUrl + "Currency", { psu: b.PDExcessCurrency }, function (jsonData) {
			        $(jsonData).each(function (index, item) {
			            a._PDExcessCurrency(item.Psu + " : " + item.Name);

			            return false;
			        });
			    });
			}

			a.PDExcessAmount(b.PDExcessAmount);
			a.BIPctgAmtDays(b.BIPctgAmtDays);
			a.BIExcessCurrency(b.BIExcessCurrency);
			if ((b._BIExcessCurrency != undefined) && (b._BIExcessCurrency != null))
			    a._BIExcessCurrency(b._BIExcessCurrency);
			else if (b.BIExcessCurrency) {
			    $.getJSON(window.ValidusServicesUrl + "Currency", { psu: b.BIExcessCurrency }, function (jsonData) {
			        $(jsonData).each(function (index, item) {
			            a._BIExcessCurrency(item.Psu + " : " + item.Name);

			            return false;
			        });
			    });
			}

			a.BIExcessAmount(b.BIExcessAmount);
			a.LineSize(b.LineSize);
			a.LineToStand(b.LineToStand);
		};

	    // Need to instaniate function before it is applied in Javascript
	    //self.AddOptionVersion_PV = function (obj, e) {
	    //    var useNewOptionVersionWithExtraProperties = true;

	    //    var newOV = new OptionVersion(domId, this.CurrentVersion());

	    //    newOV.PDPctgAmt = ko.observable("");
	    //    newOV.PDExcessCurrency = ko.observable("");
	    //    newOV.PDExcessAmount = ko.observable("");
	    //    newOV.BIPctgAmtDays = ko.observable("");

	    //    var length = this.CurrentVersion().AddOptionVersion(newOV, e, useNewOptionVersionWithExtraProperties);

	    //    return length;
	    //};
		
        //  Submission
		self.Model.submissionType = ko.observable('PV Submission');
		self.Model.submissionTypeId = ko.observable('PV');

		self.Model.Industry = ko.observable('');
		
		self.Model.Situation = ko.observable('');
		self.Model.Order = ko.observable('');
		self.Model.EstSignPctg = ko.observable(100);

		self.PDPctgAmtList = ko.observableArray(["%", "Amt"]);
		self.BIPctgAmtDaysList = ko.observableArray(["%", "Amt", "Days"]);		

        //  TODO: Why isn't this in the base?
		var option1 = self.CreateFirstOption(self);
		
		self.Save_PVSubmission = function(element, e)
		{
			var isNew = (self.Id === 0);
			var ajaxUrl = (!isNew) ? "/submission/EditSubmission" : "/submission/CreateSubmission";

			self.Save(element, e, null, ajaxUrl, self.syncPVJSON);
		};
		
        $.when(
		    $.ajax({
		        type: 'GET',
		        url: window.ValidusServicesUrl + 'interest',
		        dataType: 'json'
		    })
		).done(
            function (data, data2)
		    {
                data.unshift({ Code: "", Description: "" });
                self.InterestsList = data;

                self.Initialise(self); 
            }
        );

		return self;
	};
});