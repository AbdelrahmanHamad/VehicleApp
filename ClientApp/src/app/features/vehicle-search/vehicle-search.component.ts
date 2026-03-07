import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { VehicleService } from '../../core/services/vehicle.service';
import { VehicleMake, VehicleModel, VehicleType } from '../../core/models/vehicle.model';
import { debounceTime, distinctUntilChanged, map } from 'rxjs';
import { LoadingSpinnerComponent } from '../../shared/loading-spinner.component';

@Component({
  selector: 'app-vehicle-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, LoadingSpinnerComponent],
  templateUrl: './vehicle-search.component.html',
  styleUrls: ['./vehicle-search.component.css']
})
export class VehicleSearchComponent implements OnInit {
  searchForm: FormGroup;
  makes: VehicleMake[] = [];
  filteredMakes: VehicleMake[] = [];
  types: VehicleType[] = [];
  models: VehicleModel[] = [];

  loadingMakes = false;
  searching = false;
  showAutocomplete = false;
  submitted = false;

  constructor(private vehicleService: VehicleService) {
    const currentYear = new Date().getFullYear();
    this.searchForm = new FormGroup({
      make: new FormControl('', Validators.required),
      makeId: new FormControl(null, Validators.required),
      year: new FormControl('', [
        Validators.required,
        Validators.min(1990),
        Validators.max(currentYear + 1),
        Validators.pattern('^[0-9]{4}$')
      ])
    });
  }

  ngOnInit(): void {
    this.loadMakes();

    this.searchForm.get('make')?.valueChanges.pipe(
      debounceTime(200),
      distinctUntilChanged()
    ).subscribe(value => {
      this.filterMakes(value);
    });
  }

  loadMakes(): void {
    this.loadingMakes = true;
    this.vehicleService.getAllMakes().subscribe({
      next: (data) => {
        this.makes = data;
        this.loadingMakes = false;
      },
      error: () => this.loadingMakes = false
    });
  }

  filterMakes(value: string): void {
    if (!value || typeof value !== 'string') {
      this.filteredMakes = [];
      this.showAutocomplete = false;
      return;
    }

    const filterValue = value.toLowerCase();
    this.filteredMakes = this.makes
      .filter(make => make.make_Name.toLowerCase().includes(filterValue))
      .slice(0, 10);
    this.showAutocomplete = this.filteredMakes.length > 0;
  }

  selectMake(make: VehicleMake): void {
    this.searchForm.patchValue({
      make: make.make_Name,
      makeId: make.make_ID
    });
    this.showAutocomplete = false;
  }

  onSearch(): void {
    if (this.searchForm.invalid) return;

    this.submitted = true;
    this.searching = true;
    this.types = [];
    this.models = [];

    const { makeId, year } = this.searchForm.value;

    this.vehicleService.getVehicleTypes(makeId).subscribe({
      next: (data) => {
        this.types = data;
      }
    });

    this.vehicleService.getModels(makeId, year).subscribe({
      next: (data) => {
        this.models = data;
        this.searching = false;
      },
      error: () => this.searching = false
    });
  }

  get yearError(): string {
    const yearControl = this.searchForm.get('year');
    if (yearControl?.hasError('required')) return 'Year is required';
    if (yearControl?.hasError('min')) return 'Min year is 1990';
    if (yearControl?.hasError('max')) return `Max year is ${new Date().getFullYear() + 1}`;
    if (yearControl?.hasError('pattern')) return 'Must be a 4-digit year';
    return '';
  }
}
