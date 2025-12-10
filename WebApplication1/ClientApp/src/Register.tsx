import React, { useState } from 'react';
import './RegisterAndInlog.css';
import { useNavigate } from 'react-router-dom';

function Register() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState<{ username: string; email: string; password: string; rol: string }>({
        username: '',
        email: '',
        password: '',
        rol: ''
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>)  => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const klantData = {
            naam: formData.username,
            email: formData.email,
            wachtwoord: formData.password,
            rol: formData.rol,
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
                    <select
                        name="rol"
                        value={formData.rol}
                        onChange={handleChange}
                        required
                    >
                        <option value="">Selecteer een rol</option>
                        <option value="klant">Klant</option>
                        <option value="aanvoerder">Aanvoerder</option>
                        <option value="veilingmeester">Veilingmeester</option>
                    </select>
                </div>


                <div>
                    <button className="RegisterSubmit" type="submit">Registreren</button>
                    <button className="Back" type="button" onClick={() => navigate("/")}>Terug</button>
                </div>
            </form>
        </div>
    );
}

export default Register;