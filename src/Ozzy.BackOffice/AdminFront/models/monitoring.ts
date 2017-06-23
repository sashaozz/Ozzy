import axios from 'axios';
import { action, observable } from "mobx";
import * as httpClient from '../services/httpclient';

export interface INodeInfo {
    nodeId: string;
    machineName: string;
    backgroundTasks: ITaskInfo[];
}

export interface ITaskInfo {
    taskId: string;
    taskName : string;
    taskState : string;
    isRunning: boolean;
}

export class TaskItem {
    public taskId: string;    
    taskName : string;
    taskState : string;
    public isRunning: boolean;

    constructor(task : ITaskInfo) {
        this.taskId = task.taskId;
        this.taskName = task.taskName;
        this.taskState = task.taskState;
        this.isRunning = task.isRunning;
    }
}

export class NodeItem {
    public nodeId: string;
    public machineName: string;
    public backgroundTasks: TaskItem[];

    constructor(nodeId: string, machineName: string, tasks: ITaskInfo[]) {
        this.nodeId = nodeId;
        this.machineName = machineName;
        this.backgroundTasks = tasks.map(t => new TaskItem(t));
    }
}

export class MonitoringStore {
    private serverRequest;

    @observable nodes: NodeItem[] = [];

    @action public async getNodesFromServer() {
        var nodes: INodeInfo[] = (await httpClient.Get('/api/monitoring/nodes')).data;
        
        this.nodes = nodes.map(item => {
            return new NodeItem(item.nodeId, item.machineName, item.backgroundTasks);
        });
    };

    @action public async stopProcess(nodeId: string, processId: string) {
        var nodes: INodeInfo[] = (await httpClient.Post('/api/monitoring/StopProcess', {
            NodeId: nodeId,
            ProcessId: processId
        })).data;
    };

    @action public async startProcess(nodeId: string, processId: string) {
        var nodes: INodeInfo[] = (await httpClient.Post('/api/monitoring/StartProcess', {
            NodeId: nodeId,
            ProcessId: processId
        })).data;
    };
}