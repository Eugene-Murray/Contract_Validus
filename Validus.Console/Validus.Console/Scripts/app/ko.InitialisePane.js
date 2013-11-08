// TODO: To remove
var ConsoleApp = window.ConsoleApp = window.ConsoleApp || { };

ConsoleApp.InitialisePane = function(element, submission)
{
	element = (!element) ? (!self.Form) ? document.body : self.Form : element;

	ConsoleApp.InitialiseDatePickers(element);
};
