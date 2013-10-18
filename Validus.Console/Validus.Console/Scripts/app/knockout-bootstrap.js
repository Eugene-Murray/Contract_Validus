
/*
	Knockout Bootstrap-Tabs Binding Handler

	Description;
		This is a custom binding handler for the Twitter Bootstrap Tabs component (designed for 
		Bootstrap versions 2 and 3).

	Known Issue;
		Requires the knockout-{version}.debug.js script file to be included instead of the minified
		knockout-{version}.js, because referenced objects have been obfuscated in the minified
		version.

	References;
		http://getbootstrap.com/2.3.2/components.html#navs
	
	Usage;
		<div data-bind="bootstrap-tabs: { data: ObservableArray, afterRender: OptionalFunction, afterTabRender: OptionalFunction }">
			<ul class="nav nav-tabs">
				<li>
                    <a data-toggle="tab" href='#'>
                        <span data-bind="text: Observable"></span>
                    </a>
				</li>
			</ul>
			<div class="tab-content">
				<div class="tab-pane">
					<span data-bind="text: Observable"></span>
				</div>
			</div>
		</div>
*/
ko.bindingHandlers["bootstrap-tabs"] = {
	"init": function(element, valueAccessor, bindingsAccessor, viewModel, bindingContext)
	{
		var domId = ko.bindingHandlers["bootstrap-tabs"]["uid"](element),
		    domTabs = $("ul.nav.nav-tabs:first", element),
		    domPanes = $("div.tab-content:first", element),
		    domTabTemplate = domTabs.children("li"),
		    domPaneTemplate = domPanes.children("div.tab-pane"),
		    koTabTemplate = ko.utils.moveCleanedNodesToContainerElement(domTabTemplate),
		    koPaneTemplate = ko.utils.moveCleanedNodesToContainerElement(domPaneTemplate),
			koAccessor = ko.utils.unwrapObservable(valueAccessor());

		ko.bindingHandlers["bootstrap-tabs"]["tab-count"][domId] = 0;
		ko.bindingHandlers["bootstrap-tabs"]["tab-template"] = koTabTemplate;
		ko.bindingHandlers["bootstrap-tabs"]["pane-template"] = koPaneTemplate;

		new ko.templateSources.anonymousTemplate(koTabTemplate)["nodes"](koTabTemplate);
		new ko.templateSources.anonymousTemplate(koPaneTemplate)["nodes"](koPaneTemplate);

		ko.renderTemplateForEach(koTabTemplate, koAccessor.data(), {}, domTabs.get(0), bindingContext);
		ko.renderTemplateForEach(koPaneTemplate, koAccessor.data(), {}, domPanes.get(0), bindingContext);

		ko.bindingHandlers["bootstrap-tabs"]["init-tabs"](element, koAccessor, domId, domTabs, domPanes);

		return { "controlsDescendantBindings": true };
	},
	"update": function(element, valueAccessor, bindingsAccessor, viewModel, bindingContext)
	{
		var domId = ko.bindingHandlers["bootstrap-tabs"]["uid"](element),
			domTabs = $("ul.nav.nav-tabs:first", element),
		    domPanes = $("div.tab-content:first", element),
			koTabTemplate = ko.bindingHandlers["bootstrap-tabs"]["tab-template"],
			koPaneTemplate = ko.bindingHandlers["bootstrap-tabs"]["pane-template"],
			koAccessor = ko.utils.unwrapObservable(valueAccessor()),
		    koObservable = ko.utils.unwrapObservable(koAccessor.data()); // DON'T REMOVE: koObservable creates a dependency

		ko.renderTemplateForEach(koTabTemplate, koAccessor.data(), {}, domTabs.get(0), bindingContext);
		ko.renderTemplateForEach(koPaneTemplate, koAccessor.data(), {}, domPanes.get(0), bindingContext);

		ko.bindingHandlers["bootstrap-tabs"]["init-tabs"](element, koAccessor, domId, domTabs, domPanes);

		return { "controlsDescendantBindings": true };
	},
	"init-tabs": function(element, koAccessor, domId, domTabs, domPanes)
	{
		$("li a[data-toggle='tab'][href='#']", domTabs).each(function(index, item)
		{
			var tabCount = ko.bindingHandlers["bootstrap-tabs"]["tab-count"][domId],
				tabId = domId + "-" + tabCount,
			    domPane = $("div.tab-pane:not([id]):first", domPanes).attr("id", tabId);

			$(this).attr("href", "#" + tabId).click(function(e)
			{
				e.preventDefault();

				$(this).tab("show");
			});

			if (koAccessor.afterTabRender) koAccessor.afterTabRender($(this), domPane);

			ko.bindingHandlers["bootstrap-tabs"]["tab-count"][domId] = tabCount + 1;
		});

		$("li:last a[data-toggle='tab']", domTabs).tab("show");

		if (koAccessor.afterRender) koAccessor.afterRender($(element));
	},
	"uid": function(element)
	{
		var uid = element ? $(element).attr("id") : null;

		if (!uid)
		{
			uid = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(c)
			{
				var r = Math.random() * 16 | 0, v = c == "x" ? r : (r & 0x3 | 0x8);

				return v.toString(16);
			});

			uid = $("#" + uid).length > 0 ? ko.bindingHandlers["bootstrap-tabs"]["uid"](element) : uid;

			if (element) $(element).attr("id", uid);
		}

		return uid;
	},
	"tab-template": null,
	"pane-template": null,
	"tab-count": []
};


