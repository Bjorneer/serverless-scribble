import React from 'react';

import classes from './css/Backdrop.module.css';

const Backdrop = props => {
    return props.show ? <div onClick={props.onClick}></div> : null;
};

export default Backdrop;