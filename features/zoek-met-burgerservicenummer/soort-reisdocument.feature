# language: nl

@api @geen-protocollering
Functionaliteit: Soort reisdocument

  Achtergrond:
    Gegeven adres 'A1' heeft de volgende gegevens
    | gemeentecode (92.10) |
    | 0800                 |
    En de persoon met burgerservicenummer '000000024' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) |
    | 0800                              |

  Regel: Soort reisdocument wordt geleverd als code plus de bijbehorende omschrijving uit tabel 48 Nederlands Reisdocument

    Abstract Scenario: soort reisdocument heeft de waarde <code>
      Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
      | naam                        | waarde    |
      | soort reisdocument (35.10)  | <code>    |
      | nummer reisdocument (35.20) | NE3663258 |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000024                  |
      | fields              | soort                      |
      Dan heeft de response een reisdocument met de volgende gegevens
      | naam               | waarde         |
      | soort.code         | <code>         |
      | soort.omschrijving | <omschrijving> |

      Voorbeelden:
      | code | omschrijving                    |
      | PD   | Diplomatiek paspoort            |
      | R1   | Reisdocument ouder1             |
      | TE   | Tweede paspoort (zakenpaspoort) |
      | ..   | Onbekend                        |
    