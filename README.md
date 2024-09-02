[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/VL9E0aaN)


API Documentation
MoviesController
GET /api/movies
Hämtar en lista över alla filmer.

Parametrar: Inga

GET /api/movies/{id}
Hämtar information om en specifik film baserat på ID.

Parametrar:

id: ID för filmen (Integer)
GET /api/movies/{id}/reviews
Hämtar recensioner för en specifik film baserat på filmens ID.

Parametrar:

id: ID för filmen (Integer)
POST /api/movies
Skapar en ny film.

Parametrar:

title: Filmtitel (String)
releaseYear: År då filmen släpptes (Integer)
PUT /api/movies/{id}
Uppdaterar information om en specifik film baserat på ID.

Parametrar:

id: ID för filmen som ska uppdateras (Integer)
title: Filmtitel (String)
releaseYear: År då filmen släpptes (Integer)
DELETE /api/movies/{id}
Raderar en specifik film baserat på ID.

Parametrar:

id: ID för filmen som ska raderas (Integer)
ReviewsController
GET /api/reviews
Hämtar en lista över alla recensioner.

Parametrar: Inga

GET /api/reviews/{id}
Hämtar information om en specifik recension baserat på ID.

Parametrar:

id: ID för recensionen (Integer)
GET /api/reviews/(user)
Hämtar recensioner för den aktuella användaren.

Parametrar: Inga

POST /api/reviews
Skapar en ny recension.

Parametrar:

movieId: ID för filmen som recensionen gäller (Integer)
comment: Kommentar till recensionen (String)
rating: Betyg för recensionen (Integer)
PUT /api/reviews/{id}
Uppdaterar information om en specifik recension baserat på ID.

Parametrar:

id: ID för recensionen som ska uppdateras (Integer)
movieId: ID för filmen som recensionen gäller (Integer)
comment: Kommentar till recensionen (String)
rating: Betyg för recensionen (Integer)
DELETE /api/reviews/{id}
Raderar en specifik recension baserat på ID.

Parametrar:

id: ID för recensionen som ska raderas (Integer)

## Analys

Detta API hanterar ett register med filmer samt recensioner lagrade i en SQL Server-databas. Projeket är byggt på ASP.NET med Entity Framework för att kommunicera med databasen. Auktoriseringen sköts med ASP.NET Core Identity och HTTPS används för att kryptera trafiken mellan server och klient. För att hantera data används två modeller (Movies, Reviews) samt två DTOs (Data Transfer Objects). Varje modell har en egen controller som ansvarar för respektive endpoints. Async är viktigt för API-programmering då det möjliggör parallell bearbetning och förbättrar svarstider. Det ökar också skalbarheten genom att hantera flera användarbegäranden samtidigt, vilket förbättrar övergripande prestanda.

## Reflektion

Om man vill skala upp projektet och hantera stora mängder data kan "get all movies"-metoden bli tung eftersom den hämtar alla filmer med deras recensioner. Det finns möjligheter till förbättring för att undvika onödig datahantering. Implementering av en metod för att hämta data i hanterbara delar kan vara fördelaktigt, då det inte är lämpligt att visa användaren flera hundra eller tusen inlägg samtidigt. Det är bättre att bygga en metod som bara hämtar de filmer som behövs för att underlätta för både server och klient. Detta kan åstadkommas genom att ha en Get-metod som visar ett visst antal inlägg från ett visst index eller från ett index till ett annat.

Vid skalning av APIet för offentlig användning kan det vara klokt att anpassa "get all movies" endpointen så den bara ger filmer och inte recensioner (när de inte behövs) för att undvika onödig belastning. Implementering av ett caching-system kan vara fördelaktigt om APIet används av många användare som efterfrågar liknande data för att snabba upp återkommande hämtningar.

Eftersom varje request är gratis i detta API, är det viktigt med någon form av Rate Limiting för att undvika överbelastning, antingen medveten eller oavsiktlig, kallat API Rate Limiting Abuse.

För att öka säkerheten använder projektet hashade lösenord för att undvika att de lagras som klartext, samt HTTPS för att kryptera trafiken. Ytterligare förbättringar kan göras genom att "salta" de hashade lösenorden för att öka krypteringens styrka.
