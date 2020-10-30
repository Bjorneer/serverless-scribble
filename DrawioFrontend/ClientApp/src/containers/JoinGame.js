import React, { useState } from 'react';
import Button from '../components/ui/Button';
import Input from '../components/ui/Input';
import FormValidator from '../Helpers/FormValidator';


const JoinGame = props => {
    const [formIsValid, setFormIsValid] = useState(false);
    const [joinGameForm, setJoinGameForm] = useState({
        username: {
            elementType: 'input',
            elementConfig: {
                type: 'text',
                placeholder: 'Your Name'
            },
            value: '',
            validation: {
                required: true
            },
            isValid: false,
            touched: false
        },
        gameCode: {
            elementType: 'input',
            elementConfig: {
                type: 'text',
                placeholder: 'Your Name'
            },
            value: '',
            validation: {
                required: true
            },
            isValid: false,
            touched: false
        },
    });

    const onInputChangedHandler = (event, inputIdentifier) => {
        const updatedForm = {
            ...joinGameForm
        };
        const updatedFormElement = {
            ...updatedForm[inputIdentifier]
        };
        updatedFormElement.value = event.target.value;
        updatedFormElement.valid = FormValidator.validate(updatedFormElement.value, updatedFormElement.validation);
        updatedFormElement.touched = true;
        updatedForm[inputIdentifier] = updatedFormElement;

        let formValid = true;
        for (let inputIdentifiers in form){
            formValid = formValid && form[inputIdentifiers].valid
        }
        setJoinGameForm(form);
        setFormIsValid(formValid);
    };

    const form = [];
    for (let element in joinGameForm) {
        form.push(<Input key={element} placeholder={joinGameForm[element].placeholder} type={joinGameForm[element].type} ></Input>)
    }

    return (
        <>
            <h1>Join Game</h1>
            <form onSubmit={props.onSubmit}>
                {form}
                <Button type='Success' disabled={!formIsValid}>Join Game</Button>
            </form>
        </>
    );

};

export default JoinGame;
