using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            string generatedLevel = Generator.GenerateLevel(tbName.Text, (int)nudLevelWidth.Value, (int) nudLevelHeight.Value,
                knightsAvailable.Checked, archersAvailable.Checked, ghostsAvailable.Checked, wizardsAvailable.Checked,
                crushersAvailable.Checked, spikeballsAvailable.Checked, blowtorchersAvailable.Checked,
                silverDoorAvailable.Checked, goldenDoorAvailable.Checked, teleportsAvailable.Checked,
                shopAvailable.Checked, spikeballsAvailable.Checked, trampolinesAvailable.Checked);
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
    }
}
