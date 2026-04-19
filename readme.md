## Singapore Weather API (C# / .NET)

This project is a RESTful Web API built using C# and .NET that provides real-time weather data for Singapore. It allows users to retrieve key environmental information such as temperature, humidity, and rainfall, with support for area-based filtering and intelligent alert subscriptions.

The API integrates with public data sources and exposes structured JSON responses, making it easy to consume for web, mobile, or analytics applications. It follows a clean, scalable architecture with background services handling data ingestion, filtering, and alert processing.

Key Features
Retrieve real-time weather data (temperature, humidity, rainfall)
Filter weather data by specific areas/regions in Singapore
Access weather forecasts for selected locations
Subscribe to weather alerts based on:
Custom thresholds (e.g., humidity > X percentage, temperature > Y°C)
Forecast conditions (e.g., heavy rain expected in a specific area)
Automated alert notifications (email, webhook)
Background services to fetch, process, and evaluate weather data

## Tech Stack

- C# / .NET Web API
- RESTful services
- SQL Server
- Background Hosted Services (Worker Service)
- Optional: Notification integrations (Email) / can be expanded to webhook
- Documentation: Swagger

### Installation

- Clone the repository:
- Bash
- git clone https://github.com/sudhar81/WeatherWebServices.git
- Open the solution:
- Open WeatherWebServices.sln in Visual Studio.
- Update Connection String:
  Navigate to appsettings.json and update the ConnectionStrings section to point to your local database.

  JSON
  "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=YourDB;Trusted_Connection=True;"
  }

### Database

- Open /scripts/initial_database_script.sql
- Execute the script to create & tables
- Open /scripts/Initial_data_WeatherStation_Master_table_script.sql
- Execute the script to generate WeatherStation Master

## ## API Endpoints

| Method   | Endpoint                              | Description                                                                                            |
| -------- | ------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| **POST** | `/api/v1/auth/token`                  | Get JWT token (default: `admin / password123`)                                                         |
| **GET**  | `/api/WeatherForecast/Current`        | Fetch latest weather forecasts can be filtered by regions (Regions: East, West, North, South, Central) |
| **GET**  | `/api/WeatherForecast/History`        | Fetch historical weather data (filter by date and region)                                              |
| **GET**  | `/api/WeatherForecast/export-csv`     | Export weather data as CSV (filter by date and region)                                                 |
| **POST** | `/api/WeatherForecast/Subscribe`      | Subscribe to alerts when conditions exceed thresholds (e.g., temp > 30°C or forecast = heavy showers)  |
| **POST** | `/api/WeatherForecast/UnSubscribe`    | allows to unsubscribe from alerts                                                                      |
| **GET**  | `/api/WeatherReadings/WeatherStation` | Get all the Station Master Data which can be used to filter the data                                   |
| **GET**  | `/api/WeatherReadings/Current`        | Fetch latest weather readings can be filtered by stations                                              |
| **GET**  | `/api/WeatherReadings/Current`        | Fetch historical weather reading data (filter by date and stations)                                    |
| **GET**  | `/api/WeatherForecast/export-csv`     | Export weather readings data as CSV (filter by date and region)                                        |

## Weather condition codes to pass in subscribe

| Code | Description           |
| ---- | --------------------- |
| CL   | Cloudy                |
| PC   | Partly Cloudy (Day)   |
| PN   | Partly Cloudy (Night) |
| SH   | Showers               |
| TL   | Thundery Showers      |

---

## ## Project Structure

- **Controllers/**: Handles incoming HTTP requests and maps them to actions.
- **Models/**: Data structures and DTOs (Data Transfer Objects).
- **Data/**:(Folder has both Services and Repository)
- **Services**/:Business logic layer.
- **Repositories/**: Data access layer (e.g., Dapper queries).

---

## License

This project is licensed under the MIT License.

You are free to use, modify, and distribute this project with attribution.

See the [LICENSE](LICENSE) file for full details.
