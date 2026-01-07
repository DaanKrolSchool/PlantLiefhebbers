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
    const [makkelijkheid, setmakkelijkheid] = useState<number>(5);
    const [seizoensplant, setseizoensplant] = useState<string>("-");
    const [temperatuur, settemperatuur] = useState<number>(0);
    const [water, setwater] = useState<number>(0);
    const [leeftijd, setleeftijd] = useState<number>(0);

    const [maxPrijs, setMaxPrijs] = useState<number>(0);
    const [prijsVerandering, setPrijsVerandering] = useState<number>(0);


    const [progresiebar, setProgresiebar] = useState<number>(100);

    const navigate = useNavigate();

    const [price, setPrice] = useState<number>(0);

    const [error, setError] = useState("");
    const [notf, setNotf] = useState("");


    async function fetchData() {
        const token = localStorage.getItem("token");

        // de huidige veiling die start
        const res = await fetch(`https://localhost:7225/Product/eerste`, {
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        });

        const text = await res.text();
        const data = text ? JSON.parse(text) : null;

        // dit doet hij als er geen actieve veiling is
        if (!data || !data.productId) {
            setNaam("geen veiling gestart");
            setSoort("—");
            setHoeveelheid(0);
            setPotmaat(0);
            setSteellengte(0);
            setmakkelijkheid(0);
            settemperatuur(0);
            setwater(0);
            setleeftijd(0);
            setseizoensplant("—");

            setCurrentProductId(null);

            setMprijs(0);
            setMaxPrijs(0);
            setPrijsVerandering(0);
            setPrice(0);
        } else {
            setNaam(data.naam ?? "—");
            setSoort(data.soortPlant ?? "—");
            setHoeveelheid(data.aantal ?? 0);
            setPotmaat(data.potMaat ?? 0);
            setSteellengte(data.steelLengte ?? 0);
            setCurrentProductId(data.productId ?? null);

            setmakkelijkheid(data.makkelijkheid ?? 0);
            settemperatuur(data.temperatuur ?? 0);
            setwater(data.water ?? 0);
            setleeftijd(data.leeftijd ?? 0);
            setseizoensplant(data.seizoensplant ?? "—");

            setMprijs(data.minimumPrijs ?? 0);
            setMaxPrijs(data.maximumPrijs ?? 0);
            setPrijsVerandering(data.prijsVerandering ?? 0);

            setPrice(data.maximumPrijs ?? 0);
        }

        // 3 volgende producten
        const resNext = await fetch(`https://localhost:7225/Product/volgende`, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!resNext.ok) {
            setNextProducts(["🪴", "🪴", "🪴"]);
            return;
        }

        const nextText = await resNext.text();
        const nextDataRaw = nextText ? JSON.parse(nextText) : [];

        // Soms komt .NET als { $values: [...] }
        const arr =
            Array.isArray(nextDataRaw) ? nextDataRaw :
                Array.isArray(nextDataRaw?.$values) ? nextDataRaw.$values :
                    [];

        setNextProducts(arr.length ? arr : ["🪴", "🪴", "🪴"]);
    }

    // elke 2 secondden kijken of er een veiling gestart is
    useEffect(() => {
        fetchData();

        const interval = setInterval(() => {
            fetchData();
        }, 2000);

        return () => clearInterval(interval);
    }, []);

    // Timer: loopt alleen als er een actief product is
    useEffect(() => {
        if (!currentProductId) return;
        if (maxPrijs <= 0) return;
        if (prijsVerandering <= 0) return;

        setPrice(maxPrijs);

        const timer = setInterval(() => {
            setPrice(prev => {
                const newPrice = prev - prijsVerandering;

                if (newPrice <= mprijs) {
                    clearInterval(timer);
                    setError("Helaas, te lang gewacht!");
                    setTimeout(() => setError(""), 2500);
                    return mprijs;
                }

                return newPrice;
            });
        }, 1000);

        return () => clearInterval(timer);
    }, [currentProductId, maxPrijs, prijsVerandering, mprijs]);

    async function handleBuy() {
        if (!currentProductId) return;

        try {
            const token = localStorage.getItem("token");

            // TIJDELIJK: delete product (later koppelen aan klant)
            await fetch(`https://localhost:7225/Product/${currentProductId}`, {
                method: "DELETE",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            setNotf("GEFELICITEERD!!! Je hebt het plantje gekocht");
            setTimeout(() => setNotf(""), 2500);

            await fetchData();
        } catch (e) {
            console.error(e);
            setError("Er is iets misgegaan bij het kopen van het plantje.");
            setTimeout(() => setError(""), 2500);
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

                <p className="TitleL">Seizoen:</p>
                <p className="FeatureL">{seizoensplant}</p>


                <p className="TitleL">Temperatuur:</p>
                <p className="FeatureL">{temperatuur}</p>

                <p className="TitleL">Makkelijkheid (1/10):</p>
                <p className="FeatureL">{makkelijkheid}</p>
            </div>

            {error && <div className="ErrorBox">{error}</div>}
            {notf && <div className="NotBox">{notf}</div>}

            <div className="KenmerkenR">
                <p className="TitleR">Steel lengte:</p>
                <p className="FeatureR">{steellengte}</p>

                <p className="TitleR">Hoeveelheid:</p>
                <p className="FeatureR">{hoeveelheid}</p>

                <p className="TitleR">L water per week:</p>
                <p className="FeatureR">{water}</p>

                <p className="TitleR">Leeftijd in maanden:</p>
                <p className="FeatureR">{leeftijd}</p>

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

            <div className="Klok">
                <img src="/klok2.png" alt="Klok2"/>
            </div>

            {/*<div className="ProgresBar">*/}
            {/*    <div className="ProgressFill" style={{ width: `${progresiebar}%` }}></div>*/}
            {/*</div>*/}


            <button className="Buy" type="button" onClick={() => handleBuy()}>Kopen</button>


        </div>
    );
}

export default VeilingScherm;
