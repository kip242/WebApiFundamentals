
using System;
using System.Collections.Generic;

namespace TheCodeCamp.Data
{
	//in the Entities folder is the actual "shapes" of the data in the datastore, in this case MSSQL server built into VS2019
  public class Camp
  {
    public int CampId { get; set; }
    public string Name { get; set; }
    public string Moniker { get; set; }
    public Location Location  { get; set; }
    public DateTime EventDate { get; set; } = DateTime.MinValue;
    public int Length { get; set; } = 1;
    public ICollection<Talk> Talks { get; set; }
  }
}