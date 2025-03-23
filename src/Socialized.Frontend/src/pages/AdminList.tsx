import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Container, 
  Typography, 
  Table, 
  TableBody, 
  TableCell, 
  TableContainer, 
  TableHead, 
  TableRow, 
  Paper,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Alert
} from '@mui/material';

interface Admin {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
}

const AdminList = () => {
  const navigate = useNavigate();
  const [admins, setAdmins] = useState<Admin[]>([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedAdmin, setSelectedAdmin] = useState<Admin | null>(null);

  useEffect(() => {
    const adminToken = localStorage.getItem('adminToken');
    if (!adminToken) {
      navigate('/admin/login');
      return;
    }

    fetchAdmins();
  }, [navigate]);

  const fetchAdmins = async () => {
    try {
      const response = await fetch('http://localhost:5217/1.0/Admins/GetAdmins', {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('adminToken')}`
        }
      });

      if (response.ok) {
        const data = await response.json();
        console.log('Отримані дані адміністраторів:', data);
        setAdmins(data);
      } else {
        setError('Помилка при отриманні списку адміністраторів');
      }
    } catch (err) {
      setError('Помилка при отриманні списку адміністраторів');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteClick = (admin: Admin) => {
    console.log('Вибраний адміністратор для видалення:', admin);
    setSelectedAdmin(admin);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!selectedAdmin) return;

    try {
      console.log('Відправка запиту на видалення адміністратора:', selectedAdmin);
      const response = await fetch(`http://localhost:5217/1.0/Admins/Delete?adminId=${selectedAdmin.id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('adminToken')}`
        }
      });

      if (response.ok) {
        setAdmins(admins.filter(admin => admin.id !== selectedAdmin.id));
        setDeleteDialogOpen(false);
        setSelectedAdmin(null);
      } else {
        const errorData = await response.json();
        console.error('Помилка при видаленні:', errorData);
        setError('Помилка при видаленні адміністратора');
      }
    } catch (err) {
      console.error('Помилка при видаленні:', err);
      setError('Помилка при видаленні адміністратора');
    }
  };

  const handleDeleteCancel = () => {
    setDeleteDialogOpen(false);
    setSelectedAdmin(null);
  };

  if (loading) {
    return <Typography>Завантаження...</Typography>;
  }

  return (
    <Container>
      <Typography variant="h4" component="h1" gutterBottom>
        Список адміністраторів
      </Typography>
      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Ім'я</TableCell>
              <TableCell>Прізвище</TableCell>
              <TableCell>Дії</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {admins.map((admin) => (
              <TableRow key={admin.id}>
                <TableCell>{admin.id}</TableCell>
                <TableCell>{admin.email}</TableCell>
                <TableCell>{admin.firstName}</TableCell>
                <TableCell>{admin.lastName}</TableCell>
                <TableCell>
                  <Button 
                    variant="contained" 
                    color="error"
                    onClick={() => handleDeleteClick(admin)}
                  >
                    Видалити
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={deleteDialogOpen} onClose={handleDeleteCancel}>
        <DialogTitle>Підтвердження видалення</DialogTitle>
        <DialogContent>
          <Typography>
            Ви впевнені, що хочете видалити адміністратора {selectedAdmin?.email}?
            Ця дія незворотна.
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel}>Скасувати</Button>
          <Button onClick={handleDeleteConfirm} color="error" variant="contained">
            Видалити
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default AdminList; 