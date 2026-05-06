Vertex is a containerised, high-performance microservice architecture built with .NET 10 and React. It demonstrates advanced system design patterns including In-Memory Caching, Rate Limiting, and Relational Analytics.


What was added:

1. Backend Architecture (C# / .NET 9)Vertical Slice Logic: Separated concerns into Controllers, Services (Hashing logic), and Models.Database Integration: Implemented Entity Framework Core with SQLite (and prepared for PostgreSQL in production).Database Indexing: Added a Unique Index on the ShortCode column for \(O(\log n)\) lookup speeds and data integrity.In-Memory Caching: Integrated IMemoryCache to optimize the "hot path" (redirects), reducing database load.Global Exception Middleware: Built a centralized "safety net" that catches all server errors and returns professional, structured JSON responses.Rate Limiting: Implemented the .NET 9 Fixed Window Limiter to protect the API from bot spam and DDoS attacks.Analytics Engine: Created a relational tracking system that logs every click, including Timestamps and User-Agent data.


2. Frontend Architecture (React / TypeScript)Vite + Tailwind CSS v4: Used the latest 2026 standards for a high-performance, modern UI.Custom Hooks (useHistory): Abstracted all state management and data fetching out of the UI components.Service Layer (urlApi): Dedicated API client using the Fetch API for clean, reusable network calls.Smart Polling: Implemented a useEffect strategy in the ResultCard that auto-refreshes click counts when the user switches back to the tab.Modular Components: Split the UI into UrlInput, HistoryList, and ResultCard for better maintainability.


3. Quality & ReliabilityxUnit Testing Suite: Added Unit Tests for the hashing service using [Fact] and [Theory] (Data-Driven testing) to ensure code quality.TypeScript Type-Safety: Used strict interfaces and verbatimModuleSyntax to ensure zero runtime type errors.