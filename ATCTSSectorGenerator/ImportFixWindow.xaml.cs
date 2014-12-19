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
	/// Логика взаимодействия для ImportFixWindow.xaml
	/// </summary>
	public partial class ImportFixWindow : Window
	{
		int SelectedDataRow = 0;

		public ImportFixWindow ( )
		{
			InitializeComponent ( );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvFixes.Rows.Clear ( );
			foreach ( FIX CurrentFix in MainWindow.MySector.Fixes )
			{
				dgvFixes.Rows.Add ( CurrentFix.Name );
			}
		}

		public FIX GetSelectedFix ( )
		{

			if ( SelectedDataRow != -1 )
			{
				return MainWindow.MySector.Fixes [ SelectedDataRow ];
			}
			else
			{
				return null;
			}
		}


		private void dgvFixesSelectionChanged ( object sender, EventArgs e )
		{
			SelectedDataRow = dgvFixes.SelectedCells [ 0 ].RowIndex;
		}

		private void btnCancelClick ( object sender, RoutedEventArgs e )
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
