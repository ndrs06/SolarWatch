import React from 'react';
import AdminNavbar from "../../components/Admin_Components/admin_Navbar/admin_Navbar.jsx";
import Footer from "../../components/Footer/Footer.jsx";
import {Outlet} from "react-router-dom";

export default function Admin() {
    return (
        <div>
            <h1>ADMIN</h1>
            <AdminNavbar/>
            <Outlet/>
            <Footer/>
        </div>
    );
}