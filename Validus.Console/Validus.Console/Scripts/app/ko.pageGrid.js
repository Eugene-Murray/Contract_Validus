(function () {
    // Private function
    function getColumnsForScaffolding(data) {
        if ((typeof data.length !== 'number') || data.length === 0) {
            return [];
        }
        var columns = [];
        for (var propertyName in data[0]) {
            columns.push({ headerText: propertyName, rowText: propertyName });
        }
        return columns;
    }

    ko.pageGrid = {
        // Defines a view model class you can use to populate a grid
        ViewModel: function (configuration) {
            this.parentViewModel = configuration.parentViewModel;
            this.data = configuration.data;
            this.currentPageIndex = ko.observable(0);
            this.pageSize = configuration.pageSize || 5;
            this.caption = configuration.caption || "--";

            // If you don't specify columns configuration, we'll use scaffolding
            this.columns = configuration.columns || getColumnsForScaffolding(ko.utils.unwrapObservable(this.data));

            this.itemsOnCurrentPage = ko.computed(function () {
                if (this.data == undefined) return 0;
                var startIndex = this.pageSize * this.currentPageIndex();
                return this.data.slice(startIndex, startIndex + this.pageSize);
            }, this);

            this.maxPageIndex = ko.computed(function () {
                if (this.data == undefined) return 0;
                return Math.ceil(ko.utils.unwrapObservable(this.data).length / this.pageSize) - 1;
            }, this);

            this.rowClicked = function(item) {
              
            };
            this.rowClicked = configuration.rowClicked || this.rowClicked;
        }
    };

    // Templates used to render the grid
    var templateEngine = new ko.nativeTemplateEngine();

    templateEngine.addTemplate = function (templateName, templateMarkup) {
        document.write("<script type='text/html' id='" + templateName + "'>" + templateMarkup + "<" + "/script>");
    };

    templateEngine.addTemplate("ko_pageGrid_grid", "\
                    <table class=\"ko-grid table table-condensed table-striped table-bordered table-hover\" cellspacing=\"0\">\
                     <caption><span data-bind=\"text: caption\"></span></caption>\
                        <thead>\
                            <tr data-bind=\"foreach: columns\">\
                               <th data-bind=\"text: headerText\"></th>\
                            </tr>\
                        </thead>\
                        <tbody data-bind=\"foreach: itemsOnCurrentPage\">\
                           <tr data-bind=\"foreach: $parent.columns\">\
                               <td data-bind=\"text: typeof rowText == 'function' ? rowText($parent) : $parent[rowText],click: function() { $root.rowClicked($parent) } \"></td>\
                            </tr>\
                        </tbody>\
                    </table>");
    templateEngine.addTemplate("ko_pageGrid_pageLinks", "\
                    <div class=\"pagination\"> <ul >\
                        <!-- ko foreach: ko.utils.range(0, maxPageIndex) -->\
                              <li><a href=\"#\" data-bind=\"text: $data + 1, click: function() { $root.currentPageIndex($data) }, css: { selected: $data == $root.currentPageIndex() }\">\
                            </a></li>\
                        <!-- /ko -->\
                    </ul></div>");

    // The "simpleGrid" binding
    ko.bindingHandlers.pageGrid = {
        init: function () {
            return { 'controlsDescendantBindings': true };
        },
        // This method is called to initialize the node, and will also be called again if you change what the grid is bound to
        update: function (element, valueAccessor, allBindingsAccessor,parentViewModel) {
            var value = valueAccessor(), allBindings = allBindingsAccessor();
            var viewModel = new ko.pageGrid.ViewModel(
                {
                    parentViewModel:parentViewModel,
                    data: value,
                    pageSize: allBindings.pageSize || 4,
                    caption: allBindings.caption || "--",
                    rowClicked: allBindings.rowClicked ||  function () {
                        alert("click event not handled");
                    },
                    columns: allBindings.columns || undefined
                });

            // Empty the element
            while (element.firstChild)
                ko.removeNode(element.firstChild);

            // Allow the default templates to be overridden
            var gridTemplateName = allBindings.pageGridTemplate || "ko_pageGrid_grid",
                pageLinksTemplateName = allBindings.pageGridPagerTemplate || "ko_pageGrid_pageLinks";

            // Render the main grid
            var gridContainer = element.appendChild(document.createElement("DIV"));
            ko.renderTemplate(gridTemplateName, viewModel, { templateEngine: templateEngine }, gridContainer, "replaceNode");

            // Render the page links
            var pageLinksContainer = element.appendChild(document.createElement("DIV"));
            ko.renderTemplate(pageLinksTemplateName, viewModel, { templateEngine: templateEngine }, pageLinksContainer, "replaceNode");
        }
    };
})();