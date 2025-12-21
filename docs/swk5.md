1. Fu r welches Datenmodell haben Sie sich entschieden? ER-Diagramm, etwaige Besonderheiten erklären: Welche Entscheidungen mussten Sie treffen, wofür (und wogegen) haben Sie
sich entschieden und warum?
    ```text
    - Es wurde ein relationelles ER-modell mit den Kernentitäten Customer, Shipment, Address, TrackingEvent, Payment, ShipmentPrice, ContactMethod, NotificationSubscription und Carrier gewählt. Als Primärschlüssel werden durchgehend Int-ids eingesetzt, da sie in Sql-Server performant indizierbar sind und Referenzen in ADO.NET einfch bleiben. Für die externe Identifikation der Sendung exsistiert zusätzlich trackingNumber als UNIQUE-Attribut in Shipment, damit die Kennung schwer zu erraten ist und unabhängig von internen Ids bleibt.
    Addressen sind als eigene Entität modelliert und werden über senderAddressId und recipientAddressId referenziert, um Redundanz zu reduzieren und Wiederverwendung ermöglichen.
    Der aktuelle Status der Pakets wird in Shipment.currentStatus gehalten., während die Historie über TrackingEvent als 1:n Beziehung zu Shipment gespeichert wird, um schnelle Abfragen zu ermöglichen.
    Zahlung sind in Payment separat von Shipment abgebieldet, damit der zweistufige externe Zahlungsflow sauber modelliert und testbar bleibt. Kontakmöglichkeiten sind als 1:n Bezihung zu Customer umgesetzt, und Notifications werden über NotificationSubscribtion als Join-Tabelle realisiert, da ein Kunde mehrere Sendugen abonieren kann und umgekehrt. 
    Carrier ist als separate Tabelle mit apiKey und isActive vorgesehen, um API-Key-Authentifizierung für Versanddienstleister zu unterstützen
    ```
    ![Er](ER.png)

2. Dokumentieren Sie auf Request-Ebene den gesamten Workflow anhand eines durchga ngigen Beispiels. Sie ko nnen ein Tool Ihrer Wahl einsetzen, z. B. Postman Workflows, VS Code,
etc. HTTP-Requests inkl. HTTP-Verb, URL, Parametern, Body und Headern

# AccessToken
```text
Zuerst wird in Postman der Request "GetAccessToken" gegen {{KcUrl}}/realms/{{KcReal}}/protocol/openid-connect/token
ausgeführt. Nach 200 Ok wird die erhaltene Token im Enviroment gespeichert. Danach wird beim jedem Controller-request der [Authorize] diese Token benutzt.
```
![alt text](image.png)
![alt text](image-1.png)
![alt text](image-2.png)

# Customers
- EnsureCurrentCustomer
```text
Für Customers EnsureCurrentCustomer wird ein Post Request {{ApiUrl}}/api/customers erstellt, der Header Authorization: Bearer {{accessToken}} gesetzt und kein Body mitgeschickt.
Im Tab Tests dieses POST-Requests wird ein Script hinterlegt, das pm.environment.set("customerId", pm.response.json().customerId); ausführt, damit die ID für Folge-Requests verfügbar ist.
```
![alt text](image-3.png)

- GetAllCustomers
```text
    Für GetAll wird ein Request GET {{ApiUrl}}/api/customers erstellt und ohne Authorization gesendet, da der Endpoint im Controller nicht mit [Authorize] markiert ist. Es existiert nur für Tests und wird späteer gelöscht.

```
![alt text](image-5.png)
- GetByID
```text
GetById wird ein Request GET {{ApiUrl}}/api/customers/{{customerId}} erstellt und ebenfalls Authorization: Bearer {{accessToken}} gesetzt.
Damit der GetById-Call nicht mit 403 Forbid endet, muss {{customerId}} aus dem vorherigen Ensure-Call stammen, weil der Controller prüft, ob die Token-Sub zur gleichen Customer-ID gehört.
```
![alt text](image-4.png)

