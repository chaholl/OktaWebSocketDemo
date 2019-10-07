import React, { Component } from 'react';

export class Buttons extends Component {

    render() {  
        if (!this.props.visible) {
            return <div/>
        }
        return (            
            <div className="text-center">
                <a className="btn btn-success" href="/connect4" role="button">Play Again</a>
                <a className="btn btn-danger" href="/" role="button">Quit</a>                
            </div>
        );
    }
}
