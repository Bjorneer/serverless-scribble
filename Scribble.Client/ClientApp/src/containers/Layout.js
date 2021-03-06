import React, {useCallback, useState} from 'react';
import MainMenu from './MainMenu';
import Game from './Game';
import { useHistory } from "react-router-dom";
import { Switch, Route } from 'react-router-dom';
import classes from './css/Layout.module.css';
import Footer from '../components/ui/Footer';
import Backdrop from '../components/ui/Backdrop';


const Layout = () => {
  const [gameState, setGameState] = useState(null);
  const [hubConnection, setHubConnection] = useState(null);
  const history = useHistory();

  const onExitGame = useCallback(() => {
      history.push('/');
      setGameState(null);
      if(hubConnection)
        hubConnection.stop();
      setHubConnection(null); // maybe close connection first if exists
    },
    [history]
  )

  const onGameStarting = (gamestate, hubConnection) => {
      setGameState(gamestate);
      setHubConnection(hubConnection);
      history.push('/game');
  };

  return (
    <>
      <div className={classes.Layout}>
        <Switch>
          <Route exact path='/game' render={() => <Game gameState={gameState} hubConnection={hubConnection} exit={onExitGame}/>} />
          <Route path='/' render={() => <MainMenu gameStarting={onGameStarting}/>} />
        </Switch>
        <Footer />
        <Backdrop show/>
      </div>
    </>



  );
};



export default Layout;
