import React, { Component } from 'react'
import InfoBar from './InfoBar'
import Board from './Board'
import { Player } from './Player'
import { Buttons } from './Buttons'
import { withAuth } from '@okta/okta-react';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'


export default withAuth(class Connect4 extends Component {
    constructor(props) {
        super(props)

        this.state = {
            board: null,
            hubConnection: null,
            message: 'Waiting for another player...',
            yourTurn: false,
            player1: { email: '', name: '', color: '' },
            player2: { email: '', name: '', color: '' },
            showButtons: false
        }
    }

    async componentDidMount() {

        const hubConnection = new HubConnectionBuilder().withUrl("/game").configureLogging(LogLevel.Information).build();
        const user = await this.props.auth.getUser();

        this.setState({ hubConnection }, () => {
            this.state.hubConnection
                .start()
                .then(
                    () => {
                        console.log('Connection started!');
                        this.state.hubConnection.invoke('updateuser', user.email, user.name);
                    })
                .catch(err => console.log('Error while establishing connection :('));

            this.state.hubConnection.on('renderBoard', board => {
                this.setState({ board: board })
            });
            this.state.hubConnection.on('color', color => {
                this.setState({ color: color })
            });
            this.state.hubConnection.on('turn', player => {
                if (player === this.state.color) {
                    this.setState({ message: "You're up. What's your move?", yourTurn: true, showButtons: false })
                } else {
                    this.setState({ message: player + ' is thinking...', yourTurn: false, showButtons: false })
                }
            });

            this.state.hubConnection.on('rollcall', (player1, player2) => {
                this.setState({ player1: player1, player2: player2, })
            });

            this.state.hubConnection.on('concede', () => {
                this.setState({ message: 'Your opponent conceded. You win', yourTurn: false, showButtons: true })
                this.state.hubConnection.stop()
            });

            this.state.hubConnection.on('victory', (player, board) => {
                let newState = { yourTurn: false }
                if (player === this.state.color) {
                    newState['message'] = 'You win!'
                } else {
                    newState['message'] = 'You lose!'
                }
                newState["board"] = board;
                newState["showButtons"] = true;
                this.setState(newState)
                this.state.hubConnection.stop()
            });

        });


    }

    onColumnClick = column => this.state.hubConnection.invoke('columnClick', column).catch(err => console.error(err.toString()));

    render() {
        return (
            <div>
                <div>
                    <h1 className="display-3 text-center">Connect 4</h1>
                </div>
                <div className="row bg-dark border border-dark text-light">
                    <Player player={this.state.player1} />
                    <div className="col-sm">
                        <InfoBar color={this.state.color} message={this.state.message} />
                        <Board visible={this.state.showBoard} board={this.state.board} onColumnClick={this.onColumnClick} yourTurn={this.state.yourTurn} />
                    </div>
                    <Player player={this.state.player2} />
                </div>
                <div className="text-center">
                    <Buttons visible={this.state.showButtons} />
                </div>
            </div>
        )
    }
});
