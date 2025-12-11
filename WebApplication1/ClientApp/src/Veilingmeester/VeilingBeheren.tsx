import React, { useEffect, useState } from "react";

function AangemeldeProducten() {
    const [products, setProducts] = useState([]);
    const [formValuesByLocatie, setFormValuesByLocatie] = useState({}); // state per locatie

    const today = new Date();

    useEffect(() => {
        async function fetchProducts() {
            const token = localStorage.getItem("token");
            const res = await fetch(`https://localhost:7225/Product/datum`, {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            const data = await res.json();
            setProducts(data);

            // Init formValuesByLocatie
            const initialFormValues = {};
            data.forEach(p => {
                if (!initialFormValues[p.klokLocatie]) initialFormValues[p.klokLocatie] = {};
                initialFormValues[p.klokLocatie][p.productId] = p.positie;
            });
            setFormValuesByLocatie(initialFormValues);
        }
        fetchProducts();
    }, []);

    const upcomingProducts = products.filter(product => {
        const productDate = new Date(product.veilDatum);
        return productDate >= today;
    });

    const sorted = [...upcomingProducts].sort((a, b) => {
        const da = new Date(a.veilDatum);
        const db = new Date(b.veilDatum);

        if (da - db !== 0) return da - db;

        return a.klokLocatie.localeCompare(b.klokLocatie);
    });

    const grouped = sorted.reduce((acc, product) => {
        const date = new Date(product.veilDatum).toLocaleDateString("nl-NL", {
            year: "numeric",
            month: "long",
            day: "numeric"
        });

        if (!acc[date]) acc[date] = {};
        const loc = product.klokLocatie;
        if (!acc[date][loc]) acc[date][loc] = [];
        acc[date][loc].push(product);
        acc[date][loc].sort((a, b) => a.positie - b.positie);
        return acc;
    }, {});

    const handleSaveLocatie = async (locatie, items) => {
        const token = localStorage.getItem("token");
        const locFormValues = formValuesByLocatie[locatie];

        try {
            for (const [productId, positie] of Object.entries(locFormValues)) {
                await fetch(`https://localhost:7225/Product/positie/${productId}`, {
                    method: "PUT",
                    headers: {
                        "Authorization": `Bearer ${token}`,
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ productId, positie })
                });
            }

            // update lokale state
            setProducts(prev =>
                prev.map(p => locFormValues[p.productId] != null
                    ? { ...p, positie: locFormValues[p.productId] }
                    : p
                )
            );

        } catch (error) {
            console.error(error);
            alert("Kon posities niet bijwerken");
        }
    };

    return (
        <div className="producten-overzicht">
            {Object.entries(grouped).map(([date, locaties]) => (
                <div key={date} className="datum-sectie">
                    <h2>{date}</h2>

                    {Object.entries(locaties).map(([locatie, items]) => (
                        <div key={locatie} className="locatie-sectie">
                            <h3>
                                {locatie}
                                <button className="beheer-knop" style={{ marginLeft: '10px' }}  onClick={() => handleSaveLocatie(locatie, items)}>
                                    Opslaan
                                </button>
                            </h3>

                            <div className="producten-rij">
                                {items.map(p => (
                                    <div key={p.productId} className="product-kaart">
                                        <h3>{p.naam}</h3>
                                        <p>Minimum Prijs: €{p.minimumPrijs.toFixed(2)}</p>
                                        <p>Prijs Daling: €{p.prijsVerandering.toFixed(2)}</p>
                                        <p>Maximum Prijs: €{p.maximumPrijs.toFixed(2)}</p>
                                        <p>
                                            Positie:
                                            <input
                                                type="number"
                                                value={formValuesByLocatie[locatie][p.productId]}
                                                onChange={e =>
                                                    setFormValuesByLocatie(prev => ({
                                                        ...prev,
                                                        [locatie]: {
                                                            ...prev[locatie],
                                                            [p.productId]: Number(e.target.value)
                                                        }
                                                    }))
                                                }
                                                style={{ width: '50px', marginLeft: '5px' }}
                                            />
                                        </p>
                                    </div>
                                ))}
                            </div>
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
}

export default AangemeldeProducten;
