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
	/// Логика взаимодействия для ImportSIDWindow.xaml
	/// </summary>
	public partial class ImportSIDWindow : Window
	{
		int SelectedDataRow = 0;

		public ImportSIDWindow ( )
		{
			InitializeComponent ( );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvSIDs.Rows.Clear ( );
			foreach ( SID CurrentSID in MainWindow.MySector.SIDs )
			{
				dgvSIDs.Rows.Add ( CurrentSID.Name );
			}
		}

		public SID GetSelectedSID ( )
		{

			if ( SelectedDataRow != -1 )
			{
				return MainWindow.MySector.SIDs [ SelectedDataRow ];
			}
			else
			{
				return null;
			}
		}


		private void dgvSIDsSelectionChanged ( object sender, EventArgs e )
		{
			SelectedDataRow = dgvSIDs.SelectedCells [ 0 ].RowIndex;
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
