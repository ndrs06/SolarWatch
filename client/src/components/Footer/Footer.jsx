import './Footer.scss';
import React from 'react';
import { Link } from 'react-router-dom';

export default function Footer() {
    return (
        <footer className="footer">
            <div>
                <div>                    
                    <Link to="#">About</Link>
                    <Link to="#">Privacy Policy</Link>
                    <Link to="#">Licensing</Link>
                    <Link to="#">Contact</Link>
                </div>
                <center><p>######</p></center>
            </div>
        </footer>
    );
}
