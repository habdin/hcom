# HCOM — Healthcare Organization Management Platform

**Author:** Hassan Abdin
**Description:** A modular, composable platform to manage healthcare organizations of any size. Focused on staged development, reusable components, and SPA-native architecture in Blazor.

---

## Overview

HCOM is an ambitious platform designed to manage healthcare operations ranging from solo clinics to mini-hospitals or full polyclinics. The system is **modular by design (planned)**, allowing each unit—clinic, laboratory, radiology department, pharmacy, etc.—to run **independently** or in **composition with others**, while sharing foundational invariants.

**Core principles:**

- **Composable modules (planned):** Each department will eventually run alone or combined with others.
- **Shared invariants:** Patient, worker, and financial areas are consistent across modules.
- **Foundation-first approach:** Interaction primitives (modal CRUD, toast notifications, shared models) are stabilized before full modular integration.
- **Bottom-up architecture:** Modular design supports incremental expansion from a basic CRUD foundation to full EMR, HR, and financial functionality.
- **Solo developer project:** All design, architecture, and implementation performed by a single developer, demonstrating end-to-end reasoning.

---

## Foundation Layer (Implemented / Under Development)

The foundation layer demonstrates the **core architectural primitives** that support HCOM’s modularity:

1. **Repo Structure (Evolution Stages):**
   - The `src` directory contains both **django-hcom** and **blazor-hcom**, representing **evolutionary stages** of HCOM:
     - `django-hcom`: Legacy Django-based implementation with class-based views and JS/jQuery modal CRUD. Located at: [Django HCOM](https://github.com/habdin/hcom/tree/django-hcom)
     - `blazor-hcom`: Modern Blazor SPA implementation with centralized Layout-hosted components, fully C# modal CRUD, NotificationPanel/Toast, Toolbar, and DataDisplay. Located at: [Blazor HCOM](https://github.com/habdin/hcom/tree/blazor-hcom)
   - This stage-based structure highlights the **program’s progression from classic CRUD to SPA-native, modular-ready architecture**.

2. **Modal CRUD Interaction & Shared UI Components:**
   - All CRUD operations are **modal-based** and implemented entirely in **C#**, with **no JavaScript interop**.
   - **Pre-made Bootstrap modals or components are deliberately not used**; all UI primitives are custom-built.
   - The **Modal, Toast/NotificationPanel, DataDisplay, NavMenu, ToastHeader, and Toolbar** are **hosted in the Layout**, ensuring consistent shared UI across all Pages.
   - Each data-driven Page contains **two dumb forms**:
     - `ModelCreateUpdateForm` for create/update input
     - `ModelReadDeleteForm` for read/delete display
   - Forms are **stateless and purely presentational**; the Modal handles all **submit and CRUD logic**.
   - Pages host **module-specific content only**, keeping interaction logic centralized and reusable.

3. **Data Presentation Layer:**
   - Data display is encapsulated in a **DataDisplay component**, supporting **dynamic card/table interaction**.
   - Component is **expandable** for future layouts or features.
   - Separation ensures **flexibility and maintainability**, even if currently spread across placeholders.

4. **Toolbar / Index / Notifications:**
   - Toolbar provides:
     - **Filtering** (current state)
     - **Paging** (planned)
     - **Data view switching** between card and table layouts
     - **New record addition** via modal CRUD
   - Currently implemented in **Layout placeholders**, to be unified into a **single SPA-like workflow** per module.

5. **Search & Filtering:**
   - Current state is **filtering only**.
   - Central/global search is **under investigation**, with Blazor implementation separating search logic to allow **per-component or future global search**.

6. **Paging Ability:**
   - Not yet implemented; to be added for large datasets as needed.

7. **Modular Areas (Planned):**
   - True modularity is **not yet implemented**, but all foundational elements are designed to support **pluggable modules** in the future.
   - Each module will eventually include patient, worker (HR), and financial sub-areas, leveraging shared identity models and reusable components.

8. **Blazor Layout & Component Architecture:**
   - Components are separated into **Layout (shared UI logic) and Pages**.
   - The Layout hosts the **core interaction primitives**: Modal CRUD, Toolbar, NotificationPanel/Toast, DataDisplay, NavMenu, ToastHeader.
   - This design ensures **consistent behavior, SPA-native integration, and maintainable shared components**.
   - **No pre-made Bootstrap components are used**, keeping the SPA fully under custom control.

9. **Test Project:**
   - HCOM includes a **dedicated test project** for validating core functionality.
   - Tests cover **Modal CRUD interactions, DataDisplay behavior, and Layout-hosted components**.
   - Ensures **basic correctness, consistency, and maintainability** as new modules and features are added.
   - Testing will **expand alongside modularity and domain-specific functionality**.

10. **SPA Considerations:**
    - Some UI elements (e.g., Bootstrap menus) are being refined for SPA compliance, but full SPA integration is **future work**.

**Note:** All development has been performed by a **solo developer**, highlighting full-stack, architectural, and design-level reasoning.

---

## Domain Layer (Intended Scope)

HCOM is designed to eventually support **full healthcare operations**, including:

- **Electronic Medical Records (EMR)**
- **Employee Management**
- **Patient Management**
- **Clinic Management**
- **Laboratory Management**
- **Radiology Management**
- **Pharmacy Management**
- **Cafeteria & Hospital Services**
- **Financial Management** (billing, insurance, payroll, expenses)

> **Note:** Modules are planned and partially implemented. The **foundation layer** ensures that domain modules added later will rely on a stable, consistent infrastructure.

---

## Architecture & Design Principles

- **Bottom-Up Development:** Stabilize CRUD and relational foundations before adding domain complexity.
- **Reusability Across Modules:** All modules share interaction patterns and core models.
- **Pluggable Composition (Planned):** Modules will be dynamically composable once modularity is implemented.
- **Separation of Concerns:** Interaction primitives, data display, toolbar, modal, and notifications are centralized in the Layout; Pages remain module-specific and dumb.
- **Scalability & Flexibility:** DataDisplay component, filtering, paging, and toolbar designed to scale with module complexity.
- **Solo Developer Context:** Entire system is built by one developer, demonstrating end-to-end design and architectural reasoning.

---

## Planned Next Steps

- Stabilize modal CRUD across all modules.
- Consolidate toolbar, index, forms, and notifications into a unified SPA-like workflow.
- Complete the notification system (flash/toast) for consistent feedback.
- Implement paging to handle large datasets.
- Correct SPA-specific UI behaviors (e.g., Bootstrap menus).
- Explore central/global search while maintaining module-level filtering.
- Expand DataDisplay component for additional presentation layouts.
- Implement true modularity for pluggable modules (clinic, lab, radiology, etc.).
- Continue adding domain-specific modules (EMR, HR, financial, etc.) while leveraging the pluggable architecture.
- Develop a code generator for repetitive scaffolding once CRUD foundation is fully stabilized.

---

## How to Read the Repo

1. **`src/django-hcom`**: Legacy Django-based implementation.
2. **`src/blazor-hcom`**: Modern Blazor SPA implementation.
3. **Layout:** Hosts shared UI components: Modal CRUD, Toolbar, NotificationPanel/Toast, DataDisplay, NavMenu, ToastHeader.
4. **Pages:** Module-specific content only.
5. **DataDisplay component:** Handles dynamic card/table data presentation, expandable for future features.
6. **Toolbar / Index / Notifications:** Implemented as placeholders in Layout; to be unified for SPA workflow.
7. **`tests/` or `HCOM.Tests`**: Unit and integration tests for core components, Modal CRUD, DataDisplay, Toolbar, and Notifications.
8. **`docs/`**: Architecture notes, design decisions, and planned enhancements.
9. **`TODOs`**: Experimental or incomplete components; intentional and clearly marked.

---

## Summary

HCOM is **not just a CRUD demo** — it is a **foundation-first, composable healthcare management platform in staged development**.

By focusing on **Layout-hosted shared components, dumb page forms, reusable interaction primitives, planned modularity, and solo developer design**, HCOM demonstrates a professional architectural approach and prepares the system to scale from a single department to complex multi-unit healthcare organizations.
