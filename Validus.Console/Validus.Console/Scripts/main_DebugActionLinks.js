
$(".val-actionmenu a.val-debug-submission-PV1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templatePV", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionPV(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("AED : Alexandra Davies");
		vmSubmission.Model._Leader("AG");
		vmSubmission.Model._Domicile("AD : ABU DHABI");
		vmSubmission.Model.InsuredName("- N/A : 182396");
		vmSubmission.Model.Broker("1111 : AAA : AAA Insurance & Reinsurance Brokers Ltd : 822 : AAA");
		vmSubmission.Model.BrokerContact("ALLAN MURRAY");
		vmSubmission.Model.Description("Debug submission 1");
		vmSubmission.Model.Brokerage(1);

		vmQuote1._COB("BA : Direct - Aviation - Airline");
		vmQuote1._MOA("FA : Binding Authority - Full Authority");
		vmQuote1._OriginatingOffice("DUB : Dubai");
		vmQuote1._Currency("USD : Us Dollar");
		vmQuote1._ExcessCCY("USD : Us Dollar");
		vmQuote1._LimitCCY("USD : Us Dollar");
		vmQuote1.InceptionDate(moment().format("YYYY-MM-DD"));

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});


$(".val-actionmenu a.val-debug-submission-FI1").click(function () {
    Val_AddTab("New Submission", "/submission/_templateFI", true, function (newTab) {
        var vmSubmission = new ConsoleApp.vmSubmissionFI(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

        vmSubmission.Model._Underwriter("AED : Alexandra Davies");
        vmSubmission.Model._Leader("AG");
        vmSubmission.Model._Domicile("AD : ABU DHABI");
        vmSubmission.Model.InsuredName("- N/A : 182396");
        vmSubmission.Model.Broker("1111 : AAA : AAA Insurance & Reinsurance Brokers Ltd : 822 : AAA");
        vmSubmission.Model.BrokerContact("ALLAN MURRAY");
        vmSubmission.Model.Description("Debug submission 1");
        vmSubmission.Model.Brokerage(1);

        vmQuote1.COB = ko.observable("CF");
        vmQuote1.COBId = ko.observable("CF");
        vmQuote1._COB = ko.observable("CF : Direct - Casualty - Financial Institutions");
        vmQuote1._MOA("FA : Binding Authority - Full Authority");
        vmQuote1._OriginatingOffice("DUB : Dubai");
        vmQuote1._Currency("USD : Us Dollar");
        vmQuote1._ExcessCCY("USD : Us Dollar");
        vmQuote1._LimitCCY("USD : Us Dollar");
        vmQuote1.InceptionDate(moment().format("YYYY-MM-DD"));

        newTab.data("val-submission", vmSubmission);

        window.debugSubmission1 = vmSubmission;

        console.log(window.debugSubmission1);
    });
});

$(".val-actionmenu a.val-debug-submission-EN1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateEN", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionEN(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("AED : Alexandra Davies");
		vmSubmission.Model._Leader("AG");
		vmSubmission.Model._Domicile("AD : ABU DHABI");
		vmSubmission.Model.InsuredName("- N/A : 182396");
		vmSubmission.Model.Broker("1111 : AAA : AAA Insurance & Reinsurance Brokers Ltd : 822 : AAA");
		vmSubmission.Model.BrokerContact("ALLAN MURRAY");
		vmSubmission.Model.Description("Debug submission 1");
		vmSubmission.Model.Brokerage(1);
		//vmQuote1.Model._QuotingOffice('DUB : Dubai');
		
		vmQuote1._COB("BA : Direct - Aviation - Airline");
		vmQuote1._MOA("FA : Binding Authority - Full Authority");
		vmQuote1._OriginatingOffice("DUB : Dubai");
		vmQuote1._Currency("USD : Us Dollar");
		vmQuote1._ExcessCCY("USD : Us Dollar");
		vmQuote1._LimitCCY("USD : Us Dollar");
		vmQuote1.InceptionDate(moment().format("YYYY-MM-DD"));

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-EN2").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateEN", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionEN(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmOption2 = vmSubmission.Model.Options()[vmSubmission.AddOption() - 1],
			vmOption3 = vmSubmission.Model.Options()[vmSubmission.AddOption() - 1],
			vmQuote1_1 = vmOption1.CurrentVersion().Quotes()[0],
			vmQuote2_1 = vmOption2.CurrentVersion().Quotes()[0],
			vmQuote3_1 = vmOption3.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("AED : Alexandra Davies");
		vmSubmission.Model._Leader("TAL");
		vmSubmission.Model._Domicile("UK : UNITED KINGDOM");
		vmSubmission.Model.InsuredName("PRETORIA PORTLAND CEMENT : 5654");
		vmSubmission.Model.Broker("1111 : AAA : AAA Insurance & Reinsurance Brokers Ltd : 822 : AAA");
		vmSubmission.Model.BrokerContact("ALLAN MURRAY");
		vmSubmission.Model.Description("Debug submission 2");
		vmSubmission.Model.Brokerage(10);
		//vmQuote1.Model.QuotingOffice('DUB : Dubai');

		vmQuote1_1._COB("HE : Treaty - Marine - Energy");
		vmQuote1_1._MOA("FC : Direct - Open Market");
		vmQuote1_1._OriginatingOffice("LON : London");
		vmQuote1_1._Currency("USD : Us Dollar");
		vmQuote1_1._ExcessCCY("USD : Us Dollar");
		vmQuote1_1._LimitCCY("USD : Us Dollar");
		vmQuote1_1.InceptionDate(moment().format("YYYY-MM-DD"));

		vmQuote2_1._COB("CR : Direct - Casualty - Energy Liability");
		vmQuote2_1._MOA("FC : Direct - Open Market");
		vmQuote2_1._OriginatingOffice("LON : London");
		vmQuote2_1._Currency("USD : Us Dollar");
		vmQuote2_1._ExcessCCY("USD : Us Dollar");
		vmQuote2_1._LimitCCY("USD : Us Dollar");
		vmQuote2_1.InceptionDate(moment().format("YYYY-MM-DD"));

		vmQuote3_1._COB("AJ : Direct - Property - Energy Onshore");
		vmQuote3_1._MOA("FC : Direct - Open Market");
		vmQuote3_1._OriginatingOffice("LON : London");
		vmQuote3_1._Currency("USD : Us Dollar");
		vmQuote3_1._ExcessCCY("USD : Us Dollar");
		vmQuote3_1._LimitCCY("USD : Us Dollar");
		vmQuote3_1.InceptionDate(moment().format("YYYY-MM-DD"));

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission2 = vmSubmission;

		console.log(window.debugSubmission2);
	});
});

$(".val-actionmenu a.val-example-submission-EN1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateExampleEN", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionExampleEnergy(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("AED : Alexandra Davies");
		vmSubmission.Model._Leader("AG");
		vmSubmission.Model._Domicile("AD : ABU DHABI");
		vmSubmission.Model.InsuredName("- N/A : 182396");
		vmSubmission.Model.Broker("1111 : AAA : AAA Insurance & Reinsurance Brokers Ltd : 822 : AAA");
		vmSubmission.Model.BrokerContact("ALLAN MURRAY");
		vmSubmission.Model.Description("Debug submission 1");
		vmSubmission.Model.Brokerage(1);

		vmQuote1._COB("BA : Direct - Aviation - Airline");
		vmQuote1._MOA("FA : Binding Authority - Full Authority");
		vmQuote1._OriginatingOffice("DUB : Dubai");
		vmQuote1._Currency("USD : Us Dollar");
		vmQuote1._ExcessCCY("USD : Us Dollar");
		vmQuote1._LimitCCY("USD : Us Dollar");
		vmQuote1.InceptionDate(moment().format("YYYY-MM-DD"));

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});
