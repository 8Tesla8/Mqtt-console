namespace ClassLibrary.Connection
{
    public class Credentials
    {
        public string UserName { get; }
        public string Login { get; }
        public string Password { get; }


        public Credentials(string login, string password) : this(login, login, password)
        {
        }
        
        public Credentials(string userName, string login, string password)
        {
            UserName = userName;
            Login = login;
            Password = password;
        }
        
    }
}