import { Injectable } from "@angular/core";
import { CommodityGroup } from "../models/commodity-group";

@Injectable({
    providedIn: 'root'
  })

export class TableHelper
{
    createRowsForTable(commodityGroups: CommodityGroup[])
    {
         let rowData: any[] = [];

         if(commodityGroups.length == 0)
        {
            return rowData;
        }

        commodityGroups.forEach(group => {
            group.commodityResults = group.commodityResults.slice(Math.max(group.commodityResults.length - 5, 0))
            group.commodityResults.map(function(a) {
                rowData.push( {commodity: group.commodity, date: a.date, contract: a.contract, price: a.price, position: a.position, newTradeAction: a.newTradeAction, pnl: a.pnlDaily});
            });
        });

        return rowData;
    }

}