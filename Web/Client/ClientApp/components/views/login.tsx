/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
// routed properties
type Props = RouteComponentProps<{}>;
// react function with routed properties
const Login: React.FC<Props> = () => {
    return <div>
        <p>Login</p>
    </div>;
}
// export for inclusion
export default Login;