/*	Author:		Alex Erwin
	Purpose:	Add a member
*/
CREATE PROCEDURE [dbo].[sp_member_add] (@azureID nvarchar(255), @localEMAIL nvarchar(255), @remoteEMAIL nvarchar(255), @givenName nvarchar(50), @surName nvarchar(75)) AS
BEGIN

-- check if there is an account with either azureID or remote email
IF NOT EXISTS(SELECT * FROM tbl_member_accounts WHERE azure_ad_identifier = @azureID OR primary_email = @remoteEMAIL OR local_identifier = @localEMAIL)
	BEGIN
	-- insert the user
	INSERT INTO tbl_member_accounts (azure_ad_identifier, primary_email, local_identifier, given_name, sur_name) VALUES
		(@azureID, @remoteEMAIL, @localEmail, @givenName, @surName);
	-- return success
	RETURN 1
	END
-- default return unsuccessful
RETURN 0
END