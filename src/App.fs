module App

open Elmish
open Fable.React
open Fable.React.Props

open Types

let init() =
    let initialState =
        { Data = NotAsked
          Metrics =
            { Tests =               { Color = "#ffa600" ; Visible = false ; Label = "Tests" }
              TotalTests =          { Color = "#f09320" ; Visible = false ; Label = "Total tests" }
              Cases =               { Color = "#e18040" ; Visible = false ; Label = "Cases" }
              TotalCases =          { Color = "#d26d60" ; Visible = true  ; Label = "Total cases" }
              Hospitalized =        { Color = "#c35a80" ; Visible = true  ; Label = "Hospitalized" }
              HospitalizedIcu =     { Color = "#a74e8a" ; Visible = false ; Label = "Hospitalized in ICU" }
              Recovered =       { Color = "#7d4a7f" ; Visible = false ; Label = "Recovered" }
              TotalRecovered =  { Color = "#544773" ; Visible = false ; Label = "Total recovered"}
              Deaths =              { Color = "#2a4368" ; Visible = false ; Label = "Deaths" }
              TotalDeaths =         { Color = "#003f5c" ; Visible = false ; Label = "Total deaths" } } }

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
            | HospitalizedIcu -> { state.Metrics with HospitalizedIcu = { state.Metrics.HospitalizedIcu with Visible = not state.Metrics.HospitalizedIcu.Visible } }
            | Recovered -> { state.Metrics with Recovered = { state.Metrics.Recovered with Visible = not state.Metrics.Recovered.Visible } }
            | TotalRecovered -> { state.Metrics with TotalRecovered = { state.Metrics.TotalRecovered with Visible = not state.Metrics.TotalRecovered.Visible } }
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
