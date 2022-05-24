export interface ILogin {
  email: string;
  password: string;
}

export interface IAuthSession{
  userId: string;
  username: string;
  token: string;
  tokenType: string;
  role: string;
}
