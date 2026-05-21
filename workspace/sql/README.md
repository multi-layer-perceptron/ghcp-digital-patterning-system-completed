# SQL Schema

SQL artifacts store concept metadata, palette extraction results, manufacturing channel mappings, production grids, diagnostics, simulation runs, and simulation event history for the Digital Patterning System Simulator.

The feature contract remains SQL Server-compatible, while local validation uses SQLite inside a Docker container for portability.

## Validation

Run `bash validate-sqlite-container.sh` from the repository root to validate the portable SQLite schema in a disposable container.
