import React, { useState } from 'react';
import './Register.css';
import { useNavigate } from 'react-router-dom';

function Register() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState<{ username: string; email: string; password: string }>({
        username: '',
        email: '',
        password: ''
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const klantData = {
            naam: formData.username,
            email: formData.email,
            wachtwoord: formData.password,
            adres: "" 
        };

        try {
            const response = await fetch("https://localhost:7225/klantregister/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(klantData)
            });

            if (response.ok) {
                const result = await response.json();
                alert(result.message);
                navigate("/"); // go back to home
            } else {
                const error = await response.text();
                alert("Registratie mislukt: " + error);
            }
        } catch (err: any) {
            alert("Er is een fout opgetreden: " + err.message);
        }
    };

    return (
        <div className="Register">
            <h1>Registreren</h1>
            <p>Vul het formulier in om je account te maken:</p>

            <form onSubmit={handleSubmit}>
                <div>
                    <input
                        type="text"
                        name="username"
                        placeholder="Voer gebruikersnaam in"
                        value={formData.username}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <input
                        type="email"
                        name="email"
                        placeholder="Voer e-mail in"
                        value={formData.email}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <input
                        type="password"
                        name="password"
                        placeholder="Voer wachtwoord in"
                        value={formData.password}
                        onChange={handleChange}
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