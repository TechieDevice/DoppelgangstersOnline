﻿import React from 'react';


const ConnectedUsers = ({ users }) => <div className='user-list'>
    <h4>Users in play</h4>
    {users.map((u, idx) => <h6 key={idx}>{u}</h6>)}
</div>

export default ConnectedUsers;
