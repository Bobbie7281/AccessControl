namespace AccessControlApplication.Models
{
    public interface ICombinedClasses
    {
        Register? RegisterUser { get; set; }
        ButtonControls? ButtonSync { get; set; }
        LoggedUser? User { get; set; }
        SearchByCategory? Category { get; set; }
        List<Register>? RegisteredUsers { get; set; }
    }
}
