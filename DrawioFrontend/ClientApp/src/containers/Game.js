import React, {useState, useEffect} from 'react';
import Canvas from '../components/Canvas';
import Guesser from '../components/Guesser';
import ScoreBoard from '../components/ScoreBoard';
import Button from '../components/ui/Button';
import { useHistory } from "react-router-dom";
import { ApiFactory } from '../Helpers/Api';

const USER_TYPES = [
    '',
    'Painter',
    'Accepted'
];

let onNewRound;
let onMakePainter;
let onGuessCorrect;
let onDraw;

const Game = props => {
    const [state, setState] = useState({...props.gameState, players: props.gameState.players.map(p => {return {...p, state: 0}})});//useState({word: 'cat', isPainter: true, players: [{username: 'Alfred', score: 100, state: 1}]})
    const history = useHistory();
    const [frameCounter, setFrameCounter] = useState(0);
    const [canvas, setCanvas] = useState({
        brushColor: '#000000',
        lineWidth: 4,
        canvasStyle: {
          backgroundColor: 'FFFFFF'
        },
        clear: false
    });
    const [hubConnection] = useState(props.hubConnection);

    useEffect(() => {
        onNewRound = drawer => {
            console.log('onNewRound: ' + drawer);
            setState(oState => {
                const newP = {...(oState.players.find(p => p.username === drawer))};
                newP.state = 1;
                const newPlayers = [...(oState.players.filter(p => p.username !== drawer).map(p => {return {...p, state: 0}}))];
                newPlayers.push(newP);
                const newState = {...oState, word: null, players: newPlayers};
                return newState;
            })
        };
        onMakePainter = word => {
            console.log('onMakePainter: ' + word);
            setState(oState => {
                const newState = {...oState, word: word};
                return newState;
            })
        };
        onGuessCorrect = drawer => {
            setState(oState => {
                const newP = {...(oState.players.find(p => p.username === drawer))};
                newP.state = 2;
                newP.score++;
                const newPlayers = [...(oState.players.filter(p => p.username !== drawer).map(p => {return {...p, state: 0}}))];
                newPlayers.push(newP);
                const newState = {...oState, word: null, players: newPlayers};
                return newState;
            })
        };
        onDraw = draw => {
            console.log(draw);
        };
    }, []);

    useEffect(() => {
        hubConnection.off();
        hubConnection.on('newRound', (painterName) => { onNewRound(painterName); });
        hubConnection.on('makePainter', (word) => { onMakePainter(word); });
        hubConnection.on('guessCorrect', (name) => { onGuessCorrect(name); });
        hubConnection.on('draw', (drawList) => { onDraw(drawList); });
    }, [])

    const users = state.players.map((p) => {
        return{
            name: p.username,
            score: p.score,
            type: USER_TYPES[p.state]
        };
    });

    const onGuessMade = async (guess) => {
        await ApiFactory.guess({guess});
    };

    const onRegisterDraw = async (drawObj) => {
        await ApiFactory.draw({token: state.playerId, gamecode: state.gamecode, drawObjects: drawObj}); // make it not send every draw command
    }

    return (
        <div>
            <Button type='Warning'><h4>EXIT GAME</h4></Button>
            <ScoreBoard users={users}/>
            <div style={{width: '400px', height: '400px', backgroundColor: 'white', border: '1px solid black'}}>
                <Canvas isPainter={state.isPainter} registerDraw={onRegisterDraw} {...canvas} toDraw={state.movesToDraw}/>
            </div>
            <Guesser onGuessMade={onGuessMade}></Guesser>
            {state.word ? <p>WORD TO DRAW: {state.word}</p>: null}
        </div>
    );
};

export default Game;
