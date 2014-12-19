using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATCTSPortableClassLibrary
{
	public class STAR
	{
		public string Name;
        public List<FIX> Fixes = new List<FIX>();
        public bool ReciprocalRunway;
	}
}
