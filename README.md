# Mitko-Tenev-Employees

## Before running the application

**Since the app is built with .NET and Angular make sure to have .NET 8.0 SDK and Node.js installed before running it.**
- Download links:
  - .NET SDK - https://dotnet.microsoft.com/en-us/download/dotnet/8.0
  - Node.js - https://nodejs.org/
  - (optional) Visual Studio - to run the app: https://visualstudio.microsoft.com/downloads/

## To run the app
1. Clone this repo: `git clone https://github.com/mitko-tenev/mitko-tenev-employees.git`
2. Enter the `mitko-tenev-employees` directory
3. Open the project with `Visual Studio` by clicking on the `mitko-tenev-employees.sln` or execute the following commands:
     1. `cd mitko-tenev-employees.Server`
     2. `dotnet restore`
     3. `dotnet build`
     4. `dotnet run`
     5. If a browser window does not open automatically, go to [https://localhost:51615/](https://localhost:51615/)
4. Here are several CSVs with test data for testing purposes:
- https://raw.githubusercontent.com/mitko-tenev/mitko-tenev-employees/refs/heads/main/data/employee_project_data.csv
- https://raw.githubusercontent.com/mitko-tenev/mitko-tenev-employees/refs/heads/main/data/randomized_dates_employee_project_data.csv
- https://raw.githubusercontent.com/mitko-tenev/mitko-tenev-employees/refs/heads/main/data/big_employee_project_data.csv
  
## Features of the app
- Form for uploading CSVs
![image](https://github.com/user-attachments/assets/a9a9611a-70d8-49ce-8030-44dac62e54e9)

- View with the longest working pair results and table with data for the projects and employees.
![image](https://github.com/user-attachments/assets/6bc091e0-809c-41c3-8134-bf39a2644e71)

- Various formats for parsing dates are supported: `yyyy-MM-dd`, `dd-MM-yyyy`, `MM-dd-yyyy`. dash (`-`) and forward slash (`/`) can be used for delimeters.

## Unit tests
This app includes backend unit tests.
To execute the tests:
1. Make sure to clone the repo and execute the following commands in `mitko-tenev-employees.Server` folder - `dotnet restore`, `dotnet build` - to install all dependencies and build the project
2. Run `dotnet test mitko_tenev_employees.Server.Tests\mitko_tenev_employees.Server.Tests.csproj`
