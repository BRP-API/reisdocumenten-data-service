using Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten;
using Rvig.BrpApi.Shared.Util;

namespace Rvig.BrpApi.Reisdocumenten.Util
{
    public class ReisdocumentenQueryJsonInheritanceConverter : QueryBaseJsonInheritanceConverter
    {
        public ReisdocumentenQueryJsonInheritanceConverter()
        {
        }

        public ReisdocumentenQueryJsonInheritanceConverter(string discriminatorName) : base(discriminatorName)
        {
        }

        public ReisdocumentenQueryJsonInheritanceConverter(Type baseType) : base(baseType)
        {
        }

        public ReisdocumentenQueryJsonInheritanceConverter(string discriminatorName, bool readTypeProperty) : base(discriminatorName, readTypeProperty)
        {
        }

        public ReisdocumentenQueryJsonInheritanceConverter(Type baseType, string discriminatorName) : base(baseType, discriminatorName)
        {
        }

        public ReisdocumentenQueryJsonInheritanceConverter(Type baseType, string discriminatorName, bool readTypeProperty) : base(baseType, discriminatorName, readTypeProperty)
        {
        }

        protected override List<string> _subTypes => new()
        {
            nameof(RaadpleegMetReisdocumentnummer),
            nameof(ZoekMetBurgerservicenummer)
        };
        protected override string _discriminator => nameof(ReisdocumentenQuery.type);
    }
}