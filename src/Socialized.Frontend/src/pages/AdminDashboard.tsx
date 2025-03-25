import { Container, Box, Typography, Paper, Grid, Card, CardContent, Button } from '@mui/material';
import { useNavigate, Link } from 'react-router-dom';
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
          <Grid item xs={12} sm={6} md={4}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Користувачі
                </Typography>
                <Button
                  variant="contained"
                  component={Link}
                  to="/admin/users"
                  fullWidth
                >
                  Переглянути користувачів
                </Button>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Адміністратори
                </Typography>
                <Button
                  variant="contained"
                  component={Link}
                  to="/admin/admins"
                  fullWidth
                >
                  Переглянути адміністраторів
                </Button>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Звернення
                </Typography>
                <Button
                  variant="contained"
                  component={Link}
                  to="/admin/appeals"
                  fullWidth
                >
                  Переглянути звернення
                </Button>
              </CardContent>
            </Card>
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