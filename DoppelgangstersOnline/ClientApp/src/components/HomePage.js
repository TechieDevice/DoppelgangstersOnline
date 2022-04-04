import React from 'react';
import { Button, Form } from "react-bootstrap";
import '../custom.css'

export default class HomePage extends React.Component {
    static displayName = HomePage.name;

    constructor(props) {
        super(props);

        this.state = { room: "", create: false, errorText: "" };
    }

    render() {
        switch (this.props.error) {
            case 1:
                this.setState({ errorText: "Wrong room id" });
                break;
            case 2:
                this.setState({ errorText: "An unexpected error on the server" });
                break;
            case 3:
                this.setState({ errorText: "User is unauthorized" });
                break;
            default:
                //this.setState({ errorText: "" });
                break;
        }
        const token = sessionStorage.getItem("accessToken");
        const NickName = sessionStorage.getItem("NickName");
        if ((token === null) || (NickName === null)) {
            return (
                <div className="flexing"
                    style={{ margin: '1%', marginTop: '6%', marginBottom: '10%', backgroundColor: 'gray', borderColor: '#020008' }}>
                        <h4 className="font-face-gm" style={{ margin: '0%' }}>Join In first</h4>
                </div>
            );
        } else {
            return (
                <Form
                    onSubmit={e => {
                        e.preventDefault();
                        this.props.joinRoom(NickName, this.state.room, this.state.create);
                    }} >
                    <Form.Group>
                        <div className="flexing">
                            <Form.Control style={{ margin: '1%', marginTop: '6%', width: '30%' }}
                                placeholder='room code'
                                onChange={e => this.setState({ room: e.target.value })} />
                        </div>
                        <div className="flexing">
                            <h5 style={{ color: 'red' }} size='small'>{this.state.errorText}</h5>
                        </div>
                    </Form.Group>
                    <Form.Group>

                    </Form.Group>
                    <div className="flexing">
                        <Button style={{ margin: '1%', marginBottom: '10%', backgroundColor: 'gray', borderColor: '#020008' }}
                            className="btn btn-light"
                            type='submit'
                            disabled={!this.state.room}>
                            <h5 className="font-face-gm" style={{ margin: '0%' }}>Join Game</h5>
                        </Button>
                    </div>
                    <div className="flexing">
                        <Button style={{ margin: '1%', marginBottom: '10%', backgroundColor: 'gray', borderColor: '#020008' }}
                            className="btn btn-light"
                            type='submit'
                            onClick={e => this.setState({ create: true })}>
                            <h5 className="font-face-gm" style={{ margin: '0%' }}>Create room</h5>
                        </Button>
                    </div>
                </Form>
            );
        }
    }
}
