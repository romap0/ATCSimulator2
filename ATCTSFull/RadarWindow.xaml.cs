using ATCTSPortableClassLibrary;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;


namespace ATCTSFull
{
	/// <summary>
	/// Логика взаимодействия для RadarWindow.xaml
	/// </summary>
	public partial class RadarWindow : Elysium.Controls.Window
	{
		int MaximumTraffic = 0;
		bool isTrafficStable = false;
		Sector MySector;
		public static string SectorFilePath;
		static int CenterLongitude;
		static int CenterLatitude;
		static double Scale;
		static double LabelOffsetX = 5;
		static double LabelOffsetY = 5;
		BackgroundWorker MoveChartWorker = new BackgroundWorker( );

		public RadarWindow ( int MaximumTraffic )
		{
			if ( MaximumTraffic > 0 )
			{
				this.MaximumTraffic = MaximumTraffic;
				isTrafficStable = true;
			}

			try
			{
				using ( System.IO.StreamReader MyStreamReader = new System.IO.StreamReader( SectorFilePath ) )
				{
					string RawSectorFileText = MyStreamReader.ReadToEnd( );

					if ( Opening.OpenSectorFile( RawSectorFileText ) != null )
					{
						MySector = Opening.OpenSectorFile( RawSectorFileText );
						CenterLatitude = MySector.CenterLatitude;
						CenterLongitude = MySector.CenterLongitude;
						Scale = 1 / MySector.DefaultScale;
					}
					else
					{
						Elysium.Notifications.NotificationManager.Push( "Sector error", "Unable to open sector file" );
					}
				}
			}
			catch ( Exception e )
			{
				Elysium.Notifications.NotificationManager.Push( "Sector error", String.Format( "Sector file does not exist.\nError:\n{0}", e.ToString( ) ) );
			}

			InitializeComponent( );
		}

		private void Window_Closed ( object sender, EventArgs e )
		{
			Application.Current.Shutdown( );
		}

		private double GetScaledLatitude ( int Latitude )
		{
			double ScaledLatitude;
			ScaledLatitude = Latitude - CenterLatitude;
			ScaledLatitude *= Scale / 1000;

			return ScaledLatitude;
		}

		private double GetScaledLongitude ( int Longitude )
		{
			double ScaledLongitude;
			ScaledLongitude = Longitude - CenterLongitude;
			ScaledLongitude *= Scale / 1000;

			return ScaledLongitude;
		}

		private void Window_Loaded ( object sender, RoutedEventArgs e )
		{
			CenterX = cnvRadar.ActualWidth / 2;
			CenterY = cnvRadar.ActualHeight / 2;
			CenterLongitude -= Convert.ToInt32( ( MoveX - PrevMoveX ) / Scale * 1000 );
			CenterLatitude -= Convert.ToInt32( ( MoveY - PrevMoveY ) / Scale * 1000 );
			CenterLongitude -= Convert.ToInt32( CenterX * MySector.DefaultScale * 1000 );
			CenterLatitude -= Convert.ToInt32( CenterY * MySector.DefaultScale * 1000 );

			AddRunways( );	//Name_Type_Shape_Longitude_Latitude(_Longitude_Latitude)
			AddNDBs( );
			AddVORs( );
			AddFixes( );
			AddARTCCs( );

			framesTimer.Elapsed += framesTimer_Elapsed;
			framesTimer.Start( );
		}

		private void AddARTCCs ( )
		{
			foreach ( ARTCC CurrentARTCC in MySector.ARTCCs )
			{
				FIX PreviousFIX = null;

				foreach ( FIX CurrentFIX in CurrentARTCC.Fixes )
				{
					if ( PreviousFIX != null )
					{
						LineGeometry CurrentLine = new LineGeometry( );
						CurrentLine.StartPoint = new Point( GetScaledLongitude( PreviousFIX.Longitude ), GetScaledLatitude( PreviousFIX.Latitude ) );
						CurrentLine.EndPoint = new Point( GetScaledLongitude( CurrentFIX.Longitude ), GetScaledLatitude( CurrentFIX.Latitude ) );
						Path CurrentPath = new Path( );
						CurrentPath.Stroke = Brushes.Blue;
						CurrentPath.StrokeThickness = 0.1;
						CurrentPath.SnapsToDevicePixels = true;
						CurrentPath.Tag = String.Format( "{0};{1};{2};{3};{4};{5};{6}", CurrentFIX.Name, "ARTCC", "LineGeometry", PreviousFIX.Longitude, PreviousFIX.Latitude, CurrentFIX.Longitude, CurrentFIX.Latitude );
						CurrentLine.Freeze( );
						CurrentPath.Data = CurrentLine;
						cnvRadar.Children.Add( CurrentPath );
					}

					PreviousFIX = CurrentFIX;
				}
			}
		}

