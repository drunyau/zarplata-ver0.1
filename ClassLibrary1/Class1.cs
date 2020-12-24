using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace ClassLibrary1
{
    public static class DataAccess //вспомогательный класс для работы с БД и запросами
    {
        public static void InitializeDatabase()   //проверка существования датабаз в sqlite. 
        {
            string curFile = Directory.GetCurrentDirectory();  //захват нынешней директории программы
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //путь до ДБ
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))  //коннект к базе
            {
                db.Open(); //открытие доступа к ДБ 

                string tableCommand = "CREATE TABLE IF NOT EXISTS WORKERS (" +
                    "ID INTEGER PRIMARY KEY NOT NULL, " +
                    "NAME TEXT(100), " +
                    "date TEXT(100), " +
                    "IDWORK INTEGER NOT NULL REFERENCES [WORK]([id]), "+
                    "IDMANAGER INTEGER NOT NULL, "+
                    "IDROLE INTEGER NOT NULL REFERENCES [ROLE]([id])," +
                    "PASS TEXT(10))"; // создание таблицы работников, если её нет. данные первой таблицы с 6 столбцами. 1 - главный ключ - ИД работника, его имя, дата принятия на работу, 
                                     //его идентификаторы по отделу и роли, а также пароль.

                SqliteCommand createTable = new SqliteCommand(tableCommand, db); //создание ДБ

                createTable.ExecuteReader(); //посылка команды в базу
            }
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))  //коннект к базе
            {
                db.Open(); //открытие ДБ

                string tableCommand = "CREATE TABLE IF NOT EXISTS WORK (" +
                    "ID INTEGER PRIMARY KEY NOT NULL, " +
                    "NAME TEXT(100), " +
                    "IDWORKER1 INTEGER, " +
                    "IDWORKER2 INTEGER, " +
                    "IDSUPERWORKER INTEGER)"; // создание таблицы отделов, если её нет. данные первой таблицы с 4 столбцами. 1 - главный ключ - ИД отдела, 
                                               //его имя, ИД его менеджера и ИД продавца

                SqliteCommand createTable = new SqliteCommand(tableCommand, db); //создание ДБ

                createTable.ExecuteReader();//посылка команды в базу
            }
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))  //коннект к базе
            {
                db.Open(); //открытие ДБ

                string tableCommand = "CREATE TABLE IF NOT EXISTS ROLE (" +
                    "ID INTEGER PRIMARY KEY NOT NULL, " +
                    "NAME TEXT(100), " +
                    "MONEY INTEGER)"; // создание таблицы отделов, если её нет. данные первой таблицы с 4 столбцами. 1 - главный ключ - ИД отдела, 
                                      //его имя, ИД его менеджера и ИД продавца

                SqliteCommand createTable = new SqliteCommand(tableCommand, db); //создание ДБ

                createTable.ExecuteReader();//посылка команды в базу
            }
        }
        public static void AddDatatoEmployee(string name, string date, int idWork, int idRole, int pass, int IDManager)  //добавление данных в таблицу работников
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //доступ к файлу
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}")) //установка соединения с ДБ
            {
                db.Open();  //открытие ДБ

                SqliteCommand insertCommand = new SqliteCommand(); //создание нового экземпляра команды
                insertCommand.Connection = db; //ввод в команду места базы

                // выбор столбцов таблицы через SQL запрос INSERT и создание новой строки с входными данными команды
                insertCommand.CommandText = "INSERT INTO WORKERS (NAME, date, IDWORK, IDManager, IDROLE, pass) VALUES (@firstarg, @secondarg, @thirdarg, @fourtharg, @fivetharg, @sixarg)"; //вставка данных в ДБ
                insertCommand.Parameters.AddWithValue("firstarg", name);
                insertCommand.Parameters.AddWithValue("secondarg", date);
                insertCommand.Parameters.AddWithValue("thirdarg", idWork);
                insertCommand.Parameters.AddWithValue("fourtharg", IDManager);
                insertCommand.Parameters.AddWithValue("fivetharg", idRole);
                insertCommand.Parameters.AddWithValue("sixarg", pass);

                insertCommand.ExecuteReader(); //исполнение запроса

                db.Close(); //закрытие ДБ
            }

        }
        public static void AddDatatoWork(string name, int idWorker1, int idWorker2, int idSuperWorker)  //добавление данных в таблицу отделов
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //доступ к файлу
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}")) //установка соединения с ДБ
            {
                db.Open(); //открытие ДБ

                SqliteCommand insertCommand = new SqliteCommand();//создание нового экземпляра команды
                insertCommand.Connection = db;//ввод в команду места базы

                // выбор столбцов таблицы через SQL запрос INSERT и создание новой строки с входными данными команды
                insertCommand.CommandText = "INSERT INTO WORK (NAME, IDWORKER1, IDWORKER2, IDSUPERWORKER) VALUES (@firstarg, @secondarg,  @thirdarg, @fourarg)"; //вставка данных в ДБ
                insertCommand.Parameters.AddWithValue("firstarg", name);
                insertCommand.Parameters.AddWithValue("secondarg", idWorker1);
                insertCommand.Parameters.AddWithValue("thirdarg", idWorker2);
                insertCommand.Parameters.AddWithValue("fourarg", idSuperWorker);
                insertCommand.ExecuteReader();//исполнение запроса

                db.Close();//закрытие ДБ
            }

        }
        public static void AddDatatoRole(string name, int basemoney)  //добавление данных в таблицу должностей
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //доступ к файлу
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}")) //установка соединения с ДБ
            {
                db.Open();//открытие ДБ

                SqliteCommand insertCommand = new SqliteCommand();//создание нового экземпляра команды
                insertCommand.Connection = db;//ввод в команду места базы

                // выбор столбцов таблицы через SQL запрос INSERT и создание новой строки с входными данными команды
                insertCommand.CommandText = "INSERT INTO ROLE (NAME, MONEY) VALUES (@firstarg, @secondarg)"; //вставка данных в ДБ
                insertCommand.Parameters.AddWithValue("firstarg", name);
                insertCommand.Parameters.AddWithValue("secondarg", basemoney);
                insertCommand.ExecuteReader();//исполнение запроса

                db.Close();//закрытие ДБ
            }

        }
        public static void DeleteDatatoEmployee(int id)  //удаление данных из таблицы работников по ИД строки
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //доступ к файлу
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}")) //установка соединения с ДБ
            {
                db.Open();//открытие ДБ

                SqliteCommand insertCommand = new SqliteCommand();//создание нового экземпляра команды
                insertCommand.Connection = db;//ввод в команду места базы

                //выбор строки таблицы через SQL запрос DELETE с входными данными команды
                insertCommand.CommandText = "DELETE FROM WORKERS WHERE (id = @firstarg)"; //вставка данных в ДБ
                insertCommand.Parameters.AddWithValue("firstarg", id);
                insertCommand.ExecuteReader();//исполнение запроса

                db.Close();//закрытие ДБ
            }

        }
        public static void DeleteDatatoWork(int id)  //удаление данных из таблицы отделов по ИД строки
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //доступ к файлу
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}")) //установка соединения с ДБ
            {
                db.Open();//открытие ДБ

                SqliteCommand insertCommand = new SqliteCommand();//создание нового экземпляра команды
                insertCommand.Connection = db;//ввод в команду места базы

                //выбор строки таблицы через SQL запрос DELETE с входными данными команды
                insertCommand.CommandText = "DELETE FROM WORK WHERE (id = @firstarg)"; //вставка данных в ДБ
                insertCommand.Parameters.AddWithValue("firstarg", id);
                insertCommand.ExecuteReader();//исполнение запроса

                db.Close();//закрытие ДБ
            }

        }
        public static void DeleteDatatoRole(int id)  //удаление данных из таблицы отделов по ИД строки
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db"); //доступ к файлу
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}")) //установка соединения с ДБ
            {
                db.Open();//открытие ДБ

                SqliteCommand insertCommand = new SqliteCommand();//создание нового экземпляра команды
                insertCommand.Connection = db;//ввод в команду места базы

                //выбор строки таблицы через SQL запрос DELETE с входными данными команды
                insertCommand.CommandText = "DELETE FROM ROLE WHERE (id = @firstarg)"; //вставка данных в ДБ
                insertCommand.Parameters.AddWithValue("firstarg", id);
                insertCommand.ExecuteReader();//исполнение запроса

                db.Close();//закрытие ДБ
            }

        }
        public static List<String[]> GetDataAll() //вспомогательная функция для расчёта зарплаты. выбирает всех работников и потом удаляет всех, кроме Salesman.
                                                  //Необходима для расчёта ЗП всех сотрудников и вывода общих затрат на фонд оплаты труда.
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db");//доступ к файлу
            List<String[]> entries = new List<string[]>(); //Создание встроенного листа, который получает данные из ДБ

            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}")) //установка соединения с ДБ
            {
                db.Open();//открытие ДБ
                //выбор столбцов таблиц через SQL запрос SELECT с применением взаимосвязей JOIN
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT WORKERS.NAME, WORKERS.date, WORK.NAME, (SELECT WORKERS.NAME FROM WORKERS WHERE CASE WHEN WORKERS.IDMANAGER=1 THEN WORK.IDWORKER1= WORKERS.ID WHEN WORKERS.IDMANAGER=2 THEN WORK.IDWORKER2= WORKERS.ID END), " +
                    "(SELECT WORKERS.NAME FROM WORKERS WHERE WORK.IDSUPERWORKER = WORKERS.ID), ROLE.NAME, ROLE.MONEY, WORKERS.IDManager " +
                    "FROM WORKERS INNER JOIN WORK, ROLE " +
                    "WHERE WORKERS.IDWORK = WORK.ID AND WORKERS.IDROLE = ROLE.ID", db);

                SqliteDataReader query = selectCommand.ExecuteReader();//исполнение запроса и открытие потока для записи в лист.

                while (query.Read()) //пока из запроса в поток идут данные
                {
                    entries.Add(new string[8]);//создание новой строки листа и поклеточное копирование в лист.

                    entries[entries.Count - 1][0] = query[0].ToString();
                    entries[entries.Count - 1][1] = query[1].ToString();
                    entries[entries.Count - 1][2] = query[2].ToString();
                    entries[entries.Count - 1][3] = query[3].ToString();
                    entries[entries.Count - 1][4] = query[4].ToString();
                    entries[entries.Count - 1][5] = query[5].ToString();
                    entries[entries.Count - 1][6] = query[6].ToString();
                    entries[entries.Count - 1][7] = query[7].ToString();
                }

                query.Close(); //закрытие потока в лист.

                db.Close(); //закрытие ДБ

            }
            for (int i = entries.Count - 1; i >= 0; i--) //выбор строк, где должность не Salesman. Иными словами - удаление из листа Employee и Manager
            {
                if (entries[i][5] != "Salesman")
                    entries.RemoveAt(i);
            }
            return entries; //вернуть оставшиеся записи
        }
        public static List<String[]> GetDataWorkers()// функция выбора всех сотрудников
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db");//доступ к файлу
            List<String[]> entries = new List<string[]>();//Создание встроенного листа, который получает данные из ДБ

            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))//установка соединения с ДБ
            {
                db.Open();//открытие ДБ
                //выбор всей таблицы через SQL запрос SELECT
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from WORKERS", db);

                SqliteDataReader query = selectCommand.ExecuteReader();//исполнение запроса и открытие потока для записи в лист.

                while (query.Read())//пока из запроса в поток идут данные
                {
                    entries.Add(new string[7]);//создание новой строки листа и поклеточное копирование в лист.

                    entries[entries.Count - 1][0] = query[0].ToString();
                    entries[entries.Count - 1][1] = query[1].ToString();
                    entries[entries.Count - 1][2] = query[2].ToString();
                    entries[entries.Count - 1][3] = query[3].ToString();
                    entries[entries.Count - 1][4] = query[4].ToString();
                    entries[entries.Count - 1][5] = query[5].ToString();
                    entries[entries.Count - 1][6] = query[6].ToString();
                }

                query.Close();//закрытие потока в лист.

                db.Close();//закрытие ДБ
            }

            return entries;//вернуть записи для отображения
        }
        public static List<String[]> GetDataWork()// функция выбора всех отделов
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db");//доступ к файлу
            List<String[]> entries = new List<string[]>();//Создание встроенного листа, который получает данные из ДБ

            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))//установка соединения с ДБ
            {
                db.Open();//открытие ДБ
                //выбор всей таблицы через SQL запрос SELECT
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from WORK", db);

                SqliteDataReader query = selectCommand.ExecuteReader();//исполнение запроса и открытие потока для записи в лист.

                while (query.Read())//пока из запроса в поток идут данные
                {
                    entries.Add(new string[5]);//создание новой строки листа и поклеточное копирование в лист.

                    entries[entries.Count - 1][0] = query[0].ToString();
                    entries[entries.Count - 1][1] = query[1].ToString();
                    entries[entries.Count - 1][2] = query[2].ToString();
                    entries[entries.Count - 1][3] = query[3].ToString();
                    entries[entries.Count - 1][3] = query[4].ToString();
                }

                query.Close();//закрытие потока в лист.

                db.Close();//закрытие ДБ
            }

            return entries;//вернуть записи для отображения
        }
        public static List<String[]> GetDataRole()// функция выбора всех должностей
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db");//доступ к файлу
            List<String[]> entries = new List<string[]>();//Создание встроенного листа, который получает данные из ДБ

            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))//установка соединения с ДБ
            {
                db.Open();//открытие ДБ
                //выбор всей таблицы через SQL запрос SELECT
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from ROLE", db);

                SqliteDataReader query = selectCommand.ExecuteReader();//исполнение запроса и открытие потока для записи в лист.

                while (query.Read())//пока из запроса в поток идут данные
                {
                    entries.Add(new string[3]);//создание новой строки листа и поклеточное копирование в лист.

                    entries[entries.Count - 1][0] = query[0].ToString();
                    entries[entries.Count - 1][1] = query[1].ToString();
                    entries[entries.Count - 1][2] = query[2].ToString();
                }

                query.Close();//закрытие потока в лист.

                db.Close();//закрытие ДБ
            }

            return entries;//вернуть записи для отображения
        }
        public static List<String[]> GetDataOnePerson(string name, int pass) //функция идентификации пользователя по логину/паролю
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db");//доступ к файлу
            List<String[]> entries = new List<string[]>();//Создание встроенного листа, который получает данные из ДБ

            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))//установка соединения с ДБ
            {
                db.Open();//открытие ДБ
                          //выбор столбцов таблиц через SQL запрос SELECT с применением взаимосвязей JOIN и идентификацией пользователя
                SqliteCommand selectCommand = new SqliteCommand
                ("SELECT WORKERS.NAME, WORKERS.date, WORK.NAME, (SELECT WORKERS.NAME FROM WORKERS WHERE CASE WHEN WORKERS.IDMANAGER=1 THEN WORK.IDWORKER1= WORKERS.ID WHEN WORKERS.IDMANAGER=2 THEN WORK.IDWORKER2= WORKERS.ID END), " +
                "(SELECT WORKERS.NAME FROM WORKERS WHERE WORK.IDSUPERWORKER = WORKERS.ID), ROLE.NAME, ROLE.MONEY, WORKERS.IDManager " +
                "FROM WORKERS INNER JOIN WORK, ROLE " +
                "WHERE WORKERS.IDWORK = WORK.ID AND WORKERS.IDROLE = ROLE.ID AND WORKERS.NAME = @firstarg AND WORKERS.PASS =  @secondarg", db);
                selectCommand.Parameters.AddWithValue("firstarg", name);
                selectCommand.Parameters.AddWithValue("secondarg", pass);
            

                SqliteDataReader query = selectCommand.ExecuteReader();//исполнение запроса и открытие потока для записи в лист.

                while (query.Read())//пока из запроса в поток идут данные
                {
                    entries.Add(new string[8]);//создание новой строки листа и поклеточное копирование в лист.

                    entries[entries.Count - 1][0] = query[0].ToString();
                    entries[entries.Count - 1][1] = query[1].ToString();
                    entries[entries.Count - 1][2] = query[2].ToString();
                    entries[entries.Count - 1][3] = query[3].ToString();
                    entries[entries.Count - 1][4] = query[4].ToString();
                    entries[entries.Count - 1][5] = query[5].ToString();
                    entries[entries.Count - 1][6] = query[6].ToString();
                    entries[entries.Count - 1][7] = query[7].ToString();
                }

                query.Close();//закрытие потока в лист.

                db.Close();//закрытие ДБ
            }

            return entries;//вернуть записи для отображения
        }
        public static List<String[]> GetDataGroupPerson(string name)//функция идентификации пользователей по отделу
        {
            string curFile = Directory.GetCurrentDirectory();
            string dbpath = Path.Combine(curFile, "sqlitezarplata.db");//доступ к файлу
            List<String[]> entries = new List<string[]>();//Создание встроенного листа, который получает данные из ДБ

            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))//установка соединения с ДБ
            {
                db.Open();//открытие ДБ
                          //выбор столбцов таблиц через SQL запрос SELECT с применением взаимосвязей JOIN и идентификацией пользователей по отделу
                SqliteCommand selectCommand = new SqliteCommand
                ("SELECT DISTINCT WORKERS.NAME, WORKERS.date, WORK.NAME, (SELECT WORKERS.NAME FROM WORKERS WHERE CASE WHEN WORKERS.IDMANAGER=1 THEN WORK.IDWORKER1= WORKERS.ID WHEN WORKERS.IDMANAGER=2 THEN WORK.IDWORKER2= WORKERS.ID END), " +
                "(SELECT WORKERS.NAME FROM WORKERS WHERE WORK.IDSUPERWORKER = WORKERS.ID), ROLE.NAME, ROLE.MONEY, WORKERS.IDManager " +
                "FROM WORKERS INNER JOIN WORK, ROLE " +
                "WHERE WORKERS.IDWORK = WORK.ID AND WORKERS.IDROLE = ROLE.ID AND WORK.NAME = @firstarg", db);
                selectCommand.Parameters.AddWithValue("firstarg", name);

                SqliteDataReader query = selectCommand.ExecuteReader();//исполнение запроса и открытие потока для записи в лист.

                while (query.Read())//пока из запроса в поток идут данные
                {
                    entries.Add(new string[8]);//создание новой строки листа и поклеточное копирование в лист.

                    entries[entries.Count - 1][0] = query[0].ToString();
                    entries[entries.Count - 1][1] = query[1].ToString();
                    entries[entries.Count - 1][2] = query[2].ToString();
                    entries[entries.Count - 1][3] = query[3].ToString();
                    entries[entries.Count - 1][4] = query[4].ToString();
                    entries[entries.Count - 1][5] = query[5].ToString();
                    entries[entries.Count - 1][6] = query[6].ToString();
                    entries[entries.Count - 1][7] = query[7].ToString();
                }

                query.Close();//закрытие потока в лист.

                db.Close();//закрытие ДБ
            }

            return entries;//вернуть записи для отображения
        }
    }
}