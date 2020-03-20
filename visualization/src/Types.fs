module Types

type RemoteData<'data, 'error> =
    | NotAsked
    | Loading
    | Failure of 'error
    | Success of 'data

type DataPoint =
    { Date : System.DateTime
      NewTests : int option
      TotalTests : int option
      NewCases : int option
      TotalCases : int option
      Hospitalized : int option
      NewDeaths : int option
      TotalDeaths : int option }

type Data = DataPoint list

type Metric =
    { Color : string
      Visible : bool
      Label : string
      Slug : string }

type Metrics =
    { NewTests : Metric
      TotalTests : Metric
      NewCases : Metric
      TotalCases : Metric
      Hospitalized : Metric
      NewDeaths : Metric
      TotalDeaths : Metric }

type MetricMsg =
    | NewTests
    | TotalTests
    | NewCases
    | TotalCases
    | Hospitalized
    | NewDeaths
    | TotalDeaths

type State =
    { Data : RemoteData<Data, string>
      Metrics : Metrics }

type Msg =
    | DataLoaded of RemoteData<Data, string>
    | ToggleMetricVisible of MetricMsg