		private void AddRunways ( )
		{
			foreach ( Airport CurrentAirport in MySector.Airports )
			{
				foreach ( Runway CurrentRunway in CurrentAirport.Runways )
				{
					Line RunwayLine = new Line( ) { Tag = String.Format( "{0};{1};{2};{3};{4};{5};{6}", CurrentRunway.Number, "Runway", "Line", CurrentRunway.StartLongitude, CurrentRunway.StartLatitude, CurrentRunway.EndLongitude, CurrentRunway.EndLatitude ), X1 = GetScaledLongitude( CurrentRunway.StartLongitude ), Y1 = GetScaledLatitude( CurrentRunway.StartLatitude ), X2 = GetScaledLongitude( CurrentRunway.EndLongitude ), Y2 = GetScaledLatitude( CurrentRunway.EndLatitude ), StrokeThickness = 3, Stroke = Brushes.Red };
					cnvRadar.Children.Add( RunwayLine );
				}
			}
		}

		private void AddFixes ( )
		{
			foreach ( FIX CurrentFIX in MySector.Fixes )
			{
				Ellipse FIXEllipse = new Ellipse( ) { Tag = String.Format( "{0};{1};{2};{3};{4}", CurrentFIX.Name, "FIX", "Point", CurrentFIX.Longitude, CurrentFIX.Latitude ), Width = 3, Height = 3, Stroke = Brushes.Black };
				Label FIXLabel = new Label( ) { Tag = String.Format( "{0};{1};{2};{3};{4}", CurrentFIX.Name, "FIX", "Label", CurrentFIX.Longitude, CurrentFIX.Latitude ), Content = CurrentFIX.Name };
				cnvRadar.Children.Add( FIXLabel );
				cnvRadar.Children.Add( FIXEllipse );
				Canvas.SetLeft( FIXEllipse, GetScaledLongitude( CurrentFIX.Longitude ) );
				Canvas.SetTop( FIXEllipse, GetScaledLatitude( CurrentFIX.Latitude ) );
				Canvas.SetLeft( FIXLabel, GetScaledLongitude( CurrentFIX.Longitude ) + 5 );
				Canvas.SetTop( FIXLabel, GetScaledLatitude( CurrentFIX.Latitude ) + 5 );
			}
		}

		private void AddVORs ( )
		{
			foreach ( VOR CurrentVOR in MySector.VORs )
			{
				Ellipse VOREllipse = new Ellipse( ) { Tag = String.Format( "{0};{1};{2};{3};{4}", CurrentVOR.Name, "VOR", "Point", CurrentVOR.Longitude, CurrentVOR.Latitude ), Width = 3, Height = 3, Stroke = Brushes.Black };
				TextBlock VORLabel = new TextBlock( ) { Tag = String.Format( "{0};{1};{2};{3};{4}", CurrentVOR.Name, "VOR", "Label", CurrentVOR.Longitude, CurrentVOR.Latitude ), Text = CurrentVOR.Name };
				cnvRadar.Children.Add( VORLabel );
				cnvRadar.Children.Add( VOREllipse );
				Canvas.SetLeft( VOREllipse, GetScaledLongitude( CurrentVOR.Longitude ) );
				Canvas.SetTop( VOREllipse, GetScaledLatitude( CurrentVOR.Latitude ) );
				Canvas.SetLeft( VORLabel, GetScaledLongitude( CurrentVOR.Longitude ) + 5 );
				Canvas.SetTop( VORLabel, GetScaledLatitude( CurrentVOR.Latitude ) + 5 );
			}
		}

		private void AddNDBs ( )
		{
			foreach ( NDB CurrentNDB in MySector.NDBs )
			{
				Ellipse NDBEllipse = new Ellipse( ) { Tag = String.Format( "{0};{1};{2};{3};{4}", CurrentNDB.Name, "NDB", "Point", CurrentNDB.Longitude, CurrentNDB.Latitude ), Width = 3, Height = 3, Stroke = Brushes.Black };
				TextBlock NDBLabel = new TextBlock( ) { Tag = String.Format( "{0};{1};{2};{3};{4}", CurrentNDB.Name, "NDB", "Label", CurrentNDB.Longitude, CurrentNDB.Latitude ), Text = CurrentNDB.Name };
				cnvRadar.Children.Add( NDBLabel );
				cnvRadar.Children.Add( NDBEllipse );
				Canvas.SetLeft( NDBEllipse, GetScaledLongitude( CurrentNDB.Longitude ) );
				Canvas.SetTop( NDBEllipse, GetScaledLatitude( CurrentNDB.Latitude ) );
				Canvas.SetLeft( NDBLabel, GetScaledLongitude( CurrentNDB.Longitude ) + 5 );
				Canvas.SetTop( NDBLabel, GetScaledLatitude( CurrentNDB.Latitude ) + 5 );
			}
		}

		void framesTimer_Elapsed ( object sender, System.Timers.ElapsedEventArgs e )
		{
			this.Dispatcher.Invoke( System.Windows.Threading.DispatcherPriority.Normal, ( Action ) ( ( ) =>
				{
					lblFps.Content = "FPS: " + frames * 2;
					frames = 0;
				} ) );
		}

		double StartX = 0;
		double StartY = 0;
		double CurrentX = 0;
		double CurrentY = 0;
		double MoveX = 0;
		double MoveY = 0;
		double CenterX = 0;
		double CenterY = 0;
		double PrevMoveX = 0;
		double PrevMoveY = 0;
		bool MousePressed = false;

