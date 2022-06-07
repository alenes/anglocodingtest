export class CommodityResult
{
    date: Date;
    contract: string;
    price: number;
    position: number;
    newTradeAction: number;
    pnlDaily: number;

    constructor(
        date: Date,
        contract: string,
        price: number,
        position: number,
        newTradeAction: number,
        pnlDaily: number
    )
    {
        this.date=date;
        this.contract=contract;
        this.price = price;
        this.position=position;
        this.newTradeAction=newTradeAction;
        this.pnlDaily=pnlDaily;
    }

}