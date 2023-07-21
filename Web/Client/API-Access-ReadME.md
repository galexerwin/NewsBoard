1. All API method endpoints are now in the Web/WebApp/Controllers/APIController
2. Use https://localhost:<port>/api/{member|inbox|message|folders} to access these endpoints.
3. A dummy memberID is setup in the endpoint logic to enable testing.
4. Dates are already UTC
5. Base64 encoded notes will remain base64 encoded in the database and when returned to you. You should decode the response.


Use this to load the inbox : RETURNS List Newsletter JSON
Parameters: None
Sample output for Inbox (first run): GET https://localhost:44397/api/inbox/get | 

{"data":{"newsletters":{"data":[{"id":"169b8b09-095a-46a1-88dd-1497193321f3","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Recreation Newsletter","date":"2021-04-20T21:08:27.4","read_status":"false"},{"id":"452a215e-e3cf-4a3c-9751-879a6844bd8d","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Sample Newsletter","date":"2021-04-20T19:56:12.827","read_status":"false"},{"id":"61dff46f-4f6a-40cf-b6ce-8d58ef51ef40","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"The Morning","date":"2021-04-20T20:26:18.883","read_status":"false"},{"id":"6de65d57-869b-4fad-87b5-4dd0a223239d","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Test Email","date":"2021-04-20T21:29:43.363","read_status":"false"},{"id":"78f66239-ea56-405e-a87a-372e1ef1af5d","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Test","date":"2021-04-20T21:14:37.41","read_status":"false"},{"id":"7c5bccf3-d231-43ba-9cfa-31409b971f12","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"The Morning","date":"2021-04-20T21:17:52.557","read_status":"false"}],"count":""},"folders":{"folders":{"data":"","count":0}}},"error":"","success":true}

Use this to refresh the inbox : RETURNS List Newsletter JSON
Parameters: Required: None, Optional: folderID = {INBOX|FAVORITE|TRASH|folderID}, page = {1...n}
Sample output for Inbox (first run): GET https://localhost:44397/api/inbox/refresh | https://localhost:44397/api/inbox/refresh/{folderID}/{page}

{"data":{"Newsletters":{"data":[{"id":"169b8b09-095a-46a1-88dd-1497193321f3","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Recreation Newsletter","date":"2021-04-20T21:08:27.4","read_status":"false"},{"id":"452a215e-e3cf-4a3c-9751-879a6844bd8d","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Sample Newsletter","date":"2021-04-20T19:56:12.827","read_status":"false"},{"id":"61dff46f-4f6a-40cf-b6ce-8d58ef51ef40","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"The Morning","date":"2021-04-20T20:26:18.883","read_status":"false"},{"id":"6de65d57-869b-4fad-87b5-4dd0a223239d","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Test Email","date":"2021-04-20T21:29:43.363","read_status":"false"},{"id":"78f66239-ea56-405e-a87a-372e1ef1af5d","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Test","date":"2021-04-20T21:14:37.41","read_status":"false"},{"id":"7c5bccf3-d231-43ba-9cfa-31409b971f12","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"The Morning","date":"2021-04-20T21:17:52.557","read_status":"false"}],"count":null}},"error":"","success":true}

Use this to load a message (newsletter) : RETURNS Interface JSON
Parameters: newsletterID
Sample output for Message (first run): GET https://localhost:44397/api/message/get/{newsletterID}

