using Dapper;
using Microsoft.Data.SqlClient;
 
using WeatherWebServices.Models;

namespace WeatherWebServices.Data
{
    public class WeatherForecastRepository :  IWeatherForecastRepository
        {
        private readonly string _connectionString;

        public WeatherForecastRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task ProcessForecastAsync(WeatherForecastResponseApi weatherData)
        {

            if (weatherData?.Data?.Records == null) return;

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                foreach (var record in weatherData.Data.Records)
                {
                    // delete existing detail to avoid duplicate records
                    const string selectheaderSql = @"select id from WeatherForecasts where ForecastDate=@ForecastDate";
                    const string deletedetailSql = @"select id from RegionalForecasts where MainForecastId=@MainForecastId";
                    const string deleteheadersql = @"delete from WeatherForecasts where ForecastDate=@ForecastDate";

                    var forecastId = await connection.QueryFirstOrDefaultAsync<int>(selectheaderSql, new { ForecastDate = record.Date }, transaction);

                    await connection.ExecuteScalarAsync(deletedetailSql, new {MainForecastId = forecastId }, transaction);
                    await connection.ExecuteScalarAsync(deleteheadersql, new { ForecastDate = record.Date }, transaction);



                    //INSERT
                    const string mainSql = @"
                    INSERT INTO WeatherForecasts (ForecastDate, TempHigh, TempLow,TempUnits,HumidityHigh,
                            HumidityLow,HumidityUnits,Forecastcode, ForecastText,validPeriodStart,validPeriodEnd,validPeriodText,
                            WindSpeedHigh,WindSpeedLow,WindDirection,UpdatedTimestamp)
                    OUTPUT INSERTED.Id
                    VALUES (@Date, @TempHigh, @TempLow,@TempUnits,@HumidityHigh,
                            @HumidityLow,@HumidityUnits,@Forecastcode, @ForecastText,@validPeriodStart,@validPeriodEnd,@validPeriodText,
                            @WindSpeedHigh,@WindSpeedLow,@WindDirection, @UpdatedTimestamp)";

                    var mainId = await connection.QuerySingleAsync<int>(mainSql, new
                    {
                        record.Date,
                        TempHigh = record.General.Temperature.High,
                        TempLow = record.General.Temperature.Low,
                        TempUnits = record.General.Temperature.Unit,
                        HumidityHigh = record.General.Humidity.High,
                        HumidityLow = record.General.Humidity.Low,
                        HumidityUnits = record.General.Humidity.Unit,
                        Forecastcode= record.General.Forecast.Code,
                        ForecastText = record.General.Forecast.Text,
                        validPeriodStart = record.General.ValidPeriod.Start,
                        validPeriodEnd = record.General.ValidPeriod.End,
                        validPeriodText = record.General.ValidPeriod.Text,
                        WindSpeedHigh = record.General.Wind.Speed.High,
                        WindSpeedLow = record.General.Wind.Speed.Low,
                        WindDirection= record.General.Wind.Direction,
                        record.UpdatedTimestamp
                    }, transaction);

                    //INSERT 
                    const string regionalSql = @"
                    INSERT INTO RegionalForecasts (MainForecastId, RegionName, ForecastCode, ForecastText, StartTime, EndTime,TimePeriodText)
                    VALUES (@MainId, @Region, @Code, @Text, @Start, @End,@TimePeriodText)";

                    foreach (var period in record.Periods)
                    {
                        foreach (var region in period.Regions)
                        {
                            await connection.ExecuteAsync(regionalSql, new
                            {
                                MainId = mainId,
                                Region = region.Key,  
                                region.Value.Code,
                                region.Value.Text,
                                Start = period.TimePeriod.Start,
                                End = period.TimePeriod.End,
                                TimePeriodText= period.TimePeriod.Text,
                            }, transaction);
                        }
                    }
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        public async Task<WeatherForecast?> GetWeatherForecast(DateTime Fetchdate,string? RegionName)
        {
            using var connection = new SqlConnection(_connectionString);

            try
            {

                const string Sql = @"
                    select wf.id,ForecastDate,TempHigh,TempLow,TempUnits,HumidityHigh,HumidityLow,HumidityUnits,wf.Forecastcode,wf.forecastText,ValidperiodStart,
                    ValidperiodEnd,ValidPeriodText, WindSpeedHigh, WindSpeedLow,wf.WindDirection,UpdatedTimeStamp,
                    RegionName,rf.Forecastcode,rf.ForecastText,StartTime,EndTime,TimePeriodText
                    from 
                    WeatherForecasts wf inner join RegionalForecasts rf on wf.id=rf.MainForecastId
                    where  wf.ForecastDate=@FetchDate   and (rf.RegionName=@RegionName or @RegionName IS NULL)";

                var Params = new
                {
                    Fetchdate = Fetchdate,
                    RegionName = RegionName
                };
                WeatherForecast? weatherForecast = null;
                 await connection.QueryAsync<WeatherForecast,RegionalForecast,WeatherForecast>(Sql,
                     (forecast,regional)=>
                     {

                         if (weatherForecast == null)
                         {
                             weatherForecast = forecast;
                             weatherForecast.regionalForecasts = new List<RegionalForecast>();
                         }
                         if (regional != null)
                         {
                             weatherForecast.regionalForecasts.Add(regional);
                         }
                         return forecast;

                     }, Params,
                     splitOn: "RegionName"

                     );

                return weatherForecast;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IEnumerable<WeatherForecastExport?>> GetWeatherForecastExport(DateTime Fetchdate, string? RegionName)
        {
            using var connection = new SqlConnection(_connectionString);

            try
            {

                const string Sql = @"
                    select wf.id,ForecastDate,TempHigh,TempLow,TempUnits,HumidityHigh,HumidityLow,HumidityUnits,wf.Forecastcode,wf.forecastText,ValidperiodStart,
                    ValidperiodEnd,ValidPeriodText, WindSpeedHigh, WindSpeedLow,wf.WindDirection,UpdatedTimeStamp,
                    RegionName,rf.Forecastcode as RegionalForecastCode,rf.ForecastText as RegionalForecastText,StartTime,EndTime,TimePeriodText
                    from 
                    WeatherForecasts wf,RegionalForecasts rf
                    where wf.id=rf.MainForecastId and wf.ForecastDate=@FetchDate   and (rf.RegionName=@RegionName or @RegionName IS NULL)";

                var Params = new
                {
                    Fetchdate = Fetchdate,
                    RegionName = RegionName
                };
                return await connection.QueryAsync<WeatherForecastExport>(Sql, Params);

            }
            catch
            {
                throw;
            }
        }


        public async Task AlertsSubscribe(Subscribe subscribe)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                /// Insert subscribe 
                const string stationSql = @"
                IF EXISTS (SELECT 1 FROM subscribe WHERE email = @email)
                BEGIN
                    UPDATE subscribe 
                    SET ForecastCode = @ForecastCode, Humidity = @Humidity, Temperature = @Temperature ,Active=@Active,SubscribedDate=@SubscribedDate
                    WHERE Email = @Email
                END
                ELSE
                BEGIN
                    INSERT INTO subscribe (Email,ForecastCode,Humidity,Temperature,Active,SubscribedDate)
                    VALUES (@Email,@ForecastCode, @Humidity, @Temperature, @Active,@SubscribedDate)
                END";

                var Params = new
                {
                    Email = subscribe.Email,
                    ForecastCode = subscribe.Forecastcode,
                    Humidity = subscribe.Humidity,
                    Temperature = subscribe.Temperature,
                    Active = subscribe.Active,
                    SubscribedDate = DateTime.Now
                };
         
                await connection.ExecuteAsync(stationSql, Params, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task AlertsUnSubscribe(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                /// update to un subscribe 
                const string stationSql = @"
                    UPDATE subscribe 
                    SET Active =0,UnSubscribedDate=@UnSubscribedDate
                    WHERE Email = @Email";


                var Params = new
                {
                    Email = email,
                    UnSubscribedDate = DateTime.Now
                };


                await connection.ExecuteAsync(stationSql, Params, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task ExecuteSubscriptionAlert(DateTime Fetchdate)
        {
            using var connection = new SqlConnection(_connectionString);

            try
            {

                const string Sql = @"
                        select s.Email,wf.ForecastDate,wf.temphigh,wf.templow,wf.TempUnits,
                        wf.HumidityHigh,wf.HumidityLow,wf.HumidityUnits,
                        wf.Forecastcode,wf.ForecastText,wf.validPeriodStart,wf.validPeriodEnd,wf.validPeriodText
                        from WeatherForecasts wf,Subscribe s
                        where
                        (wf.TempHigh >= s.Temperature or 
                        wf.Forecastcode=s.ForecastCode or
                        wf.HumidityHigh>= s.Humidity)
                        and active=1 and Forecastdate=@ForecastDate";

                var Params = new
                {
                    ForecastDate = Fetchdate
                };

                var data= await connection.QueryAsync<WeatherForecastExport>(Sql, Params);

                if (data != null)
                {
                    // execute logic to send email
                }

            }
            catch
            {
                throw;
            }
        }
    }
}



