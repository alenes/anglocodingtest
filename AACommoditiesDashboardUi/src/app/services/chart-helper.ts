import { Injectable } from "@angular/core";
import * as Highcharts from 'highcharts';
import * as moment from "moment";
import { CommodityGroup } from "../models/commodity-group";

@Injectable({
    providedIn: 'root'
  })


export class ChartHelper
{
    addChartOptions(commodityGroups: CommodityGroup[]):  Highcharts.Options
    {
         let options: Highcharts.Options = {
            title: {
            text: "Pnl for Commodities"
            },
            chart: {
                zoomType: 'x'
            },
            xAxis: {
                title: {text:"Date"}
            },
            yAxis: {
                zoomEnabled: true,
                title: {
                    text: "GBP"
                }
            },
            series: []
        };

        if(commodityGroups.length == 0)
        {
            return options;
        }

        commodityGroups.forEach(group => {

            let coordinates: any[] = group.commodityResults.map(function(a) {return [moment(a.date).format("DD/MM/YYYY"), a.pnlDaily];});
            options.series?.push(
                {
                    data: coordinates,
                    type: 'line',
                    name: group.commodity
                }
            );
        });

        return options;
    }

}