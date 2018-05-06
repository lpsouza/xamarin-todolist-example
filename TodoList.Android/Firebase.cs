using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Xamarin.Auth;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace TodoList.Android
{
    class Firebase
    {
        public User currentUser;
        static Firebase fbInstance;
        FirebaseAuthLink link;
        FirebaseAuthProvider auth;
        FirebaseClient db;
        int id;
        string userId;

        private Firebase()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAB8V9aes6DrOJRU2WN0cQEL2znbkOi1n4"));
            db = new FirebaseClient("https://todolist-8b620.firebaseio.com/");
        }

        public static Firebase GetInstance()
        {
            if (fbInstance == null)
            {
                fbInstance = new Firebase();
            }

            return fbInstance;
        }

        public async Task Login(string email, string password)
        {
            try
            {
                link = await auth.SignInWithEmailAndPasswordAsync(email, password);
                currentUser = link.User;
                userId = currentUser.LocalId;
            }
            catch (Exception e)
            {
                Log.Error("FirebaseEx", e.Message);
            }
        }

        public async Task Add(TodoItem item)
        {
            try
            {
                id = await db.Child("autoincrement").WithAuth(link.FirebaseToken).OnceSingleAsync<int>();
                id++;

                await db.Child("todos").Child(userId).Child(id.ToString()).WithAuth(link.FirebaseToken).PutAsync<TodoItem>(item);
                await db.Child("autoincrement").WithAuth(link.FirebaseToken).PutAsync(id.ToString());
            }
            catch (Exception e)
            {
                Log.Error("FirebaseEx", e.Message);
            }
        }

        public async Task Update(TodoItem item)
        {
            try
            {
                await db.Child("todos").Child(userId).Child(item.Id.ToString()).WithAuth(link.FirebaseToken).PutAsync<TodoItem>(item);
            }
            catch (Exception e)
            {
                Log.Error("FirebaseEx", e.Message);
            }
        }

        public async Task Delete(TodoItem item)
        {
            try
            {
                await db.Child("todos").Child(userId).Child(item.Id.ToString()).WithAuth(link.FirebaseToken).DeleteAsync();
            }
            catch (Exception e)
            {
                Log.Error("FirebaseEx", e.Message);
            }
        }

        public async Task<List<TodoItem>> List()
        {
            try
            {
                var fbItems = await db.Child("todos").Child(userId).WithAuth(link.FirebaseToken).OnceAsync<TodoItem>();
                List<TodoItem> todos = new List<TodoItem>();
                foreach (var item in fbItems)
                {
                    TodoItem todo = item.Object;
                    todo.Id = int.Parse(item.Key);
                    todos.Add(todo);
                }
                return todos;
            }
            catch (Exception e)
            {
                Log.Error("FirebaseEx", e.Message);
                return new List<TodoItem>();
            }
        }

    }
}
