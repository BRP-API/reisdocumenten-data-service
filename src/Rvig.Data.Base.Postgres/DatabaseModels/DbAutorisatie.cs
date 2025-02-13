namespace Rvig.Data.Base.Postgres.DatabaseModels
{
    public record DbAutorisatie
    {
        public int afnemer_code { get; set; }
        public short geheimhouding_ind { get; set; }
        public short? adres_vraag_bevoegdheid { get; set; }
        public short? bijzondere_betrekking_kind_verstrekken { get; set; }
        public string? ad_hoc_medium { get; set; }
        public string? ad_hoc_rubrieken { get; set; }
        public string? voorwaarde_regel { get; set; }
    }
}
