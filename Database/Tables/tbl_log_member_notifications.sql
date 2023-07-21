/*  Author: Alex Erwin
    Purpose: Table for Outbound Notifications
    Todo:
        Link Foreign Keys MemberID
*/
CREATE TABLE [dbo].[tbl_log_member_notifications]
(
	[pkid] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[member_id] INT NULL, -- member the notification is for
	[notify_date] DATETIME NOT NULL DEFAULT GETDATE() -- date notification was sent
)
