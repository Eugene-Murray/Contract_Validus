/// <reference path="../lib/knockout-2.2.1.js" />


function observableToJson(rootObject, replacer, spacer)
{
	var cache = [];
	var plainJavaScriptObject = ko.toJS(rootObject);
	var replacerFunction = replacer || cycleReplacer;
	var output = ko.utils.stringifyJson(plainJavaScriptObject, replacerFunction, spacer || 2);
	cache = null; // Enable garbage collection
	return output;

	function cycleReplacer(key, value){ 
		if (['entityAspect', 'entityType', '_$typeName'].indexOf(key) !== -1)
		{
			return;
		}

		if(typeof value === 'object' && value !== null)
		{
			if (cache.indexOf(value) !== -1)
			{
				return; // Circular reference found, discard key
			}
			cache.push(value);
		}
		return value;
	};
};


ko.bindingHandlers.dumpObservable = {
	init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		var context = valueAccessor();
		var allBindings = allBindingsAccessor();
		var pre = document.createElement('pre');
		element.appendChild(pre);

		var dumpJSON = ko.computed({
			read: function()
			{
				var en = allBindings.enable === undefined || allBindings.enable;
				return en ? observableToJson(context, null, 2) : '';
			},
			disposeWhenNodeIsRemoved : element
		});

		ko.applyBindingsToNode(pre, { text: dumpJSON, visible: dumpJSON });

		return {controlsDescendentBindings : true };
	}
};