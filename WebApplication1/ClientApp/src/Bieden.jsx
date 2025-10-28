import React from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate } from 'react-router-dom';
import './Bieden.css';
import Page from './Page';

function Home() {
    const navigate = useNavigate();

    return (
        <div className="App">
            <button className="top-right-button" onClick={() => navigate('/page')}>
                Go to Page
            </button>

            <h1>Welcome to My Back Page</h1>
            <p>This is a simple React + HTML + CSS page.</p>
        </div>
    );
}

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/page" element={<Page />} />
            </Routes>
        </Router>
    );
}

export default App;
