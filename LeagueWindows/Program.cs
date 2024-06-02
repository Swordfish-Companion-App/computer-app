using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace connection_app
{
    public class Program
    {
        private NotifyIcon trayIcon;
        public static bool InMatch = false;

        [STAThread]
        public static int Main(string[] args)
        {
            // Initialize Tray Icon
            Program program = new Program();
            Application.Run();
            return 0;
        }

        public Program()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                ContextMenuStrip = new ContextMenuStrip()
            };
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, Exit);
            trayIcon.Visible = true;

            Task.Run(() => CheckUsernameLoop());
        }

        private async Task CheckUsernameLoop()
        {
            while (true)
            {
                var app = new GetUsername();
                var username = await app.GetUsernameAsync();

                if (username != null)
                {
                    if (!Program.InMatch)
                    {
                        Program.InMatch = true;
                        Logger.Log($"Username found: {username}. User is in a match.");
                    }
                    trayIcon.Text = $"{username} is in a match";
                }
                else
                {
                    if (Program.InMatch)
                    {
                        Program.InMatch = false;
                        Logger.Log("Likely not in a match or the username was not found.");
                    }
                    trayIcon.Text = "Likely not in a match or the username was not found.";
                }
                await Task.Delay(2000);
            }
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
