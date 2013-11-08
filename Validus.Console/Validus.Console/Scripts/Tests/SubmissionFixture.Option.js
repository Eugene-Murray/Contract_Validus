(function ($) {

	module('vmSubmission - Option TestFixture');
	
	test('GetOptionsForQuoteSheet_Success', function()
	{
		// Assign
		//debugger;
		var vmSubmissionParent = new Submission(0, null);
		var options = [new Option(1, '-', vmSubmissionParent).Id(1).AddToQuoteSheet(true),
			new Option(2, '-', vmSubmissionParent).Id(2).AddToQuoteSheet(false),
			new Option(3, '-', vmSubmissionParent).Id(3).AddToQuoteSheet(true),
			new Option(4, '-', vmSubmissionParent).Id(4).AddToQuoteSheet(false),
			new Option(5, '-', vmSubmissionParent).Id(5).AddToQuoteSheet(false)];

		options[0].OptionVersions().push(new OptionVersion(0, '-', options[0]).VersionNumber(0));
		options[0].OptionVersions().push(new OptionVersion(1, '-', options[0]).VersionNumber(1));
		options[0].OptionVersions().push(new OptionVersion(2, '-', options[0]).VersionNumber(2));

		options[1].OptionVersions().push(new OptionVersion(0, '-', options[3]).VersionNumber(0));
		options[1].OptionVersions().push(new OptionVersion(1, '-', options[3]).VersionNumber(1));
		options[1].OptionVersions().push(new OptionVersion(2, '-', options[3]).VersionNumber(2));
		options[1].OptionVersions().push(new OptionVersion(3, '-', options[3]).VersionNumber(3));

		vmSubmissionParent.Model.Options().push(options[0]);
		vmSubmissionParent.Model.Options().push(options[1]);
		vmSubmissionParent.Model.Options().push(options[2]);
		vmSubmissionParent.Model.Options().push(options[3]);
		vmSubmissionParent.Model.Options().push(options[4]);

		// Act
		var optionListResult = vmSubmissionParent.Model.Options()[0].GetOptionsForQuoteSheet();
		
		// Assert
		ok((optionListResult.length > 0), 'optionList greater then 0');
		equal(2, optionListResult.length, "optionList length = 2");
		equal(1, optionListResult[0].OptionId, "option id is 1");
		equal(3, optionListResult[1].OptionId, "option id is 3");
		equal(1, optionListResult[0].OptionVersionNumberList[2], "version number is 1");
	});

	asyncTest('CreateQuoteSheet_Success', function()
	{
		// Assign
		var vmSubmissionParent = new Submission(1, null);
		var options = [new Option(1, '-', vmSubmissionParent).Id(1).SubmissionId(1).AddToQuoteSheet(true)];
		options[0].OptionVersions().push(new OptionVersion(0, '-', options[0]).VersionNumber(0));
		vmSubmissionParent.Model.Options().push(options[0]);
		
		// Act
		vmSubmissionParent.Model.Options()[0].CreateQuoteSheet();
		start();
		// Assert

	});
	
	test('SetMaster_Success', function()
	{

	});
	
	test('NavigateToQuote_Success', function()
	{

	});
	
	test('NavigateToQuote_Success', function()
	{

	});
	
	test('CopyOptionVersion_Success', function()
	{

	});
	
	test('AddOptionVersion_Success', function()
	{

	});
	
	test('SetVersionIndex_Success', function()
	{

	});


}(jQuery));