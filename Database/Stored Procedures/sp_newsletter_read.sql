/*	Author:		Alex Erwin
	Purpose:	Fetch Newsletter Meta
	Todo:
		Index on htmlkey column
		Filter by memberid (Azure)
*/
CREATE PROCEDURE [dbo].[sp_newsletter_read] (@newsletterGUID nvarchar(255), @memberGUID nvarchar(255)) AS
BEGIN
-- variables
DECLARE @memberID int, @newsletterID bigint
-- retrieve the localID from the azureID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- retrieve the newsletter ID
SELECT	@newsletterID = pkid
FROM	tbl_newsletter_metadata
WHERE	html_storage_key = @newsletterGUID
-- check if the newsletter exists
IF EXISTS (SELECT * FROM tbl_newsletter_metadata WHERE pkid = @newsletterID)
	BEGIN
	-- check if the newsletter belongs to the user
	IF EXISTS (SELECT * FROM tbl_newsletter_metadata WHERE pkid = @newsletterID AND member_id = @memberID)
		BEGIN
		-- retrieve the selected newsletter
		SELECT	html_storage_key AS [id],
				COALESCE(publisher_name, raw_from) AS [sender],
				raw_subject AS [subject]
		FROM	tbl_newsletter_metadata tnm LEFT JOIN
				tbl_newsletter_publishers tnp ON tnm.publisher_id = tnp.pkid
		WHERE	tnm.pkid = @newsletterID;
		-- update the read bit
		UPDATE	tbl_newsletter_metadata
		SET		was_read = 1
		WHERE	pkid = @newsletterID
		-- return a positive result to the logic app
		RETURN 1;
		END
	ELSE
		BEGIN
		-- error needs to be thrown
		RETURN -1;
		END
	END
ELSE
	BEGIN
	-- error needs to be thrown
	RETURN 0;
	END
END