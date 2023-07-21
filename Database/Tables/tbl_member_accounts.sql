/*  Author: Alex Erwin
    Purpose: Table for Member Accounts
    Todo:

*/
CREATE TABLE [dbo].[tbl_member_accounts]
(
	[pkid] INT NOT NULL PRIMARY KEY IDENTITY, -- member id
	[azure_ad_identifier] NVARCHAR(100) NULL, -- azure ad
	[local_identifier] NVARCHAR(100) NULL, -- email address on newsboard.email
	[primary_email] NVARCHAR(100) NULL, -- email address of the user for notifications
	[given_name] NVARCHAR(50) NULL, -- first name
	[sur_name] NVARCHAR(75) NULL, -- last name
    [default_page_count] INT NULL DEFAULT 10 -- page count
)
