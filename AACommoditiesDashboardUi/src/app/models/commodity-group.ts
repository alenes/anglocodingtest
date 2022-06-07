import { CommodityResult } from "./commodity-result";

export class CommodityGroup
{
    commodity: string;
    commodityId: number;
    commodityResults: CommodityResult[];

    constructor(
        commodity: string,
        commodityId: number,
        commodityResults: CommodityResult[]
    ) {
        this.commodity=commodity;
        this.commodityId=commodityId
        this.commodityResults=commodityResults;

    }


}