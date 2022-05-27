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
    public partial class Form1 : Form
    {
        DiplomEntities db;
        public int a;
        public string log;
        MainForm f2 = new MainForm();
        string path = @"C:\Users\kinri\Desktop\Log.txt";
        public Form1()
        {
            db= new DiplomEntities();
            Program.f1 = this;
            InitializeComponent();
            

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
   

        private void button2_Click(object sender, EventArgs e)
        {
            var autoriz = db.LogZak.Where(b=> b.Logg == textBox3.Text && b.Pass== textBox4.Text).FirstOrDefault();
            if (autoriz != null)
            {
                int ii = autoriz.Hos_id;
                a = ii;
                log = "Заказчик";
                f2.Show();
                this.Hide();
                File.AppendAllText(path, $"\n {DateTime.Now}------Вошел заказчик c логном {autoriz.Logg}");
                //a = autoriz.Hos_id;
            }
            else
            {
                 MessageBox.Show("Неверный логин или пароль");
            }    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var autoriz = db.Log_Worker.Where(b => b.Logg == textBox5.Text && b.Pass == textBox6.Text).FirstOrDefault();
            if (autoriz != null)
            {
                log = "Работник";
                f2.Show();
                this.Hide();
                File.AppendAllText(path, $"\n {DateTime.Now}------Вошел работник c логном {autoriz.Logg}");
            }
            else
                MessageBox.Show("Неверный логин или пароль");
            
        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


    }
}








//Для кнопки удаления строки из dataGridView
//    int t;
//    if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows.Count <= 1)
//    {
//        t = dataGridView1.Rows.IndexOf(dataGridView1.SelectedRows[0]);
//        string str = Convert.ToString(dataGridView1.Rows[t].Cells[0].Value);
//        //MessageBox.Show(str);
//        LogZak del = db.LogZak.Where(b => b.Logg == str).FirstOrDefault();

//        if (del != null)
//        {
//            db.LogZak.Remove(del);
//            db.SaveChanges();
//            dataGridView1.DataSource = db.LogZak.Select(x => new { x.Logg, x.Pass }).ToList();
//        }
//    }
//    else
//        MessageBox.Show("Вы не выбрали строку, или выбрали больше одной");