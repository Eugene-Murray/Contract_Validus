// TODO: turn into a binding handler
var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.InitialiseDatePickers = function(element)
{
	$("input[type='text'].datepicker:not(.ui-datepicker)", element).each(function(index, item)
	{
		$(item).datepicker(
		{
			// TODO: Restrict selectable dates ?, i.e. minDate: -20, maxDate: "+1M +10D"
			dateFormat: "yy-mm-dd",
			numberOfMonths: 1,
			showOtherMonths: true,
			selectOtherMonths: true,
			changeMonth: true,
			changeYear: true
		});

		$(item).siblings("button.datepicker").click(function(e)
		{
			e.preventDefault();

			$(item).focus();
		});
	});
};