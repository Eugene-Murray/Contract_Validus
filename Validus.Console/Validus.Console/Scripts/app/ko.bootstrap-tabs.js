
ko.bindingHandlers["bootstrap-tabs"] = {
	"init": function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		var domId = ko.bindingHandlers["bootstrap-tabs"]["generate-uid"](),
			domTabs = $("ul.nav.nav-tabs:first", element),
			domPanes = $("div.tab-content:first", element),
			domTabTemplate = domTabs.children("li"),
			domPaneTemplate = domPanes.children("div.tab-pane"),
			koTabTemplate = ko.utils.moveCleanedNodesToContainerElement(domTabTemplate),
			koPaneTemplate = ko.utils.moveCleanedNodesToContainerElement(domPaneTemplate);

		$(element).attr("id", domId);

		ko.bindingHandlers["bootstrap-tabs"]["tab-count"][domId] = 0;

		new ko.templateSources.anonymousTemplate(koTabTemplate)["nodes"](koTabTemplate);
		new ko.templateSources.anonymousTemplate(koPaneTemplate)["nodes"](koPaneTemplate);

		ko.renderTemplateForEach(koTabTemplate, valueAccessor(), {}, domTabs.get(0), bindingContext);
		ko.renderTemplateForEach(koPaneTemplate, valueAccessor(), {}, domPanes.get(0), bindingContext);

		ko.bindingHandlers["bootstrap-tabs"]["init-tabs"](element);

		$("li:first a[data-toggle='tab']", domTabs).tab("show");

		return { "controlsDescendantBindings": true };
	},
	"update": function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		ko.utils.unwrapObservable(valueAccessor());

		ko.bindingHandlers["bootstrap-tabs"]["init-tabs"](element);

		return { "controlsDescendantBindings": true };
	},
	"init-tabs": function(element)
	{
		var domTabs = $("ul.nav.nav-tabs:first", element),
			domPanes = $("div.tab-content:first", element),
			domId = $(element).attr("id");

		$("li a[data-toggle='tab'][href='#']", domTabs).each(function(index, item)
		{
			var tabCount = ko.bindingHandlers["bootstrap-tabs"]["tab-count"][domId],
				tabId = domId + "-" + tabCount;

			$("div.tab-pane:not([id]):first", domPanes).attr("id", tabId);

			$(this).attr("href", "#" + tabId).click(function(e)
			{
				e.preventDefault();

				$(this).tab("show");
			});

			ko.bindingHandlers["bootstrap-tabs"]["tab-count"][domId] = tabCount + 1;
		});
	},
	"generate-uid": function()
	{
		var uid = $.generateGuid();

		return ($("#" + uid).length > 0) ? ko.bindingHandlers["bootstrap-tabs"]["generate-uid"]() : uid;
	},
	"tab-count": [],
	"log": function(element, index, value)
	{
		console.log(element);
		console.log(index);
		console.log(value());
	}
};


ko.bindingHandlers["bootstrap-tabs-init"] = {
	"init": function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		var domId = ko.bindingHandlers["bootstrap-tabs-init"]["generate-uid"](),
			domTabs = $("ul.nav.nav-tabs:first", element),
			domPanes = $("div.tab-content:first", element);

		$(element).attr("id", domId);

		ko.bindingHandlers["bootstrap-tabs-init"]["tab-count"][domId] = 0;

		$("li a[data-toggle='tab'][href='#']", domTabs).each(function(index, item)
		{
			var tabCount = ko.bindingHandlers["bootstrap-tabs-init"]["tab-count"][domId],
				tabId = domId + "-" + tabCount;

			$("> div.tab-pane:not([id]):first", domPanes).attr("id", tabId);

			$(this).attr("href", "#" + tabId).click(function(e)
			{
				e.preventDefault();

				$(this).tab("show");
			});

			ko.bindingHandlers["bootstrap-tabs-init"]["tab-count"][domId] = tabCount + 1;
		});

		$("li:first a[data-toggle='tab']", domTabs).tab("show");
	},
	"generate-uid": function()
	{
		var uid = $.generateGuid();

		return ($("#" + uid).length > 0) ? ko.bindingHandlers["bootstrap-tabs-init"]["generate-uid"]() : uid;
	},
	"tab-count": []
};