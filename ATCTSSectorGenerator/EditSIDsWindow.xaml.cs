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
	/// Логика взаимодействия для EditSIDsWindow.xaml
	/// </summary>
	public partial class EditSIDsWindow : Window
	{
		public Runway CurrentRunway;

		public EditSIDsWindow ( Runway ParamRunway )
		{
			InitializeComponent ( );
			CurrentRunway = ParamRunway;
			Title = String.Format ( "{0} / {1}", ParamRunway.Number, ParamRunway.ReciprocalNumber );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvSIDs.Rows.Clear ( );

			foreach ( SID CurrentSID in CurrentRunway.SIDs )
			{
				dgvSIDs.Rows.Add ( CurrentSID.Name, CurrentSID.ReciprocalRunway, String.Format ( "Add/Edit [{0}]", CurrentSID.Fixes.Count ), "Delete" );
			}
		}

		private void btnImportSIDClick ( object sender, RoutedEventArgs e )
		{
			ImportSIDWindow ChildWindow = new ImportSIDWindow ( );
			ChildWindow.ShowDialog ( );
			if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
			{
				CurrentRunway.SIDs.Add ( ChildWindow.GetSelectedSID ( ) );
				MainWindow.MySector.SIDs.Remove ( ChildWindow.GetSelectedSID ( ) );
				FillDataGridView ( );
			}
		}

		private void btnAddSIDClick ( object sender, RoutedEventArgs e )
		{
			CurrentRunway.SIDs.Add ( new SID ( ) );
			FillDataGridView ( );
		}

		private void dgvSIDsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 2:
					EditFixesWindow FixesWindow = new EditFixesWindow ( CurrentRunway.SIDs [ e.RowIndex ].Fixes, CurrentRunway.SIDs [ e.RowIndex ].Name );
					FixesWindow.ShowDialog ( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						CurrentRunway.SIDs [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillDataGridView ( );
					}
					break;
				case 3:
					if ( MessageBox.Show ( String.Format ( "Do you really want to delete SID {0}?", CurrentRunway.SIDs [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.SIDs.Add ( CurrentRunway.SIDs [ e.RowIndex ] );
						CurrentRunway.SIDs.RemoveAt ( e.RowIndex );
						FillDataGridView ( );
					}
					break;
			}
		}

		private void dgvSIDsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
            switch (e.ColumnIndex)
            {
                case 0:
                    CurrentRunway.SIDs[e.RowIndex].Name = dgvSIDs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
                case 1:
                    CurrentRunway.SIDs[e.RowIndex].ReciprocalRunway = Convert.ToBoolean(dgvSIDs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    break;
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
	}
}
