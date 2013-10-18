
$(".val-actionmenu a.val-debug-submission-PV1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templatePV", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionPV(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("SPT : Steven Paul Tebbutt");
		vmSubmission.Model._Leader("MJ");
		vmSubmission.Model._Domicile("US : UNITED STATES");
		vmSubmission.Model.InsuredName("MLE PROJECT TERRORISM: 150915");
		vmSubmission.Model.Broker("0576 : WIL : Willis Limited : 804 : WIL");
		vmSubmission.Model.BrokerContact("ALAN LONG");
		vmSubmission.Model.Brokerage(20);
		vmSubmission.Model._QuotingOffice("LON : London");

		vmQuote1._COB("AF : AGY-Direct - Property - Political Violence");
		vmQuote1._MOA("FC : Direct - Open Market");
		vmQuote1._OriginatingOffice("LON : London");
		vmQuote1._Currency("USD : Us Dollar");
		vmQuote1._ExcessCCY("USD : Us Dollar");
		vmQuote1._LimitCCY("USD : Us Dollar");
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-CA").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateCA", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionCA(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("DZS : David Silk");
		vmSubmission.Model._Leader("BEA");
		vmSubmission.Model._Domicile("ID : INDONESIA");
		vmSubmission.Model.InsuredName("GC RESOURCES CO LTD : 222312");
		vmSubmission.Model.Broker("0509 : CTB : Marsh Ltd : 24 : MSH");
		vmSubmission.Model.BrokerContact("ADAM HEMMINGWAY");
		vmSubmission.Model.Brokerage(15);
		vmSubmission.Model._QuotingOffice("LON : London");

		vmQuote1._COB("AV : AGY-Direct - Property - Cargo");
		vmQuote1._MOA("FC : Direct - Open Market");
		vmQuote1._OriginatingOffice("LON : London");
		vmQuote1._Currency("GBP : Sterling");
		vmQuote1._ExcessCCY("GBP : Sterling");
		vmQuote1._LimitCCY("GBP : Sterling");
		vmQuote1._ExcessAmount(6000000);
		vmQuote1._LimitAmount(4000000);
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-HM").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateHM", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionHM(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("MDM : Mike MacColl");
		vmSubmission.Model._Leader("GAM");
		vmSubmission.Model._Domicile("GR : GREECE");
		vmSubmission.Model.InsuredName("LAVARETUS UNDERWRITING AB : 185556");
		vmSubmission.Model.Broker("1156 : GCB : JLT Specialty Ltd : 947 : JLT");
		vmSubmission.Model.BrokerContact("PHIL PAVEY");
		vmSubmission.Model.Brokerage(15);
		vmSubmission.Model._QuotingOffice("LON : London");

		vmQuote1._COB("AT : Direct - Property - Hull");
		vmQuote1._MOA("FC : Direct - Open Market");
		vmQuote1._OriginatingOffice("LON : London");
		vmQuote1._Currency("GBP : Sterling");
		vmQuote1._ExcessCCY("GBP : Sterling");
		vmQuote1._LimitCCY("GBP : Sterling");
		vmQuote1._ExcessAmount(5000000);
		vmQuote1._LimitAmount(10000000);
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-ME").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateME", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionME(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("IRP : Ian Peterson");
		vmSubmission.Model._Leader("CAF");
		vmSubmission.Model._Domicile("BD : BERMUDA");
		vmSubmission.Model.InsuredName("STEAMSHIP MUTUAL UNDERWRITING ASSOCIATION BERMUDA : 126901");
		vmSubmission.Model.Broker("0621 : MIL : Miller Insurance Services Ltd : 453 : MIL");
		vmSubmission.Model.BrokerContact("ALAN LANCASTER");
		vmSubmission.Model.Brokerage(25);
		vmSubmission.Model._QuotingOffice("LON : London");

		vmQuote1._COB("CL : Direct - Casualty - Marine Liability");
		vmQuote1._MOA("FC : Direct - Open Market");
		vmQuote1._OriginatingOffice("LON : London");
		vmQuote1._Currency("GBP : Sterling");
		vmQuote1._ExcessCCY("GBP : Sterling");
		vmQuote1._LimitCCY("GBP : Sterling");
		vmQuote1._ExcessAmount(2500000);
		vmQuote1._LimitAmount(2500000);
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-FI1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateFI", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionFI(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model._Underwriter("AXW : Anthony Westlake");
		vmSubmission.Model._Leader("FED");
		vmSubmission.Model._Domicile("AD : ABU DHABI");
		vmSubmission.Model.InsuredName("BANK OF TOKIO: 87368");
		vmSubmission.Model.Broker("0509 : CTB : Marsh Ltd: 24 : MSH");
		vmSubmission.Model.BrokerContact("ADAM WAKELEY");

		vmSubmission.Model.Brokerage(20);

		vmQuote1.COB = ko.observable("CF");
		vmQuote1.COBId = ko.observable("CF");
		vmQuote1._COB = ko.observable("CF : Direct - Casualty - Financial Institutions");
		vmQuote1._MOA("FC : Direct - Open Market ");
		vmQuote1._OriginatingOffice("LON : London");
		vmQuote1._Currency("USD : Us Dollar");
		vmQuote1._ExcessCCY("USD : Us Dollar");
		vmQuote1._LimitCCY("USD : Us Dollar");
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug");

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

		vmSubmission.Model._Underwriter("JMC : James McDonald");
		vmSubmission.Model._Leader("HEB");
		vmSubmission.Model._Domicile("SF : SOUTH AFRICA");
		vmSubmission.Model.InsuredName("MARINE PLATFORMS LTD: 240425");
		vmSubmission.Model.Broker("1907 : AON : Aon Benfield Italia S.p.A. : 834 : ABI");
		vmSubmission.Model.BrokerContact("GIORGIO SAVIANE");
		vmSubmission.Model.Brokerage(15);
		vmSubmission.Model._QuotingOffice("LON : London");

		vmQuote1._COB("AR : AGY-Direct - Property - Rig");
		vmQuote1._MOA("FC : Direct - Open Market");
		vmQuote1._OriginatingOffice("LON : London");
		vmQuote1._Currency("USD : Us Dollar");
		vmQuote1._ExcessCCY("USD : Us Dollar");
		vmQuote1._LimitCCY("USD : Us Dollar");
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug");

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

		vmSubmission.Model._Underwriter("AJS : Tony Sarjeant");
		vmSubmission.Model._Leader("TAL");
		vmSubmission.Model._Domicile("UK : UNITED KINGDOM");
		vmSubmission.Model.InsuredName("MORECAMBE BAY PLATFORM DP4 : 125713");
		vmSubmission.Model.Broker("0878 : AHJ : Alwen Hough Johnson Ltd : 8 : AHJ");
		vmSubmission.Model.BrokerContact("CHARLES BRAY");
		vmSubmission.Model.Brokerage(20);
		vmSubmission.Model._QuotingOffice("LON : London");

		vmQuote1_1._COB("AR : AGY-Direct - Property - Rig");
		vmQuote1_1._MOA("FC : Direct - Open Market ");
		vmQuote1_1._OriginatingOffice("LON : London");
		vmQuote1_1._Currency("USD : Us Dollar");
		vmQuote1_1._ExcessCCY("USD : Us Dollar");
		vmQuote1_1._LimitCCY("USD : Us Dollar");
		vmQuote1_1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1_1.Description("Debug");

		vmQuote2_1._COB("CR : Direct - Casualty - Energy Liability");
		vmQuote2_1._MOA("FC : Direct - Open Market");
		vmQuote2_1._OriginatingOffice("LON : London");
		vmQuote2_1._Currency("USD : Us Dollar");
		vmQuote2_1._ExcessCCY("USD : Us Dollar");
		vmQuote2_1._LimitCCY("USD : Us Dollar");
		vmQuote2_1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote2_1.Description("Debug");

		vmQuote3_1._COB("CL : AGY-Direct - Casualty - Marine Liability ");
		vmQuote3_1._MOA("FC : Direct - Open Market");
		vmQuote3_1._OriginatingOffice("LON : London");
		vmQuote3_1._Currency("USD : Us Dollar");
		vmQuote3_1._ExcessCCY("USD : Us Dollar");
		vmQuote3_1._LimitCCY("USD : Us Dollar");
		vmQuote3_1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote3_1.Description("Debug");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission2 = vmSubmission;

		console.log(window.debugSubmission2);
	});
});