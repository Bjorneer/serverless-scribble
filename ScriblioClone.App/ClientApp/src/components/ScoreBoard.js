import React from 'react';
import User from './User';
import classes from './css/ScoreBoard.module.css';
import Button from './ui/Button';

const ScoreBoard = props => {
    props.users.sort((a, b) => {
        if(a.score < b.score) return 0;
        return -1;
    })
    const users = props.users.map((u, index) => {
        return <User key={u.name} {...u}></User>
    });

    return (
        <div className={classes.ScoreBoard}>
            {users}
        </div>
    );
};

export default ScoreBoard;