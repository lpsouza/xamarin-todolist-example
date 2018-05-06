using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TodoList.Android
{
    [Activity(Label = "Todo App", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        EditText email;
        EditText password;
        Button loginButton;
        ProgressBar spinner;

        Firebase fb = Firebase.GetInstance();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);

            email = FindViewById<EditText>(Resource.Id.emailEditText);
            password = FindViewById<EditText>(Resource.Id.passEditText);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            spinner = FindViewById<ProgressBar>(Resource.Id.spinner);

            loginButton.Click += LoginButton_Click;
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            spinner.Visibility = ViewStates.Visible;
            await fb.Login(email.Text, password.Text);
            if (fb.currentUser != null)
            {
                StartActivity(typeof(MainActivity));
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Erro");
                alert.SetMessage("Usuário ou senha inválidos!");

                alert.SetPositiveButton("OK", (senderAlert, eAlert) => { });

                Dialog alertDialog = alert.Create();
                alertDialog.Show();

            }
            spinner.Visibility = ViewStates.Invisible;
        }
    }
}