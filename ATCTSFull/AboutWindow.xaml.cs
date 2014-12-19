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

namespace ATCTSFull
{
	/// <summary>
	/// Логика взаимодействия для StartGameWindow.xaml
	/// </summary>
	public partial class AboutWindow : Elysium.Controls.Window
	{
		public AboutWindow ( )
		{
			InitializeComponent( );
		}

		private void btnBackWindowClick ( object sender, RoutedEventArgs e )
		{
			base.Close( );
		}
	}
}