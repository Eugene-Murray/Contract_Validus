var ConsoleAppTests = window.ConsoleAppTests = window.ConsoleAppTests || {};

// Helper Functions...
ConsoleAppTests.ReturnEditSubmissionJson = function()
{
	return JSON.stringify({ "Submission": { "Id": 80, "Title": "New Submission", "InsuredName": "	WOODBROOK VALE HIGH SCHOOL", "InsuredId": 0, "BrokerPseudonym": "AON", "BrokerCode": "0712", "BrokerSequenceId": 822, "BrokerContact": " STEVE DALCHOW", "Comments": null, "Options": [{ "Id": 123, "Title": "Option 1", "Comments": null, "SubmissionId": 80, "OptionVersions": [{ "OptionId": 123, "VersionNumber": 0, "Title": "Version 1", "Comments": null, "Quotes": [{ "Id": 212, "SubscribeReference": "CDY161357A13", "OptionId": 123, "VersionNumber": 0, "COBId": "CD", "COB": null, "MOA": "BD", "AccountYear": 2013, "OriginatingOfficeId": "MIA", "OriginatingOffice": null, "InceptionDate": "/Date(1365980400000)/", "ExpiryDate": "/Date(1366066800000)/", "DeclinatureReason": null, "DeclinatureComments": null, "TechnicalPremium": 1, "TechnicalPricingMethod": null, "TechnicalPricingBindStatus": null, "TechnicalPricingPremiumIsPctg": false, "BenchmarkPremium": 1, "QuotedPremium": 1, "LimitAmount": 1, "ExcessAmount": 1, "Currency": "USD", "Comment": null, "QuoteExpiryDate": "/Date(1397516400000)/", "IsPromisedLine": false, "FacilityRef": null, "CreatedById": 5, "LastModifiedById": 5, "SubmissionStatus": "SUBMITTED", "Created": "/Date(1366030803213)/", "LastModified": "/Date(1366030803213)/", "SubscribeTimestamp": 2499635598, "IsSubscribeMaster": true, "CopiedFromQuoteId": null, "CorrelationToken": "57bbec6b-d8d1-4ab7-9420-9cbc6c1b0e4d", "Timestamp": [0, 0, 0, 0, 0, 0, 40, 154] }], "IsExperiment": false, "IsLocked": false, "QuoteSheets": null, "Timestamp": [0, 0, 0, 0, 0, 0, 40, 153] }], "Timestamp": [0, 0, 0, 0, 0, 0, 40, 152] }, { "Id": 124, "Title": "Option 2", "Comments": null, "SubmissionId": 80, "OptionVersions": [{ "OptionId": 124, "VersionNumber": 0, "Title": "Version 1", "Comments": null, "Quotes": [{ "Id": 213, "SubscribeReference": "BDN161358A13", "OptionId": 124, "VersionNumber": 0, "COBId": "BD", "COB": null, "MOA": "CI", "AccountYear": 2013, "OriginatingOfficeId": "MIA", "OriginatingOffice": null, "InceptionDate": "/Date(1365980400000)/", "ExpiryDate": "/Date(1366153200000)/", "DeclinatureReason": null, "DeclinatureComments": null, "TechnicalPremium": 2, "TechnicalPricingMethod": null, "TechnicalPricingBindStatus": null, "TechnicalPricingPremiumIsPctg": false, "BenchmarkPremium": 2, "QuotedPremium": 2, "LimitAmount": 2, "ExcessAmount": 2, "Currency": "USD", "Comment": null, "QuoteExpiryDate": "/Date(1397516400000)/", "IsPromisedLine": false, "FacilityRef": null, "CreatedById": 5, "LastModifiedById": 5, "SubmissionStatus": 'SUBMITTED', 'Created': '/Date(1366030803610)/', 'LastModified': '/Date(1366030803610)/', 'SubscribeTimestamp': 2499635638, 'IsSubscribeMaster': true, 'CopiedFromQuoteId': null, 'CorrelationToken': 'da808f9f-9b65-44fe-b188-7dc6bad54689', 'Timestamp': [0, 0, 0, 0, 0, 0, 40, 158] }], 'IsExperiment': false, 'IsLocked': false, 'QuoteSheets': null, 'Timestamp': [0, 0, 0, 0, 0, 0, 40, 157] }], 'Timestamp': [0, 0, 0, 0, 0, 0, 40, 156] }], 'Timestamp': [0, 0, 0, 0, 0, 0, 40, 155], 'Underwriter': 'AXN', 'Leader': 'AAA', 'Domicile': 'AL', 'Brokerage': 1, 'Description': 'description' } });
};

ConsoleAppTests.CreateBasicSubmission = function(form) {

	var vmSubmission = new Submission(0, 'formTestFixture'),
			    vmOption1 = vmSubmission.Model.Options()[0],
			    vmQuote1 = vmOption1.CurrentVersion().Quotes()[0];

	vmSubmission.Form = form;
	vmSubmission.Model._Underwriter("AFM : Adam Murphy");
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
	vmQuote1.QuoteExpiryDate(moment().add("days", 30).format("YYYY-MM-DD"));
	

	return vmSubmission;
};


ConsoleAppTests.CreateMultiValueSubmission = function(form) {
};