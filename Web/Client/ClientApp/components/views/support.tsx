/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
// routed properties
type Props = RouteComponentProps<{}>;
// react function with routed properties
const Support: React.FC<Props> = () => {
    return <div>
        <p>Support Page</p>
    </div>;
}
// export for inclusion
export default Support;