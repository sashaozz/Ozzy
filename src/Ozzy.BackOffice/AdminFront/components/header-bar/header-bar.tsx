import * as React from 'react';
import * as RB from 'react-bootstrap';

export class HeaderBar extends React.Component<{}, {}> {
    public render() {
        return (
            <header className="main-header">
                <a href="index2.html" className ="logo">
                    <span className ="logo-mini"><b>A</b>LT</span>
                    <span className ="logo-lg"><b>Admin</b>LTE</span>
                </a>
                <RB.Navbar staticTop fluid>
                    <a href="#" className ="sidebar-toggle" data-toggle="offcanvas" role="button">
                        <span className ="sr-only">Toggle navigation</span>
                    </a>
                    {this.props.children}                    
                </RB.Navbar>
            </header>
        )
    }
}