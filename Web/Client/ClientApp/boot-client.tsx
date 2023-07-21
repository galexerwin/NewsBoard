// Import polyfills.
import "core-js/stable";
import "custom-event-polyfill";
import "event-source-polyfill";
// Import global styles.
import "bootstrap/dist/css/bootstrap.min.css";
import "react-toastify/dist/ReactToastify.css";
// Other imports.
import * as React from "react";
import * as ReactDOM from "react-dom";
import configureStore from "@Application/store";
import { AppContainer } from "react-hot-loader";
import { Provider } from "react-redux";
import { ConnectedRouter } from "connected-react-router";
import { createBrowserHistory } from "history";
import * as RoutesModule from "@Application/routes";
import { IApplicationState } from "@Components/controllers/index";
let routes = RoutesModule.routes;
// create browser history to use in the Redux store.
const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href")!;
const history = createBrowserHistory({ basename: baseUrl });
// get the application-wide store instance, prepopulating with state from the server where available.
const initialState = (window as any).initialReduxState as IApplicationState;
const store = configureStore(history, initialState);
// render app function
function renderApp() {
    // This code starts up the React app when it runs in a browser. 
    // It sets up the routing configuration and injects the app into a DOM element.
    ReactDOM.hydrate(
        <AppContainer>
            <Provider store={ store }>
                <ConnectedRouter history={ history } children={ routes } />
            </Provider>
        </AppContainer>,
        document.getElementById("react-app")
    );
}
// setup the application and render it.
renderApp();
// allow Hot Module Replacement.
if (module.hot) {
    module.hot.accept("@Application/routes", () => {
        routes = require<typeof RoutesModule>("@Application/routes").routes;
        renderApp();
    });
}