import React, {useState} from 'react';
import classes from './css/Guesser.module.css';

const Guesser = props => {
    const [input, setInput] = useState('');

    const onKeyUp  = (event) => {
        if(event.keyCode === 13 && input.length > 0){
            event.preventDefault();
            props.onGuessMade(input);
        }
    };

    const handleChange = (e) => {
        setInput(e.target.value);
    };

    return (
        <div className={classes.Guesser}>
            <input onKeyUp={onKeyUp} value={input} onChange={(e) => handleChange(e)} placeholder='Enter here'/>
        </div>
    )
};

export default Guesser;