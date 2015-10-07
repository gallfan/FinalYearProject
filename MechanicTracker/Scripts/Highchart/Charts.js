//Sets some global option to use with all the charts
Highcharts.setOptions({
    chart: {
        style: {
            fontFamily: '"Lucida Grande", "Lucida Sans Unicode", Verdana, Arial, Helvetica, sans-serif',
            fontSize: '16px',
            color: "#FFFFFF"
        }
    },
    lang: {
        loading: "Loading..."
    },
    loading: {
        hideDuration: 400,
        showDuration: 400
    },
    noData: {
        style: {
            "color": "#ffffff",
            "fontSize": "18px",
            "fontWeight": "bold"
        }
    },
    credits: {
        enabled: false
    },
    legend: {
        enabled: true,
        itemStyle: { "color": "#ffffff" },
        itemHoverStyle: { "color": "#ffffff", "cursor": "default" }
    },
    title: {
        align: "left",
        margin: 0,
        style: { "color": "#ffffff", "fontSize": "20px", "fontWeight": "bold" }
    },
    tooltip: {
        hideDelay: 300
    }
});

//Retrieves the json data to create the data for the charts
function RetrieveJSON(Id, url, method, ele, name) {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: url,
        data: { Id: Id },
        success: function (data) {
            method(data, ele, name);
            $(ele).fadeIn();
        },
        error: function () {
            $(ele).html("<p>Error Retrieving Chart Data</p>");
            $(ele).fadeIn();
        }
    });
}

function PieChart(PieData, ele, name) {
    if (PieData != null) {
        var a = PieData[0];
        var b = PieData[1];
        var c = PieData[2];
    }
    else
        a = b = c = 0;

    if(a != 0 || b!=0 || c!=0)
    {
        $(ele).highcharts({
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 60,
                    beta: 0,
                },
                margin: [-70, -20, -20, -20],
            },
            title: {
                text: "",
            },
            tooltip: {
                headerFormat: '<span style="font-size: 14px"><b>{point.key}</b></span><br/>',
                pointFormat: 'Number: <b>{point.y}</b><br/>Percentage: <b>{point.percentage:.1f}%</b>',
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    point: {
                        events: {
                            legendItemClick: function () {
                                return false;
                            },
                            click: function () {
                                return false;
                            }
                        }
                    },
                    center: ["50%", "50%"],
                    colors: ["rgba(51,204,51,.8)", "rgba(255,153,0, .8)", "rgba(255,0,0, .8)"],
                    cursor: 'pointer',
                    depth: 30,
                    dataLabels: {
                        enabled: false,
                        format: '{point.name}',
                        borderRadius: 10,
                        connectorWidth: 1,
                        distance: 5,
                        style: {
                            fontWeight: 'bold',
                            fontsize: "12px",
                            color: "#ffffff"
                        }
                    },
                    showInLegend: true,
                    shadow: true,
                    slicedOffset: 20,
                    states: {
                        hover: {
                            brightness: 0.2
                        }
                    },
                    startAngle: 45,
                }
            },
            series: [{
                type: 'pie',
                name: name,
                data: [
                    ['On Time', a],
                    ['Acceptable Time', b],
                    ['Over Time', c]
                ]
            }]
        });
    }
    else
    {
        $(ele).html("<p>No Data to Display</p>");
    }
}

//Creates Line Charts using data passed in to create a different one for each different Line chart needed
    function LineChart(data, ele, name) {
        //Putting the Data into two arrays, one to be used on the x-axix and the other one the y-axis
        var Group = new Array();
        var Count = new Array();
        for (var i = 0; i < data.length; i++) {
            Group.push(data[i].Month);
            Count.push(data[i].Count);
        }

        $(ele).highcharts({
            chart: {
                type: 'line'
            },

            title: {
                text: ""
            },
            xAxis: {
                categories: Group
            },
            yAxis: {
                title: {
                    text: name
                },
                minTickInterval: 0.5,
                min: 0,
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            plotOptions: {
                line: {
                    events: {
                        legendItemClick: function () {
                            return false;
                        }
                    },
                    enableMouseTracking: true,
                    color: "rgba(255,153,0,1)",
                    lineWidth: 3,
                    marker: {
                        symbol: "diamond",
                        radius: 6
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size: 14px"><b>{point.x}</b></span><br/>',
                pointFormat: 'Number: <b>{point.y}</b></b>',
            },
            series: [{
                name: 'Number of Jobs',
                data: Count
            }]
        });
};