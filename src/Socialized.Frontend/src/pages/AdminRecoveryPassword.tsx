import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Box, Typography, TextField, Button, Alert, Paper } from '@mui/material';
import { API_ENDPOINTS } from '../ApiEndPoints';
const AdminRecoveryPassword = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    try {
      const response = await fetch(API_ENDPOINTS.admins.recoveryPassword + '?email=' + email, {
        method: 'GET'
      });

      if (response.ok) {
        localStorage.setItem('adminRecoveryEmail', email);
        setSuccess('Код відновлення надіслано на вашу електронну пошту');
        setTimeout(() => {
          navigate('/admin/verify-recovery-code');
        }, 2000);
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при відновленні паролю');
      }
    } catch (err) {
      setError('Помилка при відновленні паролю');
    }
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
            Відновлення паролю адміністратора
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
          <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1, width: '100%' }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="Email"
              name="email"
              autoComplete="email"
              autoFocus
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Відновити пароль
            </Button>
            <Button
              fullWidth
              variant="text"
              onClick={() => navigate('/admin/login')}
            >
              Повернутися до входу
            </Button>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default AdminRecoveryPassword; 