interface RegisterData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

interface LoginResponse {
    token: string;
    // додайте інші поля, які повертає ваш API
  }
  
  interface LoginData {
    email: string;
    password: string;
  }
  
  export const authService = {
    async login(data: LoginData): Promise<LoginResponse> {
      const response = await fetch('http://localhost:5217/1.0./Users/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
      });
  
      if (!response.ok) {
        throw new Error('Помилка авторизації');
      }
  
      return response.json();
    },
    async register(data: RegisterData): Promise<void> {
      const response = await fetch('http://localhost:5217/1.0/Users/Registration', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Помилка реєстрації');
      }
    },
    async recoveryPassword(email: string): Promise<void> {
      const response = await fetch('http://localhost:5217/1.0/Users/RecoveryPassword?email=' + email, {
        method: 'GET'
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Помилка відновлення паролю');
      }
    }
  };