import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Box,
  Paper,
  Typography,
  Alert,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  CircularProgress,
  Chip,
  IconButton,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Grid
} from '@mui/material';
import { Visibility } from '@mui/icons-material';

interface Appeal {
  id: string;
  subject: string;
  state: number;
  createdAt: string;
  updatedAt: string;
  userId: string;
  userEmail: string;
}

const AdminAppeals = () => {
  const navigate = useNavigate();
  const [appeals, setAppeals] = useState<Appeal[]>([]);
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [statusFilter, setStatusFilter] = useState(0);

  useEffect(() => {
    const fetchAppeals = async () => {
      const token = localStorage.getItem('adminToken');
      if (!token) {
        navigate('/admin/login');
        return;
      }

      try {
        const response = await fetch('http://localhost:5217/1.0/Appeals/GetAppealsByAdmin', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        if (response.ok) {
          const data = await response.json();
          setAppeals(data);
        } else if (response.status === 401) {
          localStorage.removeItem('adminToken');
          navigate('/admin/login');
        } else {
          const data = await response.json();
          setError(data.message || 'Помилка при отриманні звернень');
        }
      } catch (err) {
        console.error('Помилка при отриманні звернень:', err);
        setError('Помилка сервера');
      } finally {
        setIsLoading(false);
      }
    };

    fetchAppeals();
  }, [navigate]);

  const handleStatusChange = async (appealId: string, state: number) => {
    const token = localStorage.getItem('adminToken');
    if (!token) return;

    try {
      const response = await fetch(`http://localhost:5217/1.0/Appeals/UpdateStatus/${appealId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ status: state })
      });

      if (response.ok) {
        setAppeals(prevAppeals =>
          prevAppeals.map(appeal =>
            appeal.id === appealId
              ? { ...appeal, state: state }
              : appeal
          )
        );
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при оновленні статусу');
      }
    } catch (err) {
      console.error('Помилка при оновленні статусу:', err);
      setError('Помилка сервера');
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString('uk-UA');
  };

  const getStatusColor = (state: number) => {
    switch (state) {
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

  const filteredAppeals = statusFilter === 0
    ? appeals
    : appeals.filter(appeal => appeal.state === statusFilter);

  if (isLoading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Container component="main" maxWidth="lg">
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
          <Typography component="h1" variant="h5" gutterBottom>
            Управління зверненнями
          </Typography>
          {error && (
            <Alert severity="error" sx={{ mt: 2, width: '100%' }}>
              {error}
            </Alert>
          )}

          <Grid container spacing={2} sx={{ width: '100%', mb: 3 }}>
            <Grid item xs={12} sm={6}>
              <FormControl fullWidth>
                <InputLabel>Фільтр за статусом</InputLabel>
                <Select
                  value={statusFilter}
                  label="Фільтр за статусом"
                  onChange={(e) => setStatusFilter(0)}
                >
                  <MenuItem value="all">Всі</MenuItem>
                  <MenuItem value="pending">В очікуванні</MenuItem>
                  <MenuItem value="in progress">В обробці</MenuItem>
                  <MenuItem value="completed">Завершені</MenuItem>
                </Select>
              </FormControl>
            </Grid>
          </Grid>
          
          <TableContainer sx={{ width: '100%' }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>ID</TableCell>
                  <TableCell>Користувач</TableCell>
                  <TableCell>Тема</TableCell>
                  <TableCell>Статус</TableCell>
                  <TableCell>Створено</TableCell>
                  <TableCell>Оновлено</TableCell>
                  <TableCell align="right">Дії</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {filteredAppeals.map((appeal) => (
                  <TableRow key={appeal.id}>
                    <TableCell>{appeal.id}</TableCell>
                    <TableCell>{appeal.userEmail}</TableCell>
                    <TableCell>{appeal.subject}</TableCell>
                    <TableCell>
                      <FormControl size="small" sx={{ minWidth: 120 }}>
                        <Select
                          value={appeal.state}
                          onChange={(e) => handleStatusChange(appeal.id, appeal.state)}
                          sx={{
                            '& .MuiSelect-select': {
                              color: getStatusColor(appeal.state),
                              fontWeight: 'bold'
                            }
                          }}
                        >
                          <MenuItem value="pending">В очікуванні</MenuItem>
                          <MenuItem value="in progress">В обробці</MenuItem>
                          <MenuItem value="completed">Завершено</MenuItem>
                        </Select>
                      </FormControl>
                    </TableCell>
                    <TableCell>{formatDate(appeal.createdAt)}</TableCell>
                    <TableCell>{formatDate(appeal.updatedAt)}</TableCell>
                    <TableCell align="right">
                      <IconButton
                        color="primary"
                        onClick={() => navigate(`/appeal/${appeal.id}`)}
                        size="small"
                      >
                        <Visibility />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
      </Box>
    </Container>
  );
};

export default AdminAppeals; 