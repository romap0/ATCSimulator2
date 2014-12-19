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
using ATCTSPortableClassLibrary;

namespace ATCTrainingSimulatorSectorGenerator
{
	/// <summary>
	/// Логика взаимодействия для ImportRunwayWindow.xaml
	/// </summary>
	public partial class ImportRunwayWindow : Window
	{
		int SelectedDataRow = 0;

		public ImportRunwayWindow ( )
		{
			InitializeComponent ( );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvRunways.Rows.Clear ( );
			foreach ( Runway CurrentRunway in MainWindow.MySector.Runways )
			{
				dgvRunways.Rows.Add ( CurrentRunway.Number, CurrentRunway.ReciprocalNumber, CurrentRunway.Heading, CurrentRunway.ReciprocalHeading, CurrentRunway.StartLatitude, CurrentRunway.StartLongitude, CurrentRunway.EndLatitude, CurrentRunway.EndLongitude );
			}
		}

		public Runway GetSelectedRunway ( )
		{
			
			if ( SelectedDataRow != -1 )
			{
				return MainWindow.MySector.Runways [ SelectedDataRow ];
			}
			else
			{
				return null;
			}
		}

		private void dgvRunwaysSelectionChanged ( object sender, EventArgs e )
		{
			SelectedDataRow = dgvRunways.SelectedCells [ 0 ].RowIndex;
		}

		private void btnCancel_Click ( object sender, RoutedEventArgs e )
		{
			DialogResult = false;
			this.Close ( );
		}

		private void btnImportClick ( object sender, RoutedEventArgs e )
		{
			DialogResult = true;
			this.Close ( );
		}
	}
}
