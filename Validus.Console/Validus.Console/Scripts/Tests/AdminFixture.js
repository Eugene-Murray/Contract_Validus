(function ($) {

	module('Admin TestFixture - vmManageLinks, vmManageTeams, vmManageUsers');
	
	test('Team_SetIsDirtyFlag_Success', function()
	{
		// Assign
		//debugger;
		var vmManageTeams = ConsoleApp.vmManageTeams;

		var team = new ConsoleApp.Team(
			0,
			'Title 1',
			'MOA',
			'DefaultDomicile',
			25,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		);

		// Act
		var initialDirtyFlag = team.dirtyFlag.isDirty();

		team.Title('New Title');
		
		var afterChangeDirtyFlag = team.dirtyFlag.isDirty();

		// Assert
		equal(false, initialDirtyFlag, "initialDirtyFlag is false");
		equal(true, afterChangeDirtyFlag, "afterChangeDirtyFlag is true");
	});
	
	test('Team_ResetToOrigianlValue_Success', function()
	{
		// Assign
		//debugger;
		var team = new ConsoleApp.Team(
			0,
			'Title 1',
			'MOA',
			'DefaultDomicile',
			25,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		);

		// Act
		var initialDirtyFlag = team.dirtyFlag.isDirty();

		team.Title('New Title');

		var afterChangeDirtyFlag = team.dirtyFlag.isDirty();

		team.dirtyFlag.reset();
		
		var afterResetDirtyFlag = team.dirtyFlag.isDirty();

		// Assert
		equal(false, initialDirtyFlag, "initialDirtyFlag is false");
		equal(true, afterChangeDirtyFlag, "afterChangeDirtyFlag is true");
		equal(false, afterResetDirtyFlag, "afterResetDirtyFlag is true");
		equal('Title 1', team.Title(), "Title set back to initial value");
	});


}(jQuery));