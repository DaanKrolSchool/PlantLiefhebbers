import React from 'react';
import './VeilingScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";

function VeilingScherm() {

    const navigate = useNavigate();

    return (
        <div>
            <div className="Veiling"> 
                <h1> De Veiling</h1>
                <p>Het huidige product:</p>
            </div>

        
            <h1 className="Name"> De corostische kamerplant </h1>

            <p1 className="image"> plaatje </p1>

            <button className="Back" type="button" onClick={() => navigate(-1)}>Terug</button>

            <div className="Kenmerken">
                <h1> Kenmerken</h1>
                <p2>Afmetingen (LxBxH):</p2>
                <p3>20x20x100</p3>
            </div>

        </div>
    );
}

export default VeilingScherm;
