import React from 'react';
import './Register.css';
import { useNavigate } from 'react-router-dom';

function Register() {
    const navigate = useNavigate();
    return (
        <div className="Register">
            <h1>Registreren</h1>
            <p>Vul het formulier in om je account te maken:</p>

            <form>
                <div>
                    <label>Gebruikersnaam:</label><br />
                    <input
                        type="text"
                        name="username"
                        placeholder="Voer gebruikersnaam in"
                        required
                    />
                </div>

                <div>
                    <label>E-mail:</label><br />
                    <input
                        type="email"
                        name="email"                       
                        placeholder="Voer e-mail in"
                        required
                    />
                </div>

                <div>
                    <label>Wachtwoord:</label><br />
                    <input
                        type="password"
                        name="password"                        
                        placeholder="Voer wachtwoord in"
                        required
                    />
                </div>

                <div className="button-group">
                    <button type="button" onClick={() => navigate("/")}>Terug</button>                
                    <button type="submit">Registreren</button>
                </div>
            </form>
        </div>
    );
}

export default Register;
