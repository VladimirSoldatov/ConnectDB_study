using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ConnectDB
{

    internal class Program
    {
        static void select_values(SqlCommand sqlCommand)
        {
            sqlCommand.CommandText = "SELECT * FROM Persons;";
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                if (sqlDataReader.HasRows)
                {
                    int line = 0;
                    int fields = sqlDataReader.FieldCount;
                    if(line==0)
                    for(int i = 0; i<fields;i++)
                    {
                        Console.Write($"{sqlDataReader.GetName(i)}\t");
                    }
                    Console.WriteLine();

                    do {
                        while (sqlDataReader.Read())
                        {

                            for (int i = 0; i < fields; i++)
                                Console.Write($"{sqlDataReader.GetValue(i)}\t");
                            Console.WriteLine();
                            line++;


                        }
                    }
                    while (sqlDataReader.NextResult()) ;

                    Console.WriteLine(line);

                }
            }
        }
        static void insert_values(SqlCommand sqlCommand)
        {
            Console.WriteLine("Enter name");
            string name = Console.ReadLine();
            Console.WriteLine("Enter age");
            string age = Console.ReadLine();
            sqlCommand.CommandText = $"INSERT INTO Persons (Name, age) Values ('{name}', {age})";
            int count = sqlCommand.ExecuteNonQuery();
            
        }
        static void update_values(SqlCommand sqlCommand)
        {
            Console.WriteLine("Enter Names");
            string ids = Console.ReadLine();
            string[] names = ids.Split(',');
            Console.WriteLine("Enter Age");
            string age = Console.ReadLine();
            sqlCommand.CommandText = $"UPDATE Persons SET Age = {age} WHERE name IN ('{String.Join("','", names)}');";
            int count = sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"Изменено {count} строк");

        }
        static void update_values_with_param(SqlCommand sqlCommand)
        {
            Console.WriteLine("Enter Names");
            string ids = Console.ReadLine();
            string[] names = ids.Split(',');
            Console.WriteLine("Enter Age");
            string age = Console.ReadLine();

            sqlCommand.Parameters.AddWithValue("@p1", age);
            sqlCommand.Parameters.AddWithValue("@p2",String.Join("','", names));
            sqlCommand.CommandText = $"UPDATE Persons SET Age = @p1 WHERE name IN (@p2);";
            int count = sqlCommand.ExecuteNonQuery();
            Console.WriteLine($"Изменено {count} строк");

        }

        static void Main(string[] args)
        {
            //string dbName = "SharpDemo";// Имя сервера
            //string serverName = "localhost"; // имя базы
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                //sqlConnection = new SqlConnection( //Создаем подключение
                //$"Data Source={serverName};" +
                //$"Initial Catalog={dbName};" +
                //$"Integrated Security=True");
            bool flag = true;
            int count = 0;
            while (flag)
            try
            {
                sqlConnection.Open();//Попытка открыть соединения
       
                    if (count == 0)
                    {
                        Console.WriteLine("Connection is OK!");
                        count++;
                    }

                Console.WriteLine("s,u,i");
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "s":
                        select_values(sqlCommand);
                            sqlConnection.Close();
                        break;
                    case "i":
                        insert_values(sqlCommand);
                            sqlConnection.Close();
                        break;
                    case "u":
                            update_values_with_param(sqlCommand);
                            sqlConnection.Close();
                            break;
                    default:
                        Console.WriteLine("Exit");
                        sqlConnection.Close();
                        flag = false;
                        break;
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    break;
            }


        }
    }
}
