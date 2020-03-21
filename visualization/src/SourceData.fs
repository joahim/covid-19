module SourceData

open Fable.SimpleHttp
open Fable.SimpleJson

open Types

let private dataUrl = "https://raw.githubusercontent.com/joahim/covid-19/master/COVID-19-SI-temp.json"

type private TransferDataPoint =
    { dayFromStart : int
      year : int
      month : int
      day : int
      phase : string
      performedTestsToDate : int option
      performedTests : int option
      positiveTestsToDate : int option
      positiveTests : int option
      statePerTreatment :
        {| inCare : int option
           inHospital : int option
           needsO2 : int option
           inICU : int option
           critical : int option
           deceasedToDate : int option
           outOfHospitalToDate : int option |}
      statePerRegion :
        {| kp : int option
           foreign : int option
           sg : int option
           ms : int option
           ng : int option
           nm : int option
           po : int option
           unknown : int option
           kk : int option
           za : int option
           ce : int option
           kr : int option
           lj : int option
           mb : int option |}
      statePerAgeToDate :
        {| age0to15 :
            {| allToDate : int option
               femaleToDate : int option
               maleToDate : int option |}
           age16to29 :
            {| allToDate : int option
               femaleToDate : int option
               maleToDate : int option |}
           age30to49 :
            {| allToDate : int option
               femaleToDate : int option
               maleToDate : int option |}
           age50to59 :
            {| allToDate : int option
               femaleToDate : int option
               maleToDate : int option |}
           ageAbove60 :
            {| allToDate : int option
               femaleToDate : int option
               maleToDate : int option |} |}
    }

    member this.ToDomain : DataPoint =
        { Date = System.DateTime(this.year, this.month, this.day)
          NewTests = this.performedTests
          TotalTests = this.performedTestsToDate
          NewCases = this.positiveTests
          TotalCases = this.positiveTestsToDate
          Hospitalized = this.statePerTreatment.inHospital
          NewDeaths = None
          TotalDeaths = this.statePerTreatment.deceasedToDate }

type private TransferData = TransferDataPoint list

let loadData =
    async {
        let! (statusCode, response) = Http.get dataUrl
        if statusCode <> 200 then
            return DataLoaded (sprintf "Error loading data: %d" statusCode |> Failure)
        else
            try
                let transferData =
                    response
                    |> SimpleJson.parse
                    |> SimpleJson.mapKeys (function
                        | "0_15" -> "age0to15"
                        | "16_29" -> "age16to29"
                        | "30-49" -> "age30to49"
                        | "50-59" -> "age50to59"
                        | "60+" -> "ageAbove60"
                        | key -> key)
                    |> Json.convertFromJsonAs<TransferData>

                let data =
                    transferData
                    |> List.map (fun transferDataPoint -> transferDataPoint.ToDomain)
                    |> List.mapFold (fun (previousTotalDeaths : int) (dataPoint : DataPoint) ->
                        match dataPoint.TotalDeaths with
                        | None -> dataPoint, previousTotalDeaths
                        | Some totalDeaths -> { dataPoint with NewDeaths = Some (totalDeaths - previousTotalDeaths) }, totalDeaths) 0
                    |> fst

                return DataLoaded (Success data)
            with
                | ex -> return DataLoaded (sprintf "Error parsing data: %s" ex.Message |> Failure)
    }
