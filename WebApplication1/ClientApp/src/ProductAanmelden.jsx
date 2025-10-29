import React from 'react';
import './App.css';

function ProductAanmelden() {
    return (
        <div className="App">
            <nav>
                <a>Product aanmelden</a>
                <a>Aangemelde prodcuten</a>
                <a>Verkoop overzicht</a>
            </nav>
            <form>
                <label htmlFor="naam">Naam:</label>
                <input type="text" name="naam"/><br/>
                <label htmlFor="soort">Soort:</label>
                <input type="text" name="soort"/><br/>
                <label htmlFor="hoeveelheid">Hoeveelheid:</label>
                <input type="number" name="hoeveelheid"/><br/>
                <label htmlFor="mprijs">Minimumprijs:</label>
                <input type="number" step="0.01" name="mprijs"/><br/>
                <label htmlFor="locatie">Kloklocatie:</label>
                <select name="locatie">
                    <option value="aalsmeer">Aalsmeer</option>
                    <option value="rijnsburg">Rijnsburg</option>
                    <option value="eelde">Eelde</option>
                    <option value="naaldwijk">Naaldwijk</option>
                </select><br />
                <label htmlFor="vdatum">Veildatum:</label>
                <input type="date" name="vdatum"/><br/>
                <label htmlFor="afbeelding">Afbeelding:</label>
                <input type="file" name="afbeelding"/><br/><br />
                <input type="submit" value="Product aanmelden"/>
            </form>
        </div>
    );
}

export default ProductAanmelden;
