
/*
	Knockout Auto-Numeric Binding Handler

	Description;
		This is a custom binding handler for the customised auto-numeric component extension for currency text inputs.

	References;
		http://www.decorplanit.com/plugin/
	
	Usage;
		<input type="text" data-bind="auto-numeric: { data: Observable, options: { vMin: 0, vMax: 100 } } " />
*/
ko.bindingHandlers["auto-numeric"] = {
	"init": function(element, valueAccessor, bindingsAccessor, viewModel, bindingContext)
	{
		var koAccessor = ko.utils.unwrapObservable(valueAccessor()),
			inputOptions = {
				vMin: 0.00,
				vMax: 9999999999999.99,
				noDecimals: true
			};

		if (koAccessor.options)
		{
			for (var option in koAccessor.options)
			{
				inputOptions[option] = koAccessor.options[option];
			}
		}

		$(element).autoNumeric("init", inputOptions);

		if ($(element).is("input[type='text']"))
		{				
			ko.utils.registerEventHandler(element, bindingsAccessor().valueUpdate || "change", function()
			{
				koAccessor.data($(element).autoNumeric("get"));
			});
		}
	},
	"update": function(element, valueAccessor, bindingsAccessor, viewModel, bindingContext)
	{
		var koAccessor = ko.utils.unwrapObservable(valueAccessor());

		if (!/\./.test($(element).val()))
		{
			$(element).autoNumeric("set", koAccessor.data());
		}
	}
};