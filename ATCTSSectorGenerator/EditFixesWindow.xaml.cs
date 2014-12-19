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
	/// Логика взаимодействия для EditFixesWindow.xaml
	/// </summary>
	public partial class EditFixesWindow : Window
	{
		public List<FIX> FixesList;

		public EditFixesWindow ( List<FIX> ParamList, string ParamTitle )
		{
			InitializeComponent ( );
			FixesList = ParamList;
			Title = ParamTitle;

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvFixes.Rows.Clear ( );

			foreach ( FIX CurrentFix in FixesList )
			{
				dgvFixes.Rows.Add ( CurrentFix.Name, CurrentFix.Latitude, CurrentFix.Longitude, CurrentFix.Altitude, CurrentFix.Speed, CurrentFix.FlyOver, "Delete" );
			}
		}

		private void btnImportFixClick ( object sender, RoutedEventArgs e )
		{
			ImportFixWindow ChildWindow = new ImportFixWindow ( );
			ChildWindow.ShowDialog ( );
			if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
			{
				FixesList.Add ( ChildWindow.GetSelectedFix ( ) );
				FillDataGridView ( );
			}
		}

		private void btnAddFixClick ( object sender, RoutedEventArgs e )
		{
			FixesList.Add ( new FIX ( ) );
			FillDataGridView ( );
		}

		private void btnImportVORClick ( object sender, RoutedEventArgs e )
		{
			ImportVORWindow ChildWindow = new ImportVORWindow ( );
			ChildWindow.ShowDialog ( );
			if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
			{
				FixesList.Add ( ChildWindow.GetSelectedVOR ( ).ToFIX ( ) );
				FillDataGridView ( );
			}
		}

		private void btnImportNDBClick ( object sender, RoutedEventArgs e )
		{
			ImportNDBWindow ChildWindow = new ImportNDBWindow ( );
			ChildWindow.ShowDialog ( );
			if ( ChildWindow.DialogResult.HasValue && ChildWindow.DialogResult.Value )
			{
				FixesList.Add ( ChildWindow.GetSelectedNDB ( ).ToFIX ( ) );
				FillDataGridView ( );
			}
		}

		private void dgvFixesCellClick ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 6:
					if ( MessageBox.Show ( String.Format ( "Do you really want to delete Fix {0}?", FixesList [ e.RowIndex ].Name ), "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No ) == MessageBoxResult.Yes )
					{
						FixesList.RemoveAt ( e.RowIndex );
						FillDataGridView ( );
					}
					break;
			}
		}

		private void dgvFixesCellEndEdit ( object sender, System.Windows.Forms.DataGridViewCellEventArgs e )
		{
			switch ( e.ColumnIndex )
			{
				case 0:
					FixesList [ e.RowIndex ].Name = dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value.ToString ( );
					break;
				case 1:
					FixesList [ e.RowIndex ].Latitude = Convert.ToInt16(dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value);
					break;
				case 2:
					FixesList [ e.RowIndex ].Longitude = Convert.ToInt16( dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
				case 3:
					FixesList [ e.RowIndex ].Altitude = Convert.ToInt16 ( dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value );
					break;
				case 4:
					FixesList [ e.RowIndex ].Speed = Convert.ToInt16(dgvFixes.Rows [ e.RowIndex ].Cells [ e.ColumnIndex ].Value);
                    break;
                case 5:
                    FixesList[e.RowIndex].FlyOver = Convert.ToBoolean(dgvFixes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
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
