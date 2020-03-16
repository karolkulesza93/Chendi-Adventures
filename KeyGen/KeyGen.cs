using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyGen
{
    public partial class KeyGen : Form
    {
        public KeyGen()
        {
            InitializeComponent();
        }

        private void bGenerate_Click(object sender, EventArgs e)
        {
            var key = Generator.ToString(Generator.Generate()).Split('-');
            tb1.Text = key[0];
            tb2.Text = key[1];
            tb3.Text = key[2];
            tb4.Text = key[3];

            bGenerate.Text = "GENERATED!";
            bGenerate.ForeColor = Color.Green;

            bCopy.Text = "COPY";
            bCopy.ForeColor = Color.Black;
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            var str = new StringBuilder();
            str.Append(tb1.Text).Append("-");
            str.Append(tb2.Text).Append("-");
            str.Append(tb3.Text).Append("-");
            str.Append(tb4.Text);

            Clipboard.SetText(str.ToString());

            tb1.Clear();
            tb2.Clear();
            tb3.Clear();
            tb4.Clear();

            bCopy.Text = "COPIED!";
            bCopy.ForeColor = Color.Green;

            bGenerate.Text = "GENERATE";
            bGenerate.ForeColor = Color.Black;
        }
    }
}
