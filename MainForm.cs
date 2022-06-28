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
        string path = @"C:\Users\kinri\Desktop\Log.txt";
        public MainForm()
        {
            Program.mainForm = this;
            InitializeComponent();


            tovarDataGridView.DataSource = db.Tovar.Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
            tovarDataGridView.Columns[0].Visible = false;
            dgvPaint();
            zakazListDataGridView.ContextMenuStrip = contextMenuStrip2;
            
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "id";
            comboBox3.DataSource = db.Hospital.Select(x => new { x.Name, x.id }).ToList();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            dgvPaint();
            //загрузка списка заказов, для работников выводятся все заказы, а для заказчиков только их заказы
            if (Program.f1.log == "Работник")
            {
                zakazListDataGridView.DataSource = db.ZakazList.Select(a => new { id = a.id, Название = a.Tovar.Name, Количество = a.Count, Статус = a.Status, Заказчик = a.Hospital.Name, Цена = a.Tovar.Price * a.Count }).ToList();
                zakazListDataGridView.Columns[0].Visible = false;
                tovarDataGridView.ContextMenuStrip = contextMenuStrip1;
                dvgStatPaint();
                richTextBox1.Text=File.ReadAllText(path);
            }
            else
            {
                zakazListDataGridView.DataSource = db.ZakazList.Where(x => x.id_zak == Program.f1.a).Select(x => new { id = x.id, Название = x.Tovar.Name, Количество = x.Count, Статус = x.Status, Заказчик = x.Hospital.Name, Цена = x.Tovar.Price * x.Count + "" }).ToList();
                zakazListDataGridView.Columns[0].Visible = false;
                comboBox3.Enabled = false;
                button1.Enabled = false;
                tabPage3.Parent = null;
                tabPage4.Parent = null;
                contextMenuStrip2.Items[1].Enabled = false;
                contextMenuStrip2.Items[2].Enabled = false;
                dvgStatPaint();
            }
            dataGridView1.DataSource = db.Prodaja.Select(x => new { Название = x.ZakazList.Tovar.Name, Дата_продажи = x.Data, Количество = x.ZakazList.Count, Цена_за_единицу = x.ZakazList.Tovar.Price, Общая_цена = x.ZakazList.Tovar.Price * x.ZakazList.Count, Заказчик = x.ZakazList.Hospital.Name }).ToList();
            dataGridView2.DataSource = db.Postavka.Select(x => new { x.Tovar.Name, x.Kol_vo, x.Data }).ToList();
            //dataGridView2.Columns[0].Visible = false;
            //dataGridView1.Columns[0].Visible = false;

        }
        //Открыть форму добавления заказа
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.f1.log == "Работник")
                MessageBox.Show("Работнику нельзя добавлять заказы");
            else
            {
                AddForm f2 = new AddForm();
                f2.ShowDialog();
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostavkaForm postavka = new PostavkaForm();
            postavka.ShowDialog();

        }
        //Обновляет таблицу заказов
        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.f1.log == "Работник")
            {
                zakazListDataGridView.DataSource = db.ZakazList.Select(a => new { id = a.id, Название = a.Tovar.Name, Количество = a.Count, Статус = a.Status, Заказчик = a.Hospital.Name, Цена = a.Tovar.Price * a.Count }).ToList();
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
                    tovarDataGridView.DataSource = db.Tovar.OrderBy(x => x.Name).Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
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
                if (Convert.ToString(tovarDataGridView.Rows[i].Cells[1].Value).ToLower().Contains(textBox1.Text.ToLower()) && textBox1.Text != null && textBox1.Text != "")
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
                up.ShowDialog();
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
                    File.AppendAllText(path, $"\n[{DateTime.Now}]------Удален заказ на {del.Tovar.Name}");
                    db.ZakazList.Remove(del);
                    db.SaveChanges();

                    if (Program.f1.log == "Работник")
                    {
                        zakazListDataGridView.DataSource = db.ZakazList.Select(a => new { id = a.id, Название = a.Tovar.Name, Количество = a.Count, Статус = a.Status, Заказчик = a.Hospital.Name }).ToList();
                        zakazListDataGridView.Columns[0].Visible = false;
                        dvgStatPaint();
                    }
                    else
                    {
                        zakazListDataGridView.DataSource = db.ZakazList.Where(x => x.id_zak == Program.f1.a).Select(x => new { id = x.id, Название = x.Tovar.Name, Количество = x.Count, Статус = x.Status, Заказчик = x.Hospital.Name }).ToList();
                        zakazListDataGridView.Columns[0].Visible = false;
                        dvgStatPaint();
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
            //это тут какя-то лицензия нужна хз зачем это надо но без него у меня не работало
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //тут путь изначальный чо покажется при выборе куда сохранять
            var file = new FileInfo(fileName: @"C:\Users\kinri\Desktop\Otchet\Otchet.xlsx");
            // тут хз что но в фильтре вроде показывается формат         |
            //                                                           v вот тут
            using (SaveFileDialog fileDialog = new SaveFileDialog() { Filter = "Excel Workbook| *.xlsx" })
            {
                //это если ты в выборе куда сохранять файл нажал на ок, то он будет делать
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    //тут он имя файла сохраняет которое ты должен был написать при указании куда сохранить файл
                    var fileInfo = new FileInfo(fileDialog.FileName);
                    //тут создает файл с таким именем
                    using (var pakage = new ExcelPackage(fileInfo))
                    {
                        //добавляет страничку "Prodaja"
                        var worksheet = pakage.Workbook.Worksheets.Add("Prodaja");
                        // тут устанавливает формат ячейки шоб дата нормально отображалась
                        worksheet.Cells["C2:C300"].Style.Numberformat.Format = "yyyy-mm-dd";
                        //тут вывод из бд у меня, в видосе смотрел можно и их таблицы данные брать, а не селектом как я
                        worksheet.Cells.LoadFromCollection(db.Prodaja.Select(x => new { x.id, Название = x.ZakazList.Tovar.Name, Дата_продажи = x.Data, Количество = x.ZakazList.Count, Цена_за_единицу = x.ZakazList.Tovar.Price, Общая_цена = x.ZakazList.Tovar.Price * x.ZakazList.Count, Заказчик = x.ZakazList.Hospital.Name }), true);
                        // тут оно короче делает так что бы размер ячейки подстраивался под текст, растягивается ячейка короче
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        // тут ячейкам в диапазоне задаются границы толстенькие
                        worksheet.Cells["A1:G1"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:G1"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:G1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:G1"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        //тут он фильтрацию добавляет на ячейку С1
                        worksheet.Cells["C1"].AutoFilter = true;
                        //ну и тут сохраняет файл
                        pakage.Save();
                        File.AppendAllText(path, $"\n[{DateTime.Now}]------Сформирован отчет по заказам");
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

        private void изменитьРасположениеТовараToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeSklad changeSklad = new ChangeSklad();
            int t = tovarDataGridView.Rows.IndexOf(tovarDataGridView.SelectedRows[0]);
            int str = Convert.ToInt32(tovarDataGridView.Rows[t].Cells[0].Value);
            Tovar del = db.Tovar.FirstOrDefault(b => b.id == str);
            changeSklad.skladID = str;
            changeSklad.ShowDialog();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < zakazListDataGridView.RowCount; i++)
                if (Convert.ToString(zakazListDataGridView.Rows[i].Cells[1].Value).ToLower().Contains(textBox2.Text.ToLower()) && textBox2.Text != null && textBox2.Text != "")
                    zakazListDataGridView.Rows[i].Cells[1].Selected = true;
                else zakazListDataGridView.Rows[i].Cells[1].Selected = false;
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            zakazListDataGridView.DataSource = db.ZakazList.Where(x => x.Hospital.id == (int)comboBox3.SelectedValue).Select(a => new { id = a.id, Название = a.Tovar.Name, Количество = a.Count, Статус = a.Status, Заказчик = a.Hospital.Name, Цена = a.Tovar.Price * a.Count }).ToList();
            dvgStatPaint();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //это тут какя-то лицензия нужна хз зачем это надо но без него у меня не работало
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //тут путь изначальный чо покажется при выборе куда сохранять
            var file = new FileInfo(fileName: @"C:\Users\kinri\Desktop\Otchet\Otchet.xlsx");
            // тут хз что но в фильтре вроде показывается формат         |
            //                                                           v вот тут
            using (SaveFileDialog fileDialog = new SaveFileDialog() { Filter = "Excel Workbook| *.xlsx" })
            {
                //это если ты в выборе куда сохранять файл нажал на ок, то он будет делать
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    //тут он имя файла сохраняет которое ты должен был написать при указании куда сохранить файл
                    var fileInfo = new FileInfo(fileDialog.FileName);
                    //тут создает файл с таким именем
                    using (var pakage = new ExcelPackage(fileInfo))
                    {
                        //добавляет страничку "Prodaja"
                        var worksheet = pakage.Workbook.Worksheets.Add("Postavki");
                        // тут устанавливает формат ячейки шоб дата нормально отображалась
                        worksheet.Cells["C2:C300"].Style.Numberformat.Format = "yyyy-mm-dd";
                        //тут вывод из бд у меня, в видосе смотрел можно и их таблицы данные брать, а не селектом как я
                        worksheet.Cells.LoadFromCollection(db.Postavka.Select(x=>new { x.id, Название= x.Tovar.Name, Дата= x.Data, Количество= x.Kol_vo}) , true);
                        // тут оно короче делает так что бы размер ячейки подстраивался под текст, растягивается ячейка короче
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        // тут ячейкам в диапазоне задаются границы толстенькие
                        worksheet.Cells["A1:D1"].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:D1"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:D1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells["A1:D1"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        //тут он фильтрацию добавляет на ячейку С1
                        worksheet.Cells["B1:C1"].AutoFilter = true;
                        //ну и тут сохраняет файл
                        pakage.Save();
                        File.AppendAllText(path, $"\n[{DateTime.Now}]------Сформирован отчет по поставкам товаров");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = File.ReadAllText(path);
        }

        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (zakazListDataGridView.SelectedRows.Count > 0 && zakazListDataGridView.SelectedRows.Count <= 1)
            {
                int t = tovarDataGridView.Rows.IndexOf(tovarDataGridView.SelectedRows[0]);
                int str = Convert.ToInt32(tovarDataGridView.Rows[t].Cells[0].Value);
                Tovar del = db.Tovar.FirstOrDefault(b => b.id == str);

                if (del != null)
                {
                    File.AppendAllText(path, $"\n {DateTime.Now}------Удален товар {del.Name}");
                    db.Tovar.Remove(del);
                    db.SaveChanges();
                    tovarDataGridView.DataSource = db.Tovar.Select(x => new { id = x.id, Название = x.Name, Цена = x.Price, Количество = x.Kol_vo, Склад = x.Sklad.Name }).ToList();
                    tovarDataGridView.Columns[0].Visible = false;
                    dgvPaint();


                }
            }
            else
                MessageBox.Show("Вы не выбрали строку, или выбрали больше одной");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            File.WriteAllText(path, string.Empty);
            richTextBox1.Clear();

        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 1:
                    dgvPaint();
                    break;
                case 2:
                    dataGridView1.DataSource = db.Prodaja.Select(x => new { Название = x.ZakazList.Tovar.Name, Дата_продажи = x.Data, Количество = x.ZakazList.Count, Цена_за_единицу = x.ZakazList.Tovar.Price, Общая_цена = x.ZakazList.Tovar.Price * x.ZakazList.Count, Заказчик = x.ZakazList.Hospital.Name }).ToList();
                    dataGridView2.DataSource = db.Postavka.Select(x => new { x.Tovar.Name, x.Kol_vo, x.Data }).ToList();
                    break;
                case 3: richTextBox1.Text = File.ReadAllText(path);
                    break;
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SpravkaForm spravka = new SpravkaForm();
            if (Program.f1.log=="Работник")
            {
                spravka.SpravkaName = "На вкладке 'Заказы' Вы можете изменять уже существующие заказы(ПКМ по таблице-> Изменить)\n и удалять их(ПКМ по таблице-> Удалить).\nДля поиска по названию, введите полное названия товара, или часть и Вам подсветит те элементы, где встречается данное сочетиние символов.\nДля фильтрации заказов нажните на выпадающее меню сверху и выберите нужное";
            }
            else
            spravka.SpravkaName = "На вкладке 'Заказы' Вы можете добавить новый заказ(ПКМ по таблице-> Добавить\nДля поиска по названию, введите полное названия товара, или часть и Вам подсветит те элементы, где встречается данное сочетиние символов.)";
            spravka.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SpravkaForm spravka = new SpravkaForm();
            if (Program.f1.log == "Работник")
            {
                spravka.SpravkaName = "На вкладке 'Склад' вы можеет просматривать информацию по колличеству определенного товара на складе\nТакже Вы можете изменять название товара((ПКМ по таблице-> Измениь название) и изменять номер склада на котором находится этот товар(ПКМ по таблице-> Изменить расположение товара), а также Вы можете удалить любой товар (ПКМ по таблице-> Удалить)), и проводить поиск, введя необходимое словосочетание в поле сверху";
            }
            else
                spravka.SpravkaName = "На вкладке 'Склад' вы можеет просматривать информацию по колличеству определенного товара на складе, а также проводить поиск, введя необходимое словосочетание в поле сверху";
            spravka.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SpravkaForm spravka = new SpravkaForm();
            spravka.SpravkaName = "На вкладке 'Отчет' Вы можете создавать отчеты на поставкам или продажам, нажав на соответствующие кнопки над таблицами";
            spravka.Show();
        }

        private void MainForm_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

