using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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
            // trayIcon.Icon = Icon.ExtractAssociatedIcon("LeagueWindows/assets/img/logos/trayicon.ico"); // FIX: Icon not found error
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
                        string[] parts = username.Split('#');
                        Program.InMatch = true;
                        var username_cut = parts[0];
                        var gametag = parts[1];

                        Logger.Log($"Username found: '{username_cut}', with game tag '{gametag}'");
                        trayIcon.Text = $"Username found: '{username_cut}', with game tag '{gametag}'";
                    }
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
