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
	/// Логика взаимодействия для ImportSTARWindow.xaml
	/// </summary>
	public partial class ImportSTARWindow : Window
	{
		int SelectedDataRow = 0;

		public ImportSTARWindow ( )
		{
			InitializeComponent ( );

			FillDataGridView ( );
		}

		private void FillDataGridView ( )
		{
			dgvSTARs.Rows.Clear ( );
			foreach ( STAR CurrentSTAR in MainWindow.MySector.STARs )
			{
				dgvSTARs.Rows.Add ( CurrentSTAR.Name );
			}
		}

		public STAR GetSelectedSTAR ( )
		{

			if ( SelectedDataRow != -1 )
			{
				return MainWindow.MySector.STARs [ SelectedDataRow ];
			}
			else
			{
				return null;
			}
		}


		private void dgvSTARsSelectionChanged ( object sender, EventArgs e )
		{
			SelectedDataRow = dgvSTARs.SelectedCells [ 0 ].RowIndex;
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
