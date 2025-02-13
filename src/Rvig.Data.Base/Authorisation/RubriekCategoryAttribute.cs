namespace Rvig.Data.Base.Authorisation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RubriekCategoryAttribute : Attribute
{
    public short Actueel { get; set; }
    public short? Historisch { get; set; }

    public RubriekCategoryAttribute(short actueel, short historisch = -1)
    {
        Actueel = actueel;
        Historisch = historisch == -1 ? null : historisch;
    }
}
