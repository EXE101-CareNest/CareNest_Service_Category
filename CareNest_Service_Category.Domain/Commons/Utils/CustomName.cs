namespace CareNest_Service_Category.Domain.Commons.Utils
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomName : Attribute
    {
        public string Name { get; set; }
        public CustomName(string name)
        {
            Name = name;
        }
    }
}
