import React from 'react';
import { Route } from 'react-router';
import { Switch } from 'react-router-dom';
import { Container } from 'reactstrap';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import HomePage from './components/HomePage';
import UsersOnlinePage from './components/UsersOnlinePage';
import RolesPage from './components/RolesPage';
import RegJoinPage from './components/RegJoinPage';
import Menu from './components/Menu';
import LobbyPage from './components/gameComponents/LobbyPage';
import './custom.css'

export default class App extends React.Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { connection: null, room: "", messages: [], users: [], error: 0, token: null, data: null};
        this.joinRoom = this.joinRoom.bind(this);
    }

    joinRoom = async (name, room, create) => {
        try {
            this.setState({ error: 0 });
            this.state.room = room;
            const token = sessionStorage.getItem("accessToken");

            const connection = new HubConnectionBuilder()
                .withUrl("http://192.168.1.102:5000/api/game", { accessTokenFactory: () => token })
                .configureLogging(LogLevel.Information)
                .build();

            connection.on("RecieveRoomId", (roomId) => {
                this.setState({ room: roomId });
            });

            connection.on("UsersInRoom", (users) => {
                this.state.users = users;
            });

            connection.on("RecieveMessage", (user, message) => {
                this.setState(prevState => ({
                    messages: [...prevState.messages, { user: user, message }]
                }))
            });

            connection.on("RecieveBotMessage", (message) => {
                this.setState(prevState => ({
                    messages: [...prevState.messages, { user: "Game", message }]
                }))
            });

            connection.onclose(e => {
                this.setState({ messages: [], users: [], connection: null })
            });

            await connection.start();
            if (create) {
                await connection.invoke("RoomCreate", name);
            }
            else
                await connection.invoke("RoomConnect", name, room);


            this.setState({ connection: connection });
        } catch (e) {
            if (e.message === "An unexpected error occurred invoking 'RoomConnect' on the server.") {
                this.setState({ error: 1 });
            }
            else if (e.message === "Failed to invoke 'RoomConnect' because user is unauthorized") {
                this.setState({ error: 3 });
            }
            else if (e.message === "Failed to invoke 'RoomCreate' because user is unauthorized") {
                this.setState({ error: 3 });
            }
            else {
                this.setState({ error: 2 });
                console.log(e);
            }
        }
    }

    closeConnection = async () => {
        try {
            await this.state.connection.stop();
        } catch (e) {
            console.log(e);
        }
    }

    sendMessage = async (message) => {
        try {
            await this.state.connection.invoke("SendMessage", message);
        } catch (e) {
            console.log(e);
        }
    }

    render() {
        try {
            if (this.state.connection == null) {
                return (
                    <div>
                        <Menu />
                        <div className="mid">
                            <Container>
                                <Switch>
                                    <Route exact path='/'>
                                        <HomePage joinRoom={this.joinRoom} error={this.state.error}/>
                                    </Route>
                                    <Route path='/roles'>
                                        <RolesPage />
                                    </Route>
                                    <Route path='/users-online'>
                                        <UsersOnlinePage />
                                    </Route>
                                    <Route path='/reg-join'> 
                                        <RegJoinPage />
                                    </Route>
                                </Switch>
                            </Container>
                        </div>
                    </div>
                );
            } else {
                return (
                    <div className='mid'>
                        <Route exact path='/'>
                            <LobbyPage
                                connection={this.state.connection}
                                room={this.state.room}
                                messages={this.state.messages}
                                users={this.state.users}
                                closeConnection={this.closeConnection}
                                sendMessage={this.sendMessage}/>
                        </Route>
                    </div>
                );
            }
        } catch(e) {
            console.log(e);
        }
    }
}


