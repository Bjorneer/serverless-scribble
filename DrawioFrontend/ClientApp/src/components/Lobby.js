import React, { useState } from 'react';
import Button from './ui/Button';


const Lobby = props => {
    
    return (
        <>
            <h1>Lobby</h1>
            {props.players ? props.players.map(p => <h2 key={p.username}>{p.username}</h2>) : null}
            {props.isOwner ? <Button onClick={props.startGame} type='Success'>Start Game</Button> : null}
            <h1>{props.lobbyCode}</h1>
        </>
    );

};

export default Lobby;
