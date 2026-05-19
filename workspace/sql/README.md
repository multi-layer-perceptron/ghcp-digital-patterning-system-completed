# SQL Schema

SQL artifacts store concept metadata, palette extraction results, manufacturing channel mappings, production grids, diagnostics, simulation runs, and simulation event history for the Digital Patterning System Simulator.

Apply migrations against a local SQL Server-compatible database before running report or repository validation.

## Validation

When SQL Server tooling is available, apply `migrations/V1__create_patterning_tables.sql` to a disposable database and verify the tables listed in the feature contract are created.
