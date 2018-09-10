using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using FarmaEnlace.Interfaces;
using FarmaEnlace.Models;
using SQLite;


using Xamarin.Forms;

namespace FarmaEnlace.Data
{
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;

        public DataAccess()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FarmaEnlace.db");
            connection = new SQLiteConnection(databasePath);
            connection.CreateTable<Splash>();
            connection.CreateTable<Category>();
            connection.CreateTable<Product>();
            connection.CreateTable<StockProduct>();
            connection.CreateTable<Commerce>();
            connection.CreateTable<Brand>();
            connection.CreateTable<ImageBrand>();
            connection.CreateTable<TokenResponse>();
            connection.CreateTable<StatisticGeneral>();
            connection.CreateTable<StatisticCategory>();
            connection.CreateTable<StatisticProduct>();
        }

        public void Insert<T>(T model)
        {
            connection.InsertWithChildren(model);
        }

        public void Update<T>(T model)
        {
            connection.UpdateWithChildren(model);
        }

        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }

        public T First<T>(bool WithChildren) where T : new()
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().FirstOrDefault();
            }
            else
            {
                return connection.Table<T>().FirstOrDefault();
            }
        }

        public List<T> GetList<T>(bool WithChildren) where T : new()
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().ToList();
            }
            else
            {
                return connection.Table<T>().ToList();
            }
        }

        public T Find<T>(int pk, bool WithChildren) where T : new()
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>()
                                .FirstOrDefault(m => m.GetHashCode() == pk);
            }
            else
            {
                return connection.Table<T>()
                                 .FirstOrDefault(m => m.GetHashCode() == pk);
            }
        }

        public Product Find(int internalCode)
        {
            //return connection.GetAllWithChildren<T>()
            //                   .FirstOrDefault(m => m.GetHashCode() == pk);

            List<Product> query = connection.Query<Product>("Select * From Product where InternalCode == " + internalCode);
            return query.FirstOrDefault();

        }

        internal object Find<T>(Func<object, object> p, bool v) where T : new()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
