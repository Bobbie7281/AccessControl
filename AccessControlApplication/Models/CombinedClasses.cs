namespace AccessControlApplication.Models
{
    public class CombinedClasses : ICombinedClasses
    {
       
        public Register? RegisterUser { get; set; }
        public ButtonControls? ButtonSync { get; set; }
        public LoggedUser ? User { get; set; }
        public SearchByCategory? Category { get; set; }

        public List<Register>? RegisteredUsers { get; set; }
    }
}
