import { useState } from 'react';
import { 
  Container, Box, TextField, Button, Typography, 
  Paper, Alert, CircularProgress 
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { API_ENDPOINTS } from '../config';
const RecoveryPassword = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    try {
      const response = await fetch(API_ENDPOINTS.users.recoveryPassword + '?email=' + email, {
        method: 'GET'
      });

      if (response.ok) {
        // Перенаправляємо на сторінку введення коду
        navigate(`/verify-recovery-code?email=${encodeURIComponent(email)}`);
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при відправці коду');
      }
    } catch (err) {
      setError('Помилка сервера');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Paper elevation={3} sx={{ mt: 8, p: 4 }}>
        <Typography component="h1" variant="h5" align="center">
          Відновлення паролю
        </Typography>
        <Typography variant="body2" sx={{ mt: 2, mb: 2 }} align="center">
          Введіть вашу email адресу для отримання коду відновлення
        </Typography>
        
        <Box component="form" onSubmit={handleSubmit}>
          <TextField
            fullWidth
            required
            label="Email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            margin="normal"
          />
          
          {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
          
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3 }}
            disabled={isLoading}
          >
            {isLoading ? <CircularProgress size={24} /> : 'Отримати код'}
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default RecoveryPassword;