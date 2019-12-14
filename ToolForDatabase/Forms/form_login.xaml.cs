using Business;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ToolForDatabase
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class form_login : Window
	{
		private LoginFunction function = new LoginFunction();
		private bool rendered = false;
		private Thread thread;

		public static form_login Instance;

		public form_login()
		{
			InitializeComponent();
			LoadServersToCombobox();
			Instance = this;
		}

		#region Events

		/// <summary>
		/// Bởi vì sự kiện của control này xảy ra nhưng không truy cập được control khác do chưa rendered đầy đủ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_ContentRendered(object sender, EventArgs e)
		{
			rendered = true;
			LoadLoginInfo();
		}

		/// <summary>
		/// Lưu lại tên server mà người dùng nhập vào combobox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbxServerName_LostFocus(object sender, RoutedEventArgs e)
		{
			string serverUserInput = cbxServerName.Text.Trim();
			var current = cbxServerName.Items.OfType<string>().ToList();
			var contained = current.Where(x => x.ToLower() == serverUserInput.ToLower()).Count() > 0;
			if (!contained)
			{
				current.Add(serverUserInput);
				cbxServerName.ItemsSource = current;
			}
		}

		private void cbxServerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			panelDatabase.IsEnabled = false;
		}

		private void cbxAuthentication_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (rendered)
				if (cbxAuthentication.SelectedIndex == 0)
					panelUA.IsEnabled = false;
				else
					panelUA.IsEnabled = true;
		}

		private void btn_Test_Click(object sender, RoutedEventArgs e)
		{
			progressBar.Visibility = Visibility.Visible;
			imgOK.Visibility = Visibility.Collapsed;
			thread = new Thread(() =>
			{
				btnCancel.Dispatcher.Invoke(() => btnCancel.Visibility = Visibility.Visible);
				bool connectable = TestConnection();
				if (connectable)
				{
					imgOK.Dispatcher.Invoke(() => imgOK.Visibility = Visibility.Visible);
					LoadDatabasesToCombobox();
				}
				else
				{
					MessageBox.Show("Can't connect to server", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Error);
					imgOK.Dispatcher.Invoke(() => imgOK.Visibility = Visibility.Collapsed);
				}
				progressBar.Dispatcher.Invoke(() => progressBar.Visibility = Visibility.Hidden);
				btnCancel.Dispatcher.Invoke(() => btnCancel.Visibility = Visibility.Collapsed);
			});
			thread.Start();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			thread.Abort();
			progressBar.Visibility = Visibility.Hidden;
			btnCancel.Visibility = Visibility.Collapsed;
			panelDatabase.IsEnabled = false;
		}

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			if (TestConnection())
			{
				function.SaveServers(cbxServerName.Items.OfType<string>().ToList());
				SaveLoginInfo();
				SaveConnectionString(function.GetSQLConnectionString());
				this.Visibility = Visibility.Hidden;
				(new form_main()).Show();
			}
			else
			{
				MessageBox.Show("Can't connect to server", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Error);
				imgOK.Visibility = Visibility.Collapsed;
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MessageBoxResult dialog = MessageBox.Show("Do you want to close application?", "Closing form", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (dialog == MessageBoxResult.Yes)
			{
				function.SaveServers(cbxServerName.Items.OfType<string>().ToList());
				Application.Current.Shutdown();
			}
			else
				e.Cancel = true;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Lấy danh sách tên server từ máy và từ file hiển thị ra combobox
		/// </summary>
		private void LoadServersToCombobox()
		{
			cbxServerName.ItemsSource = function.GetServers();
			cbxServerName.SelectedIndex = 0;
		}

		/// <summary>
		/// Lấy danh sách tên cơ sở dữ liệu hiển thị ra combobox
		/// </summary>
		private void LoadDatabasesToCombobox()
		{
			cbxDatabase.Dispatcher.Invoke(() => cbxDatabase.ItemsSource = function.GetDatabases());
			cbxDatabase.Dispatcher.Invoke(() => cbxDatabase.SelectedIndex = 0);
			panelDatabase.Dispatcher.Invoke(() => panelDatabase.IsEnabled = true);
		}

		/// <summary>
		/// Lấy thông tin đăng nhập đã lưu trữ
		/// </summary>
		private void LoadLoginInfo()
		{
			string data = function.GetLoginInfo();
			if (data != string.Empty)
			{
				string[] info = function.GetLoginInfo().Split(' ');
				tbxLogin.Text = info[0];
				tbxPassword.Password = info[1];
				cbxAuthentication.SelectedIndex = 1;
				chkRemember.IsChecked = true;
			}
		}

		/// <summary>
		/// Kiểm tra kết nối
		/// </summary>
		/// <returns>Kết nối thành công</returns>
		private bool TestConnection()
		{
			string server = cbxServerName.Dispatcher.Invoke(() => cbxServerName.Text.Trim());
			string database = cbxDatabase.Dispatcher.Invoke(() => cbxDatabase.Text.Trim());
			if (cbxAuthentication.Dispatcher.Invoke(() => cbxAuthentication.SelectedIndex == 0))
			{
				if (database != null)
					return function.TestConnection(server, database);
				return function.TestConnection(server);
			}
			else
			{
				string username = tbxLogin.Dispatcher.Invoke(() => tbxLogin.Text.Trim());
				string password = tbxPassword.Dispatcher.Invoke(() => tbxPassword.Password.Trim());
				if (database != null)
					return function.TestConnection(server, database, username, password);
				return function.TestConnection(server, username, password);
			}
		}

		/// <summary>
		/// Lưu trữ thông tin đăng nhập ra file
		/// </summary>
		private void SaveLoginInfo()
		{
			if (cbxAuthentication.SelectedIndex == 1)
			{
				string username = tbxLogin.Dispatcher.Invoke(() => tbxLogin.Text.Trim());
				string password = tbxPassword.Dispatcher.Invoke(() => tbxPassword.Password.Trim());
				function.SaveLoginInfo(username, password);
			}
		}

		/// <summary>
		/// Lưu trữ chuỗi kết nối vào bộ nhớ chương trình
		/// </summary>
		/// <param name="connectionString"></param>
		private void SaveConnectionString(string connectionString)
		{
			Properties.Settings.Default.ConnectionString = connectionString;
			Properties.Settings.Default.Database = cbxDatabase.Text;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Upgrade();
		}

		#endregion

	}
}
