import React, { useState, useEffect } from 'react';
import { useHistory } from "react-router-dom";
import MainForm from './MainForm';
import Lobby from '../components/Lobby';
import classes from './css/MainMenu.module.css';
import TitleLogo from '../components/ui/TitleLogo';
import { GameAPI } from '../Helpers/Api';

const MainMenu = props => {
    const [isInLobby, setIsInLobby] = useState(false);
    const [gameState, setGameState] = useState(null);
    const [isLobbyLeader, setIsLobbyLeader] = useState(false);
    const history = useHistory();

    useEffect(() => {
        const interval = window.setInterval(async () => {
            if(gameState){
                const res = await GameAPI.getGameState({gamecode: gameState.gamecode, token: gameState.playerId});
                const data = await res.json();
                setGameState(data);
            }
        }, 2000);
        return () => {
            window.clearInterval(interval);
        }
    },[gameState]);

    const onJoinGameHandler = async (e, gameCode, username) => {
        e.preventDefault();
        const res = await GameAPI.join({ gamecode: gameCode, username: username })
        const data = await res.json();
        setGameState(data);
        setIsInLobby(true);
    };

    const onCreateNewGameHandler = async (e, username) => {
        e.preventDefault();
        console.log(username);
        const res = await GameAPI.create({username: username});
        const data = await res.json();
        setGameState(data);
        setIsInLobby(true);
        setIsLobbyLeader(true);
    };

    const onGameStartHandler = () => {
        history.push('/game');
    };

    let form = null;
    if(!isInLobby){
        form = <MainForm onJoinGame={onJoinGameHandler} onCreateGame={onCreateNewGameHandler} />;
    }
    
    return (
        <div className={classes.MainMenu}>
            <TitleLogo />
            {form}  
            {isInLobby ? <Lobby isOwner={isLobbyLeader} players={gameState ? gameState.players : null} startGame={onGameStartHandler} lobbyCode={gameState.gamecode}/> : null}
        </div>
    );
};

export default MainMenu;