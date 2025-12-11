import React from 'react';
import {Outlet, NavLink, useNavigate} from "react-router-dom";
import "./veilingmeester.css";

export default function Veilingmeester() {
    const navigate = useNavigate();
    
    return (
        <div class="veilingmeester">
            <header class="veilingmeester">
                <h1 class="veilingmeester">Veilingmeester Dashboard</h1>
            </header>
            <button className="LogOutButton" onClick={() => navigate("/")}>Uitloggen</button>
            <button className="HomeButton" onClick={() => { navigate("/") }}>Home</button>
            <nav class="veilingmeester">
                <NavLink to="/veilingmeester/veiling-beheren">Veiling Beheren</NavLink>
                <NavLink to="/veilingmeester/aangemelde-producten">Aangemelde Producten</NavLink>
                <NavLink to="/veilingmeester/verkoop-overzicht">Verkoop Overzicht</NavLink>
            </nav>
            <Outlet />
        </div>
    );
}