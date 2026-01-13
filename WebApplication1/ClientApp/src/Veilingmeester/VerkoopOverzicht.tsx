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

    const groupedByDate = verkopen
        .sort((a, b) => new Date(a.datum).getTime() - new Date(b.datum).getTime())
        .reduce((groups: Record<string, VerkoopRegel[]>, p) => {
            const date = new Date(p.datum).toLocaleDateString("nl-NL", {
                year: "numeric",
                month: "long",
                day: "numeric",
            });

            if (!groups[date]) groups[date] = [];
            groups[date].push(p);
            return groups;
        }, {});

    return (
        <div className="producten-overzicht">
            {Object.entries(groupedByDate).map(([date, items]) => (
                <div key={date} className="datum-sectie">
                    <h2>{date}</h2>
                    <div className="producten-rij">
                        {items.map(p => (
                            <div key={p.productId} className="product-kaart">
                                <h3>{p.soortPlant}</h3>
                                <p>Aantal verkocht: {p.aantalVerkocht}</p>
                                <p>Prijs per stuk: {p.prijsPerStuk.toFixed(2)}</p>
                                <p style={{ opacity: 0.7 }}>Naam klant: {p.aanvoerderNaam}</p>
                            </div>
                        ))}
                    </div>
                </div>
            ))}
        </div>
    );
}

export default VerkoopOverzichtVeilingmeester;