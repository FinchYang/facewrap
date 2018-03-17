#include "sdtapi.h"
    
int main()
{
  int ret;
  int dev;

  ret =InitComm (1001);//USB接口的iDR210
  if (ret)
  {
        char csn[1024]={0};
        if (Routon_IC_HL_ReadCardSN(csn))//读A卡卡号
        {
               //
        }
        else
        {
              //  MessageBox( NULL, "Routon_IC_HL_ReadCardSN error。",
                //        "错误", MB_OK | MB_ICONERROR );
                printf("Routon_IC_HL_ReadCardSN error\n");
                        return -1;

        }
        int sid=0,bid=0;
        char da[64]={0};
        unsigned char pw[6]={0xff,0xff,0xff,0xff,0xff,0xff};
        ret=Routon_IC_HL_ReadCard (sid,bid,0x60,pw,da);//读0扇区0块的内容
		
}
CloseComm();
}
