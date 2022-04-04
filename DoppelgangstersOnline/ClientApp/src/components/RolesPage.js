import React from 'react';

export default class RolesPage extends React.Component {
    static displayName = RolesPage.name;

    constructor(props) {
        super(props);

        this.state = {
            text: <p>Something about</p>
        };
    }

    render() {
        return (
            <div>
                {this.state.text}
            </div>
        );
    }
}