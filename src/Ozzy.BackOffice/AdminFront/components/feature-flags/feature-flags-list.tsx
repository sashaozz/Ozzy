import * as React from 'react';
import * as RB from 'react-bootstrap';
import * as OZ from '../';
import * as Models from "../../models"
import { action, observable } from "mobx";
import { observer } from 'mobx-react';

interface Props {
}
interface State {
}

@observer export class FeatureFlags extends React.Component<Props, State> {
    private createPopup: OZ.CreateFeatureFlagPopup;
    private flagStore = new Models.FeatureFlagStore();

    componentDidMount() {
        this.flagStore.getFlagsFromServer();
    }
    componentWillUnmount() {
    }

    flagChange = async (state, flag: Models.FeatureFlagItem) => {
        flag.toggle();
        await this.flagStore.updateFlagsOnServer(flag);
    }
    
    toggleCreatePopup = () => {
        this.createPopup.togglePopup();
    }

    createFeatureFlag = (name) => {
        this.flagStore.addFeatureFlag(name);
    }

    public render() {
        return (
            <div>
                <OZ.CreateFeatureFlagPopup ref={(popup) => { this.createPopup = popup; }} onSave={this.createFeatureFlag} />
                <div className="box-body">
                    <ul className="todo-list">
                        {this.flagStore.flags.map(f => <OZ.FeatureFlagRow flag={f} onToggle={this.flagChange} key={f.id} />)}
                    </ul>
                </div>
                <div className="box-footer clearfix no-border">
                    <RB.Button onClick={this.toggleCreatePopup} bsClass="btn default pull-right">
                        <RB.Glyphicon glyph="plus" /> Create New
                    </RB.Button>
                </div>
            </div>
        )
    }
}