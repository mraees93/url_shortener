Vertex | High-Performance URL Shortener:

Vertex is a full-stack, containerised URL shortening service designed for speed and reliability. Built with a modern .NET 10 backend and a React + Tailwind frontend, it leverages a decoupled MVC architecture within a distributed system to provide permanent link history and real-time click analytics.



System Design & Microservice Principles implemented:

Vertex was engineered following modern distributed system patterns to ensure scalability and environmental portability:

1. Decoupled Architecture (SoC): The system is split into three distinct layers (Client, Application, and Data) communicating via RESTful APIs. This ensures that the frontend and backend can scale or be updated independently.

2. Cloud-Native & Containerised: The backend is packaged via Docker, ensuring "Build Once, Run Anywhere" consistency. The application uses a Universal DB Strategy, automatically switching between SQLite for local development and PostgreSQL for production based on environment detection.

3. Application-Level Identity (GUIDs): To bypass the common "Identity Handshake" failures in distributed databases, Vertex uses String GUIDs generated at the application level. This ensures high write availability and makes the system database-agnostic.

4. Optimistic UI & State Management: The frontend implements Optimistic UI updates, allowing links to appear in the history log instantly before the server round-trip is complete, providing a zero-latency user experience.

5. Traffic Management: Includes Fixed Window Rate Limiting and Proxy Awareness (Forwarded Headers) to ensure the service remains secure and stable under high load on cloud providers like Render.



Tech Stack:

Frontend: React (TypeScript), Tailwind CSS, hosted on Vercel.
Backend: ASP.NET Core 10 Web API (Dockerized), hosted on Render.
Database: Aiven PostgreSQL (Production) and SQLite (Local).
CI/CD: Automatic deployment pipelines via GitHub integration.



Key Features:

1. Permanent History: A cloud-hosted PostgreSQL database ensures your "System Activity Log" survives server restarts.

2. Server Spin-up Detection: Custom frontend logic to notify users when the free-tier server is waking up.

3. Clean Redirection: Seamlessly redirects users from short codes to original destinations with 302 status codes.

4. Database Projection: Uses DTO patterns to protect internal schemas and minimize network payloads.



Testing Guide:
To verify the full functionality of the Vertex system, follow these steps:

1. Verification of Core LogicShorten a Link: 
Paste a long URL (e.g., https://google.com) into the input field and click Shorten.
Check the UI: The new link should appear immediately at the top of the System Activity Log.
Verify Redirection: Click the generated short link. It should open a new tab and successfully navigate to your destination.

2. Persistence Test (The "Acid Test") 
Refresh the Browser: Reload the page. The System Activity Log should remain visible with all your previously shortened links.
Cross-Browser Check: Open the site in an Incognito window. Your history will be there, proving the data is stored in the Aiven Cloud and not just local state.

3. Error Handling & Resilience
Duplicate Prevention: Try shortening the same URL twice. The system will prevent duplicate entries in the UI.
Invalid URLs: Enter a string that isn't a URL. The system should provide a validation error.
Rate Limiting: Rapidly click shorten 10 times. You should see a "Too many requests" message, confirming the security middleware is active.