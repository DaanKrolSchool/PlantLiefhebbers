import React, {useState} from 'react';

function ProductAanmelden() {
    const [Naam, setNaam] = useState("");
    const [soort, setSoort] = useState("");
    const [hoeveelheid, setHoeveelheid] = useState("");
    const [potmaat, setPotmaat] = useState("");
    const [steellengte, setSteellengte] = useState("");
    const [mprijs, setMprijs] = useState("");
    const [locatie, setLocatie] = useState("");
    const [vdatum, setVdatum] = useState("");
    // const [afbeelding, setAfbeelding] = useState("");
    // const [aanvoerderid, setAanvoerderid] = useState("");
    
    async function productAanmelden(e) {
        e.preventDefault();
        
        const res = await fetch(`https://localhost:7225/Product`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ 
                Naam, 
                soortPlant: soort, 
                aantal: hoeveelheid, 
                potMaat: potmaat ? parseInt(potmaat) : null, 
                steelLengte: steellengte ? parseInt(steellengte) : null, 
                minimumPrijs: parseFloat(mprijs), 
                klokLocatie: locatie, 
                VeilDatum: vdatum 
            }),
        });

        if(res.ok) alert("Product toegevoegd!");
        else alert("Er ging iets mis: error " + res.status);
    }
    
    return (
        <div class="aanvoerder">
            <form class="aanvoerder" onSubmit={productAanmelden}>
                <h2> Product informatie</h2>
                <label for="Naam">Naam:</label>
                <input type="text" id="Naam" name="Naam" value={Naam} onChange={(e) => setNaam(e.target.value)} required/><br/>
                <label for="soort">Soort:</label>
                <input type="text" id="soort" name="soort" value={soort} onChange={(e) => setSoort(e.target.value)} required/><br/>
                <label for="hoeveelheid">Hoeveelheid:</label>
                <input type="number" id="hoeveelheid" min="1" name="hoeveelheid" value={hoeveelheid} onChange={(e) => setHoeveelheid(e.target.value)} required/><br/>
                <label for="potmaat">Potmaat:</label>
                <input type="number" id="potmaat" min="0" step="1" name="potmaat" value={potmaat} onChange={(e) => setPotmaat(e.target.value)}/><br/>
                <label for="steellengte">Steellengte</label>
                <input type="number" id="steellengte" min="0" step="1" name="steellengte" value={steellengte} onChange={(e) => setSteellengte(e.target.value)}/><br/>
                <label for="mprijs">Minimumprijs:</label>
                <input type="number" id="mprijs" min="0.01" step="0.01" name="mprijs" value={mprijs} onChange={(e) => setMprijs(e.target.value)} required/><br/>
                <label for="locatie">Kloklocatie:</label>
                <select id="locatie" name="locatie" value={locatie} onChange={(e) => setLocatie(e.target.value)} required>
                    <option value="" disabled></option>
                    <option value="aalsmeer">Aalsmeer</option>
                    <option value="eelde">Eelde</option>
                    <option value="naaldwijk">Naaldwijk</option>
                    <option value="rijnsburg">Rijnsburg</option>
                </select><br/>
                <label for="vdatum">VeilDatum:</label>
                <input type="date" id="vdatum" name="vdatum" value={vdatum} onChange={(e) => setVdatum(e.target.value)} required/><br/>
                <label for="afbeelding">Afbeelding:</label>
                <input type="file" id="afbeelding" name="afbeelding"/><br/><br/>
                <input type="submit" value="Product aanmelden"/>
            </form>
        </div>
    );
}

export default ProductAanmelden;
