export interface ILogin {
  email: string;
  password: string;
}

export interface IAuthSession{
  username: string;
  token: string;
  tokenType: string;
  role: string;
}
