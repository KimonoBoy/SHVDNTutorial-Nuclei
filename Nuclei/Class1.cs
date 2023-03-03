using System.Windows.Forms;
using GTA;
using GTA.UI;

namespace Nuclei
{
    public class Class1 : Script
    {
        public Class1()
        {
            KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.T) Notification.Show("Hello GTA V!");
        }
    }
}