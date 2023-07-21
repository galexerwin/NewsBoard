/*  Author: Alex Erwin
    Purpose: Table for Email Metadata
    Todo:
        Link Foreign Keys MemberID, PublisherID
        Issue Relation?
*/
CREATE TABLE [dbo].[tbl_newsletter_metadata]
(
	[pkid] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [member_id] INT NULL, -- member for which the newsletter is for
    [publisher_id] INT NULL, -- identified safe publisher (publication id instead?)
    [author_id] INT NULL, -- identified author
    [html_storage_key] NVARCHAR(100) NULL, -- storage account unique
    [received_date] DATETIME NOT NULL DEFAULT GETDATE(), -- date of receipt
    [raw_from] NVARCHAR(100) NULL, -- raw data
    [raw_to] NVARCHAR(100) NULL,
    [raw_subject] NVARCHAR(200) NULL,
    [is_inboxed] BIT NOT NULL DEFAULT 1, -- if the item is in the inbox
    [is_labeled]  BIT NOT NULL DEFAULT 0, -- if the item has been placed into a folder (label)
    [is_favorited] BIT NOT NULL DEFAULT 0, -- if the item has been favorited
    [is_trashed] BIT NOT NULL DEFAULT 0, -- if the item has been marked for deletion
    [was_read] BIT NOT NULL DEFAULT 0 -- if item has been read
)