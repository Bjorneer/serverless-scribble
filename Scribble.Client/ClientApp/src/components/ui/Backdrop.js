import React from 'react';

import classes from './css/Backdrop.module.css';

const Backdrop = props => {
    return props.show ? <div onClick={props.onClick} className={classes.Backdrop}></div> : null;
};

export default Backdrop;