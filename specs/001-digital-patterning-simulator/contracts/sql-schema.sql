-- Contract: SQL run-history schema for Digital Patterning System Simulator
-- Target: SQL Server-compatible DDL for the C# orchestration/reporting layer.

CREATE TABLE design_concepts (
    concept_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    source_name NVARCHAR(255) NOT NULL,
    source_type NVARCHAR(20) NOT NULL,
    mime_type NVARCHAR(32) NOT NULL,
    file_size_bytes BIGINT NOT NULL,
    width_px INT NULL,
    height_px INT NULL,
    aspect_ratio DECIMAL(12, 6) NULL,
    estimated_unique_colors INT NULL,
    has_transparency BIT NOT NULL DEFAULT 0,
    analysis_status NVARCHAR(32) NOT NULL,
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT ck_design_concepts_source_type CHECK (source_type IN ('upload', 'sample')),
    CONSTRAINT ck_design_concepts_mime_type CHECK (mime_type IN ('image/png', 'image/jpeg')),
    CONSTRAINT ck_design_concepts_file_size CHECK (file_size_bytes BETWEEN 1 AND 10485760),
    CONSTRAINT ck_design_concepts_dimensions CHECK (
        (width_px IS NULL OR width_px BETWEEN 1 AND 4096) AND
        (height_px IS NULL OR height_px BETWEEN 1 AND 4096)
    )
);

CREATE TABLE palette_colors (
    palette_color_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    concept_id UNIQUEIDENTIFIER NOT NULL,
    color_key NVARCHAR(40) NOT NULL,
    hex_color CHAR(7) NOT NULL,
    display_label NVARCHAR(80) NOT NULL,
    coverage_percent DECIMAL(6, 3) NOT NULL,
    sample_count INT NOT NULL,
    sort_order INT NOT NULL,
    CONSTRAINT fk_palette_colors_concept FOREIGN KEY (concept_id) REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    CONSTRAINT ck_palette_colors_coverage CHECK (coverage_percent BETWEEN 0 AND 100),
    CONSTRAINT ck_palette_colors_sample_count CHECK (sample_count >= 0),
    CONSTRAINT ck_palette_colors_sort_order CHECK (sort_order BETWEEN 1 AND 16)
);

CREATE TABLE manufacturing_channels (
    channel_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    concept_id UNIQUEIDENTIFIER NOT NULL,
    channel_key NVARCHAR(40) NOT NULL,
    display_label NVARCHAR(40) NOT NULL,
    hex_color CHAR(7) NOT NULL,
    sort_order INT NOT NULL,
    CONSTRAINT fk_manufacturing_channels_concept FOREIGN KEY (concept_id) REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    CONSTRAINT ck_manufacturing_channels_sort_order CHECK (sort_order BETWEEN 1 AND 8)
);

CREATE TABLE channel_mappings (
    mapping_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    concept_id UNIQUEIDENTIFIER NOT NULL,
    palette_color_id UNIQUEIDENTIFIER NOT NULL,
    channel_id UNIQUEIDENTIFIER NULL,
    mapping_status NVARCHAR(20) NOT NULL,
    visual_delta DECIMAL(10, 4) NOT NULL,
    notes NVARCHAR(255) NULL,
    CONSTRAINT fk_channel_mappings_concept FOREIGN KEY (concept_id) REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    CONSTRAINT fk_channel_mappings_palette FOREIGN KEY (palette_color_id) REFERENCES palette_colors(palette_color_id),
    CONSTRAINT fk_channel_mappings_channel FOREIGN KEY (channel_id) REFERENCES manufacturing_channels(channel_id),
    CONSTRAINT ck_channel_mappings_status CHECK (mapping_status IN ('exact', 'approximate', 'unresolved')),
    CONSTRAINT ck_channel_mappings_delta CHECK (visual_delta >= 0)
);

