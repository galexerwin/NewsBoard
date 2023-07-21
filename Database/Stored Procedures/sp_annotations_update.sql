/*	Author:		Alex Erwin
	Purpose:	Update Newsletter Annotation
*/
CREATE PROCEDURE [dbo].[sp_annotations_update] (@annotation nvarchar(MAX), @annotationGUID nvarchar(255), @newsletterGUID nvarchar(255), @memberGUID nvarchar(255)) AS
BEGIN
-- variables
DECLARE @memberID int, @newsletterID bigint, @annotationID bigint
-- retrieve the localID from the azureID
SELECT	@memberID = pkid
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- retrieve the newsletter ID
SELECT	@newsletterID = pkid
FROM	tbl_newsletter_metadata
WHERE	html_storage_key = @newsletterGUID
-- retrieve the annotation ID
SELECT	@annotationID = pkid 
FROM	tbl_newsletter_annotations
WHERE	CONVERT(nvarchar(255), annotation_GUID) = @annotationGUID
-- check if the newsletter exists
IF EXISTS (SELECT * FROM tbl_newsletter_metadata WHERE pkid = @newsletterID)
	BEGIN
	-- check if the newsletter belongs to the user
	IF EXISTS (SELECT * FROM tbl_newsletter_metadata WHERE pkid = @newsletterID AND member_id = @memberID)
		BEGIN
		-- check if the annotation exists
		IF EXISTS (SELECT * FROM tbl_newsletter_annotations WHERE pkid = @annotationID AND newsletter_id = @newsletterID)
			BEGIN
			-- update annotation
			UPDATE	tbl_newsletter_annotations
			SET		annotation_txt = @annotation
			WHERE	pkid = @annotationID AND 
					newsletter_id = @newsletterID
			-- return a positive result to the logic app
			RETURN 1;
			END
		ELSE
			BEGIN
			-- error needs to be thrown
			RETURN -2;
			END
		END
	ELSE
		BEGIN
		-- error needs to be thrown
		RETURN 0
		END
	END
ELSE
	BEGIN
	-- error needs to be thrown
	RETURN 0
	END
END