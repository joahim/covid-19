[<RequireQualifiedAccess>]
module Chart

open Feliz
open Feliz.Recharts

open Types

let formatDate (date : System.DateTime) =
    sprintf "%d.%d." date.Date.Day date.Date.Month

let renderChart (data : Data) (metrics : Metrics) =

    let renderMetric color (dataKey : DataPoint -> string) =
        Recharts.line [
            line.monotone
            line.stroke color
            line.dataKey dataKey
        ]

    let children =
        seq {
            yield Recharts.xAxis [ xAxis.dataKey (fun point -> formatDate point.Date) ]
            yield Recharts.yAxis [ ]
            yield Recharts.tooltip [ ]
            yield Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]

            if metrics.NewTests.Visible then
                yield renderMetric metrics.NewTests.Color
                    (fun (point : DataPoint) -> point.NewTests |> Option.map (fun p -> p.ToString()) |> Option.defaultValue "")

            if metrics.TotalTests.Visible then
                yield renderMetric metrics.TotalTests.Color
                    (fun (point : DataPoint) -> point.TotalTests |> Option.map (fun p -> p.ToString()) |> Option.defaultValue "")

            if metrics.NewCases.Visible then
                yield renderMetric metrics.NewCases.Color
                    (fun (point : DataPoint) -> point.NewCases |> Option.map (fun p -> p.ToString()) |> Option.defaultValue "")

            if metrics.TotalCases.Visible then
                yield renderMetric metrics.TotalCases.Color
                    (fun (point : DataPoint) -> point.TotalCases |> Option.map (fun p -> p.ToString()) |> Option.defaultValue "")

            if metrics.Hospitalized.Visible then
                yield renderMetric metrics.Hospitalized.Color
                    (fun (point : DataPoint) -> point.Hospitalized |> Option.map (fun p -> p.ToString()) |> Option.defaultValue "")

            if metrics.NewDeaths.Visible then
                yield renderMetric metrics.NewDeaths.Color
                    (fun (point : DataPoint) -> point.NewDeaths |> Option.map (fun p -> p.ToString()) |> Option.defaultValue "")

            if metrics.TotalDeaths.Visible then
                yield renderMetric metrics.TotalDeaths.Color
                    (fun (point : DataPoint) -> point.TotalDeaths |> Option.map (fun p -> p.ToString()) |> Option.defaultValue "")
        }

    Recharts.lineChart [
        lineChart.data data
        lineChart.children (Seq.toList children)
    ]

let renderChartContainer data metrics =
    Recharts.responsiveContainer [
        responsiveContainer.width (length.percent 100)
        responsiveContainer.height 400
        responsiveContainer.chart (renderChart data metrics)
    ]

let renderMetricSelector (metric : Metric) metricMsg dispatch =
    Html.div [
        prop.onClick (fun _ -> ToggleMetricVisible metricMsg |> dispatch)
        prop.className [ true, "button metric-selector metric-selector--" + metric.Slug; metric.Visible, "selected" ]
        prop.text metric.Label ]

let renderMetricsSelectors metrics dispatch =
    Html.div [
        prop.className "metrics-selectors buttons"
        prop.children [
            renderMetricSelector metrics.NewTests NewTests dispatch
            renderMetricSelector metrics.TotalTests TotalTests dispatch
            renderMetricSelector metrics.NewCases NewCases dispatch
            renderMetricSelector metrics.TotalCases TotalCases dispatch
            renderMetricSelector metrics.Hospitalized Hospitalized dispatch
            renderMetricSelector metrics.NewDeaths NewDeaths dispatch
            renderMetricSelector metrics.TotalDeaths TotalDeaths dispatch ] ]

let render data metrics dispatch =
    Html.div [
        renderChartContainer data metrics
        renderMetricsSelectors metrics dispatch
    ]
