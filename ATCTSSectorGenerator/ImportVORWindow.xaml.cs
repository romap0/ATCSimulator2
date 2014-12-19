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
	/// Логика взаимодействия для ImportVORWindow.xaml
	/// </summary>
	public partial class ImportVORWindow : Window
	{
		int SelectedDataRow = 0;

		public ImportVORWindow ( )
		{
			InitializeComponent ( );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvVORs.Rows.Clear ( );
			foreach ( VOR CurrentVOR in MainWindow.MySector.VORs )
			{
				dgvVORs.Rows.Add ( CurrentVOR.Name );
			}
		}

		public VOR GetSelectedVOR ( )
		{

			if ( SelectedDataRow != -1 )
			{
				return MainWindow.MySector.VORs [ SelectedDataRow ];
			}
			else
			{
				return null;
			}
		}


		private void dgvVORsSelectionChanged ( object sender, EventArgs e )
		{
			SelectedDataRow = dgvVORs.SelectedCells [ 0 ].RowIndex;
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
