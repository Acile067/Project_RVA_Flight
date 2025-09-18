using CsvHelper;
using CsvHelper.Configuration;
using RVA_Flight.Common.Contracts;
using RVA_Flight.Common.Entities;
using RVA_Flight.Server.Mappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RVA_Flight.Server.DataStorage
{
    public class CsvDataStorage : IDataStorage
    {
        public void Save<T>(string filePath, T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            IList<object> list;

            if (data is IEnumerable enumerable && !(data is string))
            {
                list = enumerable.Cast<object>().ToList();
            }
            else
            {
                list = new List<object> { data };
            }

            var elementType = list.First().GetType();

            using (var writer = new StreamWriter(filePath, false))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(GetMap(elementType));
                csv.WriteHeader(elementType);
                csv.NextRecord();
                csv.WriteRecords(list);
            }
        }

        public T Load<T>(string filePath)
        {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    var listElementType = typeof(T).GetGenericArguments()[0];
                    return (T)Activator.CreateInstance(typeof(List<>).MakeGenericType(listElementType));
                }
                return default;
            }

            Type elementType = typeof(T);
            bool isList = false;

            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                elementType = typeof(T).GetGenericArguments()[0];
                isList = true;
            }

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(GetMap(elementType));
                var method = typeof(CsvReader).GetMethod("GetRecords", Type.EmptyTypes)
                    .MakeGenericMethod(elementType);
                var records = ((IEnumerable)method.Invoke(csv, null)).Cast<object>().ToList();

                if (isList)
                {
                    var typedList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                    foreach (var r in records) typedList.Add(r);
                    return (T)typedList;
                }

                return (T)records.FirstOrDefault();
            }
        }

        private ClassMap GetMap(Type type)
        {
            if (type == typeof(City)) return new CityMap();
            if (type == typeof(Airplane)) return new AirplaneMap();
            if (type == typeof(Flight)) return new FlightMap();
            if (type == typeof(CharterFlight)) return new CharterFlightMap();
            throw new InvalidOperationException($"No CSV map defined for {type.Name}");
        }
    }
}
