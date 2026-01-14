import './VeilingScherm.css';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import Tippy, { tippy } from '@tippyjs/react';
import 'tippy.js/dist/tippy.css';
//import 'tippy.js/themes/light.css';
import 'tippy.js/animations/perspective.css';

function formatDatum(d: string) {
    // verwacht "YYYY-MM-DD"
    const date = new Date(d);
    return date.toLocaleDateString("nl-NL"); // bv 16-1-2026
}

function VeilingScherm() {
    let pvMultiplier = 1.0
    const [naam, setNaam] = useState<string>("");
    const [soort, setSoort] = useState<string>("");
    const [prijsGeschiedenis, setPrijsGeschiedenis] = useState<
        { datum: string; prijs: number; aanvoerder: string }[]
    >([]);


    const [lastLoadedId, setLastLoadedId] = useState<number | null>(null);


    const [hoeveelheidKopen, setHoeveelheidKopen] = useState<number>(0);
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
    const [klokLocatie, setKlokLocatie] = useState<string>("rijnsburg");
    const [maxPrijs, setMaxPrijs] = useState<number>(0);
    const [prijsVerandering, setPrijsVerandering] = useState<number>(0);

    const [aanvoerder, setAanvoerder] = useState<string>("—");



    const [progresiebar, setProgresiebar] = useState<number>(100);

    const navigate = useNavigate();

    const [price, setPrice] = useState<number>(0);

    const [error, setError] = useState("");
    const [notf, setNotf] = useState("");


    //  async function prijsGeschiedenisBerekenen(id: number) {
    //      const response = await fetch(`https://localhost:7225/Product/id/${id}`);
    //  //    alert(response.ok)

    //      const dto = await response.json();
    ////      alert(dto)
    //      setPrijsGeschiedenis(String(id));

    //  }





    async function fetchVeilingInfo(id: number) {
        const token = localStorage.getItem("token");

        const res = await fetch(`https://localhost:7225/Product/veilinginfo/${id}`, {
            headers: {
                // token is optioneel omdat endpoint AllowAnonymous is
                ...(token ? { "Authorization": `Bearer ${token}` } : {})
            }
        });

        if (!res.ok) return null;
        
        return await res.json();
    }

    async function fetchHistoriePerSoort(soortPlant: string) {
        const res = await fetch(`https://localhost:7225/Product/historie/soort/${encodeURIComponent(soortPlant)}`);
        if (!res.ok) return [];
        return await res.json();
    }

    async function fetchData() {
        const token = localStorage.getItem("token");

        // de huidige veiling die start
        const res = await fetch(`https://localhost:7225/Product/klant/eerste/${klokLocatie}`, {
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        });

        const data = res.ok ? await res.json() : null;
        

        // dit doet hij als er geen actieve veiling is
        if (!data || !data.productId) {
            
            setPrijsGeschiedenis([]);
            
            setLastLoadedId(null);
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
            setAanvoerder("—");

            setCurrentProductId(null);
            setKlokLocatie(klokLocatie);

            setMprijs(0);
            setMaxPrijs(0);
            setPrijsVerandering(0);
            setPrice(0);
        } else {
            const id = data.productId as number;
           
            setCurrentProductId(id);
            
            // Alleen opnieuw laden als het een nieuw product is
            if (lastLoadedId !== id) {
                setLastLoadedId(id);
                
                // haal info
                const info = await fetchVeilingInfo(id);
                if (!info) return;
               
                setAanvoerder(info.aanvoerderNaam ?? info.aanvoerder ?? info.firmaNaam ?? "—");

                // haal prijs geschiedenis
                //const geschiedenis = await fetchHistoriePerSoort(info.soortPlant);
                //setPrijsGeschiedenis(geschiedenis);


                //alert(info.aantal)
                // zet state
                setNaam(info.naam ?? "—");
                setSoort(info.soortPlant ?? "—");
                
                setHoeveelheid(info.aantal ?? 0);
                setPotmaat(info.potMaat ?? 0);
                setSteellengte(info.steelLengte ?? 0);

                setmakkelijkheid(info.makkelijkheid ?? 0);
                settemperatuur(info.temperatuur ?? 0);
                setwater(info.water ?? 0);
                setleeftijd(info.leeftijd ?? 0);
                setseizoensplant(info.seizoensplant ?? "—");

                setMprijs(info.minimumPrijs ?? 0);
                setMaxPrijs(info.maximumPrijs ?? 0);
                setPrijsVerandering(info.prijsVerandering ?? 0);
                //setKlokLocatie(info.klokLocation ?? "-");

                //setPrice(info.maximumPrijs ?? 0);
            }
        }

        // 3 volgende producten
        const resNext = await fetch(`https://localhost:7225/Product/klant/volgende/${klokLocatie}`, {
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

    // KAN WEG??
    // elke 1 secondden kijken of er een veiling gestart is 
    useEffect(() => {

        if (!klokLocatie) return;

        fetchData();

        const interval = setInterval(fetchData, 1000);
        return () => clearInterval(interval);
    }, [klokLocatie]);

    //laad data als je refreshed of pagina opend
    useEffect(() => {
        fetchData();
    }, []);

    // Timer: loopt alleen als er een actief product is
    //useEffect(() => {
    //    if (!currentProductId) return;
    //    if (maxPrijs <= 0) return;

    //    if (prijsVerandering <= 0) return;

    //    setPrice(maxPrijs);

    //    const timer = setInterval(() => {
    //        setPrice(prev => {

    //            const newPrice = prev - (prijsVerandering * pvMultiplier);

    //            if (newPrice <= mprijs) {
    //                setPrice(maxPrijs)
    //                if (pvMultiplier < (maxPrijs / 10)) {
    //                    pvMultiplier = (pvMultiplier * 1.15)
    //                } else {
    //                    pvMultiplier = (maxPrijs / 10)
    //                }
    //                //const newPrijsVerandering = prijsVerandering * 1.5
    //                //setPrijsVerandering(newPrijsVerandering)
    //                //clearInterval(timer);

    //                setError("Prijs word gereset");
    //                setTimeout(() => setError(""), 3000);
    //                //return mprijs;
    //            }

    //            return newPrice;
    //        });
    //    }, 1000);

    //    return () => clearInterval(timer);
    //}, [currentProductId, maxPrijs, prijsVerandering, mprijs]);

    useEffect(() => {
        if (!currentProductId) return;

        const fetchPrice = async () => {
            try {
                const res = await fetch("https://localhost:7225/api/prijs/price")
                const data = await res.json();

                //prijs
                setPrice(data);
                //alert("test")

            } catch (err) {
                console.error("Failed to fetch price", err);
            }
        };

        // fetch immediately
        fetchPrice();

        // then fetch every second
        const interval = setInterval(fetchPrice, 1000);

        return () => clearInterval(interval);
    }, [currentProductId]);


    async function veranderLocatie(locatie: string) {
        
        setKlokLocatie(locatie)
        setPrice(maxPrijs);
        //alert(Progresiebar)
        setProgresiebar(100);
        //await fetchData();
    }

    async function handleBuy() {
        if (!currentProductId) return;

        try {
            const token = localStorage.getItem("token");
            // TIJDELIJK VERWIJDERD DIT HET PRODUCT IPV DAT DIE AAN USER GEKOPPELD WORDT EN DAARNA GESKIPT WORDT
            await fetch(`https://localhost:7225/Product/${currentProductId}?hoeveelheidKopen=${hoeveelheidKopen}&price=${price}`, {
                method: "PATCH",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            const info = await fetchVeilingInfo(currentProductId);
            //alert(info.isVerkocht);
            //setHoeveelheid(info.aantal)
            //await fetchData();
            setNotf("GEFELICITEERD!!! Je hebt het plantje gekocht")
            setTimeout(() => setNotf!(""), 2500)

            

            // Timer resetten
            if (hoeveelheid < 1) {
                setPrice(maxPrijs);
                setProgresiebar(100);
            }

        } catch (error) {
            console.error(error);
            //await fetchData();
            const info = await fetchVeilingInfo(currentProductId);
            alert(info.isVerkocht);
            setError("Er is iets misgegaan bij het kopen van het plantje.")
            setTimeout(() => setError!(""), 2500)
        }
    }

    //async function fetchVeilingInfo(id: number) {
    //    const token = localStorage.getItem("token");

    //    const res = await fetch(`https://localhost:7225/Product/veilinginfo/${id}`, {
    //        headers: {
    //            "Authorization": token ? `Bearer ${token}` : "",
    //            "Content-Type": "application/json"
    //        }
    //    });

    //    if (!res.ok) return;

    //    const info = await res.json();

    //    setNaam(info.naam ?? "—");
    //    setSoort(info.soortPlant ?? "—");
    //    setHoeveelheid(info.aantal ?? 0);
    //    setPotmaat(info.potMaat ?? 0);
    //    setSteellengte(info.steelLengte ?? 0);

    //    setmakkelijkheid(info.makkelijkheid ?? 0);
    //    settemperatuur(info.temperatuur ?? 0);
    //    setwater(info.water ?? 0);
    //    setleeftijd(info.leeftijd ?? 0);
    //    setseizoensplant(info.seizoensplant ?? "—");

    //    setMprijs(info.minimumPrijs ?? 0);
    //    setMaxPrijs(info.maximumPrijs ?? 0);
    //    setPrijsVerandering(info.prijsVerandering ?? 0);

    //    setPrice(info.maximumPrijs ?? 0);
    //}



    // ===== Historie lijsten (zelfde soort / zelfde aanvoerder) =====
    //const historieSoort = prijsGeschiedenis
    //    .slice()
    //    .sort((a, b) => new Date(b.datum).getTime() - new Date(a.datum).getTime()); // nieuwste eerst

    //const last10Soort = historieSoort.slice(0, 10);

    //const historieAanvoerder = historieSoort.filter(p => p.aanvoerderNaam === aanvoerder);
    //const last10Aanvoerder = historieAanvoerder.slice(0, 10);

    //function avg(list: { prijs: number }[]) {
    //    if (!list.length) return 0;
    //    return list.reduce((s, x) => s + Number(x.prijs), 0) / list.length;
    //}

    //const avgSoort = avg(historieSoort);
    //const avgAanvoerder = avg(historieAanvoerder);

    return (
        <div>
            <h1 className="Name"> {naam} </h1>

            
            {(currentProductId &&
                <img className="MainImage" src={`https://localhost:7225/images/${currentProductId}.png`} alt={`Afbeelding van ${naam}`}></img>
            )}

            <button className="Back" type="button" onClick={() => { localStorage.removeItem("token"); navigate("/") }}>Uitloggen</button>

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
                <img src="/klok2.png" alt="Klok2" />
            </div>
            

            {/*<div className="ProgresBar">*/}
            {/*    <div className="ProgressFill" style={{ width: `${progresiebar}%` }}></div>*/}
            {/*</div>*/}


            <button className="Buy" type="button" onClick={() => handleBuy()}>
                Kopen
            </button>
            {/*<Tippy*/}
            {/*    animation={"perspective"}*/}
            {/*    interactive={true}*/}
            {/*    content={*/}
            {/*        <div style={{ minWidth: 420 }}>*/}
            {/*            <h3 style={{ margin: "0 0 6px 0" }}>Verkoopgeschiedenis van {soort}</h3>*/}

            {/*            {last10Soort.length === 0 ? (*/}
            {/*                <p style={{ margin: 0 }}>Nog geen verkopen</p>*/}
            {/*            ) : (*/}
            {/*                <>*/}
            {/*                    */}{/* Header */}
            {/*                    <div*/}
            {/*                        style={{*/}
            {/*                            display: "grid",*/}
            {/*                            gridTemplateColumns: "1.2fr 1fr 1fr",*/}
            {/*                            gap: 12,*/}
            {/*                            fontWeight: 700,*/}
            {/*                            borderBottom: "1px solid rgba(255,255,255,0.3)",*/}
            {/*                            paddingBottom: 4,*/}
            {/*                            marginBottom: 6*/}
            {/*                        }}*/}
            {/*                    >*/}
            {/*                        <span>Aanvoerder</span>*/}
            {/*                        <span>Datum</span>*/}
            {/*                        <span>Prijs</span>*/}
            {/*                    </div>*/}

            {/*                    */}{/* Rows */}
            {/*                    <div style={{ display: "grid", gap: 6, maxHeight: 240, overflowY: "auto" }}>*/}
            {/*                        {last10Soort.map((p, i) => (*/}
            {/*                            <div*/}
            {/*                                key={i}*/}
            {/*                                style={{*/}
            {/*                                    display: "grid",*/}
            {/*                                    gridTemplateColumns: "1.2fr 1fr 1fr",*/}
            {/*                                    gap: 12*/}
            {/*                                }}*/}
            {/*                            >*/}
            {/*                                <span>{p.aanvoerder}</span>*/}
            {/*                                <span>{formatDatum(p.datum)}</span>*/}
            {/*                                <span><b>€{Number(p.prijs).toFixed(2)}</b></span>*/}
            {/*                            </div>*/}
            {/*                        ))}*/}
            {/*                    </div>*/}

            {/*                    */}{/* Gemiddelde */}
            {/*                    <div style={{ marginTop: 10, paddingTop: 8, borderTop: "1px solid rgba(255,255,255,0.2)" }}>*/}
            {/*                        <b>*/}
            {/*                            Gemiddelde prijs (alle aanbieders): €{avgSoort.toFixed(2)} per {soort}*/}
            {/*                        </b>*/}
            {/*                    </div>*/}
            {/*                </>*/}
            {/*            )}*/}
            {/*        </div>*/}
            {/*    }*/}
            {/*>*/}
            {/*    <button className="Historie1" type="button">*/}
            {/*        Historische prijzen van soort*/}
            {/*    </button>*/}
            {/*</Tippy>*/}

            <Tippy
                animation={"perspective"}
                interactive={true}
                content={
                    <div style={{ minWidth: 320 }}>
                        <h3 style={{ margin: "0 0 6px 0" }}>Verkoopgeschiedenis van deze aanvoerder</h3>
                        <p style={{ margin: "0 0 10px 0", opacity: 0.85 }}>
                            <b>Bloemsoort:</b> {soort}
                            <br />
                            <b>Aanvoerder:</b> {aanvoerder}
                        </p>

                        {prijsGeschiedenis.length === 0 ? (
                            <p style={{ margin: 0 }}>Nog geen verkopen</p>
                        ) : (
                            <>
                                <div style={{ display: "grid", gap: 8, maxHeight: 240, overflowY: "auto" }}>
                                    {prijsGeschiedenis
                                        .slice()
                                        .reverse()
                                        .slice(0, 10) // laatste 10
                                        .map((p, i) => (
                                            <div key={i} style={{ display: "flex", justifyContent: "space-between", gap: 12 }}>
                                                <span>{formatDatum(p.datum)}</span>
                                                <span>
                                                    <b>€{Number(p.prijs).toFixed(2)}</b> per {soort}
                                                </span>
                                            </div>
                                        ))}
                                </div>

                                {/* Gemiddelde */}
                                <div style={{ marginTop: 10, paddingTop: 8, borderTop: "1px solid rgba(255,255,255,0.2)" }}>
                                    <b>
                                        Gemiddelde prijs (alle verkopen):{" "}
                                        €{(
                                            prijsGeschiedenis.reduce((sum, p) => sum + Number(p.prijs), 0) / prijsGeschiedenis.length
                                        ).toFixed(2)}{" "}
                                        per {soort}
                                    </b>
                                </div>
                            </>
                        )}
                    </div>
                }
            >
                <button className="Historie2" type="button" > Historische prijzen van aanvoeder
                </button>
            </Tippy>
            <div className="Adder">
                <input
                    type="number"
                    value={hoeveelheidKopen}
                    onChange={(e) => setHoeveelheidKopen(Number(e.target.value))}
                />
            </div>
            <div >
                <select className="kloklocatie" id="locatie" name="locatie" value={klokLocatie} onChange={(e) => veranderLocatie(e.target.value)} required>
                    <option value="" disabled></option>
                    <option value="aalsmeer">Aalsmeer</option>
                    <option value="eelde">Eelde</option>
                    <option value="naaldwijk">Naaldwijk</option>
                    <option value="rijnsburg">Rijnsburg</option>
                </select><br />
            </div>
        </div>
    );
}
export default VeilingScherm;

{/*            <Tippy animation={'perspective'} interactive={true} content={*/ }
{/*                <div>*/ }
{/*                    <h1>{prijsGeschiedenis}</h1>*/ }
{/*                </div>*/ }
{/*            }>*/ }
{/*                <button*/ }
{/*                    className="Buy"*/ }
{/*                    type="button"*/ }
{/*                    onClick={() => handleBuy()}*/ }
{/*                >*/ }
{/*                    Kopen*/ }
{/*                </button>*/ }
{/*            </Tippy>*/ }

{/*        </div>*/ }
{/*    );*/ }
{/*}*/ }