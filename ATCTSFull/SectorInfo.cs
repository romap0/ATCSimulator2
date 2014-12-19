using System;
using System.IO;

namespace ATCTSFull
{
	public class SectorInfo
	{
		public string Name;
		public string ICAO;
		public bool isLocal;
		public int LocalVersion;
		public int ServerVersion;

		public SectorInfo ( string ICAO, ATCTSDBDataSet.GetSectorsDataTable QDT )
		{
			for ( int CurrentRow = 0; CurrentRow < QDT.Rows.Count; CurrentRow++ )
			{
				if ( QDT [ CurrentRow ] [ "ICAO" ].ToString( ) == ICAO )
				{
					Name = QDT [ CurrentRow ] [ "Name" ].ToString( );
					this.ICAO = ICAO;
					ServerVersion = Convert.ToInt16( QDT [ CurrentRow ] [ "Version" ] );

					FileInfo SectorFile = new FileInfo( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors\\" + ICAO + ".sector" );

					isLocal = SectorFile.Exists;
				}
			}
		}

		public SectorInfo ( string ICAO )
		{
			this.ICAO = ICAO;

			FileInfo SectorFile = new FileInfo( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + "\\NWD-Group\\ATC Training Simulator Full\\Sectors\\" + ICAO + ".sector" );

			if ( SectorFile.Exists )
			{
				isLocal = true;
			}
			else
			{
				isLocal = false;
			}
		}
	}
}