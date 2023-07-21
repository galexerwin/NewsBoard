/*  Author: Alex Erwin
    Purpose: Table for Member Defined Folders
    Detail:  User is able to define preset categorization of the newsletter data
    Todo:
        Link Foreign Keys MemberID
        Apply sorting and filtering directly? as with a rule?
*/
CREATE TABLE [dbo].[tbl_member_defined_folders]
(
	[pkid] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[member_id] INT NULL,
    [folder_guid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	[folder_name] NVARCHAR(50) NULL,
	[folder_order] INT NULL
)
