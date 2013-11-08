ko.observableArray.fn.setAt = function (index, value) {
    this.valueWillMutate();
    this()[index] = value;
    this.valueHasMutated();
};


$(function () {

    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

    ConsoleApp.vmManageMarketWordings = new function () {
        var self = this;

        self.teamList = ko.observableArray();
        self.officeList = ko.observableArray();
        self.allMarketWordingsList = ko.observableArray([new ConsoleApp.MarketWording()]);
        self.marketWordingsForTeamOfficeSelectList = ko.observableArray();
        self.teamOfficeMarketWordingsList = ko.observableArray([]);
        self.selectedTeam = ko.observable();
        self.selectedOffice = ko.observable();
        self.showImageProcessing_LoadingMarketWordings = ko.observable('block');
        self.editMarketWordingButton_Status = ko.observable(false);
        self.saveTeamOfficeMarketWordingsButton_Status = ko.observable(false);
        self.createButtonVisible = ko.observable(true);
        self.editButtonVisible = ko.observable(true);
        self.createEditTitle = ko.observable(true);
        // Selected
        self.selectedTeamMarketWording = ko.observable(new ConsoleApp.MarketWording());
        self.selectedAllMarketWording = ko.observable(new ConsoleApp.MarketWording());
        self.selectedMarketWordingForTeamOfficeSelectList = ko.observable(-1);


        self.Initialize = function () {
            self.GetMarketWordings();
            self.GetTeamsFullData();
        };

        self.SaveNewMarketWording = function () {

            var data = ko.toJSON(self.selectedAllMarketWording());

            var ajaxConfig = { Url: "/Admin/CreateMarketWording", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                self.allMarketWordingsList.unshift(new ConsoleApp.MarketWording()
                        .Id(data.Id)
                        .WordingRefNumber(data.WordingRefNumber)
                        .Title(data.Title)
                );

                self.selectedAllMarketWording(new ConsoleApp.MarketWording());
                toastr.info("MarketWording successfully created");

                $('#CreateEditMarketWording_ModalAlert').modal('hide');
            });
        };

        self.EditMarketWording = function () {

            var data = ko.toJSON(self.selectedAllMarketWording());

            var ajaxConfig = { Url: "/Admin/EditMarketWording", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                toastr.info("MarketWording successfully updated");
                $('#CreateEditMarketWording_ModalAlert').modal('hide');
            });
        };

        self.DeleteMarketWording = function () {

            var data = ko.toJSON(self.selectedAllMarketWording());

            var ajaxConfig = { Url: "/Admin/DeleteMarketWording", VerbType: "DELETE", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                if (data == "Success") {
                    self.allMarketWordingsList.remove(function (item) { return item.Id == self.selectedAllMarketWording().Id; });
                    toastr.info("Market wording successfully deleted");
                }
                else {
                    toastr.info("Market wording not deleted");
                }
            });
        };


        self.GetMarketWordings = function () {

            self.allMarketWordingsList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetMarketWordings", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);
            response.success(function (data) {

                if (data != null) {
                    $.each(data, function (key, value) {
                        self.allMarketWordingsList.push(new ConsoleApp.MarketWording()
                                        .Id(value.Id)
                                        .Key(value.Key)
                                        .WordingRefNumber(value.WordingRefNumber)
                                        .Title(value.Title)
                        );
                    });
                }
                else {
                    toastr.info("No MarketWordings Found");
                }


                self.showImageProcessing_LoadingMarketWordings('none');

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

        self.GetMarketWordingsForTeamOffice = function () {

            self.marketWordingsForTeamOfficeSelectList.removeAll();
            self.teamOfficeMarketWordingsList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetMarketWordingsForTeamOffice?teamId=" + self.selectedTeam() + "&officeId=" + self.selectedOffice(), VerbType: "GET", ContentType: "application/json;charset=utf-8" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                //console.log(data);

                if (data.length > 0) {

                    var currentTeamMarketWordingsTempArray = ko.observableArray();
                    currentTeamMarketWordingsTempArray.removeAll();

                    $.each(data, function (key, value) {

                        self.marketWordingsForTeamOfficeSelectList.push({ key: value.MarketWording.Id, name: value.MarketWording.Title, WordingRefNumber: value.MarketWording.WordingRefNumber });

                        currentTeamMarketWordingsTempArray.push(new ConsoleApp.MarketWording()
                            .Id(value.MarketWording.Id)
                            .Key(value.MarketWording.Key));
                    });

                    var differenceList = ko.utils.compareArrays(currentTeamMarketWordingsTempArray(), self.allMarketWordingsList());

                    var diffResults = ko.observableArray();
                    ko.utils.arrayForEach(differenceList, function (difference) {
                        if (difference.status === "deleted") {
                            diffResults.push(difference.value.Id());
                        }
                    });

                    $.each(self.allMarketWordingsList(), function (key, value) {
                        if (diffResults.indexOf(value.Id()) == -1)

                            self.teamOfficeMarketWordingsList.push(new ConsoleApp.MarketWording()
                                .Id(value.Id())
                                .WordingRefNumber(value.WordingRefNumber())
                                .Title(value.Title()));
                    });

                } else {
                    self.marketWordingsForTeamOfficeSelectList.push({ key: 0, name: "No MarketWordings..."});

                    $.each(self.allMarketWordingsList(), function (key, value) {
                        //self.teamLinksList.push(value);
                        self.teamOfficeMarketWordingsList.push(new ConsoleApp.MarketWording()
                                .Id(value.Id())
                                .WordingRefNumber(value.WordingRefNumber())
                                .Title(value.Title()));
                    });
                }

            });
        };

        self.SaveMarketWordingsForTeamOffice = function () {
            self.marketWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == '0'; });
            var teamMarketWordingsDto = new ConsoleApp.TeamMarketWordingsDto();
            teamMarketWordingsDto.TeamId(self.selectedTeam());
            teamMarketWordingsDto.OfficeId(self.selectedOffice());

            $.each(self.marketWordingsForTeamOfficeSelectList(), function (key, value) {
                teamMarketWordingsDto.MarketWordingSettingDtoList.push(new ConsoleApp.MarketWordingSettingDto()
                    .Id(value.key)
                    .DisplayOrder(key + 1));
            });

            var data = ko.toJSON(teamMarketWordingsDto);

            var ajaxConfig = { Url: "/Admin/SaveMarketWordingsForTeamOffice", VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                toastr.info("Team MarketWordings successfully updated");
            });
        };

        // Events
        self.click_RemoveMarketWordingFromTeam = function (e) {

            if (self.selectedTeam() !== undefined && self.selectedOffice() !== undefined && self.selectedMarketWordingForTeamOfficeSelectList() !== undefined && self.selectedMarketWordingForTeamOfficeSelectList() > 0) {
                var removeItem = ko.utils.arrayFirst(self.allMarketWordingsList(), function (item) {
                    return (item.Id() == self.selectedMarketWordingForTeamOfficeSelectList());
                });

                self.marketWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == self.selectedMarketWordingForTeamOfficeSelectList(); });

                self.teamOfficeMarketWordingsList.unshift(new ConsoleApp.MarketWording()
                                .Id(removeItem.Id())
                                .WordingRefNumber(removeItem.WordingRefNumber())
                                .Title(removeItem.Title()));
                self.selectedMarketWordingForTeamOfficeSelectList(-1);
            }
            else {
                alert("Select a Market wording to remove");
            }
        };

        self.click_AddMarketWordingToTeamOffice = function (e) {

            self.marketWordingsForTeamOfficeSelectList.remove(function (item) { return item.key == '0'; });

            if (self.selectedTeam() !== undefined && self.selectedOffice() !== undefined && self.selectedTeamMarketWording().Id() > 0) {

                if (self.marketWordingsForTeamOfficeSelectList.indexOf(self.selectedTeamMarketWording().Id()) === -1) {
                    self.marketWordingsForTeamOfficeSelectList.push({ key: self.selectedTeamMarketWording().Id(), name: self.selectedTeamMarketWording().Title(), WordingRefNumber: self.selectedTeamMarketWording().WordingRefNumber() });
                    self.teamOfficeMarketWordingsList.remove(self.selectedTeamMarketWording());
                    self.selectedTeamMarketWording(new ConsoleApp.MarketWording());
                }
                else {
                    alert("MarketWording already added...");
                }
            }
            else {
                alert("Select a team/office/MarketWording first...");
            }

        };

        self.onClickTeamOfficeMarketWordingItem = function (e) {
            self.selectedTeamMarketWording(this);
        };

        self.onClickSelectedTeamOfficeMarketWordingItem = function (item) {
            if (item.key > 0) self.selectedMarketWordingForTeamOfficeSelectList(item.key);
        };

        self.click_selectedMarketWordingForTeamOfficeSelectListMoveUp = function () {
            var moveItem = ko.utils.arrayFirst(self.marketWordingsForTeamOfficeSelectList(), function (item2) {
                return (item2.key == self.selectedMarketWordingForTeamOfficeSelectList());
            });


            var i = self.marketWordingsForTeamOfficeSelectList.indexOf(moveItem);
            if (i >= 1) {
                var array = self.marketWordingsForTeamOfficeSelectList();
                self.marketWordingsForTeamOfficeSelectList.splice(i - 1, 2, array[i], array[i - 1]);
            }
        };

        self.click_selectedMarketWordingForTeamOfficeSelectListMoveDown = function () {
            var moveItem = ko.utils.arrayFirst(self.marketWordingsForTeamOfficeSelectList(), function (item2) {
                return (item2.key == self.selectedMarketWordingForTeamOfficeSelectList());
            });


            var i = self.marketWordingsForTeamOfficeSelectList.indexOf(moveItem);
            if (i >= 0 && self.marketWordingsForTeamOfficeSelectList().length != i + 1) {
                var array = self.marketWordingsForTeamOfficeSelectList();
                self.marketWordingsForTeamOfficeSelectList.splice(i, 2, array[i + 1], array[i]);
            }
        };

        self.onClickAllMarketWordingsItem = function (e) {

            self.selectedAllMarketWording(this);
            //console.log(self.selectedAllMarketWording());
        };

        self.click_DeleteMarketWording = function () {

            self.selectedAllMarketWording(this);

            var result = confirm("Delete MarketWording?");
            if (result) {
                self.DeleteMarketWording();
            }
        };

        self.click_SaveTeamOfficeMarketWordings = function () {
            self.SaveMarketWordingsForTeamOffice();
        };

        self.click_CancelCreateEdit = function () {
            self.selectedAllMarketWording(new ConsoleApp.MarketWording());
        };

        self.click_OpenCreateEditMarketWordingModal_Create = function (e) {
            self.createEditTitle("Create MarketWording");
            self.selectedAllMarketWording(new ConsoleApp.MarketWording());
            self.createButtonVisible(true);
            self.editButtonVisible(false);
            $('#CreateEditMarketWording_ModalAlert').modal('show');
        };

        self.click_OpenCreateEditMarketWordingModal_Edit = function (e) {
            self.createEditTitle("Edit MarketWording");
            self.createButtonVisible(false);
            self.editButtonVisible(true);

            $('#CreateEditMarketWording_ModalAlert').modal('show');
        };

        self.click_EditMarketWording = function (e) {
            self.EditMarketWording();
        };

        self.click_CreateMarketWording = function (e) {

            var form = $('#formCreateEditMarketWording');

            if (form.valid()) {
                self.SaveNewMarketWording();
            }
        };


        // Subscribe
        self.selectedTeam.subscribe(function () {
            if (self.selectedTeam() !== undefined) {
                $.each(self.teamList(), function (index, value) {
                    if (value.key == self.selectedTeam()) {
                        self.officeList.removeAll();
                        self.selectedTeamMarketWording(new ConsoleApp.MarketWording());
                        self.selectedMarketWordingForTeamOfficeSelectList(-1);
                        $.each(value.relatedOffices, function (officekey, officevalue) {
                            self.officeList.push({ key: officevalue.Id, name: officevalue.Title });
                        });
                    };
                });
            }
            else {
                self.marketWordingsForTeamOfficeSelectList.removeAll();
                self.teamOfficeMarketWordingsList.removeAll();
            }
        });
        self.selectedOffice.subscribe(function () {

            if (self.selectedOffice() !== undefined) {
                self.selectedTeamMarketWording(new ConsoleApp.MarketWording());
                self.selectedMarketWordingForTeamOfficeSelectList(-1);
                self.GetMarketWordingsForTeamOffice();
            }
            else {
                self.marketWordingsForTeamOfficeSelectList.removeAll();
                self.teamOfficeMarketWordingsList.removeAll();
            }
        });

        return self;
    };

});

