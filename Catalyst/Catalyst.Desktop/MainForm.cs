using System;
using Eto.Forms;
using Eto.Drawing;
using Chroma;

namespace Catalyst
{
    public partial class MainForm : Form
    {


        private Chroma.Scene scene;

        private Color backgroundColor = Color.FromRgb(2238254);
        private Color listColor = Color.FromRgb(5857646);
        private Color primaryColor = Colors.Aquamarine;
        private Color fontPrimaryColor = Color.FromRgb(14146011);
        private ListBox layerList = new ListBox();
        private StackLayout layerButtons = new StackLayout();
        private Button addLayerButton = new Button();

        public MainForm()
        {
            Title = "Catalyst Editor 0.1";
            ClientSize = new Size(1270, 640);

            layerList.BackgroundColor = listColor;

            addLayerButton.Text = "Add Layer";
            layerButtons.Items.Add(addLayerButton);

            addLayerButton.Click += (o, i)=>
            {
                AddLayer();
            };

            var layout = new TableLayout
            {
                Rows =
                {
                    new TableRow(layerList),
                    new TableRow(layerButtons)
                },
            };


            Content = new StackLayout
            {
                Items =
                {
                    layout
                }
            };

            BackgroundColor = backgroundColor;

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



        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            layerList.Width = (int)(ClientSize.Width * 0.2);
            layerList.Height = (int)(ClientSize.Height * 0.95);
            addLayerButton.Width = (int)(ClientSize.Width * 0.2);
            addLayerButton.Height = (int)(ClientSize.Height * 0.05);

        }


        private void AddLayer()
        {
            scene.CreateLayer();
        }

        private void RefreshLayer()
        {
            layerList.Items.Clear();
            foreach (SceneLayer layer in scene.GetLayerList())
            {
                //layerList.Items.Add();
            }
        }


    }
}