# ShipmentPriceCheck
- Calculate
```text
POST auf {{ApiUrl}}/api/shipping/calculate angelegt und kein Bearer Token gesetzt, weil der Endpoint [AllowAnonymous] ist.
Unter Headers wird Content-Type: application/json gesetzt.
```
![alt text](image-6.png)

# Shipment
- Create

```text
POST auf {{ApiUrl}}/api/shipments erstellt, Header Authorization: Bearer {{accessToken}}
```
![alt text](image-7.png)
![alt text](image-8.png)
![alt text](image-9.png)

- paymentUrl
![alt text](image-10.png)


- GetAllForCustomer

```text
GET auf {{ApiUrl}}/api/shipments/{{customerId}}
```
![alt text](image-11.png)


- GetByTrackingNumber

```text
GET auf {{ApiUrl}}/api/shipments/tracking/trackingNumber
```

![alt text](image-12.png)

# Payment
- GetSummary

```text
GET auf {{ApiUrl}}/api/payments/summary?paymentId=paymentId angelegt und im Header Authorization: Bearer {{accessToken}}. Die Variable paymentId wird aus der Response von POST /api/shipments übernommen
```
![alt text](image-13.png)

# Tracking

- GetTrackingStatus

```text
GET auf {{ApiUrl}}/api/tracking/postalCode/trackingNumber
```
![alt text](image-14.png)

# Notification

- GetSubscribtionStatus

```text
GET auf {{ApiUrl}}/api/notifications/subscription/postalCode/trackingNumber
```
![alt text](image-15.png)

- Subscribe

```text
POST auf {{ApiUrl}}/api/notifications/subscribe
```
![alt text](image-17.png)
![alt text](image-16.png)

- UnSubscribe

```text
POST auf {{ApiUrl}}/api/notifications/unsubscribe mit demselben JSON-Body gemacht
```
![alt text](image-18.png)
![alt text](image-19.png)

# Contact
- Create

```text
POST auf {{ApiUrl}}/api/customers/me/contactmethod
```
![alt text](image-23.png)

- GetAll
```text
GET auf {{ApiUrl}}/api/customers/me/contactmethod
```
![alt text](image-24.png)

-Delete
```text
DELETE auf {{ApiUrl}}/api/customers/me/contactmethod/{{contactId}}
```
![alt text](image-25.png)
# Carrier 

- Create
```text
POST auf {{ApiUrl}}/api/carrier/tracking/new
```
![alt text](image-20.png)

- StatusUpdate mit email notification

```text
POST auf {{ApiUrl}}/api/carrier/tracking/status erstellt, im Header zusätzlich X-DeliFHery-Api-Key: DHL-API-00
```
![alt text](image-21.png)
![alt text](image-22.png)



