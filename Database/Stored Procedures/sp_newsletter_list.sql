/*	Author:		Alex Erwin
	Purpose:	Fetch Newsletter Meta List
	Todo:
		Index on MemberID
		Filter by memberid (Azure)
		Filter by folder or inbox
*/
CREATE PROCEDURE [dbo].[sp_newsletter_list] (@memberGUID nvarchar(255), @folderGUID nvarchar(255) = NULL, @offset int = 1, @resultCount int = 0 OUT) AS
BEGIN
-- variables
DECLARE @memberID int, @itemMaxCount int
DECLARE @start int, @end int
-- return table
DECLARE @returnTable TABLE ([id] nvarchar(255), [email_addr] nvarchar(255), [sender_name] nvarchar(255), [subject] nvarchar(255), [date] datetime, [read_status] varchar(10), [folder] nvarchar(255));
-- retrieve the localID from the azureID
SELECT	@memberID = pkid, 
		@itemMaxCount = COALESCE(default_page_count, 10)
FROM	tbl_member_accounts
WHERE	azure_ad_identifier = @memberGUID
-- if folder is null, set to inbox
IF (@folderGUID IS NULL)
	SET @folderGUID = 'INBOX';
-- check if items for the user exist
IF EXISTS (SELECT * FROM tbl_newsletter_metadata WHERE member_id = @memberID)
	BEGIN
	-- insert data from view
	INSERT INTO @returnTable ([id], [email_addr], [sender_name], [subject], [read_status], [folder], [date])
		SELECT	[id], [from], [sender], 
				[subject], [read], [folder],
				[date]
		FROM	vw_member_folder_contents
		WHERE	member_id = @memberID AND
				folder = @folderGUID
	-- capture result count
	SELECT	@resultCount = COUNT(*)
	FROM	@returnTable
	-- check the results
	IF (@resultCount = 0)
		RETURN 0;
	-- apply pagination filters
	SET @offset = @offset - 1; -- adjust to a multiplier
	SET @start	= (@offset * @itemMaxCount) + 1; -- always start on the first number in the set
	SET @end	= (@start - 1) + @itemMaxCount; -- always end on last number in the set
	-- return rows
	SELECT	[id], [email_addr], [sender_name], [subject], [date], [read_status]
	FROM 
	(
		SELECT	[id], [email_addr], [sender_name], [subject], [date], [read_status], 
				ROW_NUMBER() OVER (ORDER BY ID) AS RowNum
		FROM	@returnTable

	) AS derived
	WHERE derived.RowNum BETWEEN @start AND @end
	-- return a positive result to the logic app
	RETURN 1;
	END
ELSE
	BEGIN
	-- error needs to be thrown
	RETURN -1;
	END
END