using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCodeCamp.Models;

namespace TheCodeCamp.Data
{
    public class CampMappingProfile : Profile
    {
		//This is where we create the profile for our mapping, CampMappingProfile inherits Profile from AutoMapper
		public CampMappingProfile()
		{
			//so this means create a map from Camp to CampModel, unless there is an exception it will use conventional ways to map them i.e. CampModel has a "moniker" and Camp has a "moniker"
			//so that must be how we are going to map them
			CreateMap<Camp, CampModel>()
				//we do this to TELL the map what to do because AutoMapper Conventionally does not know where to find the new "Venue" name we want to display because it doesn't exist
				//in our entity
				//for the indvidual member map    get camp.venue               from Location.VenueName
				.ForMember(                       c => c.Venue,                 opt => opt.MapFrom( m => m.Location.VenueName));

		}
    }
}
