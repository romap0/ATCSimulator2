using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATCTSPortableClassLibrary
{
	public class NDB
	{
		public NDB ( )
		{

		}

		public NDB ( string Name, string Frequency, int Latitude, int Longitude )
		{
			this.Name = Name;
			this.Frequency = Frequency;
			this.Latitude = Latitude;
			this.Longitude = Longitude;
		}

		public string Name;
		public string Frequency;
		public int Latitude;
		public int Longitude;

		public FIX ToFIX ( )
		{
			return new FIX ( Name, Latitude, Longitude );
		}
	}
}
