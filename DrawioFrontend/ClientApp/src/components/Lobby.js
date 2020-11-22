import React from 'react';
import Button from './ui/Button';
import classes from './css/Lobby.module.css';
import {FaRegCopy} from 'react-icons/fa';

const Lobby = props => {
    console.log(props.players)
    const randomColor = () => {
        var colors = ['#b84783', '#46b980', '#2f1ae5', '#67e51a', '#b94675', '#44bb94', '#e4291b', '#a4775b' , '#5c89a3'];
        return colors[Math.floor(Math.random() * 9)];
    };

    const onIconClick = () => {
        navigator.clipboard.writeText(props.lobbyCode);
    }

    return (
        <div className={classes.Lobby}>
            <div style={{width: '270px', display: 'inline-block'}}>
                {props.players ? props.players.map(p => <div key={p.username} className={classes.Player} style={{color: randomColor()}}>{p.username}</div>) : null}
            </div>
            <div>
                {props.isOwner ? <Button onClick={props.startGame} type='Secondary'>Start Game</Button> : null}
                <div className={classes.GameCode}>
                    {props.lobbyCode}
                    <div style={{padding: '3px', textAlign:"center", display: 'inline-block'}}>
                        <div className={classes.Icon} onClick={onIconClick}>
                            <FaRegCopy size='100%' />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );

};

export default Lobby;
