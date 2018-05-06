using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TodoList.Android
{
    class TodoListViewAdapter : BaseAdapter<TodoItem>
    {
        readonly IList<TodoItem> todoListItems;
        readonly Activity context;

        public override TodoItem this[int position]
        {
            get
            {
                return todoListItems[position];
            }
        }

        public TodoListViewAdapter(Activity _context, List<TodoItem> _todoListItems) : base()
        {
            context = _context;
            todoListItems = _todoListItems;
        }

        public override long GetItemId(int position)
        {
            return todoListItems[position].Id;
        }

        public override int Count
        {
            get { return todoListItems.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.TextListView, null);

            TextView textItemView = view.FindViewById<TextView>(Android.Resource.Id.textItem);
            textItemView.Text = todoListItems[position].Texto;
            if (todoListItems[position].Done)
                textItemView.PaintFlags = PaintFlags.StrikeThruText;

            return view;
        }
    }
}
