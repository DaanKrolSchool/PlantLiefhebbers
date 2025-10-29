import React from 'react';
import { Routes, Route } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import './App.css';
import ProductAanmelden from "./ProductAanmelden.jsx";

function App() {
    const navigate = useNavigate();
    
    return (
        <div className="App-background">
            <h1>Bied op planten</h1>
            <p>De plek om planten te kopen en verkopen,</p>

            <button
                className="OverzichtButton"
                onClick={() => navigate("/ProductAanmelden")}>Bekijk onze planten</button>
            <switch>
                <Routes>
                    <Route path="/ProductAanmelden" element={<ProductAanmelden />} />
                </Routes>
            </switch>

            <button
                className="AanvoerderButton"
                onClick={() => navigate("/ProductAanmelden")}>Aanvoerder Overzicht</button>
            <switch>
                <Routes>
                    <Route path="/ProductAanmelden" element={<ProductAanmelden />} />
                </Routes>
            </switch>

            <button
                className="LoginButton"
                onClick={() => navigate("/ProductAanmelden")}>Inloggen</button>
            <switch>
                <Routes>
                    <Route path="/ProductAanmelden" element={<ProductAanmelden />} />
                </Routes>

            </switch>
        </div>
    );
}
export default App;
