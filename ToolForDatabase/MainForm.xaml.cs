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
		private bool Rendered = false;

		public MainForm()
		{
			InitializeComponent();
			GetConnectionString();
		}

		public static MainForm Instance
		{
			get
			{
				//if (instance == null)
				instance = new MainForm();
				return instance;
			}
		}

		private void GetConnectionString()
		{
			Function = new MainFunction(Properties.Settings.Default.ConnectionString);
			Database = Properties.Settings.Default.Database;
		}

		private void LoadDatabases()
		{
			TreeViewItem root = new TreeViewItem();
			root.Header = Database;
			root.ItemsSource = Function.GetTables();
			treeTable.Items.Add(root);
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
			tbxContent.Document.Blocks.Add(new Paragraph(new Run(Function.GenerateClass(tbx_Namespace.Text, tbx_Table.Text))));
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			LoadDatabases();
			Rendered = true;
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

		private void treeTable_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			tbx_Table.Text = treeTable.SelectedItem.ToString();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			this.Hide();
			LoginForm.Instance.Show();
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Size newSize = e.NewSize;
			double height = newSize.Height;
			double width = newSize.Width;
			treeTable.MaxHeight = height - 200;
			dock.MaxHeight = height * 80 / 100;
			dock.Height = height * 80 / 100;
			dock.MaxWidth = width * 80 / 100;
			dock.Width = width * 80 / 100;
		}
	}
}
