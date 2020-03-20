module App

open Elmish
open Fable.SimpleHttp
open Fable.SimpleJson
open Fable.React
open Fable.React.Props

open Types

let dataUrl = "https://raw.githubusercontent.com/joahim/covid-19/master/COVID-19-SI.json"

let loadData =
    async {
        let! (statusCode, response) = Http.get dataUrl
        if statusCode <> 200 then
            return DataLoaded (sprintf "Error loading data: %d" statusCode |> Failure)
        else
            try
                let data =
                    response
                    |> SimpleJson.parse
                    |> SimpleJson.mapKeys (function
                        | "date" -> "Date"
                        | "new_cases" -> "NewCases"
                        | "total_cases" -> "TotalCases"
                        | "new_tests" -> "NewTests"
                        | "total_tests" -> "TotalTests"
                        | "hospitalized" -> "Hospitalized"
                        | "new_deaths" -> "NewDeaths"
                        | "total_deaths" -> "TotalDeaths"
                        | key -> key)
                    |> Json.convertFromJsonAs<Data>
                return DataLoaded (Success data)
            with
                | ex -> return DataLoaded (sprintf "Error parsing data: %s" ex.Message |> Failure)
    }

let init() =
    let initialState =
        { Data = NotAsked
          Metrics =
            { NewTests = { Color = "green" ; Visible = false ; Label = "New tests" ; Slug = "NewTests" }
              TotalTests = { Color = "blue" ; Visible = true ; Label = "Total tests" ; Slug = "TotalTests" }
              NewCases = { Color = "cyan" ; Visible = false ; Label = "New cases" ; Slug = "NewCases" }
              TotalCases = { Color = "orange" ; Visible = true ; Label = "Total cases" ; Slug = "TotalCases" }
              Hospitalized = { Color = "hotpink" ; Visible = true ; Label = "Hospitalized" ; Slug = "Hospitalized" }
              NewDeaths = { Color = "magenta" ; Visible = false ; Label = "New deaths" ; Slug = "NewDeaths" }
              TotalDeaths = { Color = "red" ; Visible = true ; Label = "Total deaths" ; Slug = "TotalDeaths" } } }

    initialState, Cmd.OfAsync.result loadData

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
        fragment [ ] [
            h1 [ ClassName "title is-1" ] [ str "COVID-19 in Slovenia" ]
            Chart.render data state.Metrics dispatch
            DataTable.render data state.Metrics
        ]
