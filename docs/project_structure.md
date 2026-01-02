# Deliris Monorepo - Project Structure & Libraries

This document outlines the detailed structure and technology stack for each project within the Deliris monorepo.

---

## Monorepo Structure

```
deliris/
├── backend/                    # .NET Backend Services
│   ├── src/
│   │   ├── Services/
│   │   ├── Common/
│   │   └── Gateway/
│   ├── tests/
│   └── docs/
├── apps/
│   ├── admin-dashboard/        # Next.js Admin Dashboard
│   ├── partner-web/            # Next.js Partner Web App
│   ├── customer-web/           # Next.js Customer Web App
│   ├── partner-mobile/         # React Native Partner Mobile App
│   └── customer-mobile/        # React Native Customer Mobile App
├── packages/                   # Shared packages
│   ├── ui/                     # Shared UI components
│   ├── utils/                  # Shared utilities
│   └── types/                  # Shared TypeScript types
├── docs/                       # Documentation
└── infrastructure/             # Infrastructure as Code

```

---

## 1. Backend Services (.NET Microservices)

### Structure

```
backend/
├── src/
│   ├── Services/
│   │   ├── UserService/
│   │   │   ├── UserService.API/
│   │   │   ├── UserService.Application/
│   │   │   ├── UserService.Domain/
│   │   │   └── UserService.Infrastructure/
│   │   ├── OrderService/
│   │   │   ├── OrderService.API/
│   │   │   ├── OrderService.Application/
│   │   │   ├── OrderService.Domain/
│   │   │   └── OrderService.Infrastructure/
│   │   ├── PaymentService/
│   │   ├── NotificationService/
│   │   ├── LocationService/
│   │   └── PartnerService/
│   ├── Common/
│   │   ├── Common.Domain/
│   │   ├── Common.Application/
│   │   └── Common.Infrastructure/
│   └── Gateway/
│       └── ApiGateway/
├── tests/
│   ├── UnitTests/
│   └── IntegrationTests/
└── docker-compose.yml

```

### Core Libraries & Packages

#### Framework & Runtime
- **.NET 10.0.1**: Latest version with C# 14
- **ASP.NET Core 10.0**: Web API framework

#### Architecture & Patterns
- **MediatR** (v14.0.0): CQRS and mediator pattern implementation
- **AutoMapper** (v16.0.0): Object-to-object mapping
- **FluentValidation** (v12.1.1): Input validation with fluent interface

#### Database & ORM
- **Entity Framework Core** (v10.0.1): ORM for database operations
- **Npgsql.EntityFrameworkCore.PostgreSQL** (v10.0.0): PostgreSQL provider
- **Dapper** (v2.1.66): Micro-ORM for performance-critical queries

#### API & Communication
- **Swashbuckle.AspNetCore** (v10.1.0): Swagger/OpenAPI documentation
- **Microsoft.AspNetCore.SignalR.Client** (v10.0.1): Real-time communication client
- **Refit** (v9.0.2): Type-safe REST client
- **Grpc.AspNetCore** (v2.76.0): gRPC for ASP.NET Core

#### Authentication & Authorization
- **Microsoft.AspNetCore.Authentication.JwtBearer** (v10.0.0): JWT authentication
- **OpenIddict** (v7.2.0): OAuth 2.0 and OpenID Connect server
- **OpenIddict.AspNetCore** (v7.2.0): ASP.NET Core integration
- **OpenIddict.EntityFrameworkCore** (v7.2.0): Entity Framework Core stores
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** (v10.0.1): EF Core provider for Identity

#### Caching & Performance
- **StackExchange.Redis** (v2.10.1): Redis client for caching
- **Microsoft.Extensions.Caching.Memory** (v10.0.1): In-memory caching

#### Logging & Monitoring
- **Serilog** (v4.3.0): Structured logging
- **Serilog.Sinks.Console** (v6.0.0): Console logging
- **Serilog.Sinks.File** (v6.0.0): File logging
- **Serilog.Sinks.Elasticsearch** (v10.0.0): Elasticsearch integration
- **OpenTelemetry** (v1.10.0): Distributed tracing
- **OpenTelemetry.Instrumentation.AspNetCore** (v1.10.1): ASP.NET Core instrumentation

#### Message Queue & Event Bus
- **MassTransit** (v8.5.7): Distributed application framework
- **RabbitMQ.Client** (v7.2.0): RabbitMQ client
- **Azure.Messaging.ServiceBus** (v7.18.2): Azure Service Bus client (optional)

