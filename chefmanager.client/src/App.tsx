
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home.tsx';
import Login from './components/Login.tsx';
import Register from './pages/Register.tsx';
import Layout from './components/Layout'; // Import Layout component

function App() {
    return (
        <BrowserRouter>
            <Layout> 
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/" element={<Home />} />
                </Routes>
            </Layout>
        </BrowserRouter>
    );
}

export default App;
