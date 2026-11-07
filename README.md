
# APBD - Ćwiczenie 2: Projekt obiektowy w C#

## 1. Opis projektu

Aplikacja konsolowa do zarządzania wypożyczalnią sprzętu uczelnianego.

Projekt został wykonany w języku C# z wykorzystaniem platformy .NET w ramach przedmiotu Aplikacje Baz Danych (APBD).

Aplikacja umożliwia rejestrowanie użytkowników i sprzętu, wypożyczanie urządzeń, obsługę zwrotów, naliczanie kar oraz generowanie raportów.

Dane przechowywane są w pamięci aplikacji.

## 2. Funkcjonalności

- Dodawanie studentów i pracowników
- Dodawanie laptopów, projektorów i kamer
- Wyświetlanie wszystkich urządzeń i ich statusów
- Wyświetlanie dostępnego sprzętu
- Wypożyczanie urządzeń
- Sprawdzanie dostępności sprzętu
- Kontrola limitów aktywnych wypożyczeń
- Obsługa zwrotów w terminie i po terminie
- Automatyczne naliczanie kar za opóźnienie
- Oznaczanie sprzętu jako znajdującego się w serwisie
- Wyświetlanie aktywnych wypożyczeń użytkownika
- Wyświetlanie przeterminowanych wypożyczeń
- Generowanie raportu podsumowującego

## 3. Rodzaje użytkowników

System obsługuje dwa typy użytkowników:

- Student - maksymalnie 2 aktywne wypożyczenia
- Employee (pracownik) - maksymalnie 5 aktywnych wypożyczeń

Obie klasy dziedziczą po abstrakcyjnej klasie User.

## 4. Rodzaje sprzętu

System obsługuje trzy rodzaje sprzętu:

- Laptop - pamięć RAM i pojemność dysku
- Projector - jasność w lumenach i rozdzielczość
- Camera - liczba megapikseli i zoom optyczny

Wszystkie klasy dziedziczą po abstrakcyjnej klasie Equipment.

Sprzęt może mieć jeden z trzech statusów:

- Available - dostępny
- Rented - wypożyczony
- InService - znajdujący się w serwisie

Każde urządzenie ma automatycznie nadawany identyfikator.

## 5. Zasady wypożyczeń

Wypożyczenie jest możliwe tylko wtedy, gdy:

1. Użytkownik jest zarejestrowany
2. Sprzęt istnieje w systemie
3. Sprzęt jest dostępny
4. Użytkownik nie przekroczył limitu aktywnych wypożyczeń
5. Czas wypożyczenia jest dodatni

Wypożyczenie zawiera informacje o użytkowniku, urządzeniu, dacie wypożyczenia, terminie zwrotu, faktycznej dacie zwrotu i karze.

Zwrot sprzętu powoduje zakończenie wypożyczenia i przywrócenie statusu Available.

## 6. Naliczanie kar

Za każdy rozpoczęty dzień opóźnienia naliczana jest kara 5 zł.

Przykłady:

- Zwrot w terminie: 0 zł
- Opóźnienie o 2 godziny: 5 zł
- Opóźnienie o 1 dzień: 5 zł
- Opóźnienie o 1 dzień i 3 godziny: 10 zł
- Opóźnienie o 3 dni: 15 zł

Naliczanie kar zostało wydzielone do klasy DailyPenaltyPolicy, która implementuje interfejs IPenaltyPolicy.

Stawkę można zmienić w pliku Policies/DailyPenaltyPolicy.cs.

## 7. Struktura projektu

```text
apbd-cw2-csharp/
├── Enums/
│   └── EquipmentStatus.cs
├── Models/
│   ├── Equipment.cs
│   ├── Laptop.cs
│   ├── Projector.cs
│   ├── Camera.cs
│   ├── User.cs
│   ├── Student.cs
│   ├── Employee.cs
│   └── Rental.cs
├── Policies/
│   ├── IPenaltyPolicy.cs
│   └── DailyPenaltyPolicy.cs
├── Services/
│   ├── EquipmentService.cs
│   ├── UserService.cs
│   ├── RentalService.cs
│   └── ReportService.cs
├── Program.cs
├── EquipmentRental.csproj
├── .gitignore
└── README.md
```

## 8. Architektura aplikacji

Projekt został podzielony na następujące elementy:

### Models

Klasy opisujące dane i stan obiektów domenowych, np. Equipment, User oraz Rental.

### Services

Klasy realizujące logikę biznesową:

- EquipmentService - rejestrowanie sprzętu i zarządzanie jego dostępnością
- UserService - rejestrowanie i wyszukiwanie użytkowników
- RentalService - wypożyczenia, zwroty i sprawdzanie limitów
- ReportService - przygotowywanie raportów

### Policies

Klasy odpowiedzialne za zasady naliczania kar.

### Program.cs

Uruchamia aplikację i prezentuje przygotowany scenariusz demonstracyjny.

## 9. Zastosowane zasady projektowe

### Dziedziczenie i polimorfizm

Klasy Laptop, Projector i Camera dziedziczą po Equipment.

