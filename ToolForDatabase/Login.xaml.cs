using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolForDatabase
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		private LoginFunction Function = new LoginFunction();
		private bool WindowsRendered = false;
		private string ConnectionString;

		public Login()
		{
			InitializeComponent();
			LoadServersToCombobox();
		}

		public void LoadServersToCombobox()
		{
			cbx_ServerName.ItemsSource = Function.GetServers();
			cbx_ServerName.SelectedIndex = 0;
		}

		private void cbx_Authentication_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (WindowsRendered)
				if (cbx_Authentication.SelectedIndex == 0)
					panelUA.IsEnabled = false;
				else
					panelUA.IsEnabled = true;
		}

		/// <summary>
		/// Bởi vì sự kiện của control này xảy ra nhưng không truy cập được control khác do chưa rendered đầy đủ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_ContentRendered(object sender, EventArgs e)
		{
			WindowsRendered = true;
		}

		private void btn_Test_Click(object sender, RoutedEventArgs e)
		{
			var thread = new Thread(() =>
			{
				string server = cbx_ServerName.Dispatcher.Invoke(() => cbx_ServerName.Text);
				bool connectable;
				if (cbx_Authentication.Dispatcher.Invoke(() => cbx_Authentication.SelectedIndex == 0))
				{
					connectable = Function.TestConnection(server);
				}
				else
				{
					string username = tbx_Login.Dispatcher.Invoke(() => tbx_Login.Text);
					string password = tbx_Password.Dispatcher.Invoke(() => tbx_Password.Password);
					connectable = Function.TestConnection(server, username, password);
					RememberPassword(username, password);
				}
				if (connectable)
				{
					MessageBox.Show("Connection is OK", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
					LoadDatabasesToCombobox();
				}
				else
					MessageBox.Show("Can't connect to server", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
				progressBar.Dispatcher.Invoke(() => progressBar.Visibility = Visibility.Hidden);
			});
			thread.Start();
			progressBar.Visibility = Visibility.Visible;
		}

		private void cbx_ServerName_LostFocus(object sender, RoutedEventArgs e)
		{
			string serverUserInput = cbx_ServerName.Text.Trim();
			var current = cbx_ServerName.Items.OfType<string>().ToList();
			var contained = current.Where(x => x.ToLower() == serverUserInput.ToLower()).Count() > 0;
			if (!contained)
			{
				current.Add(serverUserInput);
				cbx_ServerName.ItemsSource = current;
			}
		}

		private void btn_Login_Click(object sender, RoutedEventArgs e)
		{
			Function.SaveServer(cbx_ServerName.Items.OfType<string>().ToList());
		}

		private void LoadDatabasesToCombobox()
		{
			cbx_Database.Dispatcher.Invoke(() => cbx_Database.ItemsSource = Function.GetDatabases());
			cbx_Database.Dispatcher.Invoke(() => cbx_Database.SelectedIndex = 0);
			panel_Database.Dispatcher.Invoke(() => panel_Database.IsEnabled = true);
		}

		private void RememberPassword(string username, string password)
		{
			if ((bool)chk_Remember.Dispatcher.Invoke(() => chk_Remember.IsChecked))
			{
				Properties.Settings.Default.Username = username;
				Properties.Settings.Default.Password = password;
				Properties.Settings.Default.Save();
			}
		}

		private void LoadLoginInfo()
		{
			if (Properties.Settings.Default.Username != string.Empty)
			{
				tbx_Login.Text = Properties.Settings.Default.Username;
				tbx_Password.Password = Properties.Settings.Default.Password;
			}
		}

		private void cbx_ServerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			panel_Database.IsEnabled = false;
		}
	}
}
