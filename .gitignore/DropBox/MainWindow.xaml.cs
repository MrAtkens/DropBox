using DropBox.Model;
using Microsoft.Win32;
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

namespace DropBox
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DropBoxAPI api;

		public MainWindow()
		{
			api = new DropBoxAPI();
			InitializeComponent();
		}


		private async Task UploadFile(string filePath, string fileName)
		{
			await api.Upload(fileName, "", filePath);
			DrawGrid();
		}
		

		private async Task DrawGrid()
		{
			var result = await api.ListRootFolder();


			List<string> folders;
			if (!result.TryGetValue("Dictonary", out folders))
			{
				folders = new List<string>();
				folders = result["Directory"];
			}

			List<string> files;
			if (!result.TryGetValue("Dictonary", out files))
			{
				files = new List<string>();
				files = result["Files"];
			}

			ItemDataGrid.AutoGenerateColumns = true;

			List<ModelFiles> objectFiles = new List<ModelFiles>();

			foreach (var i in files)
			{
				ModelFiles file = new ModelFiles();
				file.Name = i;
				objectFiles.Add(file);
			}

			ItemDataGrid.ItemsSource = objectFiles;
		}
		private void ListRefresh(object sender, RoutedEventArgs e)
		{
			DrawGrid();
		}

		private void ButtonUploadClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "All Files (*.*)|*.*";
			openFile.FilterIndex = 1;
			openFile.Multiselect = true;

			if (openFile.ShowDialog() == true)
			{
				string sFileName = openFile.FileName;
				string[] arrAllFiles = openFile.FileNames;

				var result = UploadFile(sFileName, fileName.Text);
			}
		}

		private void transferAuthorizationButtonClick(object sender, RoutedEventArgs e)
		{
			if (singInButton.Visibility == Visibility.Visible)
			{
				singUpButton.Visibility = Visibility.Visible;
				singInButton.Visibility = Visibility.Collapsed;
			}
			else {
				singUpButton.Visibility = Visibility.Collapsed;
				singInButton.Visibility = Visibility.Visible;
			}
		}

		private void SingInButtonClick(object sender, RoutedEventArgs e)
		{
			using (UserContext users = new UserContext())
			{
				foreach (var user in users.Users)
				{
					if (login.Text == user.Login && password.Text == user.Password)
					{
						loginGrid.Visibility = Visibility.Collapsed;
						mainGrid.Visibility = Visibility.Visible;
					}
				}
				login.Text = null;
				password.Text = null;
			}
		}

		private void SingUpButtonClick(object sender, RoutedEventArgs e)
		{
			using (UserContext user = new UserContext())
			{
				user.Users.Add(new UserModel
				{
					Login = login.Text,
					Password = password.Text
				});
				user.SaveChanges();
			}
		}
	}
}
