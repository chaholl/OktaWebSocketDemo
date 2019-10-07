import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Leaderboard from './components/Leaderboard';
import Connect4 from './components/Connect4';
import { Security, SecureRoute, ImplicitCallback } from '@okta/okta-react';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Security
                issuer={`https://{your-Organization-Url}/oauth2/default`}
                client_id='{Your-Client-Secret}'
                redirect_uri={`${window.location.origin}/implicit/callback`}
            >
                <Layout>
                    <Route exact path='/' component={Leaderboard} />
                    <SecureRoute path='/connect4' component={Connect4} />
                    <Route path="/implicit/callback" component={ImplicitCallback} />
                </Layout>
            </Security>
        );
    }
}
