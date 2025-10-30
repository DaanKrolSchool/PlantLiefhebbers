import React, { useState } from "react";
import './InlogScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";

const users = [
    { id: 1, email: "user@mail", password: "0000", name: "Klant", type: "Klant" },
    { id: 2, email: "klant", password: "0000", name: "klant2", type: "Klant" },
    { id: 3, email: "pedro@mail", password: "Pedro", name: "Pedro", type: "Aanvoerder" },
];


function InlogScherm() {
    const [email, setEmail] = useState(""); // email or id
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    function checkLogin(e) {
        e.preventDefault();
        const user = users.find(u => u.email === email && u.password === password);

        if (user) {
            if (user.type === "Aanvoerder") {
                navigate("/aanvoerder/aangemelde-producten");
            } else {
                navigate("/veilingscherm");
            }
        } else {
            alert("Onjuiste e-mail of wachtwoord!");
        }
    }

    return (
        <div className="Login">
            <h1>Login</h1>
            <p>Type hier je e-mailadres en wachtwoord in</p>

            <form>
                <input
                    id="email"
                    type="text" name="email"
                    placeholder="E-mail"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <input
                    id="password"
                    type="password" name="password"
                    placeholder="Wachtwoord"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}

                />

                <button className="CheckInlogButton" onClick={checkLogin}>Verder</button>
                <button className="Back" type="button" onClick={() => navigate("/")}>Terug</button>

            </form>
        </div>
    );
}

export default InlogScherm;
