# BusinessDays
This solutions goal is to calculate the number of business days between two given dates. Holidays are removed from the total number of working weekdays and are supplied through the use of a csv data file. These holidays come in three different types and will varie code operation based on each type (FixedDate, ShiftingDay, OccurrenceDay)

## Design
This solution is broken up into 4 layers to impove maintainablity:
- API: Application point of access

- CORE: Holds the core contracts and data objects used throughout the solution

- SERVICE: Holds the implementation of the business logic

- DATA: As an inital version, the data layer uses a csv data file to provide holiday values. This is built following the repository pattern and can be swapped out with connections to other data stores.

Algorithm design and manual testing was completed using an excel file (BusinessDays_Algorithms). These tests were then implemented in NUnit.

## Usage
1. Run the API project with the enviroment variable ```ASPNETCORE_ENVIRONMENT=Development``` 
2. Navigate to https://localhost:5001/api/businessdays?start=2020-06-20&end=2020-06-25 to view the response or use postman

The API uses a query string to read the start and end dates used for the period and take in values in the format of YYYY-MM-DD

```?start={start date}&end={end date}```

## Testing
Tests were written using NUnit and can be run with ```dotnet test```
