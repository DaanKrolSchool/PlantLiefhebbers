import React, {useState} from 'react';

function ProductAanmelden() {
    const [naam, setNaam] = useState("");
    const [soortPlant, setSoortPlant] = useState("");
    const [aantal, setAantal] = useState("");
    const [potMaat, setPotMaat] = useState("");
    const [steelLengte, setSteelLengte] = useState("");
    const [minimumPrijs, setMinimumPrijs] = useState("");
    const [maximumPrijs, setMaximumPrijs] = useState("");
    const [klokLocatie, setKlokLocatie] = useState("");
    const [veilDatum, setVeilDatum] = useState("");
    // const [afbeelding, setAfbeelding] = useState("");
    // const [aanvoerderid, setAanvoerderid] = useState("");
    
    async function productAanmelden(e) {
        e.preventDefault();
        const token = localStorage.getItem("token");
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
                minimumPrijs: parseFloat(minimumPrijs), 
                maximumPrijs: maximumPrijs ? parseFloat(maximumPrijs) : parseFloat(minimumPrijs),
                klokLocatie, 
                veilDatum,
                aanvoerderId: 1 // Hardcoded aanvoerderId (PAS LATER AAN)
            }),
        });

        if(res.ok) alert("Product toegevoegd!");
        else alert("Er ging iets mis: error " + res.status);
    }
    
    return (
        <div class="aanvoerder">
            <form class="aanvoerder" onSubmit={productAanmelden}>
                <h2> Product informatie</h2>
                <label for="naam">Naam:</label>
                <input type="text" id="naam" name="naam" value={naam} onChange={(e) => setNaam(e.target.value)} required/><br/>
                <label for="soort">Soort:</label>
                <input type="text" id="soort" name="soort" value={soortPlant} onChange={(e) => setSoortPlant(e.target.value)} required/><br/>
                <label for="hoeveelheid">Hoeveelheid:</label>
                <input type="number" id="hoeveelheid" min="1" name="hoeveelheid" value={aantal} onChange={(e) => setAantal(e.target.value)} required/><br/>
                <label for="potmaat">Potmaat:</label>
                <input type="number" id="potmaat" min="0" step="1" name="potmaat" value={potMaat} onChange={(e) => setPotMaat(e.target.value)}/><br/>
                <label for="steellengte">Steellengte</label>
                <input type="number" id="steellengte" min="0" step="1" name="steellengte" value={steelLengte} onChange={(e) => setSteelLengte(e.target.value)}/><br/>
                <label for="mprijs">Minimumprijs:</label>
                <input type="number" id="mprijs" min="0.01" step="0.01" name="mprijs" value={minimumPrijs} onChange={(e) => setMinimumPrijs(e.target.value)} required/><br/>
                <label>Maximumprijs:</label>
                <input type="number" min="0.01" step="0.01" value={maximumPrijs} onChange={e => setMaximumPrijs(e.target.value)} /><br />
                <label for="locatie">Kloklocatie:</label>
                <select id="locatie" name="locatie" value={klokLocatie} onChange={(e) => setKlokLocatie(e.target.value)} required>
                    <option value="" disabled></option>
                    <option value="aalsmeer">Aalsmeer</option>
                    <option value="eelde">Eelde</option>
                    <option value="naaldwijk">Naaldwijk</option>
                    <option value="rijnsburg">Rijnsburg</option>
                </select><br/>
                <label for="vdatum">Veildatum:</label>
                <input type="date" id="vdatum" name="vdatum" value={veilDatum} onChange={(e) => setVeilDatum(e.target.value)} required/><br/>
                <label for="afbeelding">Afbeelding:</label>
                <input type="file" id="afbeelding" name="afbeelding"/><br/><br/>
                <input type="submit" value="Product aanmelden"/>
            </form>
        </div>
    );
}

export default ProductAanmelden;
