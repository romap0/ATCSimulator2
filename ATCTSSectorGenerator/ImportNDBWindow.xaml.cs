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
	/// Логика взаимодействия для ImportNDBWindow.xaml
	/// </summary>
	public partial class ImportNDBWindow : Window
	{
		int SelectedDataRow = 0;

		public ImportNDBWindow ( )
		{
			InitializeComponent ( );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvNDBs.Rows.Clear ( );
			foreach ( NDB CurrentNDB in MainWindow.MySector.NDBs )
			{
				dgvNDBs.Rows.Add ( CurrentNDB.Name );
			}
		}

		public NDB GetSelectedNDB ( )
		{

			if ( SelectedDataRow != -1 )
			{
				return MainWindow.MySector.NDBs [ SelectedDataRow ];
			}
			else
			{
				return null;
			}
		}


		private void dgvNDBsSelectionChanged ( object sender, EventArgs e )
		{
			SelectedDataRow = dgvNDBs.SelectedCells [ 0 ].RowIndex;
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
