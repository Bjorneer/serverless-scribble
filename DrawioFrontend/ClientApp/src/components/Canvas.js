import React from 'react'
import useCanvas from '../hooks/useCanvas'

const Canvas = props => {  
  
  const { draw, ...rest } = props
  const canvasRef = useCanvas(props.draw)
  
  return <canvas style={{border: '1px solid black'}} ref={canvasRef} {...rest}/>
}

export default Canvas