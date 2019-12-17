﻿using Business;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ToolForDatabase
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class form_main : Window
	{
		private MainFunction function;
		private string database;
		private string serverName;
		private string loginName;
		private string password;
		private string selectedPath;
		private bool rendered = false;

		public form_main()
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
			Size newSize = e.NewSize;
			double height = newSize.Height;
			double width = newSize.Width;
			// Tree view
			listTables.Height = height * 70 / 100;
			listTables.Width = width * 20 / 100;
			// Text box
			if (height > 250)
				tbxContent.Height = height - 250;
			tbxContent.Width = width * 70 / 100;
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			form_login.Instance.Visibility = Visibility.Visible;
			this.Close();
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult dialog = MessageBox.Show("Do you want to close application?", "Closing form", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (dialog == MessageBoxResult.Yes)
			{
				this.Closing -= Window_Closing; //bypass FormClosing event
				Environment.Exit(1);
			}
		}

		private void cbxDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (rendered)
			{
				LoadTablesToList();
			}
		}

		private void btnCopy_Click(object sender, RoutedEventArgs e)
		{
			string data = tbxContent.Text;
			if (data != "")
			{
				Clipboard.SetText(data);
				MessageBox.Show("Successfully copied", "Copy to clipboard", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				MessageBox.Show("Can't copy with a null value", "Copy to clipboard", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void btnViewCode_Click(object sender, RoutedEventArgs e)
		{
			string @namespace = tbxNamespace.Text;
			string table = tbxTable.Text;
			if (@namespace == "" || table == "")
			{
				MessageBox.Show("Can't generate class because one or more TextBoxes are missing", "Generate class", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			tbxContent.Clear();
			tbxContent.Text = function.GenerateClass(@namespace, table);
		}

		private void btnExport_Click(object sender, RoutedEventArgs e)
		{
			btnViewCode_Click(sender, e);
			string data = tbxContent.Text;
			if (data != "")
			{
				string path = GetSelectedPath();
				if (path == "")
					return;
				string filename = tbxTable.Text + ".cs";
				function.SaveTextToFile(path + "\\" + database, filename, data);
				MessageBox.Show("Successfully saved", "Save to file", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
				MessageBox.Show("Can't export with a null value", "Export to file", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void btnAbout_Click(object sender, RoutedEventArgs e)
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.Show();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			form_login.Instance.Visibility = Visibility.Visible;
		}

		private void listTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		#endregion

		#region Methods

		/// <summary>
		/// Lấy chuỗi kết nối từ form Login thông qua setting của ứng dụng
		/// </summary>
		private void GetInfoFromLoginForm()
		{
			serverName = Properties.Settings.Default.serverName;
			loginName = Properties.Settings.Default.login;
			password = Properties.Settings.Default.password;
			if (loginName == null)
			{
				function = new MainFunction(serverName);
			}
			else
			{
				function = new MainFunction(serverName, loginName, password);
			}
		}

		/// <summary>
		/// Load các bảng có trong cơ sở dữ liệu ra TreeView
		/// </summary>
		private void LoadTablesToList()
		{
			database = cbxDatabase.SelectedItem.ToString();
			function = new MainFunction(serverName, database, loginName, password);
			listTables.ItemsSource = function.GetTables();
		}

		/// <summary>
		/// Lấy đường dẫn thư mục từ hộp thoại
		/// </summary>
		/// <returns></returns>
		private string GetSelectedPath()
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.Description = "Select the specific directory that you want to save";
				if (selectedPath != "")
					dialog.SelectedPath = selectedPath;
				dialog.ShowDialog();
				selectedPath = dialog.SelectedPath;
				return selectedPath;
			}
		}

		private void LoadDatabasesToCombobox()
		{
			cbxDatabase.ItemsSource = function.GetDatabases();
			cbxDatabase.SelectedIndex = 0;
		}

		#endregion
	}
}