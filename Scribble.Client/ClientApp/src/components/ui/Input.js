import React from 'react';
import classes from './css/Input.module.css';

const Button = props => {
    switch (props.type) {
        case 'text':
            let invalid = '';
            if (!props.isValid && props.isTouched)
                invalid = ' ' + classes.InvalidInputText;
            return <div><input value={props.value} className={classes.InputText + invalid} onClick={props.onClick} placeholder={props.placeholder} onChange={props.onChange} /></div>
        case 'dropdown':
            return null;
        default:
            return null;
    }
};

export default Button;