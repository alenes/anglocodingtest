import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import * as moment from "moment";
import { Observable} from 'rxjs';

@Injectable({
    providedIn: 'root'
  })
export class CommodityService {

  constructor(private http: HttpClient) {}

  getCMetrics(asOfDate: Date): Observable<any> {

    const asOf = moment(asOfDate).format('YYYY-MM-DD');
    return this.http.get(`http://localhost:5000/api/commodities/metrics/${asOf}`);
  }

  getCommodities(startDate: Date, endDate: Date): Observable<any> {

    const start = moment(startDate).format('YYYY-MM-DD');
    const end = moment(endDate).format('YYYY-MM-DD');
    return this.http.get(`http://localhost:5000/api/commodities?startDate=${start}&endDate=${end}`);
  }
}
