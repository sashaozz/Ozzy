import axios from 'axios';

var root = 'http://localhost:5000';

export async function Get(path: string): Promise<any> {
    var serverRequest = axios.get(root + path);
    var resp = await serverRequest;
    return resp;
}

export async function Post(path: string, data: any): Promise<any> {
    var serverRequest = axios.post(root + path, data);
    var resp = await serverRequest;
    return resp;
}