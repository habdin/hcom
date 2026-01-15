# HCOM Planned Modularity

This document explains the planned modular architecture for HCOM.

---

## 1. Module Definition

- A module represents a **functional unit** (Clinic, Lab, Radiology, Pharmacy, etc.)
- Each module contains:
  - Patient-related area
  - Worker (HR) area
  - Financial area (billing, payroll, insurance)

---

## 2. Module Deployment

- **Solo Mode**: A module can run independently as a single app.
- **Composed Mode**: Multiple modules can be combined to simulate:
  - Mini hospital
  - Polyclinic with multiple departments

---

## 3. Shared Invariants

- Patient identity, HR records, and financial data are **shared across modules**.
- This ensures **data consistency** and avoids duplication.

---

## 4. Component Reuse

- Layout-hosted components (Modal, Toolbar, NotificationPanel, DataDisplay) are **ready for any module**.
- Dumb forms per Page enable **module-specific customization** without changing shared primitives.

---

## 5. Future Enhancements

- Code generator for module scaffolding once base CRUD is stable.
- Central/global search integration per module and globally.
- Paging and advanced DataDisplay layouts.
- Automated modular deployment testing.
