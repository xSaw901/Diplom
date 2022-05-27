using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;

namespace Diplom
{
    public partial class PostavkaForm : Form
    {
        DiplomEntities db= new DiplomEntities();
        string path = @"C:\Users\kinri\Desktop\Log.txt";
        public PostavkaForm()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int post_tov_id = (int)comboBox1.SelectedValue;
            Postavka post = new Postavka
            {
                id_tov = post_tov_id,
                Data = dateTimePicker1.Value,
                Kol_vo = (int)numericUpDown1.Value
            };
            var tov = db.Tovar.FirstOrDefault(x=> x.id==post_tov_id);
            tov.Kol_vo=post.Kol_vo+ tov.Kol_vo;
            db.Postavka.Add(post);

            db.SaveChanges();
            File.AppendAllText(path, $"\n {DateTime.Now}------Работник добавил постаку товара {post.Tovar.Name} в размере {post.Kol_vo} штук ");
        }

        private void PostavkaForm_Load(object sender, EventArgs e)
        {
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = db.Tovar.Select(x=> new { x.id, x.Name}).ToList();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var nameTov = Interaction.InputBox("Введите название товара , цену и номер склада через запятую\n Например: Шприц,350,2", "Добавление товара", null);
            nameTov.Trim();
            string[] mas = new string[3];
                mas= nameTov.Split(',');
            try
            {
                if (!String.IsNullOrEmpty(nameTov))
                {
                    if (mas.Length > 3 || mas.Length < 3)
                    {
                        MessageBox.Show("Неверно введено");
                    }
                    else
                    {
                        if (db.Sklad.Count() < Convert.ToInt32(mas[2]))
                            MessageBox.Show("Такого склада нет");
                        else
                        {
                            if (Convert.ToInt32(mas[1]) < 0)
                                MessageBox.Show("Некорректная цена");
                            else
                            {
                                Tovar tov = new Tovar
                                {
                                    Name = mas[0],
                                    Kol_vo = 0,
                                    Price = Convert.ToInt32(mas[1]),
                                    id_Sklada = Convert.ToInt32(mas[2])
                                };
                                db.Tovar.Add(tov);
                                db.SaveChanges();
                                File.AppendAllText(path, $"\n {DateTime.Now}------На склад добавлен товар {tov.Name}");
                                comboBox1.DataSource = db.Tovar.Select(x => new { x.id, x.Name }).ToList();
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Вы что-то сделали не так, проверьте правильность написанного");
            }
            
        }
    }
}
