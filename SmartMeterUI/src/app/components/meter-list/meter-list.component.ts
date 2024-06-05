import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MeterService } from '../../services/meter.service';
import { MatDialog } from '@angular/material/dialog';
import { MeterReadingAddComponent } from '../meter-reading-add/meter-reading-add.component';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-meter-list',
  templateUrl: './meter-list.component.html',
  styleUrls: ['./meter-list.component.css']
})
export class MeterListComponent implements OnInit {
  displayedColumns: string[] = ['serialNumber', 'lastReadingTime', 'lastReadingVoltage', 'lastReadingPower', 'lastIndex', 'actions'];
  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  filterForm: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private meterService: MeterService,
    private fb: FormBuilder,
    public dialog: MatDialog
  ) {
    this.filterForm = this.fb.group({
      serialNumber: [''],
      lastReadingStartDate: [''],
      lastReadingEndDate: ['']
    });
  }

  ngOnInit(): void {
    this.fetchMeters();
  }

  fetchMeters(): void {
    const filterValues = this.filterForm.value;

    // Tarihleri ISO formatına çevir
    if (filterValues.lastReadingStartDate) {
      filterValues.lastReadingStartDate = new Date(filterValues.lastReadingStartDate).toISOString();
    }
    if (filterValues.lastReadingEndDate) {
      filterValues.lastReadingEndDate = new Date(filterValues.lastReadingEndDate).toISOString();
    }

    this.meterService.getMeters(filterValues).subscribe(data => {
      this.dataSource = new MatTableDataSource(data.value);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter(): void {
    this.fetchMeters();
  }

  addMeterReading(meterId: string): void {
    const dialogRef = this.dialog.open(MeterReadingAddComponent, {
      width: '400px',
      data: { meterId }
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleString();
  }
}
