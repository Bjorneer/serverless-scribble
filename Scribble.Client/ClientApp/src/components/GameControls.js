import React, { memo } from 'react';
import ScoreBoard from '../components/ScoreBoard';
import Button from '../components/ui/Button';
import Guesser from './Guesser';
import {USER_TYPES} from '../containers/Game';
import classes from './css/GameControls.module.css';
import {ImExit} from 'react-icons/im';
import Chat from '../components/Chat';

const GameControls = memo(({players,word,hubConnection, guessMade, user, exit}) => {

    const users = players.map((p) => {
        return{
            name: p.username,
            score: p.score,
            type: USER_TYPES[p.state]
        };
    });

    let wordBox = null;
    if (word){
        wordBox = (
            <div className={classes.WordBox}>
                WORD TO DRAW
                <div className={classes.Word}>
                    {word}
                </div>
            </div>
        );
    }

    return (
        <>
            <div style={{display: 'inline-block', verticalAlign: 'top', width: '20%', padding: '10px', boxSizing: 'border-box'}}>
                <Button type='Error' style={{width: '100%', margin: '0px 0px 15px 0px', maxWidth: '200px', height: '70px'}} onClick={exit} >
                    <div style={{position: 'relative'}}>
                        <div style={{ display: 'inline-block', left: '0%', position: 'absolute', top: '50%', transform: 'translate(0%, -40%)'}}>
                            <ImExit size='50px'/>
                        </div>
                        <div style={{fontWeight: 'bold', fontSize: '32px', display: 'inline-block', position: 'absolute', top: '50%', transform: 'translate(-50%, -50%)'}}>Exit</div>
                    </div>
                </Button>
                <ScoreBoard users={users} />
                <Chat user={user} hubConnection={hubConnection}/>
                <Guesser onGuessMade={guessMade} disabled={word}></Guesser>
                {wordBox}
            </div>
        </>
    )
});


export default GameControls;