3. Haben Sie im Zuge der Projektarbeit Erfahrungen mit dem Einsatz von KI-Agenten zur Generierung von Sourcecode gesammelt? Welche? Was hat gut funktioniert, wo sind Sie gescheitert?
```text
Es wurden keine KI-Agenten zur direkten Codegenerierung eingesetzt, sondern Implementierung und Debugging erfolgten manuell
```
4. Bei welchen Teilen Ihres Systems ist eine korrekte Funktionsweise aus Sicht von DeliFHery
am wichtigsten? Welche Maßnahmen haben Sie getroffen, um sie zu gewa hrleisten?
```text
Am wichtigsten ist die korrekte Abbildung des Kern-Workflows „Preis berechnen → Sendung anlegen → Zahlungsworkflow starten/abschließen“. Die KernLogik wurde in Services ausgelagert und nicht direkt im Controller implementiert damit sie zentral und konsistnet bleibt.Alle Eingaben werden vor der Verarbeitung validiert und bei Fehlern werden klare HTTP Statuscodes zurückgegeben.Authentifizierung und Autorisierung werden strikt geprüft damit nur berechtigte Nutzer eine Sendung anlegen oder Zahlungsdaten abfragen können
```
5. Wie ko nnen Sie ihr System testen, ohne tatsa chlich Zahlungen im externen Service auszulo sen?
6. Wie haben Sie die Berechnung der Preise umgesetzt? Welche Teile Ihres Codes mu ssen Sie
a ndern, um eine andere oder neue Variante bereitzustellen?
```text
Die Preisberechnung wurde als Rule Based Ansatz umgesetzt bei dem mehrere Regeln nacheinander auf einen Preis Kontext angewendet werden
Der ShippingPriceCalculator bekommt alle IShippingPriceRule Implementierungen per Dependency Injection und summiert deren Effekte zu einem Gesamtpreis
Eine neue Variante wird bereitgestellt indem eine neue Regelklasse erstellt wird die IShippingPriceRule implementiert oder indem bestehende Regeln wie BasePriceRule MonthDiscountRule StateSurChargeRule angepasst werden.
```
7. Die Preisberechnung soll sich ab dem 1. Ja nner, 0:00 auf ein neues Tarifmodell a ndern. Wie
wu rden Sie diese Anforderung lo sen?
8. Welche Ü berlegungen haben Sie beim Generieren der eindeutigen Trackingnummer angestellt?
```text
Bei der Trackingnummer war wichtig dass sie nicht wie eine laufende Nummer wirkt weil man sie sonst leicht erraten könnte deshalb wird sie zufällig generiert
Es wird ein Zeichen Set benutzt das gut lesbar ist und keine nervigen Verwechslungen macht also ohne O I 0 1
Mit 11 Zeichen bleibt die Nummer noch halbwegs kurz aber hat trotzdem extrem viele mögliche Kombinationen
Damit wirklich keine doppelt ist wird jede neue Nummer sofort in der Datenbank gegengecheckt und falls sie schon existiert wird einfach neu generiert
Als Zufall wird ein kryptographisch sicherer Generator genommen damit die Nummern nicht vorhersagbar sind und man nicht einfach durchprobieren kann
```
9. Wie haben Sie den E-Mail-Versand gelo st? Wie abha ngig ist Ihre Implementierung von einem konkreten E-Mail-Versanddienst? Die Marketingabteilung mo chte die Formatierung
der E-Mails zuku nftig selbst vera ndern ko nnen – wie wu rden Sie diese Anforderung lo sen?

```text
Der E Mail Versand ist über ein Interface IEmailSender gelöst und konkret mit MailKit per SMTP implementiert, wobei Host Port Username Passwort und Absender aus der Konfiguration kommen.
Dadurch ist die Anwendung nur an SMTP als Konzept gebunden und ein anderer Anbieter wäre möglich indem man eine zweite Implementierung von IEmailSender schreibt ohne die Business Logik zu ändern.
```
10. Denken Sie an die Skalierbarkeit Ihres Projekts: Die O sterreichische Post mo chte Ihr Produkt mit u ber 500 Millionen Paketen pro Jahr nutzen. Was macht Ihnen am meisten Kopfzerbrechen?
```text
Am meisten Kopfzerbrechen machen die Datenmengen bei TrackingEvents, weil bei 500 Millionen Paketen sehr schnell Milliarden Status Einträge entstehen und jede Abfrage dann teuer wird.
Kritisch sind auch Schreiblast und gleichzeitige Updates durch viele Scanner, weil Status Änderungen in Spitzenzeiten massiv parallel kommen

```
11. Wenn Sie das Projekt neu anfangen wu rden – was wu rden Sie anders machen?

```text
Wenn ich neu anfangen könnte würde ich mehr Zeit für die Struktur des Projektes einplanen und die Architektur früh klar festlegen. Zum Beispiel Interfaces besser trennen.
Ich würde außerdem sofort ein sauberes Docker Deployment planen und umsetzen damit die Startzeit und das Setup nicht so aufwändig sind.
Ich würde ausßerdem noch mehr Kommentare schreiben und bessere Namen vergeben, damit es mehr verständlich ist. Hier wird zum Beispiel Endpoints gemeint.

```

