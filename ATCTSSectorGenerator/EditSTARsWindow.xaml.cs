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
	/// Логика взаимодействия для EditSTARsWindow.xaml
	/// </summary>
	public partial class EditSTARsWindow : Window
	{
		public Runway CurrentRunway;

		public EditSTARsWindow ( Runway ParamRunway )
		{
			InitializeComponent ( );
			CurrentRunway = ParamRunway;
			Title = String.Format ( "{0} / {1}", ParamRunway.Number, ParamRunway.ReciprocalNumber );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvSTARs.Rows.Clear ( );

			foreach ( STAR CurrentSTAR in CurrentRunway.STARs )
			{
				dgvSTARs.Rows.Add ( CurrentSTAR.Name, CurrentSTAR.ReciprocalRunway, String.Format ( "Add/Edit [{0}]", CurrentSTAR.Fixes.Count ), "Delete" );
			}
		}

		private void btnImportSTARClick ( object sender, RoutedEventArgs e )
		{
			ImportSTARWindow ChildWindow = new ImportSTARWindow ( );
			ChildWindow.ShowDialog ( );
			if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
			{
				CurrentRunway.STARs.Add ( ChildWindow.GetSelectedSTAR ( ) );
				MainWindow.MySector.STARs.Remove ( ChildWindow.GetSelectedSTAR ( ) );
				FillDataGridView ( );
			}
		}

		private void btnAddSTARClick ( object sender, RoutedEventArgs e )
		{
			CurrentRunway.STARs.Add ( new STAR ( ) );
			FillDataGridView ( );
		}

		private void dgvSTARsCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 2:
					EditFixesWindow FixesWindow = new EditFixesWindow ( CurrentRunway.STARs [ e.RowIndex ].Fixes, CurrentRunway.STARs [ e.RowIndex ].Name );
					FixesWindow.ShowDialog ( );
					if ( FixesWindow.DialogResult.HasValue && FixesWindow.DialogResult.Value )
					{
						CurrentRunway.STARs [ e.RowIndex ].Fixes = FixesWindow.FixesList;
						FillDataGridView ( );
					}
					break;
				case 3:
					if ( MessageBox.Show ( String.Format ( "Do you really want to delete STAR {0}?", CurrentRunway.STARs [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						MainWindow.MySector.STARs.Add ( CurrentRunway.STARs [ e.RowIndex ] );
						CurrentRunway.STARs.RemoveAt ( e.RowIndex );
						FillDataGridView ( );
					}
					break;
			}
		}

		private void dgvSTARsCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
            switch (e.ColumnIndex)
            {
                case 0:
                    CurrentRunway.STARs[e.RowIndex].Name = dgvSTARs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    break;
                case 1:
                    CurrentRunway.STARs[e.RowIndex].ReciprocalRunway = Convert.ToBoolean(dgvSTARs.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
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
