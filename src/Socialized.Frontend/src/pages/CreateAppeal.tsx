import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Paper,
  Typography,
  Box,
  TextField,
  Button,
  Alert
} from '@mui/material';
import { API_ENDPOINTS } from '../ApiEndPoints';

export default function CreateAppeal() {
  const navigate = useNavigate();
  const [subject, setSubject] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!subject.trim()) return;

    try {
      const token = localStorage.getItem('token');
      const response = await fetch(API_ENDPOINTS.appeals.create, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ subject })
      });

      if (!response.ok) {
        throw new Error('Помилка створення звернення');
      }

      const data = await response.json();
      navigate(`/appeals/${data.id}`);
    } catch (err) {
      setError('Помилка при створенні звернення');
    }
  };

  return (
    <Container maxWidth="md">
      <Paper sx={{ p: 3, mt: 4 }}>
        <Typography variant="h5" gutterBottom>
          Створення нового звернення
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <Box component="form" onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Тема"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            margin="normal"
            required
          />

          <Box sx={{ mt: 3, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button
              variant="outlined"
              onClick={() => navigate('/appeals')}
            >
              Скасувати
            </Button>
            <Button
              type="submit"
              variant="contained"
              disabled={!subject.trim()}
            >
              Створити звернення
            </Button>
          </Box>
        </Box>
      </Paper>
    </Container>
  );
} 