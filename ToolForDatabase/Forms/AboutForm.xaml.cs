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
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class AboutForm : Window
	{
		private static AboutForm instance;

		public AboutForm()
		{
			InitializeComponent();
		}

		public static AboutForm Instance
		{
			get
			{
				if (instance == null)
					instance = new AboutForm();
				return instance;
			}
		}
	}
}
