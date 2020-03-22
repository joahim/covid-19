[<RequireQualifiedAccess>]
module Chart

open Feliz
open Feliz.Recharts

open Types

let formatDate (date : System.DateTime) =
    sprintf "%d.%d." date.Date.Day date.Date.Month

let renderChart (data : Data) (metrics : Metrics) =

    let renderLineLabel (input: ILabelProperties) =
        Html.text [
            prop.x(input.x)
            prop.y(input.y)
            prop.fill color.black
            prop.textAnchor.middle
            prop.dy(-10)
            prop.fontSize 10
            prop.text input.value
        ]

    let renderMetric (metric : Metric) (dataKey : DataPoint -> int) =
        Recharts.line [
            line.name metric.Label
            line.monotone
            line.stroke metric.Color
            line.label renderLineLabel
            line.dataKey dataKey
        ]

    let children =
        seq {
            yield Recharts.xAxis [ xAxis.dataKey (fun point -> formatDate point.Date) ]
            yield Recharts.yAxis [ ]
            yield Recharts.tooltip [ ]
            yield Recharts.cartesianGrid [ cartesianGrid.strokeDasharray(3, 3) ]

            if metrics.Tests.Visible then
                yield renderMetric metrics.Tests
                    (fun (point : DataPoint) -> point.Tests |> Option.defaultValue 0)

            if metrics.TotalTests.Visible then
                yield renderMetric metrics.TotalTests
                    (fun (point : DataPoint) -> point.TotalTests |> Option.defaultValue 0)

            if metrics.Cases.Visible then
                yield renderMetric metrics.Cases
                    (fun (point : DataPoint) -> point.Cases |> Option.defaultValue 0)

            if metrics.TotalCases.Visible then
                yield renderMetric metrics.TotalCases
                    (fun (point : DataPoint) -> point.TotalCases |> Option.defaultValue 0)

            if metrics.Hospitalized.Visible then
                yield renderMetric metrics.Hospitalized
                    (fun (point : DataPoint) -> point.Hospitalized |> Option.defaultValue 0)

            if metrics.HospitalizedIcu.Visible then
                yield renderMetric metrics.HospitalizedIcu
                    (fun (point : DataPoint) -> point.HospitalizedIcu |> Option.defaultValue 0)

            if metrics.Recovered.Visible then
                yield renderMetric metrics.Recovered
                    (fun (point : DataPoint) -> point.Recovered |> Option.defaultValue 0)

            if metrics.TotalRecovered.Visible then
                yield renderMetric metrics.TotalRecovered
                    (fun (point : DataPoint) -> point.TotalRecovered |> Option.defaultValue 0)

            if metrics.Deaths.Visible then
                yield renderMetric metrics.Deaths
                    (fun (point : DataPoint) -> point.Deaths |> Option.defaultValue 0)

            if metrics.TotalDeaths.Visible then
                yield renderMetric metrics.TotalDeaths
                    (fun (point : DataPoint) -> point.TotalDeaths |> Option.defaultValue 0)
        }

    Recharts.lineChart [
        lineChart.data data
        lineChart.children (Seq.toList children)
    ]

let renderChartContainer data metrics =
    Recharts.responsiveContainer [
        responsiveContainer.width (length.percent 100)
        responsiveContainer.height 500
        responsiveContainer.chart (renderChart data metrics)
    ]

let renderMetricSelector (metric : Metric) metricMsg dispatch =
    let style =
        if metric.Visible
        then [ style.backgroundColor metric.Color ; style.borderColor metric.Color ]
        else [ ]
    Html.div [
        prop.onClick (fun _ -> ToggleMetricVisible metricMsg |> dispatch)
        prop.className [ true, "button metric-selector"; metric.Visible, "metric-selector--selected" ]
        prop.style style
        prop.text metric.Label ]

let renderMetricsSelectors metrics dispatch =
    Html.div [
        prop.className "metrics-selectors"
        prop.children [
            renderMetricSelector metrics.Tests Tests dispatch
            renderMetricSelector metrics.TotalTests TotalTests dispatch
            renderMetricSelector metrics.Cases Cases dispatch
            renderMetricSelector metrics.TotalCases TotalCases dispatch
            renderMetricSelector metrics.Hospitalized Hospitalized dispatch
            renderMetricSelector metrics.HospitalizedIcu HospitalizedIcu dispatch
            renderMetricSelector metrics.Recovered Recovered dispatch
            renderMetricSelector metrics.TotalRecovered TotalRecovered dispatch
            renderMetricSelector metrics.Deaths Deaths dispatch
            renderMetricSelector metrics.TotalDeaths TotalDeaths dispatch ] ]

let render data metrics dispatch =
    Html.div [
        renderChartContainer data metrics
        renderMetricsSelectors metrics dispatch
    ]
