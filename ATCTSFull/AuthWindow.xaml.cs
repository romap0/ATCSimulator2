using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Threading;
using System.IO;

namespace ATCTSFull
{
	/// <summary>
	/// Логика взаимодействия для AuthWindow.xaml
	/// </summary>
	public partial class AuthWindow : Elysium.Controls.Window
	{
		public static RegistryKey ProgramKey = Registry.CurrentUser.OpenSubKey( "Software", true ).CreateSubKey( "ATCTSFull" );
		static Thread ConnectionThread;
		System.Timers.Timer ConnectionTimer = new System.Timers.Timer( 1000 );

		static int ReturnCondition;
		public static string Email;
		static string PasswordHash;

		public AuthWindow ( )
		{
			InitializeComponent( );
		}

		public AuthWindow ( string Email )
		{
			InitializeComponent( );
			txtEmail.Text = Email;
			CheckLoginBoxes( );
			txtPassword.Focus( );
		}

		private void btnSignIn_Click ( object sender, RoutedEventArgs e )
		{
			btnSignIn.IsEnabled = false;
			txtEmail.IsEnabled = false;
			txtPassword.IsEnabled = false;
			lblReturnMessage.Visibility = System.Windows.Visibility.Hidden;
			prgAuth.Visibility = System.Windows.Visibility.Visible;
			Email = txtEmail.Text;
			PasswordHash = Crypto.GetMD5( txtPassword.Password );
			ConnectionThread = new Thread( new ThreadStart( Connect ) );
			ConnectionThread.Start( );
			ConnectionTimer.Elapsed += ConnectionTimer_Elapsed;
			ConnectionTimer.AutoReset = true;
			ConnectionTimer.Start( );
		}

		private void CheckReturnCondition ( )
		{
			switch ( ReturnCondition )
			{
				case 1:
					lblReturnMessage.Content = "Your license is invalid. Check your profile.";
					break;
				case 2:
					lblReturnMessage.Content = "Your account is in ban list. Contact administration for details.";
					break;
				case 4:
					lblReturnMessage.Content = "Email or password is incorrect. Try again.";
					break;
				case 3:
					ProgramKey.SetValue( Crypto.GetMD5( "Logged" ), Crypto.GetMD5( "true" ) );
					ProgramKey.SetValue( Crypto.GetMD5( "Date" ), Crypto.EncryptStringAES( DateTime.UtcNow.ToString( ), "Bdp4XDP3AN" ) );
					ProgramKey.SetValue( Crypto.GetMD5( "Email" ), Crypto.EncryptStringAES( txtEmail.Text, "Bdp4XDP3AN" ) );
					UserInfo.Email = Email;
					this.Close( );
					break;
				case 5:
					lblReturnMessage.Content = "Unable to reach server. Check your internet connection.";
					break;
				case 6:
					lblReturnMessage.Content = "Connection limit exceeded. Log out on unused devices.";
					break;
			}
		}

		private void btnSignUp_Click ( object sender, RoutedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://www.google.com" );
		}

		private void ConnectionTimer_Elapsed ( object sender, System.Timers.ElapsedEventArgs e )
		{
			if ( !ConnectionThread.IsAlive )
			{
				this.Dispatcher.Invoke( System.Windows.Threading.DispatcherPriority.Normal, ( Action ) ( ( ) =>
				{
					CheckReturnCondition( );
					prgAuth.Visibility = System.Windows.Visibility.Hidden;
					btnSignIn.IsEnabled = true;
					txtEmail.IsEnabled = true;
					txtPassword.IsEnabled = true;
					lblReturnMessage.Visibility = System.Windows.Visibility.Visible;
					txtPassword.Focus( );
				} ) );
				ConnectionTimer.Stop( );
			}
		}

		private static void Connect ( )
		{
			try
			{
				ATCTSDBDataSetTableAdapters.QueriesTableAdapter QTA = new ATCTSDBDataSetTableAdapters.QueriesTableAdapter( );
				ReturnCondition = ( int ) QTA.Logging( Email, PasswordHash );
			}
			catch
			{
				ReturnCondition = 5;
			}
			ConnectionThread.Abort( );
		}

		private void txtEmail_TextChanged ( object sender, TextChangedEventArgs e )
		{
			CheckLoginBoxes( );
		}

		private void txtPassword_PasswordChanged ( object sender, RoutedEventArgs e )
		{
			CheckLoginBoxes( );
		}

		private void CheckLoginBoxes ( )
		{
			if ( txtEmail != null && txtPassword != null )
			{
				if ( txtEmail.Text != "" && txtPassword.Password != "" )
				{
					btnSignIn.IsEnabled = true;
				}
				else
				{
					btnSignIn.IsEnabled = false;
				}
			}
		}

		private void Window_Closing ( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if ( ReturnCondition != 3 )
			{
				Environment.Exit(0);
			}
		}
	}
}
