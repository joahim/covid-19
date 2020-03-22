module App

open Elmish
open Fable.React
open Fable.React.Props

open Types

let init() =
    let initialState =
        { Data = NotAsked
          Metrics =
            { Tests = { Color = "#ffa600" ; Visible = false ; Label = "Tests" ; Slug = "Tests" }
              TotalTests = { Color = "#ff764a" ; Visible = false ; Label = "Total tests" ; Slug = "TotalTests" }
              Cases = { Color = "#ef5675" ; Visible = false ; Label = "Cases" ; Slug = "Cases" }
              TotalCases = { Color = "#bc5090" ; Visible = true ; Label = "Total cases" ; Slug = "TotalCases" }
              Hospitalized = { Color = "#7a5195" ; Visible = true ; Label = "Hospitalized" ; Slug = "Hospitalized" }
              Deaths = { Color = "#374c80" ; Visible = false ; Label = "Deaths" ; Slug = "Deaths" }
              TotalDeaths = { Color = "#003f5c" ; Visible = false ; Label = "Total deaths" ; Slug = "TotalDeaths" } } }

    initialState, Cmd.OfAsync.result SourceData.loadData

let update (msg: Msg) (state: State) =
    match msg with
    | DataLoaded data ->
        { state with Data = data }, Cmd.none
    | ToggleMetricVisible metric ->
        let newMetrics =
            match metric with
            | Tests -> { state.Metrics with Tests = { state.Metrics.Tests with Visible = not state.Metrics.Tests.Visible } }
            | TotalTests -> { state.Metrics with TotalTests = { state.Metrics.TotalTests with Visible = not state.Metrics.TotalTests.Visible } }
            | Cases -> { state.Metrics with Cases = { state.Metrics.Cases with Visible = not state.Metrics.Cases.Visible } }
            | TotalCases -> { state.Metrics with TotalCases = { state.Metrics.TotalCases with Visible = not state.Metrics.TotalCases.Visible } }
            | Hospitalized -> { state.Metrics with Hospitalized = { state.Metrics.Hospitalized with Visible = not state.Metrics.Hospitalized.Visible } }
            | Deaths -> { state.Metrics with Deaths = { state.Metrics.Deaths with Visible = not state.Metrics.Deaths.Visible } }
            | TotalDeaths -> { state.Metrics with TotalDeaths = { state.Metrics.TotalDeaths with Visible = not state.Metrics.TotalDeaths.Visible } }
        { state with Metrics = newMetrics }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
    match state.Data with
    | NotAsked -> nothing
    | Loading -> str "Loading data..."
    | Failure error -> str (error)
    | Success data ->
        div [ Class "container" ] [
            h1 [ ] [ str "COVID-19 / Slovenia" ]
            section [ Class "content" ]
                [ Chart.render data state.Metrics dispatch
                  DataTable.render data state.Metrics ]
            section [ Class "source" ]
                [ str "Code and pointers to the data used for this visualization are available at "
                  a [ Href "https://github.com/joahim/covid-19" ] [ str "https://github.com/joahim/covid-19" ] ]
        ]
