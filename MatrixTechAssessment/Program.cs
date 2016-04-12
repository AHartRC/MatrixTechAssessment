using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MatrixTechAssessment
{
    class Program
    {
        public static string JsonData = Resources.JsonData;
        public static DateTime DataBeginDate = new DateTime(2010, 1, 1);
        public static DateTime DataEndDate = new DateTime(2015, 1, 1);
        public static DateTime SearchBeginDate = new DateTime(2012, 1, 1);
        public static DateTime SearchEndDate = new DateTime(2013, 1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("{0} | Instantiating MatrixTechs class", DateTime.Now);

            MatrixTechs result = JsonConvert.DeserializeObject<MatrixTechs>(Resources.JsonData);
            Console.WriteLine("BASE RESULT:\r\n{0}", result);

            result.TransformData(DataBeginDate, DataEndDate, Resources.JsonData); // Populate data and set date filters
            Console.WriteLine("DEFAULT FILTER RESULT:\r\n{0}", result);

            result.TransformData(SearchBeginDate, SearchEndDate); // Use the pre-existing dataset and set date filters
            Console.WriteLine("2012 FILTER RESULT:\r\n{0}", result);

            Console.ReadLine();
        }
    }
}
