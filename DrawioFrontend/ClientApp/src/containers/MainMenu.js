import React, { useState, useEffect } from 'react';
import MainForm from './MainForm';
import Lobby from '../components/Lobby';
import classes from './css/MainMenu.module.css';
import TitleLogo from '../components/ui/TitleLogo';
import { ApiFactory } from '../Helpers/Api';
import * as signlaR from '@microsoft/signalr';
import Backdrop from '../components/ui/Backdrop';

let onUserJoined;
let onGameStarted;

const MainMenu = props => {
    const [isInLobby, setIsInLobby] = useState(false);//useState(true);//
    const [gameState, setGameState] = useState(null);//useState({gamecode: '123ABC', players: [{username: 'hello'}, {username: 'carl'}, {username: 'marting'}, , {username: 'Filip'}, , {username: 'verygoodpainter'}, , {username: 'testing'}]})//
    const [isLobbyLeader, setIsLobbyLeader] = useState(true);
    const [hubConnection, SetHubConnection] = useState(null);

    useEffect(() => {
        onUserJoined = (username) => {
            setGameState(state => {
                console.log('User joined: ' + username)
                const newPlayers = [...state.players];
                if(!state.players.some(p => p.username === username))
                    newPlayers.push({username: username, score: 0});
                const newState = {...state, players: newPlayers}
                return newState;
            });
        };

    }, [])

    useEffect(() => {
        onGameStarted = () => {
            console.log(gameState);
            props.gameStarting(gameState, hubConnection);
        };
    }, [gameState, hubConnection]);

    const onJoinGameHandler = async (e, gameCode, username) => {
        e.preventDefault();
        await ApiFactory.join({ gamecode: gameCode, username: username })
            .then(res => {
                if(!res.ok)
                    throw Error(res.statusText);
                return res.json();
            })
            .then(data => {
                if(data){
                    setGameState(data);
                    setIsInLobby(true);

                    ApiFactory.negotiate({}, {headers: {
                        'x-ms-signalr-userid': data.playerId
                    }})
                    .then(res => res.json())
                    .then(info => {
                        console.log(info)
                        const protocol = new signlaR.JsonHubProtocol();
                        const hubConn = new signlaR.HubConnectionBuilder()
                        .withUrl(info.url, {accessTokenFactory: () => info.accessToken})
                        .withHubProtocol(protocol)
                        .build();

                        hubConn.on('userJoined', onUserJoined);
                        hubConn.on('gameStarted', () => { onGameStarted() });

                        hubConn.start()
                        .then(console.log('Connection established'))
                        .catch(err => console.error(err));
    
                        ApiFactory.joinGroup({}, {headers: {
                            'x-ms-signalr-userid': data.playerId,
                            'x-ms-signalr-group': data.gamecode
                        }})
                        .then(res => console.log('Group joined'));

                        SetHubConnection(hubConn);
                    })
                    .catch(err => console.error(err));
                }
            })
            .catch(err => console.log(err));
    };

    const onCreateNewGameHandler = async (e, username) => {
        e.preventDefault();
        await ApiFactory.create({username: username})
            .then(res => {
                if(res.ok)
                    return res.json();
            })
            .then(data => {
                if(data){
                    setGameState(data);
                    setIsInLobby(true);
                    setIsLobbyLeader(true);
                  
                    ApiFactory.negotiate({}, {headers: {
                        'x-ms-signalr-userid': data.playerId
                    }})
                    .then(res => { return res.json();})
                    .then(info => {
                        console.log(info)
                        const protocol = new signlaR.JsonHubProtocol();
                        const hubConn = new signlaR.HubConnectionBuilder()
                        .withUrl(info.url, {accessTokenFactory: () => info.accessToken})
                        .withHubProtocol(protocol)
                        .build();

                        hubConn.on('userJoined', onUserJoined);
                        hubConn.on('gameStarted', () => { onGameStarted() });

                        hubConn.start()
                        .then(console.log('Connection established'))
                        .catch(err => console.error(err));
    
                        ApiFactory.joinGroup({}, {headers: {
                            'x-ms-signalr-userid': data.playerId,
                            'x-ms-signalr-group': data.gamecode,
                        }})
                        .then(res => console.log('Group joined'));

                        SetHubConnection(hubConn);
                    })
                    .catch(err => console.error(err));
                }
            })
            .catch(err => console.log(err));
    };

    const onGameStartHandler = async (e) => {
        e.preventDefault();
        await ApiFactory.start({gamecode: gameState.gamecode, token: gameState.playerId})
            .then(res => {
                if(!res.ok)
                    throw Error(res.statusText);

                console.log(hubConnection);
            })
            .catch(err => console.log(err));
    };

    return (
        <div style={{textAlign:'center'}}>
            <div className={classes.MainMenu}>
                <TitleLogo />
                {isInLobby ? 
                    <Lobby isOwner={isLobbyLeader} players={gameState ? gameState.players : null} startGame={onGameStartHandler} lobbyCode={gameState.gamecode}/> : 
                    <MainForm onJoinGame={onJoinGameHandler} onCreateGame={onCreateNewGameHandler} />}
            </div>
            <Backdrop show/>
        </div>

    );
};

export default MainMenu;