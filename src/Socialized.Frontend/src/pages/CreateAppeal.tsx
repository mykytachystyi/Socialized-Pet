import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Paper,
  Typography,
  TextField,
  Button,
  Box,
  Alert,
  CircularProgress
} from '@mui/material';

const CreateAppeal = () => {
  const navigate = useNavigate();
  const [subject, setSubject] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);

    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
      return;
    }

    try {
      const response = await fetch('http://localhost:5217/1.0/Appeals/Create', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ 
          subject
        })
      });

      if (response.ok) {
        navigate('/my-appeals');
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при створенні звернення');
      }
    } catch (err) {
      console.error('Помилка при створенні звернення:', err);
      setError('Помилка сервера');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container maxWidth="md">
      <Paper sx={{ p: 3 }}>
        <Typography variant="h4" gutterBottom>
          Створення нового звернення
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Тема"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            margin="normal"
            required
            disabled={isLoading}
          />

          <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
            <Button
              type="submit"
              variant="contained"
              disabled={isLoading || !subject.trim()}
            >
              {isLoading ? <CircularProgress size={24} /> : 'Створити звернення'}
            </Button>
            <Button
              variant="outlined"
              onClick={() => navigate('/my-appeals')}
              disabled={isLoading}
            >
              Скасувати
            </Button>
          </Box>
        </form>
      </Paper>
    </Container>
  );
};

export default CreateAppeal; 