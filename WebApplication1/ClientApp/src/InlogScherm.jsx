import React from 'react';
import './InlogScherm.css';

function InlogScherm() {
    return (
        <div classnName="content">
            <h1>Login</h1>
            <p>Type hier je gebruikersnaam en wachtwoord in</p>

            <form classnName="inputVak">
                <label htmlFor="email">Email:</label>
                <input type="text" name="email" /><br />
                <label htmlFor="wachtwoord">Wachtwoord:</label>
                <input type="text" name="wachtwoord" /><br />
            </form>
        </div>
    );
}

export default InlogScherm;
