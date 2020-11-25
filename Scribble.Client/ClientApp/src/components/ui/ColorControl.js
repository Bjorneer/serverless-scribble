import React from 'react';

const ColorControl = props => {
    const style = {
        width: parseInt(props.size) + (props.selected ? 7 : 0),
        height: parseInt(props.size) + (props.selected ? 7 : 0),
        borderRadius: '50%',
        backgroundColor: props.color,
        border: (props.selected ? '2px' : '1px') + ' solid black',
        boxSizing: 'border-box',
        outline: 'none',
    };

    return <button style={style} onClick={() => props.onClick(props.color)} />
}

export default ColorControl;
