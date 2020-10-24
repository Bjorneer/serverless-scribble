import React, { useState } from 'react';
import Button from '../components/ui/Button';
import Input from '../components/ui/Input';


const CreateGame = props => {
    const [gameCreationForm, setGameCreationForm] = useState({
        username: {
            placeholder: 'ROUNDS',
            isValid: false,
            type: 'text', // dropdown when implemented
        },
        gameid: {
            placeholder: 'DRAW TIME',
            isValid: false,
            type: 'text'
        },
        isFormValid: false
    });

    const form = [];
    for (let element in gameCreationForm) {
        form.push(<Input key={element} placeholder={gameCreationForm[element].placeholder} type={gameCreationForm[element].type} ></Input>)
    }

    return (
        <>
            <h1>Create Game</h1>
            <div>
                <form onSubmit={props.onSubmit}>
                    {form}
                    <Button type='Success'>START GAME</Button>
                </form>
            </div>
            <div>
                <p>p1</p>
                <p>p2</p>
            </div>

        </>
    );

};

export default CreateGame;
