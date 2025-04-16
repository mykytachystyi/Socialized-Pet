import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Container,
  Box,
  Paper,
  Typography,
  Alert,
  TextField,
  CircularProgress,
  Chip,
  Avatar,
  IconButton
} from '@mui/material';
import { ArrowBack, Send, AttachFile } from '@mui/icons-material';
import { API_ENDPOINTS } from '../../ApiEndPoints';

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
          height: 'calc(100vh - 100px)',
        }}
      >
        <Paper 
          elevation={3} 
          sx={{ 
            padding: 2,
            width: '100%',
            display: 'flex',
            flexDirection: 'column',
            height: '100%',
            position: 'relative'
          }}
        >
          <Box sx={{ 
            width: '100%', 
            display: 'flex', 
            alignItems: 'center', 
            mb: 2,
            borderBottom: 1,
            borderColor: 'divider',
            pb: 2
          }}>
            <IconButton onClick={() => navigate('/my-appeals')} sx={{ mr: 2 }}>
              <ArrowBack />
            </IconButton>
            <Box sx={{ flex: 1 }}>
              <Typography variant="h6" component="h1">
                Звернення #{appeal.id}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {appeal.subject}
              </Typography>
            </Box>
            <Chip 
              label={appeal.state} 
              color={getStatusColor(appeal.state)}
              size="small"
            />
          </Box>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}
          
          {success && (
            <Alert severity="success" sx={{ mb: 2 }}>
              {success}
            </Alert>
          )}

          <Box sx={{ 
            flex: 1, 
            overflowY: 'auto',
            mb: 2,
            display: 'flex',
            flexDirection: 'column',
            gap: 2
          }}>
            {messages.map((message) => (
              <Box
                key={message.id}
                sx={{
                  display: 'flex',
                  flexDirection: 'column',
                  alignItems: message.userId === 1 ? 'flex-end' : 'flex-start',
                  maxWidth: '70%',
                  alignSelf: message.userId === 1 ? 'flex-end' : 'flex-start',
                }}
              >
                <Box
                  sx={{
                    display: 'flex',
                    alignItems: 'center',
                    mb: 0.5,
                    gap: 1
                  }}
                >
                  <Avatar 
                    sx={{ 
                      width: 24, 
                      height: 24,
                      bgcolor: message.userId === 1 ? 'primary.main' : 'secondary.main'
                    }}
                  >
                    {message.userId}
                  </Avatar>
                  <Typography variant="caption" color="text.secondary">
                    {formatDate(message.createdAt)}
                  </Typography>
                </Box>
                <Paper
                  elevation={1}
                  sx={{
                    p: 2,
                    bgcolor: message.userId === 1 ? 'primary.light' : 'grey.100',
                    color: message.userId === 1 ? 'primary.contrastText' : 'text.primary',
                    borderRadius: 2
                  }}
                >
                  <Typography variant="body1">
                    {message.message}
                  </Typography>
                  {message.files && message.files.length > 0 && (
                    <Box sx={{ mt: 1, display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                      {message.files.map((file) => (
                        <Chip
                          key={file.id}
                          icon={<AttachFile />}
                          label={file.relativePath.split('/').pop()}
                          size="small"
                          variant="outlined"
                          sx={{ 
                            borderRadius: 1,
                            bgcolor: 'background.paper'
                          }}
                        />
                      ))}
                    </Box>
                  )}
                </Paper>
              </Box>
            ))}
          </Box>

          <Box
            component="form"
            onSubmit={handleSubmitMessage}
            sx={{
              width: '100%',
              display: 'flex',
              gap: 1,
              borderTop: 1,
              borderColor: 'divider',
              pt: 2
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
              sx={{
                '& .MuiOutlinedInput-root': {
                  borderRadius: 2
                }
              }}
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