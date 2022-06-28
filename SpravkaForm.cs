using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom
{
    public partial class SpravkaForm : Form
    {
        public SpravkaForm()
        {
            InitializeComponent();
        }
        public string SpravkaName;
        private void SpravkaForm_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = SpravkaName;
        }
    }
}
