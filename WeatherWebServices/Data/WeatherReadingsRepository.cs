

using Dapper;
 
using Microsoft.Data.SqlClient;
 
using System.Data;
using WeatherWebServices.Models;
 

namespace WeatherWebServices.Data
{
    public class WeatherReadingsRepository
    {
        private readonly string _connectionString;

        public WeatherReadingsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }



        // table to fetch data directly from api and return immediately

        public async Task ProcessTemperatureAsync(List<WeatherStation> stations, List<ReponseData> readings)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Upsert Stations (Ensures new stations are added or existing names/coords updated)
                const string stationSql = @"
                IF EXISTS (SELECT 1 FROM WeatherStations WHERE StationId = @Id)
                BEGIN
                    UPDATE WeatherStations 
                    SET StationName = @Name, Latitude = @Lat, Longitude = @Lon
                    WHERE StationId = @Id
                END
                ELSE
                BEGIN
                    INSERT INTO WeatherStations (StationId, StationName, Latitude, Longitude)
                    VALUES (@Id, @Name, @Lat, @Lon)
                END";

                // Map DTO to SQL parameters
                var stationParams = stations.Select(s => new {
                    Id = s.StationId,
                    Name = s.StationName,
                    Lat = s.Location.Latitude,
                    Lon = s.Location.Longitude
                });
                await connection.ExecuteAsync(stationSql, stationParams, transaction);

                // 2. Insert Readings
                const string readingSql = @"
                INSERT INTO TemperatureReadings (StationId, Value,ReadingType,ReadingUnit, ReadingTimestamp,SessionId)
                VALUES (@StationId, @Value,@ReadingType,@ReadingUnit, @ReadingTimestamp,@SessionId)";


