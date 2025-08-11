using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoAppdater.Window
{
    public class CopyData
    {
        [JsonConstructor]
        public CopyData()
        {

        }
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct COPYDATASTRUCT32
    {
        public UInt32 dwData;
        public UInt32 cbData;
        public IntPtr lpData;
    }
    internal class MainForm : Form
    {
        public delegate void GetCopyMessageDelegate(CopyData data);
        public static event GetCopyMessageDelegate? GetCopyMessageEvent;
        const uint WM_COPYDATA = 0x004A;
        const string MainProcess_MainWindowName = "MainWindow";
        internal MainForm()
        {
            this.Text = MainProcess_MainWindowName;
            this.Visible = false;
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_COPYDATA)
            {
                COPYDATASTRUCT32 cd = Marshal.PtrToStructure<COPYDATASTRUCT32>(m.LParam);
                string? js = Marshal.PtrToStringAnsi(cd.lpData);
                if (js == null) return;
                CopyData? data = JsonSerializer.Deserialize<CopyData>(js);
                if (data == null) return;
                GetCopyMessageEvent!.Invoke(data);
            }
        }
    }
}