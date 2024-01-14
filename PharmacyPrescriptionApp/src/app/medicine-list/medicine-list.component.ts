import { Component, OnInit } from '@angular/core';
import { MedicineService } from '../services/medicine.service'; 
import { Medicine } from '../models/prescription.model'; 

@Component({
  selector: 'app-medicine-list',
  templateUrl: './medicine-list.component.html',
  styleUrls: ['./medicine-list.component.scss']
})
export class MedicineListComponent implements OnInit {
  medicines: Medicine[] = [];

  constructor(private medicineService: MedicineService) { }

  ngOnInit(): void {
    // Implement logic to get medicines, for example, on input change
  }

  addToPrescription(medicine: Medicine): void {
    // Logic to add the selected medicine to the prescription
  }
}
