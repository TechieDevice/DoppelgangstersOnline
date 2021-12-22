import React from 'react';
import { Form, Button, FormControl, InputGroup } from "react-bootstrap";
import { useState } from 'react';


const SendMessageForm = ({ sendMessage }) => {
    const [message, setMessage] = useState('');

    return <Form
        onSubmit={e => {
            e.preventDefault();
            sendMessage(message);
            setMessage('');
        }}>
        <div>
            <InputGroup hasValidation={true}>
                <FormControl placeholder='message...'
                    onChange={e => setMessage(e.target.value)} value={message} />
                <div className="input-group-append">
                    <Button variant="primary" type='submit' style={{ marginBottom: '100%' }}
                        disabled={!message}>
                        Send
                    </Button>
                </div>
            </InputGroup>
        </div>
    </Form>
}

export default SendMessageForm;