#### Testing
- **xUnit** (v2.9.3): Unit testing framework
- **xunit.runner.visualstudio** (v2.8.2): Visual Studio test runner
- **Moq** (v4.20.72): Mocking framework
- **FluentAssertions** (v8.8.0): Assertion library
- **Testcontainers** (v4.9.0): Integration testing with Docker
- **Microsoft.NET.Test.Sdk** (v17.12.0): Test SDK

#### Utilities
- **Polly** (v8.6.5): Resilience and transient-fault-handling
- **Bogus** (v35.6.5): Fake data generation for testing
- **System.Text.Json** (v10.0.0): High-performance JSON serialization
- **Newtonsoft.Json** (v13.0.3): Alternative JSON serialization (legacy support)

---

## 2. Admin Dashboard (Next.js)

### Structure

```
apps/admin-dashboard/
├── src/
│   ├── app/                    # App Router (Next.js 14+)
│   │   ├── (auth)/
│   │   │   ├── login/
│   │   │   └── layout.tsx
│   │   ├── (dashboard)/
│   │   │   ├── users/
│   │   │   ├── orders/
│   │   │   ├── partners/
│   │   │   ├── analytics/
│   │   │   └── layout.tsx
│   │   ├── api/                # API routes
│   │   ├── layout.tsx
│   │   └── page.tsx
│   ├── components/
│   │   ├── ui/                 # shadcn/ui components
│   │   ├── forms/
│   │   ├── tables/
│   │   └── charts/
│   ├── lib/
│   │   ├── api/                # API client
│   │   ├── utils/
│   │   └── hooks/
│   ├── store/                  # State management
│   ├── types/
│   └── styles/
├── public/
├── package.json
├── tsconfig.json
├── tailwind.config.ts
└── next.config.js

```

### Core Libraries & Packages

#### Framework
- **Next.js** (v16.x): React framework with App Router
- **React** (v19.0.0): UI library
- **TypeScript** (v5.7.2): Type safety

#### UI Components & Styling
- **shadcn/ui** (latest): Reusable component library built on Radix UI
- **@radix-ui/react-*** (v1.1.2): Unstyled, accessible components
- **Tailwind CSS** (v4.0.0): Utility-first CSS framework
- **tailwindcss-animate** (v1.0.7): Animation utilities
- **class-variance-authority** (v0.7.1): Component variants
- **clsx** (v2.1.1): Conditional className utility
- **tailwind-merge** (v2.6.0): Merge Tailwind classes
- **lucide-react** (v0.468.0): Icon library

#### Forms & Validation
- **React Hook Form** (v7.54.2): Form handling
- **Zod** (v3.24.1): Schema validation
- **@hookform/resolvers** (v3.9.1): Form validation resolvers

#### State Management
- **Zustand** (v5.0.2): Lightweight state management
- **@tanstack/react-query** (v5.62.7): Server state management
- **@tanstack/react-query-devtools** (v5.62.7): React Query DevTools

#### Data Fetching & API
- **Axios** (v1.7.9): HTTP client
- **SWR** (v2.2.5): Alternative data fetching hooks

#### Tables & Data Display
- **@tanstack/react-table** (v8.20.6): Headless table library
- **recharts** (v2.15.0): Chart library
- **date-fns** (v4.1.0): Date manipulation

#### Authentication
- **NextAuth.js** (v5.0.0-beta.25): Authentication for Next.js
- **@auth/core** (v0.38.0): Core authentication library
- **jose** (v5.9.6): JWT handling

#### Utilities
- **react-hot-toast** (v2.4.1): Toast notifications
- **sonner** (v1.7.1): Alternative toast library
- **cmdk** (v1.0.4): Command palette
- **react-dropzone** (v14.3.5): File upload
- **nuqs** (v2.2.3): Type-safe URL search params

---

## 3. Partner Web App (Next.js)

### Structure

```
apps/partner-web/
├── src/
│   ├── app/
│   │   ├── (auth)/
│   │   │   ├── login/
│   │   │   └── register/
│   │   ├── (dashboard)/
│   │   │   ├── orders/
│   │   │   ├── menu/
│   │   │   ├── analytics/
│   │   │   ├── settings/
│   │   │   └── layout.tsx
│   │   └── layout.tsx
│   ├── components/
│   │   ├── ui/
│   │   ├── orders/
│   │   ├── menu/
│   │   └── notifications/
│   ├── lib/
│   │   ├── api/
│   │   ├── hooks/
│   │   └── utils/
│   ├── store/
│   └── types/
├── public/
└── package.json

```

