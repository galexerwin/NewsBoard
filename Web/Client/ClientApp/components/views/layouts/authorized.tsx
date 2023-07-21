/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
import { ToastContainer } from "react-toastify";
import Header from "@Components/views/layouts/components/header";
import Footer from "@Components/views/layouts/components/footer";
//import css
import "../../../presentation/css/authorized.css"
// properties
interface IProps {
    children?: React.ReactNode;
}
// combine local properties and router properties
type Props = IProps & RouteComponentProps<any>;
// export a default class with properties
export default class Authorized extends React.Component<Props, {}> {
    public render() {
        // return the correct layout
        // <Header {...this.props} /><Footer />
        return <div id="authorized" className="authorized">
            {this.props.children}
            <ToastContainer />
        </div>;
    }
}