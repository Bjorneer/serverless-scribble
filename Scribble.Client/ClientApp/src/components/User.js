import React from 'react';

import classes from './css/User.module.css';

const User = props => {
    let extraClass = '';
    switch (props.type){
        case 'Painter':
            extraClass = classes.Painter;
            break;
        case 'Accepted':
            extraClass = classes.Accepted;
            break;
        default:
            break;
    }
    return (
        <div className={classes.User + ' ' + extraClass} >
            <text>
                {props.name}
            </text>
            <text style={{float: 'right'}}>
                {props.score}
            </text>
        </div>
    )

};

export default User;