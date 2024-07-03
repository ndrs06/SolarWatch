import './Navbar.scss';
import React from 'react';
import { Link } from 'react-router-dom';

export default function Navbar() {
    return (
        <div className="navbar">
            <Link to="/">
                <button>🏠</button>
            </Link>
            <Link to="/sign-in">
                <button>SignIn</button>
            </Link>
            <Link to="/sign-up">
                <button>SignUp</button>
            </Link>
            <Link to="/solar-watch">
                <button>SolarWatch</button>
            </Link>
        </div>
    );
}