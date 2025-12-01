import React from 'react';
import {Outlet, NavLink, useNavigate} from "react-router-dom";
import "./Aanvoerder.css";

export default function Aanvoerder() {
    const navigate = useNavigate();
    
    return (
        <div className="aanvoerder">
            <header className="aanvoerder">
                <h1 className="aanvoerder">Aanvoerder Dashboard</h1>
            </header>
            <button className="LogOutButton" onClick={() => navigate("/")}>Uitloggen</button>
            <nav className="aanvoerder">
                <NavLink to="/aanvoerder/product-aanmelden">Product Aanmelden</NavLink>
                <NavLink to="/aanvoerder/aangemelde-producten">Aangemelde Producten</NavLink>
                <NavLink to="/aanvoerder/verkoop-overzicht">Verkoop Overzicht</NavLink>
            </nav>
            <Outlet />
        </div>
    );
}