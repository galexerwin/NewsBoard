/*  Author:
 *  Purpose:
*/
// import components
import * as React from "react";
import { RouteComponentProps } from "react-router";
// routed properties
type Props = RouteComponentProps<{}>;
// react function with routed properties
const About: React.FC<Props> = () => {
	return <div className="contentblock">
		<div><h1>About Us</h1></div>
		<div>
			<h2>Our Mission</h2>
			<p>Generic Mission Statement</p>
		</div>
		<div>
			<h2>Meet The Team</h2>
			<ul className="listTable">
				<li>
					<div className="empDetail">
						<div className="empPhoto"><img src="images/no-photo-icon.png" /></div>
						<div className="empInfo">
							<h3>Prithula Hridi</h3>
							<p>Information about Prithula</p>
							<p>[E]: phridi@netboard</p>
						</div>
					</div>
				</li>
				<li>
					<div className="empDetail">
						<div className="empPhoto"><img src="images/no-photo-icon.png" alt="Team Member Photo" /></div>
						<div className="empInfo">
							<h3>Peter Lovett</h3>
							<p>Information about Peter</p>
							<p>[E]: plovett@netboard</p>
						</div>
					</div>
				</li>
				<li>
					<div className="empDetail">
						<div className="empPhoto"><img src="images/no-photo-icon.png" alt="Team Member Photo" /></div>
						<div className="empInfo">
							<h3>Alex Erwin</h3>
							<p>Information about Alex</p>
							<p>[E]: aerwin@netboard</p>
						</div>
					</div>
				</li>
			</ul>
		</div>
	</div>;
}
// export for inclusion
export default About;