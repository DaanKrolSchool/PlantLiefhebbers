import React from 'react';
import {Routes, Route, useNavigate} from "react-router-dom";

import './App.css';

// Inloggen en registreren
import InlogScherm from "./InlogScherm";
import Register from "./Register";

// AanvoerderDashboard
import Aanvoerder from "./Aanvoerder/Aanvoerder";
import ProductAanmelden from "./Aanvoerder/ProductAanmelden";
import AangemeldeProducten from "./Aanvoerder/AangemeldeProducten";
import VerkoopOverzicht from "./Aanvoerder/VerkoopOverzicht";

// VeilingmeesterDashboard
import VeilingmeesterRoute from "./Veilingmeester/VeilingmeesterRoute";
import Veilingmeester from "./Veilingmeester/Veilingmeester";
import VeilingBeheren from "./Veilingmeester/VeilingBeheren";
import VAangemeldeProducten from "./Veilingmeester/AangemeldeProducten";
import VVerkoopOverzicht from "./Veilingmeester/VerkoopOverzicht";
//Veiling scherm
import VeilingScherm from "./VeilingScherm";

function Home() {
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const isIngelogd = !!token;
    return (
        <div className="App-background">
            <h1>Bied op planten</h1>
            <p>De plek om planten te kopen en verkopen,</p>
            {isIngelogd ? (
                <button
                    className="OverzichtButton"
                    onClick={() => navigate("/veilingscherm")}
                >
                    Bekijk onze planten
                </button>
            ) : (
                <h2 className="LoginHint">
                    Log eerst in om de veilingen te bekijken
                </h2>
            )}

            {/*}
            <button className="AanvoerderButton" onClick={() => navigate("/aanvoerder/aangemelde-producten")}>Aanvoerder overzicht</button>
            <button className="VeilingmeesterButton" onClick={() => navigate("/veilingmeester/aangemelde-producten")}>Veilingmeester overzicht</button>
            */}
            <button className="LoginButton" onClick={() => navigate("/inloggen")}>Inloggen</button>
            <button className="RegistreerButton" onClick={() => navigate("/registreren")}>Registreren</button>
        </div>
    );
}

function App() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            
            <Route path="/inloggen" element={<InlogScherm />} />

            <Route path="/registreren" element={<Register />} />

            <Route path="/veilingscherm" element={<VeilingScherm />} />

            <Route path="/aanvoerder" element={<Aanvoerder />}>
                <Route path="product-aanmelden" element={<ProductAanmelden />} />
                <Route path="aangemelde-producten" element={<AangemeldeProducten />} />
                <Route path="verkoop-overzicht" element={<VerkoopOverzicht />} />
            </Route>

            <Route element={<VeilingmeesterRoute />}>
                <Route path="/veilingmeester" element={<Veilingmeester />}>
                    <Route path="veiling-beheren" element={<VeilingBeheren />} />
                    <Route path="aangemelde-producten" element={<VAangemeldeProducten />} />
                    <Route path="verkoop-overzicht" element={<VVerkoopOverzicht />} />
                </Route>
            </Route>

            {/*
            <Route path="bieden" />
            <Route path="producten" />
            */}
            
        </Routes>
    );
}

export default App;