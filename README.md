EMGMSA - Gestion de Location de Voitures
Application web ASP.NET Core pour la gestion d'une flotte de véhicules.
Prérequis

.NET 8.0 SDK
SQL Server
Visual Studio Code ou Visual Studio 2022

Installation

Clonez le repository:

git clone [url-du-repo]
cd EMGMSA

Restaurez les packages:

dotnet restore

Configurez la base de données dans appsettings.json :

jsonCopy"ConnectionStrings": {
    "DefaultConnection": "Server=localhost; Database=EMGMSA; User ID=sa; Password=VotreMotDePasse; MultipleActiveResultSets=true; TrustServerCertificate=True;"
}

Lancez l'application:

dotnet run
Comptes par défaut

Admin : admin@emg.com // Mot de passe: Admin@123
Utilisateur : mya@emg.com // Mot de passe: User@123

Tests
Pour exécuter les tests :
cd EMGMSA/EMGMSA.Tests
dotnet test
