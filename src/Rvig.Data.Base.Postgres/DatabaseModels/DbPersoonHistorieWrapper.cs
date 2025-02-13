using Rvig.Data.Base.Authorisation;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

/// <summary>
/// Combine all different database persoon parts that represent one haalcentraal ingeschreven persoon into one class.
/// </summary>
public class DbPersoonHistorieWrapper : DbPersoonBaseWrapper
{
    [RubriekCategory(8, 58)] public IEnumerable<lo3_pl_verblijfplaats> Verblijfplaatsen { get; set; }
    [RubriekCategory(8, 58)] public IEnumerable<lo3_adres> Adressen { get; set; }
	[RubriekCategory(10, 60)] public IEnumerable<lo3_pl_verblijfstitel> Verblijfstitels { get; set; }

	public DbPersoonHistorieWrapper()
    {
        Inschrijving = new lo3_pl();
        Overlijden = new lo3_pl_overlijden();
        Gezagsverhouding = new lo3_pl_gezagsverhouding();

        Ouder1 = new lo3_pl_persoon();
        Ouder2 = new lo3_pl_persoon();
        Nationaliteiten = new List<lo3_pl_nationaliteit>();
        Partners = new List<lo3_pl_persoon>();
        Kinderen = new List<lo3_pl_persoon>();

        Verblijfplaatsen = new List<lo3_pl_verblijfplaats>();
        Adressen = new List<lo3_adres>();
		Verblijfstitels = new List<lo3_pl_verblijfstitel>();
	}

    public DbPersoonHistorieWrapper(lo3_pl_persoon persoon)
    {
        Persoon = persoon;

        Nationaliteiten = new List<lo3_pl_nationaliteit>();
        Verblijfplaatsen = new List<lo3_pl_verblijfplaats>();
        Adressen = new List<lo3_adres>();
		Verblijfstitels = new List<lo3_pl_verblijfstitel>();
    }

    public DbPersoonHistorieWrapper(lo3_pl_persoon persoon,
           lo3_pl inschrijving,
           lo3_pl_overlijden overlijden,
           lo3_pl_gezagsverhouding gezagsverhouding
       )
    {
        Persoon = persoon;
        Inschrijving = inschrijving;
        Overlijden = overlijden;
        Gezagsverhouding = gezagsverhouding;

        Ouder1 = new lo3_pl_persoon();
        Ouder2 = new lo3_pl_persoon();
        Nationaliteiten = new List<lo3_pl_nationaliteit>();
        Partners = new List<lo3_pl_persoon>();
        Kinderen = new List<lo3_pl_persoon>();

        Verblijfplaatsen = new List<lo3_pl_verblijfplaats>();
        Adressen = new List<lo3_adres>();
        Verblijfstitels = new List<lo3_pl_verblijfstitel>();
    }
}