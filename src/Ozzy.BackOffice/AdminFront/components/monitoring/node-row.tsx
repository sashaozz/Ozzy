import * as Models from "../../models/index"
import axios from 'axios';
import * as React from 'react';
import { action, observable } from "mobx";
import { observer } from 'mobx-react';
import ReactBootstrapToggle from 'react-bootstrap-toggle';

export interface NodeRowProps {
    node: Models.NodeItem;
    onSelectedNode: any;
}

@observer export class NodeRow extends React.Component<NodeRowProps, {}> {
    public render() {
        return (
            <tr role="row" >
                <td className="sorting_1">{this.props.node.machineName}</td>
                <td>{this.props.node.nodeId}</td>
                <td><a onClick={x => this.props.onSelectedNode(this.props.node)}>{this.props.node.backgroundTasks.length}</a></td>
            </tr>
        )
    }
}