// Transport Module DTOs

export interface VehicleDto {
  id: string;
  vehicleNumber: string;
  vehicleType: string;
  make: string;
  model: string;
  year?: number | null;
  color?: string | null;
  capacity: number;
  registrationNumber?: string | null;
  registrationExpiryDate?: string | null;
  insuranceNumber?: string | null;
  insuranceExpiryDate?: string | null;
  driverName?: string | null;
  driverContactNumber?: string | null;
  route?: string | null;
  status: string;
  notes?: string | null;
  createdOnUtc: string;
  createdBy?: string | null;
}

export interface VehiclesListResponse {
  vehicles: VehicleDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// Request DTOs
export interface CreateVehiclePayload {
  vehicleNumber: string;
  vehicleType: string;
  make: string;
  model: string;
  capacity: number;
  color?: string | null;
  year?: number | null;
  registrationNumber?: string | null;
  registrationExpiryDate?: string | null;
  insuranceNumber?: string | null;
  insuranceExpiryDate?: string | null;
  driverName?: string | null;
  driverContactNumber?: string | null;
  route?: string | null;
  notes?: string | null;
  createdBy?: string | null;
}

export interface UpdateVehiclePayload {
  vehicleType?: string | null;
  make?: string | null;
  model?: string | null;
  capacity?: number | null;
  color?: string | null;
  year?: number | null;
  registrationNumber?: string | null;
  registrationExpiryDate?: string | null;
  insuranceNumber?: string | null;
  insuranceExpiryDate?: string | null;
  driverName?: string | null;
  driverContactNumber?: string | null;
  route?: string | null;
  notes?: string | null;
}

