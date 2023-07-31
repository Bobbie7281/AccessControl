namespace AccessControlApplication.Models
{
    public class LoggedUser
    {
        public static int currentUser = 0;
        public int CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

    }
}
