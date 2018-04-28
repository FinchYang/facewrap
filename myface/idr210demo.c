#include “sdtapi.h”
	int main()
		{
			int ret;
	int iPort=1;

    	    ret=InitComm(iPort);
			if ( ret ){
				ret= Authenticate ();
				if (ret){
					char Msg[200];
				ret= ReadBaseMsg (Msg, 0 );
					if (ret > 0 ){
					//显示文字及图片信息
					}
					char Msg1[200];
					int num;
				ret= ReadNewAppMsg (Msg1, &num );
					if (ret > 0 ){
					//显示追加地址信息
					}

				}
			}
		
		ret= CloseComm();
	return ret;
		}
