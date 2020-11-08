import React, {useState, useEffect} from 'react';
import Canvas from '../components/Canvas';
import Chat from '../components/Chat';
import ScoreBoard from '../components/ScoreBoard';
import Button from '../components/ui/Button';
import { GameAPI } from '../Helpers/Api';
import { useHistory } from "react-router-dom";




const Game = props => {
    const [state, setState] = useState(props.gameState);
    const history = useHistory();
    
    if(!state){
        history.push('/');
    }
    useEffect(() => {
        const interval = window.setInterval(async () => {
            const res = await GameAPI.getGameState({gamecode: state.gamecode, token: state.playerId});
            const data = await res.json();
            setState(data);
            console.log(data);
        }, 2000);
        return window.clearInterval(interval);
    }, [state]);
    const users = [
        {
            name: 'Alfred',
            score: 100,
            type: 'Disabled'
        },
        {
            name: 'Mattias',
            score: 300,
            type: 'Painter'
        },
        {
            name: 'Filip',
            score: 500,
            type: 'Accepted'
        },
        {
            name: 'olle',
            score: 1000
        }
    ];

    const draw = (ctx, frameCount) => {
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height)
        ctx.fillStyle = '#000000'
        ctx.beginPath()
        ctx.arc(50, 100, 20*Math.sin(frameCount*0.05)**2, 0, 2*Math.PI)
        ctx.fill()
      }

    return (
        <div>
            <Button type='Warning'><h4>EXIT GAME</h4></Button>
            <ScoreBoard users={users}/>
            <Canvas draw={draw}/>
            <Chat></Chat>
        </div>
    );
};

export default Game;
