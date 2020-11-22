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
                required: true
            },
            isValid: false,
            touched: false
        },
        gameCode: {
            elementType: 'input',
            elementConfig: {
                type: 'text',
                placeholder: 'Game Code'
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
            ...mainForm
        };
        const updatedFormElement = {
            ...updatedForm[inputIdentifier]
        };

        updatedFormElement.value = event.target.value;

        updatedFormElement.valid = validate(updatedFormElement.value, updatedFormElement.validation);
        updatedFormElement.touched = true;
        updatedForm[inputIdentifier] = updatedFormElement;

        let formValid = true;
        for (let inputIdentifiers in form){
            formValid = formValid && form[inputIdentifiers].valid
        }
        setMainForm(updatedForm);
        setFormIsValid(formValid);
    };

    const form = [];
    for (let element in mainForm) {
        form.push(
        <Input 
            key={element} 
            placeholder={mainForm[element].elementConfig.placeholder} 
            type={mainForm[element].elementConfig.type}
            onChange={(e) => onInputChangedHandler(e, element)} ></Input>)
    }
    return (
            <form onSubmit={props.onSubmit}>
                {form}
                <Button 
                    type='Success' 
                    disabled={!mainForm.isValid} 
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
