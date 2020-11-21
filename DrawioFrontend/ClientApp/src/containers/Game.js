import React, {useState, useEffect} from 'react';
import Canvas from '../components/Canvas';
import Guesser from '../components/Guesser';
import ScoreBoard from '../components/ScoreBoard';
import Button from '../components/ui/Button';
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

let objToSend = [];

const Game = props => {
    const [state, setState] = useState({...props.gameState, players: props.gameState.players.map(p => {return {...p, state: 0}})});//useState({word: 'cat', isPainter: true, players: [{username: 'Alfred', score: 100, state: 1}]})
    const [canvas, setCanvas] = useState({
        brushColor: '#000000',
        lineWidth: 4,
        canvasStyle: {
          backgroundColor: 'FFFFFF'
        },
        clear: false
    });
    const [hubConnection] = useState(props.hubConnection);
    const [toDraw, setToDraw] = useState(null);
    const [isPainter, setIsPainter] = useState(false);


    const sendDrawObjects = async () => {
        if (objToSend.length > 0){
            const temp = objToSend;
            objToSend = [];
            console.log('send: ' + temp.length);
            await ApiFactory.draw({token: state.playerId, gamecode: state.gamecode, drawObjects: temp}); // make it not send every draw command
        }
    };

    useEffect(() => {
        onNewRound = drawer => {
            console.log('onNewRound: ' + drawer);
            objToSend = [];
            setToDraw([{clear: true}]);
            setState(oState => {
                const newP = {...(oState.players.find(p => p.username === drawer))};
                newP.state = 1;
                const newPlayers = [...(oState.players.filter(p => p.username !== drawer).map(p => {return {...p, state: 0}}))];
                newPlayers.push(newP);
                const newState = {...oState, word: null, players: newPlayers};
                return newState;
            })
            if(drawer === state.user)
                setIsPainter(true);
            else
                setIsPainter(false);
        };
        onMakePainter = word => {
            console.log('onMakePainter: ' + word);
            setState(oState => {
                const newState = {...oState, word: word};
                return newState;
            })
        };
        onGuessCorrect = user => {
            setState(oState => {
                const newP = {...(oState.players.find(p => p.username === user))};
                if(newP.state !== 2){
                    newP.state = 2;
                    newP.score++;
                    const newPlayers = [...(oState.players.filter(p => p.username !== user).map(p => {return {...p}}))];
                    newPlayers.push(newP);
                    const newState = {...oState, word: null, players: newPlayers};
                    return newState;
                }
                return oState;
            })
        };
        onDraw = draw => {
            if (!isPainter){
                console.log('draw: ' + draw.length);
                setToDraw(draw);
            }
        };
    }, []);

    useEffect(() => {
        hubConnection.off();
        hubConnection.on('newRound', (painterName) => { onNewRound(painterName); });
        hubConnection.on('makePainter', (word) => { onMakePainter(word); });
        hubConnection.on('guessCorrect', (name) => { onGuessCorrect(name); });
        hubConnection.on('draw', (drawList) => { onDraw(drawList); });
    }, [])


    useEffect(() => {
        const interval = window.setInterval(async () => {
            if (isPainter){
                sendDrawObjects();
            }
        }, 2000)
    }, [isPainter])

    const users = state.players.map((p) => {
        return{
            name: p.username,
            score: p.score,
            type: USER_TYPES[p.state]
        };
    });

    const onGuessMade = async (guess) => {
        await ApiFactory.guess({guess: guess, gamecode: state.gamecode, token: state.playerId});
    };

    const onRegisterDraw = drawObj => {
        objToSend.push(drawObj);
    }

    const onResetToDraw = () => {
        setToDraw(null);
    }

    return (
        <div>
            <div>
                <div style={{display: 'inline-block', verticalAlign: 'top'}}>
                    <Button type='Warning'><h4>EXIT GAME</h4></Button>
                    <ScoreBoard users={users} />
                </div>
                <div style={{width: '800px', height: '800px', backgroundColor: 'white', border: '1px solid black', display: 'inline-block', verticalAlign: 'top'}}>
                    <Canvas isPainter={isPainter} registerDraw={onRegisterDraw} clearToDraw={onResetToDraw} {...canvas} toDraw={toDraw}/>
                </div>
            </div>
            
            
            <Guesser onGuessMade={onGuessMade}></Guesser>
            {state.word ? <p>WORD TO DRAW: {state.word}</p>: null}
        </div>
    );
};

export default Game;
