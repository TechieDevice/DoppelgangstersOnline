import React from 'react';
import Chat from './Chat';
import { Container, Navbar, NavbarBrand, NavbarToggler, NavItem, Button } from 'reactstrap';
import '../misc/Menu.css';


export default class LobbyPage extends React.Component {
    static displayName = LobbyPage.name;

    render() {      
        return (
            <div>
                <header>
                    <Navbar style={{ backgroundColor: '#020008' }} className="navbar-expand-sm navbar-toggleable-sm">
                        <Container>
                            <NavbarBrand style={{ marginTop: '1%' }}>
                                <h1 className="font-face-gm" style={{ color: '#E0E0E0' }}>Doppelgangers &nbsp;</h1>
                            </NavbarBrand>
                            <NavbarToggler className="mr-2" />
                            <ul className="navbar-nav">
                                <NavItem style={{ marginTop: '1%' }}>
                                    <h2 className="font-face-gm" style={{ color: '#E0E0E0' }}>Room code: {this.props.room}&nbsp;&nbsp;</h2>
                                </NavItem>
                                <NavItem style={{ marginTop: '1%' }}>
                                    <div className='leave-room'>
                                        <Button variant='danger' onClick={() => this.props.closeConnection()}>Leave</Button>
                                    </div>
                                </NavItem>
                            </ul>
                        </Container>
                    </Navbar>
                </header>
                <div className='app'>
                    <Chat messages={this.props.messages}
                        users={this.props.users}
                        sendMessage={this.props.sendMessage}
                        connection={this.props.connection} />
                </div>
            </div>
        );
    }
}