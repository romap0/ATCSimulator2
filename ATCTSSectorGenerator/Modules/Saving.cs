using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ATCTSPortableClassLibrary;

namespace ATCTrainingSimulatorSectorGenerator.Modules
{
	public static class Saving
	{
		static string SectorFilePath = MainWindow.SectorFilePath;
		static Sector MySector = MainWindow.MySector;

		public static void SaveSectorFile ( )
		{
			XmlWriterSettings settings = new XmlWriterSettings( )
			{
				NewLineOnAttributes = true,
				IndentChars = "	",
				OmitXmlDeclaration = true,
				Indent = true
			};

			using ( XmlWriter Sector = XmlWriter.Create( SectorFilePath, settings ) )
			{
				Sector.WriteStartDocument( );
				Sector.WriteStartElement( "Sector" );
				Sector.WriteAttributeString( "Name", MySector.Name );
				Sector.WriteAttributeString( "Version", MySector.Version.ToString( ) );
				Sector.WriteAttributeString( "MainAirport", MySector.MainAirport );
				Sector.WriteAttributeString( "CenterLatitude.ToString()", MySector.CenterLatitude.ToString().ToString() );
				Sector.WriteAttributeString( "CenterLongitude.ToString()", MySector.CenterLongitude.ToString().ToString( ) );
				Sector.WriteAttributeString( "MagneticVariation", MySector.MagneticVariation.ToString( ) );
				Sector.WriteAttributeString( "DefaultScale", MySector.DefaultScale.ToString( ) );
				Sector.WriteAttributeString( "TA", MySector.TA.ToString( ) );
				Sector.WriteAttributeString( "TL", MySector.TL.ToString( ) );

				Sector.WriteStartElement( "Fixes" );
				for ( int n = 0; n < MySector.Fixes.Count; n++ )
				{
					Sector.WriteStartElement( "FIX" );
					Sector.WriteAttributeString( "Name", MySector.Fixes [ n ].Name );
					Sector.WriteAttributeString( "Latitude.ToString()", MySector.Fixes [ n ].Latitude.ToString().ToString( ) );
					Sector.WriteAttributeString( "Longitude.ToString()", MySector.Fixes [ n ].Longitude.ToString().ToString( ) );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "VORs" );
				for ( int n = 0; n < MySector.VORs.Count; n++ )
				{
					Sector.WriteStartElement( "VOR" );
					Sector.WriteAttributeString( "Name", MySector.VORs [ n ].Name );
					Sector.WriteAttributeString( "Frequency", MySector.VORs [ n ].Frequency );
					Sector.WriteAttributeString( "Latitude.ToString()", MySector.VORs [ n ].Latitude.ToString().ToString( ) );
					Sector.WriteAttributeString( "Longitude.ToString()", MySector.VORs [ n ].Longitude.ToString().ToString( ) );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "NDBs" );
				for ( int n = 0; n < MySector.NDBs.Count; n++ )
				{
					Sector.WriteStartElement( "NDB" );
					Sector.WriteAttributeString( "Name", MySector.NDBs [ n ].Name );
					Sector.WriteAttributeString( "Frequency", MySector.NDBs [ n ].Frequency );
					Sector.WriteAttributeString( "Latitude.ToString()", MySector.NDBs [ n ].Latitude.ToString() );
					Sector.WriteAttributeString( "Longitude.ToString()", MySector.NDBs [ n ].Longitude.ToString() );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "Airports" );
				for ( int CurrentAirport = 0; CurrentAirport < MySector.Airports.Count; CurrentAirport++ )
				{
					Sector.WriteStartElement( "Airport" );
					Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Name );
					Sector.WriteAttributeString( "ICAO", MySector.Airports [ CurrentAirport ].ICAO );
					Sector.WriteAttributeString( "TowerFrequency", MySector.Airports [ CurrentAirport ].TowerFrequency );
					Sector.WriteAttributeString( "Latitude.ToString()", MySector.Airports [ CurrentAirport ].Latitude.ToString() );
					Sector.WriteAttributeString( "Longitude.ToString()", MySector.Airports [ CurrentAirport ].Longitude.ToString() );

					Sector.WriteStartElement( "Runways" );
					for ( int CurrentRunway = 0; CurrentRunway < MySector.Airports [ CurrentAirport ].Runways.Count; CurrentRunway++ )
					{
						Sector.WriteStartElement( "Runway" );
						Sector.WriteAttributeString( "Number", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].Number );
						Sector.WriteAttributeString( "ReciprocalNumber", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].ReciprocalNumber );
						Sector.WriteAttributeString( "Heading", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].Heading.ToString( ) );
						Sector.WriteAttributeString( "ReciprocalHeading", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].ReciprocalHeading.ToString( ) );
						Sector.WriteAttributeString( "StartLatitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].StartLatitude.ToString() );
						Sector.WriteAttributeString( "StartLongitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].StartLongitude.ToString() );
						Sector.WriteAttributeString( "EndLatitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].EndLatitude.ToString() );
						Sector.WriteAttributeString( "EndLongitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].EndLongitude.ToString() );

						Sector.WriteStartElement( "SIDs" );
						for ( int CurrentSID = 0; CurrentSID < MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs.Count; CurrentSID++ )
						{
							Sector.WriteStartElement( "SID" );
							Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Name );
							Sector.WriteAttributeString( "ReciprocalRunway", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].ReciprocalRunway.ToString( ) );

							Sector.WriteStartElement( "Fixes" );
							for ( int CurrentFIX = 0; CurrentFIX < MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes.Count; CurrentFIX++ )
							{
								Sector.WriteStartElement( "FIX" );
								Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Name );
								Sector.WriteAttributeString( "Latitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Latitude.ToString() );
								Sector.WriteAttributeString( "Longitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Longitude.ToString() );
								Sector.WriteAttributeString( "Altitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Altitude.ToString( ) );
								Sector.WriteAttributeString( "Speed", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Speed.ToString( ) );
								Sector.WriteAttributeString( "FlyOver", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].FlyOver.ToString( ) );
								Sector.WriteEndElement( );
							}
							Sector.WriteEndElement( );
							Sector.WriteEndElement( );
						}
						Sector.WriteEndElement( );

						Sector.WriteStartElement( "STARs" );
						for ( int CurrentSTAR = 0; CurrentSTAR < MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs.Count; CurrentSTAR++ )
						{
							Sector.WriteStartElement( "STAR" );
							Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Name );
							Sector.WriteAttributeString( "ReciprocalRunway", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].ReciprocalRunway.ToString( ) );

							Sector.WriteStartElement( "Fixes" );
							for ( int CurrentFIX = 0; CurrentFIX < MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes.Count; CurrentFIX++ )
							{
								Sector.WriteStartElement( "FIX" );
								Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Name );
								Sector.WriteAttributeString( "Latitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Latitude.ToString() );
								Sector.WriteAttributeString( "Longitude.ToString()", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Longitude.ToString() );
								Sector.WriteAttributeString( "Altitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Altitude.ToString( ) );
								Sector.WriteAttributeString( "Speed", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Speed.ToString( ) );
								Sector.WriteEndElement( );
							}
							Sector.WriteEndElement( );
							Sector.WriteEndElement( );
						}
						Sector.WriteEndElement( );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "ARTCCs" );
				for ( int CurrentARTCC = 0; CurrentARTCC < MySector.ARTCCs.Count; CurrentARTCC++ )
				{
					Sector.WriteStartElement( "ARTCC" );
					Sector.WriteAttributeString( "Name", MySector.ARTCCs [ CurrentARTCC ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.ARTCCs [ CurrentARTCC ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Name", MySector.ARTCCs [ CurrentARTCC ].Fixes [ CurrentFIX ].Name );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.ARTCCs [ CurrentARTCC ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.ARTCCs [ CurrentARTCC ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "HighARTCCs" );
				for ( int CurrentHighARTCC = 0; CurrentHighARTCC < MySector.HighARTCCs.Count; CurrentHighARTCC++ )
				{
					Sector.WriteStartElement( "HighARTCC" );
					Sector.WriteAttributeString( "Name", MySector.HighARTCCs [ CurrentHighARTCC ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.HighARTCCs [ CurrentHighARTCC ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Name", MySector.HighARTCCs [ CurrentHighARTCC ].Fixes [ CurrentFIX ].Name );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.HighARTCCs [ CurrentHighARTCC ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.HighARTCCs [ CurrentHighARTCC ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "LowARTCCs" );
				for ( int CurrentLowARTCC = 0; CurrentLowARTCC < MySector.LowARTCCs.Count; CurrentLowARTCC++ )
				{
					Sector.WriteStartElement( "LowARTCC" );
					Sector.WriteAttributeString( "Name", MySector.LowARTCCs [ CurrentLowARTCC ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.LowARTCCs [ CurrentLowARTCC ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Name", MySector.LowARTCCs [ CurrentLowARTCC ].Fixes [ CurrentFIX ].Name );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.LowARTCCs [ CurrentLowARTCC ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.LowARTCCs [ CurrentLowARTCC ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "HighAirways" );
				for ( int CurrentHighAirway = 0; CurrentHighAirway < MySector.HighAirways.Count; CurrentHighAirway++ )
				{
					Sector.WriteStartElement( "HighAirway" );
					Sector.WriteAttributeString( "Name", MySector.HighAirways [ CurrentHighAirway ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.HighAirways [ CurrentHighAirway ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Name", MySector.HighAirways [ CurrentHighAirway ].Fixes [ CurrentFIX ].Name );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.HighAirways [ CurrentHighAirway ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.HighAirways [ CurrentHighAirway ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "LowAirways" );
				for ( int CurrentLowAirway = 0; CurrentLowAirway < MySector.LowAirways.Count; CurrentLowAirway++ )
				{
					Sector.WriteStartElement( "LowAirway" );
					Sector.WriteAttributeString( "Name", MySector.LowAirways [ CurrentLowAirway ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.LowAirways [ CurrentLowAirway ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Name", MySector.LowAirways [ CurrentLowAirway ].Fixes [ CurrentFIX ].Name );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.LowAirways [ CurrentLowAirway ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.LowAirways [ CurrentLowAirway ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "ProhibitAreas" );
				for ( int CurrentProhibitArea = 0; CurrentProhibitArea < MySector.ProhibitAreas.Count; CurrentProhibitArea++ )
				{
					Sector.WriteStartElement( "ProhibitArea" );
					Sector.WriteAttributeString( "Name", MySector.ProhibitAreas [ CurrentProhibitArea ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.ProhibitAreas [ CurrentProhibitArea ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.ProhibitAreas [ CurrentProhibitArea ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.ProhibitAreas [ CurrentProhibitArea ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "Runways" );
				for ( int CurrentRunway = 0; CurrentRunway < MySector.Runways.Count; CurrentRunway++ )
				{
					Sector.WriteStartElement( "Runway" );
					Sector.WriteAttributeString( "Number", MySector.Runways [ CurrentRunway ].Number );
					Sector.WriteAttributeString( "ReciprocalNumber", MySector.Runways [ CurrentRunway ].ReciprocalNumber );
					Sector.WriteAttributeString( "Heading", MySector.Runways [ CurrentRunway ].Heading.ToString( ) );
					Sector.WriteAttributeString( "ReciprocalHeading", MySector.Runways [ CurrentRunway ].ReciprocalHeading.ToString( ) );
					Sector.WriteAttributeString( "StartLatitude.ToString()", MySector.Runways [ CurrentRunway ].StartLatitude.ToString() );
					Sector.WriteAttributeString( "StartLongitude.ToString()", MySector.Runways [ CurrentRunway ].StartLongitude.ToString() );
					Sector.WriteAttributeString( "EndLatitude.ToString()", MySector.Runways [ CurrentRunway ].EndLatitude.ToString() );
					Sector.WriteAttributeString( "EndLongitude.ToString()", MySector.Runways [ CurrentRunway ].EndLongitude.ToString() );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "SIDs" );
				for ( int CurrentSID = 0; CurrentSID < MySector.SIDs.Count; CurrentSID++ )
				{
					Sector.WriteStartElement( "SID" );
					Sector.WriteAttributeString( "Name", MySector.SIDs [ CurrentSID ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.SIDs [ CurrentSID ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "STARs" );
				for ( int CurrentSTAR = 0; CurrentSTAR < MySector.STARs.Count; CurrentSTAR++ )
				{
					Sector.WriteStartElement( "STAR" );
					Sector.WriteAttributeString( "Name", MySector.STARs [ CurrentSTAR ].Name );

					Sector.WriteStartElement( "Fixes" );
					for ( int CurrentFIX = 0; CurrentFIX < MySector.STARs [ CurrentSTAR ].Fixes.Count; CurrentFIX++ )
					{
						Sector.WriteStartElement( "FIX" );
						Sector.WriteAttributeString( "Latitude.ToString()", MySector.STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Latitude.ToString() );
						Sector.WriteAttributeString( "Longitude.ToString()", MySector.STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Longitude.ToString() );
						Sector.WriteEndElement( );
					}
					Sector.WriteEndElement( );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteEndElement( );
				Sector.WriteEndDocument( );
				// Сбрасываем буфферизированные данные
				Sector.Flush( );
				// Закрываем файл, с которым связан output
				Sector.Close( );
			}
		}
	}
}
