using System;
using System.Windows.Forms;
using GTA;

namespace Nuclei;

public class Main : Script
{
    private readonly MainMenu _mainMenu = new("Main Menu", "The main menu of Nuclei.");

    public Main()
    {
        KeyDown += OnKeyDown;
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        MenuBase.Pool.Process();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5) _mainMenu.Visible = !_mainMenu.Visible;
    }
}