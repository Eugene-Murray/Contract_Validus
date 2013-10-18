// TODO: To remove
var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

ConsoleApp.InitialisePane = function(element, submission)
{
	element = (!element) ? (!self.Form) ? document.body : self.Form : element;

	// TODO: We REALLY need to remove the need for these Typeaheads
	ConsoleApp.InitialiseTypeaheads(element, submission);
	ConsoleApp.InitialiseDatePickers(element);
};
