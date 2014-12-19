using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Threading;
using ATCTSPortableClassLibrary;

namespace ATCTrainingSimulatorSectorGenerator
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Stream ReadFileStream = null;
		static List<string> IVACSectorFileLines = null;
		public static Sector MySector = new Sector( );
		Thread ImportSectorThread = null;

		public static string SectorFilePath = null;

		public MainWindow ( )
		{
			InitializeComponent( );
		}

		private void btnImportClick ( object sender, RoutedEventArgs e )
		{
			System.Windows.Forms.OpenFileDialog ImportFileDialog = new System.Windows.Forms.OpenFileDialog( )
			{
				Multiselect = false,
				Filter = "IVAC Sector files (*.sct)|*.sct"
			};
			ImportFileDialog.FileOk += ImportFileDialogFileOk;
			ImportFileDialog.ShowDialog( );
			if ( ImportFileDialog.FileName != "" )
			{
				ImportSectorThread.Join( );
				ImportSectorThread = null;
				FillWindow( );
				btnSave.IsEnabled = true;
				btnSaveAs.IsEnabled = true;
				btnExport.IsEnabled = true;
			}
		}

		private void ImportFileDialogFileOk ( object sender, System.ComponentModel.CancelEventArgs e )
		{
			SectorFilePath = null;
			ReadLinesFromIVACSectorFile( sender );
			ImportSectorThread = new Thread( ImportIVACSectorFile );
			ImportSectorThread.Start( );
		}

		private static void ImportIVACSectorFile ( )
		{
			MySector = new Sector( );
			ImportSectorInfo( );
			ImportVORs( );
			ImportNDBs( );
			ImportFixes( );
			ImportAirports( );
			ImportRunways( );
			ImportSIDs( );
			ImportSTARs( );
			ImportHighAirways( );
			ImportLowAirways( );
			ImportARTCCs( );
			ImportHighARTCCs( );
			ImportLowARTCCs( );
			ImportProhibitAreas( );
		}

		public void FillWindow ( )
		{
			FillSectorSettings( );
			FillAirports( );
			FillVORs( );
			FillNDBs( );
			FillFixes( );
			FillHighAirways( );
			FillLowAirways( );
			FillARTCCs( );
			FillHighARTCCs( );
			FillLowARTCCs( );
			FillProhibitAreas( );
		}

		private void FillSectorSettings ( )
		{
			txtName.Text = MySector.Name;
			txtVersion.Text = MySector.Version.ToString( );
			txtMainAirport.Text = MySector.MainAirport;
			txtCenterLatitude.Text = MySector.CenterLatitude.ToString( );
			txtCenterLongitude.Text = MySector.CenterLongitude.ToString( );
			txtMagneticVariation.Text = MySector.MagneticVariation.ToString( );
			txtDefaultScale.Text = MySector.DefaultScale.ToString( );
			txtTA.Text = MySector.TA.ToString( );
			txtTL.Text = MySector.TL.ToString( );
		}

		private void FillAirports ( )
		{
			dgvAirports.Rows.Clear( );

			foreach ( Airport CurrentAirport in MySector.Airports )
			{
				dgvAirports.Rows.Add( CurrentAirport.Name, CurrentAirport.ICAO, CurrentAirport.TowerFrequency, CurrentAirport.Latitude, CurrentAirport.Longitude, String.Format( "Add/Edit [{0}]", CurrentAirport.Runways.Count ), "Delete" );
			}
		}

		private void FillVORs ( )
		{
			dgvVORs.Rows.Clear( );

			foreach ( VOR CurrentVOR in MySector.VORs )
			{
				dgvVORs.Rows.Add( CurrentVOR.Name, CurrentVOR.Frequency, CurrentVOR.Latitude, CurrentVOR.Longitude, "Delete" );
			}
		}

		private void FillNDBs ( )
		{
			dgvNDBs.Rows.Clear( );

			foreach ( NDB CurrentNDB in MySector.NDBs )
			{
				dgvNDBs.Rows.Add( CurrentNDB.Name, CurrentNDB.Frequency, CurrentNDB.Latitude, CurrentNDB.Longitude, "Delete" );
			}
		}

		private void FillFixes ( )
		{
			dgvFixes.Rows.Clear( );

			foreach ( FIX CurrentFIX in MySector.Fixes )
			{
				dgvFixes.Rows.Add( CurrentFIX.Name, CurrentFIX.Latitude, CurrentFIX.Longitude, "Delete" );
			}
		}

		private void FillHighAirways ( )
		{
			dgvHighAirways.Rows.Clear( );

			foreach ( HighAirway CurrentHighAirway in MySector.HighAirways )
			{
				dgvHighAirways.Rows.Add( CurrentHighAirway.Name, String.Format( "Add/Edit [{0}]", CurrentHighAirway.Fixes.Count ), "Delete" );
			}
		}

		private void FillLowAirways ( )
		{
			dgvLowAirways.Rows.Clear( );

			foreach ( LowAirway CurrentLowAirway in MySector.LowAirways )
			{
				dgvLowAirways.Rows.Add( CurrentLowAirway.Name, String.Format( "Add/Edit [{0}]", CurrentLowAirway.Fixes.Count ), "Delete" );
			}
		}

		private void FillARTCCs ( )
		{
			dgvARTCCs.Rows.Clear( );

			foreach ( ARTCC CurrentARTCC in MySector.ARTCCs )
			{
				dgvARTCCs.Rows.Add( CurrentARTCC.Name, String.Format( "Add/Edit [{0}]", CurrentARTCC.Fixes.Count ), "Delete" );
			}
		}

		private void FillHighARTCCs ( )
		{
			dgvHighARTCCs.Rows.Clear( );

			foreach ( HighARTCC CurrentHighARTCC in MySector.HighARTCCs )
			{
				dgvHighARTCCs.Rows.Add( CurrentHighARTCC.Name, String.Format( "Add/Edit [{0}]", CurrentHighARTCC.Fixes.Count ), "Delete" );
			}
		}

		private void FillLowARTCCs ( )
		{
			dgvLowARTCCs.Rows.Clear( );

			foreach ( LowARTCC CurrentLowARTCC in MySector.LowARTCCs )
			{
				dgvLowARTCCs.Rows.Add( CurrentLowARTCC.Name, String.Format( "Add/Edit [{0}]", CurrentLowARTCC.Fixes.Count ), "Delete" );
			}
		}

		private void FillProhibitAreas ( )
		{
			dgvProhibitAreas.Rows.Clear( );

			foreach ( ProhibitArea CurrentProhibitArea in MySector.ProhibitAreas )
			{
				dgvProhibitAreas.Rows.Add( CurrentProhibitArea.Name, String.Format( "Add/Edit [{0}]", CurrentProhibitArea.Fixes.Count ), "Delete" );
			}
		}

		static FIX GetFixFromLine ( string Line, int Index )
		{
			string ProcessedLine = Line.Substring( Index, 14 ).TrimEnd( );

			if ( MySector.GetVOR( ProcessedLine ) != null )
			{
				return MySector.GetVOR( ProcessedLine ).ToFIX( );
			}

			if ( MySector.GetNDB( ProcessedLine ) != null )
			{
				return MySector.GetNDB( ProcessedLine ).ToFIX( );
			}

			if ( MySector.GetFix( ProcessedLine ) != null )
			{
				return MySector.GetFix( ProcessedLine );
			}

			return new FIX( )
			{
				Latitude = int.Parse( Line.Substring( Index, 14 ) ),
				Longitude = int.Parse( Line.Substring( Index + 15, 14 ) )
			};
		}

		private static void ImportSectorInfo ( )
		{
			int StartOfInfoSection = IVACSectorFileLines.IndexOf( "[INFO]" );

			MySector.Name = IVACSectorFileLines [ StartOfInfoSection + 1 ];
			MySector.MainAirport = IVACSectorFileLines [ StartOfInfoSection + 3 ].Substring( 0, 4 );
			MySector.CenterLatitude = int.Parse( IVACSectorFileLines [ StartOfInfoSection + 4 ] );
			MySector.CenterLongitude = int.Parse( IVACSectorFileLines [ StartOfInfoSection + 5 ] );
			MySector.MagneticVariation = Convert.ToDouble( IVACSectorFileLines [ StartOfInfoSection + 8 ].Replace( '.', ',' ) );
			MySector.DefaultScale = 88;
		}

		private static void ImportVORs ( )
		{
			int StartOfVORSection = IVACSectorFileLines.IndexOf( "[VOR]" );

			for ( int n = StartOfVORSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 41 )
						{
							Line = Line = Line.PadRight( 41, ' ' );
						}

						AddVOR( Line );
					}
				}
			}
		}

		private static void AddVOR ( string Line )
		{
			MySector.VORs.Add( new VOR( )
			{
				Name = Line.Substring( 0, 3 ).TrimEnd( ),
				Frequency = Line.Substring( 4, 7 ),
				Latitude = int.Parse( Line.Substring( 12, 14 ) ),
				Longitude = int.Parse( Line.Substring( 27, 14 ) )
			} );
		}

		private static void ImportNDBs ( )
		{
			int StartOfNDBSection = IVACSectorFileLines.IndexOf( "[NDB]" );

			for ( int n = StartOfNDBSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 43 )
						{
							Line = Line.PadRight( 43, ' ' );
						}

						AddNDB( Line );
					}
				}
			}
		}

		private static void AddNDB ( string Line )
		{
			MySector.NDBs.Add( new NDB( )
			{
				Name = Line.Substring( 0, 3 ).TrimEnd( ),
				Frequency = Line.Substring( 6, 7 ).Trim( ),
				Latitude = int.Parse( Line.Substring( 14, 14 ) ),
				Longitude = int.Parse( Line.Substring( 29, 14 ) )
			} );
		}

		private static void ImportFixes ( )
		{
			int StartOfFIXSection = IVACSectorFileLines.IndexOf( "[FIXES]" );

			for ( int n = StartOfFIXSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 35 )
						{
							Line = Line.PadRight( 35, ' ' );
						}

						AddFIX( Line );
					}
				}
			}
		}

		private static void AddFIX ( string Line )
		{
			MySector.Fixes.Add( new FIX( )
			{
				Name = Line.Substring( 0, 5 ).TrimEnd( ),
				Latitude = int.Parse( Line.Substring( 6, 14 ) ),
				Longitude = int.Parse( Line.Substring( 21, 14 ) )
			} );
		}

		private static void ImportAirports ( )
		{
			int StartOfAirportSection = IVACSectorFileLines.IndexOf( "[AIRPORT]" );

			for ( int n = StartOfAirportSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 42 )
						{
							Line = Line.PadRight( 42, ' ' );
						}

						AddAirport( Line );
					}
				}
			}
		}

		private static void AddAirport ( string Line )
		{
			MySector.Airports.Add( new Airport( )
			{
				ICAO = Line.Substring( 0, 4 ),
				TowerFrequency = Line.Substring( 5, 7 ),
				Latitude = int.Parse( Line.Substring( 13, 14 ) ),
				Longitude = int.Parse( Line.Substring( 28, 14 ) )
			} );
		}

		private static void ImportRunways ( )
		{
			int StartOfRunwaySection = IVACSectorFileLines.IndexOf( "[RUNWAY]" );

			for ( int n = StartOfRunwaySection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 75 )
						{
							Line = Line.PadRight( 75, ' ' );
						}

						AddRunway( Line );
					}
				}
			}
		}

		private static void AddRunway ( string Line )
		{
			MySector.Runways.Add( new Runway( )
			{
				Number = Line.Substring( 0, 3 ).TrimEnd( ),
				ReciprocalNumber = Line.Substring( 4, 3 ),
				Heading = Convert.ToInt16( Line.Substring( 8, 3 ) ),
				ReciprocalHeading = Convert.ToInt16( Line.Substring( 12, 3 ) ),
				StartLatitude = int.Parse( Line.Substring( 16, 14 ) ),
				StartLongitude = int.Parse( Line.Substring( 31, 14 ) ),
				EndLatitude = int.Parse( Line.Substring( 46, 14 ) ),
				EndLongitude = int.Parse( Line.Substring( 61, 14 ) )
			} );
		}

		private static void ImportSIDs ( )
		{
			int StartOfSIDSection = IVACSectorFileLines.IndexOf( "[SID]" );

			for ( int n = StartOfSIDSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 85 )
						{
							Line = Line.PadRight( 85, ' ' );
						}

						AddSID( Line );
					}
				}
			}
		}

		private static void AddSID ( string Line )
		{
			if ( Line.Substring( 0, 15 ).Trim( ) != "" )
			{
				MySector.SIDs.Add( new SID( )
				{
					Name = Line.Substring( 0, 15 ).TrimEnd( )
				} );

				MySector.SIDs [ MySector.SIDs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
				MySector.SIDs [ MySector.SIDs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 56 ) );
			}
			else
			{
				MySector.SIDs [ MySector.SIDs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
			}
		}

		private static void ImportSTARs ( )
		{
			int StartOfSTARSection = IVACSectorFileLines.IndexOf( "[STAR]" );

			for ( int n = StartOfSTARSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 85 )
						{
							Line = Line.PadRight( 85, ' ' );
						}

						AddSTAR( Line );
					}
				}
			}
		}

		private static void AddSTAR ( string Line )
		{
			string NameOfSTAR = Line.Substring( 0, 25 ).Trim( );
			if ( NameOfSTAR.Length != 0 )
			{
				MySector.STARs.Add( new STAR( )
				{
					Name = NameOfSTAR
				} );

				MySector.STARs [ MySector.STARs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
				MySector.STARs [ MySector.STARs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 56 ) );
			}
			else
			{
				MySector.STARs [ MySector.STARs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
			}
		}

		private static void ImportHighAirways ( )
		{
			int StartOfHighAirwaySection = IVACSectorFileLines.IndexOf( "[HIGH AIRWAY]" );

			for ( int n = StartOfHighAirwaySection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 85 )
						{
							Line = Line.PadRight( 85, ' ' );
						}

						AddHighAirway( Line );
					}
				}
			}
		}

		private static void AddHighAirway ( string Line )
		{
			if ( MySector.GetHighAirway( Line.Substring( 0, 25 ).Trim( ) ) == null )
			{
				MySector.HighAirways.Add( new HighAirway( )
				{
					Name = Line.Substring( 0, 25 ).TrimEnd( )
				} );

				MySector.HighAirways [ MySector.HighAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
				MySector.HighAirways [ MySector.HighAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 56 ) );
			}
			else
			{
				MySector.HighAirways [ MySector.HighAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
				MySector.HighAirways [ MySector.HighAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 56 ) );
			}
		}

		private static void ImportLowAirways ( )
		{
			int StartOfLowAirwaySection = IVACSectorFileLines.IndexOf( "[LOW AIRWAY]" );

			for ( int n = StartOfLowAirwaySection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 85 )
						{
							Line = Line.PadRight( 85, ' ' );
						}

						AddLowAirway( Line );
					}
				}
			}
		}

		private static void AddLowAirway ( string Line )
		{
			if ( MySector.GetLowAirway( Line.Substring( 0, 25 ).Trim( ) ) == null )
			{
				MySector.LowAirways.Add( new LowAirway( )
				{
					Name = Line.Substring( 0, 25 ).TrimEnd( )
				} );

				MySector.LowAirways [ MySector.LowAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
				MySector.LowAirways [ MySector.LowAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 56 ) );
			}
			else
			{
				MySector.LowAirways [ MySector.LowAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 26 ) );
				MySector.LowAirways [ MySector.LowAirways.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 56 ) );
			}
		}

		private static void ImportARTCCs ( )
		{
			int StartOfARTCCSection = IVACSectorFileLines.IndexOf( "[ARTCC]" );

			for ( int n = StartOfARTCCSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 70 )
						{
							Line = Line.PadRight( 70, ' ' );
						}

						AddARTCC( Line );
					}
				}
			}
		}

		private static void AddARTCC ( string Line )
		{
			if ( MySector.GetARTCC( Line.Substring( 0, 10 ).Trim( ) ) == null && Line.Substring( 0, 10 ).Trim( ) != "" )
			{
				MySector.ARTCCs.Add( new ARTCC( )
				{
					Name = Line.Substring( 0, 10 ).TrimEnd( )
				} );

				MySector.ARTCCs [ MySector.ARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 11 ) );
				MySector.ARTCCs [ MySector.ARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 41 ) );
			}
			else
			{
				MySector.ARTCCs [ MySector.ARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 41 ) );
			}
		}

		private static void ImportHighARTCCs ( )
		{
			int StartOfHighARTCCSection = IVACSectorFileLines.IndexOf( "[ARTCC HIGH]" );

			for ( int n = StartOfHighARTCCSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 70 )
						{
							Line = Line.PadRight( 70, ' ' );
						}

						AddHighARTCC( Line );
					}
				}
			}
		}

		private static void AddHighARTCC ( string Line )
		{
			if ( MySector.GetHighARTCC( Line.Substring( 0, 10 ).Trim( ) ) == null && Line.Substring( 0, 10 ).Trim( ) != "" )
			{
				MySector.HighARTCCs.Add( new HighARTCC( )
				{
					Name = Line.Substring( 0, 10 ).Trim( )
				} );

				MySector.HighARTCCs [ MySector.HighARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 11 ) );
				MySector.HighARTCCs [ MySector.HighARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 41 ) );
			}
			else
			{
				MySector.HighARTCCs [ MySector.HighARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 41 ) );
			}
		}

		private static void ImportLowARTCCs ( )
		{
			int StartOfLowARTCCSection = IVACSectorFileLines.IndexOf( "[ARTCC LOW]" );

			for ( int n = StartOfLowARTCCSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 70 )
						{
							Line = Line.PadRight( 70, ' ' );
						}

						AddLowARTCC( Line );
					}
				}
			}
		}

		private static void AddLowARTCC ( string Line )
		{
			if ( MySector.GetLowARTCC( Line.Substring( 0, 10 ).Trim( ) ) == null && Line.Substring( 0, 10 ).Trim( ) != "" )
			{
				MySector.LowARTCCs.Add( new LowARTCC( )
				{
					Name = Line.Substring( 0, 10 ).TrimEnd( )
				} );

				MySector.LowARTCCs [ MySector.LowARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 11 ) );
				MySector.LowARTCCs [ MySector.LowARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 41 ) );
			}
			else
			{
				MySector.LowARTCCs [ MySector.LowARTCCs.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 41 ) );
			}
		}

		private static void ImportProhibitAreas ( )
		{
			int StartOfProhibitAreaSection = IVACSectorFileLines.IndexOf( "[GEO]" );

			for ( int n = StartOfProhibitAreaSection + 1; n < IVACSectorFileLines.Count; n++ )
			{
				string Line = IVACSectorFileLines [ n ];

				if ( isEndOfSection( Line ) )
				{
					n = IVACSectorFileLines.Count;	//End of For Method
				}
				else
				{
					if ( !isComment( Line ) )
					{
						if ( Line.Length < 74 )
						{
							Line = Line.PadRight( 74, ' ' );
						}

						if ( Line.Substring( 60, 14 ).Trim( ) == "PROHIBIT" )
						{
							AddProhibitArea( Line );
						}
					}
				}
			}
		}

		private static void AddProhibitArea ( string Line )
		{
			if ( MySector.ProhibitAreas.Count == 0 )
			{
				MySector.ProhibitAreas.Add( new ProhibitArea( )
				{
					Name = "ProhibitArea0"
				} );

				MySector.ProhibitAreas [ MySector.ProhibitAreas.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 0 ) );
				MySector.ProhibitAreas [ MySector.ProhibitAreas.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 30 ) );
			}
			else
			{
				MySector.ProhibitAreas [ MySector.ProhibitAreas.Count - 1 ].Fixes.Add( GetFixFromLine( Line, 30 ) );
			}
		}

		private static bool isEndOfSection ( string Line )
		{
			if ( Line.Trim( ) != "" )
			{
				return Line [ 0 ] == '[';
			}
			else
			{
				return false;
			}
		}

		private static bool isComment ( string Line )
		{
			if ( Line.Trim( ) != "" )
			{
				return Line [ 0 ] == ';' || Line [ 0 ] == '#' || Line [ 0 ] == ':';
			}
			else
			{
				return true;
			}
		}

		private void ReadLinesFromIVACSectorFile ( object sender )
		{
			System.Windows.Forms.OpenFileDialog ImportFileDialog = ( System.Windows.Forms.OpenFileDialog ) sender;
			string SectorFilePath = ImportFileDialog.FileName;

			try
			{
				using ( ReadFileStream )
				{
					IVACSectorFileLines = File.ReadLines( SectorFilePath ).ToList<string>( );
					for ( int n = 0; n < IVACSectorFileLines.Count; n++ )
					{
						IVACSectorFileLines [ n ] = IVACSectorFileLines [ n ].ToUpper( );
						IVACSectorFileLines [ n ] = IVACSectorFileLines [ n ].Replace( ',', '.' );
					}
				}
			}
			catch
			{
				MessageBox.Show( "Can not read IVAC sector file" );
			}
		}

		private void btnSaveClick ( object sender, RoutedEventArgs e )
		{
			if ( SectorFilePath == null )
			{
				Microsoft.Win32.SaveFileDialog SaveFileDialog = new Microsoft.Win32.SaveFileDialog( )
				{
					Filter = "ATCTS Sector files (*.sector)|*.sector"
				};
				SaveFileDialog.FileOk += SaveFileDialog_FileOk;
				SaveFileDialog.ShowDialog( );
			}
			else
			{
				Modules.Saving.SaveSectorFile( );
			}
		}

		private void SaveFileDialog_FileOk ( object sender, System.ComponentModel.CancelEventArgs e )
		{
			Microsoft.Win32.SaveFileDialog SaveFileDialog = ( Microsoft.Win32.SaveFileDialog ) sender;
			SectorFilePath = SaveFileDialog.FileName;
			Modules.Saving.SaveSectorFile( );
		}

		private void ExportSectorFile ( )
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
				Sector.WriteAttributeString( "Version", MySector.Version.ToString( ) );
				Sector.WriteAttributeString( "Name", MySector.Name );
				Sector.WriteAttributeString( "MainAirport", MySector.MainAirport );
				Sector.WriteAttributeString( "CenterLatitude", MySector.CenterLatitude.ToString( ) );
				Sector.WriteAttributeString( "CenterLongitude", MySector.CenterLongitude.ToString( ) );
				Sector.WriteAttributeString( "MagneticVariation", MySector.MagneticVariation.ToString( ) );
				Sector.WriteAttributeString( "DefaultScale", MySector.DefaultScale.ToString( ) );
				Sector.WriteAttributeString( "TA", MySector.TA.ToString( ) );
				Sector.WriteAttributeString( "TL", MySector.TL.ToString( ) );

				Sector.WriteStartElement( "Fixes" );
				for ( int n = 0; n < MySector.Fixes.Count; n++ )
				{
					Sector.WriteStartElement( "FIX" );
					Sector.WriteAttributeString( "Name", MySector.Fixes [ n ].Name );
					Sector.WriteAttributeString( "Latitude", MySector.Fixes [ n ].Latitude.ToString( ) );
					Sector.WriteAttributeString( "Longitude", MySector.Fixes [ n ].Longitude.ToString( ) );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "VORs" );
				for ( int n = 0; n < MySector.VORs.Count; n++ )
				{
					Sector.WriteStartElement( "VOR" );
					Sector.WriteAttributeString( "Name", MySector.VORs [ n ].Name );
					Sector.WriteAttributeString( "Frequency", MySector.VORs [ n ].Frequency );
					Sector.WriteAttributeString( "Latitude", MySector.VORs [ n ].Latitude.ToString( ) );
					Sector.WriteAttributeString( "Longitude", MySector.VORs [ n ].Longitude.ToString( ) );
					Sector.WriteEndElement( );
				}
				Sector.WriteEndElement( );

				Sector.WriteStartElement( "NDBs" );
				for ( int n = 0; n < MySector.NDBs.Count; n++ )
				{
					Sector.WriteStartElement( "NDB" );
					Sector.WriteAttributeString( "Name", MySector.NDBs [ n ].Name );
					Sector.WriteAttributeString( "Frequency", MySector.NDBs [ n ].Frequency );
					Sector.WriteAttributeString( "Latitude", MySector.NDBs [ n ].Latitude.ToString( ) );
					Sector.WriteAttributeString( "Longitude", MySector.NDBs [ n ].Longitude.ToString( ) );
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
					Sector.WriteAttributeString( "Latitude", MySector.Airports [ CurrentAirport ].Latitude.ToString( ) );
					Sector.WriteAttributeString( "Longitude", MySector.Airports [ CurrentAirport ].Longitude.ToString( ) );

					Sector.WriteStartElement( "Runways" );
					for ( int CurrentRunway = 0; CurrentRunway < MySector.Airports [ CurrentAirport ].Runways.Count; CurrentRunway++ )
					{
						Sector.WriteStartElement( "Runway" );
						Sector.WriteAttributeString( "Number", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].Number );
						Sector.WriteAttributeString( "ReciprocalNumber", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].ReciprocalNumber );
						Sector.WriteAttributeString( "Heading", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].Heading.ToString( ) );
						Sector.WriteAttributeString( "ReciprocalHeading", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].ReciprocalHeading.ToString( ) );
						Sector.WriteAttributeString( "StartLatitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].StartLatitude.ToString( ) );
						Sector.WriteAttributeString( "StartLongitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].StartLongitude.ToString( ) );
						Sector.WriteAttributeString( "EndLatitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].EndLatitude.ToString( ) );
						Sector.WriteAttributeString( "EndLongitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].EndLongitude.ToString( ) );

						Sector.WriteStartElement( "SIDs" );
						for ( int CurrentSID = 0; CurrentSID < MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs.Count; CurrentSID++ )
						{
							Sector.WriteStartElement( "SID" );
							Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Name );

							Sector.WriteStartElement( "Fixes" );
							for ( int CurrentFIX = 0; CurrentFIX < MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes.Count; CurrentFIX++ )
							{
								Sector.WriteStartElement( "FIX" );
								Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Name );
								Sector.WriteAttributeString( "Latitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
								Sector.WriteAttributeString( "Longitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
								Sector.WriteAttributeString( "Altitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Altitude.ToString( ) );
								Sector.WriteAttributeString( "Speed", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].SIDs [ CurrentSID ].Fixes [ CurrentFIX ].Speed.ToString( ) );
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

							Sector.WriteStartElement( "Fixes" );
							for ( int CurrentFIX = 0; CurrentFIX < MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes.Count; CurrentFIX++ )
							{
								Sector.WriteStartElement( "FIX" );
								Sector.WriteAttributeString( "Name", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Name );
								Sector.WriteAttributeString( "Latitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
								Sector.WriteAttributeString( "Longitude", MySector.Airports [ CurrentAirport ].Runways [ CurrentRunway ].STARs [ CurrentSTAR ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
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
						Sector.WriteAttributeString( "Latitude", MySector.ARTCCs [ CurrentARTCC ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
						Sector.WriteAttributeString( "Longitude", MySector.ARTCCs [ CurrentARTCC ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
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
						Sector.WriteAttributeString( "Latitude", MySector.HighARTCCs [ CurrentHighARTCC ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
						Sector.WriteAttributeString( "Longitude", MySector.HighARTCCs [ CurrentHighARTCC ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
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
						Sector.WriteAttributeString( "Latitude", MySector.LowARTCCs [ CurrentLowARTCC ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
						Sector.WriteAttributeString( "Longitude", MySector.LowARTCCs [ CurrentLowARTCC ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
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
						Sector.WriteAttributeString( "Latitude", MySector.HighAirways [ CurrentHighAirway ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
						Sector.WriteAttributeString( "Longitude", MySector.HighAirways [ CurrentHighAirway ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
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
						Sector.WriteAttributeString( "Latitude", MySector.LowAirways [ CurrentLowAirway ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
						Sector.WriteAttributeString( "Longitude", MySector.LowAirways [ CurrentLowAirway ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
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
						Sector.WriteAttributeString( "Latitude", MySector.ProhibitAreas [ CurrentProhibitArea ].Fixes [ CurrentFIX ].Latitude.ToString( ) );
						Sector.WriteAttributeString( "Longitude", MySector.ProhibitAreas [ CurrentProhibitArea ].Fixes [ CurrentFIX ].Longitude.ToString( ) );
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

		private void btnSaveAsClick ( object sender, RoutedEventArgs e )
		{
			Microsoft.Win32.SaveFileDialog SaveFileDialog = new Microsoft.Win32.SaveFileDialog( )
			{
				Filter = "ATCTS Sector files (*.sector)|*.sector"
			};
			SaveFileDialog.FileOk += SaveFileDialog_FileOk;
			SaveFileDialog.ShowDialog( );
		}

		private void btnOpenClick ( object sender, RoutedEventArgs e )
		{
			Microsoft.Win32.OpenFileDialog OpenFileDialog = new Microsoft.Win32.OpenFileDialog( )
			{
				Filter = "ATCTS Sector files (*.sector)|*.sector",
				Multiselect = false
			};
			OpenFileDialog.FileOk += OpenFileDialog_FileOk;
			OpenFileDialog.ShowDialog( );
		}

		private void OpenFileDialog_FileOk ( object sender, System.ComponentModel.CancelEventArgs e )
		{
			Microsoft.Win32.OpenFileDialog OpenFileDialog = ( Microsoft.Win32.OpenFileDialog ) sender;
			SectorFilePath = OpenFileDialog.FileName;

			//try
			//{
			using ( StreamReader sr = new StreamReader( SectorFilePath ) )
			{
				string RawSectorFileText = sr.ReadToEnd( );

				if ( Opening.OpenSectorFile( RawSectorFileText ) != null )
				{
					MySector = Opening.OpenSectorFile( RawSectorFileText );

					btnSave.IsEnabled = true;
					btnSaveAs.IsEnabled = true;
					btnExport.IsEnabled = true;

					FillWindow( );
				}
				else
				{
					MessageBox.Show( "Unable to open sector file" );
				}
			}
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show(ex.ToString());
			//}
		}

		private void dgvAirportsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 5:
					EditRunwaysWindow ChildWindow = new EditRunwaysWindow( MySector.Airports [ e.RowIndex ] );
					ChildWindow.ShowDialog( );
					if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
					{
						MySector.Airports [ e.RowIndex ] = ChildWindow.CurrentAirport;
						FillAirports( );
					}
					break;
				case 6:
					if ( MessageBox.Show( String.Format( "Do you really want to delete Airport {0}?", MySector.Airports [ e.RowIndex ].ICAO ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MySector.Airports.RemoveAt( e.RowIndex );
						dgvAirports.Rows.RemoveAt( e.RowIndex );
					}
					break;
			}
		}

		private void dgvAirportsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 0:
					MySector.Airports [ e.RowIndex ].Name = dgvAirports.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 1:
					MySector.Airports [ e.RowIndex ].ICAO = dgvAirports.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 2:
					MySector.Airports [ e.RowIndex ].TowerFrequency = dgvAirports.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 3:
					MySector.Airports [ e.RowIndex ].Latitude = Convert.ToInt16( dgvAirports.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
				case 4:
					MySector.Airports [ e.RowIndex ].Longitude = Convert.ToInt16( dgvAirports.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( ) );
					break;
			}
		}

		private void dgvVORsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 4:
					if ( MessageBox.Show( String.Format( "Do you really want to delete VOR {0}?", MySector.VORs [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MySector.VORs.RemoveAt( e.RowIndex );
						dgvVORs.Rows.RemoveAt( e.RowIndex );
					}
					break;
			}
		}

		private void dgvVORsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 0:
					MySector.VORs [ e.RowIndex ].Name = dgvVORs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 1:
					MySector.VORs [ e.RowIndex ].Frequency = dgvVORs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 2:
					MySector.VORs [ e.RowIndex ].Latitude = Convert.ToInt16( dgvVORs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( ) );
					break;
				case 3:
					MySector.VORs [ e.RowIndex ].Longitude = Convert.ToInt16( dgvVORs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( ) );
					break;
			}
		}

		private void dgvNDBsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 4:
					if ( MessageBox.Show( String.Format( "Do you really want to delete NDB {0}?", MySector.NDBs [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MySector.NDBs.RemoveAt( e.RowIndex );
						dgvNDBs.Rows.RemoveAt( e.RowIndex );
					}
					break;
			}
		}

		private void dgvNDBsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 0:
					MySector.NDBs [ e.RowIndex ].Name = dgvNDBs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 1:
					MySector.NDBs [ e.RowIndex ].Frequency = dgvNDBs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 2:
					MySector.NDBs [ e.RowIndex ].Latitude = Convert.ToInt16( dgvNDBs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( ) );
					break;
				case 3:
					MySector.NDBs [ e.RowIndex ].Longitude = Convert.ToInt16( dgvNDBs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( ) );
					break;
			}
		}

		private void dgvFixesCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 3:
					if ( MessageBox.Show( String.Format( "Do you really want to delete FIX {0}?", MySector.Fixes [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MySector.Fixes.RemoveAt( e.RowIndex );
						dgvFixes.Rows.RemoveAt( e.RowIndex );
					}
					break;
			}
		}

		private void dgvFixesCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 0:
					MySector.Fixes [ e.RowIndex ].Name = dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
					break;
				case 1:
					MySector.Fixes [ e.RowIndex ].Latitude = Convert.ToInt16( dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( ) );
					break;
				case 2:
					MySector.Fixes [ e.RowIndex ].Longitude = Convert.ToInt16( dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( ) );
					break;
			}
		}

		private void dgvHighAirwaysCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 1:
					EditFixesWindow FixesWindow = new EditFixesWindow( MySector.HighAirways [ e.RowIndex ].Fixes, MySector.HighAirways [ e.RowIndex ].Name );
					FixesWindow.ShowDialog( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						MySector.HighAirways [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillHighAirways( );
					}
					break;
				case 2:
					if ( MessageBox.Show( String.Format( "Do you really want to delete HighAirway {0}?", MySector.HighAirways [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.HighAirways.Add( MySector.HighAirways [ e.RowIndex ] );
						MySector.HighAirways.RemoveAt( e.RowIndex );
						FillHighAirways( );
					}
					break;
			}
		}

		private void dgvHighAirwaysCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			MySector.HighAirways [ e.RowIndex ].Name = dgvHighAirways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
		}

		private void dgvLowAirwaysCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 1:
					EditFixesWindow FixesWindow = new EditFixesWindow( MySector.LowAirways [ e.RowIndex ].Fixes, MySector.LowAirways [ e.RowIndex ].Name );
					FixesWindow.ShowDialog( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						MySector.LowAirways [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillLowAirways( );
					}
					break;
				case 2:
					if ( MessageBox.Show( String.Format( "Do you really want to delete LowAirway {0}?", MySector.LowAirways [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.LowAirways.Add( MySector.LowAirways [ e.RowIndex ] );
						MySector.LowAirways.RemoveAt( e.RowIndex );
						FillLowAirways( );
					}
					break;
			}
		}

		private void dgvLowAirwaysCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			MySector.LowAirways [ e.RowIndex ].Name = dgvLowAirways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
		}

		private void dgvARTCCsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 1:
					EditFixesWindow FixesWindow = new EditFixesWindow( MySector.ARTCCs [ e.RowIndex ].Fixes, MySector.ARTCCs [ e.RowIndex ].Name );
					FixesWindow.ShowDialog( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						MySector.ARTCCs [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillARTCCs( );
					}
					break;
				case 2:
					if ( MessageBox.Show( String.Format( "Do you really want to delete ARTCC {0}?", MySector.ARTCCs [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.ARTCCs.Add( MySector.ARTCCs [ e.RowIndex ] );
						MySector.ARTCCs.RemoveAt( e.RowIndex );
						FillARTCCs( );
					}
					break;
			}
		}

		private void dgvARTCCsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			MySector.ARTCCs [ e.RowIndex ].Name = dgvARTCCs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
		}

		private void dgvHighARTCCsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 1:
					EditFixesWindow FixesWindow = new EditFixesWindow( MySector.HighARTCCs [ e.RowIndex ].Fixes, MySector.HighARTCCs [ e.RowIndex ].Name );
					FixesWindow.ShowDialog( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						MySector.HighARTCCs [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillHighARTCCs( );
					}
					break;
				case 2:
					if ( MessageBox.Show( String.Format( "Do you really want to delete HighARTCC {0}?", MySector.HighARTCCs [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.HighARTCCs.Add( MySector.HighARTCCs [ e.RowIndex ] );
						MySector.HighARTCCs.RemoveAt( e.RowIndex );
						FillHighARTCCs( );
					}
					break;
			}
		}

		private void dgvHighARTCCsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			MySector.HighARTCCs [ e.RowIndex ].Name = dgvHighARTCCs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
		}

		private void dgvLowARTCCsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 1:
					EditFixesWindow FixesWindow = new EditFixesWindow( MySector.LowARTCCs [ e.RowIndex ].Fixes, MySector.LowARTCCs [ e.RowIndex ].Name );
					FixesWindow.ShowDialog( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						MySector.LowARTCCs [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillLowARTCCs( );
					}
					break;
				case 2:
					if ( MessageBox.Show( String.Format( "Do you really want to delete LowARTCC {0}?", MySector.LowARTCCs [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.LowARTCCs.Add( MySector.LowARTCCs [ e.RowIndex ] );
						MySector.LowARTCCs.RemoveAt( e.RowIndex );
						FillLowARTCCs( );
					}
					break;
			}
		}

		private void dgvLowARTCCsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			MySector.LowARTCCs [ e.RowIndex ].Name = dgvLowARTCCs.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
		}

		private void dgvProhibitAreasCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 1:
					EditFixesWindow FixesWindow = new EditFixesWindow( MySector.ProhibitAreas [ e.RowIndex ].Fixes, MySector.ProhibitAreas [ e.RowIndex ].Name );
					FixesWindow.ShowDialog( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						MySector.ProhibitAreas [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillProhibitAreas( );
					}
					break;
				case 2:
					if ( MessageBox.Show( String.Format( "Do you really want to delete ProhibitArea {0}?", MySector.ProhibitAreas [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.ProhibitAreas.Add( MySector.ProhibitAreas [ e.RowIndex ] );
						MySector.ProhibitAreas.RemoveAt( e.RowIndex );
						FillProhibitAreas( );
					}
					break;
			}
		}

		private void dgvProhibitAreasCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			MySector.ProhibitAreas [ e.RowIndex ].Name = dgvProhibitAreas.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString( );
		}

		private void btnExportClick ( object sender, RoutedEventArgs e )
		{
			if ( SectorFilePath == null )
			{
				Microsoft.Win32.SaveFileDialog ExportFileDialog = new Microsoft.Win32.SaveFileDialog( )
				{
					Filter = "ATCTS Sector files (*.sector)|*.sector"
				};
				ExportFileDialog.FileOk += ExportFileDialog_FileOk;
				ExportFileDialog.ShowDialog( );
			}
			else
			{
				ExportSectorFile( );
			}
		}

		private void ExportFileDialog_FileOk ( object sender, System.ComponentModel.CancelEventArgs e )
		{
			Microsoft.Win32.SaveFileDialog ExportFileDialog = ( Microsoft.Win32.SaveFileDialog ) sender;
			SectorFilePath = ExportFileDialog.FileName;
			ExportSectorFile( );
		}

		private void btnAddAirportClick ( object sender, RoutedEventArgs e )
		{
			MySector.Airports.Add( new Airport( ) );
			FillAirports( );
		}

		private void btnAddVORClick ( object sender, RoutedEventArgs e )
		{
			MySector.VORs.Add( new VOR( ) );
			FillVORs( );
		}

		private void btnAddNDBClick ( object sender, RoutedEventArgs e )
		{
			MySector.NDBs.Add( new NDB( ) );
			FillNDBs( );
		}

		private void btnAddFixClick ( object sender, RoutedEventArgs e )
		{
			MySector.Fixes.Add( new FIX( ) );
			FillFixes( );
		}

		private void btnAddHighAirwayClick ( object sender, RoutedEventArgs e )
		{
			MySector.HighAirways.Add( new HighAirway( ) );
			FillHighAirways( );
		}

		private void btnAddLowAirwayClick ( object sender, RoutedEventArgs e )
		{
			MySector.LowAirways.Add( new LowAirway( ) );
			FillLowAirways( );
		}

		private void btnAddARTCCClick ( object sender, RoutedEventArgs e )
		{
			MySector.ARTCCs.Add( new ARTCC( ) );
			FillARTCCs( );
		}

		private void btnAddHighARTCCClick ( object sender, RoutedEventArgs e )
		{
			MySector.HighARTCCs.Add( new HighARTCC( ) );
			FillHighARTCCs( );
		}

		private void btnAddLowARTCCClick ( object sender, RoutedEventArgs e )
		{
			MySector.LowARTCCs.Add( new LowARTCC( ) );
			FillLowARTCCs( );
		}

		private void btnAddProhibitAreaClick ( object sender, RoutedEventArgs e )
		{
			MySector.ProhibitAreas.Add( new ProhibitArea( ) );
			FillProhibitAreas( );
		}

		private void txtNameTextChanged ( object sender, TextChangedEventArgs e )
		{
			MySector.Name = txtName.Text;
		}

		private void txtMainAirportTextChanged ( object sender, TextChangedEventArgs e )
		{
			MySector.MainAirport = txtMainAirport.Text;
		}

		private void txtCenterLatitudeTextChanged ( object sender, TextChangedEventArgs e )
		{
			MySector.CenterLatitude = Convert.ToInt16( txtCenterLatitude.Text );
		}

		private void txtCenterLongitudeTextChanged ( object sender, TextChangedEventArgs e )
		{
			MySector.CenterLongitude = Convert.ToInt16( txtCenterLongitude.Text );
		}

		private void txtMagneticVariationTextChanged ( object sender, TextChangedEventArgs e )
		{
			MySector.MagneticVariation = double.Parse( txtMagneticVariation.Text );
		}

		private void txtDefaultScaleTextChanged ( object sender, TextChangedEventArgs e )
		{
			MySector.DefaultScale = double.Parse( txtDefaultScale.Text );
		}

		private void txtVersion_TextChanged ( object sender, TextChangedEventArgs e )
		{
			MySector.Version = double.Parse( txtVersion.Text );
		}
	}
}
