// import components
import * as React from "react";
import { RouteComponentProps, useLocation } from "react-router";
import { useHistory } from "react-router-dom";
import Modal from "./modal"

// svg generators
const BackNavigationSVG = () => {
    return (
        <svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" x="0px" y="0px" viewBox="0 0 26.676 26.676">
	        <path d="M26.105,21.891c-0.229,0-0.439-0.131-0.529-0.346l0,0c-0.066-0.156-1.716-3.857-7.885-4.59
            c-1.285-0.156-2.824-0.236-4.693-0.25v4.613c0,0.213-0.115,0.406-0.304,0.508c-0.188,0.098-0.413,0.084-0.588-0.033L0.254,13.815
            C0.094,13.708,0,13.528,0,13.339c0-0.191,0.094-0.365,0.254-0.477l11.857-7.979c0.175-0.121,0.398-0.129,0.588-0.029
            c0.19,0.102,0.303,0.295,0.303,0.502v4.293c2.578,0.336,13.674,2.33,13.674,11.674c0,0.271-0.191,0.508-0.459,0.562
            C26.18,21.891,26.141,21.891,26.105,21.891z"/>
        </svg>
    )
}

// react function with routed properties
const Message: React.FC<string> = () => {
    var [loaded, setLoaded] = React.useState(false);
    var [notesLoaded, setNotesLoaded] = React.useState(false);
    var [createNoteButtonClicked, setCreateNoteButtonClicked] = React.useState(false);
    var [createNoteString, setCreateNoteString] = React.useState("");
    var props = defaultProps;
    const query = new URLSearchParams(useLocation().search);
    const history = useHistory();
    const newsletter_id = query.get("msg");
    // set host and port
    var host = window.location.hostname;
    var port = window.location.port;
    // get url builder
    function getURL(path: string): string {
        return "https://" + host + ((port != '80') ? ':' + port : '') + path;
    }
    function checkIfEnter(event) {
        if (event.key === 'Enter'){
            setNotesLoaded(false);
            fetch(getURL("/api/notes/add/"+newsletter_id), {
                method: "POST",
                body: JSON.stringify({
                    "note": createNoteString
                }),
                headers: {
                    "Content-Type": "application/json"
                }
            }).then(() => {
                fetch(getURL("/api/notes/get/"+newsletter_id))
                .then(res => res.json())
                .then(
                    (result) => {
                        const new_notes = [];
                        result.data.annotations.forEach((notes) => {
                            new_notes.push(notes.note);
                        });
                        props.notes = new_notes;
                        setNotesLoaded(true);
                    }
                );
            });
            setCreateNoteString("");
            setCreateNoteButtonClicked(false);
        }
    }

    React.useEffect(() => {
        console.log("Calling: "+"https://localhost:44354/api/message/get/"+newsletter_id)
        fetch(getURL("/api/message/get/"+newsletter_id))
        .then(response => response.json())
        .then(
            (result) => {
                result = result.data.content.content
                props.newsletter_html = atob(result.newsletter);
                props.author_info = result.sender;
                document.title = result.subject;
                setLoaded(true);
            },
            (error) => {
                console.log(error);
            }
        );
        fetch(getURL("/api/notes/get/"+newsletter_id))
        .then(res => res.json())
        .then(
            (result) => {
                const new_notes = [];
                result.data.annotations.forEach((notes) => {
                    new_notes.push(notes.note);
                });
                props.notes = new_notes;
                setNotesLoaded(true);
            }
        );
    }, []);
    

    return <div className="modalRow">
            <div className="modalColumn modalSide" style={{paddingLeft: "2%"}}>
                <div style={{height: "17%", marginBottom: "10%"}}>
                    <Modal>
                        <h5>Author</h5>
                        {loaded ? (
                            <div>
                                <p style={{ textAlign: "center"}}>
                                    {props.author_info}
                                </p>
                            </div>
                        ) : (
                            <div>
                                <p style={{ textAlign: "center", fontSize: "18px"}}>
                                    Loading...
                                </p>
                            </div>
                        )}
                    </Modal>
                </div>
                <div style={{height: "80%"}}>
                    <Modal>
                        <h5>Past Issues</h5>
                        <div>
                        {loaded ? (
                            <ul style={{ textAlign: "left"}}>
                                {props.past_issues.map((issue) => 
                                    <li>{issue}</li>)}
                            </ul>
                        ) : (
                            <p style={{ textAlign: "center", fontSize: "18px"}}>
                                Loading...
                            </p>
                        )}
                        </div>
                    </Modal>
                </div>
            </div>
            <div className="modalColumn modalCenter">
                <Modal>
                    <button
                     className="newsletterBackButton"
                     onClick={history.goBack}>
                        {BackNavigationSVG()}
                    </button>
                    {loaded ? (
                        <div
                        className="newsLetter"
                        dangerouslySetInnerHTML={{ __html: props.newsletter_html }} />
                    ) : (
                        <div className="newsLetter">
                            <p style={{ textAlign: "center", fontSize: "18px"}}>
                                Loading...
                            </p>
                        </div>
                    )}
                </Modal>
            </div>
            <div className="modalColumn modalSide" style={{paddingRight: "2%"}}>
                <Modal>
                    <h5>Notes</h5>
                    <div>
                        {notesLoaded ? (
                            <ul style={{ textAlign: "left", paddingLeft: "20px"}}>
                                {props.notes.map((note) =>
                                    <li>{note}</li>)}
                            </ul>
                        ) : (
                            <p style={{ textAlign: "center", fontSize: "18px"}}>
                                Loading...
                            </p>
                        )}
                    </div>
                    <div>
                        {createNoteButtonClicked ? (
                            <textarea 
                            autoFocus
                            wrap="soft"
                            style={{ height: "auto" }}
                            value={createNoteString} 
                            onBlur={() => {
                                setCreateNoteString("");
                                setCreateNoteButtonClicked(false);
                            }}
                            onChange={(event) => 
                                {setCreateNoteString(event.target.value)}}
                            onKeyDown={checkIfEnter}
                            className="createTag" />
                        ) : (
                            <button
                                onClick={() => {setCreateNoteButtonClicked(true);}}
                                className="createTag">
                                    New Note
                            </button>
                        )}
                    </div>
                </Modal>
            </div>
    </div>;
}

const defaultProps = {
    author_info: "",
    past_issues: [],
    newsletter_html: "",
    notes: []

}
// export for inclusion
export default Message;