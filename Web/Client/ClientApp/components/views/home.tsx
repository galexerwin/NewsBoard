/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
// routed properties
type Props = RouteComponentProps<{}>;
// react function with routed properties
const Home: React.FC<Props> = () => {
    return <div>
        <p>Home Page</p>
    </div>;
}
// export for inclusion
export default Home;