# HCOM — Recruiter-Safe Packaging Checklist

This document outlines steps to make the HCOM repository **professional, clear, and safe for recruiters**.
It emphasizes **solo contribution, architecture, and UX-first design**, while clearly labeling incomplete or experimental features.

---

## Directory Structure

- Keep `src/` with `django-hcom` and `blazor-hcom` as-is (document the “stage of creation”).
- Keep `docs/` with:
  - `README.md`
  - `step-4-preserving-ux-invariants.md`
  - `step-5-resume-translation.md`
- Remove any temporary or test-only directories that don’t demonstrate architectural work.

---

## README / Documentation

- Keep the main `README.md` in the project root.
- Clearly state:
  - Project scope
  - Architectural narrative
  - Solo development context
  - Incomplete / experimental features (paging, search, notifications)
- Link the `docs/` folder for deeper architectural artifacts.

---

## Pin / Highlight

- Pin `blazor-hcom` branch if it represents the **most complete work**.
- Pin `README.md` and the `docs/` folder for architectural visibility.
- Include a **demo or screenshot** in README or `docs/` if possible.

---

## Label Experimental / Incomplete

- Clearly label in **README.md** and folder names:
  - Paging ability — experimental / TODO
  - Global search — under investigation
  - Notification / toast panel — stabilizing
  - Modular deployment — planned
- Use `TODO.md` or inline `TODO:` comments sparingly.

---

## Test Project

- Keep tests in a separate folder (`tests/` or `HCOM.Test/`).
- Clarify which tests **verify functionality** vs. which are **proof-of-concept / experimental**.

---

## Coding & Component Visibility

- Keep **Layout, Modal, Toast, DataDisplay, NavMenu, MainLayout, ToastHeader** components separate and documented.
- Highlight **C#-only modal & toast implementation** (no JS interop).
- Explicitly note **denial to use off-the-shelf Bootstrap components** where relevant.

---

## Version Control Hygiene

- Remove accidental commits to wrong branches (stash or move to the correct branch).
- Ensure the `main` branch builds and represents a coherent state.
- Use descriptive branch names like `blazor-hcom`, `django-hcom`, `feature-toast-panel`.

---

## Final Notes

- Everything in the repo should tell a **clear story**:
  - Solo development
  - Architecture evolution (Django → Blazor)
  - UX-first design for novice users
  - Thoughtful trade-offs and experimentation
- Avoid messy directories or random scripts — these distract recruiters.
