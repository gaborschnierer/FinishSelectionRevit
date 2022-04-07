using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace FinishSelectionRevit
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDlgItem(IntPtr hWnd, int nIDDlgItem);
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        // Click command
        const int WM_COMMAND = 0x0111;
        const int BN_CLICKED = 0;

        // The id of the button found using spy++
        const int ButtonId = 0x0000A4C1;

        static void Main(string[] args)
        {
            // Get every process whose name contains Revit, loop their main window handles
            foreach (var revitHandle in System.Diagnostics.Process.GetProcessesByName("Revit").Select(x => x.MainWindowHandle))
            {
                // Get the handle of the button
                IntPtr hWndButton = GetDlgItem(revitHandle, ButtonId);

                // the control id is passed as the high word of wParam and the low word of wParam is the notification code (BN_CLICKED)
                int wParam = (BN_CLICKED << 16) | (ButtonId & 0xffff);

                // Send the message to click the button
                SendMessage(revitHandle, WM_COMMAND, wParam, hWndButton);
            }
        }
    }
}
