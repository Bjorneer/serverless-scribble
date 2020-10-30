import React, {useState} from 'react';
import Canvas from '../components/Canvas';
import Chat from '../components/Chat';
import ScoreBoard from '../components/ScoreBoard';
import Button from '../components/ui/Button';



const Game = () => {
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
