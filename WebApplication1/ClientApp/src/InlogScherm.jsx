import React, { useState } from "react";
import './InlogScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";

const users = [
    { id: 1, Email: "user@mail", password: "0000", name: "Klant", type: "Klant" },
    { id: 2, Email: "klant@mail", password: "0000", name: "klant2", type: "Klant" },
    { id: 3, Email: "pedro@mail", password: "Pedro", name: "Pedro", type: "Aanvoerder" },
];
function InlogScherm() {
    const [Email, setEmail] = useState(""); // Email or id
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    async function checkLogin(e) {
        e.preventDefault();
        const userinput = users.find(u => u.Email === Email && u.password === password);
        const res = await fetch(`https://localhost:7225/Inlog/Email/${Email}`);
        const user = await res.json();
        const userpassword = user.Wachtwoord;
        const username = user.Naam;

        if (password == userpassword) {
            alert("Welkom " + username)
            navigate("/aanvoerder/aangemelde-producten");
        } else {
            alert("Onjuiste e-mail of Wachtwoord!");
        }
    }

    return (
        <div className="Login">
            <h1>Login</h1>
            <p>Type hier je e-mailadres en Wachtwoord in</p>

            <form>
                <input
                    id="Email"
                    type="text" name="Email"
                    placeholder="E-mail of GebruikersNaam"
                    value={Email}
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
