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
import { ArrowBack, Send, AttachFile } from '@mui/icons-material';
import { API_ENDPOINTS } from '../config';

interface Message {
  id: number;
  message: string;
  createdAt: string;
  userId: number;
  appealId: number;
  updatedAt: string;
  files?: FileInfo[];
}

interface FileInfo {
  id: number;
  messageId: number;
  relativePath: string;
}

interface Appeal {
  id: string;
  subject: string;
  state: number;
  createdAt: string;
  updatedAt: string;
  messages: Message[];
}

const AppealDetails = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const [appeal, setAppeal] = useState<Appeal | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetchAppeal();
    fetchMessages();
  }, [id]);

  const fetchAppeal = async () => {
    if (!id) return;
    try {
      const token = localStorage.getItem('token');
      const response = await fetch(API_ENDPOINTS.appeals.messages.list(Number(id)), {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      if (!response.ok) {
        throw new Error('Помилка завантаження звернення');
      }
      const data = await response.json();
      setAppeal(data);
    } catch (err) {
      setError('Помилка при завантаженні звернення');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchMessages = async () => {
    if (!id) return;
    try {
      const token = localStorage.getItem('token');
      const response = await fetch(API_ENDPOINTS.appeals.messages.list(Number(id)), {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      if (!response.ok) {
        throw new Error('Помилка завантаження повідомлень');
      }
      const data = await response.json();
      setMessages(data);
    } catch (err) {
      setError('Помилка при завантаженні повідомлень');
    }
  };

  const handleSubmitMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!message.trim()) return;

    try {
      const token = localStorage.getItem('token');
      const response = await fetch(API_ENDPOINTS.appeals.messages.create, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ message, appealId: id })
      });

      if (!response.ok) {
        throw new Error('Помилка відправки повідомлення');
      }

      const responseMessage = await response.json();
      setMessages([...messages, responseMessage]);
      setMessage('');
      setSuccess('Повідомлення відправлено');
    } catch (err) {
      setError('Помилка при відправці повідомлення');
    }
  };

  const handleDeleteAppeal = async () => {
    if (!id) return;
    try {
      const token = localStorage.getItem('token');
      const response = await fetch(API_ENDPOINTS.appeals.messages.delete(Number(id)), {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        throw new Error('Помилка видалення звернення');
      }

      navigate('/appeals');
    } catch (err) {
      setError('Помилка при видаленні звернення');
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString('uk-UA');
  };

  const getStatusColor = (status: number) => {
    switch (status) {
      case 0:
        return 'warning';
      case 1:
        return 'info';
      case 2:
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

          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
            <Typography variant="h4" gutterBottom>
              Деталі звернення #{id}
            </Typography>
            <Button 
              variant="contained" 
              color="primary"
              onClick={() => navigate(`/appeal/${id}/messages`)}
            >
              Переглянути повідомлення
            </Button>
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
                  label={appeal.state} 
                  color={getStatusColor(appeal.state)}
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
            {messages.map((message) => (
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
                    {message.userId}
                  </Avatar>
                  <Box>
                    <Typography variant="subtitle2" component="div">
                      {message.userId}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {formatDate(message.createdAt)}
                    </Typography>
                  </Box>
                </Box>
                <Typography variant="body1" sx={{ mt: 1, ml: 7 }}>
                  {message.message}
                </Typography>
                {message.files && message.files.length > 0 && (
                  <Box sx={{ mt: 1, ml: 7, display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                    {message.files.map((file) => (
                      <Chip
                        key={file.id}
                        icon={<AttachFile />}
                        label={file.relativePath.split('/').pop()}
                        size="small"
                        variant="outlined"
                        sx={{ borderRadius: 1 }}
                      />
                    ))}
                  </Box>
                )}
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
              value={message}
              onChange={(e) => setMessage(e.target.value)}
            />
            <IconButton 
              type="submit" 
              color="primary"
              disabled={!message.trim()}
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