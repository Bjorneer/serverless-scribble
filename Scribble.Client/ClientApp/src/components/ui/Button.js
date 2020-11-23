import React from 'react';
import classes from './css/Button.module.css';

const Button = (props) => {
    let secClass = null;
    switch (props.type) {
        case 'Success':
            secClass = classes.Success;
            break;
        case 'Error':
            secClass = classes.Error;;
            break;
        case 'Warning':
            secClass = classes.Warning;
            break;
        case 'Secondary':
            secClass = classes.Secondary;
            break;
        default:
            break;            
    }
    return <button className={classes.Button + ' ' + secClass} onClick={props.onClick}>{props.children}</button>
};

export default Button;