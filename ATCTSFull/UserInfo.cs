using System.Collections.Generic;

namespace ATCTSFull
{
	public static class UserInfo
	{
		public static int Id = -1;
		public static string Email;
		public static string FirstName;
		public static string LastName;
		public static ConnectionTypes ConnectionType;
		public enum ConnectionTypes { Online, Offline };
		public static List<SectorInfo> Sectors = new List<SectorInfo>( );

		public static void GetUserInfo ( )
		{
			try
			{
				ATCTSDBDataSetTableAdapters.GetUserInfoTableAdapter QTA = new ATCTSDBDataSetTableAdapters.GetUserInfoTableAdapter( );
				ATCTSDBDataSet.GetUserInfoDataTable QDT = QTA.GetData( UserInfo.Id );
				Email = QDT [ 0 ] [ "Email" ].ToString( );
				FirstName = QDT [ 0 ] [ "FirstName" ].ToString( );
				LastName = QDT [ 0 ] [ "LastName" ].ToString( );

				ATCTSDBDataSetTableAdapters.GetSectorsTableAdapter QTA2 = new ATCTSDBDataSetTableAdapters.GetSectorsTableAdapter( );
				ATCTSDBDataSet.GetSectorsDataTable QDT2 = QTA2.GetData( Id );

				for ( int CurrentRow = 0; CurrentRow < QDT2.Rows.Count; CurrentRow++ )
				{
					Sectors.Add( new SectorInfo( QDT2 [ CurrentRow ] [ "ICAO" ].ToString( ), QDT2 ) );
				}

				string LocalSectors = null;
				foreach ( SectorInfo CurrentSector in Sectors )
				{
					LocalSectors += CurrentSector.ICAO + ";";
				}
				AuthWindow.ProgramKey.SetValue( Crypto.GetMD5( "Sectors" ), Crypto.EncryptStringAES( LocalSectors, "Bdp4XDP3AN" ) );
			}
			catch { }
		}

		public static SectorInfo GetSectorInfo ( string ICAO )
		{
			foreach ( SectorInfo CurrentSector in Sectors )
			{
				if ( CurrentSector.ICAO == ICAO )
				{
					return CurrentSector;
				}
			}

			return null;
		}
	}
}