{"data":{"content":{"content":{"id":"169b8b09-095a-46a1-88dd-1497193321f3","sender":"galex.erwin@gmail.com","subject":"Recreation Newsletter","newsletter":"PGRpdiBkaXI9Imx0ciI+PGRpdiBjbGFzcz0iZ21haWwtbkggZ21haWwtaWYiIHN0eWxlPSJtYXJnaW46MHB4IDE2cHggMHB4IDBweDtwYWRkaW5nOjBweCI
...
seTpSb2JvdG8sUm9ib3RvRHJhZnQsSGVsdmV0aWNhLEFyaWFsLHNhbnMtc2VyaWY7Zm9udC1zaXplOm1lZGl1bSI+PGRpdiBjbGFzcz0iZ21haWwtbkgiPjwvZGl2PjwvZGl2PjxiciBjbGFzcz0iZ21haWwtQXBwbGUtaW50ZXJjaGFuZ2UtbmV3bGluZSI+PC9kaXY+DQo=","subject":"Recreation Newsletter"},"error":"","success":true}

Use this to mark message(s) as read/unread: RETURNS OK message
Parameters: state = {unread|read}; put body = {"newsletterID": [{newsletterID},...]} 
Sample output for Message Update: PUT https://localhost:44397/api/message/update/{state}

{"data":"","error":"","success":true}

Use this to delete message(s): RETURNS OK message (possible error collection)
Parameters: None; delete body = {"newsletterID": [{newsletterID},...]} 
Sample output for Message Delete: DELETE https://localhost:44397/api/message/delete

{"data":"","error":"","success":true}

Use this to move message(s): RETURNS OK message (possible error collection)
Parameters: folderID; put body = {"newsletterID": [{newsletterID},...]} 
Sample output for Message Move: PUT https://localhost:44397/api/message/move/{folderID}

{"data":"","error":"","success":true}

Use this to favorite a message (newsletter) : RETURNS OK message
Parameters: newsletterID
Sample output for Message Favorite: GET https://localhost:44397/api/message/favorite/{newsletterID}

{"data":"","error":"","success":true}

Use this to load all folders
Parameters: None
Sample output for Folders All: GET https://localhost:44397/api/folders/get

{"data":{"folders":{"data":[{"id":"AFF7238A-1FD3-4280-801F-EA114C97DF3C","name":"Test","order":0},{"id":"004BA406-61BB-4D41-9B91-92D388CABD10","name":"Test2","order":1}],"count":2}},"error":"","success":true}

Use this to load a single folder : RETURNS List Newsletter JSON
Parameters: Required: folderID = {INBOX|FAVORITE|TRASH|folderID}, Optional page = {1...n}
Sample output for Folder Get: GET https://localhost:44397/api/folders/get/{folderID} | https://localhost:44397/api/folders/get/{folderID}/{page}

{"data":{"newsletters":{"data":[{"id":"169b8b09-095a-46a1-88dd-1497193321f3","email_addr":"galex.erwin@gmail.com","sender_name":"","subject":"Recreation Newsletter","date":"2021-04-20T21:08:27.4","read_status":"false"}],"count":1}},"error":"","success":true}

Use this to create a folder: RETURNS OK message
Hint: Call Load All Folders to refresh
Parameters: None; post body = {"folderName" : string}
Sample output for Folders Add: POST https://localhost:44397/api/folders/add

{"data":"","error":"","success":true}

Use this to delete a folder: RETURNS OK message
Hint: Call Load All Folders to refresh
Parameters: folderID; delete body = None
Sample output for Folders Delete: DELETE https://localhost:44397/api/folders/delete/{folderID}

{"data":"","error":"","success":true}

Use this to load notes: RETURNS Notes JSON by NewsletterID
Parameters: newsletterID
Sample output for Notes: GET https://localhost:44397/api/notes/get/{newsletterID}

{"data":{"annotations":[{"id":"bc0d3532-0279-45fd-b6f2-4c272f96a82e","note":"SGksIEkgYW0gYSBub3RlIQ=="}]},"error":"","success":true}

Use this to add a note: RETURNS OK message
Hint: Call Load Notes to refresh
Parameters: newsletterID; post body = {"note": base64 encoded string}
Sample output for Notes Add: POST https://localhost:44397/api/notes/add/{newsletterID}

{"data":"","error":"","success":true} 

Use this to delete a note: RETURNS OK message
Hint: Call Load Notes to refresh
Parameters: newsletterID; delete body = none
Sample output for Notes Delete: DELETE https://localhost:44397/api/notes/delete/{newsletterID}/{noteID} 

{"data":"","error":"","success":true}

Todo:
Convert from Quoted Printable to Normal to B64