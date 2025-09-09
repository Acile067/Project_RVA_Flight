using RVA_Flight.Common.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RVA_Flight.Server.DataStorage
{
    public class CsvDataStorage : IDataStorage
    {
        public void Save<T>(string filePath, T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Ako je već kolekcija (npr. List<City>)
            if (data is IEnumerable<object> collection && !(data is string))
            {
                WriteCollection(filePath, collection.Cast<object>());
            }
            else
            {
                // Wrap u listu (npr. ako proslediš samo jedan objekat)
                WriteCollection(filePath, new List<object> { data });
            }
        }

        public T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var type = typeof(T);

            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
            {
                var elementType = type.GetGenericArguments()[0];
                var method = typeof(CsvDataStorage).GetMethod(nameof(ReadCollection), BindingFlags.NonPublic | BindingFlags.Instance);
                var generic = method.MakeGenericMethod(elementType);

                return (T)generic.Invoke(this, new object[] { filePath });
            }
            else
            {
                var list = ReadCollection<T>(filePath);
                return list.FirstOrDefault();
            }
        }

        private void WriteCollection(string filePath, IEnumerable<object> items)
        {
            var enumerated = items.ToList();
            if (!enumerated.Any()) return;

            var type = enumerated.First().GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Header
                writer.WriteLine(string.Join(",", properties.Select(p => p.Name)));

                // Rows
                foreach (var item in enumerated)
                {
                    var values = properties.Select(p =>
                    {
                        var val = p.GetValue(item);
                        return val != null ? val.ToString().Replace(",", ";") : "";
                    });
                    writer.WriteLine(string.Join(",", values));
                }
            }
        }

        private List<T> ReadCollection<T>(string filePath)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var result = new List<T>();

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length < 2) return result;

            var headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var obj = Activator.CreateInstance<T>();

                for (int j = 0; j < headers.Length; j++)
                {
                    var prop = properties.FirstOrDefault(p => p.Name == headers[j]);
                    if (prop != null && j < values.Length)
                    {
                        object convertedValue = Convert.ChangeType(values[j], prop.PropertyType);
                        prop.SetValue(obj, convertedValue);
                    }
                }

                result.Add(obj);
            }

            return result;
        }
    }
}
