using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropBox
{
	public class DropBoxAPI
	{
		private DropboxClient dbx;
		private string currentDirectory;
		private string prevDirectory;

		public string CurrentDirectory
		{
			get { return currentDirectory; }

			set
			{
				prevDirectory = currentDirectory;
				currentDirectory = value;
			}
		}

		public string PrevDirectory
		{
			get { return prevDirectory; }
		}

		public DropBoxAPI()
		{
			dbx = new DropboxClient("MFjF9li5oDAAAAAAAAAAEB6VRJ_vRSYESVNQ7ip1YHdnTO6Gz_tv9kpig8v0HvAO");
			currentDirectory = "";
		}

		public async Task<string> Run()
		{
			var full = await dbx.Users.GetCurrentAccountAsync();

			return full.Name.DisplayName;
		}

		public async Task<Dictionary<string, List<string>>> ListRootFolder(string folder = "")
		{
			var list = await dbx.Files.ListFolderAsync(folder);

			var dictonary = new Dictionary<string, List<string>>();

			var allDirectory = new List<string>();
			foreach (var item in list.Entries.Where(i => i.IsFolder))
			{
				allDirectory.Add(item.Name);
			}
			dictonary.Add("Directory", allDirectory);

			var allFiles = new List<string>();
			foreach (var item in list.Entries.Where(i => i.IsFile))
			{
				allFiles.Add(item.Name);
			}
			dictonary.Add("Files", allFiles);

			return dictonary;
		}

		public async Task Upload(string fileName,string folder, string file)
		{
			using (var mem = new MemoryStream(File.ReadAllBytes(file)))
			{
				var updated = await dbx.Files.UploadAsync(
					folder + "/" + fileName,
					WriteMode.Overwrite.Instance,
					body: mem);
			}
		}
	}
}
