/*	Author:		Alex Erwin
	Purpose:	Member Defined Folder Add
	Notes: Folder Order starts from zero.
*/
CREATE PROCEDURE [dbo].[sp_member_folder_add] (@folderName nvarchar(255), @memberGUID nvarchar(255)) AS
BEGIN
-- variables
DECLARE @memberID int, @currentFolderCount int = 0
-- retrieve the localID from the azureID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- check to see if the folder with that name exists already
IF NOT EXISTS (SELECT * FROM tbl_member_defined_folders WHERE folder_name = @folderName AND member_id = @memberID)
	BEGIN
	-- retrieve the number of custom folders they already have
	SELECT	@currentFolderCount = COUNT(*)
	FROM	tbl_member_defined_folders
	WHERE	member_id = @memberID
	-- insert the folder
	INSERT INTO tbl_member_defined_folders (member_id, folder_name, folder_order) VALUES
		(@memberID, @folderName, @currentFolderCount)
	-- return a positive result to the logic app
	RETURN 1
	END
ELSE
	BEGIN
	-- error needs to be thrown
	RETURN 0
	END
END