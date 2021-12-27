import React from 'react';

export default class UsersOnlinePage extends React.Component {
    static displayName = UsersOnlinePage.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true };
    }

    componentDidMount() {
        this.populateUsersData();
    }

    static renderUsersTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel" style={{ margin: '1%', color: 'white' }}>
                <thead>
                    <tr>
                        <th>user</th>
                        <th>room</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.nickName}>
                            <td>{forecast.nickName}</td>
                            <td>{forecast.room}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : UsersOnlinePage.renderUsersTable(this.state.forecasts);

        return (
            <div>
                <h1 style={{ color: 'white' }} id="tabelLabel" >Users Online</h1>
                {contents}
            </div>
        );
    }

    async populateUsersData() {
        const token = sessionStorage.getItem("accessToken");
        const response = await fetch('http://localhost:5000/api/user', {
            method: "GET", headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + token
            }
        });
        const data = await response.json();
        console.log(data);
        this.setState({ forecasts: data, loading: false });
    }
}