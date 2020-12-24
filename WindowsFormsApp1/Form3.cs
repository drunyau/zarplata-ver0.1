using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary1;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e) //выбор всех работников
        {
            dataGridView2.Rows.Clear(); //очистить поле отображения данных
            dataGridView2.Show();
            dataGridView3.Hide();
            dataGridView4.Hide();//скрыть 2 из 3 полей отображения из преднастроенных
            List<String[]> entries = DataAccess.GetDataWorkers(); //выбор всех сотрудников
            foreach (string[] s in entries)
                dataGridView2.Rows.Add(s); //вывести сотрудников в поле отображения
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();//очистить поле отображения данных
            dataGridView2.Hide();
            dataGridView3.Show();
            dataGridView4.Hide();//скрыть 2 из 3 полей отображения из преднастроенных
            List<String[]> entries = DataAccess.GetDataWork(); //выбор всех отделов
            foreach (string[] s in entries) 
                dataGridView3.Rows.Add(s);//вывести отделы в поле отображения
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();//очистить поле отображения данных
            dataGridView2.Hide();
            dataGridView3.Hide();
            dataGridView4.Show();//скрыть 2 из 3 полей отображения из преднастроенных
            List<String[]> entries = DataAccess.GetDataRole(); //выбор всех должностей
            foreach (string[] s in entries)
                dataGridView4.Rows.Add(s); //вывести должности в поле отображения
        }

        private void button1_Click(object sender, EventArgs e) //кнопка возврата в основное меню
        {
            Hide(); //свернуть форму удаления данных
        }

        private void button7_Click(object sender, EventArgs e) //кнопка удаления сотрудника
        {
            DataAccess.DeleteDatatoEmployee(Convert.ToInt32(textBox2.Text)); //удаление сотрудника с id из текстового поля
        }

        private void button3_Click(object sender, EventArgs e) //кнопка удаления отдела
        {
            DataAccess.DeleteDatatoWork(Convert.ToInt32(textBox1.Text));//удаление отдела с id из текстового поля
        }

        private void button2_Click(object sender, EventArgs e) //кнопка удаления должности
        {
            DataAccess.DeleteDatatoRole(Convert.ToInt32(textBox3.Text));//удаление должности с id из текстового поля
        }
    }
}
