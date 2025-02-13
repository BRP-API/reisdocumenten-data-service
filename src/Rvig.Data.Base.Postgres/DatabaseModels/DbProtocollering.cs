namespace Rvig.Data.Base.Postgres.DatabaseModels
{
    public record DbProtocollering
	{
		public string request_id { get; set; } = string.Empty;
        public DateTime request_datum { get; set; }
        public int? afnemer_code { get; set; }
        public long? pl_id { get; set; }
        public string? request_zoek_rubrieken { get; set; }
        public string? request_gevraagde_rubrieken { get; set; }
        public bool verwerkt { get; set; }
    }
}
