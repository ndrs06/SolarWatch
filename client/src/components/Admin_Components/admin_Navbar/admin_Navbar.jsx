import React from 'react';
import { Link } from 'react-router-dom';

export default function AdminNavbar() {
    return (
        <div className="navbar">
            <Link to="/admin/users">
                <button>Users</button>
            </Link>
            <Link to="/admin/cities">
                <button>Cities</button>
            </Link>
            <Link to="/admin/cities/solar-watches">
                <button>SolarWatches</button>
            </Link>
            <Link to="/">
                <button>Go-UserPage</button>
            </Link>
        </div>
    );
}