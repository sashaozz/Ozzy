import * as React from 'react';
import * as ReactRouter from 'react-router-dom';
import * as RB from 'react-bootstrap';
import * as OZ from '../components';

interface Props {
}
interface State {
}

export class FeatureFlags extends React.Component<ReactRouter.RouteComponentProps<any>, State> {
  public render() {
    return (
      <section className="content">
        <div className="box">
          <div className="box-header with-border">
            <h3 className="box-title">Bordered Table</h3>
          </div>
          <div className="box-body">
            <OZ.FeatureFlags></OZ.FeatureFlags>
          </div>
          <div className="box-footer clearfix">
            <ul className="pagination pagination-sm no-margin pull-right">
              <li><a href="#">&laquo;</a></li>
              <li><a href="#">1</a></li>
              <li><a href="#">2</a></li>
              <li><a href="#">3</a></li>
              <li><a href="#">&raquo;</a></li>
            </ul>
          </div>
        </div>
      </section>
    )
  }
}