import { useEffect, useState } from "react";

type VerkochtProduct = {
    productId: number;
    naam: string;
    verkoopPrijs: number;
    verkoopDatum: string;
};

function VerkoopOverzichtVeilingmeester() {
    const [verkopen, setVerkopen] = useState<VerkochtProduct[]>([]);

    useEffect(() => {
        async function fetchData() {
            const token = localStorage.getItem("token");

            const res = await fetch("https://localhost:7225/Product/verkocht", {
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            });

            if (!res.ok) {
                console.error("Error:", res.status);
                return;
            }

            const data = await res.json();
            setVerkopen(data);
        }

        fetchData();
    }, []);

    return (
        <div className="producten-overzicht">
            <h2>Alle verkochte planten</h2>

            {verkopen.length === 0 && <p>Nog geen verkochte planten.</p>}

            {verkopen.map(p => (
                <div key={p.productId} className="product-kaart">
                    <h3>{p.naam}</h3>
                    <p>Verkoopprijs: {p.verkoopPrijs.toFixed(2)}</p>
                    <p>Verkoopdatum: {new Date(p.verkoopDatum).toLocaleString()}</p>
                </div>
            ))}
        </div>
    );
}

export default VerkoopOverzichtVeilingmeester;
