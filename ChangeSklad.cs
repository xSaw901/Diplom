using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Diplom
{
    public partial class ChangeSklad : Form
    {
        DiplomEntities db= new DiplomEntities();
        string path = @"C:\Users\kinri\Desktop\Log.txt";
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
            File.AppendAllText(path, $"\n {DateTime.Now}------Работник изменил расположение товара {tovar.Name} на {(int)comboBox1.SelectedValue}");
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
