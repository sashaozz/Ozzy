import * as React from 'react';
import * as RB from 'react-bootstrap';

interface Props {
}

export class HeaderSidebarControl extends React.Component<Props, {}> {
    public render() {
        return (
            <li>
                <a href="#" data-toggle="control-sidebar"><i className ="fa fa-gears"></i></a>
            </li>
        )
    }
}