/* TODO: Clean up a little bit 
	Knockout Bootstrap-Carousel Binding Handler

	Description;
		This is a custom binding handler for the Twitter Bootstrap Carousel

	Known Issue;
		Requires the knockout-{version}.debug.js script file to be included instead of the minified
		knockout-{version}.js, because referenced objects have been obfuscated

	References;
		http://twitter.github.com/bootstrap/javascript.html#carousel
	
	Usage;
		<div data-bind="bootstrap-carousel: { data: ObservableArray, afterRender: Function }">
			<ol class="carousel-indicators">
				<li data-bind="text: Observable"></li>
			</ol>
			<div class="carousel-inner">
				<div class="item">
					<span data-bind="text: Observable"></span>
				</div>
			</div>
			<a class="carousel-control left" href="#" data-slide="prev">&lsaquo;</a>
			<a class="carousel-control right" href="#" data-slide="next">&rsaquo;</a>
		</div>
*/
ko.bindingHandlers["bootstrap-carousel"] = {
	"indicator-template": null,
	"pane-template": null,
	"init": function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		var domId = ko.bindingHandlers["bootstrap-carousel"]["generate-uid"](),
			domIndicators = $("ol.carousel-indicators:first", element),
			domPanes = $("div.carousel-inner:first", element),
			domIndicatorTemplate = domIndicators.children("li"),
			domPaneTemplate = domPanes.children("div.item"),
			koIndicatorTemplate = ko.utils.moveCleanedNodesToContainerElement(domIndicatorTemplate),
			koPaneTemplate = ko.utils.moveCleanedNodesToContainerElement(domPaneTemplate),
			koValue = ko.utils.unwrapObservable(valueAccessor());

		ko.bindingHandlers["bootstrap-carousel"]["indicator-template"] = koIndicatorTemplate;
		ko.bindingHandlers["bootstrap-carousel"]["pane-template"] = koPaneTemplate;

		new ko.templateSources.anonymousTemplate(koIndicatorTemplate)["nodes"](koIndicatorTemplate);
		new ko.templateSources.anonymousTemplate(koPaneTemplate)["nodes"](koPaneTemplate);

		ko.renderTemplateForEach(koIndicatorTemplate, koValue.data(), {}, domIndicators.get(0), bindingContext);
		ko.renderTemplateForEach(koPaneTemplate, koValue.data(), {}, domPanes.get(0), bindingContext);

		$(element).attr("id", domId).addClass("carousel slide");

		$("a.carousel-control.left", element).attr("data-slide", "next").attr("href", "#" + domId).addClass("hide");
		$("a.carousel-control.right", element).attr("data-slide", "prev").attr("href", "#" + domId).addClass("hide");

		$("li:first", domIndicators).addClass("active");
		$("div.item:first", domPanes).addClass("active");

		ko.bindingHandlers["bootstrap-carousel"]["init-carousel"](element);

		koValue.afterRender(element);

		return { "controlsDescendantBindings": true };
	},
	"update": function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext)
	{
		var koValue = ko.utils.unwrapObservable(valueAccessor()),
		    // koObservable is needed to create a dependency (update subscription to the observable)
			koObservable = ko.utils.unwrapObservable(koValue.data()),
			domIndicators = $("ol.carousel-indicators:first", element),
			domPanes = $("div.carousel-inner:first", element),
			koIndicatorTemplate = ko.bindingHandlers["bootstrap-carousel"]["indicator-template"],
			koPaneTemplate = ko.bindingHandlers["bootstrap-carousel"]["pane-template"];

		ko.renderTemplateForEach(koIndicatorTemplate, koValue.data(), {}, domIndicators.get(0), bindingContext);
		ko.renderTemplateForEach(koPaneTemplate, koValue.data(), {}, domPanes.get(0), bindingContext);

		ko.bindingHandlers["bootstrap-carousel"]["init-carousel"](element);

		if (domIndicators.children("li").length > 1)
		{
			$("a.carousel-control.left", element).removeClass("hide");
			$("a.carousel-control.right", element).removeClass("hide");
		}

		$(element).data("carousel").to($("[data-slide-to]:last", domIndicators).attr("data-slide-to")).cycle();

		koValue.afterRender(element);

		return { "controlsDescendantBindings": true };
	},
	"init-carousel": function(element)
	{
		var domIndicators = $("ol.carousel-indicators:first", element),
			/*domPanes = $("div.carousel-inner:first", element),*/
			domId = $(element).attr("id");

		domIndicators.children("li").each(function(index, item)
		{
			$(this).attr("data-target", "#" + domId).attr("data-slide-to", index);
		});

		$(element).carousel({ interval: -1 });
	},
	"generate-uid": function()
	{
		var uid = $.generateGuid();

		return ($("#" + uid).length > 0) ? ko.bindingHandlers["bootstrap-carousel"]["generate-uid"]() : uid;
	},
	"log": function(element, index, value)
	{
		console.log(element);
		console.log(index);
		console.log(ko.utils.peekObservable(value));
	}
};