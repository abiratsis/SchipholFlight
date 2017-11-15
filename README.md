# Schiphol API

## Overview
Schiphol API provides basic functionality for retrieving flight information from the Schiphol airport. The API is implemented with .NET Core.

## API Calls
List airlines:

```
GET http://localhost:8237/schiphol-api/airlines?page=0
```

List arrivals, filter takes 3 values {today, tomorrow, yesterday}:

```
GET http://localhost:8237/schiphol-api/arrivals?page=0&filter=tomorrow
```

List departures:
```
GET http://localhost:8237/schiphol-api/departures?page=0&airline=KL
```
## Runing application
Since Schiphol API is a .NET Core application in order to execute it first navigate under the Schiphol.Web folder and then execute:

```
dotnet run
```


