using CoreScanner;
using System;
using System.Diagnostics;

namespace AWPClient.Connection
{
    public class ScannerConnect
    {
        public const short CAMERA_TYPES_UVC = 14;
        public const short TOTAL_SCANNER_TYPES = CAMERA_TYPES_UVC;
        private static bool m_bSuccessOpen = false;
        private static int connectedScannerID = -1;
        public static short m_nNumberOfTypes;

        static CCoreScannerClass m_pCoreScanner;

        static short[] m_arScannerTypes;
        static bool[] m_arSelectedTypes;

        public const short SCANNER_TYPES_ALL = 1;
        public const short SCANNER_TYPES_SNAPI = 2;
        public const short SCANNER_TYPES_SSI = 3;
        public const short SCANNER_TYPES_RSM = 4;
        public const short SCANNER_TYPES_IMAGING = 5;
        public const short SCANNER_TYPES_IBMHID = 6;
        public const short SCANNER_TYPES_NIXMODB = 7;
        public const short SCANNER_TYPES_HIDKB = 8;
        public const short SCANNER_TYPES_IBMTT = 9;
        public const short SCALE_TYPES_SSI_BT = 11;

        const int STATUS_SUCCESS = 0;
        const int STATUS_FALSE = 1;
        private static void GetSelectedScannerTypes()
        {
            m_nNumberOfTypes = 0;
            for (int index = 0, k = 0; index < TOTAL_SCANNER_TYPES; index++)
            {
                if (m_arSelectedTypes[index])
                {
                    m_nNumberOfTypes++;
                    switch (index + 1)
                    {
                        case SCANNER_TYPES_ALL:
                            m_arScannerTypes[k++] = SCANNER_TYPES_ALL;
                            return;

                        case SCANNER_TYPES_SNAPI:
                            m_arScannerTypes[k++] = SCANNER_TYPES_SNAPI;
                            break;

                        case SCANNER_TYPES_SSI:
                            m_arScannerTypes[k++] = SCANNER_TYPES_SSI;
                            break;

                        case SCANNER_TYPES_NIXMODB:
                            m_arScannerTypes[k++] = SCANNER_TYPES_NIXMODB;
                            break;

                        case SCANNER_TYPES_RSM:
                            m_arScannerTypes[k++] = SCANNER_TYPES_RSM;
                            break;

                        case SCANNER_TYPES_IMAGING:
                            m_arScannerTypes[k++] = SCANNER_TYPES_IMAGING;
                            break;

                        case SCANNER_TYPES_IBMHID:
                            m_arScannerTypes[k++] = SCANNER_TYPES_IBMHID;
                            break;

                        case SCANNER_TYPES_HIDKB:
                            m_arScannerTypes[k++] = SCANNER_TYPES_HIDKB;
                            break;

                        case SCALE_TYPES_SSI_BT:
                            m_arScannerTypes[k++] = SCALE_TYPES_SSI_BT;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        public static void Connect()
        {
            if (m_bSuccessOpen)
            {
                return;
            }
            int appHandle = 0;
            GetSelectedScannerTypes();
            int status = STATUS_FALSE;

            try
            {
                m_pCoreScanner.Open(appHandle, m_arScannerTypes, m_nNumberOfTypes, out status);
                Debug.WriteLine(status.ToString() + " OPEN");
                if (STATUS_SUCCESS == status)
                {
                    m_bSuccessOpen = true;
                    short numberOfScanners; // Number of scanners expect to be used
                    int[] connectedScannerIDList = new int[255];
                    // List of scanner IDs to be returned
                    string outXML; //Scanner details output
                    int GSstatus;
                    m_pCoreScanner.GetScanners(out numberOfScanners, connectedScannerIDList, out outXML, out GSstatus);
                    foreach (int scannerID in connectedScannerIDList)
                    {
                        if (scannerID > 0)
                        {
                            connectedScannerID = scannerID;
                            Debug.WriteLine("FOUND SCANNER ID " + scannerID);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine("Error OPEN - " + exp.Message/*, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error*/);
            }
            finally
            {
                if (STATUS_SUCCESS == status)
                {
                    Debug.WriteLine("Connect successful");
                }
            }
        }
        public static bool IsMotoConnected()
        {
            return m_bSuccessOpen;
        }
        public static void Disconnect()
        {
            if (m_bSuccessOpen)
            {
                int appHandle = 0;
                int status = STATUS_FALSE;
                try
                {
                    m_pCoreScanner.Close(appHandle, out status);
                    Debug.WriteLine(status.ToString() + " CLOSE");
                    if (STATUS_SUCCESS == status)
                    {
                        m_bSuccessOpen = false;
                    }
                }
                catch (Exception exp)
                {
                    Debug.WriteLine("CLOSE Error - " + exp.Message/*, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error*/);
                }
            }
        }
    }
}
