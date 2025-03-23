import { useState, useEffect } from 'react';
import {
  Container,
  Box,
  Paper,
  Typography,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Alert,
  CircularProgress
} from '@mui/material';
import { useNavigate, Link } from 'react-router-dom';

const Profile = () => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);
  const [success, setSuccess] = useState('');

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
    }
  }, [navigate]);

  const handleDeleteAccount = async () => {
    setIsLoading(true);
    setError('');

    try {
      const token = localStorage.getItem('token');
      console.log('Delete account token:', token);
      if (!token) {
        navigate('/login');
        return;
      }
      
      console.log(token);
      const response = await fetch('http://localhost:5217/1.0/Users/Delete', {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        }
      });

      if (response.ok) {
        localStorage.removeItem('token');
        navigate('/login');
      } else if (response.status === 401) {
        localStorage.removeItem('token');
        navigate('/login');
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при видаленні акаунту');
      }
    } catch (err) {
      setError('Помилка сервера');
    } finally {
      setIsLoading(false);
      setShowDeleteDialog(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Paper 
          elevation={3} 
          sx={{ 
            padding: 4, 
            width: '100%',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center'
          }}
        >
          <Typography component="h1" variant="h5">
            Профіль користувача
          </Typography>
          {error && (
            <Alert severity="error" sx={{ mt: 2, width: '100%' }}>
              {error}
            </Alert>
          )}
          {success && (
            <Alert severity="success" sx={{ mt: 2, width: '100%' }}>
              {success}
            </Alert>
          )}
          <Box sx={{ mt: 1, width: '100%' }}>
            <Button
              fullWidth
              variant="contained"
              component={Link}
              to="/create-appeal"
              sx={{ mt: 3, mb: 2 }}
            >
              Створити звернення
            </Button>
            <Button
              fullWidth
              variant="contained"
              component={Link}
              to="/my-appeals"
              sx={{ mt: 3, mb: 2 }}
            >
              Мої звернення
            </Button>
            <Button
              fullWidth
              variant="contained"
              color="error"
              onClick={() => setShowDeleteDialog(true)}
              sx={{ mt: 3, mb: 2 }}
            >
              Видалити акаунт
            </Button>
            <Button
              fullWidth
              variant="text"
              onClick={handleLogout}
            >
              Вийти
            </Button>
          </Box>
        </Paper>
      </Box>

      <Dialog
        open={showDeleteDialog}
        onClose={() => setShowDeleteDialog(false)}
      >
        <DialogTitle>
          Видалення акаунту
        </DialogTitle>
        <DialogContent>
          <Typography>
            Ви впевнені, що хочете видалити свій акаунт? Ця дія незворотна.
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowDeleteDialog(false)}>
            Скасувати
          </Button>
          <Button
            onClick={handleDeleteAccount}
            color="error"
            variant="contained"
            disabled={isLoading}
          >
            {isLoading ? <CircularProgress size={24} /> : 'Видалити'}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default Profile; 