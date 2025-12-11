import React, { useState } from "react";
import './InlogScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";

function InlogScherm() {
    const [Email, setEmail] = useState("");
    const [Password, setPassword] = useState("");
    const navigate = useNavigate();
    const [error, setError] = useState("");
    const [notf, setNotf] = useState("");

    function CheckInlog(e: any) {
        e.preventDefault()
        if (Email === "Pedro@mail" && Password === "Pedro") {
            setNotf("Hoerey!! Succes!")
            setTimeout(() => setNotf!(""), 2500);
            navigate("/");
        }
        else {
            setError("Goed geprobeert, maar dat klopt niet.")
            setTimeout(() => setError!(""), 2500);
        }

    }
    return (
        <div className="content">
            <h1>Login</h1>
            <p>Type hier je e-mailadres en wachtwoord in</p>

            <form className="inputVak">
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

            {error && <div className="ErrorBox">{error}</div>}
            {notf && <div className="NotBox">{notf}</div>}

            <button className="CheckInlogButton" onClick={CheckInlog}>Verder</button>
            <button className="Back" type="button" onClick={() => navigate("/")}>Terug</button>

        </div>
    );
}

export default InlogScherm;
