
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

    const [error, setError] = useState("");
    const [notf, setNotf] = useState("");

    // dit haalt alle producten op wanneer de component laadt
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

    // dit slaat de wijzigingen op bij een product dat gewijzigd wordt
    const saveChanges = async (productId: number) => {
        const token = localStorage.getItem("token");
        if (!token) {
            setError("Je bent niet ingelogd of token ontbreekt!");
            setTimeout(() => setError!(""), 2500)
            return;
        }
        // put request om product te updaten
        const res = await fetch(`https://localhost:7225/Product/aanvoerder/${productId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                productId,
                naam: editValues.naam,
                soortPlant: editValues.soortPlant,
                aantal: editValues.aantal,
                minimumPrijs: editValues.minimumPrijs,
            }),
        });

        if (res.ok) {
            setEditMode(null); // stop de edit mode
            // vernieuw de productenlijst
            const res2 = await fetch("https://localhost:7225/Product/datum", {
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
            const data: Product[] = await res2.json();
            setProducts(data);
        } else {
            setError("Error: " + res.status);
            setTimeout(() => setError!(""), 2500)
        }
    };
    // dit verwijdert een product na bevestiging
    const deleteProduct = async (productId: number) => {
        if (!window.confirm("Weet je zeker dat je dit product wilt verwijderen?")) return;

        const token = localStorage.getItem("token");
        if (!token) {
            setError("Je bent niet ingelogd of token ontbreekt!");
            setTimeout(() => setError!(""), 2500)
            return;
        }

        // delete request om product te verwijderen
        const res = await fetch(`https://localhost:7225/Product/${productId}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        });

        if (res.ok) {
            // verwijder het product lokaal uit de lijst
            setProducts(products.filter(p => p.productId !== productId));
            // melding dat het product is verwijderd
            setNotf("Product verwijderd!");
            setTimeout(() => setNotf!(""), 2500)
        } else {
            // foutmelding 
            setError("Error: " + res.status);
            
            setTimeout(() => setError!(""), 2500)
        }
    };

    // groepeer producten op datum
    const groupedByDate = upcomingProducts.reduce((groups: Record<string, Product[]>, product) => {
        // format datum naar lokale Nederlandse weergave
        const date = new Date(product.veilDatum).toLocaleDateString("nl-NL", {
            year: "numeric",
            month: "long",
            day: "numeric",
        });
        // array aanmaken voor de datum als die nog niet bestaat
        if (!groups[date]) groups[date] = [];
        // product toevoegen aan de juiste datumgroep
        groups[date].push(product);
        return groups;
    }, {});

    return (
        <div className="producten-overzicht">
            {/* loop door alle datumgroepen heen */}
            {Object.entries(groupedByDate).map(([date, items]) => (
                <div key={date} className="datum-sectie">
                    <h2>{date}</h2>
                    <div className="producten-rij">
                        {/* voor elk product binnen deze datum */ }
                        {items.map(p => (
                            <div key={p.productId} className="product-kaart">
                                {/* als de edit modus aanstaat */}
                                {editMode === p.productId ? (
                                    <div className="edit-container">
                                        {/* naam bewerken */}
                                        <div className="edit-row-naam">
                                            <input
                                                value={editValues.naam}
                                                onChange={e => setEditValues({ ...editValues, naam: e.target.value })}
                                            />
                                        </div>
                                        {/* soort bewerken */}
                                        <div className="edit-row">
                                            <label>Soort: </label>
                                            <input
                                                value={editValues.soortPlant}
                                                onChange={e => setEditValues({ ...editValues, soortPlant: e.target.value })}
                                            />
                                        </div>
                                        {/* aantal bewerken */}
                                        <div className="edit-row">
                                            <label>Aantal:</label>
                                            <input
                                                type="number"
                                                value={editValues.aantal}
                                                onChange={e => setEditValues({ ...editValues, aantal: Number(e.target.value) })}
                                            />
                                        </div>
                                        {/* minimum prijs bewerken */}
                                        <div className="edit-row">
                                            <label>Min. Prijs:</label>
                                            <input
                                                type="number"
                                                value={editValues.minimumPrijs}
                                                onChange={e => setEditValues({ ...editValues, minimumPrijs: Number(e.target.value) })}
                                            />
                                        </div>
                                        {/* deze kan je niet editen */}
                                        <p>Locatie: {p.klokLocatie}</p>

                                        {/* opslaan en annuleer knoppen */}
                                        <div className="edit-buttons">
                                            <button className="beheer-knop" onClick={() => saveChanges(p.productId)}> Opslaan </button>

                                            <button className="beheer-knop" onClick={() => setEditMode(null)}> Annuleren </button>
                                        </div>
                                    </div>
                                ) : (
                                        // normale weergave van het product
                                    <>
                                        <h3>{p.naam}</h3>
                                        <p>Soort: {p.soortPlant}</p>
                                        <p>Aantal: {p.aantal}</p>
                                        <p>Min. Prijs: €{p.minimumPrijs.toFixed(2)}</p>
                                        <p>Locatie: {p.klokLocatie}</p>
                                        <div className="edit-buttons">
                                            {/* de knop om de edit mode aan te zetten */}
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
                                                Wijzigen
                                            </button>
                                            {/* de verwijderknop */}
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
