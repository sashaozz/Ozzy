import * as React from 'react';
import * as OZ from './index';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, {}> {
    public render() {
        return <div>
            <OZ.HeaderBar>
                <OZ.HeaderRightBar>
                    <OZ.HeaderMessages></OZ.HeaderMessages>
                    <OZ.HeaderTasks></OZ.HeaderTasks>
                    <OZ.HeaderNotifications></OZ.HeaderNotifications>
                    <OZ.HeaderUser></OZ.HeaderUser>
                    <OZ.HeaderSidebarControl></OZ.HeaderSidebarControl>
                </OZ.HeaderRightBar>
            </OZ.HeaderBar>

            <OZ.MainSideBar></OZ.MainSideBar>

            <div className="content-wrapper">
                <section className="content-header">
                    <h1>
                        Page Header <small>Optional description</small>
                    </h1>
                    <ol className="breadcrumb">
                        <li><a href="#"><i className="fa fa-dashboard"></i> Level</a></li>
                        <li className="active">Here</li>
                    </ol>
                </section>
                {this.props.children}
            </div>
            <footer className="main-footer">
                <div className="pull-right hidden-xs">
                    Anything you want
                        </div>
                <strong>Copyright &copy; 2018 <a href="#">Company</a>.</strong> All rights reserved.
                    </footer>
            <OZ.ControlSideBar></OZ.ControlSideBar>
            <div className="control-sidebar-bg"></div>
        </div>
    }
}