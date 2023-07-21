/*	Author:		Alex Erwin
	Purpose:	Member Defined Folder Delete
*/
CREATE PROCEDURE [dbo].[sp_member_folder_delete] (@folderGUID nvarchar(255), @memberGUID nvarchar(255)) AS
BEGIN
-- variables
DECLARE @memberID int, @folderID bigint
-- retrieve the localID from the azureID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- retrieve the folderID
SELECT  @folderID = pkid
FROM	tbl_member_defined_folders
WHERE	CONVERT(nvarchar(255), folder_guid) = @folderGUID
-- check to see if the folder exists
IF EXISTS (SELECT * FROM tbl_member_defined_folders WHERE pkid = @folderID AND member_id = @memberID)
	BEGIN
	-- delete the folder at the index
	DELETE
	FROM	tbl_member_defined_folders
	WHERE	pkid = @folderID AND member_id = @memberID
	-- return a positive result to the logic app
	RETURN 1
	END
ELSE
	BEGIN
	-- error needs to be thrown
	RETURN 0
	END
END