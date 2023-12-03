import React from 'react';
import { NavLink } from 'react-router-dom';

const navigation = [
    { name: 'Home', href: '/Home' },
    { name: 'Lights', href: '/Lights' },
    { name: 'AirConditioner', href: '/AirConditioner' },
];


function Header(props) {
    return (
        <>
            <nav className="navb">

                <div>
                    {navigation.map((item) => (
                        <NavLink
                            key={item.name}
                            to={item.href}    
                            className='nav-div'                       
                        >
                            {item.name}
                        </NavLink>
                    ))}
                </div>

            </nav>
            {props.children}
        </>
    );
}

export default Header;