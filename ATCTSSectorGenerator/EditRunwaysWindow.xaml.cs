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
	/// Логика взаимодействия для EditRunwaysWindow.xaml
	/// </summary>
	public partial class EditRunwaysWindow : Window
	{
		public Airport CurrentAirport;

		public EditRunwaysWindow ( Airport ParamAirport )
		{
			InitializeComponent ( );
			CurrentAirport = ParamAirport;
			Title = String.Format ( "{0} ({1})", ParamAirport.Name, ParamAirport.ICAO );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvRunways.Rows.Clear ( );

			foreach ( Runway CurrentRunway in CurrentAirport.Runways )
			{
				dgvRunways.Rows.Add ( CurrentRunway.Number, CurrentRunway.ReciprocalNumber, CurrentRunway.Heading, CurrentRunway.ReciprocalHeading, CurrentRunway.StartLatitude, CurrentRunway.StartLongitude, CurrentRunway.EndLatitude, CurrentRunway.EndLongitude, String.Format ( "Add/Edit [{0}]", CurrentRunway.SIDs.Count ), String.Format ( "Add/Edit [{0}]", CurrentRunway.STARs.Count ), "Delete" );
			}
		}

		private void dgvRunwaysCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 8:
					EditSIDsWindow SIDsWindow = new EditSIDsWindow ( CurrentAirport.Runways [ e.RowIndex ] );
					SIDsWindow.ShowDialog ( );
					if ( SIDsWindow.DialogResult.HasValue && SIDsWindow.DialogResult.Value )
					{
						CurrentAirport.Runways [ e.RowIndex ] = SIDsWindow.CurrentRunway;
						FillDataGridView ( );
					}
					break;
				case 9:
					EditSTARsWindow STARsWindow = new EditSTARsWindow ( CurrentAirport.Runways [ e.RowIndex ] );
					STARsWindow.ShowDialog ( );
					if ( STARsWindow.DialogResult.HasValue && STARsWindow.DialogResult.Value )
					{
						CurrentAirport.Runways [ e.RowIndex ] = STARsWindow.CurrentRunway;
						FillDataGridView ( );
					}
					break;
				case 10:
					if ( MessageBox.Show ( String.Format ( "Do you really want to delete Runway {0}/{1}?", CurrentAirport.Runways [ e.RowIndex ].Number, CurrentAirport.Runways [ e.RowIndex ].ReciprocalNumber ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.Runways.Add ( CurrentAirport.Runways [ e.RowIndex ] );
						CurrentAirport.Runways.RemoveAt ( e.RowIndex );
						dgvRunways.Rows.RemoveAt ( e.RowIndex );
					}
					break;
			}
		}

		private void dgvRunwaysCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 0:
					CurrentAirport.Runways [ e.RowIndex ].Number = dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString ( );
					break;
				case 1:
					CurrentAirport.Runways [ e.RowIndex ].ReciprocalNumber = dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString ( );
					break;
				case 2:
					CurrentAirport.Runways [ e.RowIndex ].Heading = Convert.ToInt16 ( dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
				case 3:
					CurrentAirport.Runways [ e.RowIndex ].ReciprocalHeading = Convert.ToInt16 ( dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
				case 4:
					CurrentAirport.Runways [ e.RowIndex ].StartLatitude = Convert.ToInt16(dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value);
					break;
				case 5:
					CurrentAirport.Runways [ e.RowIndex ].StartLongitude = Convert.ToInt16( dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
				case 6:
					CurrentAirport.Runways [ e.RowIndex ].EndLatitude = Convert.ToInt16( dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
				case 7:
					CurrentAirport.Runways [ e.RowIndex ].EndLongitude = Convert.ToInt16( dgvRunways.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
			}
		}

		private void btnImportRunwayClick ( object sender, RoutedEventArgs e )
		{
			ImportRunwayWindow ChildWindow = new ImportRunwayWindow ( );
			ChildWindow.ShowDialog ( );
			if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
			{
				CurrentAirport.Runways.Add ( ChildWindow.GetSelectedRunway ( ) );
				MainWindow.MySector.Runways.Remove ( ChildWindow.GetSelectedRunway ( ) );
				FillDataGridView ( );
			}
		}

		private void btnSaveClick ( object sender, RoutedEventArgs e )
		{
			this.DialogResult = true;
			this.Close ( );
		}

		private void btnCancelClick ( object sender, RoutedEventArgs e )
		{
			this.DialogResult = false;
			this.Close ( );
		}

		private void btnAddRunwayClick ( object sender, RoutedEventArgs e )
		{
			CurrentAirport.Runways.Add ( new Runway ( ) );
			FillDataGridView ( );
		}
	}
}
