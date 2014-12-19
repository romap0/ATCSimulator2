using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATCTSPortableClassLibrary
{
	public class Runway
	{
		public string Number;
		public string ReciprocalNumber;
		public int Heading;
		public int ReciprocalHeading;
		public int StartLatitude;
		public int StartLongitude;
		public int EndLatitude;
		public int EndLongitude;
		public List<SID> SIDs = new List<SID> ( );
		public List<STAR> STARs = new List<STAR> ( );
	}
}
