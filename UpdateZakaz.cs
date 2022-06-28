﻿using System;
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
    public partial class UpdateZakaz : Form
    {
        DiplomEntities db = new DiplomEntities();
        public int zak;
        ZakazList zak1;
        string path = @"C:\Users\kinri\Desktop\Log.txt";
        public UpdateZakaz()
        {
            InitializeComponent();
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = db.Tovar.Select(x => new { x.id, x.Name }).ToList();
        }
        public MainForm mainf = new MainForm();
        private void button1_Click(object sender, EventArgs e)
        {
            try 
            { 

            bool b = true;
            zak1.Tovar_id=(int)comboBox2.SelectedValue;
            zak1.Count = (int)numericUpDown1.Value;
                if (comboBox1.Text == "Одобрено")
                {
                    if (zak1.Count > db.Tovar.FirstOrDefault(x => x.id == zak1.Tovar_id).Kol_vo)
                    {
                        MessageBox.Show("Такого количества товара нет на складе");
                        b = false;
                    }
                    if (b)
                    {
                        zak1.Status = comboBox1.Text;

                        var tovar = db.Tovar.FirstOrDefault(x => x.id == zak1.Tovar_id);
                        tovar.Name = comboBox2.Text;
                        tovar.Kol_vo = tovar.Kol_vo - (int)numericUpDown1.Value;
                        db.SaveChanges();
                        Prodaja pr = new Prodaja
                        {
                            Data = DateTime.Now,
                            id_Zakaza = zak1.id
                        };
                        db.Prodaja.Add(pr);
                        db.SaveChanges();
                        File.AppendAllText(path, $"\n[{DateTime.Now}]------Был одобрен заказ для {db.Hospital.FirstOrDefault(x => x.id == zak1.id_zak).Name} на товар {zak1.Tovar.Name} в размере {(int)numericUpDown1.Value} штук");
                        Close();

                    }
                }
                else
                {
                    if (comboBox1.Text == "Отменено")
                    {
                        var tovar = db.Tovar.FirstOrDefault(x => x.id == zak1.Tovar_id);
                        tovar.Name = comboBox2.Text;
                        zak1.Status = comboBox1.Text;
                        db.SaveChanges();
                        Close();
                        File.AppendAllText(path, $"\n[{DateTime.Now}]------Был отменён заказ для {db.Hospital.FirstOrDefault(x => x.id == zak1.id_zak).Name} на товар {zak1.Tovar.Name} в размере {(int)numericUpDown1.Value} штук");
                    }
                    else
                    {
                        var tovar = db.Tovar.FirstOrDefault(x => x.id == zak1.Tovar_id);
                        tovar.Name = comboBox2.Text;
                        zak1.Status = comboBox1.Text;
                        Close();
                    }


                }
            
        }
            catch
            {
                MessageBox.Show("Вы что-то сломали");
            }

}

        private void UpdateZakaz_Load(object sender, EventArgs e)
        {
            zak1 = db.ZakazList.FirstOrDefault(x => x.id == zak);
            comboBox2.Text = zak1.Tovar.Name;
            numericUpDown1.Value = zak1.Count;
            comboBox1.Text=zak1.Status.ToString();
        }
    }
}
//
