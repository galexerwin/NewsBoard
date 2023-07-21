/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
// routed properties
type Props = RouteComponentProps<{}>;
// react function with routed properties
const UrlNotFound: React.FC<Props> = () => {
    return <div>
        <p>404 Error Oops</p>
    </div>;
}
// export for inclusion
export default UrlNotFound;