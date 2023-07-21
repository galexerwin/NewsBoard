/*
 *
*/
// import components
import * as React from "react";
import { Redirect, RouteComponentProps } from "react-router";
import { useHistory } from "react-router-dom"
import Modal from "./modal"

const MarkReadIcon = () => {
    return (
        <svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
            <path d="M31.781 20.375l-8-10c-0.19-0.237-0.477-0.375-0.781-0.375h-14c-0.304 0-0.591 0.138-0.781 0.375l-8 10c-0.142 0.177-0.219 0.398-0.219 0.625v9c0 1.105 0.895 2 2 2h28c1.105 0 2-0.895 2-2v-9c0-0.227-0.077-0.447-0.219-0.625zM30 22h-7l-4 4h-6l-4-4h-7v-0.649l7.481-9.351h13.039l7.481 9.351v0.649z"></path>
            <path d="M23 16h-14c-0.552 0-1-0.448-1-1s0.448-1 1-1h14c0.552 0 1 0.448 1 1s-0.448 1-1 1z"></path>
            <path d="M25 20h-18c-0.552 0-1-0.448-1-1s0.448-1 1-1h18c0.552 0 1 0.448 1 1s-0.448 1-1 1z"></path>
        </svg>
    )
}

const MarkUnreadIcon = () => {
    return (
    <svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
       <path d="M31.781 20.375l-8-10c-0.19-0.237-0.477-0.375-0.781-0.375h-14c-0.304 0-0.591 0.138-0.781 0.375l-8 10c-0.142 0.177-0.219 0.398-0.219 0.625v9c0 1.105 0.895 2 2 2h28c1.105 0 2-0.895 2-2v-9c0-0.227-0.077-0.447-0.219-0.625zM30 22h-7l-4 4h-6l-4-4h-7v-0.649l7.481-9.351h13.039l7.481 9.351v0.649z"></path>
    </svg>
    )
}

const TrashIcon = () => {
    return (
    <svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
        <path d="M4 10v20c0 1.1 0.9 2 2 2h18c1.1 0 2-0.9 2-2v-20h-22zM10 28h-2v-14h2v14zM14 28h-2v-14h2v14zM18 28h-2v-14h2v14zM22 28h-2v-14h2v14z"></path>
        <path d="M26.5 4h-6.5v-2.5c0-0.825-0.675-1.5-1.5-1.5h-7c-0.825 0-1.5 0.675-1.5 1.5v2.5h-6.5c-0.825 0-1.5 0.675-1.5 1.5v2.5h26v-2.5c0-0.825-0.675-1.5-1.5-1.5zM18 4h-6v-1.975h6v1.975z"></path>
    </svg>
    )
}

const TagIcon = () => {
    return (
    <svg version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
        <path d="M30.5 0h-12c-0.825 0-1.977 0.477-2.561 1.061l-14.879 14.879c-0.583 0.583-0.583 1.538 0 2.121l12.879 12.879c0.583 0.583 1.538 0.583 2.121 0l14.879-14.879c0.583-0.583 1.061-1.736 1.061-2.561v-12c0-0.825-0.675-1.5-1.5-1.5zM23 12c-1.657 0-3-1.343-3-3s1.343-3 3-3 3 1.343 3 3-1.343 3-3 3z"></path>
    </svg>
    )
}



