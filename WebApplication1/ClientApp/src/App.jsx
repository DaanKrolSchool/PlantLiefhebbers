import React from 'react';
import {Routes, Route, useNavigate, BrowserRouter} from "react-router-dom";

import './App.css';
//Inloggen en registreren
import InlogScherm from "./InlogScherm.jsx";
import Register from "./Register.jsx";
// AanvoerderDashboard
import Aanvoerder from "./Aanvoerder/Aanvoerder.jsx";
import ProductAanmelden from "./Aanvoerder/ProductAanmelden.jsx";
import AangemeldeProducten from "./Aanvoerder/AangemeldeProducten.jsx";
import VerkoopOverzicht from "./Aanvoerder/VerkoopOverzicht.jsx";
//Veiling scherm
import VeilingScherm from "./VeilingScherm.jsx";

function Home() {
    const navigate = useNavigate();

    return (
        <div className="App-background">
            <h1>Bied op planten</h1>
            <p>De plek om planten te kopen en verkopen,</p>
            <button className="OverzichtButton" onClick={() => navigate("/VeilingScherm")}>Bekijk onze planten</button>
            <button className="AanvoerderButton" onClick={() => navigate("/aanvoerder/aangemelde-producten")}>Aanvoerder overzicht</button>
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

            <Route path="/aanvoerder" element={<Aanvoerder  /> }>
                <Route path="product-aanmelden" element={<ProductAanmelden />} />
                <Route path="aangemelde-producten" element={<AangemeldeProducten />} />
                <Route path="verkoop-overzicht" element={<VerkoopOverzicht />} />
            </Route>

            {/* 
            <Route path="/veilingmeester" element={<Veilingmeester /> }>
                <Route path=""
            </Route>
            */}

            {/*
            <Route path="bieden" />
            <Route path="producten" />
            */}
            
        </Routes>
    );
}

export default App;