### Core Libraries & Packages

#### Framework & Core (Same as Admin Dashboard)
- **Next.js** (v16.x)
- **React** (v19.0.0)
- **TypeScript** (v5.7.2)

#### UI & Styling (Same as Admin Dashboard)
- **shadcn/ui** (latest)
- **Tailwind CSS** (v4.0.0)
- **@radix-ui/react-*** (v1.1.2)
- **lucide-react** (v0.468.0)

#### Forms & State (Same as Admin Dashboard)
- **React Hook Form** (v7.54.2)
- **Zod** (v3.24.1)
- **Zustand** (v5.0.2)
- **@tanstack/react-query** (v5.62.7)

#### Real-Time Features
- **@microsoft/signalr** (v8.0.7): SignalR client for real-time order updates
- **socket.io-client** (v4.8.1): WebSocket client (alternative)

#### Notifications
- **react-hot-toast** (v2.4.1): Toast notifications
- **Push API**: Browser push notifications

#### Additional Features
- **react-qr-code** (v2.0.15): QR code generation
- **html2canvas** (v1.4.1): Screenshot/export functionality
- **jspdf** (v2.5.2): PDF generation for reports

---

## 4. Customer Web App (Next.js)

### Structure

```
apps/customer-web/
├── src/
│   ├── app/
│   │   ├── (marketing)/
│   │   │   ├── page.tsx
│   │   │   └── about/
│   │   ├── (shop)/
│   │   │   ├── restaurants/
│   │   │   ├── stores/
│   │   │   ├── cart/
│   │   │   └── checkout/
│   │   ├── (account)/
│   │   │   ├── profile/
│   │   │   ├── orders/
│   │   │   └── addresses/
│   │   └── layout.tsx
│   ├── components/
│   │   ├── ui/
│   │   ├── restaurant/
│   │   ├── cart/
│   │   ├── checkout/
│   │   └── tracking/
│   ├── lib/
│   │   ├── api/
│   │   ├── hooks/
│   │   └── utils/
│   ├── store/
│   └── types/
├── public/
└── package.json

```

### Core Libraries & Packages

#### Framework & Core (Same as Admin Dashboard)
- **Next.js** (v16.x)
- **React** (v19.0.0)
- **TypeScript** (v5.7.2)

#### UI & Styling (Same as Admin Dashboard)
- **shadcn/ui** (latest)
- **Tailwind CSS** (v4.0.0)
- **@radix-ui/react-*** (v1.1.2)
- **lucide-react** (v0.468.0)
- **framer-motion** (v11.15.0): Animations

#### Maps & Location
- **@react-google-maps/api** (v2.20.3): Google Maps integration
- **mapbox-gl** (v3.8.0): Alternative mapping library
- **use-places-autocomplete** (v4.0.1): Address autocomplete

#### Payment Integration
- **@stripe/stripe-js** (v4.11.0): Stripe payment
- **@stripe/react-stripe-js** (v2.10.0): Stripe React components

#### Real-Time Tracking
- **@microsoft/signalr** (v8.0.7): Real-time order tracking

#### State & Data
- **Zustand** (v5.0.2): State management
- **@tanstack/react-query** (v5.62.7): Server state
- **immer** (v10.1.1): Immutable state updates

#### Forms & Validation
- **React Hook Form** (v7.54.2)
- **Zod** (v3.24.1)

---

## 5. Partner Mobile App (React Native)

### Structure

```
apps/partner-mobile/
├── src/
│   ├── screens/
│   │   ├── auth/
│   │   ├── orders/
│   │   ├── navigation/
│   │   └── profile/
│   ├── components/
│   │   ├── ui/
│   │   ├── orders/
│   │   └── navigation/
│   ├── navigation/
│   ├── services/
│   │   ├── api/
│   │   └── location/
│   ├── store/
│   ├── hooks/
│   ├── utils/
│   └── types/
├── android/
├── ios/
├── app.json
├── package.json
└── tsconfig.json

```

### Core Libraries & Packages

#### Framework
- **React Native** (v0.83.x): Mobile framework
- **Expo** (v56.x): Development platform (optional, recommended)
- **TypeScript** (v5.9.x): Type safety

#### Navigation
- **@react-navigation/native** (v7.0.14): Navigation library
- **@react-navigation/native-stack** (v7.1.10): Stack navigator
- **@react-navigation/bottom-tabs** (v7.1.10): Tab navigator
- **react-native-screens** (v4.4.0): Native navigation primitives
- **react-native-safe-area-context** (v5.1.1): Safe area handling

