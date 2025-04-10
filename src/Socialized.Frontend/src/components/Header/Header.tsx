import { AppBar, Toolbar, Typography, Button, Box, Container } from '@mui/material';
import { Link } from 'react-router-dom';
import './Header.css';

interface HeaderProps {
  isAuthenticated: boolean;
  isAdmin: boolean;
  onLogout: () => void;
}

const Header = ({ isAuthenticated, isAdmin, onLogout }: HeaderProps) => {
  return (
    <AppBar position="static" color="primary" elevation={0} className="header">
      <Container maxWidth="lg">
        <Toolbar className="toolbar">
          <Typography variant="h6" component="div" className="logo">
            Socialized Pet
          </Typography>
          {isAdmin ? (
            <Box className="nav-buttons">
              <Button color="inherit" component={Link} to="/admin/dashboard">
                Панель адміністратора
              </Button>
              <Button color="inherit" component={Link} to="/admin/users">
                Користувачі
              </Button>
              <Button color="inherit" component={Link} to="/admin/admins">
                Адміністратори
              </Button>
              <Button color="inherit" component={Link} to="/admin/appeals">
                Звернення
              </Button>
              <Button color="inherit" component={Link} to="/admin/create">
                Створити адміністратора
              </Button>
              <Button color="inherit" component={Link} to="/admin/change-password">
                Змінити пароль
              </Button>
              <Button 
                color="inherit" 
                component={Link} 
                to="/admin/login"
                onClick={onLogout}
              >
                Вийти
              </Button>
            </Box>
          ) : isAuthenticated ? (
            <Box className="nav-buttons">
              <Button color="inherit" component={Link} to="/profile">
                Профіль
              </Button>
              <Button color="inherit" component={Link} to="/">
                Головна
              </Button>
              <Button 
                color="inherit" 
                component={Link} 
                to="/login"
                onClick={onLogout}
              >
                Вийти
              </Button>
            </Box>
          ) : (
            <Box className="nav-buttons">
              <Button color="inherit" component={Link} to="/login">
                Увійти
              </Button>
              <Button color="inherit" component={Link} to="/register">
                Зареєструватися
              </Button>
              <Button color="inherit" component={Link} to="/">
                Головна
              </Button>
            </Box>
          )}
        </Toolbar>
      </Container>
    </AppBar>
  );
};

export default Header; 