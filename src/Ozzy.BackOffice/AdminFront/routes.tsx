import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/layout';
import * as Pages from './pages/index';

export const routes = <Layout>
    <Route exact path="/index.html" component={Pages.FeatureFlags} />
    <Route path="/data" component={Pages.DataPage} />
    <Route path="/feature-flags" component={Pages.FeatureFlags} />
    <Route path="/monitoring" component={Pages.Monitoring} />
</Layout>;