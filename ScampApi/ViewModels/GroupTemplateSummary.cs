namespace ScampApi.ViewModels
{
    public class GroupTemplateSummary
    {
        public int GroupId { get; set; }
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public string GroupTemplateUrl { get; internal set; }
    }
}
