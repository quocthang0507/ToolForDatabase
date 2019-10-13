using Business;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ToolForDatabase
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainForm : Window
	{
		private MainFunction Function;
		private string Database;
		private static MainForm instance;
		private string SelectedPath;

		public MainForm()
		{
			InitializeComponent();
			GetConnectionString();
		}

		/// <summary>
		/// Mỗi lần ẩn/ hiện form sẽ cho load lại các control
		/// </summary>
		public static MainForm Instance
		{
			get
			{
				//if (instance == null)
				instance = new MainForm();
				return instance;
			}
		}

		#region Events

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			LoadTables();
			tbxContent.Clear();
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Size newSize = e.NewSize;
			double height = newSize.Height;
			double width = newSize.Width;
			treeTable.MaxHeight = height - 200;
			treeTable.MaxWidth = width * 20 / 100;
			dock.MaxHeight = height * 80 / 100;
			dock.MaxWidth = width * 75 / 100;
			dock.Height = height * 80 / 100;
			dock.Width = width * 75 / 100;
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			this.Visibility = Visibility.Hidden;
			LoginForm.Instance.Visibility = Visibility.Visible;
		}

		private void treeTable_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			string selected = treeTable.SelectedItem.ToString();
			if (!selected.Contains("Header"))   //Bỏ qua tên server
				tbxTable.Text = selected;
			else
				tbxTable.Text = null;
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

		private void btnGenerate_Click(object sender, RoutedEventArgs e)
		{
			string @namespace = tbxNamespace.Text;
			string table = tbxTable.Text;
			if (@namespace == "" || table == "")
			{
				MessageBox.Show("Can't generate class because one or more textbox are missing", "Generate class", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			tbxContent.Clear();
			tbxContent.Text = Function.GenerateClass(@namespace, table);
		}

		private void btnExport_Click(object sender, RoutedEventArgs e)
		{
			string data = tbxContent.Text;
			if (data != "")
			{
				string path = GetSelectedPath();
				string filename = tbxTable.Text + ".cs";
				Function.SaveToFile(path + "\\" + Database, filename, data);
				MessageBox.Show("Successfully saved", "Save to file", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
				MessageBox.Show("Can't export with a null value", "Export to file", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MessageBoxResult exit = System.Windows.MessageBox.Show("Do you want to exit this application?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (exit == MessageBoxResult.Yes)
				System.Windows.Application.Current.Shutdown();
			else e.Cancel = true;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Lấy chuỗi kết nối từ form Login thông qua setting của ứng dụng
		/// </summary>
		private void GetConnectionString()
		{
			Function = new MainFunction(Properties.Settings.Default.ConnectionString);
			Database = Properties.Settings.Default.Database;
		}

		/// <summary>
		/// Load các bảng có trong cơ sở dữ liệu ra TreeView
		/// </summary>
		private void LoadTables()
		{
			TreeViewItem root = new TreeViewItem();
			root.Header = Database;
			root.ItemsSource = Function.GetTables();
			treeTable.Items.Add(root);
		}

		/// <summary>
		/// Lấy đường dẫn thư mục từ hộp thoại
		/// </summary>
		/// <returns></returns>
		private string GetSelectedPath()
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.Description = "Select the directory that you want to save";
				if (SelectedPath != "")
					dialog.SelectedPath = SelectedPath;
				dialog.ShowDialog();
				SelectedPath = dialog.SelectedPath;
				return SelectedPath;
			}
		}

		#endregion

	}
}
