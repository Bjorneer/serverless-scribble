import React, { useState } from 'react';
import classes from './css/PainterControls.module.css';
import Button from './ui/Button';
import ColorControl from './ui/ColorControl';

const PainterControls = props => {


    return (
        <div className={classes.PainterControls}>
            <ColorControl size='40px' color='red' selected={props.selectedColor==='red'} onClick={props.onColorClick}/>
            <ColorControl size='40px' color='black' selected={props.selectedColor==='black'} onClick={props.onColorClick}/>
            <ColorControl size='40px' color='green' selected={props.selectedColor==='green'} onClick={props.onColorClick}/>
            <ColorControl size='40px' color='blue' selected={props.selectedColor==='blue'} onClick={props.onColorClick}/>
            <ColorControl size='40px' color='yellow' selected={props.selectedColor==='yellow'} onClick={props.onColorClick}/>

        </div>
    );
}

export default PainterControls;