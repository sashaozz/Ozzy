import * as React from 'react';
import * as RB from 'react-bootstrap';

export interface FieldGroupProps {
    id: string;
    label?: string;
    help?: string;
    [props: string]: any;
}
export function FieldGroup({id, label, help, ...props}: FieldGroupProps) {
    return (
        <RB.FormGroup controlId={id}>
            <RB.ControlLabel>{label}</RB.ControlLabel>
            <RB.FormControl {...props} />
            {help && <RB.HelpBlock>{help}</RB.HelpBlock>}
        </RB.FormGroup>
    );
}