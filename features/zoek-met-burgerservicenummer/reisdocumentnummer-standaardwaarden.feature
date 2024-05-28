# language: nl

@api @geen-protocollering
Functionaliteit: Reisdocumentnummer met standaardwaarde

  Achtergrond:
    Gegeven adres 'A1' heeft de volgende gegevens
    | gemeentecode (92.10) |
    | 0800                 |
    En de persoon met burgerservicenummer '000000024' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) |
    | 0800                              |

  Regel: Een reisdocumentnummer met standaardwaarde wordt as-is geleverd

    Scenario: Reisdocumentnummer heeft de standaardwaarde
      Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
      | naam                                        | waarde    |
      | soort reisdocument (35.10)                  | PN        |
      | nummer reisdocument (35.20)                 | ......... |
      | datum einde geldigheid reisdocument (35.50) | 20330506  |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000024                  |
      | fields              | reisdocumentnummer         |
      Dan heeft de response een reisdocument met de volgende gegevens
      | naam               | waarde    |
      | reisdocumentnummer | ......... |
