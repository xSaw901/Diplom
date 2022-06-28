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
    public partial class AddForm : Form
    {
        string path = @"C:\Users\kinri\Desktop\Log.txt";
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
            if ((int)numericUpDown1.Value > 100)
            {
                ZakazList zak = new ZakazList
                {
                    Tovar_id = (int)comboBox1.SelectedValue,
                    Count = Convert.ToInt32(numericUpDown1.Value),
                    id_zak = Program.f1.a,
                    Status = "Ожидание"

                };

                db.ZakazList.Add(zak);
                db.SaveChanges();
                File.AppendAllText(path, $"\n[{DateTime.Now}]------Заказчик оформил завку на товар {comboBox1.Text} в размере {zak.Count} штук");
                Close();
            }
            else MessageBox.Show("Невозможно заказать меньше 100 ед. товара");
        }
    }
}
