import React, {useState} from 'react';
import { useEffect } from "react";


function ProductAanmelden() {

    const [naam, setNaam] = useState("");
    const [soortPlant, setSoortPlant] = useState("");
    const [aantal, setAantal] = useState("");
    const [potMaat, setPotMaat] = useState("");
    const [steelLengte, setSteelLengte] = useState("");
    const [minimumPrijs, setMinimumPrijs] = useState("");
    //const [maximumPrijs, setMaximumPrijs] = useState("");
    const [klokLocatie, setKlokLocatie] = useState("");
    const [veilDatum, setVeilDatum] = useState("");
    const [makkelijkheid, setmakkelijkheid] = useState("");
    const [seizoensplant, setseizoensplant] = useState("");
    const [temperatuur, settemperatuur] = useState("");
    const [water, setwater] = useState("");
    const [leeftijd, setleeftijd] = useState("");
    const [bedrijfnaam, setBedrijfnaam] = useState("");
    const [afbeelding, setAfbeelding] = useState<File | null>(null);


    const [error, setError] = useState("");
    const [notf, setNotf] = useState("");
    // product aanmelden functie
    async function productAanmelden(e: React.FormEvent) {
        e.preventDefault(); // voorkom herladen van de pagina
        const token = localStorage.getItem("token");
        // post request naar de api 
        const res = await fetch(`https://localhost:7225/Product`, {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ 
                naam, 
                soortPlant, 
                aantal: parseInt(aantal), 
                potMaat: potMaat ? parseInt(potMaat) : null, 
                steelLengte: steelLengte ? parseInt(steelLengte) : null,
                makkelijkheid: makkelijkheid ? parseInt(makkelijkheid) : null,
                temperatuur: temperatuur ? parseInt(temperatuur) : null,
                water: water ? parseInt(water) : null,
                leeftijd: leeftijd ? parseInt(leeftijd) : null,
                seizoensplant: seizoensplant ? seizoensplant : null,

                minimumPrijs: parseFloat(minimumPrijs),
                klokLocatie, 
                veilDatum,
                aanvoerderNaam: bedrijfnaam
            }),
        });
        
        if (res.ok) {
            setNotf("Product toegevoegd!")
            setTimeout(() => setNotf!(""), 2500)
        } else {
            setError("Er ging iets mis: error " + res.status)
            setTimeout(() => setError!(""), 2500)
        }

        const product = await res.json();

        if (afbeelding) {
            const formData = new FormData();
            formData.append("image", afbeelding);
            formData.append("productId", product.productId);

            const imgRes = await fetch(`https://localhost:7225/Product/UploadImage`, {
                method: "POST",
                headers: {
                    "Authorization": `Bearer ${token}`
                },
                body: formData
            });

            if (!imgRes.ok) {
                setError("Image upload failed: " + imgRes.status);
                setTimeout(() => setError(""), 2500);
            }
        }

        setNotf("Product toegevoegd!");
        setTimeout(() => setNotf(""), 2500);
    }
    
    return (
        <div className="aanvoerder">
            <form className="aanvoerder" onSubmit={productAanmelden}>
                <h2> Product informatie</h2>
                { }
                <label htmlFor="bedrijfnaam">Bedrijfnaam:</label>
                <input type="text" id="bedrijfnaam" name="bedrijfnaam" value={bedrijfnaam} onChange={(e) => setBedrijfnaam(e.target.value)} required/><br/>
                <label htmlFor="naam">Naam:</label>
                <input type="text" id="naam" name="naam" value={naam} onChange={(e) => setNaam(e.target.value)} required/><br/>
                <label htmlFor="soort">Soort:</label>
                <input type="text" id="soort" name="soort" value={soortPlant} onChange={(e) => setSoortPlant(e.target.value)} required/><br/>
                <label htmlFor="hoeveelheid">Hoeveelheid:</label>
                <input type="number" id="hoeveelheid" min="1" name="hoeveelheid" value={aantal} onChange={(e) => setAantal(e.target.value)} required/><br/>
                <label htmlFor="potmaat">Potmaat:</label>
                <input type="number" id="potmaat" min="0" step="1" name="potmaat" value={potMaat} onChange={(e) => setPotMaat(e.target.value)}/><br/>
                <label htmlFor="steellengte">Steellengte</label>
                <input type="number" id="steellengte" min="0" step="1" name="steellengte" value={steelLengte} onChange={(e) => setSteelLengte(e.target.value)} /><br />
                <label htmlFor="seizoen">Seizoen</label>
                <select value={seizoensplant} onChange={(e) => setseizoensplant(e.target.value)} required>
                    <option value="" disabled></option>
                    <option value="niet van toepassing">Niet van toepassing</option>
                    <option value="winter">Winter</option>
                    <option value="lente">Lente</option>
                    <option value="zomer">Zomer</option>
                    <option value="herfst">Herfst</option>
                </select><br />
                <label htmlFor="temperatuur">Temperatuur</label>
                <input type="number" value={temperatuur} onChange={(e) => settemperatuur(e.target.value)} />
                <label htmlFor="water">Liter water per week</label>
                <input type="number" value={water} onChange={(e) => setwater(e.target.value)} />
                <label htmlFor="leeftijd">Leeftijd van de plant in maanden</label>
                <input type="number" value={leeftijd} onChange={(e) => setleeftijd(e.target.value)} />
                <label htmlFor="makkelijkheid">Makkelijkheid houdbaar houden van de plant (1/10)</label>
                <input type="number" value={makkelijkheid} onChange={(e) => setmakkelijkheid(e.target.value)} />
                <label htmlFor="mprijs">Minimumprijs:</label>
                <input type="number" id="mprijs" min="0.01" step="0.01" name="mprijs" value={minimumPrijs} onChange={(e) => setMinimumPrijs(e.target.value)} required/><br/>
                <label htmlFor="locatie">Kloklocatie:</label>
                <select id="locatie" name="locatie" value={klokLocatie} onChange={(e) => setKlokLocatie(e.target.value)} required>
                    <option value="" disabled></option>
                    <option value="aalsmeer">Aalsmeer</option>
                    <option value="eelde">Eelde</option>
                    <option value="naaldwijk">Naaldwijk</option>
                    <option value="rijnsburg">Rijnsburg</option>
                </select><br />

                {error && <div className="ErrorBox">{error}</div>}
                {notf && <div className="NotBox">{notf}</div>}

                <label htmlFor="vdatum">Veildatum:</label>
                <input type="datetime-local" id="vdatum" name="vdatum" value={veilDatum} onChange={(e) => setVeilDatum(e.target.value)} required/><br/>
                <label htmlFor="afbeelding">Afbeelding:</label>
                <input type="file" accept="image/png" id="afbeelding" name="afbeelding" onChange={(e) => setAfbeelding(e.target.files ? e.target.files[0] : null)}/><br/><br/>
                <input type="submit" value="Product aanmelden"/>
            </form>
        </div>
    );
}

export default ProductAanmelden;
