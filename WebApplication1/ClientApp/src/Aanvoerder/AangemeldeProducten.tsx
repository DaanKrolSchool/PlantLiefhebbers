
import { useState, useEffect } from 'react';


type Product = {
    productId: number;
    naam: string;
    soortPlant: string;
    aantal: number;
    minimumPrijs: number;
    klokLocatie: string;
    veilDatum: string;
};

type EditValues = {
    naam: string;
    soortPlant: string;
    aantal: number;
    minimumPrijs: number;
};


function AangemeldeProducten() {

    const [editMode, setEditMode] = useState<number | null>(null);
    const [editValues, setEditValues] = useState<EditValues>({ naam: "", soortPlant: "", aantal: 0, minimumPrijs: 0 });
    const [products, setProducts] = useState<Product[]>([]);

    const today = new Date();
    const upcomingProducts = products.filter(product => new Date(product.veilDatum) >= today);


    useEffect(() => {
        async function fetchProducts() {
            const token = localStorage.getItem("token");
            const res = await fetch(`https://localhost:7225/Product/datum`, {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            if (!res.ok) {
                console.error("Error:", res.status);
                return;
            }
            const data = await res.json();
            setProducts(data);
        }
        fetchProducts();
    }, []);

    const saveChanges = async (productId: number) => {
        const res = await fetch(`https://localhost:7225/Product/${productId}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                productId,
                naam: editValues.naam,
                soortPlant: editValues.soortPlant,
                aantal: editValues.aantal,
                minimumPrijs: editValues.minimumPrijs,
            }),
        });

        if (res.ok) {
            setEditMode(null);
            const res2 = await fetch("https://localhost:7225/Product/datum");
            const data: Product[] = await res2.json();
            setProducts(data);
        } else {
            alert("Error: " + res.status);
        }
    };

    const deleteProduct = async (productId: number) => {
        if (!window.confirm("Weet je zeker dat je dit product wilt verwijderen?")) return;

        const res = await fetch(`https://localhost:7225/Product/${productId}`, {
            method: "DELETE",
        });

        if (res.ok) {
            setProducts(products.filter(p => p.productId !== productId));
            alert("Product verwijderd!");
        } else {
            alert("Error: " + res.status);
        }
    };


    const groupedByDate = upcomingProducts.reduce((groups: Record<string, Product[]>, product) => {
        const date = new Date(product.veilDatum).toLocaleDateString("nl-NL", {
            year: "numeric",
            month: "long",
            day: "numeric",
        });
        if (!groups[date]) groups[date] = [];
        groups[date].push(product);
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
                                <h3>{p.naam}</h3>
                                <p>Soort: {p.soortPlant}</p>
                                <p>Aantal: {p.aantal}</p>
                                <p>Minimum Prijs: €{p.minimumPrijs.toFixed(2)}</p>
                                <p>Locatie: {p.klokLocatie}</p>

                                {editMode === p.productId ? (
                                    <>
                                        <input
                                            value={editValues.naam}
                                            onChange={e => setEditValues({ ...editValues, naam: e.target.value })}
                                        />
                                        <input
                                            value={editValues.soortPlant}
                                            onChange={e => setEditValues({ ...editValues, soortPlant: e.target.value })}
                                        />
                                        <input
                                            type="number"
                                            value={editValues.aantal}
                                            onChange={e => setEditValues({ ...editValues, aantal: Number(e.target.value) })}
                                        />
                                        <input
                                            type="number"
                                            value={editValues.minimumPrijs}
                                            onChange={e => setEditValues({ ...editValues, minimumPrijs: Number(e.target.value) })}
                                        />
                                        <button style={{ backgroundColor: "#D0B070", color: "black" }}
                                                onClick={() => saveChanges(p.productId)}>Save</button>
                                    </>
                                ) : (
                                    <div style={{ display: "flex", gap: "8px" }}>
                                        <button
                                            style={{ backgroundColor: "#D0B070", color: "black" }}
                                            onClick={() => {
                                                setEditMode(p.productId);
                                                setEditValues({
                                                    naam: p.naam,
                                                    soortPlant: p.soortPlant,
                                                    aantal: p.aantal,
                                                    minimumPrijs: p.minimumPrijs,
                                                });
                                            }}
                                        >
                                            Edit
                                        </button>
                                        <button
                                            style={{ backgroundColor: "#D0B070", color: "black" }}
                                            onClick={() => deleteProduct(p.productId)}
                                        >
                                            Delete
                                        </button>
                                    </div>
                                )}
                            </div>
                        ))}
                    </div>
                </div>
            ))}
        </div>
    );
}

export default AangemeldeProducten;
