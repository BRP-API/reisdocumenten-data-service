using Rvig.Data.Base.Authorisation;

namespace Rvig.Data.Base.Postgres.DatabaseModels;

public interface IVolgnummer
{
    short volg_nr { get; set; }
}

/// <summary>
/// Combine all different database persoon parts that represent one haalcentraal ingeschreven persoon into one class.
/// </summary>
public class DbPersoonBaseWrapper
{
    [RubriekCategory(1, 51)] public lo3_pl_persoon Persoon { get; set; }
    [RubriekCategory(7), RubriekCategory(13)] public lo3_pl Inschrijving { get; set; }
    [RubriekCategory(6, 56)] public lo3_pl_overlijden Overlijden { get; set; }
    [RubriekCategory(4, 54)] public List<lo3_pl_nationaliteit> Nationaliteiten { get; set; }
    [RubriekCategory(11, 61)] public lo3_pl_gezagsverhouding Gezagsverhouding { get; set; }

    [RubriekCategory(2, 52)] public lo3_pl_persoon Ouder1 { get; set; }
    [RubriekCategory(3, 53)] public lo3_pl_persoon Ouder2 { get; set; }
    [RubriekCategory(5, 55)] public List<lo3_pl_persoon> Partners { get; set; }
    [RubriekCategory(9, 59)] public List<lo3_pl_persoon> Kinderen { get; set; }

    public DbPersoonBaseWrapper()
    {
        Persoon = new lo3_pl_persoon();
        Inschrijving = new lo3_pl();
        Overlijden = new lo3_pl_overlijden();
        Gezagsverhouding = new lo3_pl_gezagsverhouding();

        Ouder1 = new lo3_pl_persoon();
        Ouder2 = new lo3_pl_persoon();
        Nationaliteiten = new List<lo3_pl_nationaliteit>();
        Partners = new List<lo3_pl_persoon>();
        Kinderen = new List<lo3_pl_persoon>();
    }

    public DbPersoonBaseWrapper(lo3_pl_persoon persoon,
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
    }
}