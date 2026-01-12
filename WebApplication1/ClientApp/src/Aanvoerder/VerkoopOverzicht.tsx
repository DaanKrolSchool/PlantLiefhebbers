import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

type VerkochtProduct = {
    productId: number;
    naam: string;
    verkoopPrijs: number | null;
    verkoopDatum: string | null;
};

function VerkoopOverzicht() {
    const [verkopen, setVerkopen] = useState<VerkochtProduct[]>([]);

    useEffect(() => {
        async function fetchData() {
            const token = localStorage.getItem("token");

            if (!token) {
                console.error("Geen token gevonden. Log eerst in.");
                return;
            }
            // token decoderen
            const decoded = jwtDecode(token) as any;
            const role =
                decoded.role ||
                decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

            // aanvoerder check
            if (role !== "Aanvoerder") {
                console.log("Geen Aanvoerder rol:", role);
                return;
            }
            // verlopen check
            const now = Date.now() / 1000;
            if (decoded.exp && decoded.exp < now) {
                console.log("Token is verlopen");
                return;
            }

            const res = await fetch("https://localhost:7225/Product/eigenverkocht", { headers: { "Authorization": `Bearer ${token}` } });


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
            <h2>Mijn verkochte planten</h2>

            {verkopen.length === 0 && <p>Nog geen verkochte planten.</p>}

            {verkopen.map(p => (
                <div key={p.productId} className="product-kaart">
                    <h3>{p.naam}</h3>
                    <p>Verkoopprijs: {p.verkoopPrijs !== null ? p.verkoopPrijs.toFixed(2) : "-"}</p>
                    <p>Verkoopdatum: {p.verkoopDatum ? new Date(p.verkoopDatum).toLocaleString() : "-"}</p>
                </div>
            ))}
        </div>
    );
}

export default VerkoopOverzicht;
