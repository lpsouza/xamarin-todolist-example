using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;

namespace TodoList.Android
{
    class Database
    {
        static string pasta = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
        static string db = System.IO.Path.Combine(pasta, "TodoList.db");

        public Database()
        {
            try
            {
                using (var conexao = new SQLiteConnection(db))
                {
                    conexao.CreateTable<TodoItem>();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
            }
        }

        public bool Add(TodoItem item)
        {
            try
            {
                using (var conexao = new SQLiteConnection(db))
                {
                    conexao.Insert(item);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool Update(TodoItem item)
        {
            try
            {
                using (var conexao = new SQLiteConnection(db))
                {
                    conexao.Query<TodoItem>("UPDATE TodoItem set Texto=?,Done=? Where Id=?",
                        item.Texto, item.Done, item.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (var conexao = new SQLiteConnection(db))
                {
                    TodoItem item = conexao.Find<TodoItem>(a => a.Id == id);
                    conexao.Delete(item);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<TodoItem> List()
        {
            try
            {
                using (var conexao = new SQLiteConnection(db))
                {
                    return conexao.Table<TodoItem>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public TodoItem Get(int id)
        {
            try
            {
                using (var conexao = new SQLiteConnection(db))
                {
                    return conexao.Table<TodoItem>().Where(a => a.Id == id).FirstOrDefault();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
    }
}