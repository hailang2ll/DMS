namespace DMS.Api.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuCompanyModel
    {
        public long Cid { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }
        public List<MenuModel> ChildNodes { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class MenuModel
    {
        public int? Id { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public string IconName { get; set; }
        public string Url { get; set; }
        public List<MenuModel> ChildNodes { get; set; }
    }
}
