/*	Author:		Alex Erwin
	Purpose:	Insert Newsletter
	Additional:	Create New Publisher, New Author
	Todo:	rip from for tagline (relates to the publisher_id)
			scan content and rip for byline (relates to the author_id)
*/
CREATE PROCEDURE [dbo].[sp_newsletter_add](@senderName nvarchar(512), @senderEmail nvarchar(512), @to nvarchar(255), @tagline nvarchar(255), @subject nvarchar(255), @byline nvarchar(1000), @htmlKey nvarchar(100)) AS
BEGIN
-- variables
DECLARE @memberID int, @authorID int, @publisherID int
-- retrieve the memberid from localID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	local_identifier = @to
-- check HTML Key for length, because we should be receiving a newsletter
IF (LEN(@htmlKey) > 0)
	BEGIN
	-- check if local user exists
	IF (@memberID IS NOT NULL)
		BEGIN
		-- check senderName as the publisher
		IF NOT EXISTS (SELECT * FROM tbl_newsletter_publishers WHERE publisher_name = @senderName)
			BEGIN
			-- insert publisher
			INSERT INTO tbl_newsletter_publishers (publisher_name)
				VALUES (@senderName)
			-- copy the id
			SET @publisherID = @@IDENTITY;
			END
		ELSE
			SELECT	@publisherID = pkid
			FROM	tbl_newsletter_publishers
			WHERE	publisher_name = @senderName
		-- store the data
		INSERT INTO tbl_newsletter_metadata (member_id, publisher_id, author_id, html_storage_key, raw_from, raw_to, raw_subject) VALUES
			(@memberID, @authorID, @publisherID, @htmlKey, @senderEmail, @to, @subject)
		END
	END
END