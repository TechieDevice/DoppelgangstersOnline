import React from 'react';
import { Container } from 'react-bootstrap';

export default class AboutPage extends React.Component {
    static displayName = AboutPage.name;

    constructor(props) {
        super(props);

        this.state = {
            data: null
        };
    }

    getData = () => {
        const Page = "About";
        const response = fetch('https://172.28.112.1:44334/api/content', {
            method: "GET",
            headers: { "Accept": 'application/json', 'Content-Type': 'application/json' },
            body: JSON.stringify({
                Page
            })
        });

        const data = response.body;
        return data;
    }

    //componentWillMount() {
    //    var data = this.getData();
    //    this.setState({ data: data });
    //}

    render() {
        return (
            <div>
                <Container>
                    <h2>About</h2>
                    <p>{this.state.fata}</p>
                </Container>
            </div>
        );
    }
}