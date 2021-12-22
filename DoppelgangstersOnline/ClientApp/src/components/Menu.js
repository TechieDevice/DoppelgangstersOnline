import React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Button} from 'reactstrap';
import { Link } from 'react-router-dom';
import './misc/AstroSpace.ttf';
import './misc/Menu.css';

export default class Menu extends React.Component {
    static displayName = Menu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header>
                <Navbar style={{ backgroundColor: '#020008' }} className="navbar-expand-sm navbar-toggleable-sm">               
                    <Container>
                        <NavbarBrand style={{ marginTop: '1%' }} tag={Link} to="/">
                            <h1 className="font-face-gm" style={{ color: '#E0E0E0'}}>Doppelgangers</h1>
                        </NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse style={{ marginTop: '1%' }} className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav">
                                <NavItem>
                                    <NavLink tag={Link} className="text-light" to="/"><h5 className="font-face-gm">Play &nbsp;</h5></NavLink>
                                </NavItem>
                                <NavItem >
                                    <NavLink tag={Link} className="text-light" to="/roles"><h5 className="font-face-gm">Roles &nbsp;</h5></NavLink>
                                </NavItem>
                                <NavItem >
                                    <NavLink onClick={this.getData} tag={Link} className="text-light" to="/about"><h5 className="font-face-gm">About &nbsp;&nbsp;</h5></NavLink>
                                </NavItem>
                                <NavItem>
                                    <Button outline
                                        className="btn btn-outline-light"
                                        href="/reg-join" >
                                        <h5 className="font-face-gm" style={{ margin: '0%' }}>Log In</h5>
                                    </Button>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}