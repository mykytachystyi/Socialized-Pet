import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import RecoveryPassword from './pages/RecoveryPassword';
import VerifyAndReset from './pages/VerifyAndReset';
import Profile from './pages/Profile';
import { useState, useEffect } from 'react';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    setIsAuthenticated(!!token);
  }, []);

  return (
    <Router>
      <nav style={{ padding: '20px' }}>
        {isAuthenticated ? (
          <>
            <Link to="/profile" style={{ marginRight: '10px' }}>Профіль</Link>
            <Link to="/" style={{ marginRight: '10px' }}>Головна</Link>
            <Link 
              to="/login" 
              onClick={() => {
                localStorage.removeItem('token');
                setIsAuthenticated(false);
              }}
            >
              Вийти
            </Link>
          </>
        ) : (
          <>
            <Link to="/login" style={{ marginRight: '10px' }}>Увійти</Link>
            <Link to="/register" style={{ marginRight: '10px' }}>Зареєструватися</Link>
            <Link to="/">Головна</Link>
          </>
        )}
      </nav>
      <Routes>
        <Route path="/" element={<div>Головна сторінка</div>} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/recovery-password" element={<RecoveryPassword />} />
        <Route path="/verify-recovery-code" element={<VerifyAndReset />} />
        <Route path="/profile" element={<Profile />} />
      </Routes>
    </Router>
  );
}

export default App;