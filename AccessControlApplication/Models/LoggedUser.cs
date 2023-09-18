namespace AccessControlApplication.Models
{
    public class LoggedUser : ILoggedUser
    {
        private static bool userCheck = false;
        private static int currentUser = 0;
        private static bool adminRights = false;

        public bool UserCheck
        {
            set { userCheck = value; }
            get { return userCheck; }
        }

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
