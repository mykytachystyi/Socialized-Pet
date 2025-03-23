import { Box, Container, Typography, Paper, Grid, Button } from '@mui/material';
import { Link } from 'react-router-dom';
import { Pets, Support, Security, Speed } from '@mui/icons-material';

const Home = () => {
  return (
    <Container maxWidth="lg">
      {/* Hero Section */}
      <Box
        sx={{
          mt: 8,
          mb: 6,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center',
        }}
      >
        <Typography variant="h2" component="h1" gutterBottom>
          Ласкаво просимо до Socialized Pet
        </Typography>
        <Typography variant="h5" color="text.secondary" paragraph>
          Ваша платформа для звернень та підтримки домашніх улюбленців
        </Typography>
        <Box sx={{ mt: 4 }}>
          <Button
            component={Link}
            to="/register"
            variant="contained"
            size="large"
            sx={{ mr: 2 }}
          >
            Зареєструватися
          </Button>
          <Button
            component={Link}
            to="/login"
            variant="outlined"
            size="large"
            sx={{ ml: 2 }}
          >
            Увійти
          </Button>
        </Box>
      </Box>

      {/* Features Section */}
      <Grid container spacing={4} sx={{ mb: 8 }}>
        <Grid item xs={12} md={3}>
          <Paper
            elevation={3}
            sx={{
              p: 3,
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              textAlign: 'center',
            }}
          >
            <Pets sx={{ fontSize: 48, color: 'primary.main', mb: 2 }} />
            <Typography variant="h6" gutterBottom>
              Домашні улюбленці
            </Typography>
            <Typography color="text.secondary">
              Створюйте звернення та отримуйте підтримку для ваших улюбленців
            </Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <Paper
            elevation={3}
            sx={{
              p: 3,
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              textAlign: 'center',
            }}
          >
            <Support sx={{ fontSize: 48, color: 'primary.main', mb: 2 }} />
            <Typography variant="h6" gutterBottom>
              Підтримка 24/7
            </Typography>
            <Typography color="text.secondary">
              Наші адміністратори завжди готові допомогти вам
            </Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <Paper
            elevation={3}
            sx={{
              p: 3,
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              textAlign: 'center',
            }}
          >
            <Security sx={{ fontSize: 48, color: 'primary.main', mb: 2 }} />
            <Typography variant="h6" gutterBottom>
              Безпека
            </Typography>
            <Typography color="text.secondary">
              Ваші дані захищені та конфіденційні
            </Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <Paper
            elevation={3}
            sx={{
              p: 3,
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              textAlign: 'center',
            }}
          >
            <Speed sx={{ fontSize: 48, color: 'primary.main', mb: 2 }} />
            <Typography variant="h6" gutterBottom>
              Швидкість
            </Typography>
            <Typography color="text.secondary">
              Швидкі відповіді на ваші звернення
            </Typography>
          </Paper>
        </Grid>
      </Grid>

      {/* Call to Action */}
      <Box
        sx={{
          mt: 8,
          mb: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center',
        }}
      >
        <Paper
          elevation={3}
          sx={{
            p: 6,
            width: '100%',
            background: 'linear-gradient(45deg, #2196F3 30%, #21CBF3 90%)',
            color: 'white',
          }}
        >
          <Typography variant="h4" gutterBottom>
            Готові почати?
          </Typography>
          <Typography variant="h6" paragraph>
            Приєднуйтесь до нашої спільноти та отримуйте підтримку для ваших улюбленців
          </Typography>
          <Button
            component={Link}
            to="/register"
            variant="contained"
            size="large"
            sx={{
              mt: 2,
              bgcolor: 'white',
              color: 'primary.main',
              '&:hover': {
                bgcolor: 'grey.100',
              },
            }}
          >
            Зареєструватися зараз
          </Button>
        </Paper>
      </Box>
    </Container>
  );
};

export default Home; 