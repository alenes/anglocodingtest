import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import * as Highcharts from 'highcharts';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridApi, GridReadyEvent } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { CommodityMetrics } from './models/commodity-metrics';
import { CommodityService } from './services/commodity-service';
import { MatDatepickerInputEvent} from '@angular/material/datepicker';
import { ChartHelper } from './services/chart-helper';
import { TableHelper } from './services/table-helper';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  selectedAsOfDate = new Date(2019, 0, 2);
  viewAsOfDate = new FormControl(this.selectedAsOfDate);
  metrics: CommodityMetrics[] = [];
  selectedMetric!: CommodityMetrics;
  selectedPnLD: any;

  range = new FormGroup({
    start: new FormControl(new Date(2019, 0, 1)),
    end: new FormControl(new Date(2020, 0, 1)),
  });

  highcharts = Highcharts;
  chartOptions: Highcharts.Options | undefined;

  // For accessing the Grid's API

  public columnDefs: ColDef[] = [
      { field: 'commodity', headerName:"Commodity", filter: 'agTextColumnFilter'},
      { field: 'date', headerName:"Date"},
      { field: 'contract', headerName:"Contract", filter: 'agTextColumnFilter'},
      { field: 'price', headerName:"Price"},
      { field: 'position', headerName:"Position", filter: 'agNumberColumnFilter'},
      { field: 'newTradeAction', headerName:"New Trade Action", filter: 'agNumberColumnFilter'},
      { field: 'pnl', headerName:"Pnl Daily"},
  ];
 @ViewChild(AgGridAngular) agGrid!: AgGridAngular;
 private gridApi!: GridApi;
  metricSubscription!: Subscription;
  commoditiesSubscription: any;

 constructor(private commoditiesService: CommodityService, private chartHelper: ChartHelper, private tableHelper: TableHelper) {
}
    // DefaultColDef sets props common to all Columns
    public defaultColDef: ColDef = {
      sortable: true,
      filter: true,
    };

    rowData: any[] = [];

    onGridReady(params: GridReadyEvent) {
      this.gridApi = params.api;
    }

    ngOnInit() {
      this.getMetrics();
      this.getCommodityGroups();
    }

    changeAsOfDate(type: string, event: MatDatepickerInputEvent<Date>) {
      if(event.value != null)
      {
        this.selectedAsOfDate = event.value;
        this.getMetrics();
      }
    }

    getMetrics(): void{
      this.metricSubscription = this.commoditiesService.getCMetrics(this.selectedAsOfDate)
        .subscribe(x => {
          this.metrics = x;
          this.selectedMetric = new CommodityMetrics(0,"N/A",0,0,0,0);
        },
        error => {alert("Error fetching metrics from the source!");});
    }

    getCommodityGroups(): void{
      this.commoditiesSubscription = this.commoditiesService.getCommodities(this.range.controls['start'].value, this.range.controls['end'].value)
        .subscribe(x => {
          this.chartOptions = this.chartHelper.addChartOptions(x);
          this.rowData = this.tableHelper.createRowsForTable(x);
          this.gridApi.setRowData(this.rowData);
        },
        error => {
          this.chartOptions = this.chartHelper.addChartOptions([]);
          this.rowData = this.tableHelper.createRowsForTable([]);
          this.gridApi.setRowData(this.rowData);
          alert("Error fetching commodities from the source!");
        });
    }

    ngOnDestroy() {
      this.metricSubscription.unsubscribe();
      this.commoditiesSubscription.unsubscribe();
    }
}
