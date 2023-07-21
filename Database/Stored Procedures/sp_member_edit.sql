/*	Author:		Alex Erwin
	Purpose:	Add a member
*/
CREATE PROCEDURE [dbo].[sp_member_edit] (@azureID nvarchar(255), @givenName nvarchar(50), @surName nvarchar(75)) AS
BEGIN
-- check if there is an account with either azureID or remote email
IF EXISTS(SELECT * FROM tbl_member_accounts WHERE azure_ad_identifier = @azureID)
	BEGIN
	-- update the user
	UPDATE	tbl_member_accounts 
	SET		given_name = @givenName, sur_name = @surName
	WHERE	azure_ad_identifier = @azureID
	-- return success
	RETURN 1
	END
-- default return unsuccessful
RETURN 0
END