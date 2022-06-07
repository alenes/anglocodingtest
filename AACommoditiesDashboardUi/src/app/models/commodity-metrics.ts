

export class CommodityMetrics {
    commodityId?: number;
    commodity?: string;
    price?: number;
    position?: number;
    pnlDaily?: number;
    pnlYTD?:number;


    constructor(commodityId: number, commodity: string,price: number, position: number, pnlDaily: number, pnlYTD:number) {
        this.commodityId=commodityId;
        this.commodity=commodity;
        this.price=price;
        this.position=position;
        this.pnlDaily=pnlDaily;
        this.pnlYTD=pnlYTD;
      }
  }