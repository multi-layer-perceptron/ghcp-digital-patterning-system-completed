<!--
Sync Impact Report
Version change: 1.0.0 -> 1.0.1
Modified principles: None; existing principles retained with governance metadata added
Added sections: Sync Impact Report
Removed sections: None
Templates requiring updates:
- ✅ .specify/templates/plan-template.md
- ✅ .specify/templates/spec-template.md
- ✅ .specify/templates/tasks-template.md
- ✅ .specify/templates/commands/*.md (no command templates present)
Follow-up TODOs: None
-->

# Digital Patterning Workshop Constitution

## Core Principles

### I. Confidentiality-Safe Workshop Artifacts

All specs, generated text, user-facing UI, reports, sample data, and documentation MUST avoid proprietary company names, site names, customer-identifiable references, and production-sensitive details. Sample assets MUST be generic, synthetic, or explicitly approved for workshop use.

### II. Industrial Stack Alignment

Implementation tasks MUST stay aligned with the selected C#, C++, C, SQL, TCP/IP, Windows, Linux, FPGA, and PLC stack. Any tooling-only exception MUST be documented in the active plan before tasks depend on it.

### III. Testable Independent Stories

Each user story MUST remain independently testable with explicit validation tasks before dependent workflows are layered on top. Tests SHOULD be created before implementation tasks for the same behavior and MUST cover validation, protocol, persistence, and report boundaries when those boundaries are touched.

### IV. Simulated Control Boundaries

The simulator MUST NOT directly control production equipment. TCP/IP, PLC, and FPGA components MUST remain proof-of-concept stubs, emulators, or validation harnesses unless a separate approved requirement explicitly changes the control boundary.

### V. Explicit Quality Gates

Plans and tasks MUST include validation for build, tests, schema, protocol compatibility, timing goals, and confidentiality scans. Implementation is not complete until these gates are documented and can be executed by a workshop participant or maintainer.

## Technology Constraints

- Application code belongs under `workspace/` unless the change is documentation, specification, prompt, or repository configuration.
- The selected feature stack is C#, C++, C, SQL, TCP/IP, Windows, Linux, FPGA, and PLCs.
- Persistence is limited to run history, diagnostics, report-source data, and simulator metadata needed for the proof-of-concept.
- External production services, authentication, queues, and live equipment integrations are out of scope unless explicitly approved in a new requirement.

## Development Workflow

- Keep specs, plans, and tasks aligned before implementation begins.
- Preserve independently testable user-story phases.
- Keep examples and validation data confidentiality-safe by default.
- Prefer simple, explicit workshop-friendly implementations over opaque abstractions.
- Re-run consistency analysis after substantial changes to spec, plan, contracts, or tasks.

## Governance

This constitution supersedes feature plans, task lists, and implementation preferences for this repository. Amendments require an explicit constitution update with rationale, date, and expected downstream artifacts to review. Any conflict between this constitution and a feature artifact MUST be resolved by changing the feature artifact or by performing a separate approved constitution amendment.

**Version**: 1.0.1 | **Ratified**: 2026-05-19 | **Last Amended**: 2026-05-19
