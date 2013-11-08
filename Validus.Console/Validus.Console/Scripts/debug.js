
//* Start DEBUG 
$(function()
{
	var debugSubmissions = [];//["CA", "EN", "FI", "HM", "ME", "PV"];

	function OpenDebugSubmission(debugSubmission)
	{
		Val_AddTab("Debug " + debugSubmission + " Submission", "/submission/_template" + debugSubmission, true, function(newTab)
		{
			newTab.data("val-submission", new ConsoleApp["vmSubmission" + debugSubmission](null, newTab.attr("id"), false));

			$("abbr[title]", newTab).tooltip();
		});
	}

	for (var submissionIter in debugSubmissions)
	{
		OpenDebugSubmission(debugSubmissions[submissionIter]);
	}
});
// End DEBUG */

$(".val-actionmenu a.val-debug-submission-CA1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateCA", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionCA(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model.InsuredId(222312);
		vmSubmission.Model.BrokerSequenceId("900");
		vmSubmission.Model.UnderwriterCode("DZS");
		vmSubmission.Model.LeaderNo("3623");
		vmSubmission.Model.Domicile("ID");
		vmSubmission.Model.Brokerage(15);
		vmSubmission.Model.QuotingOfficeId("LON");

		vmQuote1.COBId("AV");
		vmQuote1.MOA("FC");
		vmQuote1.OriginatingOfficeId("LON");
		vmQuote1.LimitCCY("GBP");
		vmQuote1.ExcessAmount(6000000);
		vmQuote1.LimitAmount(4000000);
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug CA1 Submission");

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

		vmSubmission.Model.InsuredId(240425);
		vmSubmission.Model.BrokerSequenceId("834");
		vmSubmission.Model.UnderwriterCode("JMC");
		vmSubmission.Model.LeaderNo("2010");
		vmSubmission.Model.Domicile("SF");
		vmSubmission.Model.Brokerage(15);
		vmSubmission.Model.QuotingOfficeId("LON");

		vmQuote1.COBId("AR");
		vmQuote1.MOA("FC");
		vmQuote1.OriginatingOfficeId("LON");
		vmQuote1.LimitCCY("USD");
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug EN1 Submission");

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

		vmSubmission.Model.InsuredId(87368);
		vmSubmission.Model.BrokerSequenceId(832);
		vmSubmission.Model.UnderwriterCode("AXW");
		vmSubmission.Model.Domicile("AD");
		vmSubmission.Model.Brokerage(20);
		vmSubmission.Model.QuotingOfficeId("LON");

		vmQuote1.COBId = ko.observable("CF");
		vmQuote1.MOA("FC");
		vmQuote1.OriginatingOfficeId("LON");
		vmQuote1.LimitCCY("USD");
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug FI1 Submission");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-HM1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateHM", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionHM(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model.InsuredId(185556);
		vmSubmission.Model.BrokerSequenceId(974);
		vmSubmission.Model.UnderwriterCode("MDM");
		vmSubmission.Model.LeaderNo("1200");
		vmSubmission.Model.Domicile("GR");
		vmSubmission.Model.Brokerage(15);
		vmSubmission.Model.QuotingOfficeId("LON");

		vmQuote1.COBId("AT");
		vmQuote1.MOA("FC");
		vmQuote1.OriginatingOfficeId("LON");
		vmQuote1.LimitCCY("GBP");
		vmQuote1.ExcessAmount(5000000);
		vmQuote1.LimitAmount(10000000);
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug HM1 Submission");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-ME1").click(function()
{
	Val_AddTab("New Submission", "/submission/_templateME", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionME(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model.InsuredId(185556);
		vmSubmission.Model.BrokerSequenceId(917);
		vmSubmission.Model.UnderwriterCode("IRP");
		vmSubmission.Model.Domicile("BD");
		vmSubmission.Model.BrokerContact("ALAN LANCASTER");
		vmSubmission.Model.Brokerage(25);
		vmSubmission.Model.QuotingOfficeId("LON");

		vmQuote1.COBId("CL");
		vmQuote1.MOA("FC");
		vmQuote1.OriginatingOfficeId("LON");
		vmQuote1.LimitCCY("GBP");
		vmQuote1.ExcessAmount(2500000);
		vmQuote1.LimitAmount(2500000);
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug ME1 Submission");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});

$(".val-actionmenu a.val-debug-submission-PV1").click(function()
{
	Val_AddTab("Debug Submission", "/submission/_templatePV", true, function(newTab)
	{
		var vmSubmission = new ConsoleApp.vmSubmissionPV(null, newTab.attr("id"), false),
			vmOption1 = vmSubmission.Model.Options()[0],
			vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

		vmSubmission.Model.InsuredId(150915);
		vmSubmission.Model.BrokerSequenceId(82);
		vmSubmission.Model.UnderwriterCode("SPT");
		vmSubmission.Model.UnderwriterContactCode("SPT");
		vmSubmission.Model.Domicile("US");
		vmSubmission.Model.Brokerage(20);
		vmSubmission.Model.QuotingOfficeId("MIA");

		vmQuote1.COBId("AF");
		vmQuote1.MOA("FC");
		vmQuote1.OriginatingOfficeId("MIA");
		vmQuote1.LimitCCY("USD");
		vmQuote1.InceptionDate(moment().format("DD MMM YYYY"));
		vmQuote1.Description("Debug PV1 Submission");

		newTab.data("val-submission", vmSubmission);

		window.debugSubmission1 = vmSubmission;

		console.log(window.debugSubmission1);
	});
});