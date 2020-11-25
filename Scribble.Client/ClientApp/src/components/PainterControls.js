import React, { useState } from 'react';
import classes from './css/PainterControls.module.css';
import Button from './ui/Button';
import ColorControl from './ui/ColorControl';

const PainterControls = props => {


    return (
        <div className={classes.PainterControls}>
            <div className={classes.ColorPicker}>
                <ColorControl size='50px' color='red' selected={props.selectedColor==='red'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='black' selected={props.selectedColor==='black'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='green' selected={props.selectedColor==='green'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='blue' selected={props.selectedColor==='blue'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='yellow' selected={props.selectedColor==='yellow'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='brown' selected={props.selectedColor==='brown'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='orange' selected={props.selectedColor==='orange'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='purple' selected={props.selectedColor==='purple'} onClick={props.onColorClick}/>
                <ColorControl size='50px' color='white' selected={props.selectedColor==='white'} onClick={props.onColorClick}/>
            </div>
            <div style={{display: 'inline-block', width: '30%', height: '90%', float: 'left'}}>
                <Button style={{fontSize: '30px', margin: '0px', height: '100%'}} type='Warning' onClick={props.onClear}>CLEAR</Button>
            </div>
        </div>
    );
}

export default PainterControls;