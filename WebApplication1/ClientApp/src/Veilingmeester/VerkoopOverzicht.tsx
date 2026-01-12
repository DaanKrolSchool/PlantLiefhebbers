import { useEffect, useState } from "react";

type VerkoopRegel = {
    productId: number;
    soortPlant: string;
    aanvoerderNaam: string;
    aantalVerkocht: number;
    prijsPerStuk: number;
    datum: string;
};


function VerkoopOverzichtVeilingmeester() {
    const [verkopen, setVerkopen] = useState<VerkoopRegel[]>([]);

    useEffect(() => {
        async function fetchData() {
            const token = localStorage.getItem("token");

            const res = await fetch("https://localhost:7225/Product/alleverkocht", { headers: { "Authorization": `Bearer ${token}` } });

            if (!res.ok) {
                console.error("Error:", res.status);
                return;
            }

            const data = await res.json();
            if (!Array.isArray(data)) return;
            setVerkopen(data);
        }

        fetchData();
    }, []);

    return (
        <div className="producten-overzicht">
            <h2>Alle verkochte planten</h2>

            {verkopen.length === 0 && <p>Nog geen verkochte planten.</p>}

            {verkopen.map((p, i) => (
                <div key={i} className="product-kaart">
                    <h3>{p.soortPlant}</h3>
                    <p>Aantal verkocht: {p.aantalVerkocht}</p>
                    <p>Prijs per stuk: {p.prijsPerStuk.toFixed(2)}</p>
                    <p>Datum: {new Date(p.datum).toLocaleDateString("nl-NL")}</p>
                    <p style={{ opacity: 0.7 }}>Naam klant: {p.aanvoerderNaam}</p>
                </div>
            ))}
        </div>
    );
}

export default VerkoopOverzichtVeilingmeester;