                await connection.ExecuteAsync(readingSql, readings, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ProcessRainfallAsync(List<WeatherStation> stations, List<ReponseData> readings)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Upsert Stations (Ensures new stations are added or existing names/coords updated)
                const string stationSql = @"
                IF EXISTS (SELECT 1 FROM WeatherStations WHERE StationId = @Id)
                BEGIN
                    UPDATE WeatherStations 
                    SET StationName = @Name, Latitude = @Lat, Longitude = @Lon
                    WHERE StationId = @Id
                END
                ELSE
                BEGIN
                    INSERT INTO WeatherStations (StationId, StationName, Latitude, Longitude)
                    VALUES (@Id, @Name, @Lat, @Lon)
                END";

                // Map DTO to SQL parameters
                var stationParams = stations.Select(s => new {
                    Id = s.StationId,
                    Name = s.StationName,
                    Lat = s.Location.Latitude,
                    Lon = s.Location.Longitude
                });
                await connection.ExecuteAsync(stationSql, stationParams, transaction);

                // 2. Insert Readings
                const string readingSql = @"
                INSERT INTO RainFallReadings  (StationId, Value,ReadingType,ReadingUnit, ReadingTimestamp,SessionId)
                VALUES (@StationId, @Value,@ReadingType,@ReadingUnit, @ReadingTimestamp,@SessionId)";




                await connection.ExecuteAsync(readingSql, readings, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ProcessHumidityAsync(List<WeatherStation> stations, List<ReponseData> readings)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Upsert Stations (Ensures new stations are added or existing names/coords updated)
                const string stationSql = @"
                IF EXISTS (SELECT 1 FROM WeatherStations WHERE StationId = @Id)
                BEGIN
                    UPDATE WeatherStations 
                    SET StationName = @Name, Latitude = @Lat, Longitude = @Lon
                    WHERE StationId = @Id
                END
                ELSE
                BEGIN
                    INSERT INTO WeatherStations (StationId, StationName, Latitude, Longitude)
                    VALUES (@Id, @Name, @Lat, @Lon)
                END";

                // Map DTO to SQL parameters
                var stationParams = stations.Select(s => new {
                    Id = s.StationId,
                    Name = s.StationName,
                    Lat = s.Location.Latitude,
                    Lon = s.Location.Longitude
                });
                await connection.ExecuteAsync(stationSql, stationParams, transaction);

                // 2. Insert Readings
                const string readingSql = @"
                INSERT INTO HumidityReadings (StationId, Value,ReadingType,ReadingUnit, ReadingTimestamp,SessionId)
                VALUES (@StationId, @Value,@ReadingType,@ReadingUnit, @ReadingTimestamp,@SessionId)";


                await connection.ExecuteAsync(readingSql, readings, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ProcessWindDirectionAsync(List<WeatherStation> stations, List<ReponseData> readings )
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Upsert Stations (Ensures new stations are added or existing names/coords updated)
                const string stationSql = @"
                IF EXISTS (SELECT 1 FROM WeatherStations WHERE StationId = @Id)
                BEGIN
                    UPDATE WeatherStations 
                    SET StationName = @Name, Latitude = @Lat, Longitude = @Lon
                    WHERE StationId = @Id
                END
                ELSE
                BEGIN
                    INSERT INTO WeatherStations (StationId, StationName, Latitude, Longitude)
                    VALUES (@Id, @Name, @Lat, @Lon)
                END";

                // Map DTO to SQL parameters
                var stationParams = stations.Select(s => new {
                    Id = s.StationId,
                    Name = s.StationName,
                    Lat = s.Location.Latitude,
                    Lon = s.Location.Longitude
                });
                await connection.ExecuteAsync(stationSql, stationParams, transaction);

                // 2. Insert Readings
                const string readingSql = @"
                INSERT INTO WindDirectionReadings (StationId, Value,ReadingType,ReadingUnit, ReadingTimestamp,SessionId)
                VALUES (@StationId, @Value,@ReadingType,@ReadingUnit, @ReadingTimestamp,@SessionId)";


                await connection.ExecuteAsync(readingSql, readings, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        // WindSpeedReadings ready insert

        public async Task ProcessWindSpeedAsync(List<WeatherStation> stations, List<ReponseData> readings)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Upsert Stations (Ensures new stations are added or existing names/coords updated)
                const string stationSql = @"
                IF EXISTS (SELECT 1 FROM WeatherStations WHERE StationId = @Id)
                BEGIN
                    UPDATE WeatherStations 
                    SET StationName = @Name, Latitude = @Lat, Longitude = @Lon
                    WHERE StationId = @Id
                END
                ELSE
                BEGIN
                    INSERT INTO WeatherStations (StationId, StationName, Latitude, Longitude)
                    VALUES (@Id, @Name, @Lat, @Lon)
                END";

                // Map DTO to SQL parameters
                var stationParams = stations.Select(s => new {
                    Id = s.StationId,
                    Name = s.StationName,
                    Lat = s.Location.Latitude,
                    Lon = s.Location.Longitude
                });
                await connection.ExecuteAsync(stationSql, stationParams, transaction);

                // 2. Insert Readings
                const string readingSql = @"
                INSERT INTO WindSpeedReadings (StationId, Value,ReadingType,ReadingUnit, ReadingTimestamp,SessionId)
                VALUES (@StationId, @Value,@ReadingType,@ReadingUnit, @ReadingTimestamp,@SessionId)";


                await connection.ExecuteAsync(readingSql, readings, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<IEnumerable<WeatherReadings>> GetLatestWeatherDetails(string SessionId, string? StationId)
        {
            using var connection = new SqlConnection(_connectionString);



            try
            {


                const string stationSql = @"select ws.*,
                    rf.Value as Rainfall,
                     case when rf.value>0 then rf.ReadingUnit				
                     else null					end                     as Rainfallunit, 
                    case when rf.value >0 then 
                    rf.ReadingTimeStamp
                    else null end as RainfallReadingTimeStamp,
                    wsr.value  as windspeed,  wsr.ReadingUnit  as WindSpeedunit,
                    wsr.ReadingTimeStamp as windspeedReadingTimeStamp,
                    wdr.value as winddirection, wdr.ReadingUnit as winddirectionunit,
                    wdr.ReadingTimeStamp as winddirectionReadingTimeStamp,
                    HR.value  as Humidity, HR.ReadingUnit as Humidityunit,
                    HR.ReadingTimeStamp as HumidityReadingTimeStamp,
                    TR.value  as Temperature,  TR.ReadingUnit as Temperatureunit,
                    TR.ReadingTimeStamp as TemperatureReadingTimeStamp
                    from weatherstations ws
                    left outer join RainfallReadings rf on rf.stationid = ws.stationid 
                    left outer join WindSpeedReadings  wsr on wsr.stationid=ws.stationid and wsr.SessionId=rf.SessionId
                    left outer join WindDirectionReadings  wdr on wdr.stationid=ws.stationid  and wdr.SessionId=rf.SessionId
                    left outer join HumidityReadings  HR on HR.stationid=ws.stationid  and HR.SessionId=rf.SessionId
                    left outer join TemperatureReadings  TR on TR.stationid=ws.stationid  and TR.SessionId=rf.SessionId
                    Where rf.SessionId=@SessionId
                    and (isnull(rf.value,0)>0 or wsr.value is not null or wdr.value is not null or hr.value is not null or tr.value is not null)
                    and (ws.Stationid=@stationId or @stationId IS NULL)
                    order by ws.stationid";

                var Params = new
                {
                    SessionId = SessionId,
                    StationId = StationId
                };


                return await connection.QueryAsync<WeatherReadings>(stationSql, Params);

            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<WeatherStation>> GetAllWeatherStations()
        {
            using var connection = new SqlConnection(_connectionString);



            try
            {


                const string stationSql = @"select top 1000 StationId,StationName,Latitude,Longitude from weatherstations ws order by ws.stationid";



                return await connection.QueryAsync<WeatherStation>(stationSql);





            }
            catch
            {
                throw;
            }
        }


    }
}
