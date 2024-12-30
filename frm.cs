using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq.Expressions;
using static frrjiftest.WebSocketServer;
using Newtonsoft.Json;

namespace frrjiftest
{
    public partial class frm : System.Windows.Forms.Form
    {
        private System.Text.Encoding encode = System.Text.Encoding.GetEncoding("shift_jis");

        public string HostName;
        private WebSocketServer _webSocketServer;
        private ClientWebSocket _clientSocket;

        private const string cnstApp = "frrjiftest";
        private const string cnstSection = "setting";
        public int positionRegister;

        private Random rnd = new Random();

        private FRRJIf.Core mobjCore;
        private FRRJIf.DataTable mobjDataTable;
        private FRRJIf.DataCurPos mobjCurPos;
        private FRRJIf.DataCurPos mobjCurPosUF;
        private FRRJIf.DataCurPos mobjCurPos2;
        private FRRJIf.DataCurPos mobjCurPos3;
        private FRRJIf.DataCurPos mobjCurPos4;
        private FRRJIf.DataCurPos mobjCurPos5;
        private FRRJIf.DataTask mobjTask;
        private FRRJIf.DataTask mobjTaskIgnoreMacro;
        private FRRJIf.DataTask mobjTaskIgnoreKarel;
        private FRRJIf.DataTask mobjTaskIgnoreMacroKarel;
        private FRRJIf.DataPosReg mobjPosReg;
        private FRRJIf.DataPosReg mobjPosReg2;
        private FRRJIf.DataPosReg mobjPosReg3;
        private FRRJIf.DataPosReg mobjPosReg4;
        private FRRJIf.DataPosReg mobjPosReg5;
        private FRRJIf.DataPosRegXyzwpr mobjPosRegXyzwpr;
        private FRRJIf.DataPosRegMG mobjPosRegMG;
        private FRRJIf.DataSysVar mobjSysVarInt;
        private FRRJIf.DataSysVar mobjSysVarInt2;
        private FRRJIf.DataSysVar mobjSysVarReal;
        private FRRJIf.DataSysVar mobjSysVarReal2;
        private FRRJIf.DataSysVar mobjSysVarString;
        private FRRJIf.DataSysVar mobjSysVarString2;
        private FRRJIf.DataSysVarPos mobjSysVarPos;
        private FRRJIf.DataSysVar[] mobjSysVarIntArray;
        private FRRJIf.DataNumReg mobjNumReg;
        private FRRJIf.DataNumReg mobjNumReg2;
        private FRRJIf.DataAlarm mobjAlarm;
        private FRRJIf.DataAlarm mobjAlarmCurrent;
        private FRRJIf.DataAlarm mobjAlarmPasswd;
        private FRRJIf.DataSysVar mobjVarString;
        private FRRJIf.DataString mobjStrReg;
        private FRRJIf.DataString mobjStrRegComment;
        private FRRJIf.DataString mobjDIComment;
        private FRRJIf.DataString mobjDOComment;
        private FRRJIf.DataString mobjRIComment;
        private FRRJIf.DataString mobjROComment;
        private FRRJIf.DataString mobjUIComment;
        private FRRJIf.DataString mobjUOComment;
        private FRRJIf.DataString mobjSIComment;
        private FRRJIf.DataString mobjSOComment;
        private FRRJIf.DataString mobjWIComment;
        private FRRJIf.DataString mobjWOComment;
        private FRRJIf.DataString mobjWSIComment;
        private FRRJIf.DataString mobjAIComment;
        private FRRJIf.DataString mobjAOComment;
        private FRRJIf.DataString mobjGIComment;
        private FRRJIf.DataString mobjGOComment;

        private FRRJIf.DataTable mobjDataTable2;
        private FRRJIf.DataNumReg mobjNumReg3;


        public frm()
        {
            InitializeComponent();
            WebSocketServer.OnDataReceived += UpdateTextBoxes;
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    _webSocketServer?.Stop();
        //    base.OnFormClosing(e);
        //}

        private void cmdClearAlarm_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (mobjCore == null)
                return;

            mobjCore.ClearAlarm(0);
            cmdRefresh.PerformClick();
        }

        private void cmdConnect_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (mobjCore == null)
            {
                //connect
                msubInit();
            }
            else
            {
                //disconnect
                mobjCore.Disconnect();
                msubDisconnected2();
            }
        }

        private void cmdRefresh_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int ii = 0;
            string strTmp = null;
            object vntValue = null;
            Array xyzwpr = new float[9];
            Array config = new short[7];
            Array joint = new float[9];
            short intUF = 0;
            short intUT = 0;
            short intValidC = 0;
            short intValidJ = 0;
            string strProg = "";
            short intLine = 0;
            short intState = 0;
            string strParentProg = "";
            Array intSDO = new short[100];
            Array intSDO2 = new short[100];
            Array intSDO3 = new short[100];
            Array intSDI = new short[10];
            Array intRDO = new short[10];
            Array intRDI = new short[10];
            Array intSO = new short[10];
            Array intSI = new short[10];
            Array intUO = new short[10];
            Array intUI = new short[10];
            Array lngAO = new int[3];
            Array lngAI = new int[3];
            Array lngGO = new int[3];
            Array lngGO2 = new int[3];
            Array lngGI = new int[3];
            Array intWO = new short[5];
            Array intWI = new short[5];
            Array intWSI = new short[5];
            bool blnDT = false;
            bool blnSDO = false;
            bool blnSDO2 = false;
            bool blnSDO3 = false;
            bool blnSDI = false;
            bool blnRDO = false;
            bool blnRDI = false;
            bool blnSO = false;
            bool blnSI = false;
            bool blnUO = false;
            bool blnUI = false;
            bool blnGO = false;
            bool blnGO2 = false;
            bool blnGI = false;
            string strValue = "";

            //check
            if (mobjCore == null)
            {
                return;
            }

            cmdRefresh.Enabled = false;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            mobjDataTable.modfrif.gfdblGetTickCountExStart();

