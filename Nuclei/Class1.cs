using System.Windows.Forms;
using GTA;

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
            if (e.KeyCode == Keys.T)
            {
                Game.Player.Character.Health = Game.Player.Character.MaxHealth;
                Game.Player.Character.Armor = Game.Player.MaxArmor;
            }
        }
    }
}