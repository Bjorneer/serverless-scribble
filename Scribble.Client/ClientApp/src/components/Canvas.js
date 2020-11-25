import React from 'react';
import ReactDOM from 'react-dom';
import assign from 'object-assign'

class Canvas extends React.Component {
  componentDidMount(){
    const canvas = ReactDOM.findDOMNode(this);

    canvas.style.width = '100%';
    canvas.style.height = '100%';
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;

    const context = canvas.getContext('2d');

    context.beginPath();

    this.setState({
      canvas
    });
  }

  componentWillReceiveProps(nextProps) {
    if(nextProps.toDraw){
      nextProps.toDraw.forEach(e => {
        if(e.clear){
          this.resetCanvas();
        }
        else if(!nextProps.isPainter)
          this.draw(e.fromX, e.fromY, e.toX, e.toY, e.color, e.width, true);
        }
      );
      if(nextProps.toDraw){
        nextProps.clearToDraw();
      }
    }
    const context = this.state.canvas.getContext('2d');
    context.beginPath();
  }

  static getDefaultStyle() {
    return {
      brushColor: '#FFFF00',
      lineWidth: 10,
      canvasStyle: {
        backgroundColor: '#00FFDC'
      },
      clear: false
    };
  }

  handleOnTouchStart (e) {
    const rect = this.state.canvas.getBoundingClientRect();
    this.setState({
      lastX: e.targetTouches[0].pageX - rect.left,
      lastY: e.targetTouches[0].pageY - rect.top,
      drawing: true
    });
  }

  handleOnMouseDown(e){
    const rect = this.state.canvas.getBoundingClientRect();
    this.setState({
      lastX: e.clientX - rect.left,
      lastY: e.clientY - rect.top,
      drawing: true
    });
  }

  handleOnTouchMove (e) {
    if (this.state.drawing) {
      const rect = this.state.canvas.getBoundingClientRect();
      const lastX = this.state.lastX;
      const lastY = this.state.lastY;
      let currentX = e.targetTouches[0].pageX - rect.left;
      let currentY = e.targetTouches[0].pageY - rect.top;
      this.draw(lastX, lastY, currentX, currentY);
      this.setState({
        lastX: currentX,
        lastY: currentY
      });
    }
  }

  handleOnMouseMove(e){
    if(this.state.drawing){
      const rect = this.state.canvas.getBoundingClientRect();
      const lastX = this.state.lastX;
      const lastY = this.state.lastY;
      let currentX = e.clientX - rect.left;
      let currentY = e.clientY - rect.top;

      this.draw(lastX, lastY, currentX, currentY);
      this.setState({
        lastX: currentX,
        lastY: currentY
      });
    }
  }

  handleonMouseUp() {
    this.setState({
      drawing: false
    });
  }

  handleOnMouseLeave(e){
    if(this.state.drawing){
      const rect = this.state.canvas.getBoundingClientRect();
      const lastX = this.state.lastX;
      const lastY = this.state.lastY;
      let currentX = e.clientX - rect.left;
      let currentY = e.clientY - rect.top;
      this.draw(lastX, lastY, currentX, currentY);

      this.setState({
        drawing: false
      });
    }
  }

  handleOnMouseEnter(e){
    if(e.buttons === 1){
      const rect = this.state.canvas.getBoundingClientRect();
      let currentX = e.clientX - rect.left;
      let currentY = e.clientY - rect.top;

      this.setState({
        lastX: currentX,
        lastY: currentY,
        drawing: true
      });
    }
  }

  draw(lX, lY, cX, cY, color, width, dontRegister) {
    const context = this.state.canvas.getContext('2d');

    context.strokeStyle = color ? color : this.props.brushColor;
    context.lineWidth = width ? width : this.props.lineWidth;

    context.moveTo(lX, lY);
    context.lineTo(cX, cY);
    context.stroke();
    if (this.props.isPainter && dontRegister !== true){
      this.props.registerDraw({color: this.props.brushColor, width: this.props.lineWidth, fromX: lX, fromY: lY, toX: cX, toY: cY});
    }
  }

  resetCanvas(){
    const context = this.state.canvas.getContext('2d');

    const width = context.canvas.width;
    const height = context.canvas.height;
    context.clearRect(0, 0, width, height);

    context.beginPath();
  }

  canvasStyle(){
    const defaults = Canvas.getDefaultStyle();
    const custom = this.props.canvasStyle;
    
    return assign({}, defaults, custom);
  }
  
  render() {
    return (
      <>
        {this.props.isPainter ? 
        <canvas style = {this.canvasStyle()}
        onMouseDown = {this.handleOnMouseDown.bind(this)}
        onTouchStart = {this.handleOnTouchStart.bind(this)}
        onMouseMove = {this.handleOnMouseMove.bind(this)}
        onTouchMove = {this.handleOnTouchMove.bind(this)}
        onMouseUp = {this.handleonMouseUp.bind(this)}
        onTouchEnd = {this.handleonMouseUp.bind(this)}
        onMouseLeave = {this.handleOnMouseLeave.bind(this)}
        onMouseEnter = {this.handleOnMouseEnter.bind(this)}
      /> : <canvas style={this.canvasStyle()}></canvas>}
      </>
      
    );
  }

}

export default Canvas;