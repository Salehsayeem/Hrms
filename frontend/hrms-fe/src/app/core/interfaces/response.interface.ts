export interface CommonResponse {
  statusCode: number;
  succeed: boolean;
  message: string;
  data:any;
}

export interface HouseTable{
  sl: number;
  id: number;
  name: string;
  address: string;
  contact:string;
}
export interface CommonDdl {
  value: number;
  label: string;
  description:string;
}
