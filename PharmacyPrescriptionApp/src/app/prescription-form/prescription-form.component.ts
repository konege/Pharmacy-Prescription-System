import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, ReactiveFormsModule } from '@angular/forms';
import { PrescriptionService } from '../services/prescription.service';
import { Validators } from '@angular/forms';

@Component({
  selector: 'app-prescription-form',
  templateUrl: './prescription-form.component.html',
  styleUrls: ['./prescription-form.component.scss'],
  imports: [ReactiveFormsModule], // Import ReactiveFormsModule here
  standalone: true
})
export class PrescriptionFormComponent implements OnInit {
  prescriptionForm: FormGroup = this.fb.group({
    pharmacyId: ['', Validators.required],
    tcIdNo: ['', [Validators.required, Validators.pattern(/^\d{11}$/)]],
    fullName: ['', Validators.required],
    medicines: this.fb.array([])
  });
  constructor(
    private fb: FormBuilder,
    private prescriptionService: PrescriptionService
  ) { }

  ngOnInit(): void {
    this.prescriptionForm = this.fb.group({
      pharmacyId: ['', Validators.required],
      tcIdNo: ['', [Validators.required, Validators.pattern(/^\d{11}$/)]], // Assuming Turkish ID No. of 11 digits
      fullName: ['', Validators.required],
      medicines: this.fb.array([])
    });
  }

  get medicines(): FormArray {
    return this.prescriptionForm.get('medicines') as FormArray;
  }

  addMedicine(): void {
    const medicineForm = this.fb.group({
      name: [''],
      price: [0]
    });
    this.medicines.push(medicineForm);
  }

  removeMedicine(index: number): void {
    this.medicines.removeAt(index);
  }

  onSubmit(): void {
    if (this.prescriptionForm.valid) {
      this.prescriptionService.submitPrescription(this.prescriptionForm.value).subscribe({
        next: (res) => {
          // Handle response
          console.log('Prescription submitted', res);
        },
        error: (err) => {
          // Handle error
          console.error('Error submitting prescription', err);
        }
      });
    }
  }
}