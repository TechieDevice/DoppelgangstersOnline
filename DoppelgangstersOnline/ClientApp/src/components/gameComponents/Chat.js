import React from 'react';
import MessageContainer from './MessageContainer';
import SendMessageForm from './SendMessageForm';
import ConnectedUsers from './ConnectedUsers';


export default class Chat extends React.Component {
    static displayName = Chat.name;

    render() {
        return (
            <div>
                <ConnectedUsers users={this.props.users} />
                <div className='chat'>
                    <MessageContainer messages={this.props.messages} />
                    <SendMessageForm sendMessage={this.props.sendMessage} />
                </div>
            </div>
        );
    }
}
