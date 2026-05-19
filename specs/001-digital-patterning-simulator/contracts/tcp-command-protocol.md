# Contract: TCP/IP Patterning Command Protocol

## Purpose

Defines the local TCP/IP protocol between the C# orchestration/dashboard layer, C++ pattern-processing service, C control emulator, PLC stub, and FPGA signal-timing stub.

## Transport

- Protocol: TCP/IP loopback for proof-of-concept, routable TCP/IP for lab extensions.
- Encoding: UTF-8 JSON Lines (`\n` delimited) for readability and testability.
- Default ports:
  - `5100`: C++ pattern processing service
  - `5110`: C control emulator
  - `5120`: PLC stub gateway
  - `5130`: FPGA timing stub gateway
- Correlation: every request includes `message_id`; responses include the same `message_id`.
- Timeouts: command requests should fail gracefully after 2 seconds without response in the proof-of-concept.

## Common Envelope

```json
{
  "message_id": "uuid",
  "message_type": "string",
  "schema_version": "0.1",
  "sent_at": "2026-05-19T00:00:00Z",
  "payload": {}
}
```

## Message Types

| Message Type | Direction | Purpose |
| --- | --- | --- |
| `concept.analyze` | C# -> C++ | Normalize source image metadata and derive palette/grid inputs |
| `concept.analyzed` | C++ -> C# | Return metadata, palette, and analysis diagnostics |
| `grid.convert` | C# -> C++ | Convert mapped palette to a selected production grid |
| `grid.converted` | C++ -> C# | Return grid summary, command estimates, and diagnostics |
| `run.start` | C# -> C emulator / PLC gateway | Start pass-by-pass production simulation |
| `run.pause` | C# -> C emulator / PLC gateway | Pause the active run |
| `run.resume` | C# -> C emulator / PLC gateway | Resume a paused run |
| `run.reset` | C# -> C emulator / PLC gateway | Clear run progress while preserving concept state |
| `channel.activate` | C++/C emulator -> PLC/FPGA gateway | Activate one channel for one grid pass segment |
| `status.update` | C emulator / PLC gateway -> C# | Return machine status, active channels, progress, and warnings |
| `diagnostic.raised` | Any -> C# | Report blocking error, warning, or info finding |
| `run.completed` | C emulator / PLC gateway -> C# | Report completion metrics and final status |

## `concept.analyze` Payload

```json
{
  "concept_id": "uuid",
  "source_name": "sample-pattern.png",
  "mime_type": "image/png",
  "file_size_bytes": 1048576,
  "image_bytes_base64": "...",
  "max_width_px": 4096,
  "max_height_px": 4096,
  "max_palette_colors": 16
}
```

### Validation

- `mime_type` must be `image/png` or `image/jpeg`.
- `file_size_bytes` must be between `1` and `10485760`.
- Decoded dimensions must not exceed `4096 x 4096`.
- Source names must be display-safe and must not include local file paths.

## `grid.convert` Payload

```json
{
  "concept_id": "uuid",
  "grid_size": 128,
  "palette": [
    { "id": "palette-1", "hex": "#336699", "coverage_percent": 24.5 }
  ],
  "channels": [
    { "id": "channel-1", "label": "Channel 1", "hex": "#336699", "sort_order": 1 }
  ],
  "mappings": [
    { "palette_color_id": "palette-1", "channel_id": "channel-1", "status": "exact", "delta": 0 }
  ]
}
```

### Validation

- `grid_size` must be `64`, `128`, or `256`.
- Exactly 8 channels must be present.
- Each palette color must have one mapping.
- Any unresolved mapping should create an `error` diagnostic and prevent `run.start`.

## `channel.activate` Payload

```json
{
  "run_id": "uuid",
  "pass_index": 12,
  "segment_index": 4,
  "channel_id": "channel-3",
  "pulse_width_us": 128,
  "duration_ms": 25,
  "x_start": 64,
  "x_end": 96
}
```

### Validation

- `pass_index` must be between `0` and `grid_size - 1`.
- `pulse_width_us` must be between `0` and `255`.
- `x_start` must be less than or equal to `x_end`.
- `channel_id` must reference one of the 8 active channels.

## `status.update` Payload

```json
{
  "run_id": "uuid",
  "status": "running",
  "progress_percent": 42.5,
  "current_pass": 54,
  "total_passes": 128,
  "active_channels": ["channel-1", "channel-4"],
  "machine_state": "processing",
  "event_message": "PASS 54 CHANNEL channel-4 SEGMENT 3"
}
```

## Error Response

```json
{
  "message_id": "uuid",
  "message_type": "error",
  "schema_version": "0.1",
  "sent_at": "2026-05-19T00:00:00Z",
  "payload": {
    "code": "INVALID_GRID_SIZE",
    "severity": "error",
    "message": "Grid size must be 64, 128, or 256.",
    "blocking": true
  }
}
```

## Security And Confidentiality

- Do not transmit local absolute file paths.
- Do not include customer, site, restricted, or identifying names in generated payloads.
- Treat all image bytes as local-only proof-of-concept data.
- Reject control characters in labels and source names.
