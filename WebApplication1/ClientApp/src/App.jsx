import React from 'react';
import { Routes, Route, useNavigate } from "react-router-dom";
import './App.css';
import ProductAanmelden from "./ProductAanmelden.jsx";
import InlogScherm from "./InlogScherm.jsx";

function Home() {
    const navigate = useNavigate();

    return (
        <div className="App-background">
            <h1>Bied op planten</h1>
            <p>De plek om planten te kopen en verkopen,</p>
            <button className="OverzichtButton" onClick={() => navigate("/ProductAanmelden")}>Bekijk onze planten</button>
            <button className="AanvoerderButton" onClick={() => navigate("/ProductAanmelden")}>Aanvoerder overzicht</button>
            <button className="LoginButton" onClick={() => navigate("/InlogScherm")}>Inloggen</button>
        </div>
    );
}

function App() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/ProductAanmelden" element={<ProductAanmelden />} />
            <Route path="/InlogScherm" element={<InlogScherm />} />

        </Routes>
    );
}

export default App;