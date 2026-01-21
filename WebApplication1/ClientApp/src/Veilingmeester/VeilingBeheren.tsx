import React, { useEffect, useState } from "react";

type Product = {
    productId: number;
    naam: string;
    aantal: number;
    minimumPrijs: number;
    prijsVerandering: number | null;
    maximumPrijs: number | null;
    klokLocatie: string;
    veilDatum: string;
    positie: number;
    veilTijd: string | null;
};

function AangemeldeProducten() {
    const [products, setProducts] = useState<Product[]>([]);
    const [veilTijdByGroup, setVeilTijdByGroup] = useState<Record<string, string>>({});
    const [formValuesByLocatie, setFormValuesByLocatie] = useState<Record<string, Record<number, number>>>({});

    const today = new Date().toISOString().split('T')[0];

    useEffect(() => {
        async function fetchProducts() {
            const token = localStorage.getItem("token");

            const res = await fetch(`/Product/veilingmeester/most`, {
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

    const sorted = [...products].sort((a, b) => {
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

    const handleSaveLocatie = async (locatie: string, items: Product[], date: string) => {
        const token = localStorage.getItem("token");

        const selectedVeilTijd = veilTijdByGroup[`${date}_${locatie}`];

        let timeSpan: string | null = null;

        if (selectedVeilTijd && /^\d{2}:\d{2}$/.test(selectedVeilTijd)) {
            const [hours, minutes] = selectedVeilTijd.split(":").map(Number);
            timeSpan = `${hours.toString().padStart(2, "0")}:${minutes
                .toString()
                .padStart(2, "0")}:00`;
        } else {
            timeSpan = null;
        }

        try {
            await Promise.all(
                items.map(p => {
                    const newPositie =
                        formValuesByLocatie[locatie]?.[p.productId] ?? p.positie;

                    return fetch(`/Product/positie/${p.productId}`, {
                        method: "PUT",
                        headers: {
                            "Authorization": `Bearer ${token}`,
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({ productId: p.productId, positie: newPositie, veilTijd: timeSpan })
                    });
                })
            );

            /*await Promise.all(
                items.map(p =>
                    fetch(`https://localhost:7225/Product/veilingMeester/${p.productId}`, {
                        method: "PUT",
                        headers: {
                            "Authorization": `Bearer ${token}`,
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({
                            productId: p.productId,
                            veilTijd: timeSpan
                        })
                    })
                )
            );*/
            const res = await fetch("/Product/veilingmeester/most", {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            const data: Product[] = await res.json();
            setProducts(data);
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div className="producten-overzicht">
            {Object.entries(grouped).map(([date, locaties]: any) => (
                <div key={date} className="datum-sectie">
                    <h2>{date}</h2>

                    {Object.entries(locaties).map(([locatie, items]: any) => {
                        const firstProduct = items[0];
                        const defaultVeilTijd =
                            firstProduct.veilTijd ? firstProduct.veilTijd.substring(0, 5) : "";

                        return (
                            <div key={locatie} className="locatie-sectie">
                                <h3>
                                    {locatie}
                                    <input
                                        type="time"
                                        value={veilTijdByGroup[`${date}_${locatie}`] || defaultVeilTijd}                                        onChange={e =>
                                            setVeilTijdByGroup(prev => ({
                                                ...prev,
                                                [`${date}_${locatie}`]: e.target.value
                                            }))
                                        }
                                        style={{ marginLeft: '10px' }}
                                    />
                                    <button
                                        className="beheer-knop"
                                        style={{ marginLeft: '10px' }}
                                        onClick={() => handleSaveLocatie(locatie, items, date)}
                                    >
                                        Opslaan
                                    </button>
                                </h3>

                                <div className="producten-rij">
                                    {items.map((p: Product) => (
                                        <div key={p.productId} className="product-kaart">
                                            <h3>{p.naam}</h3>

                                            <p>Aantal: {p.aantal}</p>

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
                        );
                    })}
                </div>
            ))}
        </div>
    );
}

export default AangemeldeProducten;
