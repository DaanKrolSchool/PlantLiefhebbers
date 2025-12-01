
import { useState, useEffect } from 'react';

function AangemeldeProducten() {
    const [products, setProducts] = useState([]);
    const today = new Date();
    const upcomingProducts = products.filter(product => {
        const productDate = new Date(product.veilDatum);
        return productDate >= today;
    });

    useEffect(() => {
        async function fetchProducts() {
            const res = await fetch(`https://localhost:7225/Product/datum`);
            const data = await res.json();
            setProducts(data);
        }
        fetchProducts();
    }, []);

    const groupedByDate = upcomingProducts.reduce((groups, product) => {
        const date = new Date(product.veilDatum).toLocaleDateString("nl-NL", {
            year: "numeric",
            month: "long",
            day: "numeric"
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
                        {items.map((p) => (
                            <div key={p.productId} className="product-kaart">
                                <h3>{p.naam}</h3>
                                <p>Soort: {p.soortPlant}</p>
                                <p>Aantal: {p.aantal}</p>
                                <p>Minimum Prijs: €{p.minimumPrijs.toFixed(2)}</p>
                                <p>Locatie: {p.klokLocatie}</p>
                            </div>
                        ))}
                    </div>
                </div>
            ))}
        </div>
    );
}

export default AangemeldeProducten;
