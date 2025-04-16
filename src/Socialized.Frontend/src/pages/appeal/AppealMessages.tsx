import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Container,
  Paper,
  Typography,
  Box,
  TextField,
  Button,
  List,
  ListItem,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Alert,
  CircularProgress,
  Chip,
  Avatar,
  Divider,
  useTheme
} from '@mui/material';
import { Delete as DeleteIcon, Edit as EditIcon, Send as SendIcon, CloudUpload as CloudUploadIcon, AttachFile as AttachFileIcon, ArrowBack } from '@mui/icons-material';
import { API_ENDPOINTS } from '../../ApiEndPoints';

interface FileInfo {
  id: number;
  messageId: number;
  relativePath: string;
}

interface Message {
  id: number;
  message: string;
  createdAt: string;
  userId: number;
  appealId: number;
  updatedAt: string;
  files?: FileInfo[];
}

export default function AppealMessages() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [editingMessage, setEditingMessage] = useState<Message | null>(null);
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [editText, setEditText] = useState('');
  const [success, setSuccess] = useState('');
  const [files, setFiles] = useState<FileList | null>(null);
  const [selectedMessage, setSelectedMessage] = useState<Message | null>(null);
  const [addFileDialogOpen, setAddFileDialogOpen] = useState(false);

  const theme = useTheme();

  useEffect(() => {
    fetchMessages();
  }, [id]);

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
    } finally {
      setLoading(false);
    }
  };

  const handleSendMessage = async () => {
    if (!id || !newMessage.trim()) return;

    try {
      const token = localStorage.getItem('token');
      const formData = new FormData();
      
      if (files) {
        Array.from(files).forEach((file) => {
          formData.append('files', file);
        });
      }

      const response = await fetch(API_ENDPOINTS.appeals.messages.create + '?appealId=' + id + '&message=' + newMessage, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formData
      });

      if (!response.ok) {
        throw new Error('Помилка відправки повідомлення');
      }

      const newMsg = await response.json();
      setMessages([...messages, newMsg]);
      setNewMessage('');
      setFiles(null);
      setSuccess('Повідомлення відправлено');
    } catch (err) {
      setError('Помилка при відправці повідомлення');
    }
  };

  const handleUpdateMessage = async () => {
    if (!editingMessage) return;

    try {
      const token = localStorage.getItem('token');
      const formData = new FormData();
      formData.append('messageId', editingMessage.id.toString());
      formData.append('message', editText);
      
      if (files) {
        Array.from(files).forEach((file) => {
          formData.append('files', file);
        });
      }

      const response = await fetch(API_ENDPOINTS.appeals.messages.update, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formData
      });

      if (!response.ok) {
        throw new Error('Помилка оновлення повідомлення');
      }

      const updatedMessage = await response.json();
      setMessages(messages.map(msg => 
        msg.id === updatedMessage.id ? updatedMessage : msg
      ));
      setEditDialogOpen(false);
      setEditingMessage(null);
      setEditText('');
      setFiles(null);
      setSuccess('Повідомлення оновлено');
    } catch (err) {
      setError('Помилка при оновленні повідомлення');
    }
  };

  const handleDeleteMessage = async (messageId: number) => {
    try {
      const token = localStorage.getItem('token');
      const response = await fetch(API_ENDPOINTS.appeals.messages.delete(messageId), {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        throw new Error('Помилка видалення повідомлення');
      }

      setMessages(messages.filter(msg => msg.id !== messageId));
      setSuccess('Повідомлення видалено');
    } catch (err) {
      setError('Помилка при видаленні повідомлення');
    }
  };

  const handleEditClick = (message: Message) => {
    setEditingMessage(message);
    setEditText(message.message);
    setEditDialogOpen(true);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFiles(e.target.files);
    }
  };

  const handleAddFiles = async () => {
    if (!selectedMessage || !files) return;

    try {
      const token = localStorage.getItem('token');
      const formData = new FormData();
      
      Array.from(files).forEach((file) => {
        formData.append('files', file);
      });

      const response = await fetch(API_ENDPOINTS.appeals.messages.files.addFiles(selectedMessage.id), {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formData
      });

      if (!response.ok) {
        throw new Error('Помилка додавання файлів');
      }

      const updatedMessage = await response.json();
      setMessages(messages.map(msg => 
        msg.id === updatedMessage.id ? updatedMessage : msg
      ));
      setAddFileDialogOpen(false);
      setSelectedMessage(null);
      setFiles(null);
      setSuccess('Файли додані');
    } catch (err) {
      setError('Помилка при додаванні файлів');
    }
  };

  const handleAddFilesClick = (message: Message) => {
    setSelectedMessage(message);
    setAddFileDialogOpen(true);
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Container maxWidth="md">
      <Box sx={{ mb: 3, display: 'flex', alignItems: 'center', gap: 2 }}>
        <IconButton onClick={() => navigate('/appeals')}>
          <ArrowBack />
        </IconButton>
        <Typography variant="h4" component="h1">
          Повідомлення звернення #{id}
        </Typography>
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

      <Paper 
        elevation={3} 
        sx={{ 
          p: 3, 
          mb: 3,
          borderRadius: 2,
          backgroundColor: theme.palette.background.default
        }}
      >
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          <TextField
            fullWidth
            multiline
            rows={3}
            value={newMessage}
            onChange={(e) => setNewMessage(e.target.value)}
            placeholder="Введіть ваше повідомлення..."
            variant="outlined"
            sx={{
              '& .MuiOutlinedInput-root': {
                borderRadius: 2
              }
            }}
          />
          <Box sx={{ display: 'flex', gap: 1, alignItems: 'center' }}>
            <input
              accept="image/*,.pdf,.doc,.docx"
              style={{ display: 'none' }}
              id="file-upload"
              type="file"
              multiple
              onChange={handleFileChange}
            />
            <label htmlFor="file-upload">
              <Button
                component="span"
                variant="outlined"
                startIcon={<CloudUploadIcon />}
                sx={{ borderRadius: 2 }}
              >
                {files ? `${files.length} файлів вибрано` : 'Завантажити файли'}
              </Button>
            </label>
            {files && (
              <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                (Опціонально)
              </Typography>
            )}
            <Box sx={{ flexGrow: 1 }} />
            <Button
              variant="contained"
              onClick={handleSendMessage}
              disabled={!newMessage.trim()}
              startIcon={<SendIcon />}
              sx={{ borderRadius: 2 }}
            >
              Відправити
            </Button>
          </Box>
        </Box>
      </Paper>

      <Paper 
        elevation={3} 
        sx={{ 
          p: 3,
          borderRadius: 2,
          backgroundColor: theme.palette.background.default
        }}
      >
        <List>
          {messages.map((message, index) => (
            <Box key={message.id}>
              <ListItem 
                sx={{ 
                  display: 'flex', 
                  flexDirection: 'column', 
                  alignItems: 'flex-start',
                  py: 2
                }}
              >
                <Box sx={{ width: '100%', display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Avatar sx={{ bgcolor: 'primary.main' }}>
                      {message.userId}
                    </Avatar>
                    <Box>
                      <Typography variant="subtitle2" component="div">
                        Користувач #{message.userId}
                      </Typography>
                      <Typography variant="caption" color="text.secondary">
                        {new Date(message.createdAt).toLocaleString()}
                      </Typography>
                    </Box>
                  </Box>
                  <Box>
                    <IconButton 
                      size="small"
                      onClick={() => handleAddFilesClick(message)}
                      sx={{ mr: 1 }}
                    >
                      <CloudUploadIcon />
                    </IconButton>
                    <IconButton 
                      size="small"
                      onClick={() => handleEditClick(message)}
                      sx={{ mr: 1 }}
                    >
                      <EditIcon />
                    </IconButton>
                    <IconButton 
                      size="small"
                      onClick={() => handleDeleteMessage(message.id)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Box>
                </Box>
                <Typography 
                  variant="body1" 
                  sx={{ 
                    mt: 1, 
                    ml: 7,
                    whiteSpace: 'pre-wrap',
                    wordBreak: 'break-word'
                  }}
                >
                  {message.message}
                </Typography>
                {message.files && message.files.length > 0 && (
                  <Box sx={{ mt: 1, ml: 7, display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                    {message.files.map((file) => (
                      <Chip
                        key={file.id}
                        icon={<AttachFileIcon />}
                        label={file.relativePath.split('/').pop()}
                        size="small"
                        variant="outlined"
                        sx={{ borderRadius: 1 }}
                      />
                    ))}
                  </Box>
                )}
              </ListItem>
              {index < messages.length - 1 && <Divider />}
            </Box>
          ))}
        </List>
      </Paper>

      <Dialog 
        open={editDialogOpen} 
        onClose={() => setEditDialogOpen(false)}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>Редагувати повідомлення</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Текст повідомлення"
            fullWidth
            multiline
            rows={4}
            value={editText}
            onChange={(e) => setEditText(e.target.value)}
            sx={{ mt: 2 }}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setEditDialogOpen(false)}>Скасувати</Button>
          <Button onClick={handleUpdateMessage} variant="contained">
            Зберегти
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog 
        open={addFileDialogOpen} 
        onClose={() => setAddFileDialogOpen(false)}
        maxWidth="sm"
        fullWidth
      >
        <DialogTitle>Додати файли до повідомлення</DialogTitle>
        <DialogContent>
          <Box sx={{ mt: 2 }}>
            <input
              accept="image/*,.pdf,.doc,.docx"
              style={{ display: 'none' }}
              id="add-files-upload"
              type="file"
              multiple
              onChange={handleFileChange}
            />
            <label htmlFor="add-files-upload">
              <Button
                component="span"
                variant="outlined"
                startIcon={<CloudUploadIcon />}
                sx={{ borderRadius: 2 }}
              >
                {files ? `${files.length} файлів вибрано` : 'Виберіть файли'}
              </Button>
            </label>
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setAddFileDialogOpen(false)}>Скасувати</Button>
          <Button 
            onClick={handleAddFiles} 
            variant="contained"
            disabled={!files || files.length === 0}
          >
            Додати файли
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
} 