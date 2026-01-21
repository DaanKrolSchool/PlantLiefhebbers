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
    veilTijd: string | null;
};

type EditValues = {
    prijsVerandering: number;
    maximumPrijs: number;
    veilTijd: string | null;
};

function formatVeilTijd(veilTijd: string | null | undefined): string {
    if (!veilTijd) return "—";

    // "HH:mm:ss" of "HH:mm"
    const hhmm = veilTijd.match(/^(\d{1,2}):(\d{2})(?::\d{2})?$/);
    if (hhmm) {
        const h = hhmm[1].padStart(2, "0");
        const m = hhmm[2];
        return `${h}:${m}`;
    }

    // "PT13H45M" (ISO duration)
    const iso = veilTijd.match(/^PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?$/);
    if (iso) {
        const h = (iso[1] ?? "0").padStart(2, "0");
        const m = (iso[2] ?? "0").padStart(2, "0");
        return `${h}:${m}`;
    }

    return veilTijd;
}


function AangemeldeProducten() {
    //de getters en setters
    const [editMode, setEditMode] = useState<number | null>(null);
    const [editValues, setEditValues] = useState<EditValues>({
        prijsVerandering: 0,
        maximumPrijs: 0,
        veilTijd: null
    });    const [products, setProducts] = useState<Product[]>([]);
    //constante
    //de dag van vandaag
    const today = new Date().toISOString().split('T')[0];
    //deze kijkt naar welke producten nog na vandaag komen.
    const upcomingProducts = products.filter(product => product.veilDatum >= today);

    //Deze gebruikt productController om zo alle producten te krijgen.
    useEffect(() => {
        async function fetchProducts() {
            const token = localStorage.getItem("token");
            //de locatie waar hij te vinden is binnen de controller
            const res = await fetch(`/Product/veilingmeester/all`, {
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
        const res = await fetch(`/Product/veilingMeester/${productId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                productId,
                prijsVerandering: editValues.prijsVerandering,
                maximumPrijs: editValues.maximumPrijs,
                veilTijd: editValues.veilTijd ? `${editValues.veilTijd}:00` : null
            }),
        });

        //hier word er gekeken naar welke datum hoort bij welk product en zet hem in een volgorde
        if (res.ok) {
            setEditMode(null);
            const res2 = await fetch("/Product/veilingmeester/all", {
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
        const res = await fetch(`/Product/${productId}`, {
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
                                                min="0.01"
                                                step="0.01"
                                                value={editValues.prijsVerandering}
                                                onChange={e => setEditValues({ ...editValues, prijsVerandering: Number(e.target.value) })}
                                            />
                                        </div>

                                        <div className="edit-row">
                                            <label>Max. Prijs:</label>
                                            <input
                                                type="number"
                                                min="0.01"
                                                step="0.01"
                                                value={editValues.maximumPrijs}
                                                onChange={e => setEditValues({ ...editValues, maximumPrijs: Number(e.target.value) })}
                                            />
                                        </div>
                                        {/*<div className="edit-row">
                                            <label>Prijs Daling:</label>
                                            <input
                                                type="time"
                                                value={editValues.veilTijd ?? ""}
                                                onChange={e =>
                                                    setEditValues({ ...editValues, veilTijd: e.target.value || null })
                                                }
                                            />
                                        </div>*/}


                                        <p>Veildatum: {new Date(p.veilDatum).toLocaleDateString("nl-NL")}</p>
                                        {/*<p>Starttijd: {formatVeilTijd(p.veilTijd)}</p>*/}
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

                                            <p>
                                                Prijs Daling: €
                                                {p.prijsVerandering != null ? p.prijsVerandering.toFixed(2) : "—"}
                                            </p>

                                            <p>
                                                Max. Prijs: €
                                                {p.maximumPrijs != null ? p.maximumPrijs.toFixed(2) : "—"}
                                            </p>

                                            <p>Veildatum: {new Date(p.veilDatum).toLocaleDateString("nl-NL")}</p>
                                            {/*<p>Starttijd: {formatVeilTijd(p.veilTijd)}</p>*/}
                                        <p>Locatie: {p.klokLocatie}</p>
                                        <div className="edit-buttons">
                                            <button
                                                style={{ backgroundColor: "#D0B070", color: "black" }}
                                                onClick={() => {
                                                    setEditMode(p.productId);
                                                    setEditValues({
                                                        prijsVerandering: p.prijsVerandering,
                                                        maximumPrijs: p.maximumPrijs,
                                                        veilTijd: p.veilTijd ? p.veilTijd.slice(0, 5) : null
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

