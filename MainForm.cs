using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Microsoft.VisualBasic;

namespace Diplom
{
    public partial class MainForm : Form
    {
        DiplomEntities db = new DiplomEntities();
        public ZakazList zak1;
        public int str;
        //List<Class1> fse;
        //int i = 0;
        public MainForm()
        {
            Program.mainForm = this;
            InitializeComponent();
           

            tovarDataGridView.DataSource = db.Tovar.Select(x => new {id=x.id, Название = x.Name,Цена= x.Price,Количество = x.Kol_vo,Склад =x.Sklad.Name }).ToList();
            tovarDataGridView.Columns[0].Visible = false;
            
            zakazListDataGridView.ContextMenuStrip = contextMenuStrip2;
            tovarDataGridView.ContextMenuStrip = contextMenuStrip1;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            dgvPaint();
            //загрузка списка заказов, для работников выводятся все заказы, а для заказчиков только их заказы
            if (Program.f1.log == "Работник")
            {
                zakazListDataGridView.DataSource = db.ZakazList.Select(a => new { id = a.id, Название = a.Tovar.Name, Количество = a.Count, Статус = a.Status, Заказчик = a.Hospital.Name,Цена = a.Tovar.Price * a.Count }).ToList();
                zakazListDataGridView.Columns[0].Visible = false;
                dvgStatPaint();
            }
            else
            {
                zakazListDataGridView.DataSource = db.ZakazList.Where(x => x.id_zak == Program.f1.a).Select(x => new { id = x.id, Название = x.Tovar.Name, Количество = x.Count, Статус = x.Status, Заказчик = x.Hospital.Name, Цена = x.Tovar.Price * x.Count+"" }).ToList();
                zakazListDataGridView.Columns[0].Visible = false;
                dvgStatPaint();
            }

        }
        //Открыть форму добавления заказа
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddForm f2 = new AddForm();
            f2.Show();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostavkaForm postavka= new PostavkaForm();
            postavka.Show();

        }
        //Обновляет таблицу заказов
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.f1.log == "Работник")
            {
                zakazListDataGridView.DataSource = db.ZakazList.Select(a => new { id = a.id, Название = a.Tovar.Name, Количество = a.Count, Статус = a.Status, Заказчик = a.Hospital.Name, Цена= a.Tovar.Price*a.Count }).ToList();
                zakazListDataGridView.Columns[0].Visible = false;
                dvgStatPaint();
            }
            else
            {
                zakazListDataGridView.DataSource = db.ZakazList.Where(x => x.id_zak == Program.f1.a).Select(x => new { id = x.id, Название = x.Tovar.Name, Количество = x.Count, Статус = x.Status, Заказчик = x.Hospital.Name, Цена = x.Tovar.Price * x.Count }).ToList();
                zakazListDataGridView.Columns[0].Visible = false;
                dvgStatPaint();
            }
        }
        //Сортировка списка товаров
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
           
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    tovarDataGridView.DataSource = db.Tovar.OrderBy(x => x.Name).Select(x => new {id=x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
                    tovarDataGridView.Columns[0].Visible = false;
                    dgvPaint();
                    break;
                case 1:
                    tovarDataGridView.DataSource = db.Tovar.OrderByDescending(x => x.Name).Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
                    tovarDataGridView.Columns[0].Visible = false;
                    dgvPaint();
                    break;
                case 2:
                    tovarDataGridView.DataSource = db.Tovar.OrderBy(x => x.Price).Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
                    tovarDataGridView.Columns[0].Visible = false;
                    dgvPaint();
                    break;
                case 3:
                    tovarDataGridView.DataSource = db.Tovar.OrderByDescending(x => x.Price).Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
                    tovarDataGridView.Columns[0].Visible = false;
                    dgvPaint();
                    break;
                case 4:
                    tovarDataGridView.DataSource = db.Tovar.OrderBy(x => x.Kol_vo).Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
                    tovarDataGridView.Columns[0].Visible = false;
                    dgvPaint();
                    break;
                case 5:
                    tovarDataGridView.DataSource = db.Tovar.OrderByDescending(x => x.Kol_vo).Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
                    tovarDataGridView.Columns[0].Visible = false;
                    dgvPaint();
                    break;


            }
        }
        //Поиск по списку товаров
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tovarDataGridView.RowCount; i++)
                if (Convert.ToString(tovarDataGridView.Rows[i].Cells[1].Value).ToLower().Contains(textBox1.Text.ToLower())&& textBox1.Text!=null && textBox1.Text!="")
                    tovarDataGridView.Rows[i].Cells[1].Selected = true;
            else tovarDataGridView.Rows[i].Cells[1].Selected = false;
        }
        //Если заказчик попытается отредактировать свой заказ, то не получится, а если работник, то его перекинет на форму изменения заказа
        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.f1.log == "Заказчик")
                MessageBox.Show("Заказчик не может редактировать заказы");
            else
            if (zakazListDataGridView.SelectedRows.Count > 0 && zakazListDataGridView.SelectedRows.Count <= 1)
            {
                int t = zakazListDataGridView.Rows.IndexOf(zakazListDataGridView.SelectedRows[0]);
                str = Convert.ToInt32(zakazListDataGridView.Rows[t].Cells[0].Value);
                //zak1 = db.ZakazList.FirstOrDefault(x=> x.id==str);
                UpdateZakaz up = new UpdateZakaz();
                up.zak = str;
                up.Show();
            }
        }
        //Удаление выбранного заказа из списка заказа
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (zakazListDataGridView.SelectedRows.Count > 0 && zakazListDataGridView.SelectedRows.Count <= 1)
            {
                int t = zakazListDataGridView.Rows.IndexOf(zakazListDataGridView.SelectedRows[0]);
                int str = Convert.ToInt32(zakazListDataGridView.Rows[t].Cells[0].Value);
                ZakazList del = db.ZakazList.FirstOrDefault(b => b.id == str);

                if (del != null)
                {
                    db.ZakazList.Remove(del);
                    db.SaveChanges();

                    if (Program.f1.log == "Работник")
                    {
                        zakazListDataGridView.DataSource = db.ZakazList.Select(a => new { id = a.id, Название = a.Tovar.Name, Количество = a.Count, Статус = a.Status, Заказчик = a.Hospital.Name }).ToList();
                        zakazListDataGridView.Columns[0].Visible = false;
                    }
                    else
                    {
                        zakazListDataGridView.DataSource = db.ZakazList.Where(x => x.id_zak == Program.f1.a).Select(x => new { id = x.id, Название = x.Tovar.Name, Количество = x.Count, Статус = x.Status, Заказчик = x.Hospital.Name }).ToList();
                        zakazListDataGridView.Columns[0].Visible = false;
                    }
                }
            }
            else
                MessageBox.Show("Вы не выбрали строку, или выбрали больше одной");
        }
        //Метод для покраски сточек товаров по их колличеству
        void dgvPaint()
        {
            for (int i = 0; i < tovarDataGridView.Rows.Count; i++)
            {
                if (Convert.ToInt32(tovarDataGridView.Rows[i].Cells[3].Value) < 10000)
                    tovarDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                if (Convert.ToInt32(tovarDataGridView.Rows[i].Cells[3].Value) < 1000)
                    tovarDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            }
        }
        //Метод для покраски сточек заказов по их статусу
        void dvgStatPaint()
        {
            for (int i = 0; i < zakazListDataGridView.Rows.Count; i++)
            {
                if (Convert.ToString(zakazListDataGridView.Rows[i].Cells[3].Value) == "Одобрено")
                {
                    zakazListDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
                if (Convert.ToString(zakazListDataGridView.Rows[i].Cells[3].Value) == "Отменено")
                {
                    zakazListDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = new FileInfo(fileName: @"C:\Users\kinri\Desktop\Otchet\Otchet.xlsx");
            using (SaveFileDialog fileDialog = new SaveFileDialog() { Filter = "Excel Workbook| *.xlsx" })
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                { 
                var fileInfo= new FileInfo(fileDialog.FileName);
                    using (var pakage = new ExcelPackage(fileInfo))
                    {
                        var worksheet = pakage.Workbook.Worksheets.Add("Prodaja");
                        var prod = pakage.Workbook.Worksheets["Prodaja"];
                        worksheet.Cells["C2:C300"].Style.Numberformat.Format = "yyyy-mm-dd";
                        worksheet.Cells.LoadFromCollection(db.Prodaja.Select(x => new { x.id, Название = x.ZakazList.Tovar.Name, Дата_продажи = x.Data, Количество = x.ZakazList.Count, Цена_за_единицу = x.ZakazList.Tovar.Price, Общая_цена = x.ZakazList.Tovar.Price * x.ZakazList.Count }), true);
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        worksheet.Cells["A1:F1"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:F1"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:F1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:F1"].Style.Border.Top.Style = ExcelBorderStyle.Thick;

                        worksheet.Cells["C1"].AutoFilter = true;
                        //worksheet.Cells["D1"].Sort.Fil
                        worksheet.Cells["D2:D300"].Sort(0, true);
                        pakage.Save();
                    }
                }
            }

            
        }

        private void редактироватьНазваниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            
                if (tovarDataGridView.SelectedRows.Count > 0 && tovarDataGridView.SelectedRows.Count <= 1)
                {
                    int t = tovarDataGridView.Rows.IndexOf(tovarDataGridView.SelectedRows[0]);
                    int str = Convert.ToInt32(tovarDataGridView.Rows[t].Cells[0].Value);
                    Tovar del = db.Tovar.FirstOrDefault(b => b.id == str);
                    var s = Interaction.InputBox($"Редактировать название, {del.Name}");

                        if (!String.IsNullOrEmpty(s))
                        {
                            if (del != null)
                            {
                               del.Name = s;
                                db.SaveChanges();
                            }
                        }

                }
        }
    }
}