Klasy Student i Employee dziedziczą po User.

Polimorfizm pozwala obsługiwać różne typy sprzętu oraz użytkowników przez wspólne klasy bazowe.

### Kohezja (cohesion)

Każda klasa posiada określony zakres odpowiedzialności.

Przykładowo RentalService obsługuje wypożyczenia, a ReportService przygotowuje raporty.

Pozwala to zachować wysoką kohezję.

### Sprzężenie (coupling)

Zależności między klasami są ograniczane przez podział na modele, serwisy i polityki.

Obiekty serwisów przekazywane są przez konstruktory, zamiast być tworzone wewnątrz innych serwisów.

Ułatwia to zmianę implementacji i testowanie poszczególnych elementów.

### SOLID

SRP (Single Responsibility Principle):
Klasy mają rozdzielone odpowiedzialności, np. obsługa wypożyczeń i generowanie raportów.

OCP (Open/Closed Principle):
Nowy sposób naliczania kar można dodać przez kolejną implementację IPenaltyPolicy bez zmieniania kodu RentalService.

LSP (Liskov Substitution Principle):
Obiekty Student i Employee mogą być wykorzystywane jako User, a poszczególne rodzaje urządzeń jako Equipment.

ISP (Interface Segregation Principle):
IPenaltyPolicy zawiera tylko metodę niezbędną do obliczania kary.

DIP (Dependency Inversion Principle):
RentalService zależy od abstrakcji IPenaltyPolicy, a nie bezpośrednio od klasy DailyPenaltyPolicy.

### Dependency Injection

Zależności między serwisami przekazywane są przez konstruktory.

Pozwala to korzystać ze wspólnych instancji i oddzielać tworzenie obiektów od logiki biznesowej.

## 10. Uruchomienie projektu

Wymagane jest zainstalowane środowisko .NET SDK zgodne z wersją wskazaną w pliku EquipmentRental.csproj.

W terminalu należy przejść do głównego folderu projektu.

Kompilacja:

```bash
dotnet build
```

Uruchomienie:

```bash
dotnet run
```

Program automatycznie wykonuje scenariusz demonstracyjny i wyświetla wyniki w konsoli.

## 11. Scenariusz demonstracyjny

Po uruchomieniu aplikacji wykonywane są następujące operacje:

1. Rejestracja studenta i pracownika
2. Rejestracja pięciu urządzeń
3. Oznaczenie jednego urządzenia jako znajdującego się w serwisie
4. Wyświetlenie listy sprzętu
5. Wyświetlenie dostępnych urządzeń
6. Wykonanie trzech poprawnych wypożyczeń
7. Próba przekroczenia limitu wypożyczeń studenta
8. Próba wypożyczenia zajętego sprzętu
9. Próba wypożyczenia urządzenia znajdującego się w serwisie
10. Wyświetlenie aktywnych wypożyczeń studenta
11. Terminowy zwrot urządzenia
12. Zwrot urządzenia po terminie i naliczenie kary
13. Próba ponownego zwrotu tego samego urządzenia
14. Wyświetlenie przeterminowanych wypożyczeń
15. Wygenerowanie raportu końcowego

W demonstracji zastosowano stałe daty, aby wyniki były powtarzalne.

## 12. Przykładowy raport

Dla przygotowanego scenariusza aplikacja generuje podsumowanie:

- 2 użytkowników
- 5 urządzeń
- 3 dostępne urządzenia
- 1 urządzenie wypożyczone
- 1 urządzenie w serwisie
- 3 wypożyczenia łącznie
- 1 aktywne wypożyczenie
- 2 zakończone wypożyczenia
- 1 aktywne wypożyczenie przeterminowane
- 1 zwrot terminowy
- 1 zwrot po terminie
- Łączna kara: 15 zł

## 13. Obsługa błędów

Aplikacja obsługuje nieprawidłowe operacje przez zgłaszanie wyjątków.

Przykłady:

- Próba wypożyczenia niedostępnego sprzętu
- Przekroczenie limitu aktywnych wypożyczeń
- Próba ponownego zwrotu urządzenia
- Wyszukiwanie nieistniejącego użytkownika lub sprzętu
- Nieprawidłowy czas wypożyczenia
- Nieprawidłowa data zwrotu

Wybrane przypadki są prezentowane w scenariuszu demonstracyjnym

## 14. Ograniczenia projektu

Aplikacja jest programem konsolowym.

Dane przechowywane są wyłącznie w pamięci i nie są zapisywane w bazie danych ani plikach.

Po zakończeniu programu dane są usuwane.

Aplikacja prezentuje przygotowany scenariusz demonstracyjny, bez interaktywnego menu.

## 15. Organizacja pracy

Projekt rozwijano z wykorzystaniem systemu Git.

Zastosowano gałęzie:

- main - główna gałąź projektu
- feature/models - implementacja modeli domenowych
- feature/services - implementacja logiki biznesowej i demonstracji

Zmiany zapisywano w kolejnych commitach opisujących poszczególne etapy implementacji.
