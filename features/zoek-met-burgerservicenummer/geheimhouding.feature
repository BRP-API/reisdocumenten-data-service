# language: nl

@api @geen-protocollering
Functionaliteit: gba geheimhouding leveren bij ZoekMetBurgerservicenummer

  Wanneer een consumer van de Reisdocumenten API gegevens bevraagt van een persoon die geen toestemming heeft gegeven voor het verstrekken van zijn gegevens aan derden
  Dan wil de consumer hiervan worden geattendeerd middels een indicatie bij de gevraagde gegevens

    Achtergrond:
      Gegeven adres 'A1' heeft de volgende gegevens
      | gemeentecode (92.10) |
      | 0800                 |
      Gegeven de persoon met burgerservicenummer '000000152' heeft een 'reisdocument' met de volgende gegevens
      | naam                                        | waarde    |
      | soort reisdocument (35.10)                  | PN        |
      | nummer reisdocument (35.20)                 | NE3663258 |
      | datum einde geldigheid reisdocument (35.50) | 20330506  |
      En de persoon is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) |
      | 0800                              |


  Regel: indicatie geheim waarde 0 wordt niet geleverd

    Scenario: reisdocument van een persoon die toestemming heeft gegeven voor het verstrekken van zijn gegevens aan derden, wordt geraadpleegd
      Gegeven de persoon heeft de volgende 'inschrijving' gegevens
      | naam                     | waarde |
      | indicatie geheim (70.10) | 0      |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | reisdocumentnummer         |
      Dan heeft de response een reisdocument met de volgende gegevens
      | naam               | waarde    |
      | reisdocumentnummer | NE3663258 |


  Regel: indicatie geheim met waarde hoger dan 0 wordt ongevraagd meegeleverd

    Abstract Scenario: persoon met indicatie geheim <waarde>, wordt geraadpleegd met reisdocumentnummer
      Gegeven de persoon heeft de volgende 'inschrijving' gegevens
      | naam                     | waarde   |
      | indicatie geheim (70.10) | <waarde> |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | reisdocumentnummer         |
      Dan heeft de response een reisdocument met de volgende gegevens
      | naam                                 | waarde    |
      | reisdocumentnummer                   | NE3663258 |
      | houder.geheimhoudingPersoonsgegevens | <waarde>  |

      Voorbeelden:
      | waarde |
      | 1      |
      | 2      |
      | 3      |
      | 4      |
      | 5      |
      | 6      |
      | 7      |

    Abstract Scenario: persoon met indicatie geheim en <fields> wordt gevraagd
      Gegeven de persoon heeft de volgende 'inschrijving' gegevens
      | naam                     | waarde |
      | indicatie geheim (70.10) | 7      |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | <fields>                   |
      Dan heeft de response een reisdocument met de volgende gegevens
      | naam                                 | waarde     |
      | <veld 1>                             | <waarde 1> |
      | <veld 2>                             | <waarde 2> |
      | houder.geheimhoudingPersoonsgegevens | 7          |

      Voorbeelden:
      | fields                           | veld 1                     | waarde 1  | veld 2             | waarde 2           |
      | reisdocumentnummer               | reisdocumentnummer         | NE3663258 |                    |                    |
      | soort                            | soort.code                 | PN        | soort.omschrijving | Nationaal paspoort |
      | datumEindeGeldigheid             | datumEindeGeldigheid       | 20330506  |                    |                    |
      | inhoudingOfVermissing.datum      |                            |           |                    |                    |
      | inhoudingOfVermissing.aanduiding |                            |           |                    |                    |
      | houder                           | houder.burgerservicenummer | 000000152 |                    |                    |
      | houder.burgerservicenummer       | houder.burgerservicenummer | 000000152 |                    |                    |


  Regel: geheimhoudingPersoonsgegevens mag niet worden gevraagd, omdat het automatisch wordt geleverd

    @fout-case
    Abstract Scenario: veld geheimhoudingPersoonsgegevens mag niet worden gevraagd, omdat het automatisch wordt geleverd
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | <fields>                   |
      Dan heeft de response de volgende gegevens
      | naam     | waarde                                                      |
      | type     | https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1 |
      | title    | Een of meerdere parameters zijn niet correct.               |
      | status   | 400                                                         |
      | detail   | De foutieve parameter(s) zijn: fields[<index>].             |
      | code     | paramsValidation                                            |
      | instance | /haalcentraal/api/reisdocumenten/reisdocumenten             |
      En heeft de response invalidParams met de volgende gegevens
      | code   | name            | reason                                        |
      | fields | fields[<index>] | Parameter bevat een niet toegestane veldnaam. |

      Voorbeelden:
      | fields                                                                             | index |
      | houder.geheimhoudingPersoonsgegevens                                               | 0     |
      | reisdocumentnummer,houder.geheimhoudingPersoonsgegevens,houder.burgerservicenummer | 1     |
      | soort,inhoudingOfVermissing.datum,houder.geheimhoudingPersoonsgegevens             | 2     |
