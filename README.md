# FootballHub

## Setup in Visual Studio

### 1. Apri il progetto
Apri `FootballHub.sln` con Visual Studio 2022.

### 2. Ripristina i pacchetti NuGet
Visual Studio lo fa automaticamente. Se non succede:
Tools > NuGet Package Manager > Package Manager Console:
```
dotnet restore
```

### 3. Crea le tabelle nel database
Package Manager Console:
```
Add-Migration InitialCreate
Update-Database
```

### 4. Avvia
Premi F5.

## Credenziali configurate
- DB: FreedB (sql.freedb.tech)
- API: Sportmonks v3

## Pagine
- `/`           Partite live
- `/Classifica` Classifica Serie A
- `/Partite`    Partite per data
- `/Preferiti`  Squadre preferite (DB)
- `/Pronostici` Pronostici con quote (DB + API)
