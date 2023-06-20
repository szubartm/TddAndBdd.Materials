# language: pl
#http://lucenty.vot.pl/blog/specflow-polskie-slowa-kluczowe/

Właściwość: DayInAuction Po polsku

Scenariusz: Prosty Scenariusz
Zakładając że jest OpenAuction/Beginning
Jeżeli nowe zlecenie zostało otrzymane
Wtedy zlecenie zostało zaakceptowane

Scenariusz: Inny scenariusz
Zakładając że jest OpenAuction/Beginning
Jeżeli nowe zlecenie zostało otrzymane z parametrami
| Field        | Value |
| Symbol       | AZN.L |
| Side         | Buy   |
| Price        | 100   |
| Quantity     | 1000  |
| StrategyType | Day   |
Wtedy zlecenie zostało zaakceptowane
Jeżeli jest 16:31
Wtedy podzlecenie zostało wysłane