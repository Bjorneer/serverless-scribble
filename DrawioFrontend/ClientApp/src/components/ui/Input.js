import React from 'react';
import classes from './css/Input.module.css';

const Button = (props) => {
    switch (props.type) {
        case 'text':
            return <input className={classes.InputText} onClick={props.onClick} placeholder={props.placeholder} />
        case 'dropdown':
            return null;
        default:
            return null;
    }
};

export default Button;