import { useState, useEffect } from 'react';
import { 
  Container, Box, TextField, Button, Typography, 
  Paper, Alert, CircularProgress, Stepper, Step, 
  StepLabel 
} from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import { API_ENDPOINTS } from '../../ApiEndPoints';
const VerifyAndReset = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [email, setEmail] = useState('');
  const [recoveryCode, setRecoveryCode] = useState('');
  const [userPassword, setUserPassword] = useState('');
  const [userConfirmPassword, setUserConfirmPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [activeStep, setActiveStep] = useState(0);
  const [recoveryToken, setRecoveryToken] = useState('');

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const emailParam = params.get('email');
    if (!emailParam) {
      navigate('/recovery-password');
    } else {
      setEmail(emailParam);
    }
  }, [location, navigate]);

  const handleVerifyCode = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    try {
      const response = await fetch(API_ENDPOINTS.users.checkRecoveryCode, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, recoveryCode })
      });

      if (response.ok) {
        const data = await response.json();
        setRecoveryToken(data.recoveryToken);
        setActiveStep(1);
      } else {
        const data = await response.json();
        setError(data.message || 'Невірний код');
      }
    } catch (err) {
      setError('Помилка сервера');
    } finally {
      setIsLoading(false);
    }
  };

  const handleResetPassword = async (e: React.FormEvent) => {
    e.preventDefault();
    if (userPassword !== userConfirmPassword) {
      setError('Паролі не співпадають');
      return;
    }

    setIsLoading(true);
    setError('');

    try {
      const response = await fetch(API_ENDPOINTS.users.changePassword, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          recoveryToken,
          userPassword,
          userConfirmPassword
        })
      });

      if (response.ok) {
        navigate('/login', { 
          state: { message: 'Пароль успішно змінено' } 
        });
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при зміні паролю');
      }
    } catch (err) {
      setError('Помилка сервера');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container component="main" maxWidth="xs">
      <Paper elevation={3} sx={{ mt: 8, p: 4 }}>
        <Stepper activeStep={activeStep} sx={{ mb: 4 }}>
          <Step>
            <StepLabel>Код підтвердження</StepLabel>
          </Step>
          <Step>
            <StepLabel>Новий пароль</StepLabel>
          </Step>
        </Stepper>

        {activeStep === 0 ? (
          <Box component="form" onSubmit={handleVerifyCode}>
            <Typography variant="h6" align="center">
              Введіть код підтвердження
            </Typography>
            <Typography variant="body2" sx={{ mt: 1, mb: 2 }} align="center">
              Код відправлено на {email}
            </Typography>
            
            <TextField
              fullWidth
              required
              label="Код підтвердження"
              value={recoveryCode}
              onChange={(e) => setRecoveryCode(e.target.value.replace(/\D/g, ''))}
              margin="normal"
              inputProps={{ maxLength: 6 }}
            />
            
            {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
            
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3 }}
              disabled={isLoading || recoveryCode.length !== 6}
            >
              {isLoading ? <CircularProgress size={24} /> : 'Перевірити код'}
            </Button>
          </Box>
        ) : (
          <Box component="form" onSubmit={handleResetPassword}>
            <Typography variant="h6" align="center">
              Встановіть новий пароль
            </Typography>
            
            <TextField
              fullWidth
              required
              type="password"
              label="Новий пароль"
              value={userPassword}
              onChange={(e) => setUserPassword(e.target.value)}
              margin="normal"
            />
            
            <TextField
              fullWidth
              required
              type="password"
              label="Підтвердження паролю"
              value={userConfirmPassword}
              onChange={(e) => setUserConfirmPassword(e.target.value)}
              margin="normal"
            />
            
            {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
            
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3 }}
              disabled={isLoading}
            >
              {isLoading ? <CircularProgress size={24} /> : 'Змінити пароль'}
            </Button>
          </Box>
        )}
      </Paper>
    </Container>
  );
};

export default VerifyAndReset;