// Hostel Module DTOs

export interface HostelRoomDto {
  id: string;
  roomNumber: string;
  blockName: string;
  floorNumber: string;
  capacity: number;
  occupiedBeds: number;
  availableBeds: number;
  roomType: string;
  monthlyRent?: number | null;
  facilities?: string | null;
  status: string;
  notes?: string | null;
  createdOnUtc: string;
  createdBy?: string | null;
}

export interface HostelRoomsListResponse {
  rooms: HostelRoomDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// Request DTOs
export interface CreateHostelRoomPayload {
  roomNumber: string;
  blockName: string;
  floorNumber: string;
  capacity: number;
  roomType: string;
  monthlyRent?: number | null;
  facilities?: string | null;
  notes?: string | null;
  createdBy?: string | null;
}

export interface UpdateHostelRoomPayload {
  blockName?: string | null;
  floorNumber?: string | null;
  capacity?: number | null;
  roomType?: string | null;
  monthlyRent?: number | null;
  facilities?: string | null;
  notes?: string | null;
}

export interface AllocateRoomPayload {
  roomId: string;
  studentId: string;
  allocationDate: string;
  monthlyRent?: number | null;
  bedNumber?: string | null;
  remarks?: string | null;
  allocatedBy?: string | null;
}

export interface VacateRoomPayload {
  allocationId: string;
  vacatedDate: string;
  vacatedBy?: string | null;
  remarks?: string | null;
}

export interface UpdateHostelRoomPayload {
  blockName?: string | null;
  floorNumber?: string | null;
  capacity?: number | null;
  roomType?: string | null;
  monthlyRent?: number | null;
  facilities?: string | null;
  notes?: string | null;
}

