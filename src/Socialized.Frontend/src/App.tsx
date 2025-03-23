import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import RecoveryPassword from './pages/RecoveryPassword';
import VerifyAndReset from './pages/VerifyAndReset';
import Profile from './pages/Profile';
import { useState, useEffect } from 'react';
import AdminLogin from './pages/AdminLogin';
import CreateAdmin from './pages/CreateAdmin';
import UserList from './pages/UserList';
import AdminList from './pages/AdminList';
import AdminDashboard from './pages/AdminDashboard';
import AdminChangePassword from './pages/AdminChangePassword';
import AdminRecoveryPassword from './pages/AdminRecoveryPassword';
import AdminVerifyAndReset from './pages/AdminVerifyAndReset';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    const adminToken = localStorage.getItem('adminToken');
    setIsAuthenticated(!!token);
    setIsAdmin(!!adminToken);
  }, []);

  return (
    <Router>
      <nav style={{ padding: '20px' }}>
        {isAdmin ? (
          <>
            <Link to="/admin/dashboard" style={{ marginRight: '10px' }}>Панель адміністратора</Link>
            <Link to="/admin/users" style={{ marginRight: '10px' }}>Користувачі</Link>
            <Link to="/admin/admins" style={{ marginRight: '10px' }}>Адміністратори</Link>
            <Link to="/admin/create" style={{ marginRight: '10px' }}>Створити адміністратора</Link>
            <Link to="/admin/change-password" style={{ marginRight: '10px' }}>Змінити пароль</Link>
            <Link 
              to="/admin/login" 
              onClick={() => {
                localStorage.removeItem('adminToken');
                setIsAdmin(false);
              }}
            >
              Вийти
            </Link>
          </>
        ) : isAuthenticated ? (
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
        <Route path="/admin/login" element={<AdminLogin />} />
        <Route path="/admin/dashboard" element={<AdminDashboard />} />
        <Route path="/admin/create" element={<CreateAdmin />} />
        <Route path="/admin/change-password" element={<AdminChangePassword />} />
        <Route path="/admin/recovery-password" element={<AdminRecoveryPassword />} />
        <Route path="/admin/verify-recovery-code" element={<AdminVerifyAndReset />} />
        <Route path="/register" element={<Register />} />
        <Route path="/recovery-password" element={<RecoveryPassword />} />
        <Route path="/verify-recovery-code" element={<VerifyAndReset />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/admin/users" element={<UserList />} />
        <Route path="/admin/admins" element={<AdminList />} />
      </Routes>
    </Router>
  );
}

export default App;