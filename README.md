# Overview
This is a .NET 8 webapp with Blazor/Radzen frontend and microservices arquitecture, it manages offices for use in a real life setting where you'd want to have a constant information flow between the attention places of a given office and it's customers, also includes metrics about the daily operative of the offices, alongside graphs to show them.

Here we got the Domain Model diagram to show the architecture this project employs:
![image](https://github.com/user-attachments/assets/28d56903-b396-4b48-9c09-a2e1e11bcb70)



### Instalation
In Windows, download [Docker Desktop](https://www.docker.com/products/docker-desktop/)
Make sure to download [Microsoft SQL Server Managment Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) Express Version
  Once you've got the required dependencies, download the project and head to
```
Taller-Dotnet/Commercial-Office/
```
in which we will need to download the migrations for setting up the Databases.


For the we will need to install a tool to manage said migrations, with the following commando
```
dotnet tool install --global dotnet-ef
```


Once the tool has been installed, we get the database up:
```
dotnet ef database update
```
**this last script must be repeated for all the following projects:**
- Commercial-Office
- Quality-Managment
- Authentication


### Running
It is now that we can run the program, we *could* execute each and everyone of them manually, but it would be much better to execute the Aspire hosting project found on the next location:
```
Taller-DotNet/App-Host/
```
and once we're there, we just execute the program *Remember to have Docker Desktop open while doing so*
```
dotnet run
```
after this we can go to our browser and go to the page *http://localhost/5214*
The first time it will prompt you to a login page and ask you for a token, it can be found on the console that is running the program.
![image](https://github.com/user-attachments/assets/8856ff80-ec5d-4794-8675-84be5572e4c8)


### Demonstration
Once you've correctly set up, executed and went to the dashboard, the following screen should appear:
![image](https://github.com/user-attachments/assets/1da6e8a0-cfd6-4fab-b68c-27122e9c4f6b)

From here on out, you can check all of the projects individually, but ideally, the API-Gateway is where you are going to spend the most time, as it is the one that has all the access to the views, which then, makes use of the rest of the projects 
