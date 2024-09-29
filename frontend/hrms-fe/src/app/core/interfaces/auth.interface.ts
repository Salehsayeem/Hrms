export interface LoginRequest {
  phone: string;
  password: string;
}

export interface LoginResponse {
  statusCode: number;
  message: string;
  succeed: boolean;
  data: {
    token: string;
    permissions: Permission[];
  };
}

export interface Permission {
  firstLevelMenuId: number;
  name: string;
  link: string;
  secondLevelMenu: SecondLevelMenu[];
}

export interface SecondLevelMenu {
  secondLevelMenuId: number;
  name: string;
  link: string;
}
export interface DecodedToken {
  UserId: string;
  Email: string;
  Phone: string;
  Name: string;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}
