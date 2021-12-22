import React from 'react';
import { Button, Form } from "react-bootstrap";
import "../custom.css";

export default class RegJoinPage extends React.Component {
    static displayName = RegJoinPage.name;

    constructor(props) {
        super(props);

        this.state = { nickName: "", pass: "", regLog: "login", error:"", text: "Don't have an account? Register" };
    }

    switch = () => {
        if (this.state.regLog === "login") {
            this.setState({ regLog: "register", text: "Alreagy have an account? Login"});
            
        }
        else {
            this.setState({ regLog: "login", text: "Don't have an account? Register" });
        }
    }

    submit = async () => {
        try {
            const formData = new FormData();
            const NickName = this.state.nickName;
            const Password = this.state.pass;
            formData.append("NickName", this.state.nickName);
            formData.append("Password", this.state.pass);

            if (this.state.regLog === "login") {
                const response = await fetch('https://localhost:44334/api/user/login', {
                    method: "POST",
                    headers: { "Accept": 'application/json', 'Content-Type': 'application/json' },
                    credentials: 'include',
                    body: JSON.stringify({
                        NickName,
                        Password
                    })
                });

                const data = await response.json();

                if (response.ok === true) {
                    sessionStorage.setItem("accessToken", data.token);
                    sessionStorage.setItem("NickName", NickName);
                }
                else {
                    console.log("Error: ", response.status, data.errorText);
                }
            }
            else {
                await fetch('https://localhost:44334/api/user/register', {
                    method: "POST",
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        NickName,
                        Password
                    })
                });

                this.switch();
            }
        }
        catch (e) {
            this.state.error = e.message;
        }
    }

    logout = () => {
        sessionStorage.clear();
    }


    render() {
        const token = sessionStorage.getItem("accessToken");
        if (token !== null) {
            return (
                <Form className="flexing"
                    onSubmit={e => {
                        this.logout();
                    }}>
                    <Button style={{ margin: '1%', marginTop: '6%', marginBottom: '10%', backgroundColor: 'gray', borderColor: '#020008' }}
                        className="btn btn-light"
                        type='submit'>
                        <h4 className="font-face-gm" style={{ margin: '0%' }}>Logout</h4>
                    </Button>
                </Form>
            );
        }
        else {
            return (
                <Form
                    onSubmit={e => {                 
                        this.submit();
                    }}>             
                    <Form.Group>
                        <div className="flexing">
                            <h2 className="font-face-gm"
                                style={{ margin: '1%', marginTop: '6%', color: 'white' }}>
                                {this.state.regLog}</h2>
                        </div>
                        <div className="flexing">
                            <Form.Control style={{ margin: '1%', width: '30%' }}
                                placeholder='nickname'
                                onChange={e => this.setState({ nickName: e.target.value })} />
                        </div>
                        <div className="flexing">
                            <Form.Control style={{ margin: '1%', width: '30%' }}
                                type="password" name="password"
                                placeholder='password'
                                onChange={e => this.setState({ pass: e.target.value })} />
                        </div>
                        <h5 style={{ color: 'red' }} size='small'>{this.state.error}</h5>
                    </Form.Group>
                    <div className="flexing">
                        <Button style={{ margin: '1%', backgroundColor: 'gray', borderColor: '#020008' }}
                            className="btn btn-light"
                            type='submit'
                            disabled={!this.state.nickName || !this.state.pass}>
                            <h4 className="font-face-gm" style={{ margin: '0%' }}>Submit</h4>
                        </Button>
                    </div>
                    <div className="flexing">
                        <Button style={{ margin: '1%', marginBottom: '10%', color: 'white'}}
                            variant="link"
                            onClick={() => this.switch()}>
                            <h5 className="font-face-gm">{this.state.text}</h5>
                        </Button>
                    </div>
                </Form>
            );
        }
    }
}