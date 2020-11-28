import React, {useState, useEffect} from 'react';
import Canvas from '../components/Canvas';
import GameControls from '../components/GameControls';
import { ApiFactory } from '../Helpers/Api';
import classes from './css/Game.module.css';
import PainterControls from '../components/PainterControls';

export const USER_TYPES = [
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
    //const [state, setState] = useState({...props.gameState, players: props.gameState.players.map(p => {return {...p, state: 0}})});
    const [state, setState] = useState({word: 'cat', user: 'Alfred', isPainter: true, players: [{username: 'Alfred', score: 100, state: 1}, {username: 'Mattias', score: 100, state: 0}, {username: 'Filip', score: 100, state: 2}]});
    const [canvas, setCanvas] = useState({
        brushColor: 'red',
        lineWidth: 10,
        canvasStyle: {
          backgroundColor: 'FFFFFF'
        },
        clear: false
    });
    const [hubConnection] = useState(props.hubConnection);
    const [toDraw, setToDraw] = useState(null);
    const [isPainter, setIsPainter] = useState(false);
    const [startDate] = useState(Date.now);


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
            setToDraw([{clear: true, timeFromStart: Date.now() - startDate - 1000}]);
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
                    const newState = {...oState,  players: newPlayers};
                    return newState;
                }
                return oState;
            })
        };

    }, []);

    useEffect(() => {
        if(hubConnection !== null){
            hubConnection.off();
            hubConnection.on('newRound', (painterName) => { onNewRound(painterName); });
            hubConnection.on('makePainter', (word) => { onMakePainter(word); });
            hubConnection.on('guessCorrect', (name) => { onGuessCorrect(name); });
            hubConnection.on('draw', (drawList) => { onDraw(drawList); });
        }
    }, [])


    useEffect(() => {
        const interval = window.setInterval(async () => {
            if (isPainter){
                sendDrawObjects();
            }
        }, 300)
        onDraw = draw => {
            if (!isPainter){
                console.log('draw: ' + draw.length);
                setToDraw(draw);
            }
        };
        return () => {
            window.clearInterval(interval);
        }
    }, [isPainter])



    const onGuessMade = async (guess) => {
        await ApiFactory.guess({guess: guess, gamecode: state.gamecode, token: state.playerId});
    };

    const onRegisterDraw = drawObj => {
        objToSend.push({...drawObj, timeFromStart: Date.now() - startDate});
    };

    const onResetToDraw = () => {
        setToDraw(null);
    };

    const onColorControlClick = (color) => {
        sendDrawObjects();
        setCanvas(pcanvas => {
            return {
                ...pcanvas,
                brushColor: color
            };
        });
    };

    const onClear = () => {
        setToDraw([{clear: true, timeFromStart: Date.now() - startDate - 2000}]);
        objToSend.push({clear: true, timeFromStart: Date.now() - startDate});
    };
    return (
        <div className={classes.Game}>
            <GameControls players={state.players} guessMade={onGuessMade} word={state.word} exit={props.exit} user={state.user} hubConnection={hubConnection}/>
            <div style={{textAlign: 'center', display: 'inline-block', width: '80%'}}>
                <div style={{width: '700px', height: '700px', backgroundColor: 'white', border: '1px solid black', margin: '0 auto'}}>
                    <Canvas isPainter={isPainter} registerDraw={onRegisterDraw} clearToDraw={onResetToDraw} {...canvas} toDraw={toDraw}/>
                </div>
                {state.word ? <PainterControls colorChanged={props.colorChanged} clear={props.clear} selectedColor={canvas.brushColor} onColorClick={onColorControlClick} onClear={onClear}/> : null}
            </div>
        </div>
    );
};

export default Game;
