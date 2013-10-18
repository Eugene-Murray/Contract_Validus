ko.observableArray.fn.setAt = function (index, value) {
    this.valueWillMutate();
    this()[index] = value;
    this.valueHasMutated();
};


$(function () {

    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

    ConsoleApp.vmManageSubjectToClauseWordings = new function () {
        var self = this;

        self.teamList = ko.observableArray();
        self.officeList = ko.observableArray();
        self.allSubjectToClauseWordingsList = ko.observableArray([new ConsoleApp.SubjectToClauseWording()]);
        self.subjectToClauseWordingsForTeamOfficeSelectList = ko.observableArray();
        self.teamOfficeSubjectToClauseWordingsList = ko.observableArray([]);
        self.selectedTeam = ko.observable();
        self.selectedOffice = ko.observable();
        self.showImageProcessing_LoadingSubjectToClauseWordings = ko.observable('block');
        self.editSubjectToClauseWordingButton_Status = ko.observable(false);
        self.saveTeamOfficeSubjectToClauseWordingsButton_Status = ko.observable(false);
        self.createButtonVisible = ko.observable(true);
        self.editButtonVisible = ko.observable(true);
        self.createEditTitle = ko.observable(true);
        // Selected
        self.selectedTeamSubjectToClauseWording = ko.observable(new ConsoleApp.SubjectToClauseWording());
        self.selectedAllSubjectToClauseWording = ko.observable(new ConsoleApp.SubjectToClauseWording());
        self.selectedSubjectToClauseWordingForTeamOfficeSelectList = ko.observable(-1);


        self.Initialize = function () {
            self.GetSubjectToClauseWordings();
            self.GetTeamsFullData();
        };

        self.SaveNewSubjectToClauseWording = function () {

            var data = ko.toJSON(self.selectedAllSubjectToClauseWording());

            var ajaxConfig = { Url: "/Admin/CreateSubjectToClauseWording", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                self.allSubjectToClauseWordingsList.unshift(new ConsoleApp.SubjectToClauseWording()
                        .Id(data.Id)
                        .Title(data.Title)
                );

                self.selectedAllSubjectToClauseWording(new ConsoleApp.SubjectToClauseWording());
                toastr.info("SubjectToClauseWording successfully created");

                $('#CreateEditSubjectToClauseWording_ModalAlert').modal('hide');
            });
        };

        self.EditSubjectToClauseWording = function () {

            var data = ko.toJSON(self.selectedAllSubjectToClauseWording());

            var ajaxConfig = { Url: "/Admin/EditSubjectToClauseWording", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                toastr.info("SubjectToClauseWording successfully updated");
                $('#CreateEditSubjectToClauseWording_ModalAlert').modal('hide');
            });
        };

        self.DeleteSubjectToClauseWording = function () {

            var data = ko.toJSON(self.selectedAllSubjectToClauseWording());

            var ajaxConfig = { Url: "/Admin/DeleteSubjectToClauseWording", VerbType: "DELETE", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                if (data == "Success") {
                self.allSubjectToClauseWordingsList.remove(function (item) { return item.Id == self.selectedAllSubjectToClauseWording().Id; });

                toastr.info("Subjectivity successfully deleted");
                }
                else {
                    toastr.info("Subjectivity not deleted");
                }
            });
        };


        self.GetSubjectToClauseWordings = function () {

            self.allSubjectToClauseWordingsList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetSubjectToClauseWordings", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);
            response.success(function (data) {

                if (data != null) {
                    $.each(data, function (key, value) {
                        self.allSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWording()
                                        .Id(value.Id)
                                        .Key(value.Key)
                                        .Title(value.Title)
                        );
                    });
                }
                else {
                    toastr.info("No SubjectToClauseWordings Found");
                }


                self.showImageProcessing_LoadingSubjectToClauseWordings('none');

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

        self.GetSubjectToClauseWordingsForTeamOffice = function () {

            self.subjectToClauseWordingsForTeamOfficeSelectList.removeAll();
            self.teamOfficeSubjectToClauseWordingsList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetSubjectToClauseWordingsForTeamOffice?teamId=" + self.selectedTeam() + "&officeId=" + self.selectedOffice(), VerbType: "GET", ContentType: "application/json;charset=utf-8" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                //console.log(data);

                if (data.length > 0) {

                    var currentTeamSubjectToClauseWordingsTempArray = ko.observableArray();
                    currentTeamSubjectToClauseWordingsTempArray.removeAll();

                    $.each(data, function (key, value) {

                        self.subjectToClauseWordingsForTeamOfficeSelectList.push({ key: value.SubjectToClauseWording.Id, name: value.SubjectToClauseWording.Title, IsStrikeThrough: value.IsStrikeThrough });

                        currentTeamSubjectToClauseWordingsTempArray.push(new ConsoleApp.SubjectToClauseWording()
                            .Id(value.SubjectToClauseWording.Id)
                            .Key(value.SubjectToClauseWording.Key));
                    });

                    var differenceList = ko.utils.compareArrays(currentTeamSubjectToClauseWordingsTempArray(), self.allSubjectToClauseWordingsList());

                    var diffResults = ko.observableArray();
                    ko.utils.arrayForEach(differenceList, function (difference) {
                        if (difference.status === "deleted") {
                            diffResults.push(difference.value.Id());
                        }
                    });

                    $.each(self.allSubjectToClauseWordingsList(), function (key, value) {
                        if (diffResults.indexOf(value.Id()) == -1)

                            self.teamOfficeSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWording()
                                .Id(value.Id())
                                .Title(value.Title()));
                    });

                } else {
                    self.subjectToClauseWordingsForTeamOfficeSelectList.push({ key: 0, name: "No SubjectToClauseWordings...", IsStrikeThrough: false });

                    $.each(self.allSubjectToClauseWordingsList(), function (key, value) {
                        //self.teamLinksList.push(value);
                        self.teamOfficeSubjectToClauseWordingsList.push(new ConsoleApp.SubjectToClauseWording()
                                .Id(value.Id())
                                .Title(value.Title()));
                    });
                }
            });
        };

        self.SaveSubjectToClauseWordingsForTeamOffice = function () {
            self.subjectToClauseWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == '0'; });
            var teamSubjectToClauseWordingsDto = new ConsoleApp.TeamSubjectToClauseWordingsDto();
            teamSubjectToClauseWordingsDto.TeamId(self.selectedTeam());
            teamSubjectToClauseWordingsDto.OfficeId(self.selectedOffice());

            $.each(self.subjectToClauseWordingsForTeamOfficeSelectList(), function (key, value) {
                teamSubjectToClauseWordingsDto.SubjectToClauseWordingSettingDtoList.push(new ConsoleApp.SubjectToClauseWordingSettingDto()
                    .Id(value.key)
                    .DisplayOrder(key + 1)
                    .IsStrikeThrough(value.IsStrikeThrough)
                );
            });

            var data = ko.toJSON(teamSubjectToClauseWordingsDto);

            var ajaxConfig = { Url: "/Admin/SaveSubjectToClauseWordingsForTeamOffice", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                toastr.info("Team SubjectToClauseWordings successfully updated");
            });
        };

        // Events
        self.click_RemoveSubjectToClauseWordingFromTeam = function (e) {

            if (self.selectedTeam() !== undefined && self.selectedOffice() !== undefined && self.selectedSubjectToClauseWordingForTeamOfficeSelectList() !== undefined && self.selectedSubjectToClauseWordingForTeamOfficeSelectList() > 0) {
                var removeItem = ko.utils.arrayFirst(self.allSubjectToClauseWordingsList(), function (item) {
                    return (item.Id() == self.selectedSubjectToClauseWordingForTeamOfficeSelectList());
                });

                self.subjectToClauseWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == self.selectedSubjectToClauseWordingForTeamOfficeSelectList(); });

                self.teamOfficeSubjectToClauseWordingsList.unshift(new ConsoleApp.SubjectToClauseWording()
                                .Id(removeItem.Id())
                                .Title(removeItem.Title()));
                self.selectedSubjectToClauseWordingForTeamOfficeSelectList(-1);
            }
            else {
                alert("Select a Market wording to remove");
            }

        };

        self.click_AddSubjectToClauseWordingToTeamOffice = function (e) {

            self.subjectToClauseWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == '0'; });

            if (self.selectedTeam() !== undefined && self.selectedOffice() !== undefined && self.selectedTeamSubjectToClauseWording().Id() > 0) {

                if (self.subjectToClauseWordingsForTeamOfficeSelectList.indexOf(self.selectedTeamSubjectToClauseWording().Id()) === -1) {
                    self.subjectToClauseWordingsForTeamOfficeSelectList.push({ key: self.selectedTeamSubjectToClauseWording().Id(), name: self.selectedTeamSubjectToClauseWording().Title(), IsStrikeThrough: false });
                    self.teamOfficeSubjectToClauseWordingsList.remove(self.selectedTeamSubjectToClauseWording());
                    self.selectedTeamSubjectToClauseWording(new ConsoleApp.SubjectToClauseWording());
                }
                else {
                    alert("SubjectToClauseWording already added...");
                }
            }
            else {
                alert("Select a team/office/SubjectToClauseWording first...");
            }

        };

        self.onClickTeamOfficeSubjectToClauseWordingItem = function (e) {
            self.selectedTeamSubjectToClauseWording(this);
        };

        self.onClickSelectedTeamOfficeSubjectToClauseWordingItem = function (item) {
            if (item.key > 0) self.selectedSubjectToClauseWordingForTeamOfficeSelectList(item.key);
        };

        self.onClickSelectedTeamOfficeSubjectToClauseWordingItemToggleIsStrikeThrough = function (item) {
            if (item.key > 0) {
                var changeItem = ko.utils.arrayFirst(self.subjectToClauseWordingsForTeamOfficeSelectList(), function (item2) {
                    return (item2.key == item.key);
                });

                var index = self.subjectToClauseWordingsForTeamOfficeSelectList.indexOf(changeItem);
                self.subjectToClauseWordingsForTeamOfficeSelectList.remove(function (item2) { return item2.key == item.key; });
                self.subjectToClauseWordingsForTeamOfficeSelectList.splice(index, 0, changeItem);

                self.selectedSubjectToClauseWordingForTeamOfficeSelectList(item.key);
            }
        };

        self.click_selectedSubjectToClauseWordingForTeamOfficeSelectListMoveUp = function () {
            var moveItem = ko.utils.arrayFirst(self.subjectToClauseWordingsForTeamOfficeSelectList(), function (item2) {
                return (item2.key == self.selectedSubjectToClauseWordingForTeamOfficeSelectList());
            });


            var i = self.subjectToClauseWordingsForTeamOfficeSelectList.indexOf(moveItem);
            if (i >= 1) {
                var array = self.subjectToClauseWordingsForTeamOfficeSelectList();
                self.subjectToClauseWordingsForTeamOfficeSelectList.splice(i - 1, 2, array[i], array[i - 1]);
            }
        };

        self.click_selectedSubjectToClauseWordingForTeamOfficeSelectListMoveDown = function () {
            var moveItem = ko.utils.arrayFirst(self.subjectToClauseWordingsForTeamOfficeSelectList(), function (item2) {
                return (item2.key == self.selectedSubjectToClauseWordingForTeamOfficeSelectList());
            });


            var i = self.subjectToClauseWordingsForTeamOfficeSelectList.indexOf(moveItem);
            if (i >= 0 && self.subjectToClauseWordingsForTeamOfficeSelectList().length != i + 1) {
                var array = self.subjectToClauseWordingsForTeamOfficeSelectList();
                self.subjectToClauseWordingsForTeamOfficeSelectList.splice(i, 2, array[i + 1], array[i]);
            }
        };

        self.onClickAllSubjectToClauseWordingsItem = function (e) {

            self.selectedAllSubjectToClauseWording(this);
            //console.log(self.selectedAllSubjectToClauseWording());
        };

        self.click_DeleteSubjectToClauseWording = function () {

            self.selectedAllSubjectToClauseWording(this);

            var result = confirm("Delete SubjectToClauseWording?");
            if (result) {
                self.DeleteSubjectToClauseWording();
            }
        };

        self.click_SaveTeamOfficeSubjectToClauseWordings = function () {
            self.SaveSubjectToClauseWordingsForTeamOffice();
        };

        self.click_CancelCreateEdit = function () {
            self.selectedAllSubjectToClauseWording(new ConsoleApp.SubjectToClauseWording());
        };

        self.click_OpenCreateEditSubjectToClauseWordingModal_Create = function (e) {
            self.createEditTitle("Create SubjectToClauseWording");
            self.selectedAllSubjectToClauseWording(new ConsoleApp.SubjectToClauseWording());
            self.createButtonVisible(true);
            self.editButtonVisible(false);
            $('#CreateEditSubjectToClauseWording_ModalAlert').modal('show');
        };

        self.click_OpenCreateEditSubjectToClauseWordingModal_Edit = function (e) {
            self.createEditTitle("Edit SubjectToClauseWording");
            self.createButtonVisible(false);
            self.editButtonVisible(true);

            $('#CreateEditSubjectToClauseWording_ModalAlert').modal('show');
        };

        self.click_EditSubjectToClauseWording = function (e) {
            self.EditSubjectToClauseWording();
        };

        self.click_CreateSubjectToClauseWording = function (e) {

            var form = $('#formCreateEditSubjectToClauseWording');

            if (form.valid()) {
                self.SaveNewSubjectToClauseWording();
            }
        };


        // Subscribe
        self.selectedTeam.subscribe(function () {
            if (self.selectedTeam() !== undefined) {
                $.each(self.teamList(), function (index, value) {
                    if (value.key == self.selectedTeam()) {
                        self.officeList.removeAll();
                        self.selectedTeamSubjectToClauseWording(new ConsoleApp.SubjectToClauseWording());
                        self.selectedSubjectToClauseWordingForTeamOfficeSelectList(-1);
                        $.each(value.relatedOffices, function (officekey, officevalue) {
                            self.officeList.push({ key: officevalue.Id, name: officevalue.Title });
                        });
                    };
                });
            }
            else {
                self.subjectToClauseWordingsForTeamOfficeSelectList.removeAll();
                self.teamOfficeSubjectToClauseWordingsList.removeAll();
            }
        });
        self.selectedOffice.subscribe(function () {

            if (self.selectedOffice() !== undefined) {
                self.selectedTeamSubjectToClauseWording(new ConsoleApp.SubjectToClauseWording());
                self.selectedSubjectToClauseWordingForTeamOfficeSelectList(-1);
                self.GetSubjectToClauseWordingsForTeamOffice();
            }
            else {
                self.subjectToClauseWordingsForTeamOfficeSelectList.removeAll();
                self.teamOfficeSubjectToClauseWordingsList.removeAll();
            }
        });

        return self;
    };

});

