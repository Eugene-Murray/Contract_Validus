(function ($) {

	module('vmSubmission - Submission TestFixture');

	test('Submission.prototype.toJSON_Success', function()
	{
		// Assign
		//debugger;
		var vmSubmission = ConsoleAppTests.CreateBasicSubmission();
		
		// Act
		var actualResult = vmSubmission.toJSON();
		console.log(actualResult);
		// Assert
		ok((actualResult !== null), 'Submission object not null');
		equal("string", typeof actualResult, "actualResult is json string");
		ok((actualResult.indexOf('"InsuredId":"182396"') !== -1), "InsuredId set correctly");
	});

	asyncTest('Save_SimpleSubmission_Success', function()
	{
		// Assign
		//debugger;
		var vmSubmission = ConsoleAppTests.CreateBasicSubmission($('#formTestFixture'));
		//ko.applyBindings(vmSubmission); Note - not getting set

		// Act
		vmSubmission.Save();

		start();
		

		// Assert
		vmSubmission.Model.Options()[0].CurrentVersion().Quotes()[0].SubscribeReference.subscribe(function(newVal)
		{
			console.log('SubscribeReference: ' + newVal);
			ok((newVal !== '' && newVal !== undefined), 'SubscribeReference has been set - submission saved');
		});
		
	});
	
	//asyncTest('Save_SimpleSubmission_Success', function()
	//{
	//	// Assign
	//	//debugger;
	//	var vmSubmission = ConsoleAppTests.CreateMultiValueSubmission($('#formTestFixture'));
	//	//ko.applyBindings(vmSubmission); Note - not getting set

	//	// Act
	//	//vmSubmission.Save();
	//	//start();

	//	//// Assert
	//	//ok((vmSubmission.Model.Options()[0].OptionVersions()[0].Quotes()[0].SubscribeReference !== null), 'SubscribeReference has been set - submission saved');
	//});

	test('Edit_SimpleSubmission_Success', function()
	{
		// Assign
		//debugger;
		//var vmSubmission = new Submission(0, null);

		// Act


		// Assert

	});
	
	test('CopyOption_Success', function()
	{
		// Assign
		//debugger;
		//var vmSubmission = new Submission(0, null);

		// Act


		// Assert

	});
	
	test('AddOption_Success', function()
	{
		// Assign
		//debugger;
		//var vmSubmission = new Submission(0, null);

		// Act


		// Assert

	});
	
	
	test('Initialise_Success', function()
	{
		// Assign
		//debugger;
		//var vmSubmission = new Submission(0, null);

		// Act


		// Assert

	});


}(jQuery));