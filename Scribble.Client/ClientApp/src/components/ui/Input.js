import React from 'react';
import classes from './css/Input.module.css';

const Button = (props) => {
    switch (props.type) {
        case 'text':
            return <div><input value={props.value} className={classes.InputText} onClick={props.onClick} placeholder={props.placeholder} onChange={props.onChange} /></div>
        case 'dropdown':
            return null;
        default:
            return null;
    }
};

export default Button;