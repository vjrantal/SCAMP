namespace ScampApi.Infrastructure
{
    public interface ILinkHelper
    {
        string Groups();
        string Group(int groupId);
        string GroupResource(int groupId, int resourceId);
        string GroupTemplate(int groupId, int templateId);
        string GroupUser(int groupId, int userId);
        string User();
        string Users();
        string Users(int userId);
    }
}