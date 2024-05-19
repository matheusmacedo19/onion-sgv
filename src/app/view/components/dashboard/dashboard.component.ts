import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';  
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { DashboardOrder } from '../../../models/dashboardOrder';
import { DashboardService } from '../../../services/dashboard.service';
import { response } from 'express';
import { MatInputModule } from '@angular/material/input';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterOutlet,MatTableModule,MatFormFieldModule,MatPaginatorModule,MatInputModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})


export class DashboardComponent implements AfterViewInit {
  dashboardOrders: DashboardOrder[] = [];
  displayedColumns: string[] = ['cliente', 'produto', 'valor', 'dataEntrega'];
  dataSource = new MatTableDataSource<DashboardOrder>(this.dashboardOrders);

  @ViewChild(MatPaginatorModule) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service : DashboardService){}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.findAll();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  findAll(): void{
    this.service.findAll().subscribe((response) => {
      this.dashboardOrders = response;
      console.log(this.dashboardOrders);
    })
  }
}
