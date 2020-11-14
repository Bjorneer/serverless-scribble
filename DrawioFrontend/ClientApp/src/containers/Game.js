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
    const [state, setState] = useState(props.gameState);//useState({players: [{username: 'Alfred', score: 100, state: 0}]})
    const history = useHistory();

    if(!state){
        history.push('/');
    }

    useEffect(() => {
        const interval = window.setInterval(async () => {
            const res = await GameAPI.getGameState({gamecode: state.gamecode, token: state.playerId});
            const data = await res.json();
            setState(data);
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

    const draw = (ctx, frameCount) => {
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height)
        ctx.fillStyle = '#000000'
        ctx.beginPath()
        ctx.arc(50, 100, 20*Math.sin(frameCount*0.05)**2, 0, 2*Math.PI)
        ctx.fill()
      }
    const word = state.word;
    if(word){

    }
    return (
        <div>
            <Button type='Warning'><h4>EXIT GAME</h4></Button>
            <ScoreBoard users={users}/>
            <Canvas draw={draw}/>
            <Guesser onGuessMade={onGuessMade}></Guesser>
            {state.word ? <p>WORD TO DRAW: {state.word}</p>: null}
        </div>
    );
};

export default Game;
