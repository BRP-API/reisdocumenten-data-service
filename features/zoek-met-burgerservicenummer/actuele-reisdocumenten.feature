#language: nl

@api @geen-protocollering
Functionaliteit: zoeken van de actuele reisdocumenten van een persoon met behulp van zijn burgerservicenummer
  Als afnemer
  Wil ik de reisdocumenten kunnen bevragen van een persoon
  Zodat ik kan zien welke reisdocumenten hij volgens de registratie in zijn bezit heeft

    Achtergrond:
      Gegeven adres 'A1' heeft de volgende gegevens
      | gemeentecode (92.10) |
      | 0800                 |
      En de persoon met burgerservicenummer '000000152' is ingeschreven op adres 'A1' met de volgende gegevens
      | gemeente van inschrijving (09.10) |
      | 0800                              |

  Regel: bij zoeken van reisdocumenten met burgerservicenummer worden alleen reisdocumenten geleverd die volgens de registratie nog in het bezit zijn van de persoon

    Abstract Scenario: de persoon heeft een <omschrijving> en heeft geen ander reisdocument
      Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
      | naam                                                                    | waarde       |
      | soort reisdocument (35.10)                                              | PN           |
      | nummer reisdocument (35.20)                                             | NE3663258    |
      | datum uitgifte Nederlands reisdocument (35.30)                          | 20221106     |
      | datum einde geldigheid reisdocument (35.50)                             | 20371106     |
      | datum inhouding dan wel vermissing Nederlands reisdocument (35.60)      | 20221229     |
      | aanduiding inhouding dan wel vermissing Nederlands reisdocument (35.70) | <aanduiding> |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | reisdocumentnummer         |
      Dan heeft de response 0 reisdocumenten

      Voorbeelden:
      | aanduiding | omschrijving            |
      | I          | reisdocument ingeleverd |
      | V          | vermist reisdocument    |

    Abstract Scenario: de persoon heeft een <omschrijving> en heeft geen ander reisdocument
      Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
      | naam                                                                    | waarde       |
      | soort reisdocument (35.10)                                              | PN           |
      | nummer reisdocument (35.20)                                             | NE3663258    |
      | datum uitgifte Nederlands reisdocument (35.30)                          | 20221106     |
      | datum einde geldigheid reisdocument (35.50)                             | 20371106     |
      | datum inhouding dan wel vermissing Nederlands reisdocument (35.60)      | 20221229     |
      | aanduiding inhouding dan wel vermissing Nederlands reisdocument (35.70) | <aanduiding> |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | reisdocumentnummer         |
      Dan heeft de response 1 reisdocumenten

      Voorbeelden:
      | aanduiding | omschrijving                                                |
      | R          | van rechtswege vervallen reisdocument                       |
      | .          | vermist of ingehouden reisdocument met onbekende aanduiding |

    Scenario: de persoon heeft een reisdocument ingeleverd en heeft een ander reisdocument van dezelfde soort nog in bezit
      Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
      | naam                                                                    | waarde    |
      | soort reisdocument (35.10)                                              | PN        |
      | nummer reisdocument (35.20)                                             | NE3663258 |
      | datum uitgifte Nederlands reisdocument (35.30)                          | 20131106  |
      | datum einde geldigheid reisdocument (35.50)                             | 20231106  |
      | datum inhouding dan wel vermissing Nederlands reisdocument (35.60)      | 20221229  |
      | aanduiding inhouding dan wel vermissing Nederlands reisdocument (35.70) | I         |
      En de persoon heeft een 'reisdocument' met de volgende gegevens
      | naam                                           | waarde    |
      | soort reisdocument (35.10)                     | PN        |
      | nummer reisdocument (35.20)                    | NWE45TN71 |
      | datum uitgifte Nederlands reisdocument (35.30) | 20230317  |
      | datum einde geldigheid reisdocument (35.50)    | 20330317  |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | reisdocumentnummer         |
      Dan heeft de response 1 reisdocumenten
      En heeft de response een reisdocument met de volgende gegevens
      | naam               | waarde    |
      | reisdocumentnummer | NWE45TN71 |

  Regel: een reisdocument wordt alleen geleverd wanneer er ten minste één gegeven uit groep 35 een waarde heeft
    # Een standaardwaarde geldt hier als waarde
    # Een reisdocument wordt niet geleverd wanneer alleen gegevens uit groep 36, 81, 82, 83, 85 en/of 86 een waarde hebben

    Scenario: de persoon staat in het Register paspoortsignaleringen
      Gegeven de persoon heeft een 'reisdocument' met de volgende gegevens
      | naam                                           | waarde    |
      | soort reisdocument (35.10)                     | PN        |
      | nummer reisdocument (35.20)                    | NE3663258 |
      | datum uitgifte Nederlands reisdocument (35.30) | 20171106  |
      | datum einde geldigheid reisdocument (35.50)    | 20271106  |
      En de persoon heeft een 'reisdocument' met de volgende gegevens 
      | naam                                                                                   | waarde           |
      | signalering met betrekking tot het verstrekken van een Nederlands reisdocument (36.10) | 1                |
      | gemeente document (82.10)                                                              | 0518             |
      | datum document (82.20)                                                                 | 20040105         |
      | beschrijving document (82.30)                                                          | D27894-2004-A782 |
      | ingangsdatum geldigheid (85.10)                                                        | 20031107         |
      | datum van opneming (86.10)                                                             | 20040112         |
      Als reisdocumenten wordt gezocht met de volgende parameters
      | naam                | waarde                     |
      | type                | ZoekMetBurgerservicenummer |
      | burgerservicenummer | 000000152                  |
      | fields              | reisdocumentnummer         |
      Dan heeft de response 1 reisdocumenten
