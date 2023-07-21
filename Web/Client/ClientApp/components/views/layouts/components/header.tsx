/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
import { ToastContainer } from "react-toastify";
import Navigation from "@Components/views/layouts/components/navigation";
// local properties
interface IProps {
    children?: React.ReactNode;
}
// combined properties
type Props = IProps & RouteComponentProps<any>;
// react function with routed properties to use in the navigation element
const Header: React.FC<Props> = (props: Props) => {
    return <header className="tempHeader">
        <div><Navigation {...props} /></div>
    </header>;
}
// export for inclusion
export default Header;