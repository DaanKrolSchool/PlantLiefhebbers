import React, { useState } from "react";
import './RegisterAndInlog.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";

function InlogScherm() {
    const [email, setEmail] = useState(""); // email or id
    const [wachtwoord, setWachtwoord] = useState("");
    const navigate = useNavigate();

    async function checkLogin(e: React.MouseEvent<HTMLButtonElement>) {
        e.preventDefault();
        const res = await fetch("https://localhost:7225/inlog/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email: email, wachtwoord: wachtwoord })
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

            const rol = (data.rol || "").toLowerCase();

            if (rol === "veilingmeester") {
                navigate("/veilingmeester/veiling-beheren");
            } else if (rol === "aanvoerder") {
                navigate("/aanvoerder/product-aanmelden");
            } else if (rol === "klant") {
                navigate("/veilingscherm");
            }
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

                <div>
                    <button className="Submit" type="submit" onClick={checkLogin}>Verder</button>
                    <button className="Back" type="button" onClick={() => navigate("/")}>Terug</button>
                </div>
            </form>
        </div>
    );
}

export default InlogScherm;
