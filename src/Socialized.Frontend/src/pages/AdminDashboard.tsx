import { Container, Box, Typography, Paper, Grid } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import PeopleIcon from '@mui/icons-material/People';
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';
import PersonAddIcon from '@mui/icons-material/PersonAdd';
import { useEffect } from 'react';

const AdminDashboard = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const adminToken = localStorage.getItem('adminToken');
    if (!adminToken) {
      navigate('/admin/login');
    }
  }, [navigate]);

  return (
    <Container component="main" maxWidth="lg">
      <Box sx={{ mt: 8, mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Панель адміністратора
        </Typography>
        
        <Grid container spacing={3}>
          <Grid item xs={12} md={4}>
            <Paper 
              sx={{ 
                p: 3, 
                display: 'flex', 
                flexDirection: 'column',
                alignItems: 'center',
                cursor: 'pointer'
              }}
              onClick={() => navigate('/admin/users')}
            >
              <PeopleIcon sx={{ fontSize: 60, mb: 2, color: 'primary.main' }} />
              <Typography variant="h6" gutterBottom>
                Користувачі
              </Typography>
              <Typography variant="body2" color="text.secondary" align="center">
                Переглянути список всіх користувачів
              </Typography>
            </Paper>
          </Grid>

          <Grid item xs={12} md={4}>
            <Paper 
              sx={{ 
                p: 3, 
                display: 'flex', 
                flexDirection: 'column',
                alignItems: 'center',
                cursor: 'pointer'
              }}
              onClick={() => navigate('/admin/admins')}
            >
              <AdminPanelSettingsIcon sx={{ fontSize: 60, mb: 2, color: 'primary.main' }} />
              <Typography variant="h6" gutterBottom>
                Адміністратори
              </Typography>
              <Typography variant="body2" color="text.secondary" align="center">
                Переглянути список адміністраторів
              </Typography>
            </Paper>
          </Grid>

          <Grid item xs={12} md={4}>
            <Paper 
              sx={{ 
                p: 3, 
                display: 'flex', 
                flexDirection: 'column',
                alignItems: 'center',
                cursor: 'pointer'
              }}
              onClick={() => navigate('/admin/create')}
            >
              <PersonAddIcon sx={{ fontSize: 60, mb: 2, color: 'primary.main' }} />
              <Typography variant="h6" gutterBottom>
                Створити адміністратора
              </Typography>
              <Typography variant="body2" color="text.secondary" align="center">
                Додати нового адміністратора
              </Typography>
            </Paper>
          </Grid>
        </Grid>
      </Box>
    </Container>
  );
};

export default AdminDashboard; 