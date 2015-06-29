namespace Jenkins.Api.Client
{
    public class JenkinsCredential
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public static JenkinsCredential Default { get { return new JenkinsCredential(string.Empty, string.Empty); } }

        public JenkinsCredential(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}