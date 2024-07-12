import './index.scss'
import React from 'react'
import ReactDOM from 'react-dom/client'
import {BrowserRouter, Route, Routes} from "react-router-dom";

import Home from "./pages/Home/Home.jsx";
import SignIn from "./pages/SignIn/SignIn.jsx";
import SignUp from "./pages/SignUp/SignUp.jsx";
import SolarWatch from "./pages/SolarWatch/SolarWatch.jsx"

import {ProfileContextProvider} from "./contexts/ProfileContext.jsx";
import ProtectedRoute from "./components/ProtectedRoute/ProtectedRoute.jsx";
import Layout from "./pages/Layout/Layout.jsx";

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
      <ProfileContextProvider>
          <BrowserRouter>
              <Routes>
                  <Route path='/' element={<Layout/>}>
                          
                      <Route path='/' element={<Home/>}/>
                      <Route path='/sign-in' element={<SignIn/>}/>
                      <Route path='/sign-up' element={<SignUp/>}/>
                      
                      <Route path='/solar-watch' element={<SolarWatch/>}/>     
                      
                      <Route element={<ProtectedRoute/>}>
                      </Route>
                      
                  </Route>                  
              </Routes>
          </BrowserRouter>
      </ProfileContextProvider>
  </React.StrictMode>
)
