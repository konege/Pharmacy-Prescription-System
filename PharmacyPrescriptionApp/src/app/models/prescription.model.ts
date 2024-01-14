export interface Prescription {
    pharmacyId: string;
    tcIdNo: string;
    fullName: string;
    medicines: Medicine[];
  }
  
  export interface Medicine {
    name: string;
    price: number;
  }
  