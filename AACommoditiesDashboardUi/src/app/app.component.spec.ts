import { HttpClient } from '@angular/common/http';
import { destroyPlatform } from '@angular/core';
import { inject, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { CommodityGroup } from './models/commodity-group';
import { CommodityMetrics } from './models/commodity-metrics';
import { CommodityResult } from './models/commodity-result';
import { ChartHelper } from './services/chart-helper';
import { CommodityService } from './services/commodity-service';
import { TableHelper } from './services/table-helper';
import { Observable} from 'rxjs';

describe('AppComponent', () => {
  let fixture;

  beforeEach(() => destroyPlatform());
  
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        { provide: CommodityService, useClass: MockCommodityService},
        { provide: ChartHelper, useClass: ChartHelper},
        { provide: TableHelper, useClass: TableHelper},
      ],
      imports: [
        RouterTestingModule
      ],
      declarations: [
        AppComponent
      ],
    });
    fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});

class MockCommodityService {
  getCMetrics(asOfDate: Date): Observable<any> {

    const metricObservable = new Observable(observer => {
          setTimeout(() => {
              observer.next([new CommodityMetrics(1, "Aluminium", 1000, 2, 1500, 1200), new CommodityMetrics(2, "Titanium", 2000, 3, 2500, 2200)]);
          }, 1000);
    });

    return metricObservable;
  }

  getCommodities(startDate: Date, endDate: Date) {

    let cpmResults = [
      new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
      new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
      new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
      new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
      new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
      new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222)
  ];

  const commsObservable = new Observable(observer => {
        setTimeout(() => {
            observer.next([new CommodityGroup("Aluminium", 1, cpmResults), new CommodityGroup("Titanium", 2, cpmResults)]);
        }, 1000);
    });
    return commsObservable;
  }
}

