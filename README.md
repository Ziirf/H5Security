# H5 Security

## Database
Jeg lavet en database *H5SecurityDb* som har en tabel *Users*. Det er det eneste hvor at der bliver gemt data. Min tabel har følgende struktur. som kan copy pastes hvis den skal testes.
```sql
USE MASTER
GO
CREATE DATABASE H5SecurityDb
GO
USE H5SecurityDb
GO

CREATE TABLE [dbo].[Users](
    [Id] [INT] IDENTITY(1,1) NOT NULL,
    [Username] [NVARCHAR](255) NOT NULL UNIQUE,
    [Password] [NVARCHAR](255) NOT NULL,
    [Salt] [VARBINARY](MAX) NOT NULL,
    [Iterations] [INT] NOT NULL
)
```

## Encryption
Som det kan observeres har jeg lavet en tabel til at gemme Salt og Iterations, det har jeg gjord da jeg bruger PBKDF2(Rfc2898DeriveBytes) hvilket er væsenligt stærker en SHA256, med den ulempe at den er væsenligt langsommere. Dette er en 1 way encryption som er brugt til at gemme kodeord.\
Jeg har dog lavet en funktion kaldt HashSha256, som også virker. Den gør dog ikke brug af Iterations, så den matcher ikke helt op med databasen.\
\
For E2EE har jeg brugt RSA med en key på længden 2048, hvilket giver mig muligheden for at lave en besked på op til 245 karaktere. - Kunne gøre den størrer, men det så jeg ingen grund til da det bare er en demo.\
Jeg valgte RSA da det var den eneste encryption jeg kende til som gjord brug af public/private keys.\
\
\
\
Ved ikke rigtig om der er mere at sige, ellers må du gerne kontakte mig på discord.