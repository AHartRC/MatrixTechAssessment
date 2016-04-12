using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixTechAssessment
{
	public class MatrixTech
	{
		[Required]
		public int HospitalID { get; set; }
		[Required]
		public DateTime ImplantDate { get; set; }
		[Required]
		public Gender GenderId { get; set; }

		[NotMapped]
		public string Gender {
			get
			{
				switch (GenderId)
				{
					case MatrixTechAssessment.Gender.Unspecified:
						return "Unspecified";
					case MatrixTechAssessment.Gender.Male:
						return "Male";
					case MatrixTechAssessment.Gender.Female:
						return "Female";
					default:
						throw new ArgumentOutOfRangeException("GenderId", "The gender that was specified is invalid or unknown.");
				}
			}
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("The gender value that was passed was unable to be processed.");

				if (value.ToLower().Equals("male"))
					GenderId = MatrixTechAssessment.Gender.Male;
				else if(value.ToLower().Equals("female"))
					GenderId = MatrixTechAssessment.Gender.Female;
				else
					GenderId = MatrixTechAssessment.Gender.Unspecified;
			}
		}
	}
}
