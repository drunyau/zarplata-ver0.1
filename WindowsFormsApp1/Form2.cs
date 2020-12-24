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
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) //добавление нового сотрудника
        {
            label13.Text = ""; //очистка поля сообщений
            if (textBox2.Text != null) //проверка вводимых данных и выдача ошибок в текстовом формате
            {
                if (textBox5 != null)
                {
                    if (textBox6 != null)
                    {
                        if (textBox4 != null)
                        {
                            //вызов функции для добавления нового сотрудника
                            DataAccess.AddDatatoEmployee(textBox2.Text, dateTimePicker1.Value.ToString(), Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox9.Text));
                            label13.Text = "OK";
                        }
                        else
                        {
                            label13.Text = "password is empty";
                        }
                    }
                    else
                    {
                        label13.Text = "Role is empty";
                    }

                }
                else
                {
                    label13.Text = "Work is empty";
                }
            }
            else
            {
                label13.Text = "Name is empty";
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //добавление нового отдела
        {
            label14.Text = "";//очистка поля сообщений
            if (textBox11.Text != null)//проверка вводимых данных и выдача ошибок в текстовом формате
            {
                if (textBox8.Text != null)
                {
                    if (textBox7.Text != null)
                    {
                        //вызов функции для добавления нового отдела
                        DataAccess.AddDatatoWork(textBox11.Text, Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox7.Text));
                        label14.Text = "OK";
                    }
                    else
                    {
                        label14.Text = "Salesman is empty";
                    }
                }
                else
                {
                    label14.Text = "Manager is empty";
                }
            }
            else
            {
                label14.Text = "Name is empty";
            }
        }
        private void button1_Click(object sender, EventArgs e) //кнопка возврата в основное меню
        {
            Hide();//свернуть форму удаления данных
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e) //добавление новой должности
        {
            label9.Text = ""; //очистка поля сообщений
            if (textBox3.Text != null)//проверка вводимых данных и выдача ошибок в текстовом формате
            {
                if (textBox1.Text != null)
                {

                    DataAccess.AddDatatoRole(textBox3.Text, Convert.ToInt32(textBox1.Text));
                    label9.Text = "OK";
                }
                else
                {
                    label9.Text = "Money is empty";
                }
            }
            else
            {
                label9.Text = "Name is empty";
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
