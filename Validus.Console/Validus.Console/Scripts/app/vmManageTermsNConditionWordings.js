ko.observableArray.fn.setAt = function(index, value) {
    this.valueWillMutate();
    this()[index] = value;
    this.valueHasMutated();
};


$(function () {

    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

    ConsoleApp.vmManageTermsNConditionWordings = new function () {
        var self = this;

        self.teamList = ko.observableArray();
        self.officeList = ko.observableArray();
        self.allTermsNConditionWordingsList = ko.observableArray([new ConsoleApp.TermsNConditionWording()]);
        self.termsNConditionWordingsForTeamOfficeSelectList = ko.observableArray();
        self.teamOfficeTermsNConditionWordingsList = ko.observableArray([]);
        self.selectedTeam = ko.observable();
        self.selectedOffice = ko.observable();
        self.showImageProcessing_LoadingTermsNConditionWordings = ko.observable('block');
        self.editTermsNConditionWordingButton_Status = ko.observable(false);
        self.saveTeamOfficeTermsNConditionWordingsButton_Status = ko.observable(false);
        self.createButtonVisible = ko.observable(true);
        self.editButtonVisible = ko.observable(true);
        self.createEditTitle = ko.observable(true);
        // Selected
        self.selectedTeamTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWording());
        self.selectedAllTermsNConditionWording = ko.observable(new ConsoleApp.TermsNConditionWording());
        self.selectedTermsNConditionWordingForTeamOfficeSelectList = ko.observable(-1);


        self.Initialize = function () {
            self.GetTermsNConditionWordings();
            self.GetTeamsFullData();
        };

        self.SaveNewTermsNConditionWording = function () {

            var data = ko.toJSON(self.selectedAllTermsNConditionWording());

            var ajaxConfig = { Url: "/Admin/CreateTermsNConditionWording", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                self.allTermsNConditionWordingsList.unshift(new ConsoleApp.TermsNConditionWording()
                        .Id(data.Id)
                        .WordingRefNumber(data.WordingRefNumber)
                        .Title(data.Title)
                );

                self.selectedAllTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
                toastr.info("TermsNConditionWording successfully created");

                $('#CreateEditTermsNConditionWording_ModalAlert').modal('hide');
            });
        };

        self.EditTermsNConditionWording = function () {

            var data = ko.toJSON(self.selectedAllTermsNConditionWording());

            var ajaxConfig = { Url: "/Admin/EditTermsNConditionWording", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                toastr.info("TermsNConditionWording successfully updated");
                $('#CreateEditTermsNConditionWording_ModalAlert').modal('hide');
            });
        };

        self.DeleteTermsNConditionWording = function () {

            var data = ko.toJSON(self.selectedAllTermsNConditionWording());

            var ajaxConfig = { Url: "/Admin/DeleteTermsNConditionWording", VerbType: "DELETE", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                if (data == "Success") {
                    self.allTermsNConditionWordingsList.remove(function (item) { return item.Id == self.selectedAllTermsNConditionWording().Id; });

                    toastr.info("TermsNCondition successfully deleted");
                }
                else {
                    toastr.info("TermsNCondition not deleted");
                }
            });
        };

        self.GetTermsNConditionWordings = function () {

            self.allTermsNConditionWordingsList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetTermsNConditionWordings", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);
            response.success(function (data) {

                if (data != null) {
                    $.each(data, function (key, value) {
                        self.allTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWording()
                                        .Id(value.Id)
                                        .Key(value.Key)
                                        .WordingRefNumber(value.WordingRefNumber)
                                        .Title(value.Title)
                        );
                    });
                }
                else {
                    toastr.info("No TermsNConditionWordings Found");
                }
				
                self.showImageProcessing_LoadingTermsNConditionWordings('none');
            });
        };

        self.GetTeamsFullData = function () {

            self.teamList.removeAll();
            self.officeList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetTeamsFullData", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                $.each(data, function (key, value) {
                    var offices = [];
                    $.each(value.RelatedOffices, function (officekey, officevalue) {
                        offices.push({ Id: officevalue.Id, Title: officevalue.Title });
                    });
                    self.teamList.push({ key: value.Id, name: value.Title, relatedOffices: offices });

                });
            });
        };

        self.GetTermsNConditionWordingsForTeamOffice = function () {

            self.termsNConditionWordingsForTeamOfficeSelectList.removeAll();
            self.teamOfficeTermsNConditionWordingsList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetTermsNConditionWordingsForTeamOffice?teamId=" + self.selectedTeam() + "&officeId=" + self.selectedOffice(), VerbType: "GET", ContentType: "application/json;charset=utf-8" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                //console.log(data);

                if (data.length > 0) {

                    var currentTeamTermsNConditionWordingsTempArray = ko.observableArray();
                    currentTeamTermsNConditionWordingsTempArray.removeAll();

                    $.each(data, function (key, value) {

                        self.termsNConditionWordingsForTeamOfficeSelectList.push({ key: value.TermsNConditionWording.Id, name: value.TermsNConditionWording.Title, WordingRefNumber: value.TermsNConditionWording.WordingRefNumber, IsStrikeThrough: value.IsStrikeThrough });

                        currentTeamTermsNConditionWordingsTempArray.push(new ConsoleApp.TermsNConditionWording()
                            .Id(value.TermsNConditionWording.Id)
                            .Key(value.TermsNConditionWording.Key));
                    });

                    var differenceList = ko.utils.compareArrays(currentTeamTermsNConditionWordingsTempArray(), self.allTermsNConditionWordingsList());

                    var diffResults = ko.observableArray();
                    ko.utils.arrayForEach(differenceList, function (difference) {
                        if (difference.status === "deleted") {
                            diffResults.push(difference.value.Id());
                        }
                    });

                    $.each(self.allTermsNConditionWordingsList(), function (key, value) {
                        if (diffResults.indexOf(value.Id()) == -1)

                            self.teamOfficeTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWording()
                                .Id(value.Id())
                                .WordingRefNumber(value.WordingRefNumber())
                                .Title(value.Title()));
                    });

                } else {
                    self.termsNConditionWordingsForTeamOfficeSelectList.push({ key: 0, name: "No TermsNConditionWordings...",IsStrikeThrough:false });

                    $.each(self.allTermsNConditionWordingsList(), function (key, value) {
                        //self.teamLinksList.push(value);
                        self.teamOfficeTermsNConditionWordingsList.push(new ConsoleApp.TermsNConditionWording()
                                .Id(value.Id())
                                .WordingRefNumber(value.WordingRefNumber())
                                .Title(value.Title()));
                    });
                }
            });
        };

        self.SaveTermsNConditionWordingsForTeamOffice = function () {
            self.termsNConditionWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == '0'; });
            var teamTermsNConditionWordingsDto = new ConsoleApp.TeamTermsNConditionWordingsDto();
            teamTermsNConditionWordingsDto.TeamId(self.selectedTeam());
            teamTermsNConditionWordingsDto.OfficeId(self.selectedOffice());

            $.each(self.termsNConditionWordingsForTeamOfficeSelectList(), function (key, value) {
                teamTermsNConditionWordingsDto.TermsNConditionWordingSettingDtoList.push(new ConsoleApp.TermsNConditionWordingSettingDto()
                    .Id(value.key)
                    .DisplayOrder(key + 1)
                    .IsStrikeThrough(value.IsStrikeThrough)
                );
            });

            var data = ko.toJSON(teamTermsNConditionWordingsDto);

            var ajaxConfig = { Url: "/Admin/SaveTermsNConditionWordingsForTeamOffice", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                toastr.info("Team TermsNConditionWordings successfully updated");
            });
        };

        // Events
        self.click_RemoveTermsNConditionWordingFromTeam = function (e) {

            if (self.selectedTeam() !== undefined && self.selectedOffice() !== undefined && self.selectedTermsNConditionWordingForTeamOfficeSelectList() !== undefined && self.selectedTermsNConditionWordingForTeamOfficeSelectList()>0) {
                var removeItem = ko.utils.arrayFirst(self.allTermsNConditionWordingsList(), function (item) {
                    return (item.Id() == self.selectedTermsNConditionWordingForTeamOfficeSelectList());
                });

                self.termsNConditionWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == self.selectedTermsNConditionWordingForTeamOfficeSelectList(); });

                self.teamOfficeTermsNConditionWordingsList.unshift(new ConsoleApp.TermsNConditionWording()
                                .Id(removeItem.Id())
                                .WordingRefNumber(removeItem.WordingRefNumber())
                                .Title(removeItem.Title()));
                self.selectedTermsNConditionWordingForTeamOfficeSelectList(-1);
            }
            else {
                alert("Select a Market wording to remove");
            }
        };

        self.click_AddTermsNConditionWordingToTeamOffice = function (e) {

            self.termsNConditionWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == '0'; });

            if (self.selectedTeam() !== undefined && self.selectedOffice() !== undefined && self.selectedTeamTermsNConditionWording().Id()>0) {

                if (self.termsNConditionWordingsForTeamOfficeSelectList.indexOf(self.selectedTeamTermsNConditionWording().Id()) === -1) {
                    self.termsNConditionWordingsForTeamOfficeSelectList.push({ key: self.selectedTeamTermsNConditionWording().Id(), name: self.selectedTeamTermsNConditionWording().Title(), WordingRefNumber: self.selectedTeamTermsNConditionWording().WordingRefNumber(), IsStrikeThrough: false });
                    self.teamOfficeTermsNConditionWordingsList.remove(self.selectedTeamTermsNConditionWording());
                    self.selectedTeamTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
                }
                else {
                    alert("TermsNConditionWording already added...");
                }
            }
            else {
                alert("Select a team/office/TermsNConditionWording first...");
            }

        };

        self.onClickTeamOfficeTermsNConditionWordingItem = function (e) {
            self.selectedTeamTermsNConditionWording(this);
        };

        self.onClickSelectedTeamOfficeTermsNConditionWordingItem = function (item) {
            if (item.key>0) self.selectedTermsNConditionWordingForTeamOfficeSelectList(item.key);
        };
        
        self.onClickSelectedTeamOfficeTermsNConditionWordingItemToggleIsStrikeThrough = function (item) {
            if (item.key > 0) {
                var changeItem = ko.utils.arrayFirst(self.termsNConditionWordingsForTeamOfficeSelectList(), function(item2) {
                    return (item2.key == item.key);
                });

                var index = self.termsNConditionWordingsForTeamOfficeSelectList.indexOf(changeItem);
                self.termsNConditionWordingsForTeamOfficeSelectList.remove(function(item2) { return item2.key == item.key; });
                self.termsNConditionWordingsForTeamOfficeSelectList.splice(index, 0, changeItem);

                self.selectedTermsNConditionWordingForTeamOfficeSelectList(item.key);
            }
        };
        
        self.click_selectedTermsNConditionWordingForTeamOfficeSelectListMoveUp = function ()
        {
            var moveItem = ko.utils.arrayFirst(self.termsNConditionWordingsForTeamOfficeSelectList(), function (item2) {
                return (item2.key == self.selectedTermsNConditionWordingForTeamOfficeSelectList());
            });


            var i = self.termsNConditionWordingsForTeamOfficeSelectList.indexOf(moveItem);
            if (i >= 1) {
                var array = self.termsNConditionWordingsForTeamOfficeSelectList();
                self.termsNConditionWordingsForTeamOfficeSelectList.splice(i - 1, 2, array[i], array[i - 1]);
            }
        };

        self.click_selectedTermsNConditionWordingForTeamOfficeSelectListMoveDown = function () {
            var moveItem = ko.utils.arrayFirst(self.termsNConditionWordingsForTeamOfficeSelectList(), function (item2) {
                return (item2.key == self.selectedTermsNConditionWordingForTeamOfficeSelectList());
            });


            var i = self.termsNConditionWordingsForTeamOfficeSelectList.indexOf(moveItem);
            if (i >= 0 && self.termsNConditionWordingsForTeamOfficeSelectList().length!= i+1) {
                var array = self.termsNConditionWordingsForTeamOfficeSelectList();
                self.termsNConditionWordingsForTeamOfficeSelectList.splice(i, 2, array[i+1], array[i]);
            }
        };

        self.onClickAllTermsNConditionWordingsItem = function (e) {

            self.selectedAllTermsNConditionWording(this);
            //console.log(self.selectedAllTermsNConditionWording());
        };

        self.click_DeleteTermsNConditionWording = function () {

            self.selectedAllTermsNConditionWording(this);

            var result = confirm("Delete TermsNConditionWording?");
            if (result) {
                self.DeleteTermsNConditionWording();
            }
        };

        self.click_SaveTeamOfficeTermsNConditionWordings = function () {
            self.SaveTermsNConditionWordingsForTeamOffice();
        };

        self.click_CancelCreateEdit = function () {
            self.selectedAllTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
        };

        self.click_OpenCreateEditTermsNConditionWordingModal_Create = function (e) {
            self.createEditTitle("Create TermsNConditionWording");
            self.selectedAllTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
            self.createButtonVisible(true);
            self.editButtonVisible(false);
            $('#CreateEditTermsNConditionWording_ModalAlert').modal('show');
        };

        self.click_OpenCreateEditTermsNConditionWordingModal_Edit = function (e) {
            self.createEditTitle("Edit TermsNConditionWording");
            self.createButtonVisible(false);
            self.editButtonVisible(true);

            $('#CreateEditTermsNConditionWording_ModalAlert').modal('show');
        };

        self.click_EditTermsNConditionWording = function (e) {
            self.EditTermsNConditionWording();
        };

        self.click_CreateTermsNConditionWording = function (e) {

            var form = $('#formCreateEditTermsNConditionWording');

            if (form.valid()) {
                self.SaveNewTermsNConditionWording();
            }
        };


        // Subscribe
        self.selectedTeam.subscribe(function () {
            if (self.selectedTeam() !== undefined) {
                $.each(self.teamList(), function (index, value) {
                    if (value.key == self.selectedTeam()) {
                        self.officeList.removeAll();
                        self.selectedTeamTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
                        self.selectedTermsNConditionWordingForTeamOfficeSelectList(-1);
                        $.each(value.relatedOffices, function (officekey, officevalue) {
                            self.officeList.push({ key: officevalue.Id, name: officevalue.Title });
                        });
                    };
                });
            }
            else {
                self.termsNConditionWordingsForTeamOfficeSelectList.removeAll();
                self.teamOfficeTermsNConditionWordingsList.removeAll();
            }
        });
        self.selectedOffice.subscribe(function () {

            if (self.selectedOffice() !== undefined) {
                self.selectedTeamTermsNConditionWording(new ConsoleApp.TermsNConditionWording());
                self.selectedTermsNConditionWordingForTeamOfficeSelectList(-1);
                self.GetTermsNConditionWordingsForTeamOffice();
            }
            else {
                self.termsNConditionWordingsForTeamOfficeSelectList.removeAll();
                self.teamOfficeTermsNConditionWordingsList.removeAll();
            }
        });

        return self;
    };

});

