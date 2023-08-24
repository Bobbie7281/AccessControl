namespace AccessControlApplication.Models
{
    public class SearchByCategory
    {
        private static string? search = "";
        
        private static bool? getAllData = true;

        public string? Search
        {
            set { search = value; }
            get { return search; }
        }
        
        public bool? GetAllData
        {
            set {  getAllData = value; } 
            get { return getAllData; } 
        }

    }
}
