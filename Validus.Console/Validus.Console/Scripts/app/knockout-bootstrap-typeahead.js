
/*
	Knockout Bootstrap Typeahead Handler

	Description;
		This is a custom binding handler for the customised auto-numeric component extension for currency text inputs.

	References;
		
	
	Usage;
		Usage;
		<input type="text" data-bind="auto-numeric: { data: Observable, options: {  labelField: 'Title',
                                                                                    valueField: 'Value',
                                                                                   },callback:function(data)
                                                                                        {
                                                                                            $(data).each(function (index, item) {
                                                                                                dataArray.push(
                                                                                                {
                                                                                                    Title: item.Code + ' : ' + item.Name,
                                                                                                    Value: item.Code
                                                                                                });
                                                                                            });
                                                                                            return dataArray;
                                                                                        }} " />
*/
ko.bindingHandlers["bootstrap-typeahead"] = {
    "init": function (element, valueAccessor, bindingsAccessor, viewModel, bindingContext) {
        var koAccessor = ko.utils.unwrapObservable(valueAccessor()),
			inputOptions = {//defaults
			};

        if (koAccessor.options) {
            for (var option in koAccessor.options) {
                inputOptions[option] = koAccessor.options[option];
            }
        }

        inputOptions.source = function(query, limit, process) {
            var dataQuery = { term: query, skip: 0, take: 12 };
            $.ajax(
                {
                    url: koAccessor.serviceurl,
                    data: dataQuery,
                    dataType: 'json',
                    contentType: 'application/json',
                    success: function(data) {
                        var dataArray = koAccessor.callback(data);
                        process(dataArray);
                    }
                });
        };
        
        inputOptions.updater = function (item) {
            koAccessor.data(item.data("value"));
            return item;
        };

        $(element).typeahead(inputOptions);

    },
    "update": function (element, valueAccessor, bindingsAccessor, viewModel, bindingContext) {
        var koAccessor = ko.utils.unwrapObservable(valueAccessor()),
            query = koAccessor.data(),
            inputOptions = {//defaults
            };
        if (koAccessor.options) {
            for (var option in koAccessor.options) {
                inputOptions[option] = koAccessor.options[option];
            }
        }

        if ($(element).is("input[type='text']")) {
            if (!!query && query.length > 0) {
                var dataQuery = { code: query };

                $.ajax(
                    {
                        url: koAccessor.serviceurl,
                        data: dataQuery,
                        dataType: 'json',
                        contentType: 'application/json',
                        success: function(data) {
                            var dataArray = koAccessor.callback(data);
                            $(element).val(dataArray[0][inputOptions.labelField]);
                        }
                    });
            }
        }
    }
};