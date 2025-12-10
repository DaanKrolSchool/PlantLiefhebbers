import './VeilingScherm.css';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';



function VeilingScherm() {
    const [naam, setNaam] = useState<string>("");
    const [soort, setSoort] = useState<string>("");

    const [hoeveelheid, setHoeveelheid] = useState<number>(0);
    const [potmaat, setPotmaat] = useState<number>(0);
    const [steellengte, setSteellengte] = useState<number>(0);
    const [mprijs, setMprijs] = useState<number>(0);
    const StartPrice = 100.87;
    const Speed = 1;

    const [progresiebar, setProgresiebar] = useState<number>(100);

    const navigate = useNavigate();

    const [price, setPrice] = useState<number>(StartPrice);


    useEffect(() => {
        async function fetchData() {
            const token = localStorage.getItem("token");
            const res = await fetch(`https://localhost:7225/Product/eerste`, {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            const data = await res.json();
            setNaam(data?.naam ?? "—");
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
            <h1 className="Name"> {naam} </h1>

            <p className="MainImage"> plaatje </p>

            <button className="Back" type="button" onClick={() => navigate(-1)}>Terug</button>

            <div className="KenmerkenL">
                <h1> Kenmerken:</h1>
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
                <h1> Volgende planten:</h1>
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