#### UI Components
- **react-native-paper** (v5.12.5): Material Design components
- **@shopify/restyle** (v2.4.4): Type-safe styling system
- **react-native-vector-icons** (v10.2.0): Icon library
- **react-native-svg** (v15.9.0): SVG support

#### State Management
- **Zustand** (v5.0.2): State management
- **@tanstack/react-query** (v5.62.7): Server state management
- **react-native-mmkv** (v3.1.0): Fast key-value storage

#### Maps & Location
- **react-native-maps** (v1.18.0): Native maps
- **@react-native-community/geolocation** (v3.4.0): Geolocation API
- **react-native-background-geolocation** (v4.17.5): Background location tracking

#### Real-Time Communication
- **@microsoft/signalr** (v8.0.7): SignalR client
- **socket.io-client** (v4.8.1): WebSocket client

#### Notifications
- **@react-native-firebase/app** (v21.5.0): Firebase core
- **@react-native-firebase/messaging** (v21.5.0): Push notifications
- **react-native-push-notification** (v8.1.1): Local notifications
- **@notifee/react-native** (v9.1.2): Advanced notifications

#### Camera & Media
- **react-native-vision-camera** (v4.6.1): Modern camera library
- **expo-camera** (v16.0.9): Expo camera (alternative)
- **react-native-image-picker** (v7.1.2): Image selection
- **react-native-permissions** (v5.1.1): Permission handling

#### Forms & Validation
- **react-hook-form** (v7.54.2): Form handling
- **zod** (v3.24.1): Schema validation

#### HTTP & API
- **axios** (v1.7.9): HTTP client
- **@tanstack/react-query** (v5.62.7): Data fetching

#### Audio & Alerts
- **react-native-sound** (v0.11.2): Sound playback for order alerts
- **react-native-haptic-feedback** (v2.3.3): Haptic feedback

#### Utilities
- **date-fns** (v4.1.0): Date manipulation
- **react-native-reanimated** (v3.16.4): Animations
- **react-native-gesture-handler** (v2.21.2): Gesture handling
- **react-native-keychain** (v9.0.1): Secure storage

---

## 6. Customer Mobile App (React Native)

### Structure

```
apps/customer-mobile/
├── src/
│   ├── screens/
│   │   ├── auth/
│   │   ├── home/
│   │   ├── restaurant/
│   │   ├── cart/
│   │   ├── checkout/
│   │   ├── tracking/
│   │   └── profile/
│   ├── components/
│   │   ├── ui/
│   │   ├── restaurant/
│   │   ├── cart/
│   │   └── tracking/
│   ├── navigation/
│   ├── services/
│   │   ├── api/
│   │   └── location/
│   ├── store/
│   ├── hooks/
│   ├── utils/
│   └── types/
├── android/
├── ios/
├── app.json
└── package.json

```

### Core Libraries & Packages

#### Framework (Same as Partner Mobile)
- **React Native** (v0.83.x)
- **Expo** (v56.x)
- **TypeScript** (v5.9.x)

#### Navigation (Same as Partner Mobile)
- **@react-navigation/native** (v7.0.14)
- **@react-navigation/native-stack** (v7.1.10)
- **@react-navigation/bottom-tabs** (v7.1.10)

#### UI Components (Same as Partner Mobile)
- **react-native-paper** (v5.12.5)
- **react-native-vector-icons** (v10.2.0)
- **react-native-svg** (v15.9.0)

#### Maps & Location (Same as Partner Mobile)
- **react-native-maps** (v1.18.0)
- **@react-native-community/geolocation** (v3.4.0)

#### Payment Integration
- **@stripe/stripe-react-native** (v0.40.1): Stripe payment
- **react-native-iap** (v12.15.6): In-app purchases (if needed)

#### Real-Time Tracking (Same as Partner Mobile)
- **@microsoft/signalr** (v8.0.7)

#### State & Data (Same as Partner Mobile)
- **Zustand** (v5.0.2)
- **@tanstack/react-query** (v5.62.7)
- **react-native-mmkv** (v3.1.0)

#### Notifications (Same as Partner Mobile)
- **@react-native-firebase/app** (v21.5.0)
- **@react-native-firebase/messaging** (v21.5.0)
- **@notifee/react-native** (v9.1.2)

#### Additional Features
- **react-native-share** (v11.0.4): Share functionality
- **react-native-rate** (v1.2.12): App rating prompts
- **react-native-splash-screen** (v3.3.0): Splash screen

