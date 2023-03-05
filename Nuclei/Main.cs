using System;
using System.Windows.Forms;
using GTA;
using LemonUI;
using LemonUI.Menus;

namespace Nuclei;

public class Main : Script
{
    private readonly NativeMenu _menu = new("Nuclei", "Main Menu");
    private readonly ObjectPool _pool = new();

    public Main()
    {
        _pool.Add(_menu);
        KeyDown += OnKeyDown;
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        _pool.Process();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5) _menu.Visible = !_menu.Visible;
    }
}