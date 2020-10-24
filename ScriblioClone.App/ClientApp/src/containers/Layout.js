import React from 'react';
import MainMenu from './MainMenu';
import Game from './Game';
import { Switch, Route } from 'react-router-dom';
import classes from './css/Layout.module.css';
import Footer from '../components/Footer';


const Layout = () => {
  return (
    <>
      <div className={classes.Layout}>
        <Switch>
          <Route exact path='/game' component={Game} />
          <Route path='/' component={MainMenu} />
        </Switch>
      </div>
      <Footer />
    </>



  );
};

export default Layout;
