export const API_URL = 'http://localhost:5217/';
export const API_VERSION = '1.0';
export const API_BASE_URL = API_URL + API_VERSION;

export const API_ENDPOINTS = {
  admins: {
    create: `${API_BASE_URL}/Admins/Create`,
    login: `${API_BASE_URL}/Admins/Login`,
    setupPassword: `${API_BASE_URL}/Admins/SetupPassword`,
    delete: (adminId: number) => `${API_BASE_URL}/Admins/Delete?adminId=${adminId}`,
    recoveryPassword: `${API_BASE_URL}/Admins/RecoveryPassword`,
    checkRecoveryCode: `${API_BASE_URL}/Admins/CheckRecoveryCode`,
    changePassword: `${API_BASE_URL}/Admins/ChangePassword`,
    changeOldPassword: `${API_BASE_URL}/Admins/ChangeOldPassword`,
    getAdmins: `${API_BASE_URL}/Admins/GetAdmins`,
    getUsers: `${API_BASE_URL}/Admins/GetUsers`,
  },
  users: {
    registration: `${API_BASE_URL}/Users/Registration`,
    registrationEmail: `${API_BASE_URL}/Users/RegistrationEmail`,
    login: `${API_BASE_URL}/Users/Login`,
    recoveryPassword: `${API_BASE_URL}/Users/RecoveryPassword`,
    checkRecoveryCode: `${API_BASE_URL}/Users/CheckRecoveryCode`,
    changePassword: `${API_BASE_URL}/Users/ChangePassword`,
    changeOldPassword: `${API_BASE_URL}/Users/ChangeOldPassword`,
    delete: `${API_BASE_URL}/Users/Delete`,
  },
  appeals: {
    create: `${API_BASE_URL}/Appeals/Create`,
    getAppealsByUser: (since: number, count: number) => `${API_BASE_URL}/Appeals/GetAppealsByUser?since=${since}&count=${count}`,
    getAppealsByAdmin: (since: number, count: number) => `${API_BASE_URL}/Appeals/GetAppealsByAdmin?since=${since}&count=${count}`,
    messages: {
      list: (appealId: number) => `${API_BASE_URL}/AppealMessage/Get?appealId=${appealId}&since=0&count=100`,
      create: `${API_BASE_URL}/AppealMessage/Create`,
      update: `${API_BASE_URL}/AppealMessage/Update`,
      delete: (messageId: number) => `${API_BASE_URL}/AppealMessage/Delete?messageId=${messageId}`,
      files: {
        addFiles: (messageId: number) => `${API_BASE_URL}/AppealFile/Create?messageId=${messageId}`
      }
    }
  }
}; 