import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Box, Paper, Typography, Alert, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, CircularProgress, Chip, IconButton, Fab } from '@mui/material';
import { Add, Visibility } from '@mui/icons-material';
import { API_ENDPOINTS } from '../ApiEndPoints';
interface Appeal {
  id: string;
  subject: string;
  status: string;
  createdAt: string;
  updatedAt: string;
}

const UserAppeals = () => {
  const navigate = useNavigate();
  const [appeals, setAppeals] = useState<Appeal[]>([]);
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchAppeals = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        navigate('/login');
        return;
      }

      try {
        const response = await fetch(API_ENDPOINTS.appeals.getAppealsByUser(0, 100), {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        if (response.ok)   
        {
          const data = await response.json();
          setAppeals(data);
        } 
        else if (response.status === 401) 
        {
          localStorage.removeItem('token');
          navigate('/login');
        } 
        else 
        {
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

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString('uk-UA');
  };

  const getStatusColor = (status: string | undefined) => {
    if (!status) return 'default';
    
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
          <Typography component="h1" variant="h5" gutterBottom>
            Мої звернення
          </Typography>
          {error && (
            <Alert severity="error" sx={{ mt: 2, width: '100%' }}>
              {error}
            </Alert>
          )}
          
          <TableContainer sx={{ width: '100%', mt: 2 }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>ID</TableCell>
                  <TableCell>Тема</TableCell>
                  <TableCell>Статус</TableCell>
                  <TableCell>Створено</TableCell>
                  <TableCell>Оновлено</TableCell>
                  <TableCell align="right">Дія</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {appeals.map((appeal) => (
                  <TableRow key={appeal.id}>
                    <TableCell>{appeal.id}</TableCell>
                    <TableCell>{appeal.subject}</TableCell>
                    <TableCell>
                      <Chip 
                        label={appeal.status || 'Невідомий статус'} 
                        color={getStatusColor(appeal.status)}
                        size="small"
                      />
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
      <Fab
        color="primary"
        sx={{ position: 'fixed', bottom: 16, right: 16 }}
        onClick={() => navigate('/create-appeal')}
      >
        <Add />
      </Fab>
    </Container>
  );
};

export default UserAppeals;