using System;
using System.Windows.Forms;
using LevelManager.Properties;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace LevelManager
{
    public partial class LevelManager : Form
    {
        public LevelManager()
        {
            InitializeComponent();
        }

        //==//==//==// GENERATE TEMPLATE //==//==//==//
        private void bGenerate_Click(object sender, EventArgs e)
        {
            string generatedTemplate = cbEdit.Checked ?
                Generator.GenerateTemplate((int)nudWidth.Value, (int)nudHeight.Value, true) : 
                Generator.GenerateTemplate((int)nudWidth.Value, (int)nudHeight.Value);

            lGenerateStatus.Text = "GENERATED";
            lGenerateStatus.Visible = true;
        }
        
        //==//==//==// GENERATE LEVEL //==//==//==//
        private void bLevelGenerate_Click(object sender, EventArgs e)
        {
            lGenerateLevelStatus.Text = $"{Generator.Title} GENERATED";
            lGenerateLevelStatus.Visible = true;
            bShow.Visible = true;

            string generatedLevel = Generator.GenerateLevel(tbName.Text, (int)nudLevelWidth.Value, (int) nudLevelHeight.Value,
                knightsAvailable.Checked, archersAvailable.Checked, ghostsAvailable.Checked, wizardsAvailable.Checked,
                crushersAvailable.Checked, spikeballsAvailable.Checked, blowtorchersAvailable.Checked,
                silverDoorAvailable.Checked, goldenDoorAvailable.Checked, teleportsAvailable.Checked,
                shopAvailable.Checked, spikeballsAvailable.Checked, trampolinesAvailable.Checked);
        }
        private void bShow_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Generator.GenerateLevel("test", (int)nudLevelWidth.Value, (int)nudLevelHeight.Value), "", MessageBoxButtons.OK);

            /*
            RenderWindow window;
            window = new RenderWindow(new VideoMode(),
                "Level View", Styles.Default);
            window.SetFramerateLimit(1);
            void OnClose(object sdr, EventArgs ea)
            {
                window.Close();
            }

            window.Closed += OnClose;
            window.SetVisible(true);

            RectangleShape shape = new RectangleShape(new Vector2f(20,20));
            shape.Position = new Vector2f(15,15);

            while (window.IsOpen)
            {
                window.Clear(SFML.Graphics.Color.Black);
                window.DispatchEvents();

                window.Draw(shape);
                window.Display();
            }
            */
        }
        //==//==//==// CLEANING //==//==//==//
        private void nudTemplateSize_ValueChange(object sender, EventArgs e)
        {
            lGenerateStatus.Visible = false;
        }
        private void nudLevelSize_ValueChange(object sender, EventArgs e)
        {
            lGenerateLevelStatus.Visible = false;
        }
        private void cbEdit_CheckedChanged(object sender, EventArgs e)
        {
            lGenerateStatus.Visible = false;
        }
        //==//==//==// SETTINGS //==//==//==//
        private void LoadSettings()
        {
            cbEdit.Checked = Settings.Default.Edit;
            tbName.Text = Settings.Default.Name;
            nudWidth.Value = Settings.Default.tempX;
            nudHeight.Value = Settings.Default.tempY;
            nudLevelWidth.Value = Settings.Default.levX;
            nudLevelHeight.Value = Settings.Default.levY;
        }
        private void SaveSettings()
        {
            Settings.Default.Edit = cbEdit.Checked;
            Settings.Default.Name = tbName.Text;
            Settings.Default.tempX = nudWidth.Value;
            Settings.Default.tempY = nudHeight.Value;
            Settings.Default.levX = nudLevelWidth.Value;
            Settings.Default.levY = nudLevelHeight.Value;
            Settings.Default.Save();
        }
        //==//==//==// OPEN/CLOSE EVENTS //==//==//==//
        private void LevelManager_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LevelManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
        }

        
    }
}
