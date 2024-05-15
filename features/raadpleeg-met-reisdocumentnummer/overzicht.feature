#language: nl

@api
Functionaliteit: Raadpleeg met reisdocumentnummer

  Achtergrond:
    Gegeven adres 'A1' heeft de volgende gegevens
    | gemeentecode (92.10) |
    | 0800                 |
    En de persoon met burgerservicenummer '000000152' is ingeschreven op adres 'A1' met de volgende gegevens
    | gemeente van inschrijving (09.10) |
    | 0800                              |

Regel: voor het raadplegen van een reisdocument moet het reisdocumentnummer worden opgegeven

  Scenario: Raadpleeg een reisdocument
    Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
    | naam                                        | waarde    |
    | soort reisdocument (35.10)                  | PN        |
    | nummer reisdocument (35.20)                 | NE3663258 |
    | datum einde geldigheid reisdocument (35.50) | 20240506  |
    Als reisdocumenten wordt gezocht met de volgende parameters
    | naam               | waarde                         |
    | type               | RaadpleegMetReisdocumentnummer |
    | reisdocumentnummer | NE3663258                      |
    | fields             | reisdocumentnummer,houder      |
    Dan heeft de response een reisdocument met de volgende gegevens
    | naam               | waarde    |
    | reisdocumentnummer | NE3663258 |
    En heeft het reisdocument de volgende 'houder' gegevens
    | naam                | waarde    |
    | burgerservicenummer | 000000152 |

  @geen-protocollering
  Scenario: Raadpleeg een niet bestaand reisdocument
    Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
    | naam                                        | waarde    |
    | soort reisdocument (35.10)                  | PN        |
    | nummer reisdocument (35.20)                 | NE3663258 |
    | datum einde geldigheid reisdocument (35.50) | 20240506  |
    Als reisdocumenten wordt gezocht met de volgende parameters
    | naam               | waarde                         |
    | type               | RaadpleegMetReisdocumentnummer |
    | reisdocumentnummer | NE3663259                      |
    | fields             | reisdocumentnummer,houder      |
    Dan heeft de response 0 reisdocumenten
