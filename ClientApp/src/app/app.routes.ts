import { Routes } from '@angular/router';
import { VehicleSearchComponent } from './features/vehicle-search/vehicle-search.component';

export const routes: Routes = [
    { path: '', component: VehicleSearchComponent },
    { path: '**', redirectTo: '' }
];