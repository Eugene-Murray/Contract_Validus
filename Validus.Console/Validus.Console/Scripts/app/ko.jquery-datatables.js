
ko.bindingHandlers["dataTable2"] = {
	"init": function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		var binding = ko.utils.unwrapObservable(valueAccessor()),
			innerBindingContext = bindingContext.extend(valueAccessor());

		$(element).dataTable(
		{
			"sDom": "<'row-fluid'<'span12't>>",
			"bLengthChange": false,
			"bFilter": false,
			"bInfo": false,
			"fnRowCallback": function(nRow, aData, iIndex)
			{
				$("td:eq(0)", nRow).attr("data-bind", "text: Model.Options()[" + iIndex + "].VersionTitle");
				$("td:eq(1)", nRow).attr("data-bind", "text: Model.Options()[" + iIndex + "].CurrentVersion().Quotes()[0].LimitAmount");
				$("td:eq(2)", nRow).attr("data-bind", "text: Model.Options()[" + iIndex + "].CurrentVersion().Quotes()[0].ExcessAmount");
				$("td:eq(3)", nRow).attr("data-bind", "text: Model.Options()[" + iIndex + "].CurrentVersion().Quotes()[0].QuotedPremium");
				$("td:eq(4)", nRow).attr("data-bind", "text: Model.Options()[" + iIndex + "].CurrentVersion().Quotes()[0].Brokerage");

				ko.applyBindingsToDescendants(innerBindingContext, nRow);

				return nRow;
			},
			/*"aoColumnDefs": [
				{
					"mRender": function(aData, sType, nRow)
					{
						toastr["success"](sType + " - " + aData + " - " + JSON.stringify(nRow));

						$("td:eq(0)", nRow).attr("data-bind", "text: Test1");

						return aData;
					},
					"aTargets": [0]
				}
			],*/
			"aoColumns": [
				{ "sTitle": "Option", "mData": "VersionTitle" },
				{ "sTitle": "Limit", "mData": "LimitAmount" },
				{ "sTitle": "Excess", "sName": "Excess", "sClass": "Excess", "mData": "ExcessAmount" },
				{ "sTitle": "Premium", "mData": "QuotedPremium" },
				{ "sTitle": "Brokerage", "mData": "Brokerage" }
			]
		});

		if (binding)
		{
			$(element).dataTable().fnClearTable();

			var data = [];

			for (var optionIter in binding)
			{
				var title = binding[optionIter].VersionTitle();
				var version = ko.utils.unwrapObservable(binding[optionIter].CurrentVersion);
				var quotes = ko.utils.unwrapObservable(version.Quotes);

				for (var quoteIter in quotes)
				{
					var quote = ko.utils.unwrapObservable(quotes[quoteIter]);

					data.push(
					{
						"VersionTitle": title,
						"ExcessAmount": ko.utils.unwrapObservable(quote.ExcessAmount),
						"LimitAmount": ko.utils.unwrapObservable(quote.LimitAmount),
						"QuotedPremium": ko.utils.unwrapObservable(quote.QuotedPremium),
						"Brokerage": ko.utils.unwrapObservable(quote.Brokerage)
					});

					break; // DEBUG: Only one quote for the moment
				}
			}

			$(element).dataTable().fnAddData(data);
		}
	},
	"update": function(element, valueAccessor, allBindingsAccessor, viewModel)
	{
		var binding = ko.utils.unwrapObservable(valueAccessor());

		if (binding)
		{
			//toastr["info"]("KO: " + binding.length.toString() + " DT: " + $(element).dataTable().fnSettings().fnRecordsTotal().toString());
			
			/*
			var ex = document.getElementById('example');

			if ( ! $.fn.DataTable.fnIsDataTable( ex ) )
			{
			  $(ex).dataTable();
			}
			*/

			if (binding.length != $(element).dataTable().fnSettings().fnRecordsTotal())
			{
				$(element).dataTable().fnClearTable();

				var data = [];

				for (var optionIter in binding)
				{
					var title = binding[optionIter].VersionTitle();
					var version = ko.utils.unwrapObservable(binding[optionIter].CurrentVersion);
					var quotes = ko.utils.unwrapObservable(version.Quotes);

					for (var quoteIter in quotes)
					{
						var quote = ko.utils.unwrapObservable(quotes[quoteIter]);

						data.push(
						{
							"VersionTitle": title,
							"ExcessAmount": ko.utils.unwrapObservable(quote.ExcessAmount),
							"LimitAmount": ko.utils.unwrapObservable(quote.LimitAmount),
							"QuotedPremium": ko.utils.unwrapObservable(quote.QuotedPremium),
							"Brokerage": ko.utils.unwrapObservable(quote.Brokerage)
						});

						break; // DEBUG: Only one quote for the moment
					}
				}

				$(element).dataTable().fnAddData(data);
			}
		}
	}
};

ko.bindingHandlers["dataTable"] = {
	"init": function(element, valueAccessor)
	{
		var binding = ko.utils.unwrapObservable(valueAccessor());

		// If the binding is an object with an options field,
		// initialise the dataTable with those options. 
		if (binding.options)
		{
			$(element).dataTable(binding.options);
		}
	},
	update: function(element, valueAccessor)//, allBindingsAccessor)
	{
		var binding = ko.utils.unwrapObservable(valueAccessor()[0]());

		// If the binding isn't an object, turn it into one. 
		if (!binding.data)
		{
			binding = { data: valueAccessor() };
		}

		// Clear table
		$(element).dataTable().fnClearTable();

		// Rebuild table from data source specified in binding
		$(element).dataTable().fnAddData(binding.data());
	}
};