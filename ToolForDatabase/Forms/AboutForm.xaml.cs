using System;
using System.Threading;
using System.Windows;

namespace ToolForDatabase
{
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class AboutForm : Window
	{
		public AboutForm()
		{
			InitializeComponent();
			ApplyLanguage(Properties.Settings.Default.Language);
		}

		/// <summary>
		/// Đổi ngôn ngữ giao diện
		/// </summary>
		/// <param name="cultureName"></param>
		void ApplyLanguage(string cultureName = null)
		{
			if (cultureName != null)
				Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
			ResourceDictionary dict = new ResourceDictionary();
			switch (Thread.CurrentThread.CurrentCulture.ToString())
			{
				case "vi-VN":
					dict.Source = new Uri("..\\Languages\\Vietnamese.xaml", UriKind.Relative);
					break;
				case "en-UK":
					dict.Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative);
					break;
			}
			this.Resources.MergedDictionaries.Add(dict);
		}

	}
}
