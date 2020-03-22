# COVID-19 Data for Slovenia

This repository contains the code for visualizing the COVID-19 Data for Slovenia.

The visualization is available at https://joahim.github.io/covid-19

The data used in the visualization is sourced from the excellent https://tinyurl.com/slo-covid-19

The data is also available in the following formats:

* CSV from https://github.com/slo-covid-19/data/tree/master/csv
* JSON from https://covid19.rthand.com/api/data

For more in-depth information about the COVID-19 in Slovenia you may also want to check https://covid19.rtfm.si/

Note: until I discovered https://tinyurl.com/slo-covid-19 this project collected it's own data. I do not plan to do this anymore.


## Development

This project is built using [Fable](http://fable.io/), [Elmish](https://fable-elmish.github.io/) and [Feliz](https://github.com/Zaid-Ajaj/Feliz).

### Requirements

* [dotnet SDK](https://www.microsoft.com/net/download/core) 2.0.0 or higher
* [node.js](https://nodejs.org) 10.0.0 or higher

### Running and building the project

Before doing anything, start with installing yarn dependencies using `yarn install`.

Then to start development mode with hot module reloading, run:
```bash
npm start
```
This will start the development server after compiling the project, once it is finished, navigate to http://localhost:8080 to view the application .

To build the application and make ready for production:
```
npm run build
```
This command builds the application and puts the generated files into the `deploy` directory (can be overwritten in webpack.config.js).
