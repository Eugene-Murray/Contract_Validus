
/*
	TODO: Knockout Date-Picker Binding Handler

	Description;
		This is a custom binding handler for the customised date-picker component extension for date text inputs.

	References;
		TODO
	
	Usage;
		<input type="text" data-bind="date-picker: Observable" />
	Or;
		<div class="input-append" data-bind="date-picker: { data: Observable, options: { format: "dd/mm/yyyy" } }">
			<input type="text" />
			<span class="add-on"><i class="icon-calendar"></i></span>
		</div>
*/
ko.bindingHandlers["date-picker"] = {
	"init": function(element, valueAccessor, bindingsAccessor, viewModel, bindingContext)
	{
		var value = ko.isObservable(valueAccessor()) ? valueAccessor() : valueAccessor().data,
			options = !ko.isObservable(valueAccessor()) ? valueAccessor().options : {},
			defaults = {
				autoclose: true,
				calendarWeeks: false,
				clearBtn: false,
				format: "dd MM yyyy",
				todayBtn: false,
				todayHighlight: false
			};

		for (var option in options)
		{
			defaults[option] = options[option];
		}

		$(element).datepicker(defaults);

		ko.utils.registerEventHandler(element, "changeDate", function(e)
		{
			value(e.date.toISOString());
		});
	},
	"update": function(element, valueAccessor, bindingsAccessor, viewModel, bindingContext)
	{
		var value = ko.isObservable(valueAccessor())
			? ko.utils.unwrapObservable(valueAccessor())
			: ko.utils.unwrapObservable(valueAccessor().data);

		if (value) $(element).data("datepicker").setDate(new Date(value));
		else $(element).val("");
	}
};
