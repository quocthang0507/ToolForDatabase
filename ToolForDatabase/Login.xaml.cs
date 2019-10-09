using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public Login()
		{
			InitializeComponent();
			LoadServersToCombobox();
		}

		public void LoadServersToCombobox()
		{
			cbx_ServerName.ItemsSource = Function.LoadServer();
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
			string server = cbx_ServerName.Text;
			if (cbx_Authentication.SelectedIndex == 0)
			{
				if (Function.TestConnection(server))
					MessageBox.Show("Connection is OK", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
				else
					MessageBox.Show("Can't connect to server", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				string username = tbx_Login.Text;
				string password = tbx_Password.Password;
				if (Function.TestConnection(server, username, password))
					MessageBox.Show("Connection is OK", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
				else
					MessageBox.Show("Can't connect to server", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);

			}
		}

		private void cbx_ServerName_LostFocus(object sender, RoutedEventArgs e)
		{
			string ServerUserInput = cbx_ServerName.Text.Trim();
			var contained = cbx_ServerName.Items.OfType<string>().ToList().Where(x => x.ToLower() == ServerUserInput.ToLower()).Count() > 0;
		}
	}
}
