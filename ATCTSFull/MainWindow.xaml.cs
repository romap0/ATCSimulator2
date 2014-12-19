using System;
using System.Windows;

namespace ATCTSFull
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Elysium.Controls.Window
	{
		public MainWindow ( )
		{
			InitializeComponent( );
		}

		private void btnHighscoreWindowClick ( object sender, RoutedEventArgs e )
		{
			HighscoreWindow ChildWindow = new HighscoreWindow( );
			ChildWindow.ShowDialog( );
		}

		private void btnAboutWindowClick ( object sender, RoutedEventArgs e )
		{
			AboutWindow ChildWindow = new AboutWindow( );
			ChildWindow.ShowDialog( );
		}

		private void btnExitClick ( object sender, System.Windows.RoutedEventArgs e )
		{
			App.Current.Shutdown( );
		}

		private void btnStartWindowClick ( object sender, System.Windows.RoutedEventArgs e )
		{
			StartWindow ChildWindow = new StartWindow( );
			ChildWindow.ShowDialog( );
			if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
			{
				RadarWindow ParentWindow = new RadarWindow( ChildWindow.MaximumTraffic );
				ParentWindow.Show( );
				this.Hide( );
			}
		}

		private void btnShop_Click ( object sender, RoutedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://www.google.com" );
		}

		private void btnLogOut_Click ( object sender, RoutedEventArgs e )
		{
			try
			{
				ATCTSDBDataSetTableAdapters.QueriesTableAdapter QTA = new ATCTSDBDataSetTableAdapters.QueriesTableAdapter( );
				DateTime LoginDate = DateTime.Parse( Crypto.DecryptStringAES( AuthWindow.ProgramKey.GetValue( Crypto.GetMD5( "Date" ) ).ToString( ), "Bdp4XDP3AN" ) );
				QTA.Logout( UserInfo.Id, LoginDate.Date );
				AuthWindow.ProgramKey.SetValue( Crypto.GetMD5( "Logged" ), Crypto.GetMD5( "False" ) );
				App.Current.Shutdown( );
			}
			catch
			{
				Elysium.Notifications.NotificationManager.Push( "Can`t reach server", "Unable to reach server. Check your internet connection." );
			}
		}

		private void Window_Loaded ( object sender, RoutedEventArgs e )
		{

			if ( AuthWindow.ProgramKey.ValueCount == 4 )
			{
				bool Logged = AuthWindow.ProgramKey.GetValue( Crypto.GetMD5( "Logged" ) ).ToString( ) == Crypto.GetMD5( "true" );
				DateTime LoginDate = DateTime.Parse( Crypto.DecryptStringAES( AuthWindow.ProgramKey.GetValue( Crypto.GetMD5( "Date" ) ).ToString( ), "Bdp4XDP3AN" ) );
				double Elapsed = ( DateTime.UtcNow - LoginDate ).TotalSeconds;
				bool Expired = Elapsed > 2592000d;

				ATCTSDBDataSetTableAdapters.QueriesTableAdapter QTA = new ATCTSDBDataSetTableAdapters.QueriesTableAdapter( );

				UserInfo.Email = Crypto.DecryptStringAES( AuthWindow.ProgramKey.GetValue( Crypto.GetMD5( "Email" ) ).ToString( ), "Bdp4XDP3AN" );

				if ( Expired )
				{
					UserInfo.Id = ( int ) QTA.GetUserId( UserInfo.Email );
					QTA.Logout( UserInfo.Id, LoginDate.Date );
				}

				if ( !Logged || Expired )
				{
					AuthWindow ChildWindow = new AuthWindow( UserInfo.Email );
					ChildWindow.ShowDialog( );
				}
			}
			else
			{
				AuthWindow ChildWindow = new AuthWindow( );
				ChildWindow.ShowDialog( );
			}

			try
			{
				ATCTSDBDataSetTableAdapters.QueriesTableAdapter QTA = new ATCTSDBDataSetTableAdapters.QueriesTableAdapter( );
				UserInfo.Id = ( int ) QTA.GetUserId( Crypto.DecryptStringAES( AuthWindow.ProgramKey.GetValue( Crypto.GetMD5( "Email" ) ).ToString( ), "Bdp4XDP3AN" ) );
				UserInfo.ConnectionType = UserInfo.ConnectionTypes.Online;
				UserInfo.GetUserInfo( );

				lblEmail.Content = UserInfo.Email;
				lblName.Content = String.Format( "{0} {1}", UserInfo.FirstName, UserInfo.LastName );
			}
			catch
			{
				Elysium.Notifications.NotificationManager.Push( "Can`t reach server", "Unable to reach server. Check your internet connection." );
				UserInfo.ConnectionType = UserInfo.ConnectionTypes.Offline;
				grdUserInfo.Visibility = System.Windows.Visibility.Hidden;
				btnLogOut.Visibility = System.Windows.Visibility.Hidden;

				string [ ] LocalSectors = Crypto.DecryptStringAES( AuthWindow.ProgramKey.GetValue( Crypto.GetMD5( "Sectors" ) ).ToString( ), "Bdp4XDP3AN" ).Split( ';' );

				for ( int i = 0; i < LocalSectors.Length - 1; i++ )
				{
					UserInfo.Sectors.Add( new SectorInfo( LocalSectors [ i ] ) );
				}
			}
		}
	}
}
