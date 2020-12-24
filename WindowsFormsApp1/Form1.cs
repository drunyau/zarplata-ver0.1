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
using ClassLibrary1;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //проверка на наличие базы
        {
            string curFile = Directory.GetCurrentDirectory();  //захват нынешней директории программы
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //путь до ДБ
            if (File.Exists(dbpath)) //есть ДБ есть - выдаём ОК. если её нет - сообщение об этом
            {
                label1.Text = "connect OK";
            }
            else
            {
                label1.Text = "Not connect";
            }
        }

        private void button3_Click(object sender, EventArgs e) //кнопка вывода зарплаты
        {
            if (textBox1.Text == "me") //логин суперадмина
            {
                if (textBox2.Text == "281295") //пароль суперадмина
                {
                    dataGridView1.Rows.Clear(); //очистить область просмотра
                    List<String[]> entries = DataAccess.GetDataAll(); //получить данные о всех сотрудниках
                    double zarplata = 0; //переменная вывода общей зарплаты
                    foreach (string[] u in entries) //поочередный обсчёт всех salesman
                    {
                        List<String[]> group = DataAccess.GetDataGroupPerson(u[2]); //получение данных об отделе
                        List<String[]> result = new List<string[]>(); //создание результирующего списка
                        double visluga; //вспомогательная переменная. отвечает за выслугу лет
                        foreach (string[] s in group) //обсчёт каждого работника отдела
                        {
                            result.Add(new string[11]); //добавление новой строки для каждого сотрудника
                            result[result.Count - 1][0] = s[0].ToString(); //имя
                            result[result.Count - 1][1] = s[1].ToString(); //дата найма
                            result[result.Count - 1][2] = s[2].ToString(); //название отдела
                            result[result.Count - 1][3] = s[3].ToString(); //имя менеджера
                            result[result.Count - 1][4] = s[4].ToString(); //имя salesman
                            result[result.Count - 1][5] = s[5].ToString(); // должность
                            result[result.Count - 1][6] = s[6].ToString(); //ставка
                            double totalYears = Math.Floor(((dateTimePicker1.Value - 
                                Convert.ToDateTime(result[result.Count - 1][1])).TotalDays) / 365);
                            result[result.Count - 1][7] = totalYears.ToString(); //выслуга в годах
                            if (totalYears != 0)
                            {
                                visluga = totalYears * 0.03;
                            }
                            else
                            {
                                visluga = 0;
                            }
                            if (Convert.ToInt32(result[result.Count - 1][7]) >= 10)
                            {
                                visluga = 0.3;
                            } //ограничение по выслуге
                            result[result.Count - 1][8] = "0"; //количество подчиненных
                            result[result.Count - 1][9] = (Convert.ToInt32(result[result.Count - 1][6]) 
                                + Convert.ToInt32(result[result.Count - 1][6]) * visluga).ToString(); //зарплата Employee
                            result[result.Count - 1][10] = s[7].ToString(); //вспомогательная величина, к какому менеджеру относится Employee
                        }
                        float sumformanager1 = 0f; //сумма надбавки для первого менеджера в отделе
                        float sumformanager2 = 0f; //сумма надбавки для второго менеджера в отделе
                        float sumforsalesman = 0f; //сумма надбавки для salesman
                        int workers1 = 0;  //количество работников у первого менеджера
                        int workers2 = 0;  //количество работников у второго менеджера
                        int workerssalesman = 0; //количество работников у salesman
                        foreach (string[] s in result) //прогон таблицы для выявления надбавок
                        {
                            if (s[5] == "Employee" && s[10] == "1") //проверка должности и менеджера
                            {
                                sumformanager1 += float.Parse(s[9]) * 0.005f;
                                sumforsalesman += float.Parse(s[9]) * 0.003f;
                                workers1++;
                                workerssalesman++;
                            }
                            if (s[5] == "Employee" && s[10] == "2") //проверка должности и менеджера
                            {
                                sumformanager2 += float.Parse(s[9]) * 0.005f;
                                sumforsalesman += float.Parse(s[9]) * 0.003f;
                                workers2++;
                                workerssalesman++;
                            }
                        }
                        foreach (string[] s in result) //высчитываем зарплату менеджеров
                        {
                            if (s[5] == "Manager" && s[10] == "1") //проверка должности и менеджера
                            {
                                var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(s[1])).TotalDays;
                                visluga = Math.Floor(totalYears / 365) * 0.05;
                                if (Convert.ToInt32(s[7]) >= 8)
                                {
                                    visluga = 0.4;
                                }
                                s[8] = workers1.ToString();
                                s[9] = (float.Parse(s[6]) + float.Parse(s[6]) * visluga + sumformanager1).ToString();
                                workerssalesman++;
                                sumforsalesman += float.Parse(s[9]) * 0.003f;
                            }
                            if (s[5] == "Manager" && s[10] == "2") //проверка должности и менеджера
                            {
                                var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(s[1])).TotalDays;
                                visluga = Math.Floor(totalYears / 365) * 0.05;
                                if (Convert.ToInt32(s[7]) >= 8)
                                {
                                    visluga = 0.4;
                                }
                                s[8] = workers2.ToString();
                                s[9] = (float.Parse(s[6]) + float.Parse(s[6]) * visluga + sumformanager2).ToString();
                                workerssalesman++;
                                sumforsalesman += float.Parse(s[9]) * 0.003f;
                            }
                        } 
                        foreach (string[] s in result) //высчитываем зарплату salesman
                        {
                            if (s[5] == "Salesman")
                            {
                                var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(s[1])).TotalDays;
                                visluga = Math.Floor(totalYears / 365) * 0.01;
                                if (Convert.ToInt32(s[7]) >= 35)
                                {
                                    visluga = 0.35;
                                }
                                s[8] = workerssalesman.ToString();
                                s[9] = Math.Floor((float.Parse(s[6]) + float.Parse(s[6]) * visluga + sumforsalesman)).ToString();
                            }
                        }
                        foreach (string[] s in result) //выводим данные в таблицу
                        {
                            dataGridView1.Rows.Add(s);// выводим данные в таблицу
                            zarplata += Math.Floor(Convert.ToDouble(s[9])); //считаем общий объём фонда оплаты труда
                        }
                        label5.Text = zarplata.ToString(); //фонд оплаты труда
                    }
                }
                else
                {
                    label6.Text = "некорректный пароль"; //проверка суперпароля
                }
            }
            else
            {
                if (textBox1.Text != null)//проверка наличия логина
                {
                    if (textBox2.Text != null)//проверка наличия пароля
                    {
                        dataGridView1.Rows.Clear(); //очистить область просмотра
                        double zarplata = 0; //вспомогательная переменная общей зарплаты
                        List<String[]> entries = DataAccess.GetDataOnePerson(textBox1.Text, Convert.ToInt32(textBox2.Text)); //получить информацию о работнике по имени и паролю
                        if (entries[entries.Count - 1][5] == "Employee") //если он Employee
                        {
                            List<String[]> result = new List<string[]>(); //создание результирующего списка
                            result.Add(new string[11]); //новая строка
                            result[result.Count - 1][0] = entries[entries.Count - 1][0].ToString(); //имя
                            result[result.Count - 1][1] = entries[entries.Count - 1][1].ToString();//дата найма
                            result[result.Count - 1][2] = entries[entries.Count - 1][2].ToString();//название отдела
                            result[result.Count - 1][3] = entries[entries.Count - 1][3].ToString();//имя менеджера
                            result[result.Count - 1][4] = entries[entries.Count - 1][4].ToString();//имя salesman
                            result[result.Count - 1][5] = entries[entries.Count - 1][5].ToString();// должность
                            result[result.Count - 1][6] = entries[entries.Count - 1][6].ToString();//ставка
                            var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(result[result.Count - 1][1])).TotalDays;
                            result[result.Count - 1][7] = (Math.Floor(totalYears / 365)).ToString();//выслуга в годах
                            double visluga = Math.Floor(totalYears / 365) * 0.03;
                            if (Convert.ToInt32(result[result.Count - 1][7]) > 10)//ограничение по выслуге
                            {
                                visluga = 0.3;
                            }
                            result[result.Count - 1][8] = "0";//количество подчиненных
                            result[result.Count - 1][9] = (Convert.ToInt32(result[result.Count - 1][6]) + 
                                    Convert.ToInt32(result[result.Count - 1][6]) * visluga).ToString();//зарплата Employee
                            result[result.Count - 1][10] = entries[entries.Count - 1][7].ToString();// вспомогательная величина, к какому менеджеру относится Employee
                            foreach (string[] s in result) //вывод информации в область просмотра
                            {
                                dataGridView1.Rows.Add(s);
                            }
                        }
                        if (entries[entries.Count - 1][5] == "Manager")//если он Manager
                        {
                            List<String[]> group = DataAccess.GetDataGroupPerson(entries[entries.Count - 1][2]); //получение информации о отделе
                            List<String[]> result = new List<string[]>();
                            double visluga;
                            foreach (string[] s in group)
                            {
                                result.Add(new string[11]); //добавление новой строки для каждого сотрудника
                                result[result.Count - 1][0] = s[0].ToString(); //имя
                                result[result.Count - 1][1] = s[1].ToString(); //дата найма
                                result[result.Count - 1][2] = s[2].ToString(); //название отдела
                                result[result.Count - 1][3] = s[3].ToString(); //имя менеджера
                                result[result.Count - 1][4] = s[4].ToString(); //имя salesman
                                result[result.Count - 1][5] = s[5].ToString(); // должность
                                result[result.Count - 1][6] = s[6].ToString(); //ставка
                                double totalYears = Math.Floor(((dateTimePicker1.Value - Convert.ToDateTime(result[result.Count - 1][1])).TotalDays)/365);
                                result[result.Count - 1][7] = totalYears.ToString(); //выслуга в годах
                                if (totalYears != 0)
                                {
                                    visluga = totalYears * 0.03;
                                }
                                else
                                {
                                    visluga = 0;
                                }
                                if (Convert.ToInt32(result[result.Count - 1][7]) >= 10) //ограничение по выслуге
                                {
                                    visluga = 0.3;
                                }
                                result[result.Count - 1][8] = "0"; //количество подчиненных
                                result[result.Count - 1][9] = (Convert.ToInt32(result[result.Count - 1][6])
                                    + Convert.ToInt32(result[result.Count - 1][6]) * visluga).ToString(); //зарплата Employee
                                result[result.Count - 1][10] = s[7].ToString(); //вспомогательная величина, к какому менеджеру относится Employee
                            }
                            float sumformanager = 0f; //величина надбавки менеджеру
                            double workers = 0; //количество подчиненных менеджеру

                            foreach (string[] s in result) //расчёт надбавки
                            {
                                if (s[5] == "Employee" && s[10] == entries[entries.Count - 1][7])//проверка на должность и свой ли это подчиненный
                                {
                                    sumformanager += float.Parse(s[9]) * 0.005f;
                                    workers++;
                                }
                            }
                            for (int i = result.Count - 1; i >= 0; i--) //удаление из cписка salesman
                            {
                                if (result[i][5] == "Salesman")
                                {
                                    result.RemoveAt(i);
                                }
                            }
                            for (int i = result.Count - 1; i >= 0; i--) //удаление из cписка чужих Employee
                            {
                                if (result[i][5] == "Employee" && result[i][10] != entries[entries.Count - 1][7])
                                {
                                    result.RemoveAt(i);
                                }
                            }
                            for (int i = result.Count - 1; i >= 0; i--) //удаление из cписка других менеджеров
                            {
                                if (result[i][5] == "Manager" && result[i][10] != entries[entries.Count - 1][7])
                                {
                                    result.RemoveAt(i);
                                }
                            }
                            foreach (string[] s in result) //расчёт зарплаты Manager
                            {
                                if (s[5] == "Manager"&& s[10] == entries[entries.Count - 1][7])
                                {
                                    var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(s[1])).TotalDays;
                                    visluga = Math.Floor(totalYears / 365) * 0.05;
                                    if (Convert.ToInt32(s[7]) >= 8)
                                    {
                                        visluga = 0.4;
                                    }
                                    s[8] = workers.ToString();
                                    s[9] = (float.Parse(s[6]) + float.Parse(s[6]) * visluga + sumformanager).ToString();
                                }
                            }
                            foreach (string[] s in result)//выведение данных в область просмотра
                                {
                                    dataGridView1.Rows.Add(s);
                                zarplata += Math.Floor(Convert.ToDouble(s[9]));
                            }
                            label5.Text = zarplata.ToString();//выведение фонда оплаты труда по группе
                        }
                        if (entries[entries.Count - 1][5] == "Salesman") //если он Salesman
                        {
                            List<String[]> group = DataAccess.GetDataGroupPerson(entries[entries.Count - 1][2]);
                            List<String[]> result = new List<string[]>();
                            double visluga;
                            foreach (string[] s in group)
                            {
                                result.Add(new string[11]); //добавление новой строки для каждого сотрудника
                                result[result.Count - 1][0] = s[0].ToString(); //имя
                                result[result.Count - 1][1] = s[1].ToString(); //дата найма
                                result[result.Count - 1][2] = s[2].ToString(); //название отдела
                                result[result.Count - 1][3] = s[3].ToString(); //имя менеджера
                                result[result.Count - 1][4] = s[4].ToString(); //имя salesman
                                result[result.Count - 1][5] = s[5].ToString(); // должность
                                result[result.Count - 1][6] = s[6].ToString(); //ставка
                                double totalYears = Math.Floor(((dateTimePicker1.Value - Convert.ToDateTime(result[result.Count - 1][1])).TotalDays) / 365);
                                result[result.Count - 1][7] = totalYears.ToString();//выслуга в годах
                                if (totalYears != 0)
                                {
                                    visluga = totalYears * 0.03;
                                }
                                else
                                {
                                    visluga = 0;
                                }
                                if (Convert.ToInt32(result[result.Count - 1][7]) >= 10)//ограничение по выслуге
                                {
                                    visluga = 0.3;
                                }
                                result[result.Count - 1][8] = "0";//количество подчиненных
                                result[result.Count - 1][9] = (Convert.ToInt32(result[result.Count - 1][6])
                                    + Convert.ToInt32(result[result.Count - 1][6]) * visluga).ToString();//зарплата Employee
                                result[result.Count - 1][10] = s[7].ToString();//вспомогательная величина, к какому менеджеру относится Employee
                            }
                            float sumformanager1 = 0f;
                            float sumformanager2 = 0f;
                            float sumforsalesman = 0f;
                            int workers1 = 0;
                            int workers2 = 0;
                            int workerssalesman = 0;
                            foreach (string[] s in result)
                            {
                                if (s[5] == "Employee" && s[10] == "1")
                                {
                                    sumformanager1 += float.Parse(s[9]) * 0.005f;
                                    sumforsalesman += float.Parse(s[9]) * 0.003f;
                                    workers1++;
                                    workerssalesman++;
                                }
                                if (s[5] == "Employee" && s[10] == "2")
                                {
                                    sumformanager2 += float.Parse(s[9]) * 0.005f;
                                    sumforsalesman += float.Parse(s[9]) * 0.003f;
                                    workers2++;
                                    workerssalesman++;
                                }
                            }
                            foreach (string[] s in result)
                            {
                                if (s[5] == "Manager" && s[10] == "1")
                                {
                                    var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(s[1])).TotalDays;
                                    visluga = Math.Floor(totalYears / 365) * 0.05;
                                    if (Convert.ToInt32(s[7]) >= 8)
                                    {
                                        visluga = 0.4;
                                    }
                                    s[8] = workers1.ToString();
                                    s[9] = (float.Parse(s[6]) + float.Parse(s[6]) * visluga + sumformanager1).ToString();
                                    workerssalesman++;
                                    sumforsalesman += float.Parse(s[9]) * 0.003f;
                                }
                                if (s[5] == "Manager" && s[10] == "2")
                                {
                                    var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(s[1])).TotalDays;
                                    visluga = Math.Floor(totalYears / 365) * 0.05;
                                    if (Convert.ToInt32(s[7]) >= 8)
                                    {
                                        visluga = 0.4;
                                    }
                                    s[8] = workers2.ToString();
                                    s[9] = (float.Parse(s[6]) + float.Parse(s[6]) * visluga + sumformanager2).ToString();
                                    workerssalesman++;
                                    sumforsalesman += float.Parse(s[9]) * 0.003f;
                                }
                            }
                            foreach (string[] s in result)
                            {
                                if (s[5] == "Salesman")
                                {
                                    var totalYears = (dateTimePicker1.Value - Convert.ToDateTime(s[1])).TotalDays;
                                    visluga = Math.Floor(totalYears / 365) * 0.01;
                                    if (Convert.ToInt32(s[7]) >= 35)
                                    {
                                        visluga = 0.35;
                                    }
                                    s[8] = workerssalesman.ToString();
                                    s[9] = Math.Floor((float.Parse(s[6]) + float.Parse(s[6]) * visluga + sumforsalesman)).ToString();
                                }
                            }

                            foreach (string[] s in result)
                            {
                                dataGridView1.Rows.Add(s);
                                zarplata += Math.Floor(Convert.ToDouble(s[9]));
                            }
                            label5.Text = zarplata.ToString();
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //вывод окна добавления сотрудников/отделов/должностей
        {
            if (textBox1.Text == "me")
            {
                if (textBox2.Text == "281295")
                {
                    Hide();
                    using (Form2 form2 = new Form2())
                        form2.ShowDialog();
                    Show();
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)//вывод окна удаления сотрудников/отделов/должностей
        {
            if (textBox1.Text == "me")
            {
                if (textBox2.Text == "281295")
                {
                    Hide();
                    using (Form3 form3 = new Form3())
                        form3.ShowDialog();
                    Show();
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
