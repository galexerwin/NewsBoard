/*  Author: Alex Erwin
    Purpose: Table for Linking Newsletters to One or More Folders
    Todo:
        Link Foreign Keys newsletter_id, folder_id
*/
CREATE TABLE [dbo].[tbl_member_defined_labels]
(
	[pkid] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[newsletter_id] BIGINT NULL, -- the newsletter
	[folder_id] BIGINT NULL -- the custom folder
)
