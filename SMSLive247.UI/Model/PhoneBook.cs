namespace SMSLive247.UI
{
    public class PhoneBook<T> 
    {
        public PhoneBook(ICollection<T> items, int totalNoItemInDB, int pageNo = 1)
        {
            Items = new();
            if(items.Count > 0) {
                Items.AddRange(items);
            }
            
            TotalNoItemInDB = totalNoItemInDB; 
            PageNo = pageNo;
        }

        public List<T> Items { get; set; }
        public int TotalNoItemInDB { get; set; }
        public int PageNo { get; set; }

        /// <summary>
        /// If true, signifies that the data in the "items" property is as a result of a search query.
        /// </summary>
        public bool IsSearchResult { get; set; } = false;
    }
}
