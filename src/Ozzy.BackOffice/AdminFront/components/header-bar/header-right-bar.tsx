import * as React from 'react';
import * as RB from 'react-bootstrap';

interface Props {
}

export class HeaderRightBar extends React.Component<Props, {}> {
    public render() {
        return (
            <div className ="navbar-custom-menu">
                <ul className ="nav navbar-nav">
                    {this.props.children}
                </ul>
            </div>
        )
    }
}