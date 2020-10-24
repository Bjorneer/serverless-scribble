import React, { useState } from 'react';
import Button from '../components/ui/Button';
import Input from '../components/ui/Input';


const JoinGame = props => {
    const [joinGameForm, setJoinGameForm] = useState({
        username: {
            placeholder: 'USERNAME',
            isValid: false,
            type: 'text',
        },
        gameid: {
            placeholder: 'GAME CODE',
            isValid: false,
            type: 'text'
        },
        isFormValid: false
    });

    const form = [];
    for (let element in joinGameForm) {
        form.push(<Input key={element} placeholder={joinGameForm[element].placeholder} type={joinGameForm[element].type} ></Input>)
    }

    return (
        <>
            <h1>Join Game</h1>
            <form onSubmit={props.onSubmit}>
                {form}
                <Button type='Success'>Join Game</Button>
            </form>
        </>
    );

};

export default JoinGame;
