import * as React from 'react';
import * as ReactRouter from 'react-router-dom';
import * as RB from 'react-bootstrap';
import * as OZ from '../components';

interface Props {
}
interface State {
}

export class DataPage extends React.Component<ReactRouter.RouteComponentProps<any>, State> {
  public render() {
    return (
      <section className="content">
        <div className="box">
          <div className="box-header with-border">
            <h3 className="box-title">Data Table</h3>
          </div>
          <div className="box-body">
            Data
          </div>          
        </div>
      </section>
    )
  }
}