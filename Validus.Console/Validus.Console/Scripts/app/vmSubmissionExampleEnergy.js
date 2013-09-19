$(function () {
	var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

	ConsoleApp.vmSubmissionExampleEnergy = function(id, domId){
		// Inherit from base
		var self = new vmSubmissionBase(id, domId, false);

		// Need to instaniate function before it is applied in Javascript
		self.AddQuote_ExampleEnergySubmission = function(obj, e)
		{
			var useNewQuoteWithExtraProperties = true;

			var newQuote = new Quote(domId, this.CurrentVersion());
			newQuote.quoteExtraProperty1 = ko.observable('');

			var length = this.CurrentVersion().AddQuote(newQuote, e, useNewQuoteWithExtraProperties);

			return length;
		};

		// Add properties to Base model
		self.Model.submissionType = ko.observable('Example Energy Submission');
		self.Model.submissionTypeId = ko.observable('EN');
		self.Model.ExampleEnergy_SubExtraProperty1 = ko.observable('');
		self.Model.ExampleEnergy_SubExtraProperty2 = ko.observable('');
		var option1 = self.CreateFirstOption(self);
		option1.CurrentVersion().AddQuote_ExampleEnergySubmission = self.AddQuote_ExampleEnergySubmission;
		self.vmQuote1 = option1.CurrentVersion().Quotes()[0];
		self.vmQuote1.ExampleEnergy_QuoteExtraProperty1 = ko.observable('');

		// Example function added to inheriting view model
		self.SomeAction = function() {
			console.log('Fire some action on ExampleEnergy Submission');
			alert('Fire some action on ExampleEnergy Submission');
		};
		
		// Over ride base method
		self.Save_ExampleEnergySubmission = function(element, e, callback)
		{
			var isNew = (self.Id === 0);
			var ajaxUrl = (!isNew) ? "/submission/EditSubmission" : "/submission/CreateSubmission";

			self.Save(element, e, callback, ajaxUrl, self.syncExampleEnergyJSON);
		};
		
		// Over ride base method
		self.AddOption_ExampleEnergySubmission = function()
		{
			var length = self.Model.Options().length,
				newOption = new Option(length, domId, self),
				submissionId = self.Model.Id();

			newOption.SubmissionId(submissionId);
			newOption.CurrentVersion().Quotes()[0].ExampleEnergy_QuoteExtraProperty1 = ko.observable('');
			newOption.CurrentVersion().AddQuote_ExampleEnergySubmission = self.AddQuote_ExampleEnergySubmission;

			length = self.Model.Options.push(newOption);

			self.AttachValidation();

			return length;
		}; 
		
		// Used as a callback 
		self.syncExampleEnergyJSON = function(submission)
		{
			//for (var prop in submission) {
			//	if (self.hasOwnProperty(prop))
			//		self[prop](submission[prop]);
			//}

			self.Model.ExampleEnergy_SubExtraProperty1(submission.ExtraProperty1);
			self.Model.ExampleEnergy_SubExtraProperty2(submission.ExtraProperty2);

			$.each(submission.Options, function(oIndex, option)
			{
				var koOption = self.Model.Options()[oIndex];

				$.each(option.OptionVersions, function(ovIndex, optionVersion)
				{
					var koOptionVersion = koOption.OptionVersions()[ovIndex];

					$.each(optionVersion.Quotes, function(qIndex, quote)
					{
						var koQuote = koOptionVersion.Quotes()[qIndex];
						koQuote.ExampleEnergy_QuoteExtraProperty1(quote.QuoteExtraProperty1);
					});
				});
			});
		};

		//self.InitialiseInheritingVM = function() {
		//	$.when(self.GetTeamBySubmissionTypeId(self.Model.submissionTypeId()))
		//		.done($.proxy(function(team) {
		//			self.GetQuoteTemplates(team.Id);
		//		}, this));
		//};

		// Initialize Base / self if necessary
 		self.Initialise(self, self.syncExampleEnergyJSON);
		//self.InitialiseInheritingVM();

		return self;
	};
});