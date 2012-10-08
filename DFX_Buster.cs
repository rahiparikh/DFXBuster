/*
 * 
 *  Project     : DFX Buster
 *  File        : DFX_Buster.cs
 *  Developer   : Rahil Parikh ( rahil@rahilparikh.me )
 *  Date        : Sept 17, 2012
 *  
 *  Copyright (c) 2012, Rahil Parikh
 *
 *  As long as you retain this notice and credit author
 *  for his work you can do whatever you want with this
 *  stuff. 
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 *  
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace DFX_Buster
{
    public partial class FrmDFX_Buster : Form
    {
        private bool _isBusterRunning;
        const int BM_CLICK = 0x00F5;

        public FrmDFX_Buster()
        {
            InitializeComponent();
        }

        private void FrmDFX_Buster_Load(object sender, EventArgs e)
        {
            //if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            //{
            //    Environment.Exit(1);
            //}

            string cmdArg = (Environment.GetCommandLineArgs().Length > 1 ? Environment.GetCommandLineArgs()[1].ToLower() : "");
            if (cmdArg.Equals("--quiet"))
            {
                this.WindowState = FormWindowState.Minimized;
            }
            this.startBuster(true);
        }

        private void FrmDFX_Buster_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            toggleNotifyIcon();
        }

        private void btnSS_Click(object sender, EventArgs e)
        {
            this.startBuster(!_isBusterRunning);
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            toggleNotifyIcon();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            toggleNotifyIcon();
        }

        private void toggleNotifyIcon()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = true;
                this.Show();
                this.Focus();
                notifyIcon.Visible = false;
            }
        }


        private void startBuster(bool doRun)
        {
            _isBusterRunning = doRun;

            if (doRun)
            {
                btnSS.Text = "Stop";
                lblStatus.Text = "DFX Buster is running";
                timer.Start();
            }
            else
            {
                btnSS.Text = "Start";
                lblStatus.Text = "DFX Buster is not running";
                timer.Stop();
            }
            notifyIcon.Text = lblStatus.Text;
        }


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        private struct WindowLong
        {
            public const int GWL_STYLE = (-16);
            public const int GWL_EXSTYLE = (-20);
        }

        private struct WindowStyle
        {
            public const UInt32 WS_POPUP = 0x80000000;
            public const UInt32 WS_VISIBLE = 0x10000000;
            public const UInt32 WS_CAPTION = 0xC00000;
        }

        private struct ExWindowStyle
        {
            public const UInt32 WS_EX_TOPMOST = 0x0008;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        private void timer_Tick(object sender, EventArgs e)
        {
            IntPtr windHdl = FindWindowByCaption(IntPtr.Zero, "DFX Message");
            if (windHdl != IntPtr.Zero)
            {
                UInt32 ws = GetWindowLong(windHdl, WindowLong.GWL_STYLE);
                if ((ws & (WindowStyle.WS_CAPTION | WindowStyle.WS_POPUP | WindowStyle.WS_VISIBLE)) == (WindowStyle.WS_CAPTION | WindowStyle.WS_POPUP | WindowStyle.WS_VISIBLE))
                {
                    IntPtr actWind = GetForegroundWindow();
                    UInt32 actWindStyle = GetWindowLong(actWind, WindowLong.GWL_STYLE);
                    UInt32 actWindExStyle = GetWindowLong(actWind, WindowLong.GWL_EXSTYLE);

                    IntPtr btnHdl = FindWindowEx(windHdl, IntPtr.Zero, "Button", "ok");
                    if (btnHdl != IntPtr.Zero)
                    {
                        Thread.Sleep(500);
                        SendMessage(btnHdl, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                        SetWindowLong(actWind, WindowLong.GWL_STYLE, actWindStyle);
                        SetWindowLong(actWind, WindowLong.GWL_EXSTYLE, actWindExStyle);
                        SetForegroundWindow(actWind);
                    }

                }
            }
        }

    }
}
