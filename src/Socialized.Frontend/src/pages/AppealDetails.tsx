import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Container,
  Box,
  Paper,
  Typography,
  Button,
  Alert,
  TextField,
  Divider,
  List,
  ListItem,
  ListItemText,
  CircularProgress,
  Chip,
  Avatar,
  Grid,
  IconButton
} from '@mui/material';
import { ArrowBack, Send } from '@mui/icons-material';

interface Message {
  id: string;
  content: string;
  createdAt: string;
  userId: string;
  userEmail: string;
}

interface Appeal {
  id: string;
  subject: string;
  status: string;
  createdAt: string;
  updatedAt: string;
  messages: Message[];
}

const AppealDetails = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const [appeal, setAppeal] = useState<Appeal | null>(null);
  const [newMessage, setNewMessage] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchAppeal = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        navigate('/login');
        return;
      }

      try {
        const response = await fetch(`http://localhost:5217/1.0/Appeals/GetAppeal/${id}`, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        if (response.ok) {
          const data = await response.json();
          setAppeal(data);
        } else if (response.status === 401) {
          localStorage.removeItem('token');
          navigate('/login');
        } else {
          const data = await response.json();
          setError(data.message || 'Помилка при отриманні звернення');
        }
      } catch (err) {
        console.error('Помилка при отриманні звернення:', err);
        setError('Помилка сервера');
      } finally {
        setIsLoading(false);
      }
    };

    fetchAppeal();
  }, [id, navigate]);

  const handleSubmitMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    const token = localStorage.getItem('token');
    if (!token || !newMessage.trim()) return;

    try {
      const response = await fetch(`http://localhost:5217/1.0/AppealsMessage/Create`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ content: newMessage })
      });

      if (response.ok) {
        const data = await response.json();
        setAppeal(prev => prev ? {
          ...prev,
          messages: [...prev.messages, data]
        } : null);
        setNewMessage('');
        setSuccess('Повідомлення додано');
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при додаванні повідомлення');
      }
    } catch (err) {
      console.error('Помилка при додаванні повідомлення:', err);
      setError('Помилка сервера');
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString('uk-UA');
  };

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case 'pending':
        return 'warning';
      case 'in progress':
        return 'info';
      case 'completed':
        return 'success';
      default:
        return 'default';
    }
  };

  if (isLoading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh">
        <CircularProgress />
      </Box>
    );
  }

  if (!appeal) {
    return (
      <Container>
        <Alert severity="error">Звернення не знайдено</Alert>
      </Container>
    );
  }

  return (
    <Container component="main" maxWidth="md">
      <Box
        sx={{
          marginTop: 4,
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
          <Box sx={{ width: '100%', display: 'flex', alignItems: 'center', mb: 3 }}>
            <IconButton onClick={() => navigate('/my-appeals')} sx={{ mr: 2 }}>
              <ArrowBack />
            </IconButton>
            <Typography variant="h5" component="h1">
              Звернення #{appeal.id}
            </Typography>
          </Box>

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
          
          <Grid container spacing={3} sx={{ width: '100%', mb: 3 }}>
            <Grid item xs={12}>
              <Typography variant="h6" gutterBottom>
                Тема: {appeal.subject}
              </Typography>
              <Box sx={{ display: 'flex', gap: 2, alignItems: 'center', mb: 2 }}>
                <Chip 
                  label={appeal.status} 
                  color={getStatusColor(appeal.status)}
                  size="small"
                />
                <Typography variant="body2" color="text.secondary">
                  Створено: {formatDate(appeal.createdAt)}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Оновлено: {formatDate(appeal.updatedAt)}
                </Typography>
              </Box>
            </Grid>
          </Grid>

          <Divider sx={{ width: '100%', my: 3 }} />

          <Typography variant="h6" gutterBottom sx={{ alignSelf: 'flex-start' }}>
            Повідомлення
          </Typography>
          <List sx={{ width: '100%' }}>
            {appeal.messages.map((message) => (
              <ListItem 
                key={message.id} 
                divider
                sx={{
                  display: 'flex',
                  flexDirection: 'column',
                  alignItems: 'flex-start',
                  py: 2
                }}
              >
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                  <Avatar sx={{ mr: 1, bgcolor: 'primary.main' }}>
                    {message.userEmail[0].toUpperCase()}
                  </Avatar>
                  <Box>
                    <Typography variant="subtitle2" component="div">
                      {message.userEmail}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {formatDate(message.createdAt)}
                    </Typography>
                  </Box>
                </Box>
                <Typography variant="body1" sx={{ ml: 7 }}>
                  {message.content}
                </Typography>
              </ListItem>
            ))}
          </List>

          <Box 
            component="form" 
            onSubmit={handleSubmitMessage} 
            sx={{ 
              width: '100%', 
              mt: 3,
              display: 'flex',
              gap: 2
            }}
          >
            <TextField
              fullWidth
              multiline
              rows={2}
              variant="outlined"
              placeholder="Введіть ваше повідомлення..."
              value={newMessage}
              onChange={(e) => setNewMessage(e.target.value)}
            />
            <IconButton 
              type="submit" 
              color="primary"
              disabled={!newMessage.trim()}
              sx={{ alignSelf: 'flex-end' }}
            >
              <Send />
            </IconButton>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default AppealDetails; 