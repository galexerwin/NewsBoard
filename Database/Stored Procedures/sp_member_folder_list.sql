/*	Author:		Alex Erwin
	Purpose:	Member Defined Folder List
*/
CREATE PROCEDURE [dbo].[sp_member_folder_list] (@memberGUID nvarchar(255), @resultCount int = 0 OUT) AS
BEGIN
-- variables
DECLARE @memberID int
-- table
DECLARE @returnTable TABLE ([id] nvarchar(255), [name] nvarchar(255), [order] int);
-- retrieve the localID from the azureID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- check if items for the user exist
IF EXISTS (SELECT * FROM tbl_member_defined_folders WHERE member_id = @memberID)
	BEGIN
	-- insert the folders into the return table
	INSERT INTO @returnTable ([id], [name], [order])
		SELECT	CONVERT(nvarchar(255), folder_guid), folder_name, folder_order
		FROM	tbl_member_defined_folders
		WHERE	member_id = @memberID
	-- capture result count
	SELECT	@resultCount = COUNT(*)
	FROM	@returnTable
	-- return the data
	SELECT	*
	FROM	@returnTable
	-- return a positive result to the logic app
	RETURN 1
	END
ELSE
	BEGIN
	-- error needs to be thrown
	RETURN 0
	END
END