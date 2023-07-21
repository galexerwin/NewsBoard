/*
    <Router layout={Guest} exact path="/" component={Home} />
    <Router layout={Guest} exact path="/about" component={About} />
    <Router layout={Guest} exact path="/support" component={Support} />
    <Router layout={Guest} exact path="/login" component={Login} />
    <Router layout={Authorized} exact path="/inbox" component={Inbox} />
    <Router layout={Authorized} exact path="/message" component={Message} />
    <Router layout={Guest} path="*" component={NotFound} statusCode={404} />
*/
// import components
import * as React from 'react';
import { Switch } from 'react-router-dom';
import Guest from "@Components/views/layouts/guest";
import Authorized from "@Components/views/layouts/authorized";
import Router from "@Application/router";
import Home from "@Components/views/home";
import About from "@Components/views/about";
import Support from "@Components/views/support";
import NotFound from "@Components/views/urlnotfound";
import Login from "@Components/views/login";
import Inbox from "@Components/views/inbox";
import Message from "@Components/views/message";
// url routing scheme : matches a layout and component with a path
export const routes = <Switch>
    <Router layout={Authorized} exact path="/newsletter" component={Inbox} />
    <Router layout={Authorized} exact path="/newsletter/inbox" component={Inbox} />
    <Router layout={Authorized} exact path="/newsletter/message" component={Message} />
    <Router layout={Guest} path="*" component={NotFound} statusCode={404} />
</Switch>;