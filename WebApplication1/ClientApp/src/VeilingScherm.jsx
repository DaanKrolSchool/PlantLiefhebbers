import './VeilingScherm.css';
import { Routes, Route, useNavigate, BrowserRouter } from "react-router-dom";
import React, { useState, useEffect } from 'react';

function VeilingScherm() {
    const [Naam, setNaam] = useState("");
    const [soort, setSoort] = useState("");
    const [hoeveelheid, setHoeveelheid] = useState("");
    const [potmaat, setPotmaat] = useState("");
    const [steellengte, setSteellengte] = useState("");
    const [mprijs, setMprijs] = useState("");
    const StartPrice = 100.87;
    //const MinPrice = 70;
    const Speed = 1;
    const [progresiebar, setProgresiebar] = useState(100);


    const navigate = useNavigate();
    const [price, setPrice] = useState(StartPrice);


    useEffect(() => {
        async function fetchData() {
            const res = await fetch(`https://localhost:7225/Product/eerste`);
            const data = await res.json();
            setNaam(data?.Naam ?? "—");
            setSoort(data?.soortPlant ?? "—");
            setHoeveelheid(data?.aantal ?? "—");
            setPotmaat(data?.potMaat ?? "—");
            setSteellengte(data?.steelLengte ?? "—");
        }
        fetchData();
    }, []);

    useEffect(() => {
        if (price > mprijs) {
            const timer = setInterval(() => setPrice(prev => prev - (Speed * (0.5) / 60)), 1);
            const percenttage = ((price - mprijs) / (StartPrice - mprijs)) * 100;
            setProgresiebar(Math.max(0, Math.min(100, percenttage)));
            return () => clearInterval(timer);
        }
        else {
            alert("Helaasss, te lang gewacht");
            //status van plantje op verkocht zetten
        }
    }, [price]);

    function handleBuy() {
        alert("GEFELICITEERD!!! Je hebt het plantje gekocht");

    }



    return (
        <div>
            <h1 className="Name"> {Naam} </h1>

            <p className="MainImage"> plaatje </p>

            <button className="Back" type="button" onClick={() => navigate(-1)}>Terug</button>

            <div className="KenmerkenL">
                <h2> Kenmerken:</h2>
                <p className="TitleL">Soort plant:</p>
                <p className="FeatureL">{soort}</p>

                <p className="TitleL">Potmaat:</p>
                <p className="FeatureL">{potmaat}</p>
            </div>

            <div className="KenmerkenR">
                <p className="TitleR">Steel lengte:</p>
                <p className="FeatureR">{steellengte}</p>

                <p className="TitleR">Hoeveelheid:</p>
                <p className="FeatureR">{hoeveelheid}</p>
            </div>

            <div className="NextProducts">
                <h2> Volgende planten:</h2>
                <p className="NextImage"> plaatje </p>
            </div>

            <div className="Timer">
                €{price.toFixed(2)}
            </div>

            <div className="ProgresBar">
                <div className="ProgressFill" style={{ width: `${progresiebar}%` }}></div>
            </div>


            <button className="Buy" type="button" onClick={() => handleBuy()}>Kopen</button>


        </div>
    );
}

export default VeilingScherm;
