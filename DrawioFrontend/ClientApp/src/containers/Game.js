import React, {useState, useEffect} from 'react';
import Canvas from '../components/Canvas';
import Guesser from '../components/Guesser';
import ScoreBoard from '../components/ScoreBoard';
import Button from '../components/ui/Button';
import { GameAPI } from '../Helpers/Api';
import { useHistory } from "react-router-dom";

const USER_TYPES = [
    '',
    'Painter',
    'Accepted'
];


const Game = props => {
    const [state, setState] = useState(props.gameState);//useState({word: 'cat', isPainter: true, players: [{username: 'Alfred', score: 100, state: 1}]})
    const history = useHistory();
    const [canvas, setCanvas] = useState({
        brushColor: '#000000',
        lineWidth: 4,
        canvasStyle: {
          backgroundColor: 'FFFFFF'
        },
        clear: false
    });

    console.log(state);
    if(!state){
        history.push('/');
    }

    useEffect(() => {
        const interval = window.setInterval(async () => {
            const res = await GameAPI.getGameState({gamecode: state.gamecode, token: state.playerId});
            const data = await res.json();
            if(data !== null){
                setState(data);
            }
        }, 2000);
        return () => {
            window.clearInterval(interval);
        }
    }, [state]);
    

    const users = state.players.map((p) => {
        return{
            name: p.username,
            score: p.score,
            type: USER_TYPES[p.state]
        };
    });

    const onGuessMade = (guess) => {
        GameAPI.makeGuess({gamecode: state.gamecode, token: state.playerId, guess: guess})
            .catch(err => {
                console.log(err);
            });
    };

    const onRegisterDraw = (drawObj) => { // change to make api call in useEffect later to send chuck of draw obj VIKTIGT
        GameAPI.sendDraw({...drawObj, token: state.playerId, gamecode: state.gamecode})
            .catch(err => {
                console.log(err);
            });
    }

    return (
        <div>
            <Button type='Warning'><h4>EXIT GAME</h4></Button>
            <ScoreBoard users={users}/>
            <div style={{width: '400px', height: '400px', backgroundColor: 'white', border: '1px solid black'}}>
                <Canvas isPainter={state.isPainter} registerDraw={onRegisterDraw} {...canvas}/>
            </div>
            <Guesser onGuessMade={onGuessMade}></Guesser>
            {state.word ? <p>WORD TO DRAW: {state.word}</p>: null}
        </div>
    );
};

export default Game;
