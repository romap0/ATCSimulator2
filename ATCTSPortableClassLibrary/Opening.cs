using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ATCTSPortableClassLibrary
{
	public static class Opening
{
    // Fields
    private static Sector MySector = new Sector();

    // Methods
    private static int GetIntFromCoord(string Coord)
    {
        int signFromChar = GetSignFromChar(Coord[0]);
        Coord = Coord.Remove(0, 1);
        Coord = Coord.Replace(".", "");
        Coord = Coord.Replace(",", "");
        Coord.PadRight(10, '0');
        return (int.Parse(Coord) * signFromChar);
    }

    private static int GetSignFromChar(char Char)
    {
        switch (Char)
        {
            case 'E':
                return 1;

            case 'N':
                return -1;

            case 'S':
                return 1;

            case 'W':
                return -1;
        }
        return 0;
    }

	public static Sector OpenSectorFile ( string RawSectorFileText )
	{
		XDocument document = XDocument.Parse( RawSectorFileText );
		MySector.Name = document.Root.Attribute( "Name" ).Value.ToString( );
		MySector.Version = double.Parse(document.Root.Attribute("Version").Value);
		MySector.MainAirport = document.Root.Attribute( "MainAirport" ).Value.ToString( );
		MySector.CenterLatitude = GetIntFromCoord( document.Root.Attribute( "CenterLatitude" ).Value.ToString( ) );
		MySector.CenterLongitude = GetIntFromCoord( document.Root.Attribute( "CenterLongitude" ).Value.ToString( ) );
		MySector.MagneticVariation = double.Parse( document.Root.Attribute( "MagneticVariation" ).Value.ToString( ) );
		MySector.DefaultScale = double.Parse( document.Root.Attribute( "DefaultScale" ).Value.ToString( ) );
		MySector.TA = int.Parse( document.Root.Attribute( "TA" ).Value.ToString( ) );
		MySector.TL = int.Parse( document.Root.Attribute( "TL" ).Value.ToString( ) );
		foreach ( XElement element in document.Root.Elements( ) )
		{
			switch ( element.Name.LocalName )
			{
				case "Fixes":
					{
						OpenFixes( element );
						continue;
					}
				case "VORs":
					{
						OpenVORs( element );
						continue;
					}
				case "NDBs":
					{
						OpenNDBs( element );
						continue;
					}
				case "Airports":
					{
						OpenAirports( element );
						continue;
					}
				case "HighAirways":
					{
						OpenHighAirways( element );
						continue;
					}
				case "LowAirways":
					{
						OpenLowAirways( element );
						continue;
					}
				case "ARTCCs":
					{
						OpenARTCCs( element );
						continue;
					}
				case "HighARTCCs":
					{
						OpenHighARTCCs( element );
						continue;
					}
				case "LowARTCCs":
					{
						OpenLowARTCCs( element );
						continue;
					}
				case "ProhibitAreas":
					{
						OpenProhibitAreas( element );
						continue;
					}
				case "Runways":
					{
						OpenRunways( element );
						continue;
					}
				case "SIDs":
					{
						OpenSIDs( element );
						continue;
					}
				case "STARs":
					{
						OpenSTARs( element );
						continue;
					}
			}
		}
		return MySector;
	}

    private static void OpenAirports(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            Airport item = new Airport {
                Name = element.Attribute("Name").Value.ToString(),
                ICAO = element.Attribute("ICAO").Value.ToString(),
                TowerFrequency = element.Attribute("TowerFrequency").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.Airports.Add(item);
            if (element.Element("Runways").HasElements)
            {
                OpenRunwaysForAirports(element.Element("Runways"));
            }
        }
    }

    private static void OpenARTCCs(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            ARTCC item = new ARTCC {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.ARTCCs.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForARTCC(element.Element("Fixes"));
            }
        }
    }

    private static void OpenFixes(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.Fixes.Add(item);
        }
    }

    private static void OpenFixesForARTCC(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.ARTCCs[MySector.ARTCCs.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForHighAirway(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.HighAirways[MySector.HighAirways.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForHighARTCC(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.HighARTCCs[MySector.HighARTCCs.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForLowAirway(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.LowAirways[MySector.LowAirways.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForLowARTCC(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.LowARTCCs[MySector.LowARTCCs.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForProhibitArea(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.ProhibitAreas[MySector.ProhibitAreas.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForSID(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.SIDs[MySector.SIDs.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForSIDForRunway(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString()),
                Altitude = int.Parse(element.Attribute("Altitude").Value.ToString()),
                Speed = int.Parse(element.Attribute("Speed").Value.ToString()),
                FlyOver = bool.Parse(element.Attribute("FlyOver").Value.ToString())
            };
            MySector.Airports[MySector.Airports.Count - 1].Runways[MySector.Airports[MySector.Airports.Count - 1].Runways.Count - 1].SIDs[MySector.Airports[MySector.Airports.Count - 1].Runways[MySector.Airports[MySector.Airports.Count - 1].Runways.Count - 1].SIDs.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForSTAR(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.STARs[MySector.STARs.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenFixesForSTARForRunway(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            FIX item = new FIX {
                Name = element.Attribute("Name").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString()),
                Altitude = int.Parse(element.Attribute("Altitude").Value.ToString()),
                Speed = int.Parse(element.Attribute("Speed").Value.ToString()),
                FlyOver = bool.Parse(element.Attribute("FlyOver").Value.ToString())
            };
            MySector.Airports[MySector.Airports.Count - 1].Runways[MySector.Airports[MySector.Airports.Count - 1].Runways.Count - 1].STARs[MySector.Airports[MySector.Airports.Count - 1].Runways[MySector.Airports[MySector.Airports.Count - 1].Runways.Count - 1].STARs.Count - 1].Fixes.Add(item);
        }
    }

    private static void OpenHighAirways(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            HighAirway item = new HighAirway {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.HighAirways.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForHighAirway(element.Element("Fixes"));
            }
        }
    }

    private static void OpenHighARTCCs(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            HighARTCC item = new HighARTCC {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.HighARTCCs.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForHighARTCC(element.Element("Fixes"));
            }
        }
    }

    private static void OpenLowAirways(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            LowAirway item = new LowAirway {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.LowAirways.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForLowAirway(element.Element("Fixes"));
            }
        }
    }

    private static void OpenLowARTCCs(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            LowARTCC item = new LowARTCC {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.LowARTCCs.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForLowARTCC(element.Element("Fixes"));
            }
        }
    }

    private static void OpenNDBs(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            NDB item = new NDB {
                Name = element.Attribute("Name").Value.ToString(),
                Frequency = element.Attribute("Frequency").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.NDBs.Add(item);
        }
    }

    private static void OpenProhibitAreas(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            ProhibitArea item = new ProhibitArea {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.ProhibitAreas.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForProhibitArea(element.Element("Fixes"));
            }
        }
    }

    private static void OpenRunways(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            Runway item = new Runway {
                Number = element.Attribute("Number").Value.ToString(),
                ReciprocalNumber = element.Attribute("ReciprocalNumber").Value.ToString(),
                Heading = int.Parse(element.Attribute("Heading").Value.ToString()),
                ReciprocalHeading = int.Parse(element.Attribute("ReciprocalHeading").Value.ToString()),
                StartLatitude = GetIntFromCoord(element.Attribute("StartLatitude").Value.ToString()),
                StartLongitude = GetIntFromCoord(element.Attribute("StartLongitude").Value.ToString()),
                EndLatitude = GetIntFromCoord(element.Attribute("EndLatitude").Value.ToString()),
                EndLongitude = GetIntFromCoord(element.Attribute("EndLongitude").Value.ToString())
            };
            MySector.Runways.Add(item);
        }
    }

    private static void OpenRunwaysForAirports(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            Runway item = new Runway {
                Number = element.Attribute("Number").Value.ToString(),
                ReciprocalNumber = element.Attribute("ReciprocalNumber").Value.ToString(),
                Heading = int.Parse(element.Attribute("Heading").Value.ToString()),
                ReciprocalHeading = int.Parse(element.Attribute("ReciprocalHeading").Value.ToString()),
                StartLatitude = GetIntFromCoord(element.Attribute("StartLatitude").Value.ToString()),
                StartLongitude = GetIntFromCoord(element.Attribute("StartLongitude").Value.ToString()),
                EndLatitude = GetIntFromCoord(element.Attribute("EndLatitude").Value.ToString()),
                EndLongitude = GetIntFromCoord(element.Attribute("EndLongitude").Value.ToString())
            };
            MySector.Airports[MySector.Airports.Count - 1].Runways.Add(item);
            foreach (XElement element2 in element.Elements())
            {
                string str = element2.Name.LocalName;
                if (str != null)
                {
                    if (!(str == "SIDs"))
                    {
                        if (str == "STARs")
                        {
                            goto Label_0194;
                        }
                    }
                    else if (element2.HasElements)
                    {
                        OpenSIDsForRunway(element2);
                    }
                }
                continue;
            Label_0194:
                if (element2.HasElements)
                {
                    OpenSTARsForRunway(element2);
                }
            }
        }
    }

    private static void OpenSIDs(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            SID item = new SID {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.SIDs.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForSID(element.Element("Fixes"));
            }
        }
    }

    private static void OpenSIDsForRunway(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            SID item = new SID {
                Name = element.Attribute("Name").Value.ToString(),
                ReciprocalRunway = bool.Parse(element.Attribute("ReciprocalRunway").Value.ToString())
            };
            MySector.Airports[MySector.Airports.Count - 1].Runways[MySector.Airports[MySector.Airports.Count - 1].Runways.Count - 1].SIDs.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForSIDForRunway(element.Element("Fixes"));
            }
        }
    }

    private static void OpenSTARs(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            STAR item = new STAR {
                Name = element.Attribute("Name").Value.ToString()
            };
            MySector.STARs.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForSTAR(element.Element("Fixes"));
            }
        }
    }

    private static void OpenSTARsForRunway(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            STAR item = new STAR {
                Name = element.Attribute("Name").Value.ToString(),
                ReciprocalRunway = bool.Parse(element.Attribute("ReciprocalRunway").Value.ToString())
            };
            MySector.Airports[MySector.Airports.Count - 1].Runways[MySector.Airports[MySector.Airports.Count - 1].Runways.Count - 1].STARs.Add(item);
            if (element.Element("Fixes").HasElements)
            {
                OpenFixesForSTARForRunway(element.Element("Fixes"));
            }
        }
    }

    private static void OpenVORs(XElement CurrentSection)
    {
        foreach (XElement element in CurrentSection.Elements())
        {
            VOR item = new VOR {
                Name = element.Attribute("Name").Value.ToString(),
                Frequency = element.Attribute("Frequency").Value.ToString(),
                Latitude = GetIntFromCoord(element.Attribute("Latitude").Value.ToString()),
                Longitude = GetIntFromCoord(element.Attribute("Longitude").Value.ToString())
            };
            MySector.VORs.Add(item);
        }
    }
}
}
