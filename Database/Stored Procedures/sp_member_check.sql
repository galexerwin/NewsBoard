/*	Author:		Alexander Erwin
	Purpose:	Check Member Account Exists
*/
CREATE PROCEDURE [dbo].[sp_member_check](@to nvarchar(255) = NULL, @id nvarchar(255) = NULL) AS
BEGIN
-- perform lookup
IF (@to IS NOT NULL) AND EXISTS(SELECT * FROM tbl_member_accounts WHERE local_identifier = @to)
	RETURN 1
ELSE IF (@id IS NOT NULL) AND EXISTS(SELECT * FROM tbl_member_accounts WHERE azure_ad_identifier = @id)
	RETURN 1
ELSE
	RETURN 0
END