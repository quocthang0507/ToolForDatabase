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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolForDatabase
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainFunction Function;

		public MainWindow()
		{
			InitializeComponent();
			GetConnectionString();
		}

		private void GetConnectionString()
		{
			Function = new MainFunction(Properties.Settings.Default.ConnectionString);
		}

		private void LoadDatabases()
		{

		}

		private void btnCopy_Click(object sender, RoutedEventArgs e)
		{
			TextRange textRange = new TextRange(tbxContent.Document.ContentStart, tbxContent.Document.ContentEnd);
			string data = textRange.Text;
			if (data != "\r\n")
			{
				Clipboard.SetText(data);
				MessageBox.Show("Successfully copied", "Copy", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				MessageBox.Show("Can't copy with a null value", "Copy", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void btnGenerate_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{

		}

		private void btnExport_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MessageBoxResult exit = MessageBox.Show("Do you want to exit this application?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (exit == MessageBoxResult.Yes)
				System.Windows.Application.Current.Shutdown();
			else e.Cancel = true;
		}
	}
}
