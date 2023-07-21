/*	Author:		Alex Erwin
	Purpose:	Member Defined Folder Move (order)
*/
CREATE PROCEDURE [dbo].[sp_member_folder_move] (@folderGUID nvarchar(255), @folderNewIndex int, @memberGUID nvarchar(255)) AS
BEGIN
-- variables
DECLARE @memberID int, @folderID bigint, @currentFolderAtIndex bigint, @folderOldIndex int, @allFoldersMaxIndex int
-- retrieve the localID from the azureID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- retrieve the folderID
SELECT  @folderID = pkid, @folderOldIndex = folder_order
FROM	tbl_member_defined_folders
WHERE	CONVERT(nvarchar(255), folder_guid) = @folderGUID
-- retrieve the count of folders belonging to this user
SELECT	@allFoldersMaxIndex = COUNT(*)
FROM	tbl_member_defined_folders
WHERE	member_id = @memberID
-- check to see if the folder exists
IF EXISTS (SELECT * FROM tbl_member_defined_folders WHERE pkid = @folderID AND member_id = @memberID)
	BEGIN
	-- check if we are attempting to move the folder to it's current location
	IF (@folderNewIndex <> @folderOldIndex)
		BEGIN
		-- get the ID of the folder at the index we are attempting to move
		SELECT	@currentFolderAtIndex = pkid
		FROM	tbl_member_defined_folders
		WHERE	member_id = @memberID AND folder_order = @folderNewIndex
		-- shift all the folders

		RETURN 1
		END
	-- return a positive result to the logic app
	RETURN 1
	END
ELSE
	BEGIN
	-- error needs to be thrown
	RETURN 0
	END
END