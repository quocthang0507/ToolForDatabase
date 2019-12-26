using Business;
using System;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ToolForDatabase.Forms
{
	/// <summary>
	/// Interaction logic for form_addvalues.xaml
	/// </summary>
	public partial class AddingValuesForm : Window
	{
		private AddFunction function;
		private string serverName;
		private string loginName;
		private string password;
		private string database;
		private bool rendered = false;

		public AddingValuesForm()
		{
			InitializeComponent();
			GetInfoFromLoginForm();
			LoadDatabasesToCombobox();
			LoadTablesToList();
		}


		#region Events
		private void Window_ContentRendered(object sender, EventArgs e)
		{
			rendered = true;
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Size window = e.NewSize;
			double height = window.Height;
			// list tables
			listTable.Height = height - 150;
			listTable.Width = 230;
			// Text box
			dg_Columns.Height = height - 100;
		}

		private void cbxDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (rendered)
				LoadTablesToList();
		}

		private void listTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedTable = listTable.SelectedItem;
			if (selectedTable == null)
				return;
			var columns = function.GetColumns(selectedTable.ToString());
			dg_Columns.ItemsSource = function.DefineTable(columns);
		}

		private void btnInsert_Click(object sender, RoutedEventArgs e)
		{
			var selectedTable = listTable.SelectedItem;
			if (selectedTable == null)
			{
				MessageBox.Show("No selected table to insert values", "Insert To Table", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				bool result = function.InsertValuesToTable(selectedTable.ToString(), dg_Columns.ItemsSource as DataView);
				if (result)
				{
					MessageBox.Show("Insert multiple rows to table successfully", "Insert To Table", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					MessageBox.Show("Insert multiple rows to table unsuccessfully", "Insert To Table", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				function = new AddFunction(serverName, database, loginName, password); //Fix connection string after doing it
			}
		}


		#endregion

		#region Methods
		private void LoadDatabasesToCombobox()
		{
			cbxDatabase.ItemsSource = function.GetDatabases();
			cbxDatabase.SelectedIndex = 0;
		}

		private void GetInfoFromLoginForm()
		{
			serverName = Properties.Settings.Default.serverName;
			loginName = Properties.Settings.Default.login;
			password = Properties.Settings.Default.password;
			if (loginName == null)
			{
				function = new AddFunction(serverName);
			}
			else
			{
				function = new AddFunction(serverName, loginName, password);
			}
		}

		private void LoadTablesToList()
		{
			Thread thread = new Thread(() =>
				Application.Current.Dispatcher.Invoke((Action)delegate
				{
					database = cbxDatabase.SelectedItem.ToString();
					function = new AddFunction(serverName, database, loginName, password);
					listTable.ItemsSource = function.GetTables(database);
				})
			);
			thread.Start();
		}

		#endregion

	}
}
