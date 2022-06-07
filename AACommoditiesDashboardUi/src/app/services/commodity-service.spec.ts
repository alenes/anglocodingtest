import { CommodityService } from './commodity-service';
import {CommodityMetrics} from "../models/commodity-metrics";
import {of} from 'rxjs';
import {CommodityGroup} from "../models/commodity-group";

describe('CommodityService', () => {
    let httpClientSpy: any;
    let service: CommodityService;
    beforeEach(() => {
        httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
        service = new CommodityService(httpClientSpy);
    });

    it('should be able to retrieve metrics from the API via GET', (done) => {
        const metrics: CommodityMetrics[] = [{
            commodity: 'Commodity 1',
            price: 100,
            position: 1,
            commodityId: 1,
            pnlDaily: 10,
            pnlYTD: 0
         }, {
            commodity: 'Commodity 2',
            price: 100,
            position: 1,
            commodityId: 2,
            pnlDaily: 20,
            pnlYTD: 0
        }];
        const asOf = new Date();
        httpClientSpy.get.and.returnValue(of(metrics));
        service.getCMetrics(asOf).subscribe({
            next: m => {
                expect(m).toEqual(metrics);
                expect(m.length).toBe(2);
                done();
            },
            error: done.fail
        });
    });
    it('should be able to retrieve commodities from the API via GET', (done) => {
        const commodityGroups: CommodityGroup[] = [
            {
                commodity: 'Commodity 1',
                commodityId: 1,
                commodityResults: [{
                    date: new Date(),price: 100, position: 1, pnlDaily:100, contract: 'test', newTradeAction: 1
                }]
            },
            {
                commodity: 'Commodity 2',
                commodityId: 2,
                commodityResults: [{
                    date: new Date(),price: 100, position: 1, pnlDaily:100, contract: 'test', newTradeAction: 1
                }]
            }];
        const start = new Date();
        const end = new Date();
        httpClientSpy.get.and.returnValue(of(commodityGroups));
        service.getCommodities(start, end).subscribe({
            next: c => {
                expect(c).toEqual(commodityGroups);
                expect(c.length).toBe(2);
                done();
            },
            error: done.fail
        });
    });
});
