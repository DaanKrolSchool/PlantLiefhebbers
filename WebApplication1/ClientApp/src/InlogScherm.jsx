import React, { useState } from "react";
import './InlogScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";

//const users = [
    //{ id: 1, email: "user@mail", password: "0000", name: "Klant", type: "Klant" },
   // { id: 2, email: "klant@mail", password: "0000", name: "klant2", type: "Klant" },
   // { id: 3, email: "pedro@mail", password: "Pedro", name: "Pedro", type: "Aanvoerder" },
//];
function InlogScherm() {
    const [email, setEmail] = useState(""); // email or id
    const [wachtwoord, setWachtwoord] = useState("");
    const navigate = useNavigate();

    async function checkLogin(e) {
        e.preventDefault();
        const res = await fetch("https://localhost:7225/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password: wachtwoord })
        });

        
        if (res.ok) {
            const data = await res.json(); // hier zit de token in
            const token = data.token || data.accessToken;
            if (!token) {
                alert("Geen token, error");
                return;
            }
            localStorage.setItem("token", token);
            alert("Welkom")
            navigate("/aanvoerder/aangemelde-producten");
        } else {
            alert("Onjuiste gebruikersgegevens!");
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
                    placeholder="E-mail of Gebruikersnaam"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <input
                    id="password"
                    type="password" name="password"
                    placeholder="Wachtwoord"
                    value={wachtwoord}
                    onChange={(e) => setWachtwoord(e.target.value)}

                />

                <button type ="button" className="CheckInlogButton" onClick={checkLogin}>Verder</button>
                <button type ="button" className="Back" onClick={() => navigate("/")}>Terug</button>

            </form>
        </div>
    );
}

export default InlogScherm;