            //Refresh data table
            blnDT = mobjDataTable.Refresh();
            if (blnDT == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read SDO
            blnSDO = mobjCore.ReadSDO(1, ref intSDO, 100);
            if (blnSDO == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }
            blnSDO2 = mobjCore.ReadSDO(10001, ref intSDO2, 100);
            if (blnSDO2 == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }
            blnSDO3 = mobjCore.ReadSDO(11001, ref intSDO3, 100);
            if (blnSDO3 == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read SDI
            blnSDI = mobjCore.ReadSDI(1, ref intSDI, 10);
            if (blnSDI == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read RDO
            blnRDO = mobjCore.ReadRDO(1, ref intRDO, 10);
            if (blnRDO == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read RDI
            blnRDI = mobjCore.ReadRDI(1, ref intRDI, 10);
            if (blnRDI == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read SO
            blnSO = mobjCore.ReadSO(0, ref intSO, 10);
            if (blnSO == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read SI
            blnSI = mobjCore.ReadSI(0, ref intSI, 10);
            if (blnSI == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read UO
            blnUO = mobjCore.ReadUO(1, ref intUO, 10);
            if (blnUO == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read UI
            blnUI = mobjCore.ReadUI(1, ref intUI, 10);
            if (blnUI == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read GO
            blnGO = mobjCore.ReadGO(1, ref lngGO, 3);
            if (blnGO == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }
            blnGO2 = mobjCore.ReadGO(10001, ref lngGO2, 3);
            if (blnGO2 == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read GI
            blnGI = mobjCore.ReadGI(1, ref lngGI, 3);
            if (blnGI == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read AO. Offset 1000 for AO
            if (mobjCore.ReadGO(1000 + 1, ref lngAO, 3) == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read AI. Offset 1000 for AO
            if (mobjCore.ReadGI(1000 + 1, ref lngAI, 2) == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read WO. Offset 8000 for WO
            if (mobjCore.ReadSDO(8001, ref intWO, 5) == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read WI. Offset 8000 for WI
            if (mobjCore.ReadSDI(8001, ref intWI, 5) == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            //read WSI. Offset 8400 for WI
            if (mobjCore.ReadSDI(8401, ref intWSI, 1) == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            try
            {
                strTmp = "Time = " + Convert.ToInt16(mobjDataTable.modfrif.gfdblGetTickCountExEnd()) + "(msec)" + Environment.NewLine;
            }
            catch (Exception)
            {
                strTmp = "Time = " + "Convert Error!! " + Environment.NewLine;
            }

            {
                if (mobjCurPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                {
                    strTmp = strTmp + "--- CurPos GP1 World ---" + Environment.NewLine;
                    strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                }
                else
                {
                    strTmp = strTmp + "CurPos Error!!!" + Environment.NewLine;
                }
            }

            {
                if (mobjCurPosUF.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                {
                    strTmp = strTmp + "--- CurPos GP1 Current UF ---" + Environment.NewLine;
                    strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                }
                else
                {
                    strTmp = strTmp + "CurPos Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjCurPos2.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                {
                    strTmp = strTmp + "--- CurPos GP2 World ---" + Environment.NewLine;
                    strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                }
                else
                {
                    strTmp = strTmp + "CurPos2 Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjCurPos3.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                {
                    strTmp = strTmp + "--- CurPos GP3 World ---" + Environment.NewLine;
                    strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                }
                else
                {
                    strTmp = strTmp + "CurPos3 Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjCurPos4.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                {
                    strTmp = strTmp + "--- CurPos GP4 World ---" + Environment.NewLine;
                    strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                }
                else
                {
                    strTmp = strTmp + "CurPos4 Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjCurPos5.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                {
                    strTmp = strTmp + "--- CurPos GP5 World ---" + Environment.NewLine;
                    strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                }
                else
                {
                    strTmp = strTmp + "CurPos5 Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjTask.GetValue(ref strProg, ref intLine, ref intState, ref strParentProg, encode))
                {
                    strTmp = strTmp + "--- Task ---" + Environment.NewLine;
                    strTmp = strTmp + mstrTask(mobjTask.Index(), strProg, intLine, intState, strParentProg);
                }
                else
                {
                    strTmp = strTmp + "Task Error!!!" + Environment.NewLine;
                }
                if (mobjTaskIgnoreMacro.GetValue(ref strProg, ref intLine, ref intState, ref strParentProg, encode))
                {
                    strTmp = strTmp + "--- Task Ignore Macro ---" + Environment.NewLine;
                    strTmp = strTmp + mstrTask(mobjTaskIgnoreMacro.Index(), strProg, intLine, intState, strParentProg);
                }
                else
                {
                    strTmp = strTmp + "Task Error!!!" + Environment.NewLine;
                }
                if (mobjTaskIgnoreKarel.GetValue(ref strProg, ref intLine, ref intState, ref strParentProg, encode))
                {
                    strTmp = strTmp + "--- Task Ignore KAREL ---" + Environment.NewLine;
                    strTmp = strTmp + mstrTask(mobjTaskIgnoreKarel.Index(), strProg, intLine, intState, strParentProg);
                }
                else
                {
                    strTmp = strTmp + "Task Error!!!" + Environment.NewLine;
                }
                if (mobjTaskIgnoreMacroKarel.GetValue(ref strProg, ref intLine, ref intState, ref strParentProg, encode))
                {
                    strTmp = strTmp + "--- Task Ignore Macro, KAREL ---" + Environment.NewLine;
                    strTmp = strTmp + mstrTask(mobjTaskIgnoreMacroKarel.Index(), strProg, intLine, intState, strParentProg);
                }
                else
                {
                    strTmp = strTmp + "Task Error!!!" + Environment.NewLine;
                }
            }
            strTmp = strTmp + "--- SysVar ---" + Environment.NewLine;
            vntValue = 0;
            {
                if (mobjSysVarInt.GetValue(ref vntValue, encode) == true)
                {
                    strTmp = strTmp + mobjSysVarInt.SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjSysVarInt.SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjSysVarInt2.GetValue(ref vntValue, encode) == true)
                {
                    strTmp = strTmp + mobjSysVarInt2.SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjSysVarInt2.SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjSysVarReal.GetValue(ref vntValue, encode) == true)
                {
                    strTmp = strTmp + mobjSysVarReal.SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjSysVarReal.SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjSysVarReal2.GetValue(ref vntValue, encode) == true)
                {
                    strTmp = strTmp + mobjSysVarReal2.SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjSysVarReal2.SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjSysVarString.GetValue(ref vntValue, encode) == true)
                {
                    strTmp = strTmp + mobjSysVarString.SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjSysVarString.SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjSysVarString2.GetValue(ref vntValue, encode) == true)
                {
                    strTmp = strTmp + mobjSysVarString2.SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjSysVarString2.SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }

            {
                if (mobjSysVarPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                {
                    strTmp = strTmp + mobjSysVarPos.SysVarName() + Environment.NewLine;
                    strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                }
                else
                {
                    strTmp = strTmp + mobjSysVarPos.SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }
            {
                if (mobjVarString.GetValue(ref vntValue, encode))
                {
                    strTmp = strTmp + mobjVarString.SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjVarString.SysVarName() + " : Error!!! " + Environment.NewLine;
                }
            }
            strTmp = strTmp + "--- NumReg ---" + Environment.NewLine;
            {
                for (ii = mobjNumReg.GetStartIndex(); ii <= mobjNumReg.GetEndIndex(); ii++)
                {
                    if (mobjNumReg.GetValue(ii, ref vntValue) == true)
                    {
                        strTmp = strTmp + "R[" + ii + "] = " + vntValue + Environment.NewLine;
                    }
                    else
                    {
                        strTmp = strTmp + "R[" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }
            {
                for (ii = mobjNumReg2.GetStartIndex(); ii <= mobjNumReg2.GetEndIndex(); ii++)
                {
                    if (mobjNumReg2.GetValue(ii, ref vntValue) == true)
                    {
                        strTmp = strTmp + "R[" + ii + "] = " + vntValue + Environment.NewLine;
                    }
                    else
                    {
                        strTmp = strTmp + "R[" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }
            strTmp = strTmp + "--- PosReg GP1 ---" + Environment.NewLine;
            {
                for (ii = mobjPosReg.GetStartIndex(); ii <= mobjPosReg.GetEndIndex(); ii++)
                {
                    if (mobjPosReg.GetValue(ii, ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                    {
                        strTmp = strTmp + "PR[" + ii + "]" + Environment.NewLine;
                        strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                    }
                    else
                    {
                        strTmp = strTmp + "PR[" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }
            strTmp = strTmp + "--- PosReg GP2 ---" + Environment.NewLine;
            {
                for (ii = mobjPosReg2.GetStartIndex(); ii <= mobjPosReg2.GetEndIndex(); ii++)
                {
                    if (mobjPosReg2.GetValue(ii, ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                    {
                        strTmp = strTmp + "PR[" + ii + "]" + Environment.NewLine;
                        strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                    }
                    else
                    {
                        strTmp = strTmp + "PR[GP2:" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }
            strTmp = strTmp + "--- PosReg GP3 ---" + Environment.NewLine;
            {
                for (ii = mobjPosReg3.GetStartIndex(); ii <= mobjPosReg3.GetEndIndex(); ii++)
                {
                    if (mobjPosReg3.GetValue(ii, ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                    {
                        strTmp = strTmp + "PR[" + ii + "]" + Environment.NewLine;
                        strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                    }
                    else
                    {
                        strTmp = strTmp + "PR[GP3:" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }

            strTmp = strTmp + "--- PosReg GP4 ---" + Environment.NewLine;
            {
                for (ii = mobjPosReg4.GetStartIndex(); ii <= mobjPosReg4.GetEndIndex(); ii++)
                {
                    if (mobjPosReg4.GetValue(ii, ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                    {
                        strTmp = strTmp + "PR[" + ii + "]" + Environment.NewLine;
                        strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                    }
                    else
                    {
                        strTmp = strTmp + "PR[GP4:" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }

            strTmp = strTmp + "--- PosReg GP5 ---" + Environment.NewLine;
            {
                for (ii = mobjPosReg5.GetStartIndex(); ii <= mobjPosReg5.GetEndIndex(); ii++)
                {
                    if (mobjPosReg5.GetValue(ii, ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
                    {
                        strTmp = strTmp + "PR[" + ii + "]" + Environment.NewLine;
                        strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
                    }
                    else
                    {
                        strTmp = strTmp + "PR[GP5:" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }
            strTmp = strTmp + "--- SDO ---" + Environment.NewLine;
            if (blnSDO == true)
            {
                strTmp = strTmp + mstrIO("SDO", 1, 100, ref intSDO) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- SDO[1000x] ---" + Environment.NewLine;
            if (blnSDO2 == true)
            {
                strTmp = strTmp + mstrIO("SDO", 10001, 10100, ref intSDO2) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- SDO[1100x] ---" + Environment.NewLine;
            if (blnSDO3 == true)
            {
                strTmp = strTmp + mstrIO("SDO", 11001, 11100, ref intSDO3) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- SDI ---" + Environment.NewLine;
            if (blnSDI == true)
            {
                strTmp = strTmp + mstrIO("SDI", 1, 10, ref intSDI) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- RDO ---" + Environment.NewLine;
            if (blnRDO == true)
            {
                strTmp = strTmp + mstrIO("RDO", 1, 8, ref intRDO) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- RDI ---" + Environment.NewLine;
            if (blnRDI == true)
            {
                strTmp = strTmp + mstrIO("RDI", 1, 8, ref intRDI) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- SO ---" + Environment.NewLine;
            if (blnSO == true)
            {
                strTmp = strTmp + mstrIO("SO", 0, 9, ref intSO) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- SI ---" + Environment.NewLine;
            if (blnSI == true)
            {
                strTmp = strTmp + mstrIO("SI", 0, 9, ref intSI) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- UO ---" + Environment.NewLine;
            if (blnUO == true)
            {
                strTmp = strTmp + mstrIO("UO", 1, 10, ref intUO) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- UI ---" + Environment.NewLine;
            if (blnUI == true)
            {
                strTmp = strTmp + mstrIO("UI", 1, 10, ref intUI) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- GO ---" + Environment.NewLine;
            if (blnGO == true)
            {
                strTmp = strTmp + mstrIO2("GO", 1, 3, ref lngGO) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- GO[1000x] ---" + Environment.NewLine;
            if (blnGO == true)
            {
                strTmp = strTmp + mstrIO2("GO", 10001, 10003, ref lngGO2) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- GI ---" + Environment.NewLine;
            if (blnGI == true)
            {
                strTmp = strTmp + mstrIO2("GI", 1, 3, ref lngGI) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- AO ---" + Environment.NewLine;
            strTmp = strTmp + mstrIO2("AO", 1, 3, ref lngAO) + Environment.NewLine;
            strTmp = strTmp + "--- AI ---" + Environment.NewLine;
            strTmp = strTmp + mstrIO2("AI", 1, 3, ref lngAI) + Environment.NewLine;
            strTmp = strTmp + "--- WO ---" + Environment.NewLine;
            if (blnSDO == true)
            {
                strTmp = strTmp + mstrIO("WO", 1, 5, ref intWO) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error" + Environment.NewLine;
            }
            strTmp = strTmp + "--- WI ---" + Environment.NewLine;
            if (blnSDI == true)
            {
                strTmp = strTmp + mstrIO("WI", 1, 5, ref intWI) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error"  + Environment.NewLine;
            }
            strTmp = strTmp + "--- WSI ---"  + Environment.NewLine;
            if (blnSDI == true)
            {
                strTmp = strTmp + mstrIO("WSI", 1, 1, ref intWSI) + Environment.NewLine;
            }
            else
            {
                strTmp = strTmp + "Error"  + Environment.NewLine;
            }

            strTmp = strTmp + "--- Alarm List ---" + Environment.NewLine;
            strTmp = strTmp + "[ mobjAlarm ]" + Environment.NewLine;
            for (ii = 1; ii <= 5; ii++)
            {
                strTmp = strTmp + mstrAlarm(ref mobjAlarm, ii);
            }

            strTmp = strTmp + "[ mobjAlarmCurrent ]" + Environment.NewLine;
            strTmp = strTmp + mstrAlarm(ref mobjAlarmCurrent, 1);

            strTmp = strTmp + "[ mobjAlarmPasswd ]" + Environment.NewLine;
            strTmp = strTmp + mstrAlarm(ref mobjAlarmPasswd, 1);

            strTmp = strTmp + "--- StrReg ---" + Environment.NewLine;
            for (ii = mobjStrReg.GetStartIndex(); ii <= mobjStrReg.GetEndIndex(); ii++)
            {
                string strComment = "";
                mobjStrRegComment.GetValue(ii, ref strComment);
                if (mobjStrReg.GetValue(ii, ref strValue) == true)
                {
                    strTmp = strTmp + String.Format("SR[{0}:{1}] = {2}", ii, strComment, strValue) + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + String.Format("SR[{0}]  : Error!!! ", ii) + Environment.NewLine;
                }
            }

            strTmp = strTmp + "--- DataPosRegMG ---" + Environment.NewLine;
            for (int i = mobjPosRegMG.GetStartIndex(); i <= mobjPosRegMG.GetEndIndex(); i++)
            {
                Array xyzwprMG = (new float[9]);
                Array configMG = (new short[7]);
                bool res = mobjPosRegMG.GetValueXyzwpr(i, 1, xyzwprMG, configMG);
                string tmp =
                    "index=" + i.ToString() +
                    " X=" + xyzwprMG.GetValue(0).ToString() +
                    " Y=" + xyzwprMG.GetValue(1).ToString() +
                    " Z=" + xyzwprMG.GetValue(2).ToString() +
                    " W=" + xyzwprMG.GetValue(3).ToString() +
                    " P=" + xyzwprMG.GetValue(4).ToString() +
                    " R=" + xyzwprMG.GetValue(5).ToString() +
                    " E1=" + xyzwprMG.GetValue(6).ToString() +
                    " E2=" + xyzwprMG.GetValue(7).ToString() +
                    " E3=" + xyzwprMG.GetValue(8).ToString() +
                    " C1=" + configMG.GetValue(0).ToString() +
                    " C2=" + configMG.GetValue(1).ToString() +
                    " C3=" + configMG.GetValue(2).ToString() +
                    " C4=" + configMG.GetValue(3).ToString() +
                    " C5=" + configMG.GetValue(4).ToString() +
                    " C6=" + configMG.GetValue(5).ToString() +
                    " C7=" + configMG.GetValue(6).ToString() +
                    Environment.NewLine;
                strTmp += tmp;

                Array jointMG = (new float[9]);
                res = mobjPosRegMG.GetValueJoint(i, 2, jointMG);
                tmp =
                    "index=" + i.ToString() +
                    " J1=" + jointMG.GetValue(0).ToString() +
                    " J2=" + jointMG.GetValue(1).ToString() +
                    " J3=" + jointMG.GetValue(2).ToString() +
                    " J4=" + jointMG.GetValue(3).ToString() +
                    " J5=" + jointMG.GetValue(4).ToString() +
                    " J6=" + jointMG.GetValue(5).ToString() +
                    " J7=" + jointMG.GetValue(6).ToString() +
                    " J8=" + jointMG.GetValue(7).ToString() +
                    " J9=" + jointMG.GetValue(8).ToString() +
                    Environment.NewLine;
                strTmp += tmp;
            }

            strTmp += Environment.NewLine;
            strTmp += mstrComment("--- DI Comment ---", mobjDIComment, encode);
            strTmp += mstrComment("--- DO Comment ---", mobjDOComment, encode);
            strTmp += mstrComment("--- RI Comment ---", mobjRIComment, encode);
            strTmp += mstrComment("--- RO Comment ---", mobjROComment, encode);
            strTmp += mstrComment("--- UI Comment ---", mobjUIComment, encode);
            strTmp += mstrComment("--- UO Comment ---", mobjUOComment, encode);
            strTmp += mstrComment("--- SI Comment ---", mobjSIComment, encode);
            strTmp += mstrComment("--- SO Comment ---", mobjSOComment, encode);
            strTmp += mstrComment("--- WI Comment ---", mobjWIComment, encode);
            strTmp += mstrComment("--- WO Comment ---", mobjWOComment, encode);
            strTmp += mstrComment("--- WSI Comment ---", mobjWSIComment, encode);
            strTmp += mstrComment("--- AI Comment ---", mobjAIComment, encode);
            strTmp += mstrComment("--- AO Comment ---", mobjAOComment, encode);
            strTmp += mstrComment("--- GI Comment ---", mobjGIComment, encode);
            strTmp += mstrComment("--- GO Comment ---", mobjGOComment, encode);

            txtResult.Text = strTmp;

            cmdRefresh.Enabled = true;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
        int static_cmdSetGO_Click_lngCount;

        private void cmdSetGO_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            Array lngVal = new int[3];
            int ii = 0;
            bool blnRes = false;
            short intStartIndex = 0;

            if (object.ReferenceEquals(eventSender, _cmdSetGO_1))
            {
                intStartIndex = 10001;
            }
            else
            {
                intStartIndex = 1;
            }

            static_cmdSetGO_Click_lngCount = static_cmdSetGO_Click_lngCount + 1;
            for (ii = 0; ii <= 2; ii++)
            {
                lngVal.SetValue((int)(static_cmdSetGO_Click_lngCount * (ii + 1)), ii);
            }

            blnRes = mobjCore.WriteGO(intStartIndex, lngVal, 3);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();

        }

        private void cmdSetNumReg_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int intRand = 0;
            int ii = 0;
            int[] intValues = new int[101];
            float[] sngValues = new float[101];

            intRand = rnd.Next(1, 10);
            {
                for (ii = 0; ii <= mobjNumReg.GetEndIndex() - mobjNumReg.GetStartIndex(); ii++)
                {
                    intValues[ii] = (ii + 1) * intRand;
                }
                if (mobjNumReg.SetValues(mobjNumReg.GetStartIndex(), intValues, mobjNumReg.GetEndIndex() - mobjNumReg.GetStartIndex() + 1) == false)
                {
                    MessageBox.Show("SetNumReg Int Error");
                }
            }
            {
                for (ii = 0; ii <= mobjNumReg2.GetEndIndex() - mobjNumReg2.GetStartIndex(); ii++)
                {
                    sngValues[ii] = (float)((ii + 1) * intRand * 1.1);
                }
                if (mobjNumReg2.SetValues(mobjNumReg2.GetStartIndex(), sngValues, mobjNumReg2.GetEndIndex() - mobjNumReg2.GetStartIndex() + 1) == false)
                {
                    MessageBox.Show("SetNumReg Real Error");
                }
            }
            cmdRefresh.PerformClick();
        }

        private void cmdSetPosReg_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int intRand = 0;
            int ii = 0;
            int jj = 0;
            Array sngJoint = new float[6];

            intRand = rnd.Next(1, 10);
            {
                for (ii = mobjPosReg.GetStartIndex(); ii <= mobjPosReg.GetEndIndex(); ii++)
                {
                    for (jj = sngJoint.GetLowerBound(0); jj <= sngJoint.GetUpperBound(0); jj++)
                    {
                        sngJoint.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    }
                    mobjPosReg.SetValueJoint(ii, ref sngJoint, 15, 15);
                }
            }
            {
                for (ii = mobjPosReg2.GetStartIndex(); ii <= mobjPosReg2.GetEndIndex(); ii++)
                {
                    jj = 0;
                    sngJoint.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    mobjPosReg2.SetValueJoint(ii, ref sngJoint, 15, 15);
                }
            }
            {
                for (ii = mobjPosReg3.GetStartIndex(); ii <= mobjPosReg3.GetEndIndex(); ii++)
                {
                    jj = 0;
                    sngJoint.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    mobjPosReg3.SetValueJoint(ii, ref sngJoint, 15, 15);
                }
            }
            {
                for (ii = mobjPosReg4.GetStartIndex(); ii <= mobjPosReg4.GetEndIndex(); ii++)
                {
                    jj = 0;
                    sngJoint.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    mobjPosReg4.SetValueJoint(ii, ref sngJoint, 15, 15);
                }
            }
            {
                for (ii = mobjPosReg5.GetStartIndex(); ii <= mobjPosReg5.GetEndIndex(); ii++)
                {
                    jj = 0;
                    sngJoint.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    mobjPosReg5.SetValueJoint(ii, ref sngJoint, 15, 15);
                }
            }
            cmdRefresh.PerformClick();
        }

        private void cmdSetPosRegX_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int intRand = 0;
            int ii = 0;
            int jj = 0;
            Array sngArray = new float[9];
            Array intConfig = new short[7];

            mobjDataTable.modfrif.gfdblGetTickCountExStart();

            intRand = rnd.Next(1, 10);
            {
                for (ii = mobjPosReg.GetStartIndex(); ii <= mobjPosReg.GetEndIndex(); ii++)
                {
                    for (jj = sngArray.GetLowerBound(0); jj <= sngArray.GetUpperBound(0); jj++)
                    {
                        sngArray.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    }
                    intConfig.SetValue((short)ii, 4);
                    intConfig.SetValue((short)ii, 5);
                    intConfig.SetValue((short)ii, 6);
                    mobjPosReg.SetValueXyzwpr(ii, ref sngArray, ref intConfig, -1, -1);
                }
            }
            Debug.Print(string.Format("Time {0} ms", Convert.ToInt16(mobjDataTable.modfrif.gfdblGetTickCountExEnd())));
            cmdRefresh.PerformClick();

        }

        private void cmdSetPosRegX2_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int intRand = 0;
            int ii = 0;
            int jj = 0;
            Array sngArray = new float[6];
            Array intConfig = new short[7];
            bool blnRes = false;

            mobjDataTable.modfrif.gfdblGetTickCountExStart();
            intRand = rnd.Next(1, 10);
            {
                for (ii = mobjPosRegXyzwpr.GetStartIndex(); ii <= mobjPosRegXyzwpr.GetEndIndex(); ii++)
                {
                    for (jj = sngArray.GetLowerBound(0); jj <= sngArray.GetUpperBound(0); jj++)
                    {
                        sngArray.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    }
                    intConfig.SetValue((short)ii, 4);
                    intConfig.SetValue((short)ii, 5);
                    intConfig.SetValue((short)ii, 6);
                    blnRes = mobjPosRegXyzwpr.SetValueXyzwpr(ii, ref sngArray, ref intConfig);
                    if (blnRes == false)
                    {
                        MessageBox.Show("Error mobjPosRegXyzwpr.SetValueXyzwpr");
                    }
                }
                blnRes = mobjPosRegXyzwpr.Update();
                if (blnRes == false)
                {
                    MessageBox.Show("Error mobjPosRegXyzwpr.Update");
                }
            }
            Debug.Print(string.Format("Time {0} ms", Convert.ToInt16(mobjDataTable.modfrif.gfdblGetTickCountExEnd())));
            cmdRefresh.PerformClick();


        }
        int static_cmdSetRDI_Click_lngCount;

        private void cmdSetRDI_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short[] intVal = new short[10];
            int ii = 0;
            bool blnRes = false;

            static_cmdSetRDI_Click_lngCount = static_cmdSetRDI_Click_lngCount + 1;
            if (static_cmdSetRDI_Click_lngCount % 2 == 1)
            {
                for (ii = 0; ii <= 7; ii++)
                {
                    intVal[ii] = 1;
                }
            }
            Array intValTmp = intVal;
            blnRes = mobjCore.WriteRDI(1, intValTmp, 8);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();

        }
        int static_cmdSetRDO_Click_lngCount;

        private void cmdSetRDO_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            Array intVal = new short[10];
            int ii = 0;
            bool blnRes = false;

            static_cmdSetRDO_Click_lngCount = static_cmdSetRDO_Click_lngCount + 1;
            if (static_cmdSetRDO_Click_lngCount % 2 == 1)
            {
                for (ii = 0; ii <= 7; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteRDO(1, intVal, 8);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();

        }
        int static_cmdSetSDI_Click_lngCount;

        private void cmdSetSDI_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            Array intVal = new short[10];
            int ii = 0;
            bool blnRes = false;

            static_cmdSetSDI_Click_lngCount = static_cmdSetSDI_Click_lngCount + 1;
            if (static_cmdSetSDI_Click_lngCount % 2 == 1)
            {
                for (ii = 0; ii <= 9; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteSDI(1, intVal, 10);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();

        }
        int static_cmdSetSDO_Click_lngCount;

        private void cmdSetSDO_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            Array intVal = new short[100];
            int ii = 0;
            bool blnRes = false;
            short intStartIndex = 0;

            if (object.ReferenceEquals(eventSender, _cmdSetSDO_1))
            {
                intStartIndex = 10001;
            }
            else if (object.ReferenceEquals(eventSender, _cmdSetSDO_2))
            {
                intStartIndex = 11001;
            }
            else
            {
                intStartIndex = 1;
            }

            static_cmdSetSDO_Click_lngCount = static_cmdSetSDO_Click_lngCount + 1;
            if (static_cmdSetSDO_Click_lngCount % 2 == 1)
            {
                for (ii = 0; ii <= 99; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteSDO(intStartIndex, intVal, 100);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();
        }
        int static_cmdsetgi_Click_lngCount;

        private void cmdsetgi_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            Array lngVal = new int[3];
            int ii = 0;
            bool blnRes = false;

            static_cmdsetgi_Click_lngCount = static_cmdsetgi_Click_lngCount + 1;
            for (ii = 0; ii <= 2; ii++)
            {
                lngVal.SetValue((int)(static_cmdsetgi_Click_lngCount * (ii + 1)), ii);
            }

            blnRes = mobjCore.WriteGI(1, lngVal, 3);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();

        }

        private void cmdWriteSysVar_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int lngOld = 0;
            float sngOld = 0;
            string strOld = "";
            float sngXOld = 0;
            int lngNew = 0;
            float sngNew = 0;
            string strNew = null;
            float sngXNew = 0;
            int lngConf = 0;
            float sngConf = 0;
            string strConf = "";
            Array xyzwpr = new float[9];
            Array config = new short[7];
            Array joint = new float[9];
            short intUF = 0;
            short intUT = 0;
            short intValidC = 0;
            short intValidJ = 0;
            object objTmp = null;

            try
            {
                if (MessageBox.Show("Are you sure to test writing system variables?", "frrjiftest", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                //store old values
                mobjDataTable.Refresh();
                objTmp = 0;
                mobjSysVarInt2.GetValue(ref objTmp, encode);
                lngOld = (int)objTmp;
                mobjSysVarReal2.GetValue(ref objTmp, encode);
                sngOld = (float)objTmp;
                mobjSysVarPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ);
                sngXOld = (float)xyzwpr.GetValue(0);
                mobjSysVarString.GetValue(ref objTmp, encode);
                strOld = (string)objTmp;

                //make new values
                lngNew = 999;
                sngNew = sngOld + 1;
                strNew = "abc";
                sngXNew = sngXOld + 1;
                xyzwpr.SetValue(sngXNew, 0);

                //write dummy values
                mobjSysVarInt2.SetValue(lngNew);
                mobjSysVarString.SetValue(strNew);
                mobjSysVarReal2.SetValue(sngNew);
                mobjSysVarPos.SetValueXyzwpr(ref xyzwpr, ref config, intUF, intUT);


                //confirm
                mobjDataTable.Refresh();
                objTmp = 0;
                mobjSysVarInt2.GetValue(ref objTmp, encode);
                lngConf = (int)objTmp;
                System.Diagnostics.Debug.Assert(lngNew == lngConf, "");
                mobjSysVarReal2.GetValue(ref objTmp, encode);
                sngConf = (float)objTmp;
                System.Diagnostics.Debug.Assert(sngNew == sngConf, "");
                mobjSysVarPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ);
                System.Diagnostics.Debug.Assert(sngXNew == (float)xyzwpr.GetValue(0), "");
                mobjSysVarString.GetValue(ref objTmp, encode);
                strConf = (string)objTmp;
                System.Diagnostics.Debug.Assert(strNew == strConf, "");


                //restore old values
                mobjSysVarInt2.SetValue(lngOld);
                mobjSysVarString.SetValue(strOld);
                mobjSysVarReal2.SetValue(sngOld);
                xyzwpr.SetValue(sngXOld, 0);
                mobjSysVarPos.SetValueXyzwpr(ref xyzwpr, ref config, intUF, intUT);

                //confirm again
                mobjDataTable.Refresh();
                objTmp = 0;
                mobjSysVarInt2.GetValue(ref objTmp, encode);
                lngConf = (int)objTmp;
                System.Diagnostics.Debug.Assert(lngOld == lngConf, "");
                mobjSysVarReal2.GetValue(ref objTmp, encode);
                sngConf = (float)objTmp;
                System.Diagnostics.Debug.Assert(sngOld == sngConf, "");
                mobjSysVarPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ);
                System.Diagnostics.Debug.Assert(sngXOld == (float)xyzwpr.GetValue(0), "");
                mobjSysVarString.GetValue(ref objTmp, encode);
                strConf = (string)objTmp;
                System.Diagnostics.Debug.Assert(strOld == strConf, "");

                System.Windows.Forms.Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        private void msubInit()
        {
            bool blnRes = false;
            string strHost = null;
            int lngTmp = 0;

            try {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                mobjCore = new FRRJIf.Core(encode);

                // You need to set data table before connecting.
                mobjDataTable = mobjCore.get_DataTable();

                {
                    mobjAlarm = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_LIST, 5, 0);
                    mobjAlarmCurrent = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_CURRENT, 1, 0);
                    mobjAlarmPasswd = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_PASSWORD, 1);
                    //mobjCurPos = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1);
                    mobjCurPos = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1, 15);
                    mobjCurPosUF = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1, 15);
                    //mobjCurPos2 = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 2);
                    mobjCurPos2 = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 2, 15);
                    mobjCurPos3 = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 3, 0);
                    mobjCurPos4 = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 4, 0);
                    mobjCurPos5 = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 5, 0);
                    mobjTask = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK, 1);

                    mobjTaskIgnoreMacro = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO, 1);
                    mobjTaskIgnoreKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_KAREL, 1);
                    mobjTaskIgnoreMacroKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO_KAREL, 1);

                    mobjPosReg = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 1, 1, 100);
                    mobjPosReg2 = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 2, 1, 4);
                    mobjPosReg3 = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 3, 1, 10);
                    mobjPosReg4 = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 4, 1, 10);
                    mobjPosReg5 = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 5, 1, 10);

                    mobjSysVarInt = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$FAST_CLOCK");
                    mobjSysVarInt2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");
                    mobjSysVarReal = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$MOR_GRP[1].$CURRENT_ANG[1]");
                    mobjSysVarReal2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$DUTY_TEMP");
                    mobjSysVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$TIMER[10].$COMMENT");
                    mobjSysVarString2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$TIMER[2].$COMMENT");
                    mobjSysVarPos = mobjDataTable.AddSysVarPos(FRRJIf.FRIF_DATA_TYPE.SYSVAR_POS, "$MNUTOOL[1,1]");

                    mobjVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$[HTTPKCL]CMDS[1]");

                    mobjNumReg = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 5);
                    mobjNumReg2 = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 6, 10);
                    mobjPosRegXyzwpr = mobjDataTable.AddPosRegXyzwpr(FRRJIf.FRIF_DATA_TYPE.POSREG_XYZWPR, 1, 1, 10);
                    mobjPosRegMG = mobjDataTable.AddPosRegMG(FRRJIf.FRIF_DATA_TYPE.POSREGMG, "C,J6", 1, 10);

                    mobjDIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.SDI_COMMENT, 1, 3);
                    mobjDOComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.SDO_COMMENT, 1, 3);
                    mobjRIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.RDI_COMMENT, 1, 3);
                    mobjROComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.RDO_COMMENT, 1, 3);
                    mobjUIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.UI_COMMENT, 1, 3);
                    mobjUOComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.UO_COMMENT, 1, 3);
                    mobjSIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.SI_COMMENT, 1, 3);
                    mobjSOComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.SO_COMMENT, 1, 3);
                    mobjWIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.WI_COMMENT, 1, 3);
                    mobjWOComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.WO_COMMENT, 1, 3);
                    mobjWSIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.WSI_COMMENT, 1, 3);
                    mobjAIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.AI_COMMENT, 1, 3);
                    mobjAOComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.AO_COMMENT, 1, 3);
                    mobjGIComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.GI_COMMENT, 1, 3);
                    mobjGOComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.GO_COMMENT, 1, 3);

                    mobjStrReg = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG, 1, 3);
                    mobjStrRegComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG_COMMENT, 1, 3);
                    Debug.Assert(mobjStrRegComment != null);
                }

                // 2nd data table.
                // You must not set the first data table.
                mobjDataTable2 = mobjCore.get_DataTable2();
                mobjNumReg3 = mobjDataTable2.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 5);
                mobjSysVarIntArray = new FRRJIf.DataSysVar[10];
                mobjSysVarIntArray[0] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[1].$TIMER_VAL");
                mobjSysVarIntArray[1] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[2].$TIMER_VAL");
                mobjSysVarIntArray[2] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[3].$TIMER_VAL");
                mobjSysVarIntArray[3] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[4].$TIMER_VAL");
                mobjSysVarIntArray[4] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[5].$TIMER_VAL");
                mobjSysVarIntArray[5] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[6].$TIMER_VAL");
                mobjSysVarIntArray[6] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[7].$TIMER_VAL");
                mobjSysVarIntArray[7] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[8].$TIMER_VAL");
                mobjSysVarIntArray[8] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[9].$TIMER_VAL");
                mobjSysVarIntArray[9] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");

                //get host name
                if (string.IsNullOrEmpty(HostName)) {
                    strHost = Interaction.GetSetting(cnstApp, cnstSection, "HostName", "");
                    strHost = Interaction.InputBox("Please input robot host name", "frrjiftest", strHost, 0, 0);
                    if (string.IsNullOrEmpty(strHost)) {
                        System.Environment.Exit(0);
                    }
                    Interaction.SaveSetting(cnstApp, cnstSection, "HostName", strHost);
                    HostName = strHost;
                } else {
                    strHost = HostName;
                }

                //get time out value
                lngTmp = Convert.ToInt32(Interaction.GetSetting(cnstApp, cnstSection, "TimeOut", "-1"));

                //connect
                if (lngTmp > 0)
                    mobjCore.set_TimeOutValue(lngTmp);
                blnRes = mobjCore.Connect(strHost);
                if (blnRes == false) {
                    msubDisconnected();
                } else {
                    msubConnected();
                }

                System.Windows.Forms.Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex) {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
                System.Environment.Exit(0);
            }


        }

        private void frm_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            msubInit();
        }

        private void frm_FormClosed(System.Object eventSender, System.Windows.Forms.FormClosedEventArgs eventArgs)
        {
            if (mobjCore != null)
            {
                mobjCore.Disconnect();
            }
            mobjCore = null;
        }

        private string mstrIO(string strIOType, short StartIndex, short EndIndex, ref Array values)
        {
            string tmp = null;
            int ii = 0;

            tmp = strIOType + "[" + Convert.ToString(StartIndex) + "-" + Convert.ToString(EndIndex) + "]=";
            for (ii = 0; ii <= EndIndex - StartIndex; ii++)
            {
                if ((short)values.GetValue(ii) == 0)
                {
                    tmp = tmp + "0";
                }
                else
                {
                    tmp = tmp + "1";
                }
            }

            return tmp;
        }

        private string mstrIO2(string strIOType, short StartIndex, short EndIndex, ref Array values)
        {
            string tmp = null;
            int ii = 0;

            tmp = strIOType + "[" + Convert.ToString(StartIndex) + "-" + Convert.ToString(EndIndex) + "]=";
            for (ii = 0; ii <= EndIndex - StartIndex; ii++)
            {
                if (ii != 0)
                {
                    tmp = tmp + ",";
                }
                tmp = tmp + values.GetValue(ii);
            }

            return tmp;
        }


        private string mstrPos(ref Array xyzwpr, ref Array config, ref Array joint, short intValidC, short intValidJ, int UF, int UT)
        {
            string tmp = "";
            int ii = 0;

            tmp = tmp + "UF = " + UF + ", ";
            tmp = tmp + "UT = " + UT + Environment.NewLine;
            if (intValidC != 0)
            {
                tmp = tmp + "XYZWPR = ";
                //5
                for (ii = 0; ii <= 8; ii++)
                {
                    tmp = tmp + xyzwpr.GetValue(ii) + " ";
                }

                tmp = tmp + Environment.NewLine + "CONFIG = ";
                if ((short)config.GetValue(0) != 0)
                {
                    tmp = tmp + "F ";
                }
                else
                {
                    tmp = tmp + "N ";
                }
                if ((short)config.GetValue(1) != 0)
                {
                    tmp = tmp + "L ";
                }
                else
                {
                    tmp = tmp + "R ";
                }
                if ((short)config.GetValue(2) != 0)
                {
                    tmp = tmp + "U ";
                }
                else
                {
                    tmp = tmp + "D ";
                }
                if ((short)config.GetValue(3) != 0)
                {
                    tmp = tmp + "T ";
                }
                else
                {
                    tmp = tmp + "B ";
                }
                tmp = tmp + String.Format("{0}, {1}, {2}", config.GetValue(4), config.GetValue(5), config.GetValue(6)) + Environment.NewLine;
            }

            if (intValidJ != 0)
            {
                tmp = tmp + "JOINT = ";
                //5
                for (ii = 0; ii <= 8; ii++)
                {
                    tmp = tmp + joint.GetValue(ii) + " ";
                }
                tmp = tmp + Environment.NewLine;
            }

            return tmp;

        }


        private string mstrTask(int Index, string strProg, short intLine, short intState, string strParentProg)
        {
            string tmp = null;

            tmp = "TASK" + Index + " : ";
            tmp = tmp + " Prog=" + Strings.Chr(34) + strProg + Strings.Chr(34);
            tmp = tmp + " Line=" + intLine;
            tmp = tmp + " State=" + intState;
            tmp = tmp + " ParentProg=" + Strings.Chr(34) + strParentProg + Strings.Chr(34);

            return tmp + Environment.NewLine;
        }


        private string mstrAlarm(ref FRRJIf.DataAlarm objAlarm, int Count)
        {
            string tmp = null;
            short intID = 0;
            short intNumber = 0;
            short intCID = 0;
            short intCNumber = 0;
            short intSeverity = 0;
            short intY = 0;
            short intM = 0;
            short intD = 0;
            short intH = 0;
            short intMn = 0;
            short intS = 0;
            string strM1 = "";
            string strM2 = "";
            string strM3 = "";
            bool blnRes = false;

            blnRes = objAlarm.GetValue(Count, ref intID, ref intNumber, ref intCID, ref intCNumber, ref intSeverity, ref intY, ref intM, ref intD, ref intH,
            ref intMn, ref intS, ref strM1, ref strM2, ref strM3, encode);
            tmp = "-- Alarm " + Count + " --" + Environment.NewLine;
            if (blnRes == false)
            {
                tmp = tmp + "Error" + Environment.NewLine;
                return tmp;
            }
            tmp = tmp + intID + ", " + intNumber + ", " + intCID + ", " + intCNumber + ", " + intSeverity + Environment.NewLine;
            tmp = tmp + intY + "/" + intM + "/" + intD + ", " + intH + ":" + intMn + ":" + intS + Environment.NewLine;
            if (!string.IsNullOrEmpty(strM1))
                tmp = tmp + strM1 + Environment.NewLine;
            if (!string.IsNullOrEmpty(strM2))
                tmp = tmp + strM2 + Environment.NewLine;
            if (!string.IsNullOrEmpty(strM3))
                tmp = tmp + strM3 + Environment.NewLine;

            return tmp;
        }

        static string mstrComment(string CommentType, FRRJIf.DataString objComment, System.Text.Encoding encode)
        {
            string tmp = CommentType + Environment.NewLine;
            string strComment = "";

            for (int i = objComment.GetStartIndex(); i <= objComment.GetEndIndex(); i++)
            {
                tmp += "[" + i + "] ";
                if (objComment.GetValue(i, ref strComment, encode) == true)
                {
                    tmp += (strComment + Environment.NewLine);
                }
                else
                {
                    tmp += ("Error" + Environment.NewLine);
                }
            }

            return tmp;
        }


        public void mnuAbout_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            string strTmp = null;
            bool blnCreated = false;

            //check
            if (mobjCore == null)
            {
                mobjCore = new FRRJIf.Core(encode);
                blnCreated = true;
            }

            {
                Microsoft.VisualBasic.ApplicationServices.ApplicationBase app = new Microsoft.VisualBasic.ApplicationServices.ApplicationBase();

                strTmp = app.Info.Title + " V" + app.Info.Version.Major + "." + app.Info.Version.Minor + "." + app.Info.Version.Revision + Environment.NewLine + Environment.NewLine;
                //strTmp = strTmp + "FRRJIF Protect Available = " + mobjCore.ProtectAvailable + "\r\n";
                //strTmp = strTmp + "FRRJIF Protect Trial Remain Days = " + mobjCore.ProtectTrialRemainDays + "\r\n";
                //strTmp = strTmp + "FRRJIF Protect Status = " + mobjCore.ProtectStatus + "\r\n";
                //strTmp = strTmp + "FRRJIF Protect Error Number = " + mobjCore.ProtectErrorNumber + "\r\n";
            }

            MessageBox.Show(strTmp);

            //if created here, clear it
            if (blnCreated == true)
            {
                mobjCore = null;
            }

        }

        public void mnuExit_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            this.Close();
            System.Environment.Exit(0);
        }


        public void mnuTimeOut_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            int lngTmp = 0;
            string strTmp = null;

            if (mobjCore == null) {
                MessageBox.Show("Not connected");
                return;
            }

            lngTmp = mobjCore.get_TimeOutValue();
            strTmp = Interaction.InputBox("Please input time out value (msec)", "frrjiftest", Convert.ToString(lngTmp), 0, 0);
            lngTmp = Convert.ToInt32(strTmp);
            mobjCore.set_TimeOutValue(lngTmp);

            //check value
            if (mobjCore.get_TimeOutValue() != lngTmp)
            {
                MessageBox.Show("Invalid value");
                return;
            }

            //save it
            Interaction.SaveSetting(cnstApp, cnstSection, "TimeOut", Convert.ToString(lngTmp));

        }

        private void timLoop_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (chkLoop.Checked)
            {
                cmdRefresh.PerformClick();
            }
        }

        private void msubSetTestControls(bool blnEnabled)
        {

            chkLoop.Enabled = blnEnabled;
            cmdRefresh.Enabled = blnEnabled;
            cmdRefresh2.Enabled = blnEnabled;
            cmdSetNumReg.Enabled = blnEnabled;
            cmdSetPosReg.Enabled = blnEnabled;
            cmdSetPosRegX.Enabled = blnEnabled;
            _cmdSetSDO_0.Enabled = blnEnabled;
            _cmdSetSDO_1.Enabled = blnEnabled;
            _cmdSetSDO_2.Enabled = blnEnabled;
            cmdSetSDI.Enabled = blnEnabled;
            cmdSetRDO.Enabled = blnEnabled;
            cmdSetRDI.Enabled = blnEnabled;
            _cmdSetGO_0.Enabled = blnEnabled;
            _cmdSetGO_1.Enabled = blnEnabled;
            cmdSetGI.Enabled = blnEnabled;
            cmdWriteSysVar.Enabled = blnEnabled;
            cmdSetPosRegX2.Enabled = blnEnabled;
            cmdSetPosRegMG.Enabled = blnEnabled;
            cmdSetStrReg.Enabled = blnEnabled;
            button1.Enabled= blnEnabled;
            button2.Enabled = blnEnabled;
            button3.Enabled = blnEnabled;
            button4.Enabled = blnEnabled;
            button5.Enabled = blnEnabled;
            button6.Enabled = blnEnabled;
        }

        private void msubConnected()
        {

            txtResult.Text = "Connect OK to " + HostName;
            lblConnect.Text = txtResult.Text;
            this.Text = HostName + " - FRRJIf Test";

            msubSetTestControls(true);
            cmdConnect.Text = "Disconnect";

            timLoop.Enabled = true;
        }

        private void msubDisconnected()
        {

            //disabled continous
            timLoop.Enabled = false;

            MessageBox.Show("Connect error");

            txtResult.Text = "Connect Failed to " + HostName;
            lblConnect.Text = txtResult.Text;
            this.Text = HostName + " - FRRJIf Test";

            msubClearVars();

            msubSetTestControls(false);
            cmdConnect.Text = "Connect";

        }

        private void msubDisconnected2()
        {

            //disabled continous
            timLoop.Enabled = false;

            txtResult.Text = "Disconnect to " + HostName;
            // & " (" & mobjCore.ProtectStatus & ")"
            lblConnect.Text = txtResult.Text;
            this.Text = "FRRJIf Test";

            msubClearVars();

            msubSetTestControls(false);
            cmdConnect.Text = "Connect";

        }

        private void msubClearVars()
        {

            mobjCore.Disconnect();

            mobjCore = null;
            mobjDataTable = null;
            mobjCurPos = null;
            mobjCurPos2 = null;
            mobjCurPos3 = null;
            mobjCurPos4 = null;
            mobjCurPos5 = null;
            mobjTask = null;
            mobjTaskIgnoreMacro = null;
            mobjTaskIgnoreKarel = null;
            mobjTaskIgnoreMacroKarel = null;
            mobjPosReg = null;
            mobjPosReg2 = null;
            mobjPosReg3 = null;
            mobjPosReg4 = null;
            mobjPosReg5 = null;
            mobjPosRegXyzwpr = null;
            mobjPosRegMG = null;
            mobjSysVarInt = null;
            mobjSysVarReal = null;
            mobjSysVarReal2 = null;
            mobjSysVarString = null;
            mobjSysVarString2 = null;
            mobjSysVarPos = null;
            for (int ii = mobjSysVarIntArray.GetLowerBound(0); ii <= mobjSysVarIntArray.GetUpperBound(0); ii++)
            {
                mobjSysVarIntArray[ii] = null;
            }
            mobjNumReg = null;
            mobjNumReg2 = null;
            mobjAlarm = null;
            mobjAlarmCurrent = null;
            mobjAlarmPasswd = null;
            mobjVarString = null;
            mobjStrReg = null;
            mobjStrRegComment = null;
            mobjDIComment = null;
            mobjDOComment = null;
            mobjRIComment = null;
            mobjROComment = null;
            mobjUIComment = null;
            mobjUOComment = null;
            mobjSIComment = null;
            mobjSOComment = null;
            mobjWIComment = null;
            mobjWOComment = null;
            mobjWSIComment = null;
            mobjAIComment = null;
            mobjAOComment = null;
            mobjGIComment = null;
            mobjGOComment = null;
            mobjDataTable2 = null;
            mobjNumReg3 = null;
        }

        private void cmdSetStrReg_Click(object sender, EventArgs e)
        {
            int intRand = 0;
            int ii = 0;
            string strTmp;
            bool blnResult;

            mobjDataTable.modfrif.gfdblGetTickCountExStart();
            intRand = rnd.Next(1, 10);

            for (ii = mobjStrReg.GetStartIndex(); ii <= mobjStrReg.GetEndIndex(); ii++)
            {
                strTmp =string.Format("str{0}", (ii + intRand));
                blnResult = mobjStrReg.SetValue(ii, strTmp);
                System.Diagnostics.Debug.Assert((blnResult), "");
            }

            //Need to call Update to send data.
            blnResult = mobjStrReg.Update();
            System.Diagnostics.Debug.Assert((blnResult), "");

            Debug.Print(String.Format("Time {0} ms", Convert.ToInt16(mobjDataTable.modfrif.gfdblGetTickCountExEnd())));
            cmdRefresh.PerformClick();
        }

        private void cmdRefresh2_Click(object sender, EventArgs e)
        {
            string strTmp = null;
            bool blnDT = false;
            object vntValue = null;
            int intRand = 0;
            int ii = 0;
            int[] intValues = new int[101];
            float[] sngValues = new float[101];

            intRand = rnd.Next(1, 10);
            {
                for (ii = 0; ii <= mobjNumReg3.GetEndIndex() - mobjNumReg3.GetStartIndex(); ii++)
                {
                    intValues[ii] = (ii + 1) * intRand;
                }
                if (mobjNumReg3.SetValues(mobjNumReg3.GetStartIndex(), intValues, mobjNumReg3.GetEndIndex() - mobjNumReg3.GetStartIndex() + 1) == false)
                {
                    MessageBox.Show("SetNumReg Int Error");
                }
            }

            //check
            if (mobjCore == null)
            {
                return;
            }

            cmdRefresh2.Enabled = false;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            mobjDataTable.modfrif.gfdblGetTickCountExStart();

            //Refresh data table
            blnDT = mobjDataTable2.Refresh();
            if (blnDT == false)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                msubDisconnected();
                return;
            }

            strTmp = "Time = " + Convert.ToInt16(mobjDataTable.modfrif.gfdblGetTickCountExEnd()) + "(msec)" + Environment.NewLine;

            strTmp = strTmp + "--- NumReg3 ---" + Environment.NewLine;
            {
                for (ii = mobjNumReg3.GetStartIndex(); ii <= mobjNumReg3.GetEndIndex(); ii++)
                {
                    if (mobjNumReg3.GetValue(ii, ref vntValue) == true)
                    {
                        strTmp = strTmp + "R[" + ii + "] = " + vntValue + Environment.NewLine;
                    }
                    else
                    {
                        strTmp = strTmp + "R[" + ii + "] : Error!!!" + Environment.NewLine;
                    }
                }
            }
            strTmp = strTmp + "--- SysVar ---" + Environment.NewLine;
            for (ii = mobjSysVarIntArray.GetLowerBound(0); ii <= mobjSysVarIntArray.GetUpperBound(0); ii++)
            {
                if (mobjSysVarIntArray[ii].GetValue(ref vntValue) == true)
                {
                    strTmp = strTmp + mobjSysVarIntArray[ii].SysVarName() + " = " + vntValue + Environment.NewLine;
                }
                else
                {
                    strTmp = strTmp + mobjSysVarIntArray[ii].SysVarName() + " : Error!!!" + Environment.NewLine;
                }
            }

            txtResult.Text = strTmp;

            cmdRefresh2.Enabled = true;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void cmdSetPosRegMG_Click(object sender, EventArgs e)
        {
            int intRand = 0;
            int ii = 0;
            int jj = 0;
            Array sngArray = new float[6];
            Array sngJoint = new float[6];
            Array intConfig = new short[7];
            bool blnRes = false;

            mobjDataTable.modfrif.gfdblGetTickCountExStart();
            intRand = rnd.Next(1, 10);
            {
                for (ii = mobjPosRegMG.GetStartIndex(); ii <= mobjPosRegMG.GetEndIndex(); ii++)
                {
                    for (jj = sngArray.GetLowerBound(0); jj <= sngArray.GetUpperBound(0); jj++)
                    {
                        sngArray.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    }
                    intConfig.SetValue((short)ii, 4);
                    intConfig.SetValue((short)ii, 5);
                    intConfig.SetValue((short)ii, 6);
                    blnRes = mobjPosRegMG.SetValueXyzwpr(ii, 1, ref sngArray, ref intConfig);
                    if (blnRes == false)
                    {
                        MessageBox.Show("Error mobjPosRegMG.SetValueXyzwpr");
                    }
                    for (jj = sngJoint.GetLowerBound(0); jj <= sngJoint.GetUpperBound(0); jj++)
                    {
                        sngJoint.SetValue((float)(11.11 * (jj + 1) * intRand * ii), jj);
                    }
                    blnRes = mobjPosRegMG.SetValueJoint(ii, 2, ref sngJoint);
                    if (blnRes == false)
                    {
                        MessageBox.Show("Error mobjPosRegMG.SetValueJoint");
                    }
                }
                blnRes = mobjPosRegMG.Update();
                if (blnRes == false)
                {
                    MessageBox.Show("Error mobjPosRegMG.Update");
                }
            }
            Debug.Print(string.Format("Time {0} ms", Convert.ToInt16(mobjDataTable.modfrif.gfdblGetTickCountExEnd())));
            cmdRefresh.PerformClick();

        }

        // Websocket
        private async Task ReceiveMessagesAsync()
        {
            System.Windows.Forms.TextBox txtLog = textBox10;

            System.Windows.Forms.TextBox x = textBox8;
            System.Windows.Forms.TextBox y = textBox4;
            System.Windows.Forms.TextBox z = textBox5;
            System.Windows.Forms.TextBox w = textBox6;
            System.Windows.Forms.TextBox p = textBox7;
            System.Windows.Forms.TextBox r = textBox9;

            byte[] buffer = new byte[1024];
            while (_clientSocket?.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    txtLog.AppendText($"\nReceived: {message}\n");
                    // How do I get the data from the message?
                    ReceivedData data = JsonConvert.DeserializeObject<ReceivedData>(message);
                    UpdateTextBoxes(data);

                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    txtLog.AppendText("Server closed the connection.\n");
                    await _clientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                }
            }
        }

        public void UpdateTextBoxes(ReceivedData data)
        {
            if (textBox8.InvokeRequired || textBox4.InvokeRequired || textBox5.InvokeRequired
                || textBox6.InvokeRequired || textBox7.InvokeRequired || textBox9.InvokeRequired)
            {
                textBox8.Invoke(new Action(() =>
                {
                    textBox8.Text = data.x.ToString();
                    textBox4.Text = data.y.ToString();
                    textBox5.Text = data.z.ToString();
                    textBox6.Text = data.w.ToString();
                    textBox7.Text = data.p.ToString();
                    textBox9.Text = data.r.ToString();
                }));
            }
            else
            {
                textBox8.Text = data.x.ToString();
                textBox4.Text = data.y.ToString();
                textBox5.Text = data.z.ToString();
                textBox6.Text = data.w.ToString();
                textBox7.Text = data.p.ToString();
                textBox9.Text = data.r.ToString();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Array intVal = new short[10];
            int ii = 0;
            bool blnRes = false;

            static_cmdSetRDO_Click_lngCount = static_cmdSetRDO_Click_lngCount + 1;
            if (static_cmdSetRDO_Click_lngCount % 2 == 1)
            {
                for (ii = 2; ii <= 2; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteRDO(1, intVal, 8);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Array intVal = new short[10];
            int ii = 0;
            bool blnRes = false;

            static_cmdSetRDO_Click_lngCount = static_cmdSetRDO_Click_lngCount + 1;
            if (static_cmdSetRDO_Click_lngCount % 2 == 1)
            {
                for (ii = 1; ii <= 1; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteRDO(1, intVal, 8);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();


        }

        private void button3_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            Array intVal = new short[100];
            int ii = 0;
            bool blnRes = false;
            short intStartIndex = 0;

            if (object.ReferenceEquals(eventSender, _cmdSetSDO_1))
            {
                intStartIndex = 10001;
            }
            else if (object.ReferenceEquals(eventSender, _cmdSetSDO_2))
            {
                intStartIndex = 11001;
            }
            else
            {
                intStartIndex = 1;
            }
            static_cmdSetSDO_Click_lngCount = static_cmdSetSDO_Click_lngCount + 1;
            if (static_cmdSetSDO_Click_lngCount % 2 == 1)
            {
                for (ii = 0; ii <= 0; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteSDO(intStartIndex, intVal, 100);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();
            cmdRefresh.PerformClick();
            cmdRefresh.PerformClick();
            cmdRefresh.PerformClick();
            cmdRefresh.PerformClick();
            cmdRefresh.PerformClick();
        }

        private void button4_Click(System.Object eventSender, System.EventArgs eventArgse)
        {
            Array intVal = new short[100];
            int ii = 0;
            bool blnRes = false;
            short intStartIndex = 0;

            if (object.ReferenceEquals(eventSender, _cmdSetSDO_1))
            {
                intStartIndex = 10001;
            }
            else if (object.ReferenceEquals(eventSender, _cmdSetSDO_2))
            {
                intStartIndex = 11001;
            }
            else
            {
                intStartIndex = 1;
            }
            static_cmdSetSDO_Click_lngCount = static_cmdSetSDO_Click_lngCount + 1;
            if (static_cmdSetSDO_Click_lngCount % 2 == 1)
            {
                for (ii = 1; ii <= 1; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteSDO(intStartIndex, intVal, 100);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Array intVal = new short[10];
            int ii = 0;
            bool blnRes = false;

            static_cmdSetSDI_Click_lngCount = static_cmdSetSDI_Click_lngCount + 1;
            if (static_cmdSetSDI_Click_lngCount % 2 == 1)
            {
                for (ii = 0; ii <= 0; ii++)
                {
                    intVal.SetValue((short)1, ii);
                }
            }
            blnRes = mobjCore.WriteSDI(1, intVal, 10);
            if (blnRes == false)
            {
                MessageBox.Show("Error");
            }
            cmdRefresh.PerformClick();

        }

        private void button6_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            //strTmp = strTmp + "--- DataPosRegMG ---" + Environment.NewLine;
            for (int i = mobjPosRegMG.GetStartIndex(); i <= mobjPosRegMG.GetEndIndex(); i++)
            {
                Array xyzwprMG = (new float[9]);
                Array configMG = (new short[7]);
                bool res = mobjPosRegMG.GetValueXyzwpr(i, 1, xyzwprMG, configMG);
                label1.Text = "index=" + i.ToString();
                label2.Text = " Joint 1=" + xyzwprMG.GetValue(0).ToString();
                label3.Text = " Joint 2=" + xyzwprMG.GetValue(1).ToString();
                label4.Text = " Joint 3=" + xyzwprMG.GetValue(2).ToString();
                label5.Text = " Joint 4=" + xyzwprMG.GetValue(3).ToString();
                label6.Text = " Joint 5=" + xyzwprMG.GetValue(4).ToString();
                label7.Text = " Joint 6=" + xyzwprMG.GetValue(5).ToString();
                label8.Text = " Joint 7=" + xyzwprMG.GetValue(6).ToString();
                label9.Text = " Joint 8=" + xyzwprMG.GetValue(7).ToString();
            }

        }

        private void button8_Click(System.Object eventSender, System.EventArgs eventArgse)
        {
            // Define position data for PR[99]
            float[] xyzwpr = new float[9]; // Array for X, Y, Z, W, P, R, E1, E2, E3
            short[] config = new short[7]; // Configuration data (F, U, T, and 3 additional config values)

            // Example position data
            xyzwpr[0] = float.Parse(textBox8.Text); // X
            xyzwpr[1] = float.Parse(textBox4.Text); // Y
            xyzwpr[2] = float.Parse(textBox5.Text); // Z
            xyzwpr[3] = float.Parse(textBox6.Text);   // W
            xyzwpr[4] = float.Parse(textBox7.Text);  // P
            xyzwpr[5] = float.Parse(textBox9.Text); // R
                                                    // E1, E2, E3 remain 0 for default 6-axis robots

            // Example configuration: F, U, T, 0, 0, 0, 0
            config[0] = 0 ;
            config[1] = 1;
            config[2] = 1;
            config[3] = 1;
            config[4] = 0;
            config[5] = 0;
            config[6] = 0;

            // Optional user frame and tool frame numbers
            short userFrame = 0;
            short userTool = 0;

            try
            {
                // Explicitly cast float[] and short[] to System.Array
                System.Array xyzwprArray = xyzwpr;
                System.Array configArray = config;

                // Write the position data to PR[2]
                bool result = mobjPosReg.SetValueXyzwpr(positionRegister, ref xyzwprArray, ref configArray, userFrame, userTool);

                if (result)
                {
                    MessageBox.Show("Position data successfully sent to PR");
                }
                else
                {
                    MessageBox.Show("Failed to send position data to PR");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            cmdRefresh.PerformClick();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(System.Object eventSender, System.EventArgs eventArgse)
        {
            float[] xyzwpr = new float[9];
            short[] config = new short[7];

            short userFrame = 1;
            short userTool = 1;
            short validC = 0;

            try
            {
                System.Array xyzwprArray = xyzwpr;
                System.Array configArray = config;

                bool result = mobjPosReg.GetValueXyzwpr(99, ref xyzwprArray, ref configArray, ref userFrame, ref userTool, ref validC);

                if (result)
                {
                    string positionData = $"Position: X={xyzwpr[0]}, Y={xyzwpr[1]}, Z={xyzwpr[2]}, " +
                                  $"W={xyzwpr[3]}, P={xyzwpr[4]}, R={xyzwpr[5]}";

                    textBox8.Text = xyzwpr[0].ToString();
                    textBox4.Text = xyzwpr[1].ToString();
                    textBox5.Text = xyzwpr[2].ToString();
                    textBox6.Text = xyzwpr[3].ToString();
                    textBox7.Text = xyzwpr[4].ToString();
                    textBox9.Text = xyzwpr[5].ToString();


                    string configData = $"Configuration: F={config[0]}, U={config[1]}, T={config[2]}";
                    string validCStatus = validC == 0 ? "Invalid" : "Valid";
                    MessageBox.Show($"Successfully retrieved data from PR[99].\n{positionData}\n{configData}");

                }
                else
                {
                    MessageBox.Show("Failed to retrieve data from PR[99]");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            cmdRefresh.PerformClick();

        }

        private void label11_Click_1(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblConnect_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            positionRegister = Convert.ToInt32(comboBox2.SelectedItem);
            cmdRefresh.PerformClick();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click_2(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private async void button12_Click(object sender, EventArgs e)
        {
            string txtIPAddress = textBox1.Text;
            string txtPort = textBox3.Text;

            try
            {
                await WebSocketServer.StartServerAsync(txtIPAddress, txtPort);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            if (_clientSocket == null || _clientSocket.State != WebSocketState.Open)
            {
                MessageBox.Show("Client is not connected to the server.");
                return;
            }

            // Pls help
            var data = new
            {
                x = 12.2,
                y = 17.1,
                z = 91.0,
                w = 0.0,
                p = 2.0,
                r = 4.0
            };

            string message = JsonConvert.SerializeObject(data);

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _clientSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

            MessageBox.Show("JSON message sent to the server.");
        }


        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button14_Click(object sender, EventArgs e)
        {
            string txtIPAddress = textBox1.Text;
            string txtPort = textBox3.Text;
            System.Windows.Forms.TextBox txtLog = textBox10;

            string serverUrl = $"ws://{txtIPAddress}:{txtPort}/";

            try
            {
                _clientSocket = new ClientWebSocket();
                await _clientSocket.ConnectAsync(new Uri(serverUrl), CancellationToken.None);
                txtLog.AppendText("Connected to server.\n");
                await ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"Error: {ex.Message}\n");
            }
        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {

        }
    }

}



