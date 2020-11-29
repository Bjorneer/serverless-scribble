import React, { useState } from 'react';
import Button from '../components/ui/Button';
import Input from '../components/ui/Input';
import validate from '../Helpers/FormValidator';


const MainForm = props => {
    const [formIsValid, setFormIsValid] = useState(false);
    const [mainForm, setMainForm] = useState({
        username: {
            elementType: 'input',
            elementConfig: {
                type: 'text',
                placeholder: 'Your Name'
            },
            value: '',
            validation: {
                required: true,
                minLength: 5,
                maxLength: 10
            },
            isValid: false,
            isTouched: false,
            onlyUpper: false
        },
        gameCode: {
            elementType: 'input',
            elementConfig: {
                type: 'text',
                placeholder: 'Game Code'
            },
            value: '',
            validation: {
                required: true,
                minLength: 6,
                maxLength: 6
            },
            isValid: false,
            isTouched: false,
            onlyUpper: true
        },
    });

    const onInputChangedHandler = (event, inputIdentifier) => {
        const updatedForm = {
            ...mainForm
        };
        const updatedFormElement = {
            ...updatedForm[inputIdentifier]
        };

        if (mainForm[inputIdentifier].onlyUpper){
            updatedFormElement.value = event.target.value.toUpperCase();
        }
        else
            updatedFormElement.value = event.target.value;

        updatedFormElement.isValid = validate(updatedFormElement.value, updatedFormElement.validation);
        updatedFormElement.isTouched = true;
        updatedForm[inputIdentifier] = updatedFormElement;

        let formValid = true;
        for (let inputIdentifiers in updatedForm){
            formValid = formValid && updatedForm[inputIdentifiers].isValid
        }
        setMainForm(updatedForm);
        setFormIsValid(formValid);
    };

    const form = [];
    for (let element in mainForm) {
        form.push(
        <Input 
            key={element}
            value={mainForm[element].value}
            placeholder={mainForm[element].elementConfig.placeholder} 
            type={mainForm[element].elementConfig.type}
            onChange={(e) => onInputChangedHandler(e, element)} 
            isValid={mainForm[element].isValid}
            isTouched={mainForm[element].isTouched}></Input>)
    }
    return (
            <form onSubmit={props.onSubmit}>
                {form}
                <Button 
                    type='Success' 
                    disabled={!mainForm.username.isValid} 
                    onClick={(e) => props.onCreateGame(e, mainForm.username.value)}>Create New Game
                </Button>
                <Button 
                    type='Success' 
                    disabled={!formIsValid} 
                    onClick={(e) => props.onJoinGame(e, mainForm.gameCode.value, mainForm.username.value) }>Join Game
                </Button>
            </form>
    );

};

export default MainForm;
