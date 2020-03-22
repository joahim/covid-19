module App

open Elmish
open Fable.React
open Fable.React.Props

open Types

let init() =
    let initialState =
        { Data = NotAsked
          Metrics =
            { NewTests = { Color = "#ffa600" ; Visible = false ; Label = "New tests" ; Slug = "NewTests" }
              TotalTests = { Color = "#ff764a" ; Visible = false ; Label = "Total tests" ; Slug = "TotalTests" }
              NewCases = { Color = "#ef5675" ; Visible = false ; Label = "New cases" ; Slug = "NewCases" }
              TotalCases = { Color = "#bc5090" ; Visible = true ; Label = "Total cases" ; Slug = "TotalCases" }
              Hospitalized = { Color = "#7a5195" ; Visible = true ; Label = "Hospitalized" ; Slug = "Hospitalized" }
              NewDeaths = { Color = "#374c80" ; Visible = false ; Label = "New deaths" ; Slug = "NewDeaths" }
              TotalDeaths = { Color = "#003f5c" ; Visible = false ; Label = "Total deaths" ; Slug = "TotalDeaths" } } }

    initialState, Cmd.OfAsync.result SourceData.loadData

let update (msg: Msg) (state: State) =
    match msg with
    | DataLoaded data ->
        { state with Data = data }, Cmd.none
    | ToggleMetricVisible metric ->
        let newMetrics =
            match metric with
            | NewTests -> { state.Metrics with NewTests = { state.Metrics.NewTests with Visible = not state.Metrics.NewTests.Visible } }
            | TotalTests -> { state.Metrics with TotalTests = { state.Metrics.TotalTests with Visible = not state.Metrics.TotalTests.Visible } }
            | NewCases -> { state.Metrics with NewCases = { state.Metrics.NewCases with Visible = not state.Metrics.NewCases.Visible } }
            | TotalCases -> { state.Metrics with TotalCases = { state.Metrics.TotalCases with Visible = not state.Metrics.TotalCases.Visible } }
            | Hospitalized -> { state.Metrics with Hospitalized = { state.Metrics.Hospitalized with Visible = not state.Metrics.Hospitalized.Visible } }
            | NewDeaths -> { state.Metrics with NewDeaths = { state.Metrics.NewDeaths with Visible = not state.Metrics.NewDeaths.Visible } }
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
