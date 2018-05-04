using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace face
{
    class ClassComminterface
    {
        [DllImport("RdCard.dll")]
        public static extern int UCommand1(byte pCmd, int parg0, string parg1, string parg2);


        [DllImport("Comminterface.dll")]
        public static extern int SS_OpenDevice(int nPort, int nBaud, StringBuilder vidpidOrblueToothName);

        [DllImport("Comminterface.dll")]
        public static extern int SS_CloseDevice();

        [DllImport("Comminterface.dll")]
        public static extern int SS_ID_GetIdCardInfo(StringBuilder idcard_info, int timeout);

        [DllImport("Comminterface.dll")]
        public static extern int SS_ID_GetCardInfo(int idType,StringBuilder idcard_info, int timeout);

        [DllImport("Comminterface.dll")]
        public static extern int SS_DEV_GetElectronicLabel(byte[] electronicLabelBuf, int waitTimeSecs);


        [DllImport("Comminterface.dll")]
        public static extern int SS_WSB_OpenDevice();
        [DllImport("Comminterface.dll")]
        public static extern int SS_WSB_CloseDevice(int iReaderHandle, StringBuilder oerrMsg);
        [DllImport("Comminterface.dll")]
        public static extern int SS_WSB_PowerOn(int iReaderHandle, int islot, StringBuilder oATR, ref int atrlen, StringBuilder oerrMsg);
        [DllImport("Comminterface.dll")]
        public static extern int SS_WSB_PowerOff(int iReaderHandle, int islot, StringBuilder oerrMsg);
        [DllImport("Comminterface.dll")]
        public static extern int SS_WSB_RDDF1EF05(int iReaderHandle, StringBuilder oKLB, StringBuilder oGFBB, StringBuilder oFKJGMC, StringBuilder oFKJGDM, StringBuilder oFKJGZS, StringBuilder oFKSJ, StringBuilder oKH, StringBuilder oAQM, StringBuilder oXPXLH, StringBuilder oYYCSDM, StringBuilder oerrMsg);
        [DllImport("Comminterface.dll")]
        public static extern int SS_WSB_RDDF1EF06(int iReaderHandle, StringBuilder oXM, StringBuilder oXB, StringBuilder oMZ, StringBuilder oCSRQ, StringBuilder oSFZH, StringBuilder oerrMsg);

        [DllImport("Comminterface.dll")]
        public static extern int SS_ICC_GetICCInfo(int ICtype, StringBuilder AIDList, StringBuilder tagList, StringBuilder infoBuf);

        [DllImport("Comminterface.dll")]
        public static extern int SS_ICC_GetARQC(int ICtype, byte[] trData, byte[] AIDList, byte[] ARQC, byte[] trAppData);

        [DllImport("Comminterface.dll")]
        public static extern int SS_ICC_ARPCExeScript(int ICtype, byte[] trData, byte[] ARPC, byte[] trAppData, byte[] ScriptResult, byte[] TC);

        [DllImport("Comminterface.dll")]
        public static extern int ss_rf_sb_FindCard();
        [DllImport("Comminterface.dll")]
        public static extern int ss_rf_sb_ReadCardIssuers(StringBuilder CardIdentifier,	StringBuilder CardType,	StringBuilder CardVersion, StringBuilder IssuersID,	StringBuilder IssuingDate, StringBuilder EffectiveData,	StringBuilder CardID);
        [DllImport("Comminterface.dll")]
        public static extern int ss_rf_sb_ReadCardholder(StringBuilder CardID, StringBuilder Name, StringBuilder Name_, StringBuilder Sex, StringBuilder Nation, StringBuilder Address, StringBuilder Birthday);


        //M1卡
        [DllImport("Comminterface.dll")]
        public static extern int SS_M1_PowerOn(byte[] serialNo, int waitTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_M1_AuthenticateKey(int keyAorKeyB, int blockNo, byte[] KeyData, byte[] uid, int waitTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_M1_ReadBlock(int blockNo, byte[] blockData, int waitTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_M1_WriteBlock(int blockNo, byte[] blockData, int waitTimeSecs);

        //指纹
        [DllImport("Comminterface.dll")]
        public static extern int SS_FP_GetFpCharWait(int timeoutsecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_FP_UpFpChar(int bufIndex, byte[] fpCharBuf, int timeoutsecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_FP_DownFpChar(int bufIndex, byte[] fpCharBuf, int timeoutsecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_FP_CompareFpChar(int waitTimeSecs);


        [DllImport("Comminterface.dll")]
        public static extern int SS_setTransmissionMode(byte[] fpCharBuf, int paraLen);
        [DllImport("Comminterface.dll")]
        public static extern int SS_getBatteryInfo(byte[] infoBuf, out int infoBufLen);
        [DllImport("Comminterface.dll")]
        public static extern int SS_LED_UpOrOff(int ledNo, int upOrOff, int color);

        [DllImport("Comminterface.dll")]
        public static extern int SS_SOUND_Beep(int nTimes, int n100millitimes);
        [DllImport("Comminterface.dll")]
        public static extern int SS_SOUND_Voice(int voiceType);

        //显示屏
        [DllImport("Comminterface.dll")]
        public static extern int SS_startDisplayScreen(int x, int y, int rgp, byte[] strMsg, int waitTimeSecs);
        
        [DllImport("Comminterface.dll")]
        public static extern int SS_closeDisplayScreen();

        //密码键盘
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_GetPin(byte[] keyBuf, int timeoutsecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_SetPin(int nMinLen, int nMaxLen, int nAutoEnd, int waitInputTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_GetKey(int keyLimitLen, byte[] keyBuf, int timeoutsecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_DownloadMasterKey(int keyIndex, int algorithmType, byte[] keyData, int keyDataLen, int waitTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_DownloadWorkKey(int keyIndex, int algorithmType, byte[] keyData, int keyDataLen, int waitTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_SetAccountnumber(byte[] accountnumber, int accountnumberLen, int waitTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_GetPinEncrypt(byte[] pinBuf, out int pinBufLen, int waitTimeSecs);

        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_DownloadMasterKeyAndReturnCheckData(int keyIndex, int algorithmType, byte[] keyData, int keyDataLen, byte[] checkData, int waitTimeSecs);
        [DllImport("Comminterface.dll")]
        public static extern int SS_KEY_DownloadWorkKeyAndReturnCheckData(int keyIndex, int algorithmType, byte[] keyData, int keyDataLen, byte[] checkData, int waitTimeSecs);
       
        [DllImport("Comminterface.dll")]
        public static extern int SS_SSSE_ReadCard(StringBuilder SocialSecurityCardInfo, StringBuilder ErrorMsg);

    }
}
