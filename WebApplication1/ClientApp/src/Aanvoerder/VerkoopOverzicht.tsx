import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

type VerkoopRegel = {
    productId: number;
    soortPlant: string;
    aanvoerderNaam: string;
    aantalVerkocht: number;
    prijsPerStuk: number;
    datum: string;
};


function VerkoopOverzicht() {
    const [verkopen, setVerkopen] = useState<VerkoopRegel[]>([]);


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

            {verkopen.map((p, i) => (
                <div key={i} className="product-kaart">
                    <h3>{p.soortPlant}</h3>
                    <p>Aantal verkocht: {p.aantalVerkocht}</p>
                    <p>Prijs per stuk: {p.prijsPerStuk.toFixed(2)}</p>
                    <p>Datum: {new Date(p.datum).toLocaleDateString("nl-NL")}</p>
                </div>
            ))}
        </div>
    );
}

export default VerkoopOverzicht;