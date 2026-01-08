import React, { useEffect, useState } from "react";

type Product = {
    productId: number;
    naam: string;
    minimumPrijs: number;
    prijsVerandering: number | null;
    maximumPrijs: number | null;
    klokLocatie: string;
    veilDatum: string;
    positie: number;
};

function AangemeldeProducten() {
    const [products, setProducts] = useState<Product[]>([]);
    const [formValuesByLocatie, setFormValuesByLocatie] = useState<Record<string, Record<number, number>>>({});

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

            const data: Product[] = await res.json();
            setProducts(data);

            const initialFormValues: Record<string, Record<number, number>> = {};
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
        if (da.getTime() !== db.getTime()) return da.getTime() - db.getTime();
        return a.klokLocatie.localeCompare(b.klokLocatie);
    });

    const grouped = sorted.reduce((acc: any, product) => {
        const date = new Date(product.veilDatum).toLocaleDateString("nl-NL", {
            year: "numeric",
            month: "long",
            day: "numeric"
        });

        if (!acc[date]) acc[date] = {};
        const loc = product.klokLocatie;
        if (!acc[date][loc]) acc[date][loc] = [];

        acc[date][loc].push(product);
        acc[date][loc].sort((a: Product, b: Product) => a.positie - b.positie);
        return acc;
    }, {});

    const handleSaveLocatie = async (locatie: string) => {
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
                    body: JSON.stringify({ productId: Number(productId), positie })
                });
            }

            setProducts(prev =>
                prev.map(p =>
                    locFormValues[p.productId] != null
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
            {Object.entries(grouped).map(([date, locaties]: any) => (
                <div key={date} className="datum-sectie">
                    <h2>{date}</h2>

                    {Object.entries(locaties).map(([locatie, items]: any) => (
                        <div key={locatie} className="locatie-sectie">
                            <h3>
                                {locatie}
                                <button
                                    className="beheer-knop"
                                    style={{ marginLeft: '10px' }}
                                    onClick={() => handleSaveLocatie(locatie)}
                                >
                                    Opslaan
                                </button>
                            </h3>

                            <div className="producten-rij">
                                {items.map((p: Product) => (
                                    <div key={p.productId} className="product-kaart">
                                        <h3>{p.naam}</h3>

                                        <p>Veildatum: {new Date(p.veilDatum).toLocaleDateString("nl-NL")}</p>

                                        <p>Minimum Prijs: €{p.minimumPrijs.toFixed(2)}</p>

                                        <p>
                                            Prijs Daling: €
                                            {p.prijsVerandering != null ? p.prijsVerandering.toFixed(2) : "—"}
                                        </p>

                                        <p>
                                            Maximum Prijs: €
                                            {p.maximumPrijs != null ? p.maximumPrijs.toFixed(2) : "—"}
                                        </p>

                                        <p>
                                            Positie:
                                            <input
                                                type="number"
                                                value={formValuesByLocatie[locatie]?.[p.productId] ?? p.positie}
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
