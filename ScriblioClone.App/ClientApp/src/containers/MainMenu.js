import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import CreateGame from './CreateGame';
import JoinGame from './JoinGame';
import Button from '../components/ui/Button';
import classes from './css/MainMenu.module.css';
import TitleLogo from '../components/ui/TitleLogo';

const MainMenu = props => {
    const [isCreatingGame, setIsCreatingGame] = useState(false);
    const [lobbyCode, setLobbyCode] = useState(null);
    const history = useHistory();

    const onCreateNewGame = () => {
        setLobbyCode("A1B2C3")
        setIsCreatingGame(true);
    };

    const onGameJoinHandler = () => {
        history.push('/game');
    };

    const onGameCreateHandler = () => {
        history.push('/game');
    };

    let form = <JoinGame onSubmit={onGameJoinHandler} />;
    if (isCreatingGame) {
        form = <CreateGame onSubmit={onGameJoinHandler} />
    }

    return (
        <div className={classes.MainMenu}>
            <TitleLogo />
            {form}
            {!isCreatingGame ?
                <Button type='Secondary' onClick={onCreateNewGame}><h2>CREATE A NEW GAME</h2></ Button > :
            <h3 style={{fontWeight:'bold'}}>GAME CODE: {lobbyCode}</h3>}
        </div>
    );
};

export default MainMenu;