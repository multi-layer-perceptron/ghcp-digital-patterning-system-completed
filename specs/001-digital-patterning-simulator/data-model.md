# Data Model: Digital Patterning System Simulator

## Entity: DesignConcept

Represents the active uploaded or sample design in the current user session.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `id` | string | Yes | Stable UUID generated when the concept is created |
| `source_name` | string | Yes | Display-safe file or sample name; no path disclosure |
| `source_type` | enum | Yes | `upload` or `sample` |
| `mime_type` | enum | Yes | `image/png` or `image/jpeg` |
| `file_size_bytes` | integer | Yes | `1..10_485_760` |
| `preview_data_url` | string | Yes | Sanitized PNG/JPEG data URL suitable for report embedding |
| `analysis_status` | enum | Yes | `empty`, `uploaded`, `analyzed`, `mapped`, `converted`, `running`, `paused`, `completed`, `blocked` |
| `created_at` | string | Yes | ISO 8601 timestamp |

### Relationships

- Has one `ImageMetadata` after upload analysis.
- Has one `ColorPalette` after palette extraction.
- Has 8 editable `ManufacturingChannel` records for the active session.
- Has one `ProductionGridModel` after conversion.
- Has many `ManufacturabilityDiagnostic` records.
- Has zero or one active `SimulationRun`.

### State Transitions

```text
empty -> uploaded -> analyzed -> mapped -> converted -> running -> paused -> running -> completed
converted -> blocked       when blocking diagnostics exist and start is requested
running -> converted       when reset clears run progress but preserves converted model
any active state -> empty   when restart clears concept state
```

## Entity: ImageMetadata

Non-sensitive characteristics derived from the uploaded or selected image.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `width_px` | integer | Yes | `1..4096` |
| `height_px` | integer | Yes | `1..4096` |
| `aspect_ratio` | number | Yes | Positive decimal rounded for display |
| `estimated_unique_colors` | integer | Yes | Non-negative estimate from sampled pixels |
| `has_transparency` | boolean | Yes | `true` only for PNGs with transparent pixels |
| `background_indicator` | string | No | Short confidentiality-safe text label |

## Entity: ColorPalette

Ranked representative colors extracted from the design.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `colors` | array of `PaletteColor` | Yes | 4 to 16 colors for standard samples unless the source has fewer distinct colors |
| `coverage_total_percent` | number | Yes | Within 2 percentage points of 100 for standard samples |
| `extraction_method` | string | Yes | `sampled-rgb-buckets` for initial implementation |

### PaletteColor

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `id` | string | Yes | Stable within concept |
| `hex` | string | Yes | `#RRGGBB` |
| `label` | string | Yes | Readable generated label such as `Color 1` |
| `coverage_percent` | number | Yes | `0..100` |
| `sample_count` | integer | Yes | Non-negative sampled-pixel count |

## Entity: ManufacturingChannel

One of 8 default generic production channels editable for the active session.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `id` | string | Yes | `channel-1` through `channel-8` by default |
| `label` | string | Yes | 1 to 40 display characters; confidentiality-safe |
| `hex` | string | Yes | `#RRGGBB` |
| `sort_order` | integer | Yes | `1..8`; unique per active concept |

## Entity: ChannelMapping

Relationship between palette colors and manufacturing channels.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `palette_color_id` | string | Yes | Must reference a palette color |
| `channel_id` | string | No | Required for exact/approximate mappings; absent for unresolved |
| `status` | enum | Yes | `exact`, `approximate`, or `unresolved` |
| `delta` | number | Yes | Non-negative visual difference estimate |
| `notes` | string | No | Display-safe explanation |

## Entity: ProductionGridModel

Simplified manufacturing-ready grid derived from mapped design colors.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `grid_size` | integer | Yes | `64`, `128`, or `256`; default `128` |
| `cells` | array | Yes | Length equals `grid_size * grid_size` |
| `channel_coverage` | object | Yes | Coverage percent by channel ID |
| `estimated_command_count` | integer | Yes | Non-negative command estimate from channel runs |
| `channel_switch_count` | integer | Yes | Non-negative complexity indicator |
| `fine_detail_score` | number | Yes | `0..1` |

### Cell

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `x` | integer | Yes | `0..grid_size-1` |
| `y` | integer | Yes | `0..grid_size-1` |
| `channel_id` | string | No | Absent when unresolved |
| `source_color_hex` | string | Yes | `#RRGGBB` |

## Entity: ManufacturabilityDiagnostic

A finding that describes a blocking error, warning, or informational observation affecting production feasibility.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `id` | string | Yes | Stable within latest analysis run |
| `severity` | enum | Yes | `error`, `warning`, or `info` |
| `category` | enum | Yes | `file`, `dimensions`, `palette`, `mapping`, `grid`, `complexity`, `confidentiality` |
| `message` | string | Yes | Clear, user-facing, confidentiality-safe text |
| `related_entity_id` | string | No | Palette color, channel, grid, or concept ID |
| `blocking` | boolean | Yes | `true` only when `severity = error` |

## Entity: SimulationRun

Lifecycle-controlled animated production run for the converted model.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `id` | string | Yes | Stable UUID generated on start |
| `status` | enum | Yes | `not_started`, `running`, `paused`, `completed`, `blocked`, `reset` |
| `progress_percent` | number | Yes | `0..100` |
| `current_pass` | integer | Yes | Non-negative line/pass index |
| `total_passes` | integer | Yes | Equals selected grid size |
| `active_channels` | array | Yes | Channel IDs active in current pass |
| `event_stream` | array of `SimulationEvent` | Yes | Most recent command/event messages |
| `started_at` | string | No | ISO 8601 timestamp |
| `completed_at` | string | No | ISO 8601 timestamp |

### SimulationEvent

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `sequence` | integer | Yes | Monotonic per run |
| `timestamp` | string | Yes | ISO 8601 timestamp |
| `event_type` | enum | Yes | `command`, `status`, `diagnostic`, `lifecycle` |
| `message` | string | Yes | Short event text suitable for dashboard display |
| `channel_id` | string | No | Present when channel-specific |

## Entity: ConceptReport

Portable review artifact exported as printable HTML and structured JSON.

| Field | Type | Required | Validation |
| --- | --- | --- | --- |
| `report_id` | string | Yes | UUID generated on export |
| `generated_at` | string | Yes | ISO 8601 timestamp |
| `concept` | `DesignConcept` summary | Yes | Excludes local file paths and secrets |
| `metadata` | `ImageMetadata` | Yes | Present after analysis |
| `palette` | `ColorPalette` | Yes | Present after extraction |
| `channels` | array of `ManufacturingChannel` | Yes | 8 current session channels |
| `mappings` | array of `ChannelMapping` | Yes | One per palette color |
| `grid_summary` | object | Yes | Dimensions, coverage, command estimates |
| `diagnostics` | array | Yes | Errors, warnings, and info findings |
| `simulation_summary` | object | No | Present when a run has started |
