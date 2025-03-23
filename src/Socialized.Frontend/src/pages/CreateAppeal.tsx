import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Box, Typography, TextField, Button, Alert, Paper } from '@mui/material';

const CreateAppeal = () => {
  const navigate = useNavigate();
  const [subject, setSubject] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    const token = localStorage.getItem('token');
    if (!token) {
      setError('Будь ласка, увійдіть в систему');
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
        setSuccess('Звернення успішно створено');
        setTimeout(() => {
          navigate('/profile');
        }, 2000);
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при створенні звернення');
      }
    } catch (err) {
      console.error('Помилка при створенні звернення:', err);
      setError('Помилка при створенні звернення');
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
            Створення звернення
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
              id="subject"
              label="Тема звернення"
              name="subject"
              autoComplete="off"
              value={subject}
              onChange={(e) => setSubject(e.target.value)}
              multiline
              rows={4}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Створити звернення
            </Button>
            <Button
              fullWidth
              variant="text"
              onClick={() => navigate('/profile')}
            >
              Повернутися до профілю
            </Button>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default CreateAppeal; 