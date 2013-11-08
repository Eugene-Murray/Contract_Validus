
ko.TeamDirtyFlag = function(root, isInitiallyDirty)
{
	var result = function() { },
		_initialState = ko.observable(ko.toJSON(root)),
        _isInitiallyDirty = ko.observable(isInitiallyDirty);

	//console.log(_initialState());

	result.isDirty = ko.computed(function()
	{
		return _isInitiallyDirty() || _initialState() !== ko.toJSON(root);
	});

	result.reset = function() {
		//console.log(_initialState());

		if (result.isDirty()) {

			root.Id(JSON.parse(_initialState()).Id);
			root.Title(JSON.parse(_initialState()).Title);
			root.DefaultMOA(JSON.parse(_initialState()).DefaultMOA);
			root.DefaultDomicile(JSON.parse(_initialState()).DefaultDomicile);
			root.QuoteExpiryDaysDefault(JSON.parse(_initialState()).QuoteExpiryDaysDefault);

			var initialAllUsers = JSON.parse(_initialState()).AllUsers;
			if (initialAllUsers !== null && initialAllUsers !== undefined) {
				var allUsers = [];
				$.each(initialAllUsers, function(key, value) {
					var user = value;
					user.IsCurrentMembership = false;
					//console.log(user);
					allUsers.push(user);
				});
				root.AllUsers(allUsers);
			}

			var initialUsers = JSON.parse(_initialState()).Users;
			if (initialUsers !== null && initialUsers !== undefined) {
				var users = [];
				$.each(initialUsers, function(key, value) {
					var user = value;
					user.IsCurrentMembership = true;
					//console.log(user);
					users.push(user);
				});
				root.Users(users);
			}

			var initialAllRelatedCOBs = JSON.parse(_initialState()).AllRelatedCOBs;
			if (initialAllRelatedCOBs !== null)
				root.AllRelatedCOBs(initialAllRelatedCOBs);

			var initialAllRelatedOffices = JSON.parse(_initialState()).AllRelatedOffices;
			if (initialAllRelatedOffices !== null)
				root.AllRelatedOffices(initialAllRelatedOffices);

			var initialAllLinks = JSON.parse(_initialState()).AllLinks;
			if (initialAllLinks !== null)
				root.AllLinks(initialAllLinks);

			var initialRelatedCOBs = JSON.parse(_initialState()).RelatedCOBs;
			if (initialRelatedCOBs !== null)
				root.RelatedCOBs(initialRelatedCOBs);

			var initialRelatedOffices = JSON.parse(_initialState()).RelatedOffices;
			if (initialRelatedOffices !== null)
				root.RelatedOffices(initialRelatedOffices);

			var initialLinks = JSON.parse(_initialState()).Links;
			if (initialLinks !== null)
				root.Links(initialLinks);

			//console.log(root);

			_isInitiallyDirty(false);
		}
	};

	return result;
};

ko.UserDirtyFlag = function(root, isInitiallyDirty) {
};