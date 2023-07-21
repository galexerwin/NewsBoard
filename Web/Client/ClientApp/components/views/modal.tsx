/*
 *
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";

// import css
import "../../presentation/css/netmodal.css"

// routed properties
type Props = RouteComponentProps<{}>;
// react function with routed properties
class Modal extends React.Component {
    render() {
      return (
        <div className="netmodal" id="modal">
          {this.props.children}
        </div>
      );
    }
  }
// export for inclusion
export default Modal;