		int frames = 0;
		System.Timers.Timer framesTimer = new System.Timers.Timer( 500 );

		private void RenderSector ( )
		{
			RenderElements( );
			frames++;
		}

		private void RenderElements ( )
		{
			//Name_Type_Shape_Longitude_Latitude(_Longitude_Latitude)
			List<UIElement> Elements = ( from UIElement child in cnvRadar.Children
										 select child ).ToList( );

			foreach ( UIElement CurrentElement in Elements )
			{
				List<string> Params = CurrentElement.GetValue( TagProperty ).ToString( ).Split( ';' ).ToList( );

				if ( Params [ 2 ] == "Line" )
				{
					CurrentElement.SetValue( Line.X1Property, GetScaledLongitude( int.Parse( Params [ 3 ] ) ) );
					CurrentElement.SetValue( Line.Y1Property, GetScaledLatitude( int.Parse( Params [ 4 ] ) ) );
					CurrentElement.SetValue( Line.X2Property, GetScaledLongitude( int.Parse( Params [ 5 ] ) ) );
					CurrentElement.SetValue( Line.Y2Property, GetScaledLatitude( int.Parse( Params [ 6 ] ) ) );
				}
				else if ( Params [ 2 ] == "LineGeometry" )
				{
					LineGeometry CurrentLine = new LineGeometry( new Point( GetScaledLongitude( int.Parse( Params [ 3 ] ) ), GetScaledLatitude( int.Parse( Params [ 4 ] ) ) ), new Point( GetScaledLongitude( int.Parse( Params [ 5 ] ) ), GetScaledLatitude( int.Parse( Params [ 6 ] ) ) ) );
					CurrentElement.SetValue( Path.DataProperty, CurrentLine );
				}
				else
				{
					double OffsetX = 0, OffsetY = 0;
					if ( Params [ 2 ] == "Label" )
					{
						OffsetX = LabelOffsetX;
						OffsetY = LabelOffsetY;
					}

					CurrentElement.SetValue( Canvas.LeftProperty, GetScaledLongitude( int.Parse( Params [ 3 ] ) ) + OffsetX );
					CurrentElement.SetValue( Canvas.TopProperty, GetScaledLatitude( int.Parse( Params [ 4 ] ) ) + OffsetY );
				}
			}
		}

		private void Window_MouseWheel ( object sender, MouseWheelEventArgs e )
		{
			if ( Scale + e.Delta * Scale / 10000d > 0 )
			{
				Scale += e.Delta * Scale / 10000d;
			}
			if ( e.Delta > 0 )
			{
				CenterLongitude += Convert.ToInt32( Mouse.GetPosition( cnvRadar ).X / Scale * 15 );
				CenterLatitude += Convert.ToInt32( Mouse.GetPosition( cnvRadar ).Y / Scale * 15 );
			}
			else
			{
				CenterLongitude -= Convert.ToInt32( CenterX / Scale * 10 );
				CenterLatitude -= Convert.ToInt32( CenterY / Scale * 10 );

			}

			//CenterLongitude = GetLongitude( Mouse.GetPosition( cnvRadar ).X );
			//CenterLatitude = GetLatitude( Mouse.GetPosition( cnvRadar ).Y );

			RenderSector( );
		}

		private void Window_Move ( object sender, MouseEventArgs e )
		{
			if ( this.MousePressed )
			{
				this.CurrentX = e.GetPosition( this.Window ).X;
				this.CurrentY = e.GetPosition( this.Window ).Y;
				Task.Factory.StartNew( new Action( this.MoveChart ) );
			}
		}

		private void SetCenter ( )
		{
			CenterLongitude -= Convert.ToInt32( ( MoveX - PrevMoveX ) / Scale * 1000 );
			CenterLatitude -= Convert.ToInt32( ( MoveY - PrevMoveY ) / Scale * 1000 );
		}

		private void MoveChart ( )
		{
			this.MoveX = this.CurrentX - this.StartX;
			this.MoveY = this.CurrentY - this.StartY;

			SetCenter( );

			base.Dispatcher.Invoke( delegate
			{
				RenderSector( );
			} );

			PrevMoveX = MoveX;
			PrevMoveY = MoveY;
		}

		private void Window_MouseDown ( object sender, MouseButtonEventArgs e )
		{
			this.MousePressed = true;
			this.StartX = Mouse.GetPosition( this.Window ).X;
			this.StartY = Mouse.GetPosition( this.Window ).Y;
			base.Cursor = Cursors.ScrollAll;
		}

		private void Window_MouseUp ( object sender, MouseButtonEventArgs e )
		{
			this.MousePressed = false;
			this.CenterX += this.MoveX;
			this.CenterY += this.MoveY;
			this.MoveX = 0.0;
			this.MoveY = 0.0;
			this.PrevMoveX = 0.0;
			this.PrevMoveY = 0.0;
			base.Cursor = Cursors.Arrow;
		}
	}
}