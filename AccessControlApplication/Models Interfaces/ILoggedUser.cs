namespace AccessControlApplication.Models
{
    public interface ILoggedUser
    {
        public bool UserCheck {  get; set; }
        int CurrentUser { get; set; }

        bool AdminRights { get; set; }
        
    }
}
