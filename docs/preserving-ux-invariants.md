# Preserving UX Behavior During an Architecture Shift
### Notes from the HCOM Project

## What this document is about

HCOM is a long-term **solo** project aimed at managing healthcare organizations.
It started as a **Django-based application** and later moved toward a **Blazor-based SPA architecture**.

This document explains **why the architectural shift happened**, focusing on one central concern:

> Preserving a simple, predictable user experience for novice computer users became increasingly difficult under the original architecture.

This is **not** a framework comparison and **not** a performance discussion.
It is about **control**, **state**, **developer fatigue**, and **where complexity should live**.

---

## The real target user (the hidden constant)

From the beginning, HCOM was designed primarily for:

- Non-technical staff
- First-time or novice computer users
- Healthcare workers whose focus is *work*, not software

This assumption shaped every UX decision:

- Fewer pages
- Fewer navigation steps
- Clear visual continuity
- Minimal context switching
- Predictable interaction patterns

The architecture was always judged by **how well it protected the user from complexity**.

---

## The original UX goal (unchanged across frameworks)

To support novice users, HCOM intentionally avoided the classic CRUD pattern:

- List page
- Create page
- Edit page
- Delete page
- Details page

Instead, the system aimed for:

- One index page per domain
- One reusable modal for Create / Update / Delete
- Clear success and error notifications
- No unnecessary page reloads
- Preserved user context at all times

This UX goal remained the same in **both Django and Blazor**.

What changed was **how hard it was to maintain this simplicity**.

---

## What became difficult in Django

Django is fundamentally built around the **request / response** model.
This is not a flaw, but it strongly influences how interaction works.

### Heavy backend influence

In the Django phase of HCOM:

- The backend dictated interaction boundaries
- JSON was heavily used to support modal-based workflows
- JavaScript had to be carefully tailored to comply with backend requirements

Over time:

- Similar UI actions required different request formats
- JSON handling became non-uniform
- Frontend code increasingly existed to *satisfy backend rules*

JavaScript stopped being an interaction layer and became a **compliance layer**.

---

## Why this caused fatigue

Both Django and Blazor render HTML on the server.
The difference was **how often the frontend had to negotiate with the backend**.

In Django:

- Every interaction restarted the full request/response cycle
- State was lost after each request
- Objects had to be serialized to JSON and reconstructed
- UI continuity depended on careful manual coordination

This resulted in:

- More traffic
- More glue code
- Higher cognitive load

The system functioned, but the effort grew steadily.

The fatigue came from **constantly adapting the frontend to backend mechanics**, not from lack of experience.

---

## UI construction vs. data flow (search as a key example)

One especially exhausting area was **search and filtering**.

### In Django

- The backend returned data, often as JSON
- JavaScript was responsible for:
  - building search results
  - updating lists
  - reshaping parts of the UI
- In practice, JavaScript had to **create the interface itself**

The interface was not fully present — it had to be rebuilt repeatedly.

This led to:

- Search logic mixed with DOM manipulation
- UI structure leaking into JavaScript
- Small UI changes requiring work across multiple layers

This was particularly harmful when designing for novice users, who rely on **visual consistency**.

### In Blazor

The model was reversed:

- The UI exists as components from the start
- Search and filtering affect **data**, not markup
- When data changes, the UI updates automatically

Instead of:

> “JavaScript builds the interface based on data”

The model became:

> “The interface exists; data flows through it”

This preserved visual stability — a key requirement for novice users.

---

## A simple mental model (the factory analogy)

A helpful way to understand the problem:

> When designing an iPhone, you define the design at a high level
> and let the factory execute it.
> You don’t ask the factory to keep deciding how the phone should look.

In the Django architecture:

- JavaScript acted like the factory
- But it was forced to make design decisions
- Instead of executing a clear interaction plan

Control lived **too low in the system**, where mistakes are costly and repetitive.

---

## State lifetime as the breaking point

Another key difference was **how long state lived**.

### Django

- Each request ended the lifetime of:
  - objects
  - UI assumptions
  - interaction context
- Everything had to be serialized, sent, and rebuilt

This increased the chance of UI inconsistency — a serious problem for novice users.

### Blazor

- State lives inside components and layouts
- Interaction context persists naturally
- Backend calls are side effects, not control boundaries

This made UI behavior predictable and stable.

---

## JSON usage and communication cost

### Django phase

- JSON interactions were frequent
- Many small requests crossed the backend boundary
- Transport cost per request was low
- Interaction count was high

The real cost was **coordination**, not bandwidth.

### Blazor phase

- JSON usage was intentionally minimal
- A persistent connection (WebSocket) was used
- Fewer interactions carried more meaning
- State changes were sent instead of UI reconstruction instructions

Maintaining a WebSocket has a cost —
but the **number of interactions was far lower**.

---

## Why the shift made sense

The move to Blazor was driven by combined reasons:

- Less repeated JSON handling
- Persistent state
- Clear ownership of UI behavior
- Layout-level control of modals and notifications
- Lower long-term cognitive load

Most importantly:

> It allowed the system to stay simple for novice users
> without becoming exhausting for the developer.

---

## Honest project status

HCOM is intentionally presented as incomplete:

- Paging is planned but not implemented yet
- Global search is still under investigation
- Notification and toast systems exist but are stabilizing
- Full modular deployment is designed but not yet realized

These limits are documented and deliberate.

---

## Why this document exists

HCOM is not presented as a finished product.

It is:

- An architectural exploration
- A study in protecting novice users from complexity
- A real example of how control placement affects both UX and developer effort

These lessons apply beyond Django or Blazor.

---

## Final thought

The architectural shift was not about performance or trends.

It was about this:

> Reducing how often the frontend has to think about backend mechanics
> so that the user never has to think about the system at all.

Once that happened, complexity stopped growing sideways —
even though many features remain unfinished.

That alone justified the migration.

---

## STEP 4 — Status

✅ **Finalized flagship architectural document**
