import { TestBed } from '@angular/core/testing';
import { CommodityGroup } from '../models/commodity-group';
import { CommodityResult } from '../models/commodity-result';
import { TableHelper } from './table-helper';

describe('TableHelper', () => {
    let service: TableHelper;
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [TableHelper]
        });
        service = TestBed.inject(TableHelper);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should return empty row set', () => {
        const result = service.createRowsForTable([]);

        expect(result).toEqual([]);
    });

    it('should return completed row set', () => {

        let cpmResults = [new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222)];

        let groups = [new CommodityGroup("Aluminium", 1, cpmResults), new CommodityGroup("Titanium", 2, cpmResults)];

        const result = service.createRowsForTable(groups);
        expect(result[0].commodity).toEqual("Aluminium");
        expect(result[1].commodity).toEqual("Titanium");
    });

    it('should return completed row set with values reduced to 5 for each commodity', () => {

        let cpmResults = [
            new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
            new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
            new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
            new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
            new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222),
            new CommodityResult(new Date(), "MAR18", 130, 2, 3, 2222)
        ];

        let groups = [new CommodityGroup("Aluminium", 1, cpmResults), new CommodityGroup("Titanium", 2, cpmResults)];

        const result = service.createRowsForTable(groups);
        expect(result.filter(x => x.commodity=="Aluminium").length).toEqual(5);
        expect(result.filter(x => x.commodity=="Titanium").length).toEqual(5);
    });
});
