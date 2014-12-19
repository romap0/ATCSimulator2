using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATCTSPortableClassLibrary
{
	public class Airport
	{
		public string Name = "";
		public string ICAO;
		public string TowerFrequency;
		public int Latitude;
		public int Longitude;
		public List<Runway> Runways = new List<Runway> ( );
	}
}
