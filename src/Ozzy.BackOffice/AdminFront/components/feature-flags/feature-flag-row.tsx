import * as Models from "../../models/index"
import axios from 'axios';
import * as React from 'react';
import { action, observable } from "mobx";
import {observer} from 'mobx-react';
import ReactBootstrapToggle from 'react-bootstrap-toggle';

export interface FeatureFlagRowProps {
    flag: Models.FeatureFlagItem;
    onToggle: any;
}

@observer export class FeatureFlagRow extends React.Component<FeatureFlagRowProps, {}> {
    public render() {
        return (

            <li>
                <ReactBootstrapToggle
                    on={"on"}
                    off={"off"}
                    active={this.props.flag.enabled}
                    onChange={state => this.props.onToggle(state, this.props.flag)} />
                <span className="text">{this.props.flag.id}</span>
                <span className="text">{this.props.flag.version}</span>
                <small className="label label-danger"><i className="fa fa-clock-o"></i> 2 mins</small>
                <div className="tools">
                    <i className="fa fa-edit"></i>
                    <i className="fa fa-trash-o"></i>
                </div>
            </li>
        )
    }
}