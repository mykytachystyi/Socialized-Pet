import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Box, Typography, TextField, Button, Alert, Paper } from '@mui/material';
import { API_ENDPOINTS } from '../../ApiEndPoints';
const AdminVerifyAndReset = () => {
  const navigate = useNavigate();
  const [recoveryCode, setRecoveryCode] = useState('');
  const [userPassword, setUserPassword] = useState('');
  const [userConfirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isCodeVerified, setIsCodeVerified] = useState(false);
  const [recoveryToken, setRecoveryToken] = useState('');
  const [email, setEmail] = useState('');

  useEffect(() => {
    const savedEmail = localStorage.getItem('adminRecoveryEmail');
    if (!savedEmail) {
      navigate('/admin/recovery-password');
    } else {
      setEmail(savedEmail);
    }
  }, [navigate]);

  const handleVerifyCode = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    try {
      const response = await fetch(API_ENDPOINTS.admins.checkRecoveryCode, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ 
          email,
          recoveryCode 
        })
      });

      if (response.ok) {
        const data = await response.json();
        console.log('Отримано токен:', data.recoveryToken);
        setRecoveryToken(data.recoveryToken);
        setIsCodeVerified(true);
        setSuccess('Код підтверджено. Тепер встановіть новий пароль.');
      } else {
        const data = await response.json();
        setError(data.message || 'Невірний код відновлення');
      }
    } catch (err) {
      console.error('Помилка при перевірці коду:', err);
      setError('Помилка при перевірці коду');
    }
  };

  const handleResetPassword = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (userPassword !== userConfirmPassword) {
      setError('Нові паролі не співпадають');
      return;
    }

    if (!recoveryToken) {
      setError('Токен відновлення відсутній. Спробуйте ще раз перевірити код.');
      return;
    }
    try {
      const response = await fetch(API_ENDPOINTS.admins.changePassword, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ 
           recoveryToken,
           userPassword,
           userConfirmPassword
        })
      });

      if (response.ok) {
        localStorage.removeItem('adminRecoveryEmail');
        setSuccess('Пароль успішно змінено');
        setTimeout(() => {
          navigate('/admin/login');
        }, 2000);
      } else {
        const data = await response.json();
        setError(data.message || 'Помилка при зміні паролю');
      }
    } catch (err) {
      console.error('Помилка при зміні паролю:', err);
      setError('Помилка при зміні паролю');
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
            {isCodeVerified ? 'Встановлення нового паролю' : 'Верифікація коду'}
          </Typography>
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
          <Box component="form" onSubmit={isCodeVerified ? handleResetPassword : handleVerifyCode} sx={{ mt: 1, width: '100%' }}>
            {!isCodeVerified ? (
              <>
                <TextField
                  margin="normal"
                  required
                  fullWidth
                  id="code"
                  label="Код відновлення"
                  name="code"
                  autoComplete="off"
                  value={recoveryCode}
                  onChange={(e) => setRecoveryCode(e.target.value)}
                />
                <Button
                  type="submit"
                  fullWidth
                  variant="contained"
                  sx={{ mt: 3, mb: 2 }}
                >
                  Перевірити код
                </Button>
              </>
            ) : (
              <>
                <TextField
                  margin="normal"
                  required
                  fullWidth
                  name="newPassword"
                  label="Новий пароль"
                  type="password"
                  id="newPassword"
                  autoComplete="new-password"
                  value={userPassword}
                  onChange={(e) => setUserPassword(e.target.value)}
                />
                <TextField
                  margin="normal"
                  required
                  fullWidth
                  name="confirmPassword"
                  label="Підтвердіть новий пароль"
                  type="password"
                  id="confirmPassword"
                  autoComplete="new-password"
                  value={userConfirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
                <Button
                  type="submit"
                  fullWidth
                  variant="contained"
                  sx={{ mt: 3, mb: 2 }}
                >
                  Змінити пароль
                </Button>
              </>
            )}
            <Button
              fullWidth
              variant="text"
              onClick={() => navigate('/admin/login')}
            >
              Повернутися до входу
            </Button>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default AdminVerifyAndReset; 