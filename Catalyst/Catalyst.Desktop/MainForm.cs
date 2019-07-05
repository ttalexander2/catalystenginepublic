using System;
using Eto.Forms;
using Eto.Drawing;
using Chroma;

namespace Catalyst
{
    public partial class MainForm : Form
    {

        private Color backgroundColor = Color.FromRgb(2238254);
        private Color fontPrimaryColor = Color.FromRgb(14146011);

        public MainForm()
        {
            Title = "Catalyst Editor 0.1";
            ClientSize = new Size(1270, 640);

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    new Label(){Text="Hello World", TextColor=fontPrimaryColor},
					// add more controls here
				}
            };

            this.BackgroundColor = backgroundColor;

            // create a few commands that can be used for the menu and toolbar
            var runGame = new Command { MenuText = "Run Game", ToolBarText = "Run Game" };
            runGame.Executed += (sender, e) =>
            {
                Chroma.Program.Main();
            };

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

            // create menu
            Menu = new MenuBar
            {
                Items =
                {
					// File submenu
					new ButtonMenuItem { Text = "&Run", Items = { runGame } },
					// new ButtonMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
					// new ButtonMenuItem { Text = "&View", Items = { /* commands/items */ } },
				},
                ApplicationItems =
                {
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand

                
            };






            
        }
    }
}
