
// TODO: Remove Typeaheads altogether and replace with jQuery Combobox

var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.InitialiseTypeaheads = function(element, submission)
{
	$(".typeahead[data-rest]", element).each(function()
	{
		if (!$(this).data("val-typeahead"))
		{
			var apiController = $(this).attr("data-rest"),
				labelFieldName = "Name",
				valueFieldName = "Name",
				onSuccess = function(callback, data)
				{
					callback(data);
				};

			switch (apiController)
			{
				case "Broker": // Title = Code : Psu : Name : GrpCd, Value = Code
					{
						labelFieldName = "Title";
						valueFieldName = "Value";
						onSuccess = function(callback, data)
						{
							var dataArray = [];

							$(data).each(function(index, item)
							{
								dataArray.push(
								{
									Title: item.Code
										+ " : " + item.Psu
										+ " : " + item.Name
										+ " : " + item.Id
										+ " : " + item.GrpCd,
									Value: item.Code
								});
							});

							callback(dataArray);
						};
					} break;
					case "BrokerContact": // TODO: Why was this added, can't use default ?
						{
							labelFieldName = "Title";
							valueFieldName = "Value";
							onSuccess = function(callback, data)
							{
								var dataArray = [];

								$(data).each(function(index, item)
								{
									dataArray.push(
									{
										Title: item.Name,
										Value: item.Name
									});
								});

								callback(dataArray);
							};
						} break;
				case "COB":
				case "Domicile":
				case "NonLondonBroker":
				case "Office":
				case "Underwriter": // Title = Code : Name, Value = Code
					{
						labelFieldName = "Title";
						valueFieldName = "Value";
						onSuccess = function(callback, data)
						{
							var dataArray = [];

							$(data).each(function(index, item)
							{
								dataArray.push(
								{
									Title: item.Code + " : " + item.Name,
									Value: item.Code
								});
							});

							callback(dataArray);
						};
					} break;
				case "Insured": // Title = Name : Id, Value = Name
					{
						labelFieldName = "Title";
						valueFieldName = "Value";
						onSuccess = function(callback, data)
						{
							var dataArray = [];

							$(data).each(function(index, item)
							{
								dataArray.push(
								{
									Title: item.Name + " : " + item.Id,
									Value: item.Name
								});
							});

							callback(dataArray);
						};
					} break;
				case "Currency": // Title = Psu : Name, Value = Psu
					{
						labelFieldName = "Title";
						valueFieldName = "Value";
						onSuccess = function(callback, data)
						{
							var dataArray = [];

							$(data).each(function(index, item)
							{
								dataArray.push(
								{
									Title: item.Psu + " : " + item.Name,
									Value: item.Psu
								});
							});

							callback(dataArray);
						};
					} break;
				case "Facility": // Title = Reference : Description, Value = Reference
					{
						labelFieldName = "Title";
						valueFieldName = "Value";
						onSuccess = function(callback, data)
						{
							var dataArray = [];

							$(data).each(function(index, item)
							{
								dataArray.push(
								{
									Title: item.Reference + " : " + item.Description,
									Value: item.Reference
								});
							});

							callback(dataArray);
						};
					} break;
				case "MOA": // Title = Code : Description, Value = Code
					{
						labelFieldName = "Title";
						valueFieldName = "Value";
						onSuccess = function(callback, data)
						{
							var dataArray = [];

							$(data).each(function(index, item)
							{
								dataArray.push(
								{
									Title: item.Code + " : " + item.Description,
									Value: item.Code
								});
							});

							callback(dataArray);
						};
					} break;
				case "Leader": // Code
					{
						labelFieldName = "Code";
						valueFieldName = "Code";
					} break;
				default: break; // Name
			}

			$(this).typeahead(
			{
				labelField: labelFieldName,
				valueField: valueFieldName,
				source: function(query, limit, callback) // TODO: Implement limit
				{
					var dataQuery = { term: query, skip: 0, take: 12 };

					if (apiController === "BrokerContact")
					{
					    var brokerCode = ko.utils.peekObservable(submission.Model.BrokerCode);

					    if (brokerCode)
					        dataQuery.brokerCode = brokerCode;
					}
					else if (apiController === "NonLondonBroker")
					{
						var officeId = ko.utils.peekObservable(submission.Model.QuotingOfficeId);

						if (officeId)
							dataQuery.office = officeId;
					}
					else if (apiController === "Facility")
					{
						var currentQuote = submission.CurrentOption().CurrentQuote(),
						    accountYear = ko.utils.peekObservable(currentQuote.AccountYear),
						    cobCode = ko.utils.peekObservable(currentQuote.COBId),
						    officeCode = ko.utils.peekObservable(submission.Model.QuotingOfficeId);

						if (accountYear)
							dataQuery.accountYear = accountYear;
						
						if (cobCode)
							dataQuery.cobCode = cobCode;
						
						if (officeCode)
							dataQuery.officeCode = officeCode;
					}
					
					$.ajax(
					{
						url: window.ValidusServicesUrl + apiController,
						data: dataQuery,
						dataType: "json",
						contentType: "application/json",
						success: function(data)
						{
							onSuccess(callback, data);
						}
					});
				}
			});

			$(this).data("val-typeahead", true);
		}
	});
};