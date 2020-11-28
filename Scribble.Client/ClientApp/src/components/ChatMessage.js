import React from 'react';
import classes from './css/Chat.module.css';

const ChatMessage = props => {
    let message = null;
    if(props.user === 'GAME_EVENT'){
        message = (
            <div className={classes.GameEvent}>
                {props.message}
            </div>
        )
    }
    else if (props.isFriendly){
        message = (
            <div className={classes.FriendlyMessage}>
                <div className={classes.Message} style={{ backgroundColor: 'lightblue', borderRadius: '10px 10px 0px 10px'}}>
                    {props.message}
                </div>
                <div className={classes.User} style={{float: 'right'}}>
                    {props.user}
                </div>
            </div>
        );
    }
    else{
        message = (
            <div className={classes.OpponentMessage}>
                <div className={classes.User} style={{float: 'left'}}>
                    {props.user}
                </div>
                <div className={classes.Message} style={{ backgroundColor: 'lightcoral', borderRadius: '10px 10px 10px 0px'}}> 
                    {props.message}
                </div>
            </div>
        );
    }

    return (
        <div className={classes.ChatMessage}>
            {message}
        </div>
    )
}

export default ChatMessage;