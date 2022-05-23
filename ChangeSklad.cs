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
    public partial class ChangeSklad : Form
    {
        DiplomEntities db= new DiplomEntities();
        public ChangeSklad()
        {
            InitializeComponent();
        }
        public int skladID;
        private void ChangeSklad_Load(object sender, EventArgs e)
        {
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = db.Sklad.Select(x=> new { x.Name,x.id}).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tovar = db.Tovar.FirstOrDefault(x=> x.id== skladID);
            tovar.id_Sklada = (int)comboBox1.SelectedValue;
            db.SaveChanges();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
