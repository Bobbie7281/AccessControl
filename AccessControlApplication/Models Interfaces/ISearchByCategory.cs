namespace AccessControlApplication.Models
{
    public interface ISearchByCategory
    {

        public string? SearchType { set; get; }
        public int SearchIdValue{ set; get; }
        public string SearchNameValue { set; get; }
        public string SearchIdCardValue { set; get; }
        public bool? GetAllData { set; get; }
    }
}
