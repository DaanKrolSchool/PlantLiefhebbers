import './VeilingScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";
import React, { useState, useEffect } from 'react';

function VeilingScherm() {

    const StartPrice = 100.87;
    const MinPrice = 70;
    const Speed = 1;


    const navigate = useNavigate();
    const [price, setPrice] = useState(StartPrice);

    useEffect(() => {
        if (price > MinPrice) {
            const timer = setInterval(() => setPrice(prev => prev - (Speed * (0.5)/60)), 1);
            return () => clearInterval(timer);
        }
        else {
            alert("Helaasss, te lang gewacht");
            //delete het plantje of markeer als verkocht
            //volgende plantje laden
        }
    }, [price]);

    function handleBuy() {
        alert("GEFELICITEERD!!! Je hebt het plantje gekocht");
        //delete het plantje of markeer als verkocht
        //volgende plantje laden
    }



    return (
        <div>
            

        
            <h1 className="Name"> De corostische kamerplant </h1>

            <p className="MainImage"> plaatje </p>

            <button className="Back" type="button" onClick={() => navigate(-1)}>Terug</button>

            <div className="Kenmerken">
                <h1> Kenmerken:</h1>
                <p className = "Title">Afmetingen (LxBxH):</p>
                <p className = "Feature">{"  - " + "20x20x100"}</p>
            </div>

            <div className="NextProducts">
                <h1> Volgende planten:</h1>
                <p className="NextImage"> plaatje </p>
            </div>

            <div className="Timer">
                €{price.toFixed(2)}
            </div>

            <button className="Buy" type="button" onClick={() => handleBuy()}>Kopen</button>


        </div>
    );
}

export default VeilingScherm;
