namespace DropBox
{
	using DropBox.Model;
	using System.Data.Entity;

	public class UserContext : DbContext
	{

		public UserContext()
			: base("name=UserContext")
		{
		}

		public DbSet<UserModel> Users { set; get; }

	}
}