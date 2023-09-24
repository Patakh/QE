using Npgsql;
using System;
using System.Collections.Generic;
namespace Function;

public static class Prerecord
{
    public class PrerecordData
    {
        public long SDayWeekId { get; set; }
        public string DayName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTimePrerecord { get; set; }
        public TimeSpan StopTimePrerecord { get; set; }
    }

    public static List<PrerecordData> GetPrerecordData(long in_s_office_id, DateOnly in_date)
    {
        List<PrerecordData> data = new List<PrerecordData>();

        // Создаем подключение к базе данных
        using (NpgsqlConnection connection = new NpgsqlConnection("Server=176.113.83.242;User Id=postgres;Password=!ShamiL19;Port=5432;Database=EQ"))
        {
            connection.Open();

            // Создаем команду для вызова функции и получения данных
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.select_prerecord(@in_s_office_id, @in_date)", connection))
            {
                // Добавляем параметры в команду
                command.Parameters.AddWithValue("in_s_office_id", in_s_office_id);
                command.Parameters.AddWithValue("in_date", in_date);

                // Выполняем команду и получаем результаты
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Создаем объект PrerecordData и заполняем его данными из результата
                        PrerecordData prerecord = new PrerecordData();
                        prerecord.SDayWeekId = (long)reader["out_s_day_week_id"];
                        prerecord.DayName = (string)reader["out_day_name"];
                        prerecord.Date = (DateTime)reader["out_date"];
                        prerecord.StartTimePrerecord = (TimeSpan)reader["out_start_time_prerecord"];
                        prerecord.StopTimePrerecord = (TimeSpan)reader["out_stop_time_prerecord"];

                        // Добавляем объект PrerecordData в список данных
                        data.Add(prerecord);
                    }
                }
            }
        }

        return data;
    }
}