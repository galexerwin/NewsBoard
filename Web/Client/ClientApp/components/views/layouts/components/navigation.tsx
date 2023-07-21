/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
// properties
type Props = RouteComponentProps<{}>;
// react function
const Navigation: React.FC<Props> = (props: Props) => {
    return <div className="tempNavigation">
        <ul>
            <li><a href="/">Home</a></li>
            <li><a href="/about">About</a></li>
            <li><a href="/support">Support</a></li>
            <li><a href="/login">Login</a></li>
        </ul>
    </div>;
}
// export for inclusion
export default Navigation;