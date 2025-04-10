import { Box, Container, Typography, Button } from '@mui/material';
import { Link } from 'react-router-dom';
import './Footer.css';

const Footer = () => {
  return (
    <Box component="footer" className="footer">
      <Container maxWidth="lg">
        <Box className="footer-content">
          <Typography variant="body2" color="text.secondary" className="copyright">
            © {new Date().getFullYear()} Socialized Pet. Всі права захищені.
          </Typography>
          <Box className="footer-links">
            <Button color="inherit" component={Link} to="/privacy">
              Політика конфіденційності
            </Button>
            <Button color="inherit" component={Link} to="/terms">
              Умови використання
            </Button>
          </Box>
        </Box>
      </Container>
    </Box>
  );
};

export default Footer; 