using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Drawing;
using System.Windows.Media;

namespace ATCTSFull
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void StartupHandler(object sender, System.Windows.StartupEventArgs e)
		{
			Elysium.Manager.Apply(this, Elysium.Theme.Dark, Elysium.AccentBrushes.Blue, Brushes.White);
		}
	}
}
