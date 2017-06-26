import * as React from 'react';
import * as RB from 'react-bootstrap';

interface Props {
}

export class HeaderUser extends React.Component<Props, {}> {
    public render() {
        return (
            <li className ="dropdown user user-menu">

                <a href="#" className ="dropdown-toggle" data-toggle="dropdown">

                    <img src="lib/admin-lte/dist/img/user2-160x160.jpg" className ="user-image" alt="User Image"/>

                    <span className ="hidden-xs">Alexander Pierce</span>
                </a>
                <ul className ="dropdown-menu">

                    <li className ="user-header">
                        <img src="lib/admin-lte/dist/img/user2-160x160.jpg" className ="img-circle" alt="User Image" />
                        <p>
                            Alexander Pierce - Web Developer
                            <small>Member since Nov.2012</small>
                        </p>
                    </li>

                    <li className ="user-body">
                        <div className ="row">
                            <div className ="col-xs-4 text-center">
                                <a href="#">Followers</a>
                            </div>
                            <div className ="col-xs-4 text-center">
                                <a href="#">Sales</a>
                            </div>
                            <div className ="col-xs-4 text-center">
                                <a href="#">Friends</a>
                            </div>
                        </div>

                    </li>

                    <li className ="user-footer">
                        <div className ="pull-left">
                            <a href="#" className ="btn btn-default btn-flat">Profile</a>
                        </div>
                        <div className ="pull-right">
                            <a href="#" className ="btn btn-default btn-flat">Sign out</a>
                        </div>
                    </li>
                </ul>
            </li>
        )
    }
}