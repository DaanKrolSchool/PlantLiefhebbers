import React, { useEffect, useState } from "react";

function AangemeldeProducten() {
    const [products, setProducts] = useState([]);
    const [formValues, setFormValues] = useState({});

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
        }
        fetchProducts();
    }, []);

    const upcomingProducts = products.filter(product => {
        const productDate = new Date(product.veilDatum);
        return productDate >= today;
    });

    // SORT BY date + locatie
    const sorted = [...upcomingProducts].sort((a, b) => {
        const da = new Date(a.veilDatum);
        const db = new Date(b.veilDatum);

        if (da - db !== 0) return da - db;

        return a.klokLocatie.localeCompare(b.klokLocatie);
    });

    // GROUP by date -> then by klokLocatie
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
        return acc;
    }, {});

    const handleInput = (id, field, value) => {
        setFormValues(prev => ({
            ...prev,
            [id]: {
                ...prev[id],
                [field]: value
            }
        }));
    };

    return (
        <div className="producten-overzicht">
            {Object.entries(grouped).map(([date, locaties]) => (
                <div key={date} className="datum-sectie">

                    <h2>{date}</h2>

                    {/* Loop per klokLocatie */}
                    {Object.entries(locaties).map(([locatie, items]) => (
                        <div key={locatie} className="locatie-sectie">

                            <h3>{locatie}</h3>

                            <div className="producten-rij">
                                {items.map(p => (
                                    <div key={p.productId} className="product-kaart">
                                        <h3>{p.naam}</h3>
                                        <p>Soort: {p.soortPlant}</p>
                                        <p>Aantal: {p.aantal}</p>
                                        <p>Minimum Prijs: €{p.minimumPrijs.toFixed(2)}</p>

                                        <label>
                                            Maximum Prijs: €
                                            <input
                                                type="number"
                                                value={formValues[p.productId]?.maximumPrijs || ""}
                                                onChange={e =>
                                                    handleInput(p.productId, "maximumPrijs", e.target.value)
                                                }
                                            />
                                        </label>

                                        <label>
                                            Prijs Verandering: €
                                            <input
                                                type="number"
                                                value={formValues[p.productId]?.prijsVerandering || ""}
                                                onChange={e =>
                                                    handleInput(p.productId, "prijsVerandering", e.target.value)
                                                }
                                            />
                                        </label>
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
