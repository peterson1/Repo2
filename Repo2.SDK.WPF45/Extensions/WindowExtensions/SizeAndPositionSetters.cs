using System.Windows;
using System.Windows.Forms;

namespace Repo2.SDK.WPF45.Extensions.WindowExtensions
{
    public static class SizeAndPositionSetters
    {
        public static void FitToScreenHeight(this Window win, int displayIndex = -1)
        {
            win.WindowState = WindowState.Maximized;
            var hgt = win.Height;
            win.WindowState = WindowState.Normal;
            win.Height = hgt;
        }


        public static void CenterHorizontally(this Window win, int displayIndex = -1)
        {
            var area = Display(displayIndex).WorkingArea;
            win.Left = area.X + ((area.Width / 2) - (win.Width / 2));
        }

        public static void CenterVertically(this Window win, int displayIndex = -1)
        {
            var area = Display(displayIndex).WorkingArea;
            win.Top = area.Y + ((area.Height / 2) - (win.Height / 2));
        }


        private static Screen Display(int displayIndex)
        {
            var all = Screen.AllScreens;
            return all[displayIndex == -1 ? all.Length - 1 : displayIndex];
        }
    }
}
