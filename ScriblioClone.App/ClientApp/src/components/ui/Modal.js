import React from 'react';
import Backdrop from './Backdrop';
import classes from './css/Modal.module.css';

const Modal = props => {
    if (!props.show) {
        return null;
    }
    return (
        <div className={classes.Modal}>
            <Backdrop show onClick={props.close}/>
            {props.children}
        </div>
    );
};

export default Modal;