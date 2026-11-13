# PrintHub Frontend

Frontend application for PrintHub, a 3D printing management platform for tracking:

- Projects
- Filaments
- Printers
- Materials
- Cost calculations
- Production workflows

Built with:

- React
- TypeScript
- Vite
- React Router
- Axios
- Material UI (if applicable)

---

# Prerequisites

Install:

- Node.js 20+
- npm 10+

Verify installations:

```bash
node --version
npm --version
```

---

# Getting Started

Clone the repository and install dependencies:

```bash
npm install
```

Start the development server:

```bash
npm run dev
```

The application will be available at:

```text
http://localhost:5173
```

---

# Backend API

The frontend expects the PrintHub API to be running.

Default backend URL:

```text
https://localhost:7001
```

or

```text
http://localhost:5000
```

depending on environment configuration.

Start the backend before using features that rely on API data.

---

# Available Scripts

Start development server:

```bash
npm run dev
```

Build for production:

```bash
npm run build
```

Preview production build:

```bash
npm run preview
```

Run linting:

```bash
npm run lint
```

---

# Project Structure

```text
src/
├── api/
├── components/
├── layouts/
├── pages/
├── services/
├── hooks/
├── types/
├── routes/
└── App.tsx
```

---

# Current Features

## Completed

- Dashboard framework
- Filament management
- Printer management
- API integration
- Shared layouts/components

## In Progress

- Project cost calculations
- Global settings management

## Planned

- Projects page
- Materials page
- Reporting/dashboard metrics
- Import/export workflows

---

# Development Notes

PrintHub uses a service-driven backend architecture.

Frontend pages generally follow this pattern:

1. API Service
2. DTO/Types
3. Page Component
4. Form/Dialog Components
5. Routing Integration

New entity screens should follow existing Filament and Printer implementations for consistency.