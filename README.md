# WashFlow – Service Management pentru Stații de Spălare Auto

## Descriere
WashFlow este o aplicație software de tip **REST API**, dezvoltată pentru managementul stațiilor de spălare auto self-service. Sistemul permite administrarea stațiilor și programelor de spălare, gestionarea sesiunilor de utilizare și a tranzacțiilor, precum și generarea de rapoarte de venituri.

Aplicația este destinată utilizării de către administratori, tehnicieni și manageri și poate fi integrată cu terminale POS sau aplicații front-end externe.

---

## Funcționalități principale
- Management stații de spălare (creare, editare, dezactivare, status)
- Gestionare mentenanță stații
- Management programe de spălare
- Pornire, închidere și anulare sesiuni de spălare
- Gestionare tranzacții (numerar / card)
- Generare rapoarte de venituri și top programe
- Validări de business și mesaje de eroare explicite

---

## Tehnologii utilizate
- .NET (ASP.NET Core Web API)
- Entity Framework Core
- SQLite
- Docker & Docker Compose
- Swagger / OpenAPI

---

## Arhitectură
Aplicația este organizată pe straturi:
- **Controllers** – expun endpoint-urile REST
- **Services** – implementează logica de business
- **Data** – acces la baza de date (DbContext, entități, migrații)

Persistența datelor este asigurată printr-un volum Docker.

---

## Rulare locală (fără Docker)

### Cerințe
- .NET SDK 10.0+

### Pași
```bash
dotnet restore
dotnet build
dotnet run
```

Swagger va fi disponibil la:
```
http://localhost:5064/swagger
```

---

## Rulare cu Docker Compose (recomandat)

### Cerințe
- Docker Desktop

### Pornire aplicație
```bash
docker compose up --build
```

Swagger va fi disponibil la:
```
http://localhost:8080/swagger
```

Baza de date SQLite este stocată într-un volum Docker și persistă la restart.

---

## Structura proiectului (simplificat)
```
WashFlow/
 ├── Controllers/
 ├── Services/
 ├── Data/
 ├── DTOs/
 ├── Dockerfile
 ├── docker-compose.yml
 └── README.md
```

---

## Flow demo (exemplu de utilizare)
1. Creare stație (`POST /api/stations`)
2. Creare program de spălare (`POST /api/programs`)
3. Pornire sesiune (`POST /api/sessions/start`)
4. Vizualizare tranzacții (`GET /api/transactions`)
5. Închidere sesiune (`POST /api/sessions/{id}/end`)
6. Vizualizare rapoarte (`GET /api/reports/revenue`)

---

## Limitări cunoscute
- Aplicația nu include autentificare și autorizare
- Nu există interfață grafică (front-end)
- SQLite este utilizat pentru simplitate, nu pentru scalare mare

---

## Posibile extinderi
- Autentificare și roluri (Admin / Operator / Manager)
- Interfață web sau mobilă
- Integrare plăți online
- Migrare la PostgreSQL sau SQL Server

---

## Autori
Proiect realizat de o echipă de 3 studenți, în cadrul disciplinei **Ingineria Sistemelor Software (IS2)**.