---

## Shared Packages

### packages/ui

Shared UI components used across web applications.

```
packages/ui/
├── src/
│   ├── components/
│   │   ├── Button/
│   │   ├── Input/
│   │   ├── Card/
│   │   └── index.ts
│   └── index.ts
├── package.json
└── tsconfig.json

```

**Libraries:**
- React (v19.0.0)
- Tailwind CSS (v4.0.0)
- @radix-ui/react-*** (v1.1.2)
- class-variance-authority (v0.7.1)

### packages/utils

Shared utility functions and helpers.

```
packages/utils/
├── src/
│   ├── date.ts
│   ├── format.ts
│   ├── validation.ts
│   └── index.ts
├── package.json
└── tsconfig.json

```

**Libraries:**
- date-fns (v4.1.0)
- lodash-es (v4.17.21)

### packages/types

Shared TypeScript types and interfaces.

```
packages/types/
├── src/
│   ├── user.ts
│   ├── order.ts
│   ├── partner.ts
│   └── index.ts
├── package.json
└── tsconfig.json

```

---

## Development Tools

### Monorepo Management
- **Turborepo** (v2.3.3) or **Nx** (v20.2.2): Monorepo build system
- **pnpm** (v9.15.0) or **yarn** (v4.6.0): Package management

### Code Quality
- **ESLint** (v9.17.0): Linting
- **@typescript-eslint/parser** (v8.18.1): TypeScript parser
- **@typescript-eslint/eslint-plugin** (v8.18.1): TypeScript rules
- **Prettier** (v3.4.2): Code formatting
- **Husky** (v9.1.7): Git hooks
- **lint-staged** (v15.2.11): Pre-commit linting
- **@commitlint/cli** (v19.6.1): Commit message linting
- **@commitlint/config-conventional** (v19.6.0): Conventional commits

### Testing
- **Jest** (v29.7.0): Testing framework
- **@testing-library/react** (v16.1.0): React component testing
- **@testing-library/jest-dom** (v6.6.3): Custom matchers
- **Playwright** (v1.49.1) or **Cypress** (v13.17.0): E2E testing
- **Vitest** (v2.1.8): Alternative fast test runner

### CI/CD
- **GitHub Actions** or **GitLab CI**: Continuous integration
- **Docker** (v27.x): Containerization
- **Docker Compose** (v2.30.x): Multi-container orchestration
- **Kubernetes** (v1.31.x): Container orchestration

---

## Infrastructure

### Cloud Services (Azure/AWS)
- **Azure App Service** or **AWS ECS/Fargate**: Container hosting
- **Azure Database for PostgreSQL** or **AWS RDS PostgreSQL**: Database hosting
- **Azure Cache for Redis** or **AWS ElastiCache Redis**: Redis hosting
- **Azure Service Bus** or **AWS SQS/SNS**: Message queue
- **Azure Blob Storage** or **AWS S3**: File storage
- **Azure Application Insights** or **AWS CloudWatch/X-Ray**: Monitoring
- **Azure CDN** or **AWS CloudFront**: Content delivery

### DevOps
- **Terraform** (v1.10.x) or **Pulumi** (v3.x): Infrastructure as Code
- **Helm** (v3.16.x): Kubernetes package manager
- **ArgoCD** (v2.13.x): GitOps continuous delivery
- **Docker Compose** (v2.30.x): Local development
- **Kubernetes** (v1.31.x): Production orchestration

---

## Getting Started

### Prerequisites
- Node.js 22.x LTS or later
- .NET 10 SDK
- Docker Desktop (latest)
- pnpm 9.x or yarn 4.x

### Installation

```bash
# Clone the repository
git clone https://github.com/your-org/deliris.git
cd deliris

# Install dependencies
pnpm install

# Start development servers
pnpm dev

# Build all projects
pnpm build

# Run tests
pnpm test
```

### Environment Variables

Each project requires specific environment variables. Refer to `.env.example` files in each project directory.

---

## Documentation

For more detailed documentation on each project, refer to:
- [Backend Services Documentation](../backend/docs/README.md)
- [Admin Dashboard Documentation](../apps/admin-dashboard/README.md)
- [Partner Web App Documentation](../apps/partner-web/README.md)
- [Customer Web App Documentation](../apps/customer-web/README.md)
- [Partner Mobile App Documentation](../apps/partner-mobile/README.md)
- [Customer Mobile App Documentation](../apps/customer-mobile/README.md)
