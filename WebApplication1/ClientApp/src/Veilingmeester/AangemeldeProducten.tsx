
import { useState, useEffect } from 'react';


type Product = {
    productId: number;
    naam: string;
    soortPlant: string;
    aantal: number;
    minimumPrijs: number;
    prijsVerandering: number;
    maximumPrijs: number;
    klokLocatie: string;
    veilDatum: string;
};

type EditValues = {
    prijsVerandering: number;
    maximumPrijs: number;
};


function AangemeldeProducten() {

    const [editMode, setEditMode] = useState<number | null>(null);
    const [editValues, setEditValues] = useState<EditValues>({ prijsVerandering: 0, maximumPrijs: 0 });
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

        const token = localStorage.getItem("token");
        if (!token) {
            alert("Je bent niet ingelogd of token ontbreekt!");
            return;
        }

        const res = await fetch(`https://localhost:7225/Product/veilingMeester/${productId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                productId,
                prijsVerandering: editValues.prijsVerandering,
                maximumPrijs: editValues.maximumPrijs,
            }),
        });

        if (res.ok) {
            setEditMode(null);
            const res2 = await fetch("https://localhost:7225/Product/datum", {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            const data: Product[] = await res2.json();
            setProducts(data);
        } else {
            alert("Error: " + res.status);
        }
    };

    const deleteProduct = async (productId: number) => {
        if (!window.confirm("Weet je zeker dat je dit product wilt verwijderen?")) return;

        const token = localStorage.getItem("token");
        if (!token) {
            alert("Je bent niet ingelogd of token ontbreekt!");
            return;
        }


        const res = await fetch(`https://localhost:7225/Product/${productId}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
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

                                {editMode === p.productId ? (
                                    <div className="edit-container">
                                        <h3>{p.naam}</h3>

                                        <p>Soort: {p.soortPlant}</p>

                                        <p>Aantal: {p.aantal}</p>

                                        <p>Min. Prijs: {p.minimumPrijs}</p>

                                        <div className="edit-row">
                                            <label>Prijs Daling:</label>
                                            <input
                                                type="number"
                                                value={editValues.prijsVerandering}
                                                onChange={e => setEditValues({ ...editValues, prijsVerandering: Number(e.target.value) })}
                                            />
                                        </div>

                                        <div className="edit-row">
                                            <label>Max. Prijs:</label>
                                            <input
                                                type="number"
                                                value={editValues.maximumPrijs}
                                                onChange={e => setEditValues({ ...editValues, maximumPrijs: Number(e.target.value) })}
                                            />
                                        </div>

                                        <p>Locatie: {p.klokLocatie}</p>

                                        <div className="edit-buttons">
                                            <button className="beheer-knop" onClick={() => saveChanges(p.productId)}> Opslaan</button>

                                            <button className="beheer-knop" onClick={() => setEditMode(null)}> Annuleren </button>
                                        </div>
                                    </div>
                                ) : (
                                    <>
                                        <h3>{p.naam}</h3>
                                        <p>Soort: {p.soortPlant}</p>
                                        <p>Aantal: {p.aantal}</p>
                                        <p>Min. Prijs: €{p.minimumPrijs.toFixed(2)}</p>
                                        <p>Prijs Daling: €{p.prijsVerandering.toFixed(2)}</p>
                                        <p>Max. Prijs: €{p.maximumPrijs.toFixed(2)}</p>
                                        <p>Locatie: {p.klokLocatie}</p>
                                        <div className="edit-buttons">
                                            <button
                                                style={{ backgroundColor: "#D0B070", color: "black" }}
                                                onClick={() => {
                                                    setEditMode(p.productId);
                                                    setEditValues({
                                                        prijsVerandering: p.prijsVerandering,
                                                        maximumPrijs: p.maximumPrijs,
                                                    });
                                                }}
                                            >
                                                Wijzigen
                                            </button>

                                            <button
                                                style={{ backgroundColor: "#D0B070", color: "black" }}
                                                onClick={() => deleteProduct(p.productId)}
                                            >
                                                Verwijderen
                                            </button>
                                        </div>
                                    </>
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
