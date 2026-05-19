-- Digital Patterning Workshop optional PostgreSQL schema.
-- Runs once on first database volume creation.

CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE IF NOT EXISTS design_concepts (
    concept_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    source_name TEXT NOT NULL,
    source_type TEXT NOT NULL CHECK (source_type IN ('upload', 'sample')),
    mime_type TEXT NOT NULL CHECK (mime_type IN ('image/png', 'image/jpeg')),
    file_size_bytes BIGINT NOT NULL CHECK (file_size_bytes BETWEEN 1 AND 10485760),
    width_px INTEGER NULL,
    height_px INTEGER NULL,
    aspect_ratio NUMERIC(12, 6) NULL,
    estimated_unique_colors INTEGER NULL,
    has_transparency BOOLEAN NOT NULL DEFAULT false,
    analysis_status TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    CHECK ((width_px IS NULL OR width_px BETWEEN 1 AND 4096) AND (height_px IS NULL OR height_px BETWEEN 1 AND 4096))
);

CREATE TABLE IF NOT EXISTS palette_colors (
    palette_color_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    concept_id UUID NOT NULL REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    color_key TEXT NOT NULL,
    hex_color CHAR(7) NOT NULL,
    display_label TEXT NOT NULL,
    coverage_percent NUMERIC(6, 3) NOT NULL CHECK (coverage_percent BETWEEN 0 AND 100),
    sample_count INTEGER NOT NULL CHECK (sample_count >= 0),
    sort_order INTEGER NOT NULL CHECK (sort_order BETWEEN 1 AND 16)
);

CREATE TABLE IF NOT EXISTS manufacturing_channels (
    channel_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    concept_id UUID NOT NULL REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    channel_key TEXT NOT NULL,
    display_label TEXT NOT NULL,
    hex_color CHAR(7) NOT NULL,
    sort_order INTEGER NOT NULL CHECK (sort_order BETWEEN 1 AND 8)
);

CREATE TABLE IF NOT EXISTS channel_mappings (
    mapping_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    concept_id UUID NOT NULL REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    palette_color_id UUID NOT NULL REFERENCES palette_colors(palette_color_id),
    channel_id UUID NULL REFERENCES manufacturing_channels(channel_id),
    mapping_status TEXT NOT NULL CHECK (mapping_status IN ('exact', 'approximate', 'unresolved')),
    visual_delta NUMERIC(10, 4) NOT NULL CHECK (visual_delta >= 0),
    notes TEXT NULL
);

CREATE TABLE IF NOT EXISTS production_grids (
    grid_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    concept_id UUID NOT NULL REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    grid_size INTEGER NOT NULL CHECK (grid_size IN (64, 128, 256)),
    estimated_command_count INTEGER NOT NULL CHECK (estimated_command_count >= 0),
    channel_switch_count INTEGER NOT NULL CHECK (channel_switch_count >= 0),
    fine_detail_score NUMERIC(6, 5) NOT NULL CHECK (fine_detail_score BETWEEN 0 AND 1),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS production_grid_cells (
    grid_cell_id BIGSERIAL PRIMARY KEY,
    grid_id UUID NOT NULL REFERENCES production_grids(grid_id) ON DELETE CASCADE,
    x INTEGER NOT NULL,
    y INTEGER NOT NULL,
    channel_id UUID NULL REFERENCES manufacturing_channels(channel_id),
    source_color_hex CHAR(7) NOT NULL
);

CREATE TABLE IF NOT EXISTS manufacturability_diagnostics (
    diagnostic_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    concept_id UUID NOT NULL REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    severity TEXT NOT NULL CHECK (severity IN ('error', 'warning', 'info')),
    category TEXT NOT NULL CHECK (category IN ('file', 'dimensions', 'palette', 'mapping', 'grid', 'complexity', 'confidentiality')),
    message TEXT NOT NULL,
    related_entity_id TEXT NULL,
    blocking BOOLEAN NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS simulation_runs (
    run_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    concept_id UUID NOT NULL REFERENCES design_concepts(concept_id),
    grid_id UUID NOT NULL REFERENCES production_grids(grid_id),
    run_status TEXT NOT NULL CHECK (run_status IN ('not_started', 'running', 'paused', 'completed', 'blocked', 'reset')),
    progress_percent NUMERIC(6, 3) NOT NULL DEFAULT 0 CHECK (progress_percent BETWEEN 0 AND 100),
    current_pass INTEGER NOT NULL DEFAULT 0,
    total_passes INTEGER NOT NULL,
    started_at TIMESTAMPTZ NULL,
    completed_at TIMESTAMPTZ NULL
);

CREATE TABLE IF NOT EXISTS simulation_events (
    event_id BIGSERIAL PRIMARY KEY,
    run_id UUID NOT NULL REFERENCES simulation_runs(run_id) ON DELETE CASCADE,
    sequence_number INTEGER NOT NULL,
    event_type TEXT NOT NULL CHECK (event_type IN ('command', 'status', 'diagnostic', 'lifecycle')),
    event_message TEXT NOT NULL,
    channel_id UUID NULL REFERENCES manufacturing_channels(channel_id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE INDEX IF NOT EXISTS ix_palette_colors_concept ON palette_colors(concept_id);
CREATE INDEX IF NOT EXISTS ix_channels_concept ON manufacturing_channels(concept_id);
CREATE INDEX IF NOT EXISTS ix_mappings_concept ON channel_mappings(concept_id);
CREATE INDEX IF NOT EXISTS ix_diagnostics_concept_severity ON manufacturability_diagnostics(concept_id, severity);
CREATE INDEX IF NOT EXISTS ix_simulation_runs_concept ON simulation_runs(concept_id);
CREATE INDEX IF NOT EXISTS ix_simulation_events_run_sequence ON simulation_events(run_id, sequence_number);

