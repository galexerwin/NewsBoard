/*	Author:		Alex Erwin
	Purpose:	Member Defined Folder Contents
*/
CREATE VIEW [dbo].[vw_member_folder_contents] AS 
	SELECT	tmdl.folder_id, 
			tmdl.newsletter_id, 
			tnm.member_id, 
			html_storage_key AS [id],
			COALESCE(tnp.publisher_name, '') AS [sender],
			raw_from AS [from], 
			raw_subject AS [subject], 
			received_date AS [date],
			CASE 
				WHEN was_read = 0 THEN 'false'
				WHEN was_read = 1 THEN 'true'
			END AS [read],
			CASE
				WHEN tnm.is_inboxed = 1 THEN 'INBOX'
				WHEN tnm.is_favorited = 1 THEN 'FAVORITES'
				WHEN tnm.is_trashed = 1 THEN 'TRASH'
				WHEN tnm.is_labeled = 1 THEN CONVERT(nvarchar(255), tmdf.folder_guid)
			END AS [folder]
	FROM	tbl_newsletter_metadata tnm LEFT JOIN
			tbl_newsletter_publishers tnp ON tnm.publisher_id = tnp.pkid LEFT JOIN
			tbl_member_defined_labels tmdl ON tnm.pkid = tmdl.newsletter_id LEFT JOIN
			tbl_member_defined_folders tmdf ON tmdl.folder_id = tmdf.pkid