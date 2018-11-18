using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace DataLoaderLibrary.Services
{
    public class LoaderService : ILoaderService
    {
        // Строка подключения к БД.
        private SqlConnectionStringBuilder ConnectionStringBuilder { get; set; }

        /// <summary>
        /// Перегрузка конструктора, позволяющая явно указать параметры  строки подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public LoaderService(string connectionString)
        {
            ConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        /// <summary>
        /// Перегрузка конструктора, генерирующая подключение по названию сервера.
        /// Используется авторизация подлиности Windows.
        /// </summary>
        /// <param name="serverName">Название сервера</param>
        /// <param name="initialCatalog">База данных по-умолчанию</param>
        public LoaderService(string serverName, string initialCatalog = "master")
        {
            ConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                ["Data Source"] = serverName,
                ["Integrated Security"] = true,
                ["Initial Catalog"] = initialCatalog
            };
        }

        /// <summary>
        /// Возвращает выгрузку из базы данных в динамическом словаре.
        /// </summary>
        /// <param name="sqlExpression">SQL запрос</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetQueryResults(string sqlExpression)
        {
            IEnumerable<dynamic> queryResult = null;

            using (var connection = new SqlConnection(ConnectionStringBuilder.ConnectionString))
            {
                queryResult = connection.Query<dynamic>(sqlExpression);
            }

            return queryResult;
        }
    }
}
