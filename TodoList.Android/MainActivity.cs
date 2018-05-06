using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using System.Threading.Tasks;

namespace TodoList.Android
{
    [Activity(Label = "TodoList", Theme = "@android:style/Theme.Material.NoActionBar")]
    public class MainActivity : Activity
    {
        private Button novoItemButton;
        private ListView todoListView;

        // Metodo usando SQLite (Local DB)
        //Database db = new Database();

        // Metodo usando o Google Firebase (Serverless DB + Auth)
        Firebase fb = Firebase.GetInstance();
        List<TodoItem> todos;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            novoItemButton = FindViewById<Button>(Resource.Id.novoItemBtn);
            novoItemButton.Click += NovoItemButton_Click;

            todoListView = FindViewById<ListView>(Resource.Id.todoList);
            todoListView.ItemLongClick += TodoListView_ItemLongClick;
            todoListView.ItemClick += TodoListView_ItemClick;

            await ReloadListView();
        }

        private async void TodoListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            TodoItem item = todos[e.Position];
            item.Done = !item.Done;
            await fb.Update(item);
            await ReloadListView();
        }

        private void TodoListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Excluir item");
            alert.SetMessage("Você quer realmente excluir este item?");

            alert.SetPositiveButton("Sim", async (senderAlert, eAlert) => {
                TodoItem item = todos[e.Position];
                await fb.Delete(item);
                await ReloadListView();
            });

            alert.SetNegativeButton("Não", (senderAlert, eAlert) => { });

            Dialog alertDialog = alert.Create();
            alertDialog.Show();
        }

        private void NovoItemButton_Click(object sender, System.EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Novo item");
            alert.SetMessage("Insira um item abaixo.");

            EditText text = new EditText(this);
            text.Hint = "Adicione um item";
            text.SetSingleLine(true);
            alert.SetView(text);

            alert.SetPositiveButton("OK", async (senderAlert, eAlert) => {
                //db.Add(new TodoItem() { Texto = text.Text, Done = false });
                await fb.Add(new TodoItem() { Texto = text.Text, Done = false });
                await ReloadListView();
            });

            alert.SetNegativeButton("Cancelar", (senderAlert, eAlert) => { });

            Dialog alertDialog = alert.Create();
            alertDialog.Show();
        }

        private async Task ReloadListView()
        {
            todos = await fb.List();
            TodoListViewAdapter todoAdapter = new TodoListViewAdapter(this, todos);
            todoListView.Adapter = todoAdapter;
        }
    }
}

