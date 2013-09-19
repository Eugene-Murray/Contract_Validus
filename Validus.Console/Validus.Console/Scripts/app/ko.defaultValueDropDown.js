ko.observableArray.fn.find = function(prop, data)
{
	var valueToMatch = data[prop];
	return ko.utils.arrayFirst(this(), function(item)
	{
		return item[prop] === valueToMatch;
	});
};

ko.bindingHandlers.defaultValueDropDown = {
	init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		var value = valueAccessor(),
			allBindings = allBindingsAccessor();


	}
	//,
	//update: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	//{
	//	// This will be called once when the binding is first applied to an element,
	//	// and again whenever the associated observable changes value.
	//	// Update the DOM element based on the supplied values here.
	//}
};