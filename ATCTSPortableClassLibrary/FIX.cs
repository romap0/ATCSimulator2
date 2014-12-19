using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATCTSPortableClassLibrary
{
	public class FIX
	{
		public FIX ( )
		{

		}

		public FIX ( string Name, int Latitude, int Longitude )
		{
			this.Name = Name;
			this.Latitude = Latitude;
			this.Longitude = Longitude;
		}

		public string Name;
		public int Latitude;
        public int Longitude;

        public int Altitude;
        public int Speed;
        public bool FlyOver;
	}
}
