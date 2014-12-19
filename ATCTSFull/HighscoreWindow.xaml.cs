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
using System.Windows.Shapes;
using System.IO;

namespace ATCTSFull
{
	/// <summary>
	/// Логика взаимодействия для HighscoreWindow.xaml
	/// </summary>
	public partial class HighscoreWindow : Elysium.Controls.Window
	{
		public HighscoreWindow ( )
		{
			InitializeComponent ( );
		}

		private void btnbtnBackWindowClick ( object sender, RoutedEventArgs e )
		{
			this.Close ( );
		}

		private void WindowLoad ( object sender, RoutedEventArgs e )
		{
			int Highscore = 0;

			string RecordFilePath = Environment.SpecialFolder.ApplicationData + "\\last.stat";

			if ( File.Exists ( RecordFilePath ) )
			{
				Highscore = Convert.ToInt32 ( File.ReadAllText ( RecordFilePath ) );
			}

			lblHighscoreValue.Content = Highscore.ToString ( ) + "$";
		}
	}
}
