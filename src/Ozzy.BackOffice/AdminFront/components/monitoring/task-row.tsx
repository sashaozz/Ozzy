import * as Models from "../../models/index"
import axios from 'axios';
import * as React from 'react';
import { action, observable } from "mobx";
import { observer } from 'mobx-react';
import ReactBootstrapToggle from 'react-bootstrap-toggle';

export interface TaskRowProps {
    task: Models.TaskItem;
    nodeId: string;
    onStop: any;
    onStart: any;
}

@observer export class TaskRow extends React.Component<TaskRowProps, {}> {
    public render() {
        return (
            <tr role="row" >
                <td className="sorting_1">{this.props.task.taskName}</td>
                <td className="sorting_1">{this.props.task.taskId}</td>
                <td>{this.props.task.isRunning ? 'Running' : 'Not running'}</td>
                <td className="sorting_1">{this.props.task.taskState}</td>
                <td>
                    <button className="btn btn-default btn-sm" onClick={() => this.props.onStop(this.props.nodeId, this.props.task.taskId)}>Stop</button>
                    <button className="btn btn-default btn-sm" onClick={() => this.props.onStart(this.props.nodeId, this.props.task.taskId)}>Start</button>
                </td>
            </tr>
        )
    }
}