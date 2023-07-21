/*	Author:		Alex Erwin
	Purpose:	Move Newsletter to Folder
	Todo:
		Index on htmlkey column
		Filter by memberid (Azure)
*/
CREATE PROCEDURE [dbo].[sp_newsletter_move] (@memberGUID nvarchar(255), @folderGUID nvarchar(255), @newsletterGUID nvarchar(255)) AS
BEGIN
-- variables
DECLARE @memberID int, @folderID bigint, @newsletterID bigint
-- return table
DECLARE @returnTable TABLE ([id] nvarchar(255), [from] nvarchar(255), [subject] nvarchar(255), [date] datetime, [read] varchar(10), [folder] nvarchar(255));
-- retrieve the localID from the azureID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- retrieve the newsletter ID
SELECT	@newsletterID = pkid
FROM	tbl_newsletter_metadata
WHERE	html_storage_key = @newsletterGUID
-- retrieve the folderID
SELECT  @folderID = pkid
FROM	tbl_member_defined_folders
WHERE	CONVERT(nvarchar(255), folder_guid) = @folderGUID
-- check if the newsletter exists
IF EXISTS (SELECT * FROM tbl_newsletter_metadata WHERE pkid = @newsletterID)
	BEGIN
	-- check if the newsletter belongs to the user
	IF EXISTS (SELECT * FROM tbl_newsletter_metadata WHERE pkid = @newsletterID AND member_id = @memberID)
		BEGIN
		-- check if the target folder exists
		IF EXISTS (SELECT * FROM tbl_member_defined_folders WHERE pkid = @folderID) OR (@folderGUID = 'INBOX') OR (@folderGUID = 'TRASH')
			BEGIN
			-- delete all instances of this label association
			DELETE FROM	tbl_member_defined_labels 
			WHERE		newsletter_id = @newsletterID
			-- check the type of move
			IF (@folderGUID = 'INBOX' OR @folderGUID = 'TRASH')
				BEGIN
				-- if we are moving to inbox
				IF (@folderGUID = 'INBOX')
					UPDATE	tbl_newsletter_metadata
					SET		is_inboxed = 1,
							is_labeled = 0,
							is_trashed = 0
					WHERE	pkid = @newsletterID
				-- if we are trashing it
				IF (@folderGUID = 'TRASH')
					UPDATE	tbl_newsletter_metadata
					SET		is_inboxed = 0,
							is_labeled = 0,
							is_trashed = 1
					WHERE	pkid = @newsletterID
				END
			ELSE
				BEGIN
				-- only add a new definition if it doesn't exist
				IF NOT EXISTS (SELECT * FROM tbl_member_defined_labels WHERE newsletter_id = @newsletterID AND folder_id = @folderID)
					BEGIN
					-- insert the data
					INSERT INTO tbl_member_defined_labels (newsletter_id, folder_id) 
						VALUES (@newsletterID, @folderID)
					-- update the location
					UPDATE	tbl_newsletter_metadata
					SET		is_inboxed = 0,
							is_labeled = 1,
							is_trashed = 0
					WHERE	pkid = @newsletterID
					END
				END
			-- return positive result
			RETURN 1;
			END
		-- invalid destination
		RETURN -3;
		END
	ELSE
		BEGIN
		-- item doesn't belong to user
		RETURN -2;
		END
	END
ELSE
	BEGIN
	-- newsletter doesn't exist
	RETURN -1;
	END
END