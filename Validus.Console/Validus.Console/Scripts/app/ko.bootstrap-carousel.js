
/*
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
			domPanes = $("div.carousel-inner:first", element),
			domId = $(element).attr("id");

		if ($("li.active", domIndicators).length === 0)
		    $("li:first", domIndicators).addClass("active");

		if ($("div.item.active", domPanes).length === 0)
	        $("div.item:first", domPanes).addClass("active");

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