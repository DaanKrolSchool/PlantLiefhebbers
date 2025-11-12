import React from 'react';

function ProductAanmelden() {
    return (
        <div class="aanvoerder">
            <form class="aanvoerder">
                <h2> Product informatie</h2>
                <label for="naam">Naam:</label>
                <input type="text" id="naam" name="naam"/><br/>
                <label for="soort">Soort:</label>
                <input type="text" id="soort" name="soort"/><br/>
                <label for="hoeveelheid">Hoeveelheid:</label>
                <input type="number" id="hoeveelheid" min="1" name="hoeveelheid"/><br/>
                <label for="mprijs">Minimumprijs:</label>
                <input type="number" id="mprijs" min="0.01" step="0.01" name="mprijs"/><br/>
                <label for="locatie">Kloklocatie:</label>
                <select id="locatie" name="locatie">
                    <option value="aalsmeer">Aalsmeer</option>
                    <option value="eelde">Eelde</option>
                    <option value="naaldwijk">Naaldwijk</option>
                    <option value="rijnsburg">Rijnsburg</option>
                </select><br />
                <label for="vdatum">Veildatum:</label>
                <input type="date" id="vdatum" name="vdatum"/><br/>
                <label for="afbeelding">Afbeelding:</label>
                <input type="file" id="afbeelding" name="afbeelding"/><br/><br />
                <input type="submit" value="Product aanmelden"/>
            </form>
        </div>
    );
}

export default ProductAanmelden;
