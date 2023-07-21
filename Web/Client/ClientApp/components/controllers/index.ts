// import components
import { connect } from "react-redux";
import * as inbox from "@Components/controllers/inbox";
import * as message from "@Components/controllers/message"
// top level state object
export interface IApplicationState {
    inbox: inbox.iInboxState;
    message: message.iMessageState;
}
// reducers
export const reducers = {

}
// method declarations 

export interface IAppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => IApplicationState): void;
}

export interface IAppThunkActionAsync<TAction, TResult> {
    (dispatch: (action: TAction) => void, getState: () => IApplicationState): Promise<TResult>
}

export function withStore<TStoreState, TActionCreators, TComponent extends React.ComponentType<TStoreState & TActionCreators & any>>(
    component: TComponent,
    stateSelector: (state: IApplicationState) => TStoreState,
    actionCreators: TActionCreators
): TComponent {
    return <TComponent><unknown>connect(stateSelector, actionCreators)(component);
}