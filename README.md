# Vertex | Decoupled Distributed Architecture

Vertex is a full-stack, Docker-containerised URL shortening service designed for speed, security, and cloud scalability. Built with a modern .NET 10 backend and a React + Tailwind frontend, it leverages a decoupled MVC architecture and distributed system design to provide permanent link history, temporal caching, and real-time click analytics.

---

## 🏛️ System Design & Distributed Architecture Principles Implemented

Vertex was engineered following modern distributed system patterns to ensure scalability and environmental portability:

1. **Decoupled Architecture (SoC)**: The system is split into three distinct layers (Client, Application, and Data) communicating via RESTful APIs. This ensures that the frontend and backend can scale or be updated independently.

2. **Cloud-Native & Containerised**: The backend is packaged via Docker, ensuring "Build Once, Run Anywhere" consistency. The application uses a Universal DB Strategy, automatically switching between SQLite for local development and PostgreSQL for production based on environment detection.

3. **Application-Level Identity (GUIDs)**: To bypass the common "Identity Handshake" failures in distributed databases, Vertex uses String GUIDs generated at the application level. This ensures high write availability and makes the system database-agnostic.

4. **Zero-Trust Security Perimeter & Algorithmic Classification**: Implemented an inline `SecurityPerimeterService` that scans inbound URLs before database execution. By parsing the semantic topology of target links against known spoofing indicators and structural risk metrics, the system calculates real-time threat probabilities. This architecture eliminates integer truncation division errors, lowers edge-case file-extension threats to a 30% boundary constraint, and is fully backed by an automated xUnit and Moq unit testing suite to prevent regression bugs under code updates.

5. **Optimistic UI & State Management**: The frontend implements Optimistic UI updates, allowing links to appear in the history log instantly before the server round-trip is complete, providing a zero-latency user experience.

6. **Traffic & Resilience Management**: Includes Fixed Window Rate Limiting and Proxy Awareness (Forwarded Headers) alongside **Global Exception Handling Middleware** to ensure the service remains secure, standardizes error payloads, and stands stable under high load on cloud providers like Render.

7. **High-Performance B-Tree Data Indexing**: Configured unique database indices on the high-frequency `ShortCode` lookup columns using Entity Framework Fluent API. This optimizes short-link redirections down to O(log N) search latency, completely eliminating expensive, sequential table scans.

8. **Temporal Client-Side Caching (HTTP 302 Redirection)**: Leverages native HTTP protocol rules to implement a pragmatic client-side caching loop. Returning `302 Found` responses instructs client web browsers to temporarily cache the link target path, dropping repeat redirect latency to 0ms while safely preserving backend data mutability lifecycles.

---

## 💻 Tech Stack

* **Frontend**: React (TypeScript), Tailwind CSS, hosted on Vercel.
* **Backend**: ASP.NET Core 10 Web API (Dockerized), hosted on Render.
* **Database**: Aiven PostgreSQL (Production) and SQLite (Local).
* **CI/CD**: Automatic deployment pipelines via GitHub integration.

---

## 🌟 Key Features

1. **Permanent History**: A cloud-hosted PostgreSQL database ensures your "System Activity Log" survives server restarts.
2. **Server Spin-up Detection**: Custom frontend logic to notify users when the free-tier server is waking up.
3. **Clean Redirection**: Seamlessly redirects users from short codes to original destinations with 302 status codes.
4. **Database Projection**: Uses DTO patterns to protect internal schemas and minimize network payloads.
5. **Reusable UI Notification Modals**: Centralized UI alert components that catch destructive actions (deletions) and security violations (blocked vectors) using distinct Tailwind theme styling profiles.

---

## 🧪 Automated Quality Assurance & Testing

Vertex includes an isolated, root-level test project built using **xUnit** and **Moq** to validate core domain logic independently of active database connections or network state.

### Core Coverage Matrices
* **Validation Bounds**: Ensures standard web configurations pass through security validation layers cleanly.
* **Malicious Vectors**: Validates pattern matching against high-risk top-level domains (`.zip`, `.mov`, `.ru`) and credential hijacking syntax configurations.

### Running the Test Engine
To clear old build artifacts and execute the automated test suites from the root directory, open your PowerShell terminal and run:
```powershell
dotnet test
```

---

## 🔍 Manual Verification Guide
To verify the full functionality of the Vertex system live in your browser, follow these testing pipelines:

### 1. Verification of Core Logic
* **Shorten a Link**: Paste a long URL (e.g., `https://google.com`) into the input field and click Shorten.
* **Check the UI**: The new link should appear immediately at the top of the System Activity Log.
* **Verify Redirection**: Click the generated short link. It should open a new tab and successfully navigate to your destination.

### 2. Persistence Test (The "Acid Test") 
* **Refresh the Browser**: Reload the page. The System Activity Log should remain visible with all your previously shortened links.
* **Cross-Browser Check**: Open the site in an Incognito window. Your history will be there, proving the data is stored in the Aiven Cloud and not just local state.

### 3. Error Handling & Resilience
* **Duplicate Prevention**: Try shortening the same URL twice. The system will prevent duplicate entries in the UI.
* **Invalid URLs**: Enter a string that isn't a URL. The system should provide a validation error.
* **Rate Limiting**: Rapidly click shorten 10 times. You should see a "Too many requests" message, confirming the security middleware is active.
* **Firewall Interception Modal**: Paste a known high-risk threat vector link (e.g., `http://signin-spoof-free-crypto-claim-reward.ru`) into the shortener box. The custom **Red Security Modal** will display on your screen, blocking the database entry.
* **Destructive Confirmation Modal**: Click the delete icon button on any history card. An **Amber Warning Modal** will pop up, forcing an explicit confirmation handshake before removing the dataset.
