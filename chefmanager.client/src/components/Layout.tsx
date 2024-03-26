import React, { useEffect, useState } from 'react';
import Navbar from './Navbar';

interface LayoutProps {
    children: React.ReactNode;
}

function Layout({ children }: LayoutProps) {
    const [isdark, setIsdark] = useState(false);

    useEffect(() => {
        setIsdark(JSON.parse(localStorage.getItem('isdark')) || false);
    }, []);

    const theme = isdark ? 'nord' : 'business';

    return (
        <main data-theme={theme}>
            <header>
                <Navbar isdark={isdark} setIsdark={setIsdark} />
            </header>
            <main>
                {children}
            </main>
        </main>
    );
}

export default Layout;
