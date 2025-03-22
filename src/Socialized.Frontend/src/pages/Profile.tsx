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
import { useNavigate } from 'react-router-dom';

const Profile = () => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    console.log('Profile token check:', token);
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

  return (
    <Container component="main" maxWidth="md">
      <Box sx={{ mt: 8 }}>
        <Paper elevation={3} sx={{ p: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            Профіль користувача
          </Typography>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Box sx={{ mt: 4 }}>
            <Typography variant="body1" gutterBottom>
              Тут буде інформація про профіль користувача
            </Typography>

            <Button
              variant="contained"
              color="error"
              onClick={() => setShowDeleteDialog(true)}
              sx={{ mt: 2 }}
            >
              Видалити акаунт
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