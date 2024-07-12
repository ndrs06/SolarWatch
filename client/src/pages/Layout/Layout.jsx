import React, {useEffect} from "react";
import Navbar from "../../components/Navbar/Navbar.jsx";
import {Outlet} from "react-router-dom";

export default function Layout() {

   /* useEffect(() => {
        let lastScrollTop = 0;
        const navbar = document.querySelector('.navbar');

        const onScroll = () => {
            let scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            if (scrollTop > lastScrollTop) {
                // Scroll down
                navbar.style.top = '-60px';
            } else {
                // Scroll up
                navbar.style.top = '0';
            }
            lastScrollTop = scrollTop <= 0 ? 0 : scrollTop;
        };

        window.addEventListener('scroll', onScroll);

        return () => {
            window.removeEventListener('scroll', onScroll);
        };
    }, []);*/
    
    return (
        <div className="layout">
            <Navbar />
            <Outlet />
        </div>
    );
}