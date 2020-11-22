import React from 'react';
import classes from './css/Footer.module.css';
import {GoMarkGithub} from 'react-icons/go';
import {AiOutlineMail} from 'react-icons/ai';

const Footer = () => {
    const onGithubIconClick = () => {
        window.open('https://github.com/Bjorneer/serverless-scribble');
    };

    return (
        <footer className={classes.Footer}>
            <div style={{padding: '3px', textAlign:"center", display: 'inline-block'}}>
                <div className={classes.Icon}>
                    <AiOutlineMail size='100%' />
                </div>
                Contact
            </div>
            <div style={{padding: '3px', textAlign:"center", display: 'inline-block'}}>
                <div className={classes.Icon} onClick={onGithubIconClick}>
                    <GoMarkGithub size='100%' />
                </div>
                Github
            </div>
        </footer>
    );
}

export default Footer;