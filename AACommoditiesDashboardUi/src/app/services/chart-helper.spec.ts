import { TestBed } from '@angular/core/testing';
import { SeriesOptionsType } from 'highcharts';
import { CommodityGroup } from '../models/commodity-group';
import { CommodityResult } from '../models/commodity-result';
import { ChartHelper } from './chart-helper';

describe('ChartHelper', () => {
    let service: ChartHelper;
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [ChartHelper]
        });
        service = TestBed.inject(ChartHelper);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should return empty option series', () => {
        const result = service.addChartOptions([]);

        expect(result.series).toEqual([]);
    });

    it('should return option series', () => {

        let cpmResults = [new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222)];

        let groups = [new CommodityGroup("Aluminium", 1, cpmResults), new CommodityGroup("Titanium", 2, cpmResults)];

        const result = service.addChartOptions(groups);
        const series: SeriesOptionsType[] = result.series != undefined ? result.series : [];
        expect(series[0].name).toEqual("Aluminium");
        expect(series[1].name).toEqual("Titanium");
        expect(series[0].point).toEqual(series[1].point);
    });
});
