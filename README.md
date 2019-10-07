# Lets play the game

Try out this sample project to see how WebSocket, SignalR and ASP.Net Core can be used togther to create a simple game. Since the application is a multi-player game, one simple way to authenticate and identify individual players is to use Okta.

If you don’t already have one, sign up for a forever-free developer account at https://developer.okta.com/signup/

Log in to your developer console, navigate to __Applications__, then click __Add Application__. Select __Single-Page App__, then click __Next__.
The sample project is setup to run on port 5000 so you should add: `http://localhost:5000` as the __Base URI__. __LoginRedirect URI__ should be set to `http://localhost:5000/implicit/callback`

Once you click Done to save your Okta application, make a note of the Client ID and Organization URL. You can find the Org URL on the upper right corner of the Okta Dashboard home page.

Update the react-router component with the authentication information. In `ClientApp/src/App.js`, update the code to:

```
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
```


