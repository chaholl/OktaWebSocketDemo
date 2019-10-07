import React, { Component } from 'react';
import Gravatar from 'react-gravatar'

export class Player extends Component {

    render() {
        let style = {
            borderColor: this.props.player.color,
            borderStyle: 'solid',
            borderWidth: 3
        };
        let textStyle = {
            color: this.props.player.color
        };
        if (this.props.player.email !== '') {
            return (
                <div className="col-sm text-center bg-secondary">
                    <h2 style={textStyle}>{this.props.player.name}</h2>
                    <Gravatar email={this.props.player.email} size={100} className="rounded-circle" style={style} />
                    <h2 className="text-capitalize" style={textStyle}>{this.props.player.color}</h2>
                </div>
            );
        }
        else {
            return (<div className="col-sm text-center bg-secondary"></div>)
        }
    }
}
