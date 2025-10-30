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
                    <input
                        type="text"
                        name="username"
                        placeholder="Voer gebruikersnaam in"
                        required
                    />
                </div>

                <div>                    
                    <input
                        type="email"
                        name="email"                       
                        placeholder="Voer e-mail in"
                        required
                    />
                </div>

                <div>                    
                    <input
                        type="password"
                        name="password"                        
                        placeholder="Voer wachtwoord in"
                        required
                    />
                </div>

                <div>
                    <button className="RegisterBack" type="button" onClick={() => navigate("/")}>Terug</button>                
                    <button className="RegisterSubmit" type="submit">Registreren</button>
                </div>
            </form>
        </div>
    );
}

export default Register;
