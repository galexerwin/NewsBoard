﻿/*
 *
*/
// import components
import * as React from "react";
import { Route, RouteProps, Redirect } from "react-router";
// properties being routed along with a layout type and status code from https call
export interface IProps extends RouteProps {
    layout: React.ComponentClass<any>;
    statusCode?: number;
}
// react function to route the request using the appropriate layout and checking for logged in
const AppRoute: React.FC<IProps> =
    ({ component: Component, layout: Layout, statusCode: statusCode, path: Path, ...rest }: IProps) => {

        var isLoginPath = Path === "/login";

        //if (!SessionManager.isAuthenticated && !isLoginPath) {
        //    return <Redirect to="/login" />;
        //}

        //if (SessionManager.isAuthenticated && isLoginPath) {
        //    return <Redirect to="/" />;
        //}

        //if (statusCode == null) {
        //    responseContext.statusCode = 200;
        //} else {
        //    responseContext.statusCode = statusCode;
        //}

        return <Route {...rest} render={props => (
            <Layout>
                <Component {...props} />
            </Layout>
        )} />;
    };
// exported for inclusion
export default AppRoute;