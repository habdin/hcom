# HCOM Testing Strategy

This document outlines the current and planned testing strategy for HCOM.

---

## 1. Test Project

- Located in `tests/` or `HCOM.Tests`
- **Scope**:
  - Modal CRUD functionality
  - DataDisplay component behavior (card/table, filtering)
  - Toolbar actions
  - NotificationPanel/Toast integration

---

## 2. Approach

- **Unit Tests**: Validate individual components, forms, and Layout primitives.
- **Integration Tests**: Ensure components interact correctly in the SPA workflow (e.g., modal CRUD updates DataDisplay and triggers notifications).
- **Future Expansion**:
  - Test modular composition
  - Test paging once implemented
  - Validate central/global search when added

---

## 3. Best Practices

- Forms remain **dumb** to simplify testing.
- Modal handles all **state changes and submissions** to reduce complexity in unit tests.
- Tests written in a way that **simulates real SPA interactions**, without relying on JavaScript interop.

---

## 4. Test Goals

- Ensure **data consistency** across all components.
- Verify **notification accuracy**.
- Maintain **high confidence** as new modules and features are added.
