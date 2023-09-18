namespace AccessControlApplication.Models
{
    public class SearchByCategory : ISearchByCategory
    {
        private static string? searchType = "";
        private static int searchIdValue = 0;
        private static string searchNameValue = "";
        private static string searchIdCardValue = "";
        
        private static bool? getAllData = true;

        public string? SearchType
        {
            set { searchType = value; }
            get { return searchType; }
        }
        public int SearchIdValue 
        {
            set { searchIdValue = value; }
            get { return searchIdValue; }
        }
        public string SearchNameValue
        {
            set { searchNameValue = value; }
            get { return searchNameValue; }
        }
        public string SearchIdCardValue
        {
            set { searchIdCardValue = value; }
            get { return searchIdCardValue; }
        }
        public bool? GetAllData
        {
            set {  getAllData = value; } 
            get { return getAllData; } 
        }

    }
}
