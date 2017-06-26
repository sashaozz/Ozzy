import * as React from 'react';
import * as RB from 'react-bootstrap';
import * as Modal from 'react-modal-bootstrap';
import * as OZ from '../';
import { action, observable } from "mobx";
import {observer} from 'mobx-react';

export interface CreateFeatureFlagPopupProps {
    onSave: any;
}

@observer export class CreateFeatureFlagPopup extends React.Component<CreateFeatureFlagPopupProps, {}> {
    @observable isOpened = false;
    @observable name = "";

    @action.bound public togglePopup() {
        this.isOpened = !this.isOpened;
    }

    @action.bound handleChange(e) {
        this.name = e.target.value;
    }

    onSave = () => {
        this.togglePopup();
        var name = this.name;
        this.name = "";
        this.props.onSave(name);
    }

    public render() {
        return (
            <Modal.Modal isOpen={this.isOpened} onRequestHide={this.togglePopup}>
                <Modal.ModalHeader>
                    <Modal.ModalClose onClick={this.togglePopup} />
                    <Modal.ModalTitle>Create New Feature Flag</Modal.ModalTitle>
                </Modal.ModalHeader>
                <Modal.ModalBody>
                    <form>
                        <OZ.FieldGroup
                            id="featureFlagId"
                            type="text"
                            value={this.name}
                            label="Feature Flag Id"
                            placeholder="Enter id"
                            onChange={this.handleChange}
                        />
                    </form>
                </Modal.ModalBody>
                <Modal.ModalFooter>
                    <RB.Button bsStyle="default" onClick={this.togglePopup}>Close</RB.Button>
                    <RB.Button bsStyle="primary" onClick={this.onSave}>Save changes</RB.Button>
                </Modal.ModalFooter>
            </Modal.Modal>

        )
    }
}
