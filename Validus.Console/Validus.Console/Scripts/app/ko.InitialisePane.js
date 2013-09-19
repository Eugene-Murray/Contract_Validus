// TODO: To remove
var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.InitialisePane = function(element, submission)
{
	element = (!element) ? (!self.Form) ? document.body : self.Form : element;

	// TODO: We REALLY need to remove the need for these Typeaheads
	ConsoleApp.InitialiseTypeaheads(element, submission);
	
	ConsoleApp.InitialiseDatePickers(element);

	$("input.val-currency").unbind("keydown.currency")
	    .bind("keydown.currency", function(e)
	    {
	        var preventDefault = false,
	        	target = $(e.target),
	        	value = target.val();

	        if ((!value) || (value === "0")) value = "1";

	        value = parseFloat(value);

	        if (!isNaN(value))
	        {
	        	switch (e.keyCode)
	        	{
	        		case 66: // b
	        			{
	        				target.val(value * 1000000000);
	        				preventDefault = true;
	        			} break;
	        		case 77: // m
	        			{
	        				target.val(value * 1000000);
	        				preventDefault = true;
	        			} break;
	        		case 75: // k
	        			{
	        				target.val(value * 1000);
	        				preventDefault = true;
	        			} break;
	        		default: break;
	        	}

	        	if (preventDefault) e.preventDefault();
	        }
	    });
};
