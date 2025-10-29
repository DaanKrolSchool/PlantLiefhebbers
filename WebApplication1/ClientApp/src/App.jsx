import React from 'react';
import {Routes, Route, useNavigate, BrowserRouter} from "react-router-dom";

import './App.css';
//Inloggen en registreren
// import Registreer Scherm from "./RegistreerScherm.jsx";
import InlogScherm from "./InlogScherm.jsx";
// AanvoerderDashboard
import Aanvoerder from "./Aanvoerder/Aanvoerder.jsx";
import ProductAanmelden from "./Aanvoerder/ProductAanmelden.jsx";
import AangemeldeProducten from "./Aanvoerder/AangemeldeProducten.jsx";
import VerkoopOverzicht from "./Aanvoerder/VerkoopOverzicht.jsx";

function Home() {
    const navigate = useNavigate();

    return (
        <div className="App-background">
            <h1>Bied op planten</h1>
            <p>De plek om planten te kopen en verkopen,</p>
            {/* <button className="OverzichtButton" onClick={() => navigate("/(Link naar overzicht)")}>Bekijk onze planten</button> */}
            <button className="AanvoerderButton" onClick={() => navigate("/aanvoerder/product-aanmelden")}>Aanvoerder overzicht</button>
            <button className="LoginButton" onClick={() => navigate("/inloggen")}>Inloggen</button>
        </div>
    );
}

function App() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            
            <Route path="/inloggen" element={<InlogScherm />} />

            <Route path="/aanvoerder" element={<Aanvoerder  /> }>
                <Route path="product-aanmelden" element={<ProductAanmelden />} />
                <Route path="aangemelde-producten" element={<AangemeldeProducten />} />
                <Route path="verkoop-overzicht" element={<VerkoopOverzicht />} />
            </Route>
        </Routes>
    );
}

export default App;