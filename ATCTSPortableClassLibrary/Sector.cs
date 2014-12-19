using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATCTSPortableClassLibrary
{
	public class Sector
	{
		public string Name;
		public double Version;
		public string MainAirport;
		public int CenterLatitude;
		public int CenterLongitude;
		public double MagneticVariation;
		public double DefaultScale;
        public int TA;
        public int TL;
		public List<Airport> Airports = new List<Airport> ( );
		public List<FIX> Fixes = new List<FIX> ( );
		public List<NDB> NDBs = new List<NDB> ( );
		public List<VOR> VORs = new List<VOR> ( );
		public List<HighAirway> HighAirways = new List<HighAirway> ( );
		public List<LowAirway> LowAirways = new List<LowAirway> ( );
		public List<ARTCC> ARTCCs = new List<ARTCC> ( );
		public List<HighARTCC> HighARTCCs = new List<HighARTCC> ( );
		public List<LowARTCC> LowARTCCs = new List<LowARTCC> ( );
		public List<ProhibitArea> ProhibitAreas = new List<ProhibitArea> ( );
		public List<Runway> Runways = new List<Runway> ( );
		public List<SID> SIDs = new List<SID> ( );
		public List<STAR> STARs = new List<STAR> ( );

		public FIX GetFix ( string Name )
		{
			foreach ( FIX Fix in Fixes )
			{
				if ( Fix.Name == Name )
				{
					return Fix;
				}
			}
			return null;
		}

		public NDB GetNDB ( string Name )
		{
			foreach ( NDB Ndb in NDBs )
			{
				if ( Ndb.Name == Name )
				{
					return Ndb;
				}
			}
			return null;
		}

		public VOR GetVOR ( string Name )
		{
			foreach ( VOR Vor in VORs )
			{
				if ( Vor.Name == Name )
				{
					return Vor;
				}
			}
			return null;
		}

		public HighAirway GetHighAirway ( string Name )
		{
			foreach ( HighAirway Airway in HighAirways )
			{
				if ( Airway.Name == Name )
				{
					return Airway;
				}
			}
			return null;
		}

		public LowAirway GetLowAirway ( string Name )
		{
			foreach ( LowAirway Airway in LowAirways )
			{
				if ( Airway.Name == Name )
				{
					return Airway;
				}
			}
			return null;
		}

		public ARTCC GetARTCC ( string Name )
		{
			foreach ( ARTCC Artcc in ARTCCs )
			{
				if ( Artcc.Name == Name )
				{
					return Artcc;
				}
			}
			return null;
		}

		public HighARTCC GetHighARTCC ( string Name )
		{
			foreach ( HighARTCC HighArtcc in HighARTCCs )
			{
				if ( HighArtcc.Name == Name )
				{
					return HighArtcc;
				}
			}
			return null;
		}

		public LowARTCC GetLowARTCC ( string Name )
		{
			foreach ( LowARTCC LowArtcc in LowARTCCs )
			{
				if ( LowArtcc.Name == Name )
				{
					return LowArtcc;
				}
			}
			return null;
		}

		public ProhibitArea GetProhibitArea ( string Name )
		{
			foreach ( ProhibitArea prohibitArea in ProhibitAreas )
			{
				if ( prohibitArea.Name == Name )
				{
					return prohibitArea;
				}
			}
			return null;
		}
	}
}
