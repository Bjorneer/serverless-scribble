import React, { useState, useEffect } from 'react';
import { useHistory } from "react-router-dom";
import MainForm from './MainForm';
import Lobby from '../components/Lobby';
import classes from './css/MainMenu.module.css';
import TitleLogo from '../components/ui/TitleLogo';
import { GameAPI } from '../Helpers/Api';

const MainMenu = props => {
    const [isInLobby, setIsInLobby] = useState(false);
    const [lobbyCode, setLobbyCode] = useState(null);
    const [gameState, setGameState] = useState(null);
    const [isLobbyLeader, setIsLobbyLeader] = useState(false);
    const history = useHistory();

    useEffect(() => {
        setInterval(async () => {
            if(isInLobby && gameState){
                GameAPI.getGameState({gamecode: lobbyCode, token: gameState.playerId})
                    .then(response => response.json())
                    .then(data => {
                        console.log(data);
                        setGameState(data);
                    })
                    .catch(err => console.log(err));
            }
        }, 5000);
    });

    const onJoinGameHandler = (e, gameCode, username) => {
        e.preventDefault();
        GameAPI.join({ gamecode: gameCode, username: username })
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setGameState(data);
                setIsInLobby(true);
                setLobbyCode(data.gamecode);
            })
            .catch(err => {
                console.log(err);
            });
    };

    const onCreateNewGameHandler = (e, username) => {
        e.preventDefault();
        console.log(username);
        GameAPI.create({username: username})
            .then(response => response.json())
            .then(data => {
                console.log(data);
                setIsInLobby(true);
                setLobbyCode(data.gamecode);
                setGameState(data);
                setIsLobbyLeader(true);
            })
            .catch((err) => {
                console.error(err);
            });
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
            {isInLobby ? <Lobby isOwner={isLobbyLeader} players={gameState ? gameState.players : null} startGame={onGameStartHandler} lobbyCode={lobbyCode}/> : null}
        </div>
    );
};

export default MainMenu;