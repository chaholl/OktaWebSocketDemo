import React, { Component } from 'react';

export default class Leaderboard extends Component {

    constructor(props) {
        super(props);
        this.state = { leaderboard: [], loading: true };
    }

    componentDidMount() {
        this.populateLeaderboardData();
    }

    static renderLeaderboard(leaderboard) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Player</th>
                        <th>Played</th>
                        <th>Won</th>
                        <th>Percentage</th>
                    </tr>
                </thead>
                <tbody>
                    {leaderboard.map(score =>
                        <tr key={score.playerName}>
                            <td>{score.playerName}</td>
                            <td>{score.played}</td>
                            <td>{score.won}</td>
                            <td>{score.percentage}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Leaderboard.renderLeaderboard(this.state.leaderboard);

        return (
            <div className="jumbotron">
                <h1 className="display-4">Okta WebSockets Sample App </h1>
                <p className="lead">Play Connect 4. You know you want to.</p>
                <hr className="my-4" /> 
                <p>A simple application that demonstrates the use of websockets to manage two-way communication between server and browser</p>
                <h1>Leaderboard</h1>
                {contents}
                <a className="btn btn-success btn-lg" href="/connect4" role="button">Play</a>
            </div>            
        );
    }

    async populateLeaderboardData() {
        const response = await fetch('api/leaderboard');
        const data = await response.json();
        this.setState({ leaderboard: data, loading: false });
    }
}