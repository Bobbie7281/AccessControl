namespace AccessControlApplication.Models
{
    public class LoggedUser
    {
        private static int currentUser = 0;
        private static bool adminRights = false;
        public int CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
        public bool AdminRights
        {
            get { return adminRights; }
            set { adminRights = value; }
        }
    }
}
