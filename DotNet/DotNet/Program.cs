using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DotNet
{
    public interface IProductDal
    {

        List<Products> GetAllProducts();
        Products GetProductById(int id);
        List<Products> Find(string productName);

        int Count();
        int Create(Products p);
        int Update(Products p);
        int Delete(Products p);

    }

    public class MySQLProductDal : IProductDal
    {

        private MySqlConnection GetMySqlServer()
        {
            string connectionString = @"server=*****;port=****;database=northwind;user=******;password=******";

            return new MySqlConnection(connectionString);


        }
        public int Create(Products p)

        {
            int result = 0;
            using (var connection = GetMySqlServer())


            {

                try
                {
                    connection.Open();
                    string sql = "insert into products (product_name,list_price,discontinued) VALUES (@product_name,@list_price,@discontinued)";
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@product_name", p.Name);
                    command.Parameters.AddWithValue("@list_price", p.Price);
                    command.Parameters.AddWithValue("@discontinued", 1);

                    result = command.ExecuteNonQuery();

                    Console.WriteLine($"{result} adet kayıt eklendi.");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
                finally
                {

                    connection.Close();
                }
            }

            return result;

        }

        public int Delete(Products p)
        {
            int result = 0;
            using (var connection = GetMySqlServer())

            {
                try
                {
                    connection.Open();
                    string sql = "delete from products where id=@id";
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@id", p.ProductId);


                    result = command.ExecuteNonQuery();


                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
                finally
                {

                    connection.Close();
                }
            }

            return result;
        }
        public int Update(Products p)
        {
            int result = 0;
            using (var connection = GetMySqlServer())

            {
                try
                {
                    connection.Open();
                    string sql = "update products set product_name=@productname,list_price=@list_price where id = @id";
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@productname", p.Name);
                    command.Parameters.AddWithValue("@list_price", p.Price);
                    command.Parameters.AddWithValue("@id", p.ProductId);

                    result = command.ExecuteNonQuery();


                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
                finally
                {

                    connection.Close();
                }
            }

            return result;
        }

        public List<Products> GetAllProducts()
        {
            List<Products> products = null;
            using (var connection = GetMySqlServer())
            {

                try
                {

                    connection.Open();
                    string sql = "select * from products";
                    MySqlCommand command = new MySqlCommand(sql, connection);

                    MySqlDataReader reader = command.ExecuteReader();

                    products = new List<Products>();

                    while (reader.Read())
                    {
                        products.Add(
                            new Products
                            {
                                ProductId = int.Parse(reader["id"].ToString()),
                                Name = reader["product_name"].ToString(),
                                Price = double.Parse(reader["list_price"]?.ToString())

                            }

                        );
                        Console.WriteLine($"name :  {reader[3]} price : {reader[6]}");
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {

                    connection.Close();

                }
            }

            return products;
        }

        public Products GetProductById(int id)
        {
            Products product = null;
            using (var connection = GetMySqlServer())
            {

                try
                {

                    connection.Open();
                    string sql = "select * from products where id=@productid";
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.Add("@productid", MySqlDbType.Int32).Value = id;

                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        product = new Products()
                        {
                            ProductId = int.Parse(reader["id"].ToString()),
                            Name = reader["product_name"].ToString(),
                            Price = double.Parse(reader["list_price"]?.ToString())

                        };

                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {

                    connection.Close();

                }
            }

            return product;
        }


        public List<Products> Find(string productName)
        {
            List<Products> products = null;

            using (var connection = GetMySqlServer())
            {

                try
                {

                    connection.Open();
                    string sql = "select * from products where product_name LIKE @productname ";
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.Add("@productname", MySqlDbType.String).Value = "%" + productName + "%";

                    MySqlDataReader reader = command.ExecuteReader();

                    products = new List<Products>();

                    while (reader.Read())
                    {

                        products.Add(
                                new Products
                                {

                                    ProductId = int.Parse(reader["id"].ToString()),
                                    Name = reader["product_name"].ToString(),
                                    Price = double.Parse(reader["list_price"]?.ToString())
                                }

                        );



                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {

                    connection.Close();

                }
            }

            return products;

        }

        public int Count()
        {

            int count = 0;

            using (var connection = GetMySqlServer())
            {

                try
                {

                    connection.Open();
                    string sql = "select count(*) from products";
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {

                        count = Convert.ToInt32(result);
                    }




                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {

                    connection.Close();

                }
            }

            return count;
        }
    }


    public class ProductManager : IProductDal
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {

            _productDal = productDal;

        }

        public int Count()
        {
            return _productDal.Count();
        }

        public int Create(Products p)
        {
            return _productDal.Create(p);
        }

        public int Delete(Products p)
        {
            return _productDal.Delete(p);
        }

        public List<Products> Find(string productName)
        {
            return _productDal.Find(productName);
        }

        public List<Products> GetAllProducts()
        {
            return _productDal.GetAllProducts();
        }

        public Products GetProductById(int id)
        {
            return _productDal.GetProductById(id);
        }

        public int Update(Products p)
        {
            return _productDal.Update(p);
        }
    }

    class Program
    {

        static void Main(string[] args)
        {

            var productDal = new ProductManager(new MySQLProductDal());
            /*
                        var p = new Products()
                        {
                            ProductId = 100,
                            Name = "Samsung S10",
                            Price = 9000


                        };*/

            // var p = productDal.GetProductById(101);
            // p.Name = "Iphone";
            //p.Price = 10000;
            //int count = productDal.Update(p);


            //Console.WriteLine($"Updated products count : {count}");

            var p2 = new Products()
            {
                ProductId = 99
            };

            int count = productDal.Delete(p2);

            Console.WriteLine($"Deleted products count : {count}");

        }

        static MySqlConnection GetMySqlConnection()
        {
            string connectionString = @"server=localhost;port=3306;database=northwind;user=root;password=mysqlserver";

            return new MySqlConnection(connectionString);


        }

    }

}
