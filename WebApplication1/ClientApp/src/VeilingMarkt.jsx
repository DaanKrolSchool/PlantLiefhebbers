import React, { useState } from "react";
import './InlogScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";

function InlogScherm() {
    const [Email, setEmail] = useState("");
    const [Password, setPassword] = useState("");
    const navigate = useNavigate();

    function CheckInlog(e) {
        e.preventDefault()
        if (Email === "Pedro@mail" && Password === "Pedro") {
            alert("Hoerey!! Succes!")
            navigate("/");
        }
        else { alert("Goed geprobeert, maar dat klopt niet.") }

    }
    return (
        <div className="content">
            <h1>Login</h1>
            <p>Type hier je e-mailadres en wachtwoord in</p>

            <form classnName="inputVak">
                <input
                    id="email"
                    type="text" name="email"
                    value={Email}
                    placeholder="Email"
                    onChange={(e) => setEmail(e.target.value)}
                />
                <br />
                <input
                    id="pssword"
                    type="password" name="password"
                    value={Password}
                    placeholder="Wachtwoord"
                    onChange={(e) => setPassword(e.target.value)}
                />
                <br />
            </form>

            <button className="CheckInlogButton" onClick={CheckInlog}>Verder</button>
            <button className="Back" type="button" onClick={() => navigate("/")}>Terug</button>

        </div>
    );
}

export default InlogScherm;
