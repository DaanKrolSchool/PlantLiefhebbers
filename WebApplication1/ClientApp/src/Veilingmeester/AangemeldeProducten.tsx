import { useState, useEffect } from 'react';

//de teypes aangeven van alle variabele
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
    //de getters en setters
    const [editMode, setEditMode] = useState<number | null>(null);
    const [editValues, setEditValues] = useState<EditValues>({ prijsVerandering: 0, maximumPrijs: 0 });
    const [products, setProducts] = useState<Product[]>([]);
    //constante
    //de dag van vandaag
    const today = new Date();
    //deze kijkt naar welke producten nog na vandaag komen.
    const upcomingProducts = products.filter(product => new Date(product.veilDatum) >= today);

    //Deze gebruikt productController om zo alle producten te krijgen.
    useEffect(() => {
        async function fetchProducts() {
            const token = localStorage.getItem("token");
            //de locatie waar hij te vinden is binnen de controller
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
        //hier checkt hij of je wel recht hebt om de producten te bekijken
        const token = localStorage.getItem("token");
        if (!token) {
            alert("Je bent niet ingelogd of token ontbreekt!");
            return;
        }
        //dit is nodig zodat een veiling meester de prijs kan aanpassen, begin en max prijs
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

        //hier word er gekeken naar welke datum hoort bij welk product en zet hem in een volgorde
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

    //Dit is nodig om een product te kunnen verwijderen
    const deleteProduct = async (productId: number) => {
        //wanneer er word geklikt op dat je zeker bent het product te willen verwijderen
        if (!window.confirm("Weet je zeker dat je dit product wilt verwijderen?")) return;

        const token = localStorage.getItem("token");
        //voor wanneer je het recht niet hebt om dit te doen
        if (!token) {
            alert("Je bent niet ingelogd of token ontbreekt!");
            return;
        }

        //om een product te kunnen verwijderen, komt ook uit de product controller
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

    //je gebruikt waar de producten zijn gesorteerd op datum dan geef je de aankomende producten
    const groupedByDate = upcomingProducts.reduce((groups: Record<string, Product[]>, product) => {
        const date = new Date(product.veilDatum).toLocaleDateString("nl-NL", {
            //het aangeven welk deel van de datum waarbij hoort
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

