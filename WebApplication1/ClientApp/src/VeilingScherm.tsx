import './VeilingScherm.css';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';



function VeilingScherm() {
    const [naam, setNaam] = useState<string>("");
    const [soort, setSoort] = useState<string>("");

    const [nextProducts, setNextProducts] = useState<string[]>([]);
    const [currentProductId, setCurrentProductId] = useState<number | null>(null);

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
            setCurrentProductId(data?.productId ?? null);


            const resNext = await fetch(`https://localhost:7225/Product/volgende`, {
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            });

            const nextData = await resNext.json();
            setNextProducts(nextData);

        }
        fetchData();
    }, []);

    async function fetchData() {
        const token = localStorage.getItem("token");

        // Eerste product ophalen
        const res = await fetch(`https://localhost:7225/Product/eerste`, {
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        });
        const data = await res.json();

        setNaam(data?.naam ?? "—");
        setSoort(data?.soortPlant ?? "—");
        setHoeveelheid(data?.aantal ?? 0);
        setPotmaat(data?.potMaat ?? 0);
        setSteellengte(data?.steelLengte ?? 0);
        setCurrentProductId(data?.productId ?? null);

        const resNext = await fetch(`https://localhost:7225/Product/volgende`);
        const nextData = await resNext.json();
        setNextProducts(nextData);
    }

    useEffect(() => {
        if (!currentProductId) return;

        setPrice(StartPrice); 
        setProgresiebar(100);

        const timer = setInterval(() => {
            setPrice(prev => {
                const newPrice = prev - (Speed * 0.5) / 60;

                if (newPrice <= mprijs) {
                    clearInterval(timer);
                    alert("Helaas, te lang gewacht!");
                    return mprijs;
                }

                return newPrice;
            });
        }, 100);

        return () => clearInterval(timer);
    }, [currentProductId, mprijs]);

    async function handleBuy() {
        if (!currentProductId) return;

        try {
            const token = localStorage.getItem("token");
            // TIJDELIJK VERWIJDERD DIT HET PRODUCT IPV DAT DIE AAN USER GEKOPPELD WORDT EN DAARNA GESKIPT WORDT
            await fetch(`https://localhost:7225/Product/${currentProductId}`, {
                method: "DELETE",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            alert("GEFELICITEERD!!! Je hebt het plantje gekocht");

            await fetchData();

            // Timer resetten
            setPrice(StartPrice);
            setProgresiebar(100);
        } catch (error) {
            console.error(error);
            alert("Er is iets misgegaan bij het kopen van het plantje.");
        }
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
                <h1>Volgende planten:</h1>

                {nextProducts.map((naam, i) => (
                    <p key={i} className="NextImage">{naam}</p>
                ))}
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