// routed properties
type Props = RouteComponentProps<{}>;
// react function with routed properties
const Inbox: React.FC<Props> = () => {
    if (sessionStorage.getItem("selectedTag") === null){
        var [selectedTag, setSelectedTag] = React.useState("INBOX");
    } else {
        var [selectedTag, setSelectedTag] = React.useState(sessionStorage.getItem("selectedTag"));
    }
    var [customTags, setCustomTags] = React.useState([]);
    var [selectedLetters, setSelectedLetters] = React.useState(new Set<string>());
    var [fetchedLetters, setFetchedLetters] = React.useState([]);
    var [isLoaded, setIsLoaded] = React.useState(false);
    var [tagsAreLoaded, setTagsLoaded] = React.useState(false);
    var [createTagButtonClicked, setCreateTagButtonClicked] = React.useState(false);
    var [createTagString, setCreateTagString] = React.useState("");
    var [hoveredTag, setHoveredTag] = React.useState({});
    var [tagButtonClicked, setTagButtonClicked] = React.useState(false);
    // set host and port
    var host = window.location.hostname;
    var port = window.location.port;
    // How often we check for new messages
    const REFRESH_FREQUENCY = 15000;
    // get url builder
    function getURL(path: string): string {
        return "https://" + host + ((port != '80') ? ':' + port : '') + path;
    }

    function getNavClass(filter: string): string{
        if (filter == selectedTag){
            return "inboxNavButton selectedButton";
        } else {
            if (filter in hoveredTag && hoveredTag[filter] == true){
                return "inboxNavButton selectedButton";
            } else {
                return "inboxNavButton unselectedButton";
            }
            
        }
    }

    function setNavClass(filter: string, status: boolean): void{
        hoveredTag[filter] = status;
        const new_dict = {};
        for (var key in hoveredTag){
            new_dict[key] = hoveredTag[key];
        }
        setHoveredTag(new_dict);
    }

    function getLetterClass(cur_name: string): string{
        if (selectedLetters.has(cur_name)){
            return "inboxNewsletter selectedButton";
        } else {
            return "inboxNewsletter unselectedButton";
        }
    }

    function getReadStatus(letter): string {
        if (letter.read_status){
            return "inboxReadDot readNewsletter";
        } else {
            return "inboxReadDot unreadNewsletter";
        }
    }

    function flipLetterState(letter_id: string): Set<string>{
        // Create new set so react will rerender page
        const new_set = new Set<string>();
        selectedLetters.forEach((letter) =>
            new_set.add(letter)
        );
        if (new_set.has(letter_id)){
            new_set.delete(letter_id);
        } else {
            new_set.add(letter_id);
        }
        return new_set;
    }

    function getNewsletters(first_load: boolean) {

        // Fetch newsletters from server
        if (first_load){
            fetch(getURL("/api/inbox/get"))
            .then(res => res.json())
            .then(
                (result) => {
                    const newLetters = [];
                    result.data.newsletters.data.forEach((letter) => {
                        console.log(letter);
                        letter.date = new Date(letter.date).toLocaleDateString();
                        letter.read_status = (letter.read_status == 'true');
                        newLetters.push(letter);
                    });
                    setFetchedLetters(newLetters);
                    const custom_folders = [];
                    if (result.data.folders.count > 0){
                        result.data.folders.data.forEach((folder) => {
                            custom_folders.push(folder);
                        });
                        custom_folders.sort((x) => x.order);
                    }
                    setCustomTags(custom_folders);
                    setIsLoaded(true);
                    setTagsLoaded(true);
                },
                (error) => {
                    setIsLoaded(false);
                    console.log(error);
                }
            );
        } else {
            // TODO add pagination logic here
            var url_string: string;
            if (selectedTag === 'INBOX'){
                url_string = getURL("/api/inbox/refresh");
            } else {
                url_string = getURL("/api/inbox/refresh/"+selectedTag+"/1");
            }
            fetch(url_string)
            .then(res => res.json())
            .then(
                (result) => {
                    if (result.success == false) {
                        setFetchedLetters([]);
                        setIsLoaded(true);
                        return;
                    }
                    const newLetters = [];
                    result.data.newsletters.data.forEach((letter) => {
                        letter.date = new Date(letter.date).toLocaleDateString();
                        letter.read_status = (letter.read_status == 'true');
                        newLetters.push(letter);
                    });
                    setFetchedLetters(newLetters);
                    setIsLoaded(true);
                },
                (error) => {
                    setIsLoaded(false);
                    console.log(error);
                }
            )
        }
    }

    function markSelectedReadStatus(status: boolean) {
        var status_name;
        if (status){
            status_name = "read";
        } else {
            status_name = "unread";
        }
        console.log("https://localhost:44354/api/message/update/"+status_name);
        fetch(getURL("/api/message/update/"+status_name), {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "newsletterID": Array.from(selectedLetters)
            })
        })
        .then((response) => response.json())
        .then((response) => { 
            console.log(response);
            if (response.success){
                const new_letters = [];
                fetchedLetters.forEach((letter) => {
                    if (selectedLetters.has(letter.id)){
                        letter.read_status = status;
                    }
                    new_letters.push(letter);
                })
                setFetchedLetters(new_letters);
                setIsLoaded(true);
            }
        });
        setSelectedLetters(new Set<string>());
    }

    function markLetterRead(letter_id: string) {
        function anonymousFunc(event) {
            const selected_letter = fetchedLetters.find((letter) => letter.id == letter_id);
            var status_name;
            if (selected_letter.read_status){
                status_name = "read";
            } else {
                status_name = "unread";
            }
            console.log("Calling: "+"https://localhost:44354/api/message/update/"+status)
            fetch(getURL("/api/message/update/" + status_name), {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    "newsletterID": Array.from(letter_id)
                })
            })
            .then((response) => response.json())
            .then((response) => { 
                console.log(response);
                if (response.success){
                    const new_letters = [];
                    fetchedLetters.forEach((letter) => {
                        if (letter.id == letter_id){
                            letter.read_status = !letter.read_status;
                        }
                        new_letters.push(letter);
                    })
                    setFetchedLetters(new_letters);
                    setIsLoaded(true);
                }
            });            
            event.stopPropagation();
        }
        return anonymousFunc;
    }

    function trashSelected() {
        // For now just move things to trash folder
        fetch(getURL("/api/message/move/TRASH"), {
            method: "PUT",
            body: JSON.stringify({
                "newsletterID": Array.from(selectedLetters)
            }),
            headers: {
                "Content-Type": "application/json"
            }
        });
        /*
        fetch(getURL("/api/message/delete"), {
            method: "DELETE",
            body: JSON.stringify({
                "newsletterID": Array.from(selectedLetters)
            }),
            headers: {
                "Content-Type": "application/json"
            }
        })
        */
        const new_newsletters = [];
        fetchedLetters.forEach((letter) => {
            if (!selectedLetters.has(letter.id)){
                new_newsletters.push(letter);
            }
        })
        setFetchedLetters(new_newsletters);
        setSelectedLetters(new Set<string>());
    }

    function setCurrentTag(tag_name: string) {
        sessionStorage.setItem('selectedTag', tag_name);
        setSelectedTag(tag_name);
        setIsLoaded(false);
        if (tagButtonClicked){
            moveSelectedLetters(tag_name);
            setTagButtonClicked(false);
        }
    }

    function moveSelectedLetters(filter: string) {
        setIsLoaded(false);
        setFetchedLetters([]);
        fetch(getURL("/api/message/move/"+filter), {
            method: "PUT",
            body: JSON.stringify({
                "newsletterID": Array.from(selectedLetters)
            }),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(() => {
            setNavClass(filter, false);
            setSelectedLetters(new Set<string>());
            setSelectedTag(filter);
        });
        
    }

    function checkIfEnter(event) {
        if (event.key === 'Enter'){
            setTagsLoaded(false);
            fetch(getURL("/api/folders/add"), {
                method: "POST",
                body: JSON.stringify({
                    "folderName": createTagString
                }),
                headers: {
                    "Content-Type": "application/json"
                }
            }).then(() => {
                fetch(getURL("/api/inbox/get"))
                .then(res => res.json())
                .then(
                    (result) => {
                        const custom_folders = [];
                        result.data.folders.data.forEach((folder) => {
                            custom_folders.push(folder);
                        });
                        custom_folders.sort((x) => x.order);
                        setCustomTags(custom_folders);
                        setTagsLoaded(true);
                    });
            });
            setCreateTagString("");
            setCreateTagButtonClicked(false);
        }
    }

    function handleDragStart(letter_id) {
        // If this wasn't a selected item only select it
        if (selectedLetters.has(letter_id) === false){
            const new_set = new Set<string>();
            new_set.add(letter_id);
            setSelectedLetters(new_set);
        }
        // The other case is if one or more letters is selected
        // in this case, all the selected items will be tagged
    }

    // Perform first time load
    React.useEffect(() => {
        getNewsletters(true);
    }, []);

    var refresh_interval = React.useRef(null);
    React.useEffect(() => {
        clearInterval(refresh_interval.current);
        getNewsletters(false);
        refresh_interval.current = setInterval(
            () => getNewsletters(false),
            REFRESH_FREQUENCY);
    }, [selectedTag]); // Only call again when filter changes
    
    const history = useHistory();
    const default_filters = [["Inbox", "INBOX"],
                            ["Favorites","FAVORITE"],
                            ["Trash", "TRASH"]];
    
    return <div className="modalRow">
            <div className="modalColumn modalNavColumn" style={{paddingLeft: "2%"}}>
                <div style={{height: "19%", marginBottom: "8%"}}>
                    <Modal>
                        <div>
                            <button 
                            title="Mark selected newsletters read"
                            className="tagButton"
                            onClick={() => markSelectedReadStatus(true)}
                            >
                                {MarkReadIcon()}
                            </button>
                            <button 
                            title="Mark selected newsletters unread"
                            className="tagButton"
                            onClick={() => markSelectedReadStatus(false)}>
                                {MarkUnreadIcon()}
                            </button>
                        </div>
                        <div>
                            <button
                            title="Trash selected newsletters"
                            className="tagButton"
                            onClick={() => trashSelected()}>
                                {TrashIcon()}
                            </button>
                            <button 
                            title="Move selected newsletters"
                            className="tagButton"
                            onClick={() => {selectedLetters.size ? (
                                        setTagButtonClicked(true)
                                    ):( 
                                        null)}}>
                                {TagIcon()}
                            </button>
                        </div>
                    </Modal>
                </div>
                <div style={{height: "80%"}}>
                    <Modal>
                        {default_filters.map((filter) =>
                            <div className="lineDiv">
                                <button 
                                    onClick={() => setCurrentTag(filter[1])} 
                                    className={getNavClass(filter[1])}
                                    onDrop={(event) => moveSelectedLetters(filter[1])}
                                    onDragEnter={(event) => setNavClass(filter[1], true)}
                                    onDragLeave={(event) => setNavClass(filter[1], false)}
                                    onDragOver={(event) => event.preventDefault()}>
                                    {filter[0]}
                                </button>
                            </div>
                        )}
                        {tagsAreLoaded ? (
                            customTags.map((folder) =>
                                <div className="lineDiv">
                                    <button 
                                        onClick={() => setCurrentTag(folder.id)} 
                                        className={getNavClass(folder.id)}
                                        onDrop={(event) => moveSelectedLetters(folder.id)}
                                        onDragEnter={(event) => setNavClass(folder.id, true)}
                                        onDragLeave={(event) => setNavClass(folder.id, false)}
                                        onDragOver={(event) => event.preventDefault()}>
                                        {folder.name}
                                    </button>
                                </div>
                            )
                        ) : (
                            <div>
                                <p style={{ marginTop: "10px", textAlign: "center", fontSize: "18px"}}>
                                    Loading...
                                </p>
                            </div>
                        )}
                        <div>
                            {createTagButtonClicked ? (
                                <input 
                                autoFocus
                                type="text" 
                                value={createTagString} 
                                onBlur={() => {
                                    setCreateTagString("");
                                    setCreateTagButtonClicked(false);
                                }}
                                onChange={(event) => 
                                    {setCreateTagString(event.target.value)}}
                                onKeyDown={checkIfEnter}
                                className="createTag" />
                            ) : (
                                <button
                                    onClick={() => {setCreateTagButtonClicked(true);}}
                                    className="createTag">
                                        Create Folder
                                </button>
                            )}
                        </div>
                    </Modal>
                </div>
            </div>
            <div className="modalColumn modalInbox">
                <Modal>
                    <div 
                     className="lineDiv TopRow" 
                     style={{marginRight: "0%", marginLeft: "0%"}}>
                        <div style={{float: "left", width: "50%", paddingBottom: "2px"}}>
                            <p className="inboxTopRow">Title</p>
                        </div>
                        <div style={{float: "left", width: "40%", paddingBottom: "2px"}}>
                            <p className="inboxTopRow">Author</p>
                        </div>
                        <div style={{float: "left", width: "10%", paddingBottom: "2px"}}>
                            <p className="inboxTopRow">Date</p>
                        </div>
                    </div>
                    {isLoaded ? (
                        tagButtonClicked ? (
                            <div style={
                                {"display": "table", 
                                "height": "80%", 
                                "width": "100%", 
                                "overflow": "hidden"}}>
                                <div style={
                                    {"display": "table-cell", 
                                    "textAlign": "center", 
                                    "verticalAlign": "middle"}}>
                                    <h1>
                                    {'<== Select the destination folder'}
                                    </h1>
                                </div>
                            </div>
                        ) : (
                            fetchedLetters.map((letter) =>
                            <div>
                                <button 
                                onClick={() => setSelectedLetters(flipLetterState(letter.id))}
                                className={getLetterClass(letter.id)}
                                draggable="true"
                                onDrag={(event) => (null)}
                                onDragStart={(event) => handleDragStart(letter.id)}
                                onDoubleClick={() => {
                                    clearInterval(refresh_interval.current);
                                    history.push("/newsletter/message?msg=" + letter.id.toString())}}>
                                    <div className="newsletterReadStatus">
                                        <button
                                            className={getReadStatus(letter)}
                                            onClick={markLetterRead(letter.id)}>
                                        </button>
                                    </div>
                                    <div className="newsletterTitle">
                                        {letter.subject}
                                    </div>
                                    <div className="newsletterAuthor">
                                        {letter.email_addr}
                                    </div>
                                    <div className="newsletterDate">
                                        {letter.date}
                                    </div>
                                </button>
                            </div>
                            )
                        )
                     ) : (
                        <div>
                            <p style={{ textAlign: "center", fontSize: "18px"}}>
                                Loading Newsletters...
                            </p>
                        </div>
                     )
                    }
                
                </Modal>
            </div>
    </div>;

}
// export for inclusion
export default Inbox;