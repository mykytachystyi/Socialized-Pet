import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ThemeProvider, CssBaseline, Box, Container } from '@mui/material';
import theme from './theme';
import Login from './pages/Login';
import Register from './pages/user/Register';
import RecoveryPassword from './pages/user/RecoveryPassword';
import VerifyAndReset from './pages/user/VerifyAndReset';
import Profile from './pages/user/Profile';
import { useState, useEffect } from 'react';
import AdminLogin from './pages/admin/AdminLogin';
import CreateAdmin from './pages/admin/CreateAdmin';
import UserList from './pages/admin/UserList';
import AdminList from './pages/admin/AdminList';
import AdminDashboard from './pages/admin/AdminDashboard';
import AdminChangePassword from './pages/admin/AdminChangePassword';
import AdminRecoveryPassword from './pages/admin/AdminRecoveryPassword';
import AdminVerifyAndReset from './pages/admin/AdminVerifyAndReset';
import CreateAppeal from './pages/appeal/CreateAppeal';
import UserAppeals from './pages/user/UserAppeals';
import AdminAppeals from './pages/admin/AdminAppeals';
import AppealDetails from './pages/appeal/AppealDetails';
import AppealMessages from './pages/appeal/AppealMessages';
import ProtectedRoute from './components/ProtectedRoute';
import Home from './pages/Home';
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    const adminToken = localStorage.getItem('adminToken');
    setIsAuthenticated(!!token);
    setIsAdmin(!!adminToken);
  }, []);

  const handleLogout = () => {
    if (isAdmin) {
      localStorage.removeItem('adminToken');
      setIsAdmin(false);
    } else {
      localStorage.removeItem('token');
      setIsAuthenticated(false);
    }
  };

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
          <Header 
            isAuthenticated={isAuthenticated}
            isAdmin={isAdmin}
            onLogout={handleLogout}
          />
          <Container 
            component="main" 
            maxWidth="lg"
            sx={{ 
              flexGrow: 1,
              width: '100%',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              py: 3,
              '& > *': {
                width: '100%',
                maxWidth: '100%'
              }
            }}
          >
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/profile" element={<ProtectedRoute><Profile /></ProtectedRoute>} />
              <Route path="/recovery-password" element={<RecoveryPassword />} />
              <Route path="/verify-recovery-code" element={<VerifyAndReset />} />
              <Route path="/admin/login" element={<AdminLogin />} />
              <Route path="/admin/dashboard" element={<AdminDashboard />} />
              <Route path="/admin/users" element={<UserList />} />
              <Route path="/admin/admins" element={<AdminList />} />
              <Route path="/admin/create" element={<CreateAdmin />} />
              <Route path="/admin/change-password" element={<AdminChangePassword />} />
              <Route path="/admin/recovery-password" element={<AdminRecoveryPassword />} />
              <Route path="/admin/verify-recovery-code" element={<AdminVerifyAndReset />} />
              <Route path="/create-appeal" element={<ProtectedRoute><CreateAppeal /></ProtectedRoute>} />
              <Route path="/my-appeals" element={<ProtectedRoute><UserAppeals /></ProtectedRoute>} />
              <Route path="/appeal/:id" element={<ProtectedRoute><AppealDetails /></ProtectedRoute>} />
              <Route path="/appeal/:id/messages" element={<ProtectedRoute><AppealMessages /></ProtectedRoute>} />
              <Route path="/admin/appeals" element={<AdminAppeals />} />
            </Routes>
          </Container>
          <Footer />
        </Box>
      </Router>
    </ThemeProvider>
  );
}

export default App;