using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ToolForDatabase.Forms;
using TreeView;

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
		private bool rendered = false;

		public form_main()
		{
			InitializeComponent();
			GetInfoFromLoginForm();
			LoadDatabasesToCombobox();
			LoadTablesToTreeView();
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
			double width = window.Width;
			// Tree view
			treeTable.Height = height - 180;
			treeTable.Width = width * 0.25;
			// Text box
			tabContent.Height = height - 240;
			rightstack.Width = width * 0.68;
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			form_login.Instance.Visibility = Visibility.Visible;
			this.Closing -= Window_Closing;
			this.Close();
		}

		private void btnAddValues_Click(object sender, RoutedEventArgs e)
		{
			bool isopened = false;
			foreach (Window window in Application.Current.Windows)
			{
				if (window is AddingValuesForm)
				{
					isopened = true;
					window.Activate();
				}
			}
			if (!isopened)
			{
				AddingValuesForm addform = new AddingValuesForm();
				addform.Show();
			}
		}

		private void btnCreateBaseFunctions_Click(object sender, RoutedEventArgs e)
		{
			bool isopened = false;
			foreach (Window window in Application.Current.Windows)
			{
				if (window is CreatingBaseForm)
				{
					isopened = true;
					window.Activate();
				}
			}
			if (!isopened)
			{
				CreatingBaseForm createform = new CreatingBaseForm();
				createform.Show();
			}
		}

		private void cbxDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (rendered)
			{
				LoadTablesToTreeView();
				tabContent.Items.Clear();
			}
		}

		private void btnCopy_Click(object sender, RoutedEventArgs e)
		{
			string data = "";
			try
			{
				data = ((tabContent.SelectedItem as TabItem).Content as ICSharpCode.AvalonEdit.TextEditor).Text;
				if (data == "" || data == null)
					throw new Exception();
			}
			catch (Exception)
			{
				MessageBox.Show("Can't copy with a null value", "Copy to clipboard", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			Clipboard.SetText(data);
			MessageBox.Show("Successfully copied", "Copy to clipboard", MessageBoxButton.OK, MessageBoxImage.Information);

		}

		private void btnViewCode_Click(object sender, RoutedEventArgs e)
		{
			string @namespace = tbxNamespace.Text;
			var selectedTables = GetSelectedTables();
			if (@namespace == "" || selectedTables.Count == 0)
			{
				MessageBox.Show("Can't generate class because NameSpace is missing or No selected tables", "Generate class", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			var treeViewSource = treeTable.ItemsSource as List<TreeViewModel>;
			tabContent.Items.Clear();
			foreach (var item in selectedTables)
			{
				Thread thread = new Thread(() =>
				Application.Current.Dispatcher.Invoke((Action)delegate
				{
					CreateTabWithContent(item, function.GenerateClass(treeViewSource, @namespace, item));
				}
				));
				thread.Start();
			}
		}

		private void btnExport_Click(object sender, RoutedEventArgs e)
		{
			var list = GetAllContents();
			if (list.Count == 0 || tbxPath.Text == "")
			{
				MessageBox.Show("There aren't existing tab or location is null", "Export to file", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			foreach (var @class in list)
			{
				string filename = @class.Key + ".cs";
				function.SaveTextToFile(tbxPath.Text + "\\" + database, filename, @class.Value);
			}
			MessageBox.Show("Save successfully", "Save to file", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void btnBrowse_Click(object sender, RoutedEventArgs e)
		{
			GetSelectedPath();
		}

		private void btnAbout_Click(object sender, RoutedEventArgs e)
		{
			bool isopened = false;
			foreach (Window window in Application.Current.Windows)
			{
				if (window is AboutForm)
				{
					isopened = true;
					window.Activate();
				}
			}
			if (!isopened)
			{
				AboutForm aboutForm = new AboutForm();
				aboutForm.Show();
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MessageBoxResult dialog = MessageBox.Show("Do you want to close application?", "Closing form", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (dialog == MessageBoxResult.Yes)
			{
				this.Closing -= Window_Closing; //bypass FormClosing event
				Environment.Exit(1);
			}
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
		private void LoadTablesToTreeView()
		{
			Thread thread = new Thread(() =>
				Application.Current.Dispatcher.Invoke((Action)delegate
				{
					database = cbxDatabase.SelectedItem.ToString();
					function = new MainFunction(serverName, database, loginName, password);
					treeTable.ItemsSource = function.GetDetailTable(database);
				})
			);
			thread.Start();
		}

		/// <summary>
		/// Lấy đường dẫn thư mục từ hộp thoại
		/// </summary>
		/// <returns></returns>
		private void GetSelectedPath()
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.Description = "Select the specific directory that you want to save";
				dialog.ShowDialog();
				if (dialog.SelectedPath != null)
					tbxPath.Text = dialog.SelectedPath;
			}
		}

		/// <summary>
		/// Hiển thị danh sách cơ sở dữ liệu lên combobox
		/// </summary>
		private void LoadDatabasesToCombobox()
		{
			cbxDatabase.ItemsSource = function.GetDatabases();
			cbxDatabase.SelectedIndex = 0;
		}

		/// <summary>
		/// Tạo một tab mới tương ứng với một bảng
		/// </summary>
		/// <param name="name"></param>
		/// <param name="content"></param>
		private void CreateTabWithContent(string name, string content)
		{
			foreach (TabItem item in tabContent.Items)  //Kiểm tra đã tồn tại một tab cùng tên chưa
			{
				if (name == item.Header.ToString())
					return;
			}
			TabItem subTab = new TabItem();
			ICSharpCode.AvalonEdit.TextEditor textEditor = new ICSharpCode.AvalonEdit.TextEditor();
			textEditor.Name = name;
			textEditor.Text = content;
			textEditor.IsReadOnly = true;
			textEditor.FontSize = 14;
			textEditor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
			textEditor.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
			textEditor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
			textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("C#");
			subTab.Content = textEditor;
			subTab.Header = name;
			subTab.IsSelected = true;
			tabContent.Items.Insert(0, subTab);
		}

		private List<string> GetSelectedTables()
		{
			return function.GetSelectedTables(treeTable.ItemsSource as List<TreeViewModel>);
		}

		private List<string> GetSelectedColumnsInTables(string table)
		{
			List<string> list = new List<string>();

			return list;
		}

		private List<KeyValuePair<string, string>> GetAllContents()
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (TabItem tab in tabContent.Items)
			{
				var text = (tab.Content as ICSharpCode.AvalonEdit.TextEditor).Text;
				var header = tab.Header.ToString();
				list.Add(new KeyValuePair<string, string>(header, text));
			}
			return list;
		}

		#endregion

	}
}
