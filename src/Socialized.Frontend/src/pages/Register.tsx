import { useState } from 'react';
import { 
  Container, 
  Box, 
  TextField, 
  Button, 
  Typography, 
  Paper,
  Grid,
  Link as MuiLink,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  CircularProgress
} from '@mui/material';
import { useNavigate, Link } from 'react-router-dom';
import { API_ENDPOINTS } from '../config';
const Register = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: ''
  });
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [showResendDialog, setShowResendDialog] = useState(false);
  const [resendEmail, setResendEmail] = useState('');
  const [resendSuccess, setResendSuccess] = useState('');
  const [isResending, setIsResending] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);

    try {
      const response = await fetch(API_ENDPOINTS.users.registration, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData)
      });

      if (response.ok) {
        navigate('/login');
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при реєстрації');
      }
    } catch (err) {
      setError('Помилка сервера');
    } finally {
      setIsLoading(false);
    }
  };

  const handleResendConfirmation = async () => {
    if (!resendEmail) {
      setError('Будь ласка, введіть email');
      return;
    }
    
    setIsResending(true);
    setError('');
    setResendSuccess('');

    try {
        const response = await fetch(API_ENDPOINTS.users.registrationEmail + '?email=' + resendEmail, {
        method: 'GET',
    });

      if (response.ok) {
        setResendSuccess('Лист підтвердження успішно надіслано');
        setTimeout(() => {
          setShowResendDialog(false);
          setResendEmail('');
          setResendSuccess('');
        }, 3000);
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при надсиланні листа');
      }
    } catch (err) {
      setError('Помилка сервера');
    } finally {
      setIsResending(false);
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
            Реєстрація
          </Typography>
          
          <Box component="form" onSubmit={handleSubmit} sx={{ mt: 3 }}>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  required
                  fullWidth
                  label="Ім'я"
                  name="firstName"
                  value={formData.firstName}
                  onChange={(e) => setFormData({...formData, firstName: e.target.value})}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  required
                  fullWidth
                  label="Прізвище"
                  name="lastName"
                  value={formData.lastName}
                  onChange={(e) => setFormData({...formData, lastName: e.target.value})}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  label="Email"
                  name="email"
                  type="email"
                  value={formData.email}
                  onChange={(e) => setFormData({...formData, email: e.target.value})}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  required
                  fullWidth
                  label="Пароль"
                  name="password"
                  type="password"
                  value={formData.password}
                  onChange={(e) => setFormData({...formData, password: e.target.value})}
                />
              </Grid>
            </Grid>

            {error && (
              <Alert severity="error" sx={{ mt: 2 }}>
                {error}
              </Alert>
            )}

            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
              disabled={isLoading}
            >
              {isLoading ? <CircularProgress size={24} /> : 'Зареєструватися'}
            </Button>

            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <MuiLink component={Link} to="/login" variant="body2">
                  Вже маєте акаунт? Увійти
                </MuiLink>
              </Grid>
              <Grid item xs={12} sm={6}>
                <MuiLink 
                  component="button"
                  variant="body2"
                  onClick={() => setShowResendDialog(true)}
                  sx={{ textAlign: 'right', width: '100%' }}
                >
                  Надіслати лист підтвердження повторно
                </MuiLink>
              </Grid>
            </Grid>
          </Box>
        </Paper>
      </Box>

      {/* Діалог для повторного надсилання листа */}
      <Dialog 
        open={showResendDialog} 
        onClose={() => {
          setShowResendDialog(false);
          setResendEmail('');
          setError('');
          setResendSuccess('');
        }}
      >
        <DialogTitle>
          Повторне надсилання листа підтвердження
        </DialogTitle>
        <DialogContent>
          <Typography variant="body2" sx={{ mb: 2 }}>
            Введіть email, який ви використовували при реєстрації
          </Typography>
          <TextField
            autoFocus
            margin="dense"
            label="Email"
            type="email"
            fullWidth
            variant="outlined"
            value={resendEmail}
            onChange={(e) => setResendEmail(e.target.value)}
            disabled={isResending}
          />
          {error && (
            <Alert severity="error" sx={{ mt: 2 }}>
              {error}
            </Alert>
          )}
          {resendSuccess && (
            <Alert severity="success" sx={{ mt: 2 }}>
              {resendSuccess}
            </Alert>
          )}
        </DialogContent>
        <DialogActions>
          <Button 
            onClick={() => {
              setShowResendDialog(false);
              setResendEmail('');
              setError('');
              setResendSuccess('');
            }}
            disabled={isResending}
          >
            Скасувати
          </Button>
          <Button 
            onClick={handleResendConfirmation}
            variant="contained"
            disabled={isResending}
          >
            {isResending ? <CircularProgress size={24} /> : 'Надіслати'}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default Register;