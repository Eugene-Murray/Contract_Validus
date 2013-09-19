$(function () {

    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

    ConsoleApp.vmManageQuoteTemplates = new function () {
        var self = this;

        self.teamList = ko.observableArray();
        self.allQuoteTemplatesList = ko.observableArray([new ConsoleApp.QuoteTemplate()]);
        self.QuoteTemplatesForTeamSelectList = ko.observableArray();
        self.teamQuoteTemplatesList = ko.observableArray([]);
        self.selectedTeam = ko.observable();
        self.showImageProcessing_LoadingQuoteTemplates = ko.observable('block');
        self.editQuoteTemplateButton_Status = ko.observable(false);
        self.saveTeamQuoteTemplatesButton_Status = ko.observable(false);
        self.createButtonVisible = ko.observable(true);
        self.editButtonVisible = ko.observable(true);
        self.createEditTitle = ko.observable(true);
        // Selected
        self.selectedTeamQuoteTemplate = ko.observable(new ConsoleApp.QuoteTemplate());
        self.selectedAllQuoteTemplate = ko.observable(new ConsoleApp.QuoteTemplate());
        self.selectedQuoteTemplateForTeamSelectList = ko.observable();


        self.Initialize = function () {
            self.GetQuoteTemplates();
            self.GetTeamsBasicData();
        };

        self.SaveNewQuoteTemplate = function () {

            var data = ko.toJSON(self.selectedAllQuoteTemplate());

            var ajaxConfig = { Url: "/Admin/CreateQuoteTemplate", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                self.allQuoteTemplatesList.unshift(new ConsoleApp.QuoteTemplate()
                        .Id(data.Id)
                        .Name(data.Name)
                        .RdlPath(data.RdlPath));

                self.selectedAllQuoteTemplate(new ConsoleApp.QuoteTemplate());
                toastr["info"]("QuoteTemplate successfully created");

                $('#CreateEditQuoteTemplate_ModalAlert').modal('hide');
            });

            response.fail(function (jqXhr, textStatus) {
                toastr["error"]("An Error Has Occurred!");
                console.log(jqXhr + " " + textStatus);
            });
        };

        self.EditQuoteTemplate = function () {

            var data = ko.toJSON(self.selectedAllQuoteTemplate());

            var ajaxConfig = { Url: "/Admin/EditQuoteTemplate", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                toastr["info"]("QuoteTemplate successfully updated");
                $('#CreateEditQuoteTemplate_ModalAlert').modal('hide');
            });

            response.fail(function (jqXhr, textStatus) {
                toastr["error"]("An Error Has Occurred!");
                console.log(jqXhr + " " + textStatus);
            });
        };

        self.DeleteQuoteTemplate = function () {

            var data = ko.toJSON(self.selectedAllQuoteTemplate());

            var ajaxConfig = { Url: "/Admin/DeleteQuoteTemplate", VerbType: "DELETE", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                self.allQuoteTemplatesList.remove(function (item) { return item.Id == self.selectedAllQuoteTemplate().Id });

                toastr["info"]("QuoteTemplate successfully deleted");
            });

            response.fail(function (jqXhr, textStatus) {
                toastr["error"]("An Error Has Occurred!");
                console.log(jqXhr + " " + textStatus);
            });
        };


        self.GetQuoteTemplates = function () {

            self.allQuoteTemplatesList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetQuoteTemplates", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                if (data != null) {
                    $.each(data, function (key, value) {
                        self.allQuoteTemplatesList.push(new ConsoleApp.QuoteTemplate()
                            .Id(value.Id)
                            .Name(value.Name)
                            .RdlPath(value.RdlPath));
                    });
                }
                else {
                    toastr["info"]("No QuoteTemplates Found");
                }
                    

                self.showImageProcessing_LoadingQuoteTemplates('none');

            });

            response.fail(function (jqXhr, textStatus) {
                toastr["error"]("An Error Has Occurred!");
                console.log(jqXhr);
                console.log(textStatus);
            });

        };

        self.GetTeamsBasicData = function () {

            self.teamList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetTeamsBasicData", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                $.each(data, function (key, value) {
                    self.teamList.push({ key: value.Id, name: value.Title });
                });
            });

            response.fail(function (jqXhr, textStatus) {
                toastr["error"]("An Error Has Occurred!");
            });
        };

        self.GetQuoteTemplatesForTeam = function () {

            self.QuoteTemplatesForTeamSelectList.removeAll();
            self.teamQuoteTemplatesList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetQuoteTemplatesForTeam?teamId=" + self.selectedTeam(), VerbType: "GET", ContentType: "application/json;charset=utf-8" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                console.log(data);

                if (data.length > 0) {

                    var currentTeamQuoteTemplatesTempArray = ko.observableArray();
                    currentTeamQuoteTemplatesTempArray.removeAll();

                    $.each(data, function (key, value) {

                        self.QuoteTemplatesForTeamSelectList.push({ key: value.Id, name: value.Name });

                        currentTeamQuoteTemplatesTempArray.push(new ConsoleApp.QuoteTemplate()
                            .Id(value.Id)
                            .Name(value.Name)
                            .RdlPath(value.RdlPath));
                    });

                    var differenceList = ko.utils.compareArrays(currentTeamQuoteTemplatesTempArray(), self.allQuoteTemplatesList());

                    var diffResults = ko.observableArray();
                    ko.utils.arrayForEach(differenceList, function (difference) {
                        if (difference.status === "deleted") {
                            diffResults.push(difference.value.Id());
                        }
                    });

                    $.each(self.allQuoteTemplatesList(), function (key, value) {
                        if (diffResults.indexOf(value.Id()) == -1)

                            self.teamQuoteTemplatesList.push(new ConsoleApp.QuoteTemplate()
                                .Id(value.Id())
                                .Name(value.Name())
                                .RdlPath(value.RdlPath()));
                    });

                } else {
                    self.QuoteTemplatesForTeamSelectList.push({ key: 0, name: "No QuoteTemplates..." });

                    $.each(self.allQuoteTemplatesList(), function (key, value) {
                        //self.teamQuoteTemplatesList.push(value);
                        self.teamQuoteTemplatesList.push(new ConsoleApp.QuoteTemplate()
                                .Id(value.Id())
                                .Name(value.Name())
                                .RdlPath(value.RdlPath()));
                    });
                }

            });

            response.fail(function (jqXhr, textStatus) {
                toastr["error"]("An Error Has Occurred!");
                console.log(jqXhr + " " + textStatus);
            });
        };

        self.SaveQuoteTemplatesForTeam = function () {

            var teamQuoteTemplatesDto = new ConsoleApp.TeamQuoteTemplatesDto();
            teamQuoteTemplatesDto.TeamId(self.selectedTeam());

            $.each(self.QuoteTemplatesForTeamSelectList(), function (key, value) {
                teamQuoteTemplatesDto.TeamQuoteTemplatesIdList.push(value.key);
            });

            var data = ko.toJSON(teamQuoteTemplatesDto);

            var ajaxConfig = { Url: "/Admin/SaveQuoteTemplatesForTeam", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                toastr["info"]("Team QuoteTemplates successfully updated");
            });

            response.fail(function (jqXhr, textStatus) {
                toastr["error"]("An Error Has Occurred!");
                console.log(jqXhr + " " + textStatus);
            });
        };

        // Events
        self.click_RemoveQuoteTemplateFromTeam = function (e) {

            if (self.selectedQuoteTemplateForTeamSelectList() !== undefined) {
                var removeItem = ko.utils.arrayFirst(self.allQuoteTemplatesList(), function (item) {
                    return (item.Id() == self.selectedQuoteTemplateForTeamSelectList());
                });

                self.QuoteTemplatesForTeamSelectList.remove(function (item) { return item.key == self.selectedQuoteTemplateForTeamSelectList(); });

                self.teamQuoteTemplatesList.unshift(new ConsoleApp.QuoteTemplate()
                                .Id(removeItem.Id())
                                .Name(removeItem.Name())
                                .RdlPath(removeItem.RdlPath()));

            }
            else {
                alert("Select a QuoteTemplate to remove");
            }

        };

        self.click_AddQuoteTemplateToTeam = function (e) {

            self.QuoteTemplatesForTeamSelectList.remove(function (item) { return item.key == '0'; });

            if (self.selectedTeam() !== undefined) {

                if (self.QuoteTemplatesForTeamSelectList.indexOf(self.selectedTeamQuoteTemplate().Id()) === -1) {
                    self.QuoteTemplatesForTeamSelectList.push({ key: self.selectedTeamQuoteTemplate().Id(), name: self.selectedTeamQuoteTemplate().Name() });
                    self.teamQuoteTemplatesList.remove(self.selectedTeamQuoteTemplate());
                    self.selectedTeamQuoteTemplate(null);
                }
                else {
                    alert("QuoteTemplate already added...");
                }
            }
            else {
                alert("Select a team first...");
            }

        };

        self.onClickTeamQuoteTemplateItem = function (e) {
            self.selectedTeamQuoteTemplate(this);
        };

        self.onClickAllQuoteTemplatesItem = function (e) {

            self.selectedAllQuoteTemplate(this);
            console.log(self.selectedAllQuoteTemplate());
        };

        self.click_DeleteQuoteTemplate = function () {

            self.selectedAllQuoteTemplate(this);

            var result = confirm("Delete QuoteTemplate?");
            if (result) {
                self.DeleteQuoteTemplate();
            }
        };

        self.click_SaveTeamQuoteTemplates = function () {
            self.SaveQuoteTemplatesForTeam();
        };

        self.click_CancelCreateEdit = function () {
            self.selectedAllQuoteTemplate(new ConsoleApp.QuoteTemplate());
        };

        self.click_OpenCreateEditQuoteTemplateModal_Create = function (e) {
            self.createEditTitle("Create QuoteTemplate");
            self.selectedAllQuoteTemplate(new ConsoleApp.QuoteTemplate());
            self.createButtonVisible(true);
            self.editButtonVisible(false);
            $('#CreateEditQuoteTemplate_ModalAlert').modal('show');
        };

        self.click_OpenCreateEditQuoteTemplateModal_Edit = function (e) {
            self.createEditTitle("Edit QuoteTemplate");
            self.createButtonVisible(false);
            self.editButtonVisible(true);

            $('#CreateEditQuoteTemplate_ModalAlert').modal('show');
        };

        self.click_EditQuoteTemplate = function (e) {
            self.EditQuoteTemplate();
        };

        self.click_CreateQuoteTemplate = function (e) {

            var form = $('#formCreateEditQuoteTemplate');

            if (form.valid()) {
                self.SaveNewQuoteTemplate();
            }
        };


        // Subscribe
        self.selectedTeam.subscribe(function () {

            if (self.selectedTeam() !== undefined) {
                self.GetQuoteTemplatesForTeam();
            }
            else {
                self.QuoteTemplatesForTeamSelectList.removeAll();
                self.teamQuoteTemplatesList.removeAll();
            }
        });


        return self;
    };

});