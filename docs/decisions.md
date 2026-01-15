# HCOM Architectural & Design Decisions

This document explains the key design choices made during the development of HCOM.

---

## 1. Layout vs. Pages

- **Layout-hosted components**:
  - Modal CRUD
  - NotificationPanel/Toast
  - DataDisplay
  - Toolbar
  - NavMenu
  - ToastHeader
- **Pages**:
  - Contain **dumb forms only** (`ModelCreateUpdateForm`, `ModelReadDeleteForm`)
  - No direct handling of CRUD logic; delegated entirely to the Modal
- **Rationale**: Ensures **consistent UI, SPA-native behavior, and reusability** across all modules.

---

## 2. Modal CRUD Implementation

- Entirely **C#**, no JavaScript interop.
- Deliberate avoidance of **pre-built Bootstrap components**.
- Each page can operate with **create/update and read/delete forms** that are stateless.
- **Benefits**: Simplifies future modularity, ensures centralized validation, easier testing.

---

## 3. Data Presentation

- Encapsulated in **DataDisplay component**
- Supports **dual presentation**:
  - Card view
  - Table view
- Component is **expandable** for future layout requirements.
- Filtering is built-in; central/global search is **planned**.

---

## 4. Toolbar

- Provides filtering, paging (planned), data view switching, and new record addition.
- Hosted in Layout to ensure **consistent placement and interaction patterns**.
- Designed to work for both **single module and combined module scenarios**.

---

## 5. Notification System

- Central **NotificationPanel** with integrated **toast messages**
- Provides success/failure feedback from modal CRUD operations.
- Hosted in Layout for **universal availability**.
- Future work: refine SPA-specific menu interactions.

---

## 6. Testing & Quality

- Core components designed to be **unit-testable and integration-testable**.
- Foundation for **test project** ensures maintainability and correctness.

---

## 7. Modularity (Planned)

- Each module (Clinic, Lab, Radiology, etc.) can run independently or in combination.
- Shared invariants:
  - Patient data
  - Worker/HR data
  - Financial data
- Architectural choices (Layout-hosted primitives, dumb forms, expandable DataDisplay) are **foundation-ready** for modular composition.
