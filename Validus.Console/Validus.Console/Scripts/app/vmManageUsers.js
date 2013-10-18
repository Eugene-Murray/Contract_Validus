$(function () {

    var ConsoleApp = window.ConsoleApp = window.ConsoleApp || {};

    ConsoleApp.vmManageUsers = new function () {
        var self = this;
        self.userSearchList = ko.observableArray();
        self.usersTeamList = ko.observableArray();
        self.userList = ko.observableArray(["Users in Team..."]);
        self.selectedTeam = ko.observable('');
        self.selectedUserName = ko.observable('');
        self.searchUserNameTxt = ko.observable('');
        self.remove_Status = ko.observable(false);
        self.add_Status = ko.observable(false);
        self.showDivUserSearchArea = ko.observable(false);
        self.showDivTopButtonArea = ko.observable(false);
        self.enableUserDetailsEdit = ko.observable(true);
        self.personalSettingsTitle_Visible = ko.observable(false);
        self.showDivAddRemoveTeamMembershipsArea = ko.observable(true);
        self.createNewUserBtnTxt = ko.observable('Create New User');
        self.showImageProcessing_SavingUser = ko.observable(false);
        self.saveUser_Status = ko.observable(true);
        self.createNewUserBtn_Visible = ko.observable(true);
        self.searchForUserBtn_Visible = ko.observable(false);
        self.showImageProcessing_LoadUser = ko.observable('none');
        self.showImageProcessing_SavingUser = ko.observable('none');
        self._Underwriter = ko.observable('');
        self._NonLondonBroker = ko.observable('');
        self._DomainLogon = ko.observable('');
        
        // Save Mode
        self.saveMode = ko.observable("EDIT"); // CREATE or EDIT
        // User to Create / Edit
        self.userToManage = ko.observable(new ConsoleApp.User());
        // Selected Item
        self.selectedItem = ko.observable();

        self.showNonLondonBroker = ko.observable(false);

        self.Initialize = function (pageConfig) {

        	if (pageConfig !== undefined)
        	{
                self.PageSetup(pageConfig.pageMode, pageConfig.selectedUser);
            }
            else {
                self.PageSetup("EDIT_SEARCH", undefined);
                self.GetTeamsBasicData();
            }           
        };

        // CREATE_NEW, EDIT_SEARCH, EDIT_FROM_TEAMPAGE, EDIT_FROM_PERSONALSETTINGS
        self.PageSetup = function (pageMode, selectedUser) {

        	if (pageMode == "CREATE_NEW") {

        		self.ClearAllLists();
        		self.userToManage(new ConsoleApp.User());
        		self.showDivUserSearchArea(false);
        		self.createNewUserBtn_Visible(false);
        		self.searchForUserBtn_Visible(true);
        		self.enableUserDetailsEdit(true);
        		self.personalSettingsTitle_Visible(false);
        		self._Underwriter('');
        		self.saveMode("CREATE");

        		self.GetRequiredDataCreateUser();
        	}

            if (pageMode == "EDIT_SEARCH") {
                self.ClearAllLists();
                self.userToManage(new ConsoleApp.User());
                self.showDivUserSearchArea(true);
                self.createNewUserBtn_Visible(true);
                self.searchForUserBtn_Visible(false);
                self.enableUserDetailsEdit(true);
                self.personalSettingsTitle_Visible(false);
                self.showDivTopButtonArea(true);
                self._Underwriter('');
                self.saveMode("EDIT");
            }

            if (pageMode == "EDIT_FROM_TEAMPAGE") {
                self.ClearAllLists();
                self.userToManage(new ConsoleApp.User());
                self.showDivUserSearchArea(false);
                self.createNewUserBtn_Visible(false);
                self.showDivTopButtonArea(true);
                self.searchForUserBtn_Visible(true);
                self.showDivAddRemoveTeamMembershipsArea(true);
                self.enableUserDetailsEdit(true);
                self.personalSettingsTitle_Visible(false);
                self._Underwriter('');
                self.saveMode("EDIT");
	            
                self.GetSelectedUserByName(selectedUser, null);
            }

            if (pageMode == "EDIT_FROM_PERSONALSETTINGS") {
                self.ClearAllLists();
                self.userToManage(new ConsoleApp.User());
                self.showDivUserSearchArea(false);
                self.showDivTopButtonArea(false);
                self.showDivAddRemoveTeamMembershipsArea(false);
                self.enableUserDetailsEdit(false);
                self.personalSettingsTitle_Visible(true);
                self._Underwriter('');
                self.saveMode("EDIT");
	            
                self.GetUserPersonalSettings();
            }
        };

	    self.SetUpTypeAhead = function(data) {
		    var dataArray = [];
		    $(data).each(function(index, item) {
			    dataArray.push(
				    {
					    Title: item.Code + ' : ' + item.Name,
					    Value: item.Code
				    });
		    });
		    return dataArray;
	    };
	    
	    self.SetUpTypeAheadUser = function(data)
	    {
	    	var dataArray = [];
	    	$(data).each(function(index, item)
	    	{
	    		dataArray.push(
				    {
				    	DisplayName: item.DisplayName,
				    	UserName: item.UserName
				    });
	    	});
	    	return dataArray;
	    };

        self.GetRequiredDataCreateUser = function () {

            self.showImageProcessing_LoadUser('block');
            var ajaxConfig = { Url: "/Admin/GetRequiredDataCreateUser", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
                self.SetRequiredData(data);
                self.showImageProcessing_LoadUser('none');
            });

	        response.fail(function(jqXhr, textStatus)
	        {
		        self.showImageProcessing_LoadUser('none');
	        });
        };

        self.SetRequiredData = function (data) {

	        if (data.AllTeamMemberships != null)
	        {
		        $.each(data.AllTeamMemberships, function(key, value)
		        {
			        var startDate = moment(value.StartDate),
			            endDate = moment(value.EndDate);

			        if (startDate && startDate.isValid())
			            startDate = startDate.format("YYYY-MM-DD");

			        if (endDate && endDate.isValid())
			        	endDate = endDate.format("YYYY-MM-DD");
			        
			        self.userToManage().AllTeamMemberships.push(new ConsoleApp.TeamMembership()
				        .Id(value.Id)
				        .Team(new ConsoleApp.Team().Id(value.Team.Id).Title(value.Team.Title))
				        .StartDate(startDate)
				        .EndDate(endDate));
		        });
	        }

	        if (data.AllFilterCOBs != null) {
                $.each(data.AllFilterCOBs, function (key, value) {
                    self.userToManage().AllFilterCOBs.push(new ConsoleApp.COB()
                            .Id(value.Id)
                            .Narrative(value.Narrative));
                });
            }

            if (data.AllFilterOffices != null) {

                $.each(data.AllFilterOffices, function (key, value) {
                    self.userToManage().AllFilterOffices.push(new ConsoleApp.Office()
                            .Id(value.Id)
                            .Title(value.Title));
                });
            }

            if (data.AllFilterMembers != null) {
                $.each(data.AllFilterMembers, function (key, value) {
                    self.userToManage().AllFilterMembers.push(new ConsoleApp.User()
                            .Id(value.Id)
                            .DomainLogon(value.DomainLogon));
                });
            }

            if (data.AllAdditionalCOBs != null) {
                $.each(data.AllAdditionalCOBs, function (key, value) {
                    self.userToManage().AllAdditionalCOBs.push(new ConsoleApp.COB()
                            .Id(value.Id)
                            .Narrative(value.Narrative));
                });
            }

            if (data.AllAdditionalOffices != null) {
                $.each(data.AllAdditionalOffices, function (key, value) {
                    self.userToManage().AllAdditionalOffices.push(new ConsoleApp.Office()
                            .Id(value.Id)
                            .Title(value.Title));
                });
            }

            if (data.AllAdditionalUsers != null) {
                $.each(data.AllAdditionalUsers, function (key, value) {
                    self.userToManage().AllAdditionalUsers.push(new ConsoleApp.User()
                            .Id(value.Id)
                            .DomainLogon(value.DomainLogon));
                });
            }

            if (data.AllPrimaryOffices != null) {
                $.each(data.AllPrimaryOffices, function (key, value) {
                    self.userToManage().PrimaryOfficeList.push(new ConsoleApp.Office()
                        .Id(value.Id)
                        .Title(value.Title));
                });
            }

            if (data.AllOriginatingOffices != null) {
                $.each(data.AllOriginatingOffices, function (key, value) {
                    self.userToManage().DefaultOrigOfficeList.push(new ConsoleApp.Office()
                        .Id(value.Id)
                        .Title(value.Title));
                });
            }

            if (data.AllDefaultUnderwriters != null) {
                $.each(data.AllDefaultUnderwriters, function (key, value) {
                    self.userToManage().DefaultUWList.push(new ConsoleApp.User()
                        .Id(value.Id)
                        .DomainLogon(value.DomainLogon));
                });
            }
        };

        self.GetTeamsBasicData = function () {

            self.usersTeamList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetTeamsBasicData", VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                $.each(data, function (key, value) {
                    self.usersTeamList.push({ key: value.Id, name: value.Title });
                });
            });
        };

        self.GetUsers = function () {

            self.userList.removeAll();

            var ajaxConfig = { Url: "/Admin/GetUsersInTeam?teamId=" + ConsoleApp.vmManageUsers.selectedTeam(), VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                self.userList.removeAll();

                if (data.length > 0) {
                    $.each(data, function (key, value) {
                        self.userList.push(value);
                    });
                } else {
                    self.userList.push("No Users Found in Team");
                }

            });
        };

	    self.GetSelectedUserByName = function(userName, callback)
	    {
		    self.showImageProcessing_LoadUser('block');
		    self.userToManage(new ConsoleApp.User());
		    self.ClearAllLists();

		    var ajaxConfig = { Url: "/Admin/GetSelectedUserByName?userName=" + userName, VerbType: "GET" },
		        response = ConsoleApp.AjaxHelper(ajaxConfig);

		    response.success(function(data)
		    {
			    self.SetUser(data);
			    self.showImageProcessing_LoadUser('none');

			    if (callback) callback();
		    });

		    response.fail(function(xhr, status)
		    {
			    self.showImageProcessing_LoadUser('none');
		    });
	    };

        self.GetUserPersonalSettings = function () {

            self.showImageProcessing_LoadUser('block');
            self.userToManage(new ConsoleApp.User());
            self.ClearAllLists();

            var ajaxConfig = { Url: "/Admin/GetUserPersonalSettings", VerbType: "GET" },
                response = ConsoleApp.AjaxHelper(ajaxConfig);

	        response.success(function(data)
	        {
		        self.SetUser(data);
		        self.showImageProcessing_LoadUser('none');
	        });

            response.fail(function(xhr, status)
            {
                self.showImageProcessing_LoadUser('none');
            });
        };

        self.SetUser = function (data) {

            if (data.Id != null) {
                self.userToManage().Id(data.Id);
                self.userToManage().DomainLogon(data.DomainLogon);
                self.userToManage().PrimaryOffice(new ConsoleApp.Office().Id(data.PrimaryOffice != null ? data.PrimaryOffice.Id : "").Title(data.PrimaryOffice != null ? data.PrimaryOffice.Title : ""));
                self.userToManage().DefaultOrigOffice(new ConsoleApp.Office().Id(data.DefaultOrigOffice != null ? data.DefaultOrigOffice.Id : "").Title(data.DefaultOrigOffice != null ? data.DefaultOrigOffice.Title : ""));
                self.userToManage().DefaultUW(new ConsoleApp.User().Id(data.DefaultUW != null ? data.DefaultUW.Id : "").DomainLogon(data.DefaultUW != null ? data.DefaultUW.DomainLogon : ""));
                self._Underwriter(data.UnderwriterId);
                self._NonLondonBroker(data.NonLondonBroker);
                self.userToManage().UnderwriterId(data.UnderwriterId);

                if (data.PrimaryOfficeList != null) {
                    $.each(data.PrimaryOfficeList, function (key, value) {
                        if (value != null)
                            self.userToManage().PrimaryOfficeList.push(new ConsoleApp.Office().Id(value.Id).Title(value.Title));
                    });
                }

                if (data.DefaultOrigOfficeList != null) {
                    $.each(data.DefaultOrigOfficeList, function (key, value) {
                        if(value != null)
                            self.userToManage().DefaultOrigOfficeList.push(new ConsoleApp.Office().Id(value.Id).Title(value.Title));
                    });
                }

                if (data.DefaultUWList != null) {
                    $.each(data.DefaultUWList, function (key, value) {
                        if (value != null)
                            self.userToManage().DefaultUWList.push(new ConsoleApp.User().Id(value.Id).DomainLogon(value.DomainLogon));
                    });
                }

                self.userToManage().IsActive(data.IsActive);

                if (data.TeamMemberships != null)
                {
                	$.each(data.TeamMemberships, function(key, value)
                	{
                		var startDate = moment(value.StartDate),
							endDate = moment(value.EndDate);

                		if (startDate && startDate.isValid())
                			startDate = startDate.format("YYYY-MM-DD");

                		if (endDate && endDate.isValid())
                			endDate = endDate.format("YYYY-MM-DD");

                        self.userToManage().TeamMemberships.push(new ConsoleApp.TeamMembership()
                                .Id(value.Id)
                                .Team(new ConsoleApp.Team().Id(value.Team.Id).Title(value.Team.Title))
								.PrimaryTeamMembership(value.PrimaryTeamMembership)
								.StartDate(startDate)
                                .EndDate(endDate));
                    });
                }

                if (data.FilterCOBs != null) {
                    $.each(data.FilterCOBs, function (key, value) {
                        self.userToManage().FilterCOBs.push(new ConsoleApp.COB()
                                .Id(value.Id)
                                .Narrative(value.Narrative));
                    });
                }

                if (data.FilterOffices != null) {
                    $.each(data.FilterOffices, function (key, value) {
                        self.userToManage().FilterOffices.push(new ConsoleApp.Office()
                                .Id(value.Id)
                                .Title(value.Title));
                    });
                }

                if (data.FilterMembers != null) {
                    $.each(data.FilterMembers, function (key, value) {
                        self.userToManage().FilterMembers.push(new ConsoleApp.User()
                                .Id(value.Id)
                                .DomainLogon(value.DomainLogon));
                    });
                }

                if (data.AdditionalCOBs != null) {
                    $.each(data.AdditionalCOBs, function (key, value) {
                        self.userToManage().AdditionalCOBs.push(new ConsoleApp.COB()
                                .Id(value.Id)
                                .Narrative(value.Narrative));
                    });
                }

                if (data.AdditionalOffices != null) {
                    $.each(data.AdditionalOffices, function (key, value) {
                        self.userToManage().AdditionalOffices.push(new ConsoleApp.Office()
                                .Id(value.Id)
                                .Title(value.Title));
                    });
                }

                if (data.AdditionalUsers != null) {
                    $.each(data.AdditionalUsers, function (key, value) {
                        self.userToManage().AdditionalUsers.push(new ConsoleApp.User()
                                .Id(value.Id)
                                .DomainLogon(value.DomainLogon));
                    });
                }

                // Set Required Data for the All Lists 
                self.SetRequiredData(data);

            } else {
                toastr.info("No User found");
            }
        };

        self.ClearAllLists = function () {

        	self.userToManage().TeamMemberships.removeAll();
            self.userToManage().AllTeamMemberships.removeAll();

            self.userToManage().FilterCOBs.removeAll();
            self.userToManage().AllFilterCOBs.removeAll();

            self.userToManage().FilterOffices.removeAll();
            self.userToManage().AllFilterOffices.removeAll();

            self.userToManage().FilterMembers.removeAll();
            self.userToManage().AllFilterMembers.removeAll();

            self.userToManage().AdditionalCOBs.removeAll();
            self.userToManage().AllAdditionalCOBs.removeAll();

            self.userToManage().AdditionalOffices.removeAll();
            self.userToManage().AllAdditionalOffices.removeAll();

            self.userToManage().AdditionalUsers.removeAll();
            self.userToManage().AllAdditionalUsers.removeAll();

            self.userToManage().PrimaryOfficeList.removeAll();
            self.userToManage().DefaultOrigOfficeList.removeAll();
            self.userToManage().DefaultUWList.removeAll();
        };

        self.SaveUser = function (action) {

            self.showImageProcessing_SavingUser('block');
            self.saveUser_Status(false);
            
			// Send back to server only what we need...
            var userToSave = new ConsoleApp.User();
            userToSave.Id = self.userToManage().Id;
            userToSave.DomainLogon = self.userToManage().DomainLogon;
            userToSave.PrimaryOffice = self.userToManage().PrimaryOffice;
            userToSave.NonLondonBroker = self.userToManage().NonLondonBroker;
            userToSave.DefaultOrigOffice = self.userToManage().DefaultOrigOffice;
            userToSave.DefaultUW = self.userToManage().DefaultUW;
            userToSave.IsActive = self.userToManage().IsActive;
            userToSave.UnderwriterId = self.userToManage().UnderwriterId;
            userToSave.TeamMemberships = self.userToManage().TeamMemberships;
            userToSave.FilterCOBs = self.userToManage().FilterCOBs;
            userToSave.FilterOffices = self.userToManage().FilterOffices;
            userToSave.FilterMembers = self.userToManage().FilterMembers;
            userToSave.AdditionalCOBs = self.userToManage().AdditionalCOBs;
            userToSave.AdditionalOffices = self.userToManage().AdditionalOffices;
            userToSave.AdditionalUsers = self.userToManage().AdditionalUsers;
            userToSave.AllTeamMemberships = self.userToManage().AllTeamMemberships;

            var data = ko.toJSON(userToSave);
            var ajaxConfig = { Url: "/Admin/" + action, VerbType: "POST", Data: data };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {
            	
            	toastr.success("User successfully saved");
	            
            	self.GetSelectedUserByName(self.userToManage().DomainLogon(), function() { toastr.success("User synchronised"); });

                self.showImageProcessing_SavingUser('none');
                self.saveUser_Status(true);
                self.saveMode("EDIT");
	            
                ConsoleApp.vmManageTeams.GetTeamsFullData();
            });

	        response.fail(function(xhr, status)
	        {
		        self.showImageProcessing_SavingUser('none');
		        self.saveUser_Status(true);
	        });
                                          };

        self.SearchForUser = function () {

        	self.userSearchList.removeAll();
            self.userToManage(new ConsoleApp.User());
            self.ClearAllLists();

            var ajaxConfig = { Url: "/Admin/SearchForUserByName?userName=" + self.searchUserNameTxt(), VerbType: "GET" };

            var response = ConsoleApp.AjaxHelper(ajaxConfig);

            response.success(function (data) {

                if (data.length > 0) {
                    $.each(data, function (key, value) {
                        self.userSearchList.push({ Id: value.Id, DomainLogon: value.DomainLogon });
                    });
                }
                else {
                    toastr.info("No Users Found");
                }

            });
        };

        // Events
        self.onUserSearchListSelected = function () {
            self.GetSelectedUserByName(this.DomainLogon, null);
        };

        self.click_SearchForUser = function () {
            ConsoleApp.vmManageUsers.SearchForUser();
        };

        self.click_ShowCreateUser = function () {
            self.PageSetup("CREATE_NEW", undefined);
        };

        self.click_ShowSearchArea = function () {
            self.PageSetup("EDIT_SEARCH", undefined);
        };

        // Team Memberships
        self.click_RemoveTeamMemberships = function () {
        	if (self.selectedItem() != null)
        	{
        		if (self.userToManage().TeamMemberships.indexOf(self.selectedItem()) !== -1) {
        			self.userToManage().TeamMemberships.remove(self.selectedItem());
        			self.userToManage().AllTeamMemberships.unshift(self.selectedItem().IsCurrent(false).PrimaryTeamMembership(false));
        			self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        self.click_AddTeamMemberships = function () {
        	if (self.selectedItem() != null)
        	{
        		if (self.userToManage().AllTeamMemberships.indexOf(self.selectedItem()) !== -1)
            	{
        			self.userToManage().AllTeamMemberships.remove(self.selectedItem());
        			self.userToManage().TeamMemberships.unshift(self.selectedItem().IsCurrent(true));
        			self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        // FilterCOBs
        self.click_RemoveFilterCOBs = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().FilterCOBs.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().FilterCOBs.remove(self.selectedItem());
                    self.userToManage().AllFilterCOBs.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

	    self.click_AddFilterCOBs = function()
	    {
		    if (self.selectedItem() != null)
		    {
			    if (self.userToManage().AllFilterCOBs.indexOf(self.selectedItem()) !== -1)
			    {
				    self.userToManage().AllFilterCOBs.remove(self.selectedItem());
				    self.userToManage().FilterCOBs.unshift(self.selectedItem());
				    self.selectedItem(null);
			    }
			    else
			    {
				    toastr.info("No value selected");
			    }
		    }
		    else
		    {
			    toastr.info("No value selected");
		    }
	    };

        // FilterOffices
        self.click_RemoveFilterOffices = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().FilterOffices.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().FilterOffices.remove(self.selectedItem());
                    self.userToManage().AllFilterOffices.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        self.click_AddFilterOffices = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AllFilterOffices.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AllFilterOffices.remove(self.selectedItem());
                    self.userToManage().FilterOffices.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        // FilterMembers
        self.click_RemoveFilterMembers = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().FilterMembers.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().FilterMembers.remove(self.selectedItem());
                    self.userToManage().AllFilterMembers.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        self.click_AddFilterMembers = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AllFilterMembers.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AllFilterMembers.remove(self.selectedItem());
                    self.userToManage().FilterMembers.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        // AdditionalCOBs
        self.click_RemoveAdditionalCOBs = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AdditionalCOBs.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AdditionalCOBs.remove(self.selectedItem());
                    self.userToManage().AllAdditionalCOBs.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        self.click_AddAdditionalCOBs = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AllAdditionalCOBs.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AllAdditionalCOBs.remove(self.selectedItem());
                    self.userToManage().AdditionalCOBs.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        // AdditionalOffices
        self.click_RemoveAdditionalOffices = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AdditionalOffices.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AdditionalOffices.remove(self.selectedItem());
                    self.userToManage().AllAdditionalOffices.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        self.click_AddAdditionalOffices = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AllAdditionalOffices.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AllAdditionalOffices.remove(self.selectedItem());
                    self.userToManage().AdditionalOffices.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        // AdditionalUsers
        self.click_RemoveAdditionalUsers = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AdditionalUsers.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AdditionalUsers.remove(self.selectedItem());
                    self.userToManage().AllAdditionalUsers.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        self.click_AddAdditionalUsers = function () {
            if (self.selectedItem() != null) {
                if (self.userToManage().AllAdditionalUsers.indexOf(self.selectedItem()) !== -1) {
                    self.userToManage().AllAdditionalUsers.remove(self.selectedItem());
                    self.userToManage().AdditionalUsers.unshift(self.selectedItem());
                    self.selectedItem(null);
                } else {
                    toastr.info("No value selected");
                }
            } else {
                toastr.info("No value selected");
            }
        };

        self.onClickSetSelectedItem = function () {
        	//console.log('onClickSetSelectedItem()');
        	self.selectedItem(this);
        };

        self.click_SaveUser = function () {

            var form = $('#formCreateEditUser');

	        //console.log('check is form valid');
            if (form.valid()) {
            	if (self.saveMode() === "EDIT")
            	{
            		//console.log('SaveUser - Edit Mode');
                    self.SaveUser("EditUser");
            	}
            	else if (self.saveMode() === "CREATE")
            	{
            		//console.log('SaveUser - Create Mode');
                    self.SaveUser("CreateUser");
                }
            }
        };

        self.click_PrimaryTeamMembership = function(item, event) {

	        if (item.PrimaryTeamMembership()) {
	        	item.PrimaryTeamMembership(false);
	        } else {
	        	item.PrimaryTeamMembership(true);
	        }
	        
	        $.each(self.userToManage().TeamMemberships(), function(key, value)
	        {
	        	if (self.userToManage().TeamMemberships.indexOf(item) !== key)
	        	{
	        		value.PrimaryTeamMembership(false);
	        	}
	        });

        };

    	// Subscribe
        self.selectedTeam.subscribe(function (newVal) {
            if (newVal !== undefined)
                self.GetUsers();
        });

        self.selectedUserName.subscribe(function (newVal) {
        	if (newVal !== undefined) {
        		self._Underwriter('');
		        self.GetSelectedUserByName(newVal, null);
	        }
        });
	    
        self._Underwriter.subscribe(function (newVal) {
        	if (newVal !== undefined)
        		self.userToManage().UnderwriterId(newVal);
        });
	    
        self._NonLondonBroker.subscribe(function(newVal)
        {
        	if (newVal !== undefined)
        		self.userToManage().NonLondonBroker(newVal);
        });
	    
        $.pubsub.subscribe('primaryOfficeChanged', function(topic, msg) {

	        if (msg.officeTitle !== 'London') {
		        self.showNonLondonBroker(true);
	        } else {
	        	self.showNonLondonBroker(false);
	        	self._NonLondonBroker('');
		        $('.nonLondonBroker').val('');
	        }
        });
	    
        return self;
    };

});