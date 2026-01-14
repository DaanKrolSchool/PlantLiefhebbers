import { useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

type VerkoopRegel = {
    productId: number;
    soortPlant: string;
    klantNaam: string;
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

            const res = await fetch("/Product/verkocht", { headers: { "Authorization": `Bearer ${token}` } });


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
                                <p style={{ opacity: 0.7 }}>Naam klant: {p.klantNaam}</p>
                            </div>
                        ))}
                    </div>
                </div>
            ))}
        </div>
    );
}

export default VerkoopOverzicht;