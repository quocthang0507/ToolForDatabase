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
	public partial class LoginForm : Window
	{
		private LoginFunction Function = new LoginFunction();
		private bool WindowsRendered = false;
		private static LoginForm instance;
		private Thread thread_test;

		public LoginForm()
		{
			InitializeComponent();
			LoadServersToCombobox();
		}

		/// <summary>
		/// Sử dụng Singleton để không cần tạo lại lớp nhằm mục đích ẩn/ hiện form
		/// </summary>
		public static LoginForm Instance
		{
			get
			{
				if (instance == null)
					instance = new LoginForm();
				return instance;
			}
		}

		#region Events

		/// <summary>
		/// Bởi vì sự kiện của control này xảy ra nhưng không truy cập được control khác do chưa rendered đầy đủ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_ContentRendered(object sender, EventArgs e)
		{
			WindowsRendered = true;
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
			panel_Database.IsEnabled = false;
		}

		private void cbxAuthentication_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (WindowsRendered)
				if (cbxAuthentication.SelectedIndex == 0)
					panelUA.IsEnabled = false;
				else
					panelUA.IsEnabled = true;
		}

		private void btn_Test_Click(object sender, RoutedEventArgs e)
		{
			progressBar.Visibility = Visibility.Visible;
			imgOK.Visibility = Visibility.Collapsed;
			thread_test = new Thread(() =>
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
			thread_test.Start();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			thread_test.Abort();
		}

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			if (TestConnection())
			{
				Function.SaveServers(cbxServerName.Items.OfType<string>().ToList());
				SaveLoginInfo();
				SaveConnectionString(Function.GetSQLConnectionString());
				this.Visibility = Visibility.Hidden;
				MainForm.Instance.Visibility = Visibility.Visible;
			}
			else
			{
				MessageBox.Show("Can't connect to server", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Error);
				imgOK.Visibility = Visibility.Collapsed;
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Function.SaveServers(cbxServerName.Items.OfType<string>().ToList());
			System.Windows.Application.Current.Shutdown();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Lấy danh sách tên server từ máy và từ file hiển thị ra combobox
		/// </summary>
		private void LoadServersToCombobox()
		{
			cbxServerName.ItemsSource = Function.GetServers();
			cbxServerName.SelectedIndex = 0;
		}

		/// <summary>
		/// Lấy danh sách tên cơ sở dữ liệu hiển thị ra combobox
		/// </summary>
		private void LoadDatabasesToCombobox()
		{
			cbxDatabase.Dispatcher.Invoke(() => cbxDatabase.ItemsSource = Function.GetDatabases());
			cbxDatabase.Dispatcher.Invoke(() => cbxDatabase.SelectedIndex = 0);
			panel_Database.Dispatcher.Invoke(() => panel_Database.IsEnabled = true);
		}

		/// <summary>
		/// Lấy thông tin đăng nhập đã lưu trữ
		/// </summary>
		private void LoadLoginInfo()
		{
			string data = Function.GetLoginInfo();
			if (data != string.Empty)
			{
				string[] info = Function.GetLoginInfo().Split(' ');
				tbxLogin.Text = info[0];
				tbxPassword.Password = info[1];
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
					return Function.TestConnection(server, database);
				return Function.TestConnection(server);
			}
			else
			{
				string username = tbxLogin.Dispatcher.Invoke(() => tbxLogin.Text.Trim());
				string password = tbxPassword.Dispatcher.Invoke(() => tbxPassword.Password.Trim());
				if (database != null)
					return Function.TestConnection(server, database, username, password);
				return Function.TestConnection(server, username, password);
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
				Function.SaveLoginInfo(username, password);
			}
		}

		/// <summary>
		/// Lưu trữ chuỗi kết nối vào chương trình
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
