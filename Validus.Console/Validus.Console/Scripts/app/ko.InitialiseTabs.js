// TODO: turn into a binding handler
var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

ConsoleApp.InitialiseTabs = function(element, domId, submissionBaseVm)
{
	var tabPanel = $(element).parents(".tabbable:first"),
		tabNav = $(".nav-tabs:first", tabPanel),
		tabContent = $(".tab-content:first", tabPanel);
	var prevtabIndex = -1;
	$(tabNav).children("li").children("a[data-toggle]").each(function(optionIndex) {

	    if ($(this).attr("data-target")) {
	        prevtabIndex = Number($(this).data("target").replace("#"+domId + "-option", ""));
	        return;
	    }
	        
	    
	    if ($(this).context.innerText == "Market Wordings"
	        || $(this).context.innerText == "Terms and Conditions"
	        || $(this).context.innerText == "Subjectivities") {
	        var fixedTabId = domId + "-Wording" + optionIndex;
	        $(".tab-pane:eq(" + optionIndex + ")", tabContent).attr("id", fixedTabId);
	        $(this).attr("data-target", "#" + fixedTabId)
				.click(function (e) {
				    e.preventDefault();

				    $(this).tab("show");
				}).tab();
	        return;
	    }
	    var newOptionIndex = prevtabIndex + 1;
	    prevtabIndex = newOptionIndex;
	    var tabId = domId + "-option" + newOptionIndex;

		$(".tab-pane:eq(" + optionIndex + ")", tabContent).attr("id", tabId);

		if ($(this).attr("data-toggle") === "dropdown")
		{
			$(this).dropdown();

			$("a[data-toggle]", $(this).siblings(".dropdown-menu")).each(function(versionIndex)
			{
				if ($(this).attr("data-target")) return;

				$(this).attr("data-target", "#" + tabId)
					.click(function(e)
					{
						e.preventDefault();

						self.Options()[optionIndex].VersionIndex(versionIndex);

						$(this).tab("show");
					}).tab("show");
			});
		}
		else if ($(this).attr("data-toggle") === "tab")
		{
			submissionBaseVm.OptionIndexToCopy(optionIndex);
			$(this).attr("data-target", "#" + tabId)
				.click(function(e)
				{
					e.preventDefault();

					$(this).tab("show");
				}).tab("show");
		}

		$(this).attr("data-target", "#" + tabId).on('show', function(e)
		{
			submissionBaseVm.OptionIndexToCopy(optionIndex);
		});
	});
};