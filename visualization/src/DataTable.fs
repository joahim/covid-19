[<RequireQualifiedAccess>]
module DataTable

open Feliz

open Types

let render data metrics =
    let header =
      [ "Date"
        "New tests"
        "Total tests"
        "New cases"
        "Total cases"
        "Hospitalized"
        "New deaths"
        "Total deaths" ]
      |> List.map (fun label -> Html.th [ Html.text label ])
      |> Html.tr

    let renderOption option =
        match option with
        | None -> Html.none
        | Some value -> value.ToString() |> Html.text

    let renderNumber number =
        Html.td
            [ prop.className "has-text-right"
              prop.children [ renderOption number ] ]

    let renderRow (row : DataPoint) =
        Html.tr [
            Html.td [ sprintf "%d-%02d-%02d" row.Date.Date.Year row.Date.Date.Month row.Date.Date.Day |> Html.text ]
            renderNumber row.NewTests
            renderNumber row.TotalTests
            renderNumber row.NewCases
            renderNumber row.TotalCases
            renderNumber row.Hospitalized
            renderNumber row.NewDeaths
            renderNumber row.TotalDeaths
        ]

    Html.table [
        prop.className "data-table table is-bordered is-hoverable is-fullwidth"
        prop.children [
            Html.thead [ header ]
            Html.tbody (data |> List.map renderRow)
        ]
    ]
