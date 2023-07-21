CREATE TABLE [dbo].[tbl_newsletter_annotations]
(
	[pkid] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[newsletter_id] BIGINT NULL,
	[annotation_GUID] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	[annotation_txt] NVARCHAR(MAX)
)
