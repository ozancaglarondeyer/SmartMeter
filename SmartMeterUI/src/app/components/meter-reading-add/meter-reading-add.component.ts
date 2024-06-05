import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MeterService } from '../../services/meter.service';

@Component({
  selector: 'app-meter-reading-add',
  templateUrl: './meter-reading-add.component.html',
  styleUrl: './meter-reading-add.component.css'
})
export class MeterReadingAddComponent {
  meterForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<MeterReadingAddComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { meterId: string },
    private fb: FormBuilder,
    private meterService: MeterService
  ) {
    this.meterForm = this.fb.group({
      meterId: [this.data.meterId, Validators.required],
      readingTime: ['', Validators.required],
      lastIndex: ['', Validators.required],
      voltage: ['', Validators.required],
      power: ['', Validators.required]
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.meterForm.valid) {
      this.meterService.addMeter(this.meterForm.value).subscribe(response => {
        this.dialogRef.close(response);
      });
    }
  }
}
