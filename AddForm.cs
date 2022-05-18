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
    public partial class AddForm : Form
    {
        DiplomEntities db=new DiplomEntities();
        public AddForm()
        {
            InitializeComponent();
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = db.Tovar.Select(x=> new { x.id, x.Name}).ToList();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ZakazList zak = new ZakazList
            {
                Tovar_id = (int)comboBox1.SelectedValue,
                Count = Convert.ToInt32(numericUpDown1.Value),
                id_zak = Program.f1.a,
                Status="Ожидание"
            };
            
            db.ZakazList.Add(zak);
            db.SaveChanges();
        }
    }
}
