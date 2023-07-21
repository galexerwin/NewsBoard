/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
import { ToastContainer } from "react-toastify";
import Header from "@Components/views/layouts/components/header";
import Footer from "@Components/views/layouts/components/footer";
// properties
interface IProps {
    children?: React.ReactNode;
}
// combine local properties and router properties
type Props = IProps & RouteComponentProps<any>;
// export a default class with properties
export default class Guest extends React.Component<Props, {}> {
    public render() {
        // return the correct layout
        return <div id="guest" className="layout">
            {this.props.children}
        </div>;
    }
}