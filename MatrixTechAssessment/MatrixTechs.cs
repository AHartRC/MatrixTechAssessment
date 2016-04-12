using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MatrixTechAssessment
{
    public class MatrixTechs
    {
        #region Constructors
        public MatrixTechs()
        {
            // Instantiate the Data property to prevent Null Reference Exceptions
            Data = new HashSet<MatrixTech>();
        }

        public MatrixTechs(string jsonData)
        {
            // Populate the Data property with the json result set
            Data = JsonConvert.DeserializeObject<MatrixTechs>(jsonData).Data;
        }
        #endregion Constructors

        #region Properties
        public IEnumerable<MatrixTech> Data { get; set; }
        public static DateTime? BeginDate { get; set; }
        public static DateTime? EndDate { get; set; }

        public IEnumerable<MatrixTech> FilteredData
        {
            get
            {
                var filteredData = Data;

                if (BeginDate.HasValue)
                    filteredData = filteredData.Where(w => w.ImplantDate >= BeginDate.Value);

                if (EndDate.HasValue)
                    filteredData = filteredData.Where(w => w.ImplantDate < EndDate.Value);

                // Testing . . Could be simplified and further flushed out
                //var filteredData =
                //    Data.Where(
                //        w => BeginDate.HasValue
                //                ? w.ImplantDate >= BeginDate.Value
                //                : true 
                //            &&
                //             EndDate.HasValue
                //                ? w.ImplantDate < EndDate.Value
                //                : true);

                return filteredData;
            }
        }

        public IEnumerable<Tuple<int, int, decimal>> ImplantData
        {
            get
            {
                return from yearRecord in FilteredData.GroupBy(g => g.ImplantDate.Year)
                       let recordCount = yearRecord.Count()
                       let recordPercentage = (decimal)recordCount / FilteredRecordCount
                       orderby yearRecord.Key ascending
                       select new Tuple<int, int, decimal>(yearRecord.Key, recordCount, recordPercentage);
            }
        }

        public IEnumerable<Tuple<string, int, decimal>> GenderData
        {
            get
            {
                return from genderRecord in FilteredData.GroupBy(g => g.Gender)
                       let recordCount = genderRecord.Count()
                       let recordPercentage = (decimal)recordCount / FilteredRecordCount
                       orderby genderRecord.Key ascending
                       select new Tuple<string, int, decimal>(genderRecord.Key, recordCount, recordPercentage)
                       ;
            }
        }

        public int RecordCount
        {
            get { return Data == null ? 0 : Data.Count(); }
        }

        public int HospitalCount
        {
            get { return Data == null ? 0 : Data.Select(s => s.HospitalID).Distinct().Count(); }
        }

        public int FilteredRecordCount
        {
            get { return FilteredData == null ? 0 : FilteredData.Count(); }
        }

        public int FilteredHospitalCount
        {
            get { return FilteredData == null ? 0 : FilteredData.Select(s => s.HospitalID).Distinct().Count(); }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Populate the Data property with a string containing properly formed JSON data
        /// </summary>
        /// <param name="jsonData">Properly formed Json Data</param>
        public void PopulateData(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                throw new ArgumentNullException("jsonData", "The supplied JSON data is either null or empty.");

            // Populate the data depending on whether or not a new JSON dataset was passed
            Data = JsonConvert.DeserializeObject<MatrixTechs>(jsonData).Data.OrderBy(o => o.ImplantDate).ThenBy(t => t.HospitalID).ThenBy(t => t.Gender);
        }

        public void TransformData(DateTime? beginDate = null, DateTime? endDate = null, string jsonString = null)
        {
            if (!string.IsNullOrWhiteSpace(jsonString))
                PopulateData(jsonString);

            BeginDate = beginDate;
            EndDate = endDate;
        }

        #region Overrides
        public override string ToString()
        {
            string result = "MatrixTechs Object:\r\n";
            result += string.Format("{0} Records | {1} Filtered Records | {2} Hospitals | {3} Filtered Hospitals\r\n",
                RecordCount, FilteredRecordCount, HospitalCount, FilteredHospitalCount);
            result += "Implant Date Table:\r\nYear\tRecords\tPercentage\r\n";
            result = ImplantData.Aggregate(result, (current, implantRecord) => current + string.Format("{0}\t{1}\t{2:P2}\r\n", implantRecord.Item1, implantRecord.Item2, implantRecord.Item3));
            result += string.Format("Total\t{0}\t{1:P2}\r\n\r\n", ImplantData.Sum(s => s.Item2), ImplantData.Sum(s => s.Item3));
            result += "Gender Table:\r\nGender\tRecords\tPercentage\r\n";
            result = GenderData.Aggregate(result, (current, genderRecord) => current + string.Format("{0}\t{1}\t{2:P2}\r\n", genderRecord.Item1, genderRecord.Item2, genderRecord.Item3));
            result += string.Format("Total\t{0}\t{1:P2}\r\n\r\n", GenderData.Sum(s => s.Item2), GenderData.Sum(s => s.Item3));

            return result;
        }
        #endregion Overrides
        #endregion Methods
    }
}
