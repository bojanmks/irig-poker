# irig-poker — Agent Guide

## Project Overview

A web-based multiplayer poker game with a bluffing mechanic. Built with an ASP.NET Core 10 backend (C#) and a React 19 + TypeScript frontend. The actual poker hand logic and game loop have **not yet been implemented** — only lobby, joining, and turn tracking exist.

---

## Game Rules

This is a custom poker variant. Standard poker hand rankings apply with these differences:

- **Straight** is weaker than **Three of a Kind**
- **Flush does not exist** — suit/color is disregarded, *except* in Straight Flush and Royal Flush (which require same suit)
- Hand ranking (strongest to weakest): Royal Flush > Straight Flush > Four of a Kind > Full House > Three of a Kind > Straight > Two Pair > One Pair > High Card

### How a round works

1. **Deal**: Each player receives **one card**.
2. **Claim**: The active player claims what poker hand they hold (they may bluff).
3. **Challenge**: The next player (in turn order) must either:
   - **Up the bet** — claim a stronger hand than the previous claim, OR
   - **Call bluff** — say the previous player is lying.
4. **Resolution**: When a bluff is called, **all players' cards are revealed and combined**. If the claimed hand exists within the combined pool of cards, the claimant is truthful. Otherwise, they were bluffing.
5. **Penalty**: The losing player (the bluffer if caught, or the caller if the claim was truthful) **gains one card**.
6. A player is **eliminated** when they accumulate **7 cards** (configurable in the future).
7. **Winner**: The last player remaining wins.

---

## Technologies

### Frontend

| Tech | Version | Notes |
|---|---|---|
| React | ^19.2.7 | Functional components + hooks only |
| TypeScript | ~6.0.3 | Strict mode enabled |
| Vite | ^8.0.16 | Build tool |
| React Router DOM | ^7.18.0 | Routing |
| Redux Toolkit | ^2.12.0 | State management (3 slices) |
| React Redux | ^9.3.0 | React bindings with typed hooks |
| Axios | ^1.18.0 | HTTP client |
| @microsoft/signalr | ^10.0.0 | Real-time WebSocket client |
| React Hook Form | ^7.80.0 | Form management |
| Zod | ^4.4.3 | Schema validation |
| Tailwind CSS | ^4.3.1 | Utility-first CSS with `@import "tailwindcss"` |
| shadcn/ui | new-york style | Accessible primitives via Radix UI |
| i18next | ^26.3.1 | Internationalization (en + sr) |
| Lucide React | ^1.21.0 | Icons |
| Sonner | ^2.0.7 | Toast notifications |
| ESLint | ^10.5.0 | Linting with import sorting |

### Backend

| Tech | Version | Notes |
|---|---|---|
| .NET | net10.0 | Target framework |
| ASP.NET Core | 10.0 | Web framework |
| FastEndpoints | 7.2.0 | API endpoint library |
| MediatR | 14.1.0 | CQRS mediator |
| FluentValidation | 12.1.1 | Request validation |
| SignalR | built-in | Real-time hubs |
| Swashbuckle | 10.1.1 | Swagger |

---

## Code Conventions

### General

- No tests exist anywhere in the codebase (frontend or backend)
- Pre-commit hook runs `lint-staged` on frontend only (`cd UI && npx --no-install lint-staged`)
- Both frontend and backend use structured localization with parallel JSON locale files

### Frontend (TypeScript / React)

- **File naming**:
  - Components: PascalCase (`GameLobby.tsx`)
  - Pages: PascalCase in `pages/` subdirectory (`GamePage.tsx`)
  - Hooks: camelCase with `use` prefix (`useJoinGame.ts`)
  - Models/Types: PascalCase (`Player.ts`, `PublicGameState.ts`)
  - Slices: camelCase with `Slice` suffix (`gameStateSlice.ts`)
  - Constants: PascalCase (`GamePageState.ts`)
  - Directories: lowercase, kebab-case (`game/components`, `http/clients`)
- **Components**: Functional components only, no `React.FC`. Props typed as interfaces or inline types.
- **Styling**: Tailwind CSS v4 via `cn()` utility (`tailwind-merge` + `clsx`). shadcn/ui uses `cva` for variants. SCSS exists but is unused.
- **State management**: Redux Toolkit `createSlice` with typed `useAppDispatch`/`useAppSelector` hooks. No async thunks — async logic lives in custom hooks.
- **HTTP**: Axios instance with base URL from `VITE_API_URL` env var. SignalR hub client for real-time. Both use standardized `EndpointResponse<T>` / `HubActionResponse<T>` envelopes.
- **Forms**: react-hook-form + Zod via `@hookform/resolvers`. Reusable `DynamicForm` component driven by `FieldConfig[]`.
- **Import ordering** (enforced by `eslint-plugin-simple-import-sort`):
  1. Side-effect imports (`^\u0000`)
  2. Node built-ins (`^node:`)
  3. React / Radix libraries
  4. Other npm packages (`^@?\w`)
  5. Internal aliases (`^@/`)
  6. Parent relative (`^\.\.`)
  7. Sibling relative (`^\.`)
- **Path alias**: `@/` → `./src/`
- **TypeScript strict**: `strict: true`, `noUnusedLocals`, `noUnusedParameters`, `erasableSyntaxOnly`, `verbatimModuleSyntax`

### Backend (C# / .NET)

- **Architecture**: Clean Architecture / Vertical Slicing across 4 projects:
  - `WebApi.Common` — Domain models, enums, Result types (no dependencies)
  - `WebApi.Application` — Interfaces, commands, CQRS abstractions (depends on Common)
  - `WebApi.Implementation` — Concrete implementations, handlers, stores, validators (depends on Application + Common)
  - `WebApi.Api` — Host, DI wiring, endpoints, hubs, modules (depends on all)
- **File-scoped namespaces** everywhere
- **Primary constructors** (C# 12) for DI-heavy classes
- **`record` types** for DTOs/commands
- **Result pattern**: `Result<T>` with `Success()`, `Error()`, `ValidationError()`, `NotFound()` factory methods; implicit conversion from `T`
- **Module pattern**: Every cross-cutting concern implements `IModule` (with `RegisterServices` / `UseServices` / `Priority`), auto-discovered via reflection in `Program.cs`
- **MediatR pipeline behaviors**: Logging → Authorization → Validation
- **In-memory storage**: `ConcurrentDictionary` with `SemaphoreSlim`-based per-game locking
- **Validation**: `FluentValidation` with custom `BaseValidator<T>` base class using `ITranslator` for localized messages
- **SignalR**: Global `IHubFilter` sets locale and connection ID per hub invocation; hubs use `HubActionRequest<T>` / `HubActionResponse<T>` patterns

### What's already built

- Lobby: create game, join game, player list, start game
- Turn tracking: `CurrentTurnPlayerId`, `NextTurn()`, `ShufflePlayerOrder()`
- Real-time updates via SignalR hub
- i18n infrastructure (en + sr locales on frontend and backend)

### What needs to be built

- Card deck generation, shuffling, and dealing
- Player hand management (cards held by each player)
- Claim system (player states a poker hand they claim to have)
- Challenge system (next player ups the bet or calls bluff)
- Bluff resolution (check all players' combined cards for the claimed hand)
- Card accrual (loser gains a card; elimination at 7 cards)
- Win condition detection (last player standing)
- UI for all of the above