CREATE TABLE production_grids (
    grid_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    concept_id UNIQUEIDENTIFIER NOT NULL,
    grid_size INT NOT NULL,
    estimated_command_count INT NOT NULL,
    channel_switch_count INT NOT NULL,
    fine_detail_score DECIMAL(6, 5) NOT NULL,
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT fk_production_grids_concept FOREIGN KEY (concept_id) REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    CONSTRAINT ck_production_grids_size CHECK (grid_size IN (64, 128, 256)),
    CONSTRAINT ck_production_grids_command_count CHECK (estimated_command_count >= 0),
    CONSTRAINT ck_production_grids_switch_count CHECK (channel_switch_count >= 0),
    CONSTRAINT ck_production_grids_detail_score CHECK (fine_detail_score BETWEEN 0 AND 1)
);

CREATE TABLE production_grid_cells (
    grid_cell_id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    grid_id UNIQUEIDENTIFIER NOT NULL,
    x INT NOT NULL,
    y INT NOT NULL,
    channel_id UNIQUEIDENTIFIER NULL,
    source_color_hex CHAR(7) NOT NULL,
    CONSTRAINT fk_grid_cells_grid FOREIGN KEY (grid_id) REFERENCES production_grids(grid_id) ON DELETE CASCADE,
    CONSTRAINT fk_grid_cells_channel FOREIGN KEY (channel_id) REFERENCES manufacturing_channels(channel_id)
);

CREATE TABLE manufacturability_diagnostics (
    diagnostic_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    concept_id UNIQUEIDENTIFIER NOT NULL,
    severity NVARCHAR(20) NOT NULL,
    category NVARCHAR(40) NOT NULL,
    message NVARCHAR(500) NOT NULL,
    related_entity_id NVARCHAR(80) NULL,
    blocking BIT NOT NULL,
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT fk_diagnostics_concept FOREIGN KEY (concept_id) REFERENCES design_concepts(concept_id) ON DELETE CASCADE,
    CONSTRAINT ck_diagnostics_severity CHECK (severity IN ('error', 'warning', 'info')),
    CONSTRAINT ck_diagnostics_category CHECK (category IN ('file', 'dimensions', 'palette', 'mapping', 'grid', 'complexity', 'confidentiality'))
);

CREATE TABLE simulation_runs (
    run_id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    concept_id UNIQUEIDENTIFIER NOT NULL,
    grid_id UNIQUEIDENTIFIER NOT NULL,
    run_status NVARCHAR(20) NOT NULL,
    progress_percent DECIMAL(6, 3) NOT NULL DEFAULT 0,
    current_pass INT NOT NULL DEFAULT 0,
    total_passes INT NOT NULL,
    started_at DATETIMEOFFSET NULL,
    completed_at DATETIMEOFFSET NULL,
    CONSTRAINT fk_simulation_runs_concept FOREIGN KEY (concept_id) REFERENCES design_concepts(concept_id),
    CONSTRAINT fk_simulation_runs_grid FOREIGN KEY (grid_id) REFERENCES production_grids(grid_id),
    CONSTRAINT ck_simulation_runs_status CHECK (run_status IN ('not_started', 'running', 'paused', 'completed', 'blocked', 'reset')),
    CONSTRAINT ck_simulation_runs_progress CHECK (progress_percent BETWEEN 0 AND 100)
);

CREATE TABLE simulation_events (
    event_id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    run_id UNIQUEIDENTIFIER NOT NULL,
    sequence_number INT NOT NULL,
    event_type NVARCHAR(20) NOT NULL,
    event_message NVARCHAR(500) NOT NULL,
    channel_id UNIQUEIDENTIFIER NULL,
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT fk_simulation_events_run FOREIGN KEY (run_id) REFERENCES simulation_runs(run_id) ON DELETE CASCADE,
    CONSTRAINT fk_simulation_events_channel FOREIGN KEY (channel_id) REFERENCES manufacturing_channels(channel_id),
    CONSTRAINT ck_simulation_events_type CHECK (event_type IN ('command', 'status', 'diagnostic', 'lifecycle'))
);

CREATE INDEX ix_palette_colors_concept ON palette_colors(concept_id);
CREATE INDEX ix_channels_concept ON manufacturing_channels(concept_id);
CREATE INDEX ix_mappings_concept ON channel_mappings(concept_id);
CREATE INDEX ix_diagnostics_concept_severity ON manufacturability_diagnostics(concept_id, severity);
CREATE INDEX ix_simulation_runs_concept ON simulation_runs(concept_id);
CREATE INDEX ix_simulation_events_run_sequence ON simulation_events(run_id, sequence_number);
