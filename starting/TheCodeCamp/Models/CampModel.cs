using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCodeCamp.Models
{
	//Here we are creating the data which the end user gets to see, the model if you will.  This is what we expose through the API, as you can see some of the properties have been removed
	//We can also change the name of the properties to something more user friendly if needed, if the property names of the Entities don't make much sense, but we didn't do that here
	public class CampModel
	{
		public string Name { get; set; }
		public string Moniker { get; set; }
		public DateTime EventDate { get; set; } = DateTime.MinValue;
		public int Length { get; set; } = 1;

		//in Camp there is an ICollection<Talks> we want return TalkModels similarly to the way we are returning CampModels
		public ICollection<TalkModel> Talks {get; set;}

		//Include Location Information because we would like to know info about the location of the camp because this is a 1 to 1 relationship we are adding the location info and flattening
		//it into the camp model, a join if you will

		//here we want to change the VenueName from the Location entity to just Venue, i.e. changing the name of a property.  To make this week we need to create a new rule in our mapping
		//profile CampMappingProfile
		public string Venue { get; set; }

		//If you prefix the name of the property with the Entity type you want, AutoMapper should be able to find it
		public string LocationAddress1 { get; set; }
		public string LocationAddress2 { get; set; }
		public string LocationAddress3 { get; set; }
		public string LocationCityTown { get; set; }
		public string LocationStateProvince { get; set; }
		public string LocationPostalCode { get; set; }
		public string LocationCountry { get; set; }

	}
}
