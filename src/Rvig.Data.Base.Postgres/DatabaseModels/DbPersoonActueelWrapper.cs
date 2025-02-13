using Rvig.Data.Base.Authorisation;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

/// <summary>
/// Combine all different database persoon parts that represent one haalcentraal ingeschreven persoon into one class.
/// </summary>
public class DbPersoonActueelWrapper : DbPersoonBaseWrapper
{
    [RubriekCategory(8, 58)] public lo3_pl_verblijfplaats Verblijfplaats { get; set; }
    [RubriekCategory(8, 58)] public lo3_adres Adres { get; set; }
	[RubriekCategory(10, 60)] public lo3_pl_verblijfstitel Verblijfstitel { get; set; }

	public DbPersoonActueelWrapper()
    {
        Verblijfplaats = new lo3_pl_verblijfplaats();
        Adres = new lo3_adres();
        Inschrijving = new lo3_pl();
        Overlijden = new lo3_pl_overlijden();
        Gezagsverhouding = new lo3_pl_gezagsverhouding();
        Verblijfstitel = new lo3_pl_verblijfstitel();

        Ouder1 = new lo3_pl_persoon();
        Ouder2 = new lo3_pl_persoon();
        Nationaliteiten = new List<lo3_pl_nationaliteit>();
        Partners = new List<lo3_pl_persoon>();
        Kinderen = new List<lo3_pl_persoon>();
    }

    public DbPersoonActueelWrapper(lo3_pl_persoon persoon,
           lo3_pl_verblijfplaats verblijfplaats,
           lo3_adres adres,
           lo3_pl inschrijving,
           lo3_pl_overlijden overlijden,
           lo3_pl_gezagsverhouding gezagsverhouding,
           lo3_pl_verblijfstitel verblijfstitel
       )
    {
        Persoon = persoon;
        Verblijfplaats = verblijfplaats;
        Adres = adres;
        Inschrijving = inschrijving;
        Overlijden = overlijden;
        Gezagsverhouding = gezagsverhouding;
        Verblijfstitel = verblijfstitel;

        Ouder1 = new lo3_pl_persoon();
        Ouder2 = new lo3_pl_persoon();
        Nationaliteiten = new List<lo3_pl_nationaliteit>();
        Partners = new List<lo3_pl_persoon>();
        Kinderen = new List<lo3_pl_persoon>();